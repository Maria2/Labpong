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
        delegate void VoidMethod();
        delegate void VoidMethod2(Boolean identifier);

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
                    PongModel.pongModel.PlayerYScore = scorePlayer1;
                    PongModel.pongModel.PlayerXScore = scorePlayer2;
                    break;
                //case "4":
                //    //Call method for starting game
                //    break;
                case "5":
                    StreamWriter file = new StreamWriter("resources/highscore.txt", true);
                    file.WriteLine(commands[1]);
                    file.Flush(); 
                    file.Close();
                    return false;
                case "6":
                    switch (commands[1])
                    {                        
                        case "white_screen": PongModel.pongModel.ChangeColor(); break;
                        case "freeze": new VoidMethod(PongModel.pongModel.Freeze).BeginInvoke(null,null); break;                            
                        case "invert": new VoidMethod(PongModel.pongModel.InvertMove).BeginInvoke(null,null); break;
                        case "resize": new VoidMethod2(PongModel.pongModel.ResizePlayer).BeginInvoke(false, null, null); break;
                        case "ball_hit": PongModel.PlayAudio("ball_hit"); break;
                        case "defeat": PongModel.PlayAudio("defeat"); break;
                        case "victory": PongModel.PlayAudio("victory"); break;
                        case "sizes": 
                            PongModel.WINDOW_HEIGHT_Y = double.Parse(commands[2]); 
                            PongModel.WINDOW_WIDTH_Y = double.Parse(commands[3]); 
                            break;
                    }   
                    break;
            }
            return true;
        }
    }
}
