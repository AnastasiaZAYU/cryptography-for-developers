using System;
using System.Collections.Generic;
using System.Text;

namespace Fips140.RandomnessTesting
{
    public class FipsValidator
    {
        private readonly List<IRandomnessTest> _tests;

        public FipsValidator()
        {
            _tests = new List<IRandomnessTest>
            {
                new MonobitTest(),
                new PokerTest(),
                new RunsTest(),
                new LongRunTest()
            };
        }

        public (bool IsValid, List<(string TestName, bool Passed)> Details) Validate(byte[] data)
        {
            if (data == null || data.Length != 2500)
                throw new ArgumentException("The array must be exactly 2500 bytes long.");

            var details = new List<(string TestName, bool Passed)>(_tests.Count);
            bool allPassed = true;
            foreach (var test in _tests)
            {
                bool passed = test.Execute(data);
                details.Add((test.Name, passed));
                if (!passed)
                {
                    allPassed = false;
                }
            }
            return (allPassed, details);
        }
    }
}
