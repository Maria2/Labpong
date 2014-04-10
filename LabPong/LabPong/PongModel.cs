using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LabPong
{
    class PongModel : INotifyPropertyChanged
    {
        #region private variables
        private Point ballPos = new Point(300, 500);        
        private double playerX;
        private double playerY;
        private Boolean freeze = false;
        private int playerXScore;
        private int playerYScore;
        private List<String> items = new List<string>(3);
        private int negCounter = 0;
        private int posCounter = 0;
        static SoundPlayer player;
        private Boolean invert = false;
        private double resizeX = 0;
        private double resizeY = 0;        
        private delegate void VoidMethod(bool identifier);
        private Communicator communicator;
        #endregion
        #region static variables
        public static PongModel pongModel;        
        public static double WINDOW_HEIGHT;
        public static double WINDOW_WIDTH;
        public static double WINDOW_WIDTH_Y = 0;
        public static double WINDOW_HEIGHT_Y = 0;
        public static Point BallSize = new Point(50,50);
        public static Point PlayerSizes;
        public static String[] posItems = { "resize" };
        public static String[] negItems = { "white_screen", "invert", "freeze" };
        #endregion
        #region Properties

        public double ResizeX
        {
            get { return resizeX; }
            set { resizeX = value; }
        }

        public double ResizeY
        {
            get { return resizeY; }
            set { resizeY = value; }
        }        

        public int PlayerXScore
        {
            get { return playerXScore; }
            set 
            {
                if (value == playerXScore) return;
                playerXScore = value;
                IncrementPosCounter();
                NotifyPropertyChanged("playerXScore");
            }
        }        

        public int PlayerYScore
        {
            get { return playerYScore; }
            set 
            {
                if (value == playerYScore) return;                
                playerYScore = value;
                IncrementNegCounter();
                NotifyPropertyChanged("playerYScore");
            }
        }

        public double PlayerX
        {
            get { return playerX; }
            set
            {
                if (freeze) return;
                if (invert) value = -value;
                value = (value * 4) + ((WINDOW_HEIGHT/2) - (PlayerSizes.Y / 2));                 
                if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y) + 1)
                {
                    communicator.UDPSend(Translator.encodePlayerPosition(value));
                    playerX = value;                    
                    NotifyPropertyChanged("playerX");
                }
            }
        }

        public double PlayerY
        {
            get { return playerY; }
            set
            {
                    playerY = value;
                    NotifyPropertyChanged("playerY");
            }
        }

        public Point BallPos
        {
            get { return ballPos; }
            set
            {
                ballPos = value;
                NotifyPropertyChanged("ballPos");
            }
        }

        public int NegCounter
        {
            get { return negCounter; }
            set { negCounter = value; }
        }

        public int PosCounter
        {
            get { return posCounter; }
            set { posCounter = value; }
        }

        #endregion

        public PongModel(Communicator communicator)
        {
            pongModel = this;
            this.communicator = communicator;
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(SpaceKeyUp), true);
            App.CustomListener.PropertyChanged += CustomListener_PropertyChanged;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (new Random().Next(2) == 0)
                AddItem(posItems[new Random().Next(posItems.Length)]);
            else
                AddItem(negItems[new Random().Next(negItems.Length)]);
        }

        public void ResizePlayer(bool self)
        {
            if (self)
            {
                resizeX += (WINDOW_HEIGHT / 1360) * 20;
                NotifyPropertyChanged("sizeX");
                Thread.Sleep(5000);
                resizeX -= (WINDOW_HEIGHT / 1360) * 20;
                NotifyPropertyChanged("sizeX");
            }
            else
            {
                resizeY += (WINDOW_HEIGHT / 1360) * 20;
                NotifyPropertyChanged("sizeY");
                Thread.Sleep(5000);
                resizeY -= (WINDOW_HEIGHT / 1360) * 20;
                NotifyPropertyChanged("sizeY");
            }
        }

        public void ChangeColor()
        {
            NotifyPropertyChanged("white");
        }

        public void Freeze()
        {
            freeze = true;
            Thread.Sleep(500);
            freeze = false;
        }

        public void InvertMove()
        {
            invert = true;
            Thread.Sleep(7000);
            invert = false;
        }

        public static void PlayAudio(String audio)
        {
            switch (audio)
            {
                case "ball_hit": 
                    player = new SoundPlayer("resources/Ball-hits-player.wav");
                    break;
                case "defeat":
                    player = new SoundPlayer("resources/Defeat.wav");
                    break;
                case "victory":
                    player = new SoundPlayer("resources/Victory.wav");
                    break;
            }
            player.Play();
            player.Dispose(); 
        }

        private void SpaceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
            if (items.Count() > 0)
            {
                switch (items[0])
                {
                    case "white_screen": communicator.UDPSend(Translator.encodeExtra("white_screen")); break;
                    case "freeze": communicator.UDPSend(Translator.encodeExtra("freeze")); break;                            
                    case "invert": communicator.UDPSend(Translator.encodeExtra("invert")); break;
                    case "resize": new VoidMethod(ResizePlayer).BeginInvoke(true, null, null);
                        communicator.UDPSend(Translator.encodeExtra("resize")); break;
                }
                NotifyPropertyChanged("del");
                items.RemoveAt(0);
            }
        }

        public void IncrementPosCounter()
        {
            NegCounter = 0;
            PosCounter++;
            if (PosCounter % 2 == 0){
                AddItem(posItems[new Random().Next(posItems.Length)]);
                player = new SoundPlayer("resources/2points.wav");
                player.Play();
                player.Dispose();
            }
            if (posCounter % 5 == 0)
            {
                player = new SoundPlayer("resources/5inarow.wav");
                player.Play();
                player.Dispose();
            }
        }

        public void SendPos()
        {
            communicator.UDPSend(Translator.encodeExtra("sizes|" + WINDOW_HEIGHT + "|" + WINDOW_WIDTH));
        }

        public void IncrementNegCounter()
        {
            PosCounter = 0;
            NegCounter++;
            if (NegCounter % 3 == 0)
                AddItem(negItems[new Random().Next(negItems.Length)]);
            player = new SoundPlayer("resources/Lose_point.wav");
            player.Play();
            player.Dispose();
        }

        private void CustomListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
                PlayerX = ((CustomListener)sender).Position.Y;
        }

        public void AddItem(String item)
        {
            if (items.Count < 3)
            {
                items.Add(item);                
                NotifyPropertyChanged("add:"+item);
            }
        }

        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangedEventHandler PropertyChanged;        
    }
}
