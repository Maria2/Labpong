﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LabPong
{
    public class Server
    {
        //Request bytes
        static readonly byte SERVER_REQUEST = 1;

        public static IPAddress LocalIP;
        public static IPAddress ServerIP;

        static Server()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                LocalIP = IPAddress.None;
            else
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                LocalIP = host
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }

            ServerIP = new IPAddress(getServerIP());
        }


        static byte[] getServerIP()
        {
            ////String[] serverString = Properties.Settings.Default.serverIP.Split(new char[]{'.'});
            //if (serverString.Length != 4) return null;
            //byte[] serverAddess = new byte[4];
            //for (int i = 0; i < serverString.Length; i++)
            //    serverAddess[i] = Byte.Parse(serverString[i]);
            //return serverAddess;
            return null;
        }

        public static void StartListening()
        {
            if (LocalIP.Equals(IPAddress.None) || ServerIP.Equals(IPAddress.None)) return;
            
            TcpClient tcp = new TcpClient();
            tcp.Client.Bind(new IPEndPoint(LocalIP, 20300));
            tcp.Connect(ServerIP, 30200);
            tcp.Client.Send(new byte[] { SERVER_REQUEST });
            tcp.Client.Send(new byte[] { 2, 0, 3, 0, 0});

            TcpListener tcpListener = new TcpListener(LocalIP, 30200);
            tcpListener.Start();
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests. 
                // You could also user server.AcceptSocket() here.
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Connected!");
            }
        }
    }
}
