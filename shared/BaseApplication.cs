using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace shared
{
    /// <summary>A basis for the server and client containing the shared functionality.</summary>
    public class BaseApplication {
        /// <summary>A queue containing the received messages.</summary>
        public ConcurrentQueue<Message> IncomingQueue { get; }

        /// <summary>A queue containing the messages going to be sent.</summary>
        public ConcurrentQueue<Message> OutgoingQueue { get; }

        /// <summary>A thread for capturing incoming messages from the UDP socket.</summary>
        private readonly Thread incomingThread;

        /// <summary>A thread for sending outgoing messages through the UDP socket.</summary>
        private readonly Thread outgoingThread;

        /// <summary>The UDP client used to perform UDP operations.</summary>
        private UdpClient udpClient;

        /// <summary>Build and initialize a new base application structure.</summary>
        public BaseApplication() {
            IncomingQueue = new ConcurrentQueue<Message>();
            OutgoingQueue = new ConcurrentQueue<Message>();

            // build a thread for capturing incoming messages.
            incomingThread = new Thread(() => {
                try {
                    while (true) {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] received = udpClient.Receive(ref ipEndPoint);
                        string json = Encoding.UTF8.GetString(received);
                        Message message = JsonConvert.DeserializeObject<Message>(json);
                        message.Source = ipEndPoint;
                        IncomingQueue.Enqueue(message);
                    }
                } catch (ThreadAbortException) {
                    // ... just ignore
                } catch (Exception e) {
                    Console.Error.WriteLine("Failed to receive data", e);
                }
            });

            // build a thread for sending outgoing messages.
            outgoingThread = new Thread(() => {
                try {
                    while (true) {
                        while (!OutgoingQueue.IsEmpty) {
                            if (OutgoingQueue.TryDequeue(out Message message)) {
                                string json = JsonConvert.SerializeObject(message);
                                byte[] bytes = Encoding.UTF8.GetBytes(json);
                                if (message.Targets.Count == 0) {
                                    udpClient.Send(bytes, bytes.Length);
                                } else {
                                    foreach (IPEndPoint target in message.Targets) {
                                        udpClient.Send(bytes, bytes.Length, target);
                                    }
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                } catch (ThreadAbortException) {
                    // ... just ignore
                } catch (Exception e) {
                    Console.Error.Write("Failed to send data", e);
                }
            });
        }

        public void Close() {
            incomingThread.Abort();
            outgoingThread.Abort();
        }

        public void StartListening(int port) {
            if (udpClient != null) {
                throw new Exception("Already listening or connected to a remote target.");
            }

            // make UDP client to listen the target port.
            udpClient = new UdpClient(port);

            // start receiving and sending data.
            incomingThread.Start();
            outgoingThread.Start();
        }

        public void Connect(string host, int port) {
            if (udpClient != null) {
                throw new Exception("Already listening or connected to a remote target.");
            }

            // make UDP client and connect to target host and port.
            udpClient = new UdpClient();
            udpClient.Connect(host, port);

            // start receiving and sending data.
            incomingThread.Start();
            outgoingThread.Start();
        }

    }
}
