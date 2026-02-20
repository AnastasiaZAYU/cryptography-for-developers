using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Testing
{
    class Tests
    {
        public bool Monobit_Test(List<int> key)
        {
            int count = 0;
            string s = "";
            for (int i = 0; i < key.Count; i++)
            {
                s = Convert.ToString(key[i], 2).PadLeft(8, '0');
                count += s.Count(c => c == '1');
            }
            if (count < 9654 || count > 10346)
                return false;
            return true;
        }

        public bool Series_Max_Length_Test(List<int> key)
        {
            int count = 1, max = 0;
            string s = "";
            char bit = '2';
            for (int i = 0; i < key.Count; i++)
            {
                s = bit + Convert.ToString(key[i], 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    if (s[j] == s[j + 1])
                        count++;
                    else
                    {
                        max = Math.Max(max, count);
                        count = 1;
                    }
                }
                bit = s.Last();
            }
            if (max > 36)
                return false;
            else return true;
        }

        public bool Pokker_Test(List<int> key)
        {
            int m = 4;
            int k = key.Count * 8 / m;
            int w = Convert.ToInt32(Math.Pow(2, m));
            List<int> n = new List<int>(new int[w]);
            for (int i = 0; i < key.Count; i++)
            {
                n[key[i] / w]++;
                n[key[i] % w]++;
            }
            double X3 = 0;
            for (int i = 0; i < w; i++)
                X3 += Math.Pow(n[i], 2);
            X3 = X3 * w / k - k;
            if (X3 < 1.03 || X3 > 57.4)
                return false;
            return true;
        }

        public bool Series_Length_Test(List<int> key)
        {
            if (!Intervals(Series_Length(key, '0')))
                return false;
            return Intervals(Series_Length(key, '1'));
        }

        private List<int> Series_Length(List<int> key, char w)
        {
            var count = new List<int>(new int[7]);
            string s = "";
            int n = 0;
            for (int i = 0; i < key.Count; i++)
            {
                s = Convert.ToString(key[i], 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    if (s[j] == w)
                        n++;
                    else
                    {
                        if (n > 6) n = 6;
                        count[n]++;
                        n = 0;
                    }
                }
            }
            return count;
        }

        private bool Intervals(List<int> count)
        {
            if (count[1] < 2267 || count[1] > 2733)
                return false;
            if (count[2] < 1079 || count[2] > 1421)
                return false;
            if (count[3] < 502 || count[3] > 748)
                return false;
            if (count[4] < 223 || count[4] > 402)
                return false;
            if (count[5] < 90 || count[5] > 223)
                return false;
            if (count[6] < 90 || count[6] > 223)
                return false;
            return true;
        }
    }
}
