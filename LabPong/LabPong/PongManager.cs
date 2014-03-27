using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LabPong
{
    class PongManager
    {
        Window connectPage;
        Communicator communicator;
        Translator translator;

        public PongManager(Window connect)
        {
            connectPage = connect;
            communicator = new Communicator();
        }

        public void hostGame()
        {
            communicator.Host();
            if (!communicator.Connected) return;
            connectPage.Close();
            PongLogic ponglogic = new PongLogic(communicator);
            Pong pong = new Pong();
            pong.Show();
        }

        public void joinGame(String ip)
        {
            communicator.Join(ip);
            if (!communicator.Connected) return;
            connectPage.Close();
            PongModel pongModel = new PongModel(communicator);            
            Pong pong = new Pong();
            pong.Show();
        }
    }
}
