using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Functions();
             List<int> mes = new List<int>(new int[2]);
             bool prog = true;
             int r = 0;
             while (prog == true)
             {
                 Console.WriteLine("Натиснiть “0” - щоб ввести повiдомлення в HEX;" +
                     "\nнатиснiть “1” - щоб застосувати пряме перетворення з S-блок до повiдомлення;" +
                     "\nнатиснiть “2” - щоб застосувати зворотнє перетворення з S-блок до повiдомлення;" +
                     "\nнатиснiть “3” - щоб застосувати пряме перетворення з P-блок до повiдомлення;" +
                     "\nнатиснiть “4” - щоб застосувати зворотнє перетворення з P-блок до повiдомлення;" +
                     "\nнатиснiсть будь-який iнший символ - щоб завершити роботу.");
                 r = int.Parse(Console.ReadLine());
                 switch (r)
                 {
                     case 0:
                         string s = Console.ReadLine();
                         mes = f.Input(s);
                         break;
                     case 1:
                         mes = f.S_box(mes);
                         break;
                     case 2:
                        mes = f.S_box_inv(mes);
                        break;
                     case 3:
                        mes = f.P_box(mes);
                        break;
                    case 4:
                        mes = f.P_box_inv(mes);
                        break;
                    default:
                         prog = false;
                         break;
                 }
             }
        }
    }
}
