using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LabPong
{
    class PongLogic 
    {
        #region instance-scope variables
        PongModel pongModel;
        delegate void NoArgDelegate();   
        //private Point ballPos = new Point(300,500);
        private Point ballSpeed = new Point(10, 0);
        //private double playerX;
        //private double playerY;
        //private int playerXScore;
        //private int playerYScore
        #endregion
        #region static variables
        public static Boolean GameStarted;
        //public static double WINDOW_HEIGHT;
        //public static double WINDOW_WIDTH;
        //public static Point BallSize = new Point(50,50);
        //public static Point PlayerSizes = new Point(15, 220);
        #endregion
        //public int PlayerXScore
        //{
        //    get { return playerXScore; }
        //    set 
        //    {
        //        if (value == playerXScore) return;
        //        playerXScore = value;
        //        NotifyPropertyChanged("playerXScore");
        //    }
        //}

        //public int PlayerYScore
        //{
        //    get { return playerYScore; }
        //    set 
        //    {
        //        if (value == playerYScore) return;
        //        playerYScore = value;
        //        NotifyPropertyChanged("playerYScore");
        //    }
        //}

        //public double PlayerX
        //{
        //    get { return playerX; }
        //    set
        //    {
        //        value = (value * 4) + ((WINDOW_HEIGHT / 2) - (PlayerSizes.Y / 2));
        //        if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y) + 1)
        //        {
        //            playerX = value;
        //            NotifyPropertyChanged("playerX");
        //        }
        //    }
        //}

        //public double PlayerY
        //{
        //    get { return playerY; }
        //    set
        //    {
        //        value = (value * 4) + ((WINDOW_HEIGHT / 2) - (PlayerSizes.Y / 2));
        //        if (value > -1 && value < (WINDOW_HEIGHT - PlayerSizes.Y) + 1)
        //        {
        //            playerY = value;
        //            NotifyPropertyChanged("playerY");
        //        }
        //    }
        //}

        //public Point BallPos
        //{
        //    get { return ballPos; }
        //    set
        //    {
        //        ballPos = value;
        //        NotifyPropertyChanged("ballPos");
        //    }
        //}

        //void CustomListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{         
        //    if (e.PropertyName.Equals("Position"))
        //    {
        //        PlayerX = ((CustomListener)sender).Position.Y;
        //        PlayerY = -((CustomListener)sender).Position.Y;
        //    }
        //}

        public PongLogic()
        {            
            //listener = this;
            //App.CustomListener.PropertyChanged += CustomListener_PropertyChanged;            
            pongModel = new PongModel();
            new NoArgDelegate(StartGame).BeginInvoke(null, null);
        }

        private void StartGame()
        {
            while (!GameStarted) ;
            pongModel.BallPos = new Point((PongModel.WINDOW_WIDTH / 2) - (PongModel.BallSize.X / 2), (PongModel.WINDOW_HEIGHT / 2) - (PongModel.BallSize.Y / 2));
            InitializeDirectionIncrement();
            while (pongModel.PlayerXScore < 10 && pongModel.PlayerYScore < 10)
            {
                Thread.Sleep(100);
                pongModel.BallPos = new Point(pongModel.BallPos.X + ballSpeed.X * 10, pongModel.BallPos.Y + ballSpeed.Y * 10);
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
            Console.WriteLine(new Rect(pongModel.BallPos.X, pongModel.BallPos.Y, PongModel.BallSize.X, PongModel.BallSize.Y).
                IntersectsWith(new Rect(pongModel.PlayerX, 0, PongModel.PlayerSizes.X, PongModel.PlayerSizes.Y)));
            //Change
            //if ((int)pongModel.BallPos.X == (int)(PongModel.PlayerSizes.X / 2))
            //{
            //    if ((pongModel.PlayerX - 20) < pongModel.BallPos.Y && (pongModel.PlayerX + PongModel.PlayerSizes.Y - 15) > pongModel.BallPos.Y)
            //        ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
            //    else
            //    {
            //        pongModel.PlayerYScore++;
            //        ResetBall();
            //    }
            //}
            //if ((int)(pongModel.BallPos.X + PongModel.BallSize.X / 2) == (int)(PongModel.WINDOW_WIDTH - (PongModel.PlayerSizes.X * 3)))
            //{
            //    if ((pongModel.PlayerY - 20) < pongModel.BallPos.Y && (pongModel.PlayerY + PongModel.PlayerSizes.Y - 15) > pongModel.BallPos.Y)
            //        ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
            //    else
            //    {
            //        pongModel.PlayerXScore++;
            //        ResetBall();
            //    }
            //}
            if (pongModel.BallPos.Y < 0)
            {
                pongModel.BallPos = new Point(pongModel.BallPos.X, 0);
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }
            if (pongModel.BallPos.Y > PongModel.WINDOW_HEIGHT - PongModel.BallSize.Y)
            {
                pongModel.BallPos = new Point(pongModel.BallPos.X, PongModel.WINDOW_HEIGHT - PongModel.BallSize.Y);
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }
        }

        private void ResetBall()
        {
            pongModel.BallPos = new Point((PongModel.WINDOW_WIDTH / 2) - (PongModel.BallSize.X / 2), (PongModel.WINDOW_HEIGHT / 2) - (PongModel.BallSize.Y / 2));
            InitializeDirectionIncrement();
        }
    }
}