using System.Numerics;
using System.Security.Cryptography;

namespace ElGamalCryptoTool
{
    public static class ElGamalMath
    {
        public static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            if (min >= max) 
                throw new ArgumentException("min must be less than max");
            BigInteger range = max - min;
            byte[] data = range.ToByteArray(isUnsigned: true, isBigEndian: false);
            BigInteger result;
            do
            {
                RandomNumberGenerator.Fill(data);
                data[data.Length - 1] &= 0x7F;
                result = new BigInteger(data);
            } while (result >= range);
            return result + min;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            if (a < 0)
                a = (a % n) + n;

            while (a > 0)
            {
                BigInteger q = n / a;
                (n, a) = (a, n % a);
                (v, d) = (d, v - q * d);
            }
            if (n > 1)
                throw new ArgumentException("Inverse does not exist.");
            return v < 0 ? v + i : v;
        }

        public static bool IsProbablePrime(this BigInteger source, int k = 10)
        {
            if (source < 2)
                return false;
            if (source == 2 || source == 3)
                return true;
            if (source % 2 == 0)
                return false;

            int[] smallPrimes = { 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53 };
            foreach(int prime in smallPrimes)
            {
                if (source % prime == 0)
                    return source == prime;
            }

            BigInteger d = source - 1;
            int s = 0;
            while (d.IsEven)
            {
                d >>= 1;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = RandomBigInteger(2, source - 2);
                BigInteger x = BigInteger.ModPow(a, d, source);

                if (x.IsOne || x == source - 1)
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
