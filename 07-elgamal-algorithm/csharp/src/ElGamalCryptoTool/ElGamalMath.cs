using System.Numerics;
using System.Security.Cryptography;

namespace ElGamalCryptoTool
{
    public static class ElGamalMath
    {
        public static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] data = max.ToByteArray();
            BigInteger result;
            do
            {
                RandomNumberGenerator.Fill(data);
                data[data.Length - 1] &= 0x7F;
                result = new BigInteger(data);
            } while (result < min || result >= max);
            return result;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0)
                v = (v + n) % n;
            return v;
        }

        public static bool IsProbablePrime(this BigInteger source, int k = 10)
        {
            if (source < 2)
                return false;
            if (source == 2 || source == 3)
                return true;
            if (source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = RandomBigInteger(2, source - 2);
                BigInteger x = BigInteger.ModPow(a, d, source);

                if (x == 1 || x == source - 1)
                    continue;

                bool composite = true;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == source - 1)
                    {
                        composite = false;
                        break;
                    }
                }
                if (composite)
                    return false;
            }
            return true;
        }
    }
}
