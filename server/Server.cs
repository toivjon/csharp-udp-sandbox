using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Server
    {
        // The set containing the addresses of all connected clients.
        private HashSet<IPEndPoint> clientIps;

        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;

        public Server() {
            this.clientIps = new HashSet<IPEndPoint>();
            this.udpClient = new UdpClient(28018);
        }

        public void run() {
            IPEndPoint clientIp = new IPEndPoint(IPAddress.Any, 0);
            byte[] received = udpClient.Receive(ref clientIp);
            byte[] outgoing = Encoding.UTF8.GetBytes("Thanks for the message! Bye bye!");
            udpClient.Send(outgoing, outgoing.Length, clientIp);
            udpClient.Close();

            Console.WriteLine("Received data: " + Encoding.UTF8.GetString(received));
        }

        static void Main(string[] args) {
            Server server = new Server();
            server.run();

            Console.ReadKey();
        }
    }
}
