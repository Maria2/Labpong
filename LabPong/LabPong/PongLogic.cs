using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Timers;
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
        private Point ballSpeed = new Point(10, 0);
        Communicator communicator;
        public int timeout = 20;
        Boolean collision = false;
        #endregion
        #region static variables
        public static Boolean GameStarted;
        #endregion        

        public PongLogic(Communicator communicator)
        {
            pongModel = new PongModel(communicator);
            this.communicator = communicator;
            new NoArgDelegate(StartGame).BeginInvoke(null, null);
        }

        private void sendBallPos()
        {
            communicator.UDPSend(Translator.encodeBallPosition(new Point(PongModel.WINDOW_WIDTH_Y - pongModel.BallPos.X, pongModel.BallPos.Y)));
        }

        private void StartGame()
        {
            while (!GameStarted) ;            
            pongModel.BallPos = new Point((PongModel.WINDOW_WIDTH / 2) - (PongModel.BallSize.X / 2), (PongModel.WINDOW_HEIGHT / 2) - (PongModel.BallSize.Y / 2));
            sendBallPos();
            InitializeDirectionIncrement();
            while (pongModel.PlayerXScore < 10 && pongModel.PlayerYScore < 10)
            {
                Thread.Sleep(timeout);
                pongModel.BallPos = new Point(pongModel.BallPos.X + ballSpeed.X * 10, pongModel.BallPos.Y + ballSpeed.Y * 10);
                sendBallPos();
                CheckCollision();
            }
            String highscore = new DateTime().ToString()+" Datum "
                +Properties.Settings.Default.Username+" "+pongModel.PlayerXScore+":"+pongModel.PlayerYScore+" "+Communicator.player2;
            communicator.UDPSend(Translator.encodeGameEnd(highscore));
            if (pongModel.PlayerXScore > pongModel.PlayerYScore)
                play("victory");
            else
                play("defeat");
            StreamWriter file = new StreamWriter("resources/highscore.txt", true);
            file.WriteLine(highscore); file.Flush(); file.Close();
            communicator.Connected = false;
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

        private void play(String audio)
        {
            SoundPlayer player = null;
            switch (audio)
            {
                case "ball_hit":                    
                    communicator.UDPSend(Translator.encodeExtra("ball_hit"));
                    player = new SoundPlayer("resources/Ball-hits-player.wav");
                    break;
                case "defeat":                    
                    communicator.UDPSend(Translator.encodeExtra("victory"));
                    player = new SoundPlayer("resources/Defeat.wav");
                    break;
                case "victory":                    
                    communicator.UDPSend(Translator.encodeExtra("defeat"));
                    player = new SoundPlayer("resources/Victory.wav");
                    break;
            }
            player.Play();
            player.Dispose(); 
        }

        private void CheckCollision()
        {
            if (!collision)
            {
                if (new Rect(pongModel.BallPos.X, pongModel.BallPos.Y, PongModel.BallSize.X, PongModel.BallSize.Y).
                    IntersectsWith(new Rect(0, pongModel.PlayerX, PongModel.PlayerSizes.X, PongModel.PlayerSizes.Y + pongModel.ResizeX)))
                {
                    ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
                    play("ball_hit");
                    collision = true;
                }
                if (new Rect(pongModel.BallPos.X, pongModel.BallPos.Y, PongModel.BallSize.X, PongModel.BallSize.Y).
                    IntersectsWith(new Rect(PongModel.WINDOW_WIDTH - PongModel.PlayerSizes.X, pongModel.PlayerY, PongModel.PlayerSizes.X, PongModel.PlayerSizes.Y + pongModel.ResizeY)))
                {
                    ballSpeed = new Point(-ballSpeed.X, ballSpeed.Y);
                    play("ball_hit");
                    collision = true;
                }
            }
            else
                collision = false;
            
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
                sendBallPos();
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }

            if (pongModel.BallPos.Y > PongModel.WINDOW_HEIGHT - PongModel.BallSize.Y)
            {
                pongModel.BallPos = new Point(pongModel.BallPos.X, PongModel.WINDOW_HEIGHT - PongModel.BallSize.Y);
                sendBallPos();
                ballSpeed = new Point(ballSpeed.X, -ballSpeed.Y);
            }
        }

        private void ResetBall()
        {
            pongModel.BallPos = new Point((PongModel.WINDOW_WIDTH / 2) - (PongModel.BallSize.X / 2), (PongModel.WINDOW_HEIGHT / 2) - (PongModel.BallSize.Y / 2));
            sendBallPos();
            communicator.UDPSend(Translator.encodeScore(pongModel.PlayerXScore,pongModel.PlayerYScore));
            if ((pongModel.PlayerXScore + pongModel.PlayerYScore) > 8)
                timeout = 15;
            if ((pongModel.PlayerXScore + pongModel.PlayerYScore) > 10)
                timeout = 10;
            if ((pongModel.PlayerXScore + pongModel.PlayerYScore) > 15)
                timeout = 8;
            if ((pongModel.PlayerXScore + pongModel.PlayerYScore) > 18)
                timeout = 5;            
            InitializeDirectionIncrement();
        }
        
    }
}