using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LabPong
{
    class Translator
    {
        public String encodePlayerPosition(double playerPosition)
        {
            //First one defines what kind of encoding this is, rest is only data
            return "1" + "|" + playerPosition;
        }

        public String encodeBallPosition(Point ballPosition)
        {
            return "2|" + ballPosition;
        }

        public String encodeScore(int scorePlayer1, int scorePlayer2)
        {
            return "3|" + scorePlayer1 + "|" + scorePlayer2;
        }

        public String encodeGameBegin()
        {
            return "4";
        }

        public String encodeGameEnd()
        {
            return "5";
        }

        public String encodeExtra(String extra)
        {
            return "6|" + extra;
        }

        public void decode(String message)
        {
            String[] commands = message.Split('|');
            switch (commands[0])
            {
                case "1":
                    double playerPosition = Double.Parse(commands[1]);
                    PongModel.pongModel.PlayerY = playerPosition;
                    //Call method to update the position of the ball
                    break;
                case "2":
                    Point ballPosition = Point.Parse(commands[1]);
                    //Call method to update the position of the ball
                    break;
                case "3":
                    int scorePlayer1 = Int32.Parse(commands[1]);
                    int scorePlayer2 = Int32.Parse(commands[2]);
                    //Call method to update score
                    break;
                case "4":
                    //Call method for starting game
                    break;
                case "5":
                    //Call method for ending game
                    break;
                case "6":
                    String ext = commands[1];

                    break;
                default:
                    //ERROR
                    break;
            }
        }
    }
}
