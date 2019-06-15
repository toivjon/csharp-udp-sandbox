using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace client
{
    class Client
    {
        // The IP end point address of the server.
        private IPEndPoint serverIp;

        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;
        
        // The thread used by the reactor to capture network messages.
        private Thread reactorThread;

        public Client() {
            this.serverIp = new IPEndPoint(IPAddress.Any, 0);
            this.udpClient = new UdpClient();
        }

        public void run(string host, int port) {
            reactorThread = new Thread(() => {
                while (true) {
                    // TODO Ping each connected client.
                    // TODO Receive incoming messages.
                    // TODO Send outgoing messages.
                    Thread.Sleep(1);
                }
                // TODO Send close message to connected clients.
            });
            reactorThread.Start();
        }

        public void close() {
            reactorThread.Abort();
        }
        static void Main(string[] args) {
            Client client = new Client();
            client.run("localhost", 28018);

            Console.ReadKey();
            client.close();
        }
    }
}
