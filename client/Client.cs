using System;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes("Hello from client!");

            UdpClient client = new UdpClient();
            client.Connect("localhost", 28018);
            client.Send(bytes, bytes.Length);
            client.Close();

            Console.ReadKey();
        }
    }
}
