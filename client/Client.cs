using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Client
    {
        // The IP end point address of the server.
        private IPEndPoint serverIp;

        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;

        public Client() {
            this.serverIp = new IPEndPoint(IPAddress.Any, 0);
            this.udpClient = new UdpClient();
        }

        public void run(string host, int port) {
            udpClient.Connect(host, port);
            Byte[] outgoing = Encoding.UTF8.GetBytes("Hello from client!");
            udpClient.Send(outgoing, outgoing.Length);
            byte[] incoming = udpClient.Receive(ref serverIp);
            udpClient.Close();

            Console.WriteLine("Received data: " + Encoding.UTF8.GetString(incoming));
        }

        static void Main(string[] args)
        {
            Client client = new Client();
            client.run("localhost", 28018);

            Console.ReadKey();
        }
    }
}
