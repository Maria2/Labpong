using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LabPong
{
    class PongManager
    {
        ConnectPage connectPage;
        Communicator communicator;
        Translator translator;

        public PongManager(ConnectPage connect)
        {
            connectPage = connect;
            communicator = new Communicator();
        }

        public void hostGame()
        {
            connectPage.Toggle_HostButton();
            communicator.Host();            
            if (!communicator.Connected)
            {
                connectPage.Toggle_HostButton();
                return;
            }
            connectPage.Close();
            PongLogic ponglogic = new PongLogic(communicator);
            Pong pong = new Pong();
            pong.Show();
        }

        public void joinGame(String ip)
        {
            connectPage.Toggle_HostButton();
            communicator.Join(ip);            
            if (!communicator.Connected)
            {
                connectPage.Toggle_HostButton();
                return;
            }
            connectPage.Close();
            PongModel pongModel = new PongModel(communicator);            
            Pong pong = new Pong();
            pong.Show();
        }
    }
}
