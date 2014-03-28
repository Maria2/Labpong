using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace LabPong
{
    class PongModel : INotifyPropertyChanged
    {
        #region private variables
        private Point ballPos = new Point(300, 500);        
        private double playerX;
        private double playerY;        
        private int playerXScore;
        private int playerYScore;
        private List<String> items = new List<string>(3);
        private int negCounter = 0;
        private int posCounter = 0;
        private int shots = 0;
        private Boolean invert = false;
        private Communicator communicator;
        #endregion
        #region static variables
        public static PongModel pongModel;        
        public static double WINDOW_HEIGHT;
        public static double WINDOW_WIDTH;
        public static Point BallSize = new Point(50,50);
        public static Point PlayerSizes;
        public static String[] posItems = { "shot", "ball_direction" };
        public static String[] negItems = { "white_screen", "invert", "resize" };
        #endregion
        #region Properties

        public Boolean Invert
        {
            set {
                if (value == true) new Timer(onTimer, null, 3000, Timeout.Infinite).Change(3000, Timeout.Infinite);
                invert = value;
            }
        }

        public int PlayerXScore
        {
            get { return playerXScore; }
            set 
            {
                if (value == playerXScore) return;
                playerXScore = value;
                incrementPosCounter();
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
                incrementNegCounter();
                NotifyPropertyChanged("playerYScore");
            }
        }

        public double PlayerX
        {
            get { return playerX; }
            set
            {
                if (invert) value = -value;
                value = (value * 4) + ((WINDOW_HEIGHT/2) - (PlayerSizes.Y / 2));                 
                if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y - PlayerSizes.Y/2) + 1)
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
            communicator.UDPSend(Translator.encodeExtra("WINDOW_HEIGHT|" + System.Windows.SystemParameters.PrimaryScreenHeight));
        }

        private void SpaceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
            if (items.Count() > 0)
            {
                switch (items[0])
                {
                    //case "shot": shots = 3; break;
                    case "ball_direction": communicator.UDPSend(Translator.encodeExtra("ball_direction")); break;
                    case "white_screen": communicator.UDPSend(Translator.encodeExtra("white_screen")); break;
                    case "invert": communicator.UDPSend(Translator.encodeExtra("invert")); break;
                    case "resize": communicator.UDPSend(Translator.encodeExtra("resize")); break;
                }
                NotifyPropertyChanged("del");
                items.RemoveAt(0);
            }
        }

        private void onTimer(object state)
        {
            invert = false;
        }

        public void incrementPosCounter()
        {
            NegCounter = 0;
            PosCounter++;
            if (PosCounter % 2 == 0)
                addItem(posItems[new Random().Next(posItems.Length)]);
        }

        public void incrementNegCounter()
        {
            PosCounter = 0;
            NegCounter++;
            if (NegCounter % 3 == 0)
                addItem(negItems[new Random().Next(negItems.Length)]);
        }

        private void CustomListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
                PlayerX = ((CustomListener)sender).Position.Y;
        }

        public void addItem(String item)
        {
            if (items.Count < 3)
            {
                items.Add(item);
                NotifyPropertyChanged("add:"+item);
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
