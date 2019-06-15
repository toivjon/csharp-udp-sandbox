using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Server
    {
        static void Main(string[] args)
        {
            IPEndPoint clientIp = new IPEndPoint(IPAddress.Any, 0);
            UdpClient client = new UdpClient(28018);
            byte[] received = client.Receive(ref clientIp);
            byte[] outgoing = Encoding.UTF8.GetBytes("Thanks for the message! Bye bye!");
            client.Send(outgoing, outgoing.Length, clientIp);
            client.Close();

            Console.WriteLine("Received data: " + Encoding.UTF8.GetString(received));

            Console.ReadKey();
        }
    }
}
