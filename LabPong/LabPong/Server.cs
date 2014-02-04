using System;
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
        static readonly byte CLIENT_REQUEST = 2;

        //Types of acknowledgements
        static readonly byte SERVER_ACKNOWLEDGED = 10;
        static readonly byte CLIENT_ACKNOWLEDGED = 11;
        static readonly byte SERVER_ERROR = 20;        
        static readonly byte CLIENT_ERROR = 21;

        public static IPAddress LocalIP;
        public static IPAddress ServerIP;

        
        private static void Main(string[] args)
        {
            UdpClient udpclient = new UdpClient();

            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222"); // clientadress
            udpclient.JoinMulticastGroup(multicastaddress);
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, 2222);

            Byte[] buffer = null;

            for (int i = 0; i <= 8000; i++)
            {
                buffer = Encoding.Unicode.GetBytes(i.ToString());
                udpclient.Send(buffer, buffer.Length, remoteep);
            }
        }
        static Server()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                LocalIP = IPAddress.None;
            else
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName()); // wofür dns?
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
