using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace client
{
    class Client
    {
        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;
        
        // The thread used by the reactor to capture network messages.
        private Thread reactorThread;

        // The queue containing data that is going to be sent to server.
        private ConcurrentQueue<string> outgoingQueue;

        public Client() {
            this.udpClient = new UdpClient();
            this.outgoingQueue = new ConcurrentQueue<string>();
        }

        public void Run(string host, int port) {
            udpClient.Connect(host, port);
            reactorThread = new Thread(() => {
                while (true) {
                    // TODO Ping server.
                    
                    // Check and receive the data from the UDP socket.
                    while (udpClient.Available != 0) {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] received = udpClient.Receive(ref ipEndPoint);
                        Console.WriteLine("> " + Encoding.UTF8.GetString(received));
                    }

                    // Send all waiting outgoing messages to server.
                    while (!outgoingQueue.IsEmpty) {
                        if (outgoingQueue.TryDequeue(out string message)) {
                            byte[] bytes = Encoding.UTF8.GetBytes(message);
                            udpClient.Send(bytes, bytes.Length);
                        }
                    }
                    Thread.Sleep(1);
                }
                // TODO Send close message to server.
            });
            reactorThread.Start();
        }

        public void Close() {
            reactorThread.Abort();
        }

        public void Send(string message)
        {
            outgoingQueue.Enqueue(message);
        }

        static void Main(string[] args) {
            Client client = new Client();
            client.Run("localhost", 28018);

            for (int i = 0; i < 5; i++) {
                string message = Console.ReadLine();
                client.Send(message);
            }

            Console.ReadKey();
            client.Close();
        }
    }
}
