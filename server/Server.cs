using Newtonsoft.Json;
using shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class Server
    {
        // The set containing the addresses of all connected clients.
        private HashSet<IPEndPoint> clientIps;

        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;

        // The thread used by the reactor to capture network messages.
        private Thread reactorThread;

        // The queue containing data that is going to be distributed to clients.
        private ConcurrentQueue<Message> outgoingQueue;

        public Server() {
            this.clientIps = new HashSet<IPEndPoint>();
            this.udpClient = new UdpClient(28018);
            this.outgoingQueue = new ConcurrentQueue<Message>();
        }

        public void Run() {
            reactorThread = new Thread(() => {
                while (true) {
                    // TODO Ping each connected client.
                    
                    // Check and receive the data from the UDP socket.
                    while (udpClient.Available != 0) {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] received = udpClient.Receive(ref ipEndPoint);
                        clientIps.Add(ipEndPoint);
                        string json = Encoding.UTF8.GetString(received);
                        Message message = JsonConvert.DeserializeObject<Message>(json);
                        Console.WriteLine("> " + message.Text);
                        outgoingQueue.Enqueue(message);
                    }

                    // Send outgoing messages to connected clients.
                    while (!outgoingQueue.IsEmpty) {
                        if (outgoingQueue.TryDequeue(out Message message)) {
                            string json = JsonConvert.SerializeObject(message);
                            byte[] bytes = Encoding.UTF8.GetBytes(json);
                            foreach (IPEndPoint clientIp in clientIps) {
                                udpClient.Send(bytes, bytes.Length, clientIp);
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
                // TODO Send close message to connected clients.
            });
            reactorThread.Start();
        }

        public void Close() {
            reactorThread.Abort();   
        }

        static void Main() {
            Server server = new Server();
            server.Run();

            Console.ReadKey();
            server.Close();
        }
    }
}
