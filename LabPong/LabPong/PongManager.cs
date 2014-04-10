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
        Pong pong = null;
        delegate void DelegateVoid();

        public PongManager(ConnectPage connect)
        {
            connectPage = connect;
            mainDispatcher = connectPage.Dispatcher;
            communicator = new Communicator();
        }

        public void hostGame(String ip)
        {
            connectPage.Toggle_HostButton();
            communicator.Host(ip);            
            if (!communicator.Connected)
            {
                connectPage.Toggle_HostButton();
                return;
            }
            connectPage.Dispatcher.BeginInvoke(new Action(() => connectPage.Measure1 = false), null);
            PongLogic ponglogic = new PongLogic(communicator);
            mainDispatcher.BeginInvoke(new Action(() => this.CreatePongWindow()), null);
            while (communicator.Connected) ;
            mainDispatcher.BeginInvoke(new DelegateVoid(pong.Close), null);
            connectPage.Dispatcher.BeginInvoke(new Action(() => connectPage.Measure1 = true), null);
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
            connectPage.Dispatcher.BeginInvoke(new Action(() => connectPage.Measure1 = false), null);
            PongModel pongModel = new PongModel(communicator);
            mainDispatcher.BeginInvoke(new Action(() => this.CreatePongWindow()), null);
            while (communicator.Connected) ;
            mainDispatcher.BeginInvoke(new DelegateVoid(pong.Close), null);
            connectPage.Dispatcher.BeginInvoke(new Action(() => connectPage.Measure1 = true), null);
        }

        private void CreatePongWindow()
        {
            pong = new Pong();
            pong.Show();
        }
    }
}
