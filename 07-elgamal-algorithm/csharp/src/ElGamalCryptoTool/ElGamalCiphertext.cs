using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ElGamalCryptoTool
{
    public readonly struct ElGamalCiphertext
    {
        public BigInteger X { get; init; }
        public List<BigInteger> Y { get; init; }

        public ElGamalCiphertext(BigInteger x, List<BigInteger> y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            string hexY = string.Join(":", Y.Select(b => b.ToString("X")));
            return $"Ephemeral Key (X): 0x{X.ToString("X")}\n" +
                "Encrypted Blocks (Y): {hexY}\n";
        }
    }
}
