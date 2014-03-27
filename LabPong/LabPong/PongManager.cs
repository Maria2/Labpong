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
            communicator.Host();
            connectPage.Toggle_HostButton();
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
            communicator.Join(ip);
            connectPage.Toggle_HostButton();
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
