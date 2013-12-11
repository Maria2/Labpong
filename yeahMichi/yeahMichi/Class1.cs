using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yeahMichi
{
    class Class1
    {
     /*   public Class1()
        {
            Console.WriteLine("Ich bin meine Parameter los!!");
        }*/

        public Class1(bool MichiIsAnnoying)
   //         : this()
        {
            if (MichiIsAnnoying)
                Console.WriteLine("Michi ist echt nervig und redet lauter schwachsinn");
            else
                Console.WriteLine("Michi ist echt noch viel nerviger");
        }

        public Class1(string meinWert, bool asd)
            : this(asd)
        {
            Console.WriteLine("Mein Wert "+meinWert);
        }
    }
}
