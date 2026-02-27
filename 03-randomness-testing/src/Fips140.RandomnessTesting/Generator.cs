using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace KeyRandomnessValidator
{
    class Generator
    {
        private static readonly BigInteger P = BigInteger.Parse("0D5BBB96D30086EC484EBA3D7F9CAEB07", System.Globalization.NumberStyles.HexNumber);
        private static readonly BigInteger Q = BigInteger.Parse("0425D2B9BFDB25B9CF6C416CC6E37B59C1F", System.Globalization.NumberStyles.HexNumber);
        private static readonly BigInteger N = P * Q;

        public byte[] GenerateKey(int byteSize = 2500)
        {
            BigInteger x = new BigInteger(Random.Shared.Next(1000, 9999));
            x = BigInteger.ModPow(x, 2, N);
            byte[] result = new byte[byteSize];

            for (int i = 0; i < byteSize; i++)
            {
                byte currentByte = 0;
                for (int bit = 0; bit < 8; bit++)
                {
                    x = BigInteger.ModPow(x, 2, N);
                    if (!x.IsEven)
                        currentByte |= (byte)(1 << (7 - bit));
                }
                result[i] = currentByte;
            }
            return result;
        }

        public string ToHexString(byte[] data) =>
            BitConverter.ToString(data).Replace("-", "");
    }
}
