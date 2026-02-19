using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace _SHA_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            var sha = new SHA_1();
            bool prog = true;
            int r = 0;
            byte[] hash;
            string s;
            long len;
            StreamReader sr;
            BinaryReader br;
            TimeSpan elapsedTime;
            Process currentProcess;
            long workingSet;
            long privateMemory;
            while (prog == true)
            {
                Console.WriteLine("Натиснiть “0” - щоб отримати геш-значення тексту, введеного з консолi;" +
                    "\nнатиснiть “1” - щоб отримати геш-значення тексту, зчитаного з файлу;" +
                    "\nнатиснiть “2” - щоб отримати геш-значення тексту, зчитаного з бiнарного файлу;" +
                    "\nнатиснiть “3” - щоб за допомогою бiблiотеки отримати геш-значення тексту, введеного з консолi;" +
                    "\nнатиснiть “4” - щоб за допомогою бiблiотеки отримати геш-значення тексту, зчитаного з файлу;" +
                    "\nнатиснiть “5” - щоб за допомогою бiблiотеки отримати геш-значення тексту, зчитаного з бiнарного файлу;" +
                    "\nнатиснiсть будь-який iнший символ - щоб завершити роботу.");
                r = int.Parse(Console.ReadLine());
                switch (r)
                {
                    case 0:
                        s = Console.ReadLine();
                        hash = sha.CalculateSHA1(s, true);
                        Console.WriteLine(sha.GetHex(hash));
                        break;
                    case 1:
                        sr = File.OpenText("text.txt");
                        //stopwatch.Start();
                        //currentProcess = Process.GetCurrentProcess();
                        hash = sha.CalculateSHA1(sr, true);
                        //stopwatch.Stop();
                        //workingSet = currentProcess.WorkingSet64;
                        //privateMemory = currentProcess.PrivateMemorySize64;
                        Console.WriteLine(sha.GetHex(hash));
                        //elapsedTime = stopwatch.Elapsed;
                        //Console.WriteLine("Час виконання функції: " + elapsedTime.ToString());
                        //Console.WriteLine("Обсяг використаної оперативної пам'ятi: " + workingSet + " байт");
                        //Console.WriteLine("Приватний обсяг пам'ятi: " + privateMemory + " байт");
                        sr.Close();
                        break;
                    case 2:
                        br = new BinaryReader(File.Open("bintext.bin", FileMode.Open));
                        len = new FileInfo("bintext.bin").Length;
                        hash = sha.CalculateSHA1(br, len, true);
                        Console.WriteLine(sha.GetHex(hash));
                        br.Close();
                        break;
                    case 3:
                        s = Console.ReadLine();
                        hash = sha.CalculateSHA1(s, false);
                        Console.WriteLine(sha.GetHex(hash));
                        break;
                    case 4:
                        sr = File.OpenText("text.txt");
                        //stopwatch.Start();
                        //currentProcess = Process.GetCurrentProcess();
                        hash = sha.CalculateSHA1(sr, false);
                        //stopwatch.Stop();
                        //workingSet = currentProcess.WorkingSet64;
                        //privateMemory = currentProcess.PrivateMemorySize64;
                        Console.WriteLine(sha.GetHex(hash));
                        //elapsedTime = stopwatch.Elapsed;
                        //Console.WriteLine("Час виконання функції: " + elapsedTime.ToString());
                        //Console.WriteLine("Обсяг використаної оперативної пам'ятi: " + workingSet + " байт");
                        //Console.WriteLine("Приватний обсяг пам'ятi: " + privateMemory + " байт");
                        sr.Close();
                        break;
                    case 5:
                        br = new BinaryReader(File.Open("bintext.bin", FileMode.Open));
                        len = new FileInfo("bintext.bin").Length;
                        hash = sha.CalculateSHA1(br, len, false);
                        Console.WriteLine(sha.GetHex(hash));
                        br.Close();
                        break;
                    default:
                        prog = false;
                        break;
                }
            }
            Console.ReadKey();
        }
    }
}
