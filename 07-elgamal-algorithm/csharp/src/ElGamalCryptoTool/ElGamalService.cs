using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ElGamalCryptoTool
{
    public class ElGamalService
    {
        public ElGamalParameters GenerateParameters(int bitLength = 2048)
        {
            Console.WriteLine($"Generating a {bitLength}-bit prime number... (process may take some time)");
            BigInteger p;
            do
            {
                p = ElGamalMath.RandomBigInteger(
                    BigInteger.One << (bitLength - 1),
                    BigInteger.One << bitLength
                    );
                if (p % 2 == 0) p++;
            } while (!p.IsProbablePrime(20));
            Console.WriteLine("P generated. Searching for generator G...");

            BigInteger g;
            do
            {
                g = ElGamalMath.RandomBigInteger(2, p - 1);
            } while (BigInteger.ModPow(g, (p - 1) / 2, p) == 1);
            return new ElGamalParameters { P = p, G = g };
        }

        public ElGamalKeyPair GenerateKeyPair(ElGamalParameters parameters)
        {
            BigInteger privateKey = ElGamalMath.RandomBigInteger(2, parameters.P - 1);
            BigInteger publicKey = BigInteger.ModPow(parameters.G, privateKey, parameters.P);
            return new ElGamalKeyPair { PrivateKey = privateKey, PublicKey = publicKey };
        }
    }
}
