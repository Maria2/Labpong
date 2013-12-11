using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yeahMichi
{
    abstract class Pflanze
    {
        public int hoehe { get; set; }
        public abstract void Wachse();
        public virtual void Ausreissen()// virtual = in subclasse überschreibbar
        {
            Console.WriteLine("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH Pflanze TOT");
        }
    }

    class Bambus : Pflanze
    {
        public override void Wachse()
        {
            Console.WriteLine("ich wachse um 10");
            hoehe += 10;
            if (hoehe > 100)
            {
                base.Ausreissen();
            }
        }
    }
    class Rose : Pflanze
    {

        public override void Wachse()
        {
            hoehe += 1;
        }
    }
}
