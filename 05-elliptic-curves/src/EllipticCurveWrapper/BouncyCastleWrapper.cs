using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Parameters;

namespace EllipticCurveWrapper
{
    public class BouncyCastleWrapper : IEllipticCurveService
    {
        private readonly X9ECParameters _curve;
        private readonly ECDomainParameters _domain;

        public BouncyCastleWrapper(string curveName = "secp256k1")
        {
            _curve = CustomNamedCurves.GetByName(curveName) ?? ECNamedCurveTable.GetByName(curveName);
            _domain = new ECDomainParameters(_curve.Curve, _curve.G, _curve.N, _curve.H);
        }

        // G - generator receiving
        public ECPoint BasePointGGet() => MapToInternalPoint(_curve.G);

        // DOES P ∈ CURVE?
        public bool IsOnCurveCheck(ECPoint point)
        {
            try
            {
                var q = MapToBouncyPoint(point);
                return q.IsValid();
            }
            catch { return false; }
        }

        // P + Q
        public ECPoint AddECPoints(ECPoint p, ECPoint q)
        {
            var pointP = MapToBouncyPoint(p);
            var pointQ = MapToBouncyPoint(q);
            return MapToInternalPoint(pointP.Add(pointQ));
        }

        // 2P
        public ECPoint DoubleECPoints(ECPoint p)
        {
            var pointP = MapToBouncyPoint(p);
            return MapToInternalPoint(pointP.Twice());
        }

        // k * P
        public ECPoint ScalarMult(System.Numerics.BigInteger k, ECPoint p)
        {
            var pointP = MapToBouncyPoint(p);
            var bcK = new Org.BouncyCastle.Math.BigInteger(k.ToString());
            return MapToInternalPoint(pointP.Multiply(bcK).Normalize());
        }

        // Serialize point to string (Hex format)
        public string ECPointToString(ECPoint point)
        {
            var p = MapToBouncyPoint(point);
            return BitConverter.ToString(p.GetEncoded(false)).Replace("-", "");
        }

        // Deserialize point from string
        public ECPoint StringToECPoint(string s)
        {
            var bytes = Convert.FromHexString(s);
            var p = _curve.Curve.DecodePoint(bytes);
            return MapToInternalPoint(p);
        }

        private Org.BouncyCastle.Math.EC.ECPoint MapToBouncyPoint(ECPoint p)
        {
            if (p.IsInfinity)
                return _curve.Curve.Infinity;

            return _curve.Curve.CreatePoint(
                new Org.BouncyCastle.Math.BigInteger(p.X.ToString()),
                new Org.BouncyCastle.Math.BigInteger(p.Y.ToString()));
        }

        private ECPoint MapToInternalPoint(Org.BouncyCastle.Math.EC.ECPoint p)
        {
            if (p.IsInfinity)
                return ECPoint.Infinity;
            var normP = p.Normalize();
            return new ECPoint(
                System.Numerics.BigInteger.Parse(normP.XCoord.ToBigInteger().ToString()),
                System.Numerics.BigInteger.Parse(normP.YCoord.ToBigInteger().ToString()));
        } 
    }
}
