using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Numerics;
using System.Text;

namespace ECDSACore.Infrastructure
{
    public class Sha256HashService : IHashService
    {
        public BigInteger HashData(string data, BigInteger modulo)
        {
            if (modulo == BigInteger.Zero)
                return BigInteger.Zero;

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

            BigInteger h = new BigInteger(hashBytes, isUnsigned: true, isBigEndian: true);
            return h % modulo;
        }
    }
}
