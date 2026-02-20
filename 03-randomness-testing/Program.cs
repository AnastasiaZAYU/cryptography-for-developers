using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Generator();
            bool prog = true;
            int r = 0;
            List<int> key = new List<int>();
            while (prog == true)
            {
                Console.WriteLine("Натиснiть “0” - щоб згенерувати ключ та перевiрити його на випадковiсть;" +
                    "\nнатиснiть “1” - щоб перевiрити на випадковiсть декiлька ключiв пiдряд;" +
                    "\nнатиснiсть будь-який iнший символ - щоб завершити роботу.");
                r = int.Parse(Console.ReadLine());
                switch (r)
                {
                    case 0:
                        key = g.Key_Generator();
                        if (g.Test(key))
                            Console.WriteLine("TRUE, отже, ключ \"{0}\" є достатньо випадковою послiдовнiстю.", g.getHex(key));
                        else
                            Console.WriteLine("FALSE, отже, ключ \"{0}\" не є достатньо випадковою послiдовнiстю.", g.getHex(key));
                        break;
                    case 1:
                        Console.Write("Введiть кiлькiсть iтерацiй: ");
                        int n = int.Parse(Console.ReadLine());
                        var flag = true;
                        for (int i = 0; i < n; i++)
                            flag &= g.Test(g.Key_Generator());
                        if (flag)
                            Console.WriteLine("TRUE, отже, даний генератор є достатньо випадковим.", g.getHex(key));
                        else
                            Console.WriteLine("FALSE, отже, даний генератор є недостатньо випадковим..", g.getHex(key));
                        break;
                    default:
                        prog = false;
                        break;
                }
            }
        }
    }
}
