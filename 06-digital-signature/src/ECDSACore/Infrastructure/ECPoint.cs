using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ECDSACore.Infrastructure
{
    public readonly record struct ECPoint(BigInteger? X, BigInteger? Y)
    {
        public bool IsInfinity => !X.HasValue || !Y.HasValue;
        public static ECPoint Infinity => new(null, null);
    }
}
