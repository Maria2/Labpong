using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LabPong
{
    class Translator
    {
        public static String encodePlayerPosition(double playerPosition)
        {
            //First one defines what kind of encoding this is, rest is only data
            return "1" + "|" + playerPosition;
        }

        public static String encodeBallPosition(Point ballPosition)
        {
            return "2|" + ballPosition.X+ "|" +ballPosition.Y;
        }

        public static String encodeScore(int scorePlayer1, int scorePlayer2)
        {
            return "3|" + scorePlayer1 + "|" + scorePlayer2;
        }

        public static String encodeGameBegin()
        {
            return "4";
        }

        public static String encodeGameEnd(String gameResult)
        {
            return "5|"+gameResult;
        }

        public static String encodeExtra(String extra)
        {
            return "6|" + extra;
        }

        public bool decode(String message)
        {
            String[] commands = message.Split('|');
            switch (commands[0])
            {
                case "1":
                    double playerPosition = Double.Parse(commands[1]);
                    PongModel.pongModel.PlayerY = playerPosition;
                    break;
                case "2":
                    Point ballPosition = new Point(Double.Parse(commands[1]), Double.Parse(commands[2]));
                    PongModel.pongModel.BallPos = ballPosition;
                    break;
                case "3":
                    int scorePlayer1 = Int32.Parse(commands[1]);
                    int scorePlayer2 = Int32.Parse(commands[2]);
                    PongModel.pongModel.PlayerXScore = scorePlayer1;
                    PongModel.pongModel.PlayerYScore = scorePlayer2;
                    break;
                //case "4":
                //    //Call method for starting game
                //    break;
                case "5":
                    new StreamWriter("resources/highscore.txt", true).WriteLine(commands[1]);
                    return false;
                case "6":
                    if(commands[1].Equals("WINDOW_HEIGHT"))
                    {
                        PongModel.
                    }
                    String ext = commands[1];
                    break;                    
            }
            return true;
        }
    }
}
