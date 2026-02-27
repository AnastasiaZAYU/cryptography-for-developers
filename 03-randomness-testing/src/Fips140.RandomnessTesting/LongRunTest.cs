using System;
using System.Collections.Generic;
using System.Text;

namespace Fips140.RandomnessTesting
{
    public class LongRunTest : IRandomnessTest
    {
        public string Name => "Long Run Test";

        public bool Execute(byte[] data)
        {
            if (data.Length != 2500)
                throw new ArgumentException("The array must be exactly 2500 bytes long.");

            const int maxAllowedRun = 36;
            int currentRunLength = 0;
            int? lastBit = null;

            foreach (byte b in data)
            {
                for (int i = 7; i >= 0; i--)
                {
                    int currentBit = (b >> i) & 1;
                    if (currentBit == lastBit)
                    {
                        currentRunLength++;
                    }
                    else
                    {
                        if (currentRunLength > maxAllowedRun)
                            return false;

                        currentRunLength = 1;
                        lastBit = currentBit;
                    }
                }
            }
            return currentRunLength <= maxAllowedRun;
        }
    }
}
