using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LabPong
{
    class Communicator
    {
        List<IPAddress> ipAdresse = new List<IPAddress>();
        IPAddress hostIp;
        Socket socket;
        TcpListener tcpListener;
        IPEndPoint remoteIPEndPoint;
        const int maxBuffer = 100;
        int port = 11001;
        private IPAddress joinIP;
        private String username = Properties.Settings.Default.Username;
        public static String player2;
        static int portUDP = 11000;
        //UdpClient with port
        UdpClient udpClientR = new UdpClient(portUDP);
        UdpClient udpClientS = new UdpClient();
        static Thread thread;
        private Boolean connected = true;

        public Boolean Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        public Communicator()
        {
            GetIpAdresse(ref hostIp, Dns.GetHostName());
        }
       
       public void Join(String ip)
        {
           joinIP = IPAddress.Parse(ip);
           FTPSender(); //transmit username
           UDPReceive();
        }

        public void Host(String ip)
        {
            hostIp = IPAddress.Parse(ip);
            FTPReciever(); //start recieving
            UDPReceive();
        }
        //----- FTP Sender (Client) ----

        private void FTPSender()
        { 
            TcpClient tcpClient;
            Stream tcpStream;

            tcpClient = new TcpClient(); 
            try
            {
                tcpClient.Connect(joinIP, port);
                tcpStream = tcpClient.GetStream();
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] ba = encoding.GetBytes(username);
                tcpStream.Write(ba, 0, ba.Length);

                tcpStream.Close();
                tcpClient.Close();
            }
            catch (Exception) {
                connected = false;
            }
        }
        //----- FTP Reciever (Server) ----
        private void FTPReciever()
        {
            try
            {
                tcpListener = new TcpListener(hostIp, port);
                tcpListener.Start();
            }
            catch (Exception)
            {
                //errorMessage
                connected = false;
                return;
            }
            try
            {
                socket = tcpListener.AcceptSocket();
                int received;
                byte[] buffer;

                do
                {
                    buffer = new byte[maxBuffer];
                    received = socket.Receive(buffer);
                    
                    UTF8Encoding encoding = new UTF8Encoding();
                    player2 = encoding.GetString(buffer, 0, received);
                                        
                    joinIP = (socket.RemoteEndPoint as IPEndPoint).Address;
                }
                while (received != 0);
            }
            catch (Exception)
            {
                // error message
                connected = false;
                if (socket != null) socket.Close();
                if (tcpListener != null) tcpListener.Stop();
                return;
            }
            if (socket != null) socket.Close();
            if (tcpListener != null) tcpListener.Stop();
        }
        public string[] ReturnIp()
        {
            String[] ips = new string[ipAdresse.Count];
            for (int i = 0; i < ipAdresse.Count; i++)
                ips[i] = ipAdresse[i].ToString();
            return ips;
        }

        private IPAddress GetIpAdresse(ref IPAddress ipAdresse, string hostName)
        {
            IPAddress[] ipAdressen = Dns.GetHostEntry(hostName).AddressList;
            foreach (IPAddress ip in ipAdressen)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    this.ipAdresse.Add(ip);
            }
            return ipAdresse;
        }
        public void UDPSend(String message)
        {
            //Message
            byte[] content = Encoding.ASCII.GetBytes(message);
            try
            {
                //Sending
                remoteIPEndPoint.Port = portUDP;
                udpClientS.Send(content, content.Length, remoteIPEndPoint);
            }
            catch
            {
                Console.WriteLine("ERROR");
            }
        }
        public void UDPReceive()
        {
            //Start a new thread with the ReceiveMessage method to listen for input
            thread = new Thread(ReceiveMessage);
            thread.IsBackground = true;
            thread.Start();
        }
        public void ReceiveMessage()
        {// ruft translator auf um daten rauszulesen
            Translator t = new Translator();
            //Wait for any IPAddress to send something on port 11000
            remoteIPEndPoint = new IPEndPoint(joinIP, portUDP);
            while (connected)
            {
                //Load content
                byte[] content = udpClientR.Receive(ref remoteIPEndPoint);
                if (content.Length > 0)
                    connected = t.decode(new System.Text.ASCIIEncoding().GetString(content));
            }
            udpClientR.Close();
            udpClientS.Close();
        }        
    }
}
