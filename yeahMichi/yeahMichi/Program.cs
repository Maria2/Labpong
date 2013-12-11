using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yeahMichi.foo
{
    enum NotificationType
    {
        alert=1,
        info=2
    }
    struct Point2
    {
        private int _x;
        public int X {
            get { return _x; }
            set
            {
                _x = value;
            }
        }
    }
    class Program
    {
        delegate void del(bool mi);

        static event del delEvent;
        
        static void Main(string[] args)
        {
      //      del marm = delegate(bool foz) { Console.WriteLine(foz); };
            del marm = suppe => Console.WriteLine(suppe);
            del marm2 = suppe => Console.WriteLine(suppe+" ahahah");
            del marm3 = suppe => Console.WriteLine(suppe + " blub");
          // marm(false);
            delEvent += marm;
            delEvent += marm2;
            delEvent += marm3;
            delEvent(false);
            Console.ReadKey();
            delEvent -= marm;
            delEvent(false);
            Console.ReadKey();

            var brzl = new Bambus();
            foreach(var count in Enumerable.Range(0,11))
            brzl.Wachse();

            

            Point2 myPoint = new Point2();
            myPoint.X = 3;
            var foo = NotificationType.alert;
            Console.WriteLine(myPoint.X);

            int x = 3;
            float y = 2f;

            var nichtVorhandenerDatenTyp = 4;
            var meineListe = new List<int>();
            var meinDictionary = new string[3];

            meinDictionary[(int)NotificationType.alert]="Maria";
            meinDictionary[(int)NotificationType.info] = "Michi";
            meinDictionary[(int)NotificationType.info] = "Maria";

            meineListe.Add(3);
            meineListe.Add(3);

            var meinArray = new string[5];
            //meinArray[2];

            var str1 = "maria";
            var str2 = str1.Substring(1, 3); // str2 = ari

            //Console.WriteLine(x);
            mehtodePrivate(ref x);
            //Console.WriteLine(x);

            var xx = 5;
            Console.WriteLine(int.TryParse("1", out xx));
            Console.WriteLine(xx);
            Console.WriteLine(int.TryParse("a", out xx));
            Console.WriteLine(xx);
            Console.ReadKey(true);
        }



        private static void mehtodePrivate(ref int x)
        {
            x += 2;
            //Console.WriteLine(x);
        }

    }

}
