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
        const int maxBuffer = 100;
        int port = 11000;
        private String joinIP;
        private String username = Properties.Settings.Default.Username;
        static Int32 portUDP = 11000;
        //UdpClient with port
        static UdpClient udpClientR = new UdpClient(portUDP);
        UdpClient udpClientS = new UdpClient();
        static Thread thread;

       
       public void Join(String ip)
        {
           joinIP = ip;
           FTPReciever(); //start recieving
           FTPSender(); //transmit username              
        }

        public void Host()
        {
            FTPSender(); //transmit username
            FTPReciever(); //start recieving
        }
        //----- FTP Sender (Client) ----

        private void FTPSender()
        { 
            TcpClient tcpClient;
            Stream tcpStream;
            const int maxBuffer = 100;

            IPAddress ipAdresse = IPAddress.Parse(joinIP);
            int port = 11000;
            tcpClient = new TcpClient(); 
            try
            {
                tcpClient.Connect(ipAdresse, port);

            }
            catch (Exception exp)
            {
                // return error code connection dead
                Console.WriteLine(exp.StackTrace);
            }
            try
            {
                tcpStream = tcpClient.GetStream();

                UTF8Encoding encoding = new UTF8Encoding();

                byte[] ba = encoding.GetBytes(";"+username+";");
                tcpStream.Write(ba, 0, ba.Length);
                byte[] buffer = new byte[maxBuffer];
                int gelesen = tcpStream.Read(buffer, 0, maxBuffer);
                string empfangen = encoding.GetString(buffer, 0, gelesen);
                if (empfangen.Equals("Ack"))
                {
                    tcpStream.Close();
                    tcpClient.Close();
                    PongLogic p = new PongLogic();
                    //start udp
                }
            }
            catch (Exception) { /*error Message */ return; }
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
                    string toSend = "ACK";
                    socket.Send(encoding.GetBytes(toSend));
                }
                while (received != 0);
            }
            catch (Exception)
            {
                // error message
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
                Console.WriteLine("Keine IPV4-IP auflösbar."); Console.ReadKey();
            return ipAdresse;
        }
        public void UDPSend(String message)
        {
            //Where to send it to
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Broadcast, portUDP);
            //Message
            byte[] content = Encoding.ASCII.GetBytes(message);
            try
            {
                //Sending
                udpClientS.Send(content, content.Length, ipEndPoint);
            }
            catch
            {
                Console.WriteLine("ERROR");
            }
        }
        public void UDPReceive()
        {
            Communicator c = new Communicator();
            //Start a new thread with the ReceiveMessage method to listen for input
            thread = new Thread(c.ReceiveMessage);
            thread.IsBackground = true;
            thread.Start();
            Console.ReadKey();
        }
        public void ReceiveMessage()
        {// ruft translator auf um daten rauszulesen
            Translator t = new Translator();
            while (true)
            {
                //Wait for any IPAddress to send something on port 11000
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, portUDP);
                //Load content
                byte[] content = udpClientR.Receive(ref remoteIPEndPoint);
                String contenttoreturn = "";
                if (content.Length > 0)
                {
                    for (int i = 0; i < content.Length; i++)
                    {
                        contenttoreturn = contenttoreturn + content[i];
                        Console.WriteLine(contenttoreturn);
                    }
                    // Console.WriteLine(contenttoreturn);
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    byte[] contenttoreturn1 = enc.GetBytes(contenttoreturn);
                    t.decodeMessage(contenttoreturn1);
                }
            }
        }
    }
}
