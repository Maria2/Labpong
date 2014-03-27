using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace LabPong
{
    class PongManager
    {
        ConnectPage connectPage;
        Communicator communicator;
        Dispatcher mainDispatcher;
        delegate void DelegateVoid();

        public PongManager(ConnectPage connect)
        {
            connectPage = connect;
            mainDispatcher = connectPage.Dispatcher;
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
            connectPage.Dispatcher.BeginInvoke(new DelegateVoid(connectPage.Close), null);
            PongLogic ponglogic = new PongLogic(communicator);
            mainDispatcher.BeginInvoke(new Action(() => this.CreatePongWindow()), null);
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
            mainDispatcher.BeginInvoke(new DelegateVoid(connectPage.Close), null);
            PongModel pongModel = new PongModel(communicator);
            mainDispatcher.BeginInvoke(new Action(() => this.CreatePongWindow()), null);
        }

        private void CreatePongWindow()
        {
            Pong pong = new Pong();
            pong.Show();            
        }
    }
}
