using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        #endregion
        #region static variables
        public static PongModel pongModel;
        public static double WINDOW_HEIGHT;
        public static double WINDOW_WIDTH;
        public static Point BallSize = new Point(50,50);
        public static Point PlayerSizes = new Point(15, 220);
        public static String[] posItems = { "shot", "ball_direction" };
        public static String[] negItems = { "white_screen", "invert", "resize" };
        #endregion
        #region Properties
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
                value = (value * 4) + ((WINDOW_HEIGHT / 2) - (PlayerSizes.Y / 2));
                if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y) + 1)
                {
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
                value = (value * 4) + ((WINDOW_HEIGHT / 2) - (PlayerSizes.Y / 2));
                if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y) + 1)
                {
                    playerY = value;
                    NotifyPropertyChanged("playerY");
                }
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

        public PongModel()
        {
            pongModel = this;
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(SpaceKeyUp), true);
            App.CustomListener.PropertyChanged += CustomListener_PropertyChanged;
        }

        private void SpaceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
            if (items.Count() > 0)
            {
                switch (items[0])
                {
                    case "shot": shots = 3; break;
                    //case "ball_direction": BallS = new Point(-ballSpeed.X, ballSpeed.Y); break;
                    //case "white_screen": break;
                    case "invert": break;
                    case "resize": break;
                }
                NotifyPropertyChanged("del");
                items.RemoveAt(0);
            }
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
            {
                PlayerX = ((CustomListener)sender).Position.Y;
            }
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
