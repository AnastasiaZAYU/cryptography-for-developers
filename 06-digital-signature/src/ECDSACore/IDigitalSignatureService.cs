using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ECDSACore.Infrastructure;

namespace ECDSACore
{
    public interface IDigitalSignatureService
    {
        (BigInteger PrivateKey, ECPoint PublicKey) GenerateKeyPair();
        (BigInteger r, BigInteger s) Sign(string message, BigInteger privateKey);
        bool Verify(string message, BigInteger r, BigInteger s, ECPoint publicKey);
        string SerializeSignature(BigInteger r, BigInteger s);
        (BigInteger r, BigInteger s) DeserializeSignature(string signatureHex);
    }
}
