using Leap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabPong
{
    class CustomController : Listener
    {        

        private String handString;
        private Vector palmPosition;
            
       // Controller.addListener(listener);

        public override void OnFrame(Controller arg0)
        {
            Frame frame = arg0.Frame();
            if (!frame.Hands.IsEmpty)
            {             
                Hand hand = frame.Hands[0];
                Console.WriteLine(hand.PalmPosition.ToString());
                palmPosition = hand.PalmPosition;
                handString = hand.PalmPosition.ToString();
            }
        }
    }
}
