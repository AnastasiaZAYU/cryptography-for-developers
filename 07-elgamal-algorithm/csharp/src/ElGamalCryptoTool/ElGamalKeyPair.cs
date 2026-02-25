using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ElGamalCryptoTool
{
    public readonly struct ElGamalKeyPair
    {
        public BigInteger PrivateKey { get; init; }
        public BigInteger PublicKey { get; init; }

        public ElGamalKeyPair(BigInteger privateKey, BigInteger publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }
}
