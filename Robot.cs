using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace RobotTask
{
    // Impication of rope data struct
    public class SimpleRope: ICloneable
    {
        public string str;
        public SimpleRope left;
        public SimpleRope right;
        public int height;
        public SimpleRope(string str, SimpleRope left, SimpleRope right)
        {
            this.str = str;
            this.left = left;
            this.right = right;
            this.height = 1;
    }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public static SimpleRope Concat(SimpleRope left, SimpleRope right)
        {
            SimpleRope lr = new SimpleRope(null, left, right);
            return balance(lr);
        }
        public static int getHeight(SimpleRope node)
        {
            if (node != null)
            {
                return node.height;
            }
            else
            {
                return 0;
            }
        }
        public static int balanceFactor(SimpleRope node)
        {
            if (node == null)
            {
                return 0;
            }
            else
            {
                return getHeight(node.right) - getHeight(node.left);
            }
        }
        public void updateHeight()
        {
            var hl = getHeight(this.left);
            var hr = getHeight(this.right);
            if (hl > hr)
            {
                this.height = hl + 1;
            }
            else
            {
                this.height = hr + 1;
            }
        }
        private static SimpleRope rightRotation(SimpleRope p)
        {
            SimpleRope pcopy = p;
            SimpleRope x = pcopy.left;
            SimpleRope T2 = x.right;

            x.right = pcopy;
            pcopy.left = T2;

            pcopy.updateHeight();
            x.updateHeight();
            return x;
        }
        private static SimpleRope leftRotation(SimpleRope q)
        {
            SimpleRope qcopy = q;
            SimpleRope y = qcopy.right;
            SimpleRope T2 = y.left;

            y.left = q;
            qcopy.right = T2;

            qcopy.updateHeight();
            y.updateHeight();
            return y;
        }
        public static SimpleRope balance(SimpleRope p)
        {
            p.updateHeight();
            Console.WriteLine(balanceFactor(p));
            if (balanceFactor(p) == 2)
            {
                Console.WriteLine("rRot");
                Console.WriteLine(balanceFactor(p.right));
                if (balanceFactor(p.right) < 0)
                {
                    Console.WriteLine("brRot");
                    p.right = rightRotation(p.right);
                }
                return leftRotation(p);
            }
            if (balanceFactor(p) == -2)
            {
                Console.WriteLine("lRot");
                Console.WriteLine(balanceFactor(p.left));
                if (balanceFactor(p.left) > 0)
                {
                    Console.WriteLine("blRot");
                    p.left = leftRotation(p.left);
                }
                return rightRotation(p);
            }
            else
            {
                return p;
            }
        }
        public override string ToString()
        {
            string ans = string.Empty;
            if (this.str != null)
            {
                return this.str;
            }
            else
            {
                ans += this.left.ToString() + this.right.ToString();
            }
            return ans;
        }
    }
    // Impication of stack data struct
    public class Stack
    {
        private static SimpleRope[] array;
        private static int positionTop;
        public Stack(int length)
        {
            array = new SimpleRope[length];
            positionTop = -1;
        }

        public static void Push(SimpleRope element)
        {
            positionTop++;
            array[positionTop] = element;
        }
        public static SimpleRope Pop()
        {
            positionTop--;
            return array[positionTop + 1];
        }
        public static SimpleRope Pop(int numbers)
        {
            for (int i = 0; i < numbers - 1; i++)
            {
                Pop();
            }
            return Pop();
        }
        public static void Edit(SimpleRope element, int depth)
        {
            array[positionTop + 1 - depth] = element;
        }
        public static SimpleRope Get()
        {
            return array[positionTop];
        }
        public static SimpleRope Get(int n)
        {
            return array[positionTop + 1 - n];
        }
        public static void Copy(int n)
        {
            Push(Get(n));
        }
        public static void Swap(int a, int b)
        {
            var temp = Get(a);
            Edit(Get(b), a);
            Edit(temp, b);
        }
    }
    // Here is output, input, command list, and etc
    public class MyField
    {
        public static List<string> Commands = new List<string> { };
        public static List<string> Output = new List<string> { };
        public static Dictionary<string, int> LabelDict = new Dictionary<string, int>();
        public static List<string> Input = new List<string> { };
        public static int CurrentReadPos = 0;
        public static int CurrentExecuteLine = 0;
        public static void Clear()
        {
            Commands = new List<string> { };
            Output = new List<string> { };
            LabelDict = new Dictionary<string, int>();
            Input = new List<string> { };
            CurrentReadPos = 0;
            CurrentExecuteLine = 0;
        }
    }
    // Executor for one instruction
    public static class CommandExecutor
    {
        private static string RefineString(string str)
        {
            var oldstr = str.Trim('\'');
            string pattern = "\''";
            string replacement = "'";
            Regex rgx = new Regex(pattern);
            string newstr = rgx.Replace(oldstr, replacement);
            return newstr;
        }
        private static int PUSH(string str, bool isConsole)
        {
            string pattern = "\'.*'";
            Regex rgx = new Regex(pattern);
            string newstr = rgx.Match(str).Value;

            var strRp = new SimpleRope(RefineString(newstr), null, null);

            Stack.Push(strRp);
            return MyField.CurrentExecuteLine + 1;
        }
        private static int POP(string str, bool isConsole)
        {
            Stack.Pop();
            return MyField.CurrentExecuteLine + 1;
        }
        private static int READ(string str, bool isConsole)
        {
            if (isConsole)
            {
                var strRp = new SimpleRope(Console.ReadLine(), null, null);
                Stack.Push(strRp);
            }
            else
            {
                var strRp = new SimpleRope(MyField.Input[MyField.CurrentReadPos], null, null);
                Stack.Push(strRp);
            }
            MyField.CurrentReadPos++;
            return MyField.CurrentExecuteLine + 1;
        }
        private static int WRITE(string str, bool isConsole)
        {
            if (isConsole)
            {
                Console.WriteLine(Stack.Get());
            }
            else
            {
                var strRp = Stack.Get().ToString();
                MyField.Output.Add(strRp);
            }
            return MyField.CurrentExecuteLine + 1;
        }
        private static int SWAP(string str, bool isConsole)
        {
            var stringList = str.Split();
            var kargs = stringList.Skip<string>(1);
            var args = kargs.ToArray<string>();

            int a = int.Parse(args[0]);
            int b = int.Parse(args[1]);
            Stack.Swap(a, b);
            return MyField.CurrentExecuteLine + 1;
        }
        private static int COPY(string str, bool isConsole)
        {
            var stringList = str.Split();
            var kargs = stringList.Skip<string>(1);
            var args = kargs.ToArray<string>();

            int n = int.Parse(args[0]);
            Stack.Copy(n);
            return MyField.CurrentExecuteLine + 1;
        }
        private static int SKIP(string str, bool isConsole)
        {
            return MyField.CurrentExecuteLine + 1;
        }
        private static int JMP(string str, bool isConsole)
        {
            var stringList = str.Split();
            var kargs = stringList.Skip<string>(1);
            var args = kargs.ToArray<string>();

            if (!args.Any())
            {
                var label = Stack.Pop().ToString();
                var idx = MyField.LabelDict[label];
                return idx;
            }
            else
            {
                var idx = MyField.LabelDict[args[0]];
                return idx;
            }
        }
        private static int CONCAT(string str, bool isConsole)
        {
            var a = Stack.Pop();
            var b = Stack.Pop();
            Stack.Push(SimpleRope.Concat(a, b));
            return MyField.CurrentExecuteLine + 1;
        }
        private static int REPLACEONE(string str, bool isConsole)
        {
            SimpleRope a = Stack.Get(1);
            SimpleRope b = Stack.Get(2);
            SimpleRope c = Stack.Get(3);
            SimpleRope ret = Stack.Get(4);

            Regex regex = new Regex(b.ToString());
            bool isPossible = regex.Match(a.ToString()).Value.Any();
            if (!isPossible)
            {
                Stack.Pop(4);
                Stack.Push(a);
                var idx = MyField.LabelDict[ret.ToString()];
                return idx;
            }
            else
            {
                string newstr = regex.Replace(a.ToString(), c.ToString(), 1);
                Stack.Pop(4);
                var strRp = new SimpleRope(newstr, null, null);
                Stack.Push(strRp);
                return MyField.CurrentExecuteLine + 1;
            }
        }
        private static Dictionary<string, Func<string, bool, int>> functions = new Dictionary<string, Func<string, bool, int>>
        {
            {"PUSH", PUSH },
            {"POP", POP },
            {"READ", READ },
            {"WRITE", WRITE },
            {"SWAP", SWAP },
            {"COPY", COPY },
            {"LABEL", SKIP },
            {"JMP", JMP },
            {"CONCAT", CONCAT },
            {"REPLACEONE", REPLACEONE },
            {"", SKIP }
        };
        public static int Run(string str, bool isConsole)
        {
            var stringList = str.Split();
            var func = stringList[0];
            return functions[func](str, isConsole);
        }
    }
    // Initialize stack and run commands
    public class CodeCompiler
    {
        public Stack Stack;
        public bool IsConsole;
        public CodeCompiler(List<string> commands, List<string> input, bool isConsole)
        {
            if (!isConsole) { MyField.Input = input; }
            MyField.Commands = commands;
            int stackSize = commands.FindAll(MyPredicate).Count();
            Stack = new Stack(stackSize);
            this.IsConsole = isConsole;
        }
        public static bool MyPredicate(string str)
        {
            var stringList = str.Split();
            var func = stringList[0];
            if (func == "PUSH" || func == "READ" || func == "COPY")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Initialize()
        {
            List<string> methodCommands = new List<string>();
            string label = string.Empty;
            int commandsNumber = MyField.Commands.Count;
            for (int i = 0; i < MyField.Commands.Count; i++)
            {
                var command = MyField.Commands[i];
                if (command.Split()[0] == "LABEL")
                {
                    MyField.LabelDict.Add(command.Split()[1], i);
                }
            }
        }
        public void Execute()
        {
            while (MyField.CurrentExecuteLine < MyField.Commands.Count)
            {
                var command = MyField.Commands[MyField.CurrentExecuteLine];
                MyField.CurrentExecuteLine = CommandExecutor.Run(command, IsConsole);
            }
        }
    }
    public class Robot : IRobot
    {
        public List<string> Evaluate(List<string> commands, IEnumerable<string> input)
        {
            List<string> newinput = new List<string> { };
            foreach (string inputline in input)
            {
                newinput.Add(inputline);
            }
            CodeCompiler executor = new CodeCompiler(commands, newinput, false);
            executor.Initialize();
            executor.Execute();
            var output = MyField.Output;
            MyField.Clear();
            return output;
        }

        public void Evaluate(List<string> commands)
        {
            var input = new List<string> { };
            CodeCompiler executor = new CodeCompiler(commands, input, true);
            executor.Initialize();
            executor.Execute();
            MyField.Clear();
        }
    }
}