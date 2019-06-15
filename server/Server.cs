﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class Server
    {
        // The set containing the addresses of all connected clients.
        private HashSet<IPEndPoint> clientIps;

        // The UDP client abstraction for the UDP communication.
        private UdpClient udpClient;

        // The thread used by the reactor to capture network messages.
        private Thread reactorThread;

        public Server() {
            this.clientIps = new HashSet<IPEndPoint>();
            this.udpClient = new UdpClient(28018);
        }

        public void run() {
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
            Server server = new Server();
            server.run();

            Console.ReadKey();
            server.close();
        }
    }
}
