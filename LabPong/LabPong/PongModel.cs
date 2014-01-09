using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

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
        #endregion
        #region static variables
        public static PongModel pongModel;
        public static double WINDOW_HEIGHT;
        public static double WINDOW_WIDTH;
        public static Point BallSize = new Point(50,50);
        public static Point PlayerSizes = new Point(15, 220);
        #endregion
        #region Properties
        public int PlayerXScore
        {
            get { return playerXScore; }
            set 
            {
                if (value == playerXScore) return;
                playerXScore = value;
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
        #endregion

        public PongModel()
        {
            pongModel = this;
            App.CustomListener.PropertyChanged += CustomListener_PropertyChanged;
        }

        private void CustomListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                PlayerX = ((CustomListener)sender).Position.Y;
                PlayerY = -((CustomListener)sender).Position.Y;
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
