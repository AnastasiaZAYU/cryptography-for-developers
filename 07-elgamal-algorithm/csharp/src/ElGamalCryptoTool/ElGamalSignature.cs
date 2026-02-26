using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ElGamalCryptoTool
{
    public readonly struct ElGamalSignature
    {
        public BigInteger R { get; init; }
        public BigInteger S { get; init; }

        public ElGamalSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public override string ToString() =>
            $"R: 0x{R.ToString("X")}\nS: 0x{S.ToString("X")}\n";
    }
}
