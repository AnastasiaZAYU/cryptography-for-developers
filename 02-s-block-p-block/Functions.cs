using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes
{
    public class Functions
    {
        Random rnd = new Random();
        List<int> s0 = Enumerable.Range(0, 16).ToList();
        List<int> s1 = Enumerable.Range(0, 16).ToList();
        List<int> p = Enumerable.Range(0, 8).ToList();

        public Functions()
        {
            s0 = s0.OrderBy(x => rnd.Next()).ToList(); //створення таблиці підстановки для перших 4 бітів блоку
            s1 = s1.OrderBy(x => rnd.Next()).ToList(); //створення таблиці підстановки для останніх 4 бітів блоку
            p = p.OrderBy(x => rnd.Next()).ToList(); //створення таблиці перестановки для 8-бітного блоку
        }

        public List<int> Input(string mes) //функція, яка переводить HEX у DEC 
        {
            List<int> list = new List<int>();
            for (int i = 0; i < mes.Length; i++)
                list.Add(Convert.ToInt32(Convert.ToString(mes[i]), 16));
            return list;
        }

        public void Print(List<int> list) //функція, яка виводить HEX повідомлення 
        {
            for (int i = 0; i < list.Count; i++)
                Console.Write(Convert.ToString(list[i], 16));
            Console.WriteLine();
        }

        public string Output(List<int> list) //функція, яка повертає HEX повідомлення 
        {
            string mes = "";
            for (int i = 0; i < list.Count; i++)
                mes += Convert.ToString(list[i], 16);
            return mes;
        }

        public List<int> S_box(List<int> a) //функції для прямого перетворення за алгоритмом S-блоку
        {
            for (int i = 0; i < a.Count; i++) 
            {
                if (i % 2 == 0)                   
                    a[i] = s0[a[i]]; //заміна значення перших 4 бітів блоку на відповідну йому константу з таблиці підстановки s0
                else
                    a[i] = s1[a[i]]; //заміна значення останніх 4 бітів блоку на відповідну йому константу з таблиці підстановки s1
            }
            Print(a);
            return a;
        }

        public List<int> S_box_inv(List<int> a) //функції для зворотного перетворення за алгоритмом S-блоку
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (i % 2 == 0)
                    a[i] = s0.IndexOf(a[i]); //відновлення значення перших 4 бітів блоку за відповідною йому константою з таблиці підстановки s0
                else
                    a[i] = s1.IndexOf(a[i]); //відновлення значення останніх 4 бітів блоку за відповідною йому константою з таблиці підстановки s0
            }
            Print(a);
            return a;
        }

        public List<int> P_box(List<int> a) //функції для прямого перетворення за алгоритмом P-блоку
        {
            int n;
            string str, shuffled;
            if (a.Count % 2 != 0) //доповнення повідомлення 0 до бітової довжини кратної 8
                a.Insert(0, 0);
            for (int i = 0; i < a.Count; i+=2)
            {
                str = Convert.ToString(a[i] * 16 + a[i + 1], 2).PadLeft(8, '0'); //вилучення блоку довжини 8 бітів
                shuffled = "";
                for (int j = 0; j < str.Length; j++)
                    shuffled += str[p[j]]; //перестановка бітів блоку за правилом з таблиці p
                n = Convert.ToInt32(shuffled, 2); //переведення числа з BIN у DEC
                a[i] = n / 16;
                a[i + 1] = n % 16;
            }
            Print(a);
            return a;
        }

        public List<int> P_box_inv(List<int> a) //функції для зворотного перетворення за алгоритмом P-блоку
        {
            int n;
            string str, shuffled;
            if (a.Count % 2 != 0) //доповнення повідомлення 0 до бітової довжини кратної 8
                a.Insert(0, 0);
            for (int i = 0; i < a.Count; i += 2)
            {
                str = Convert.ToString(a[i] * 16 + a[i + 1], 2).PadLeft(8, '0'); //вилучення блоку довжини 8 бітів
                shuffled = "";
                for (int j = 0; j < str.Length; j++)
                    shuffled += str[p.IndexOf(j)]; //обернена перестановка бітів блоку за правилом з таблиці p
                n = Convert.ToInt32(shuffled, 2); //переведення числа з BIN у DEC
                a[i] = n / 16;
                a[i + 1] = n % 16;
            }
            Print(a);
            return a;
        }
    }
}
