using System;
using System.Collections.Generic;
using System.Text;

namespace Fips140.RandomnessTesting
{
    public interface IRandomnessTest
    {
        string Name { get; }
        bool Execute(byte[] data);
    }
}
