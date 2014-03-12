﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LabPong
{
    class Communicator
    {
        String FTPSendText = "";
        IPAddress ipAdresse = null;
        Socket socket;
        TcpListener tcpListener;
        const int maxBuffer = 100;
        int port = 11000;
       
        //----- FTP Sender (Client) ----

        private void FTPSender()
        { 
            TcpClient tcpClient;
            Stream tcpStream;
            const int maxBuffer = 100;

            String ipAdresse = "169.254.49.18";
            int port = 11000;
            try
            {
                tcpClient = new TcpClient();             
                tcpClient.Connect(ipAdresse, port);

            }
            catch (Exception exp)
            {
                // return error code connection dead
               return;
            }
            try
            {
                tcpStream = tcpClient.GetStream();

                UTF8Encoding encoding = new UTF8Encoding();

                byte[] ba = encoding.GetBytes(FTPSendText);
                tcpStream.Write(ba, 0, ba.Length);
                byte[] buffer = new byte[maxBuffer];
                int gelesen = tcpStream.Read(buffer, 0, maxBuffer);
                string empfangen = encoding.GetString(buffer, 0, gelesen);
          //    if (tcpStream != null) tcpStream.Close();
           //   if (tcpClient != null) tcpClient.Close();
            }
            catch (Exception exp) { /*error Message */ return; }
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
            catch (Exception exp)
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
            catch (Exception exp)
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

    }
}
