using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace LabPong
{
    class PongLogic
    {
        #region instance-scope variables
        PongModel pongModel;
        delegate void NoArgDelegate();
        private Point ballSpeed = new Point(10, 0);
        #endregion
        #region static variables
        public static Boolean GameStarted;
        #endregion        

        public PongLogic()
        {
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
                Thread.Sleep(5);
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
            ballSpeed = new Point(Math.Sin(angle), -Math.Cos(angle));
        }        

        private void CheckCollision()
        {
            if (new Rect(pongModel.BallPos.X, pongModel.BallPos.Y, PongModel.BallSize.X, PongModel.BallSize.Y).
                IntersectsWith(new Rect(0, pongModel.PlayerX, PongModel.PlayerSizes.X, PongModel.PlayerSizes.Y)))
                    ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
            if (new Rect(pongModel.BallPos.X, pongModel.BallPos.Y, PongModel.BallSize.X, PongModel.BallSize.Y).
                IntersectsWith(new Rect(PongModel.WINDOW_WIDTH - PongModel.PlayerSizes.X, pongModel.PlayerY, PongModel.PlayerSizes.X, PongModel.PlayerSizes.Y)))
                ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
            if (pongModel.BallPos.X < 0)
            {
                pongModel.PlayerYScore++;                
                ResetBall();
            }
            if (pongModel.BallPos.X > PongModel.WINDOW_WIDTH)
            {
                pongModel.PlayerXScore++;
                ResetBall();
            }

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