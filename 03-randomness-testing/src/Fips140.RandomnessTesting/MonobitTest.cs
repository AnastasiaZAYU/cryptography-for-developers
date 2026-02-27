using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Fips140.RandomnessTesting
{
    public class MonobitTest : IRandomnessTest
    {
        public string Name => "Monobit Test";

        public bool Execute(byte[] data)
        {
            if (data.Length != 2500)
                throw new ArgumentException("The array must be exactly 2500 bytes long.");

            int onesCount = 0;
            foreach (byte b in data)
            {
                onesCount += BitOperations.PopCount(b);
            }
            return onesCount > 9654 && onesCount < 10346;
        }
    }
}
