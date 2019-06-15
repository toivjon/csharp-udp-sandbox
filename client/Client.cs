using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes("Hello from client!");
            IPEndPoint serverIp = new IPEndPoint(IPAddress.Any, 0);

            UdpClient client = new UdpClient();
            client.Connect("localhost", 28018);
            client.Send(bytes, bytes.Length);
            byte[] incoming = client.Receive(ref serverIp);
            client.Close();

            Console.WriteLine("Received data: " + Encoding.UTF8.GetString(incoming));

            Console.ReadKey();
        }
    }
}
