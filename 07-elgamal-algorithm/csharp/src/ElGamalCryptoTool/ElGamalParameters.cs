using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ElGamalCryptoTool
{
    public readonly struct ElGamalParameters
    {
        public BigInteger P { get; init; }
        public BigInteger G { get; init; }

        public ElGamalParameters(BigInteger p, BigInteger g)
        {
            P = p;
            G = g;
        }
    }
}
