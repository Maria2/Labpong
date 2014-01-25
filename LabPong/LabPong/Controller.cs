using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabPong
{
    class Controller : Listener
    {
        Controller controller = new Controller();
        Listener listener = new Listener();

       // Controller.addListener(listener);

        Frame frame = controller.Frame();
    }
}
