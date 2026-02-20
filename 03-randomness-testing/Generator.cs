using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key_Testing
{
    class Generator
    {
        LongOperation op = new LongOperation();
        Tests t = new Tests();

        public bool Test(List<int> key)
        {
            var flag = true;
            flag &= t.Monobit_Test(key);
            flag &= t.Series_Max_Length_Test(key);
            flag &= t.Pokker_Test(key);
            flag &= t.Series_Length_Test(key);
            return flag;
        }

        public List<int> Key_Generator()
        {
            return BBS(2500);
        }

        private List<int> BBS(int size)
        {
            Random R = new Random();
            List<int> p = op.Input("D5BBB96D30086EC484EBA3D7F9CAEB07");
            List<int> q = op.Input("425D2B9BFDB25B9CF6C416CC6E37B59C1F");
            List<int> n = op.LongMul(p, q);
            List<int> m = op.LongM(n);
            List<int> x = new List<int>();
            List<int> r = op.Rand(R.Next(2, p.Count / 2));
            r.Add(1);
            for (int i = 0; i < size; i++)
            {
                r = op.LongMod(op.LongMul(r, r), n, m);
                x.Add(r[0]);
                if (i == size - 1 && x.Last() == 0)
                    i -= 1;
            }
            if (x.Count != size)
                x.RemoveAt(size);
            return x;
        }

        public string getHex(List<int> key)
        {
            return op.Output(key);
        }
    }
}
