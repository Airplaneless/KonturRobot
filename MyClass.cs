using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Program
{
    using RobotTask;
    class MyClass
    {
        public int num;
        public string name;
        public MyClass my;
        public MyClass(int num, string name, MyClass my)
        {
            this.num = num;
            this.name = name;
            this.my = my;
            Console.WriteLine("Construct!");
        }
        public static MyClass Chng(MyClass n)
        {
            MyClass m = n;
            m.name = "chng";
            return m;
        }
        public override string ToString()
        {
            return this.name + this.num.ToString();
        }
    }
    class Test
    {
   
        public static void Main()
        {
            /*
            SimpleRope a = new SimpleRope("00");
            SimpleRope b1 = new SimpleRope(":");
            SimpleRope b2 = new SimpleRope("15");
            SimpleRope b3 = new SimpleRope(" are");
            SimpleRope b4 = new SimpleRope(" you");
            SimpleRope b5 = new SimpleRope(" living");
            SimpleRope b6 = new SimpleRope(" - what");
            SimpleRope b7 = new SimpleRope(" about");
            SimpleRope b8 = new SimpleRope(" yOuR");
            SimpleRope b9 = new SimpleRope(" sexlife?");
            var c2 = SimpleRope.Concat(a, b1);
            var c3 = SimpleRope.Concat(c2, b2);
            var c4 = SimpleRope.Concat(c3, b3);
            var c5 = SimpleRope.Concat(c4, b4);
            var c6 = SimpleRope.Concat(c5, b5);
            var c7 = SimpleRope.Concat(c6, b6);
            var c8 = SimpleRope.Concat(c7, b7);
            var c9 = SimpleRope.Concat(c8, b8);
            var c10 = SimpleRope.Concat(c9, b9);
            Console.WriteLine(c3);
            */
            Robot robot = new Robot();
            List<string> commands = new List<string> { };
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\airplaneless\source\repos\somestuff\KonturRobot\ForOptimizations\Concat1000000");
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\airplaneless\source\repos\somestuff\KonturRobot\timer.txt");
            foreach (string line in lines)
            {
                commands.Add(line);
            }
            robot.Evaluate(commands);
            
            Console.ReadKey();
        }
    }
}
