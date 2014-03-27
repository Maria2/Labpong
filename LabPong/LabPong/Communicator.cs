using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LabPong
{
    class Communicator
    {
        IPAddress ipAdresse = null;
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
        private bool ongoing = true;

        public Boolean Connected
        {
            get { return connected; }
            set { connected = value; }
        }

       
       public void Join(String ip)
        {
           joinIP = IPAddress.Parse(ip);
           FTPSender(); //transmit username
           UDPReceive();
        }

        public void Host()
        {
            FTPReciever(); //start recieving
            UDPReceive();
        }
        //----- FTP Sender (Client) ----

        private void FTPSender()
        { 
            TcpClient tcpClient;
            Stream tcpStream;
            const int maxBuffer = 100;

            tcpClient = new TcpClient(); 
            try
            {
                tcpClient.Connect(joinIP, port);
                tcpStream = tcpClient.GetStream();
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] ba = encoding.GetBytes(";"+username+";");
                tcpStream.Write(ba, 0, ba.Length);
                byte[] buffer = new byte[maxBuffer];
                int gelesen = tcpStream.Read(buffer, 0, maxBuffer);
                string empfangen = encoding.GetString(buffer, 0, gelesen);
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
                GetIpAdresse(ref ipAdresse, Dns.GetHostName());
                tcpListener = new TcpListener(ipAdresse, port);
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
                string empfangen;

                do
                {
                    buffer = new byte[maxBuffer];
                    received = socket.Receive(buffer);
                    if (received == 0)
                    {
                        // wenn der Client "" sendet, soll der Server beenden oder errormessage
                    }
                    UTF8Encoding encoding = new UTF8Encoding();
                    empfangen = encoding.GetString(buffer, 0, received);
                    player2 = empfangen;
                    string toSend = "ACK";
                    socket.Send(encoding.GetBytes(toSend));
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
        
        private IPAddress GetIpAdresse(ref IPAddress ipAdresse, string hostName)
        {
            IPAddress[] ipAdressen = Dns.GetHostEntry(hostName).AddressList;
            foreach (IPAddress ip in ipAdressen)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAdresse = ip; break;
                }
            if (ipAdresse == null)
                Console.WriteLine("Keine IPV4-IP auflösbar.");
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
            while (ongoing)
            {
                //Load content
                byte[] content = udpClientR.Receive(ref remoteIPEndPoint);
                if (content.Length > 0)
                    ongoing = t.decode(new System.Text.ASCIIEncoding().GetString(content));
            }
            udpClientR.Close();
        }        
    }
}
