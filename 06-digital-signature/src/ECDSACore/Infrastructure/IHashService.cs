using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ECDSACore.Infrastructure
{
    public interface IHashService
    {
        BigInteger HashData(string data, BigInteger modulo);
    }
}
