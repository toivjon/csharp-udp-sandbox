using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using shared;

namespace client
{
    class Client : BaseApplication
    {
        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;
        
        // The thread used by the reactor to capture network messages.
        private Thread reactorThread;

        public Client() : base() {
            this.udpClient = new UdpClient();
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
                        string json = Encoding.UTF8.GetString(received);
                        Message message = JsonConvert.DeserializeObject<Message>(json);
                        Console.WriteLine("> " + message.Text);
                    }

                    // Send all waiting outgoing messages to server.
                    while (!OutgoingQueue.IsEmpty) {
                        if (OutgoingQueue.TryDequeue(out Message message)) {
                            string msgJson = JsonConvert.SerializeObject(message);
                            byte[] bytes = Encoding.UTF8.GetBytes(msgJson);
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

        public void Send(string text) {
            Message message = new Message();
            message.Text = text;
            OutgoingQueue.Enqueue(message);
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
