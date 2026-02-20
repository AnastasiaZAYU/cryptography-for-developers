using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Testing
{
    class LongOperation
    {
        public void Print(List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
                Console.Write(l[i] + " ");
            Console.WriteLine();
        }

        public List<int> Input(string a)
        {
            List<int> l = new List<int>();
            if (a.Length % 2 != 0)
                a = "0" + a;
            for (int i = a.Length - 1; i > 0; i -= 2)
                l.Add(Convert.ToInt32(Convert.ToString(a[i - 1]), 16) * 16 + Convert.ToInt32(Convert.ToString(a[i]), 16));
            return l;
        }

        public string Output(List<int> a)
        {
            string s = "";
            for (int i = 0; i < a.Count; i++)
            {
                string h = Convert.ToString(a[i], 16);
                if (h.Length != 2) h = "0" + h;
                s = h + s;
            }
            s = s.TrimStart('0');
            return s.ToUpper();
        }

        public List<int> LongMul(List<int> p, List<int> q)
        {
            p = LongSameSize(p, q.Count);
            q = LongSameSize(q, p.Count);
            List<int> c = new List<int>();
            List<int> temp = new List<int>();
            for (int i = 0; i < p.Count; i++)
            {
                temp = LongMulOneDigit(p, q[i]);
                temp = LongShiftDigitsToHight(temp, i);
                c = LongSameSize(c, temp.Count);
                temp = LongSameSize(temp, c.Count);
                c = LongAdd(c, temp);
            }
            p = TrueSize(p);
            q = TrueSize(q);
            return TrueSize(c);
        }

        public List<int> LongAdd(List<int> a1, List<int> b1)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            if (a1.Count < b1.Count)
            {
                a = b1.ToList();
                b = a1.ToList();
            }
            else
            {
                b = b1.ToList();
                a = a1.ToList();
            }
            List<int> c = new List<int>();
            int carry = 0, temp = 0;
            int i = 0;
            for (i = 0; i < b.Count; i++)
            {
                temp = a[i] + b[i] + carry;
                c.Add(temp & 255);
                carry = temp >> 8;
            }
            for (; i < a.Count; i++)
            {
                temp = a[i] + carry;
                c.Add(temp & 255);
                carry = temp >> 8;
            }
            if (carry != 0)
                c.Add(carry);
            return c;
        }

        private List<int> LongMulOneDigit(List<int> a, int b)
        {
            int size = a.Count;
            List<int> c = new List<int>();
            int temp = 0, carry = 0;
            for (int i = 0; i < size; i++)
            {
                temp = a[i] * b + carry;
                c.Add(temp & 255);
                carry = temp >> 8;
            }
            c.Add(carry);
            return TrueSize(c);
        }

        public List<int> LongMod(List<int> c, List<int> n, List<int> m)
        {
            List<int> q = KillLastDigits(c, n.Count - 1);
            q = LongMul(q, m);
            q = KillLastDigits(q, n.Count + 1);
            q = LongMul(q, n);
            c = LongSub(c, q);
            while (LongCmp(c, n) != -1)
                c = LongSub(c, n);
            return c;
        }

        private List<int> KillLastDigits(List<int> a, int i)
        {
            List<int> p = a.ToList();
            if (p.Count > i)
                p.RemoveRange(0, i);
            else
            {
                p.Clear();
                p.Add(0);
            }
            return p;
        }

        public List<int> LongSub(List<int> a, List<int> b)
        {
            a = LongSameSize(a, b.Count);
            b = LongSameSize(b, a.Count);
            int size = a.Count;
            List<int> c = new List<int>();
            int borrow = 0, temp = 0;
            for (int i = 0; i < size; i++)
            {
                temp = a[i] - b[i] - borrow;
                if (temp >= 0)
                {
                    c.Add(temp);
                    borrow = 0;
                }
                else
                {
                    c.Add(256 + temp);
                    borrow = 1;
                }
            }
            a = TrueSize(a);
            b = TrueSize(b);
            return TrueSize(c);
        }

        public List<int> LongDivMod(List<int> a, List<int> b)
        {
            List<int> c = new List<int>();
            int t, k = b.Count;
            List<int> q = new List<int>(new int[a.Count]);
            while (LongCmp(a, b) != -1)
            {
                t = a.Count;
                c = LongShiftDigitsToHight(b, t - k);
                if (LongCmp(a, c) == -1)
                {
                    t--;
                    c = LongShiftDigitsToHight(b, t - k);
                }
                a = LongSub(a, c);
                q[t - k]++;
            }
            return TrueSize(q);
        }

        private List<int> TrueSize(List<int> c)
        {
            c.Reverse();
            while (c.Count != 1 && c[0] == 0)
                c.RemoveAt(0);
            c.Reverse();
            return c;
        }

        public int LongCmp(List<int> a, List<int> b)
        {
            if (a.Count > b.Count)
                return 1;
            else if (b.Count > a.Count)
                return -1;
            int i = a.Count - 1;
            while (a[i] == b[i])
            {
                if (i == 0)
                    return 0;
                i -= 1;
            }
            if (a[i] > b[i])
                return 1;
            else
                return -1;
        }

        public List<int> LongM(List<int> n)
        {
            int k = n.Count;
            List<int> temp = new List<int> { 1 };
            List<int> m = LongShiftDigitsToHight(temp, 2 * k);
            m = LongDivMod(m, n);
            return m;
        }

        private List<int> LongShiftDigitsToHight(List<int> b, int i)
        {
            List<int> c = b.ToList();
            for (int j = 0; j < i; j++)
                c.Insert(0, 0);
            return c;
        }

        private List<int> LongSameSize(List<int> a, int size)
        {
            for (int i = a.Count; i < size; i++)
                a.Add(0);
            return a;
        }

        public List<int> Rand(double n)
        {
            Random R = new Random();
            List<int> l = new List<int>();
            for (int i = 0; i < n; i++)
                l.Add(R.Next(0, 256));
            return l;
        }
    }
}
