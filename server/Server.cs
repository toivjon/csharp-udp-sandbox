using Newtonsoft.Json;
using shared;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace server
{
    class Server : BaseApplication
    {
        public Server() : base() {
            // ...       
        }

        static void Main() {
            Server server = new Server();
            server.StartListening(28018);

            bool isRunning = true;
            while (isRunning) {
                while (!server.IncomingQueue.IsEmpty && server.IncomingQueue.TryDequeue(out Message message)) {
                    message.Targets.Add(message.Source);
                    server.OutgoingQueue.Enqueue(message);
                    Console.WriteLine("> " + message.Text);
                }
                Thread.Sleep(1);
            }

            Console.ReadKey();
            server.Close();
        }
    }
}
