using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabPong
{
    class Translator
    {
        public String encodePlayerPosition(String playerPosition,String playerID)
        {
            //First one defines what kind of encoding this is, rest is only data
            return "1"+"|"+playerPosition+"|"+playerID;
        }

        public String encodeBallPosition(String ballPosition)
        {
            return "2|"+ballPosition;
        }

        public String encodeScore(String scorePlayer1, String scorePlayer2)
        {
            return "3|"+scorePlayer1+"|"+scorePlayer2;
        }

        public String encodeGameBegin()
        {
            return "4";
        }

        public String encodeGameEnd()
        {
            return "5";
        }

        public void decode(String message)
        {
            String [] commands = message.Split('|');
            switch (commands[0])
            {
                case "1":
                    String playerPosition = commands[1];
                    String playerID = commands[2];
                    //Call method to update the position of the ball
                    break;
                case "2":
                    String ballPosition = commands[1];
                    //Call method to update the position of the ball
                    break;
                case "3":
                    String scorePlayer1 = commands[1];
                    String scorePlayer2 = commands[2];
                    //Call method to update score
                    break;
                case "4":
                    //Call method for starting game
                    break;
                case "5":
                    //Call method for ending game
                    break;
                default:
                    //ERROR
                    break;
            }
        }
    }
}
