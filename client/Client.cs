﻿using System;
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

        public void Run(string host, int port) {
            reactorThread = new Thread(() => {
                while (true) {
                    // TODO Ping server.
                    
                    // Check and receive the data from the UDP socket.
                    while (udpClient.Available != 0) {
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] received = udpClient.Receive(ref ipEndPoint);
                        Console.WriteLine("> " + Encoding.UTF8.GetString(received));
                    }

                    // TODO Send outgoing messages.
                    Thread.Sleep(1);
                }
                // TODO Send close message to server.
            });
            reactorThread.Start();
        }

        public void Close() {
            reactorThread.Abort();
        }
        static void Main(string[] args) {
            Client client = new Client();
            client.Run("localhost", 28018);

            Console.ReadKey();
            client.Close();
        }
    }
}
