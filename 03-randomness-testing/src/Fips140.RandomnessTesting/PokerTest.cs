using System;
using System.Collections.Generic;
using System.Text;

namespace Fips140.RandomnessTesting
{
    public class PokerTest : IRandomnessTest
    {
        public string Name => "Poker Test (m = 4)";

        public bool Execute(byte[] data)
        {
            if (data.Length != 2500)
                throw new ArgumentException("The array must be exactly 2500 bytes long.");

            const int numBins = 16;
            const int m = 4;
            int k = (data.Length * 8) / m;
            int[] counts = new int[numBins];

            foreach (byte b in data)
            {
                int highNibble = (b >> 4) & 0x0F;
                counts[highNibble]++;
                int lowNibble = b & 0x0F;
                counts[lowNibble]++;
            }

            double sumOfSquares = 0;
            for (int i = 0; i < numBins; i++)
            {
                sumOfSquares += (double)counts[i] * counts[i];
            }

            double x3 = (16.0 / k) * sumOfSquares - k;
            return x3 > 1.03 && x3 < 57.4;
        }
    }
}