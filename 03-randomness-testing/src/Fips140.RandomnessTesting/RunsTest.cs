using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Fips140.RandomnessTesting
{
    public class RunsTest : IRandomnessTest
    {
        public string Name => "Runs Test";

        private readonly Dictionary<int, (int Min, int Max)> _limits = new()
        {
            {1, (2267, 2733) },
            {2, (1079, 1421) },
            {3, (502, 748) },
            {4, (223, 402) },
            {5, (90, 223) },
            {6, (90, 223) }
        };

        public bool Execute(byte[] data)
        {
            if (data.Length != 2500)
                throw new ArgumentException("The array must be exactly 2500 bytes long.");

            int[] zerosRuns = new int[7];
            int[] onesRuns= new int[7];

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
                        if (lastBit.HasValue)
                            RecordRun(lastBit.Value == 1 ? onesRuns : zerosRuns, currentRunLength);
                        lastBit = currentBit;
                        currentRunLength = 1;
                    }
                }
            }
            RecordRun(lastBit!.Value == 1 ? onesRuns : zerosRuns, currentRunLength);
            return Validate(zerosRuns) && Validate(onesRuns);
        }

        private void RecordRun(int[] runs, int length)
        {
            if (length >= 6)
            {
                runs[6]++;
            }
            else if (length > 0)
            {
                runs[length]++;
            }
        }

        private bool Validate(int[] runs)
        {
            for (int i = 1; i <= 6; i++)
            {
                var (min, max) = _limits[i];
                if (runs[i] < min || runs[i] > max)
                    return false;
            }
            return true;
        }
    }
}
