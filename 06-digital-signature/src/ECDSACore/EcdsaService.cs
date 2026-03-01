using ECDSACore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ECDSACore
{
    public class EcdsaService : IDigitalSignatureService
    {
        private readonly IEllipticCurveService _ecService;
        private readonly IHashService _hashService;
        private readonly BigInteger _n;

        public EcdsaService(IEllipticCurveService ecService, IHashService hashService)
        {
            _ecService = ecService;
            _hashService = hashService;
            _n = _ecService.GetOrderN();
        }

        public (BigInteger PrivateKey, ECPoint PublicKey) GenerateKeyPair()
        {
            BigInteger d = GenerateRandomBigInt(1, _n);
            ECPoint q = _ecService.ScalarMult(d, _ecService.BasePointGGet());
            if (q.IsInfinity || !_ecService.IsOnCurveCheck(q))
                throw new Exception("Invalid public key generation");
            return (d, q);
        }

        public (BigInteger r, BigInteger s) Sign(string message, BigInteger privateKey)
        {
            BigInteger h = _hashService.HashData(message, _n);
            while (true)
            {
                BigInteger k = GenerateRandomBigInt(1, _n);
                ECPoint p1 = _ecService.ScalarMult(k, _ecService.BasePointGGet());
                if (p1.IsInfinity)
                    continue;

                BigInteger xCoord = p1.X ?? throw new InvalidOperationException("Point coordinate is null");
                BigInteger r = xCoord % _n;
                if (r == 0)
                    continue;
                
                BigInteger kInv = ModInverse(k, _n);
                BigInteger s = (kInv * (h + privateKey * r)) % _n;
                if (s < 0)
                    s += _n;

                if (s == 0)
                    continue;
                return (r, s);
            }
        }

        public bool Verify(string message, BigInteger r, BigInteger s, ECPoint publicKey)
        {
            if (r < 1 || r >= _n || s < 1 || s >= _n)
                return false;

            BigInteger h = _hashService.HashData(message, _n);
            BigInteger c = ModInverse(s, _n);

            BigInteger u1 = (h * c) % _n;
            BigInteger u2 = (r * c) % _n;

            ECPoint p1 = _ecService.ScalarMult(u1, _ecService.BasePointGGet());
            ECPoint p2 = _ecService.ScalarMult(u2, publicKey);
            ECPoint p0 = _ecService.AddECPoints(p1, p2);
            if (p0.IsInfinity)
                return false;

            BigInteger xCoord = p0.X ?? throw new InvalidOperationException("Point coordinate is null");
            BigInteger v = xCoord % _n;

            return v == r;
        }

        private BigInteger ModInverse(BigInteger a, BigInteger n) =>
            BigInteger.ModPow(a, n - 2, n);

        private BigInteger GenerateRandomBigInt(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger result;
            do
            {
                System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
                result = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
            } while (result < min || result >= max);
            return result;
        }

        public string SerializeSignature(BigInteger r, BigInteger s) =>
            $"{r:X}:{s:X}";

        public (BigInteger r, BigInteger s) DeserializeSignature(string signature)
        {
            var parths = signature.Split(':');
            return (BigInteger.Parse(parths[0], System.Globalization.NumberStyles.HexNumber),
                BigInteger.Parse(parths[1], System.Globalization.NumberStyles.HexNumber));
        }
    }
}
