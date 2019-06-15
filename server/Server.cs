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
        private ConcurrentQueue<string> outgoingQueue;

        public Server() {
            this.clientIps = new HashSet<IPEndPoint>();
            this.udpClient = new UdpClient(28018);
            this.outgoingQueue = new ConcurrentQueue<string>();
        }

        public void Run() {
            reactorThread = new Thread(() => {
                while (true) {
                    // TODO Ping each connected client.
                    
                    // Check and receive the data from the UDP socket.
                    while (udpClient.Available != 0) {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] received = udpClient.Receive(ref ipEndPoint);
                        Console.WriteLine("> " + Encoding.UTF8.GetString(received));
                    }

                    // Send outgoing messages to connected clients.
                    while (!outgoingQueue.IsEmpty) {
                        if (outgoingQueue.TryDequeue(out string message)) {
                            byte[] bytes = Encoding.UTF8.GetBytes(message);
                            udpClient.Send(bytes, bytes.Length);
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

        static void Main(string[] args) {
            Server server = new Server();
            server.Run();

            Console.ReadKey();
            server.Close();
        }
    }
}
