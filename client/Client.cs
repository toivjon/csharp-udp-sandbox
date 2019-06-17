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
        public Client() : base() {
            // ...
        }

        static void Main(string[] args) {
            Client client = new Client();
            client.Connect("localhost", 28018);

            for (int i = 0; i < 5; i++) {
                string message = Console.ReadLine();
                client.OutgoingQueue.Enqueue(new Message() { Text = message });
            }
            client.Close();
        }
    }
}
