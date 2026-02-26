using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace ElGamalCryptoTool
{
    public class ElGamalService
    {
        public ElGamalParameters GenerateParameters(int bitLength = 2048)
        {
            Console.WriteLine($"Generating a {bitLength}-bit prime number... (process may take some time)");
            
            BigInteger min = BigInteger.One << (bitLength - 1);
            BigInteger max = BigInteger.One << bitLength;
            BigInteger p = ElGamalMath.RandomBigInteger(min, max);
            
            if (p.IsEven) 
                p++;

            while (!p.IsProbablePrime(20))
            {
                p += 2;
                if (p >= max)
                    p = min + 1;
            }
            Console.WriteLine("P generated. Searching for generator G...");

            BigInteger g;
            do
            {
                g = ElGamalMath.RandomBigInteger(2, p - 1);
            } while (BigInteger.ModPow(g, 2, p) == 1 || BigInteger.ModPow(g, (p - 1) / 2, p) == 1);
            return new ElGamalParameters { P = p, G = g };
        }

        public ElGamalKeyPair GenerateKeyPair(ElGamalParameters parameters)
        {
            if (parameters.P <= 2)
                throw new ArgumentException("Invalid prime P in parameters.");
            BigInteger privateKey = ElGamalMath.RandomBigInteger(2, parameters.P - 1);
            BigInteger publicKey = BigInteger.ModPow(parameters.G, privateKey, parameters.P);
            return new ElGamalKeyPair { PrivateKey = privateKey, PublicKey = publicKey };
        }

        public ElGamalSignature Sign(string message, ElGamalParameters parameters, BigInteger privateKey)
        {
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(message));
            BigInteger hm = new BigInteger(hashBytes, isUnsigned: true, isBigEndian: true);
            BigInteger pMinusOne = parameters.P - 1;
            BigInteger k, r, s;
            while (true)
            {
                k = ElGamalMath.RandomBigInteger(2, pMinusOne);
                if (BigInteger.GreatestCommonDivisor(k, pMinusOne) == 1)
                {
                    r = BigInteger.ModPow(parameters.G, k, parameters.P);
                    BigInteger kInv = ElGamalMath.ModInverse(k, pMinusOne);
                    s = (kInv * (hm - (privateKey * r) % pMinusOne + pMinusOne)) % pMinusOne;
                    if (s != 0) break;
                }
            }
            return new ElGamalSignature { R = r, S = s };
        }

        public bool Verify(string message, ElGamalSignature signature, ElGamalParameters parameters, BigInteger publicKey)
        {
            if (signature.R <= 0 || signature.R >= parameters.P ||
                signature.S <= 0 || signature.S >= (parameters.P - 1))
                return false;
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(message));
            BigInteger hm = new BigInteger(hashBytes, isUnsigned: true, isBigEndian: true);
            BigInteger v1 = BigInteger.ModPow(parameters.G, hm, parameters.P);
            BigInteger term1 = BigInteger.ModPow(publicKey, signature.R, parameters.P);
            BigInteger term2 = BigInteger.ModPow(signature.R, signature.S, parameters.P);   
            BigInteger v2 = (term1 * term2) % parameters.P;
            return v1 == v2;
        }

        public ElGamalCiphertext Encrypt(string message, ElGamalParameters parameters, BigInteger publicKey)
        {
            BigInteger k = ElGamalMath.RandomBigInteger(2, parameters.P - 1);
            BigInteger x = BigInteger.ModPow(parameters.G, k, parameters.P);
            BigInteger sharedSecret = BigInteger.ModPow(publicKey, k, parameters.P);
            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            int blockSize = ((int)parameters.P.GetBitLength() - 1) / 8;
            if (blockSize <= 0)
                blockSize = 1;
            List<BigInteger> encryptedBlocks = new List<BigInteger>();
            for (int i = 0; i < msgBytes.Length; i += blockSize)
            {
                int currentBlockSize = Math.Min(blockSize, msgBytes.Length - i);
                byte[] block = new byte[currentBlockSize];
                Array.Copy(msgBytes, i, block, 0, currentBlockSize);
                BigInteger m = new BigInteger(block, isUnsigned: true, isBigEndian: true);
                BigInteger y = (m * sharedSecret) % parameters.P;
                encryptedBlocks.Add(y);
            }
            return new ElGamalCiphertext { X = x, Y = encryptedBlocks };
        }

        public string Decrypt(ElGamalCiphertext ciphertext, ElGamalParameters parameters, BigInteger privateKey)
        {
            BigInteger sharedSecret = BigInteger.ModPow(ciphertext.X, privateKey, parameters.P);
            BigInteger sInv = ElGamalMath.ModInverse(sharedSecret, parameters.P);
            int expectedBlockSize = ((int)parameters.P.GetBitLength() - 1) / 8;
            if (expectedBlockSize <= 0)
                expectedBlockSize = 1;
            List<byte> decryptedBytes = new List<byte>();
            for (int i = 0; i < ciphertext.Y.Count; i++)
            {
                BigInteger y = ciphertext.Y[i];
                BigInteger m = (y * sInv) % parameters.P;
                byte[] block = m.ToByteArray(isUnsigned: true, isBigEndian: true);
                if (i < ciphertext.Y.Count - 1 && block.Length < expectedBlockSize)
                {
                    byte[] paddedBlock = new byte[expectedBlockSize];
                    Array.Copy(block, 0, paddedBlock, expectedBlockSize - block.Length, block.Length);
                    block = paddedBlock;
                }
                decryptedBytes.AddRange(block);
            }
            return Encoding.UTF8.GetString(decryptedBytes.ToArray());
        }
    }
}
