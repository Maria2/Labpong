using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LabPong
{
    class PongLogic : INotifyPropertyChanged
    {
        #region instance-scope variables
        delegate void NoArgDelegate();   
        private Point ballPos = new Point(300,500);
        private Point ballSpeed = new Point(1,0);
        private double playerX;
        private double playerY;
        private int playerXScore;
        private int playerYScore;
        #endregion
        #region static variables
        public static Boolean GameStarted;
        public static PongLogic listener;
        public static double WINDOW_HEIGHT;
        public static double WINDOW_WIDTH;
        public static Point BallSize = new Point(50,50);
        public static Point PlayerSizes = new Point(15, 220);
        #endregion

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

        void CustomListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                PlayerX = ((CustomListener)sender).Position.Y;
                PlayerY = -((CustomListener)sender).Position.Y;
            }
        }

        public PongLogic()
        {
            listener = this;
            App.CustomListener.PropertyChanged += CustomListener_PropertyChanged;
            new NoArgDelegate(StartGame).BeginInvoke(null, null);
        }
       
        private void StartGame()
        {
            while (!GameStarted) ;
            BallPos = new Point((WINDOW_WIDTH / 2) - (BallSize.X / 2), (WINDOW_HEIGHT / 2) - (BallSize.Y / 2));
            InitializeDirectionIncrement();    
            while (PlayerXScore < 10 && PlayerYScore < 10)
            {
                Thread.Sleep(1);
                BallPos = new Point(BallPos.X + ballSpeed.X, BallPos.Y + ballSpeed.Y);
                CheckCollision();
            }            
        }

        private void InitializeDirectionIncrement()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            double angle = (Math.PI / 6.0) * 1000.0;
            angle = ((double)random.Next((int)angle) / 1000.0) +
              (Math.PI / 6.0);
            angle += random.Next(3) * (Math.PI / 2.0);
            ballSpeed = new Point(Math.Sin(angle), - Math.Cos(angle));
        }

        private void CheckCollision()
        {
            if ((int)BallPos.X == (int)(PlayerSizes.X / 2))
            {
                if ((PlayerX - 20) < BallPos.Y && (PlayerX + PlayerSizes.Y - 15) > BallPos.Y )
                    ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
                else
                {
                    PlayerYScore++;
                    ResetBall();
                }
            }
            if ((int)(BallPos.X + BallSize.X / 2) == (int)(WINDOW_WIDTH - (PlayerSizes.X * 3)))
            {
                if ((PlayerY - 20) < BallPos.Y && (PlayerY + PlayerSizes.Y - 15) > BallPos.Y)
                    ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
                else
                {
                    PlayerXScore++;
                    ResetBall();
                }
            }
            if (BallPos.Y < 0)
            {
                BallPos = new Point(BallPos.X, 0);
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }
            if (BallPos.Y > WINDOW_HEIGHT - BallSize.Y)
            {
                BallPos = new Point(BallPos.X, WINDOW_HEIGHT - BallSize.Y);
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }
        }

        private void ResetBall()
        {
            BallPos = new Point((WINDOW_WIDTH / 2) - (BallSize.X / 2), (WINDOW_HEIGHT / 2) - (BallSize.Y / 2));
            InitializeDirectionIncrement();
            Thread.Sleep(2000);
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
