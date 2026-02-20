using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigNumbers
{
    public class BigNumber
    {
        int w = 16;
        int bt = Convert.ToInt32(Math.Pow(2, 16));

        public BigNumber()
        {}

        //Реалізація власного типу даних великого числа з методами setHex і getHex

        public string getHex(List<int> array)
        {
            string s = "";
            string temp;
            for (int i = 0; i < array.Count; i++)
            {
                temp = Convert.ToString(array[i], 16);
                while (temp.Length < 4) temp = "0" + temp;
                s = temp + s;
            }
            if (s[0] == '0') s = s.TrimStart('0');
            if (String.IsNullOrEmpty(s)) s = "0";
            return s;
        }

        public List<int> setHex(string s)
        {
            string temp;
            while (s.Length % 4 != 0) s = "0" + s;
            List<int> array = new List<int>();
            for (int i = 0; i < s.Length / 4; i++)
            {
                temp = "";
                for (int j = 0; j < 4; j++)
                    temp = temp + s[i * 4 + j];
                temp = Convert.ToString(Convert.ToInt32(temp, 16), 10);
                array.Add(Convert.ToInt32(temp));
            }
            array.Reverse();
            return array;
        }

        //Реалізація побітових операцій для власного типу даних

        public List<int> INV(List<int> array)
        {
            string bin;
            for (int i = 0; i < array.Count; i++)
            {
                bin = Convert.ToString(array[i], 2);
                if (bin.Length < 16 & i != array.Count - 1)
                    bin = bin.PadLeft(16, '0');
                bin = bin.Replace("1", "2");
                bin = bin.Replace("0", "1");
                bin = bin.Replace("2", "0");
                array[i] = Convert.ToInt32(bin, 2);
            }
            return array;
        }

        public List<int> XOR(List<int> list1, List<int> list2)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            List<int> c = new List<int>();
            if (list1.Count < list2.Count)
            {
                b = list1.ToList();
                a = list2.ToList();
            }
            else
            {
                a = list1.ToList();
                b = list2.ToList();
            }
            string bin_a, bin_b, bin_c;
            for (int i = 0; i < b.Count; i++)
            {
                bin_c = "";
                bin_a = Convert.ToString(a[i], 2);
                bin_b = Convert.ToString(b[i], 2);
                if (bin_a.Length < bin_b.Length)
                    bin_a = bin_a.PadLeft(bin_b.Length, '0');
                if (bin_a.Length > bin_b.Length)
                    bin_b = bin_b.PadLeft(bin_a.Length, '0');
                for (int j = 0; j < bin_a.Length; j++)
                {
                    if (bin_a[j] == bin_b[j])
                        bin_c += "0";
                    else bin_c += "1";
                }
                c.Add(Convert.ToInt32(bin_c, 2));
            }
            return c;
        }

        public List<int> OR(List<int> list1, List<int> list2)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            List<int> c = new List<int>();
            if (list1.Count < list2.Count)
            {
                b = list1.ToList();
                a = list2.ToList();
            }
            else
            {
                a = list1.ToList();
                b = list2.ToList();
            }
            string bin_a, bin_b, bin_c;
            for (int i = 0; i < b.Count; i++)
            {
                bin_c = "";
                bin_a = Convert.ToString(a[i], 2);
                bin_b = Convert.ToString(b[i], 2);
                if (bin_a.Length < bin_b.Length)
                    bin_a = bin_a.PadLeft(bin_b.Length, '0');
                if (bin_a.Length > bin_b.Length)
                    bin_b = bin_b.PadLeft(bin_a.Length, '0');
                for (int j = 0; j < bin_a.Length; j++)
                {
                    if (bin_a[j] == '0' & bin_b[j] == '0')
                        bin_c += "0";
                    else bin_c += "1";
                }
                c.Add(Convert.ToInt32(bin_c, 2));
            }
            return c;
        }

        public List<int> AND(List<int> list1, List<int> list2)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            List<int> c = new List<int>();
            if (list1.Count < list2.Count)
            {
                b = list1.ToList();
                a = list2.ToList();
            }
            else
            {
                a = list1.ToList();
                b = list2.ToList();
            }
            string bin_a, bin_b, bin_c;
            for (int i = 0; i < b.Count; i++)
            {
                bin_c = "";
                bin_a = Convert.ToString(a[i], 2);
                bin_b = Convert.ToString(b[i], 2);
                if (bin_a.Length < bin_b.Length)
                    bin_a = bin_a.PadLeft(bin_b.Length, '0');
                if (bin_a.Length > bin_b.Length)
                    bin_b = bin_b.PadLeft(bin_a.Length, '0');
                for (int j = 0; j < bin_a.Length; j++)
                {
                    if (bin_a[j] == '1' & bin_b[j] == '1')
                        bin_c += "1";
                    else bin_c += "0";
                }
                c.Add(Convert.ToInt32(bin_c, 2));
            }
            return c;
        }

        public List<int> shiftR(List<int> a, int count)
        {
            List<int> c = new List<int>();
            string s = "";
            for (int i = 0; i < a.Count; i++)
                s = Convert.ToString(a[i], 2) + s;
            s = s.Substring(s.Length - count) + s.Remove(s.Length - count);
            while (s.Length % 16 != 0)
                s = "0" + s;
            for (int i = 0; i < s.Length; i += 16)
            {
                c.Insert(0, Convert.ToInt32(s.Substring(0, 16), 2));
                s.Remove(0, 16);
            }
            return c;
        }

        public List<int> shiftL(List<int> a, int count)
        {
            List<int> c = new List<int>();
            string s = "";
            for (int i = 0; i < a.Count; i++)
                s = Convert.ToString(a[i], 2) + s;
            s = s.Remove(0, count) + s.Substring(0, count);
            while (s.Length % 16 != 0)
                s = "0" + s;
            for (int i = 0; i < s.Length; i += 16)
            {
                c.Insert(0, Convert.ToInt32(s.Substring(0, 16), 2));
                s.Remove(0, 16);
            }
            return c;
        }

        //Реалізація арифметичних операцій для власного типу даних

        public List<int> ADD(List<int> a1, List<int> b1)
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
                c.Add(temp & (bt - 1));
                carry = temp >> w;
            }
            for (; i < a.Count; i++)
            {
                temp = a[i] + carry;
                c.Add(temp & (bt - 1));
                carry = temp >> w;
            }
            if (carry != 0)
                c.Add(carry);
            return c;
        }

        public List<int> SUB(List<int> a, List<int> b)
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
                    c.Add(bt + temp);
                    borrow = 1;
                }
            }
            a = TrueSize(a);
            b = TrueSize(b);
            return TrueSize(c);
        }

        public List<int> MOD(List<int> c, List<int> n)
        {
            List<int> m = LongM(n);
            List<int> q = KillLastDigits(c, n.Count - 1);
            q = MUL(q, m);
            q = KillLastDigits(q, n.Count + 1);
            q = MUL(q, n);
            c = SUB(c, q);
            while (LongCmp(c, n) != -1)
                c = SUB(c, n);
            return c;
        }

        public List<int> MUL(List<int> p, List<int> q)
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
                c = ADD(c, temp);
            }
            p = TrueSize(p);
            q = TrueSize(q);
            return TrueSize(c);
        }

        public List<int> DIV(List<int> a, List<int> b)
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
                a = SUB(a, c);
                q[t - k]++;
            }
            return TrueSize(q);
        }

        public List<int> POWMOD(List<int> e, List<int> bin, List<int> n)
        {
            List<int> m = LongM(n);
            List<int> a = e.ToList();
            List<int> c = new List<int> { 1 };
            string b = LongBin(bin);
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == '1')
                {
                    c = MUL(c, a);
                    TrueSize(a);
                    TrueSize(c);
                    c = MOD(c, n);
                    TrueSize(c);
                }
                a = MUL(a, a);
                a = MOD(a, n);
                TrueSize(a);
            }
            return c;
        }

        //допоміжні функції

        private List<int> LongMulOneDigit(List<int> a, int b)
        {
            int size = a.Count;
            List<int> c = new List<int>();
            uint temp = 0, carry = 0;
            uint a1, b1;
            for (int i = 0; i < size; i++)
            {
                a1 = Convert.ToUInt32(a[i]);
                b1 = Convert.ToUInt32(b);
                temp = a1 * b1 + carry;
                c.Add(Convert.ToInt32(temp & (bt - 1)));
                carry = temp >> w;
            }
            c.Add(Convert.ToInt32(carry));
            return TrueSize(c);
        }

        private string LongBin(List<int> list)
        {
            string s = getHex(list);
            string b = "";
            string temp = "";
            for (int i = 0; i < s.Length; i++)
            {
                temp = "" + s[i];
                temp = Convert.ToString(Convert.ToInt32(temp, 16), 2);
                while (temp.Length < 4) temp = "0" + temp;
                b = b + temp;
            }
            b = new string(b.Reverse().ToArray());
            return b;
        }

        private int LongCmp(List<int> a, List<int> b)
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

        private List<int> LongM(List<int> n)
        {
            int k = n.Count;
            List<int> temp = new List<int> { 1 };
            List<int> m = LongShiftDigitsToHight(temp, 2 * k);
            m = DIV(m, n);
            return m;
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

        private List<int> TrueSize(List<int> c)
        {
            c.Reverse();
            while (c.Count != 1 && c[0] == 0)
                c.RemoveAt(0);
            c.Reverse();
            return c;
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

    }
}
