using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Algebra_on_Elliptic_Curves
{
    public class AlgebraicOperations
    {
        ECCurve curve;
        ECPoint G;

        public AlgebraicOperations()
        {
            var q = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", 16);
            var a = BigInteger.Zero;
            var b = new BigInteger("7");
            var order = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);

            curve = new FpCurve(q, a, b, order, BigInteger.One);
            BasePointGGet();
        }

        public ECPoint BasePointGGet()
        {
            var gx = new BigInteger("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
            var gy = new BigInteger("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
            G = curve.CreatePoint(gx, gy);
            return G;
        }

        public ECPoint ECPointGen(BigInteger x, BigInteger y)
        {
            return curve.CreatePoint(x, y);
        }

        public bool IsOnCurveCheck(ECPoint a)
        {
            return a.IsValid();
        }

        public ECPoint AddECPoints(ECPoint a, ECPoint b)
        {
            return a.Add(b);
        }

        public ECPoint DoubleECPoints(ECPoint a)
        {
            return a.Twice();
        }

        public ECPoint ScalarMult(BigInteger k, ECPoint a)
        {
            return a.Multiply(k);
        }	

        public string ECPointToString(ECPoint a)
        {
            return a.ToString();
        }

        public ECPoint StringToECPoint(string s)
        {
            s = s.Remove(0, 1);
            string[] coordinates = s.Split(',');
            var a = curve.CreatePoint(new BigInteger(coordinates[0], 16), new BigInteger(coordinates[1], 16));
            return a;
        }

        public void PrintECPoint(ECPoint a)
        {
            Console.WriteLine("x: {0}", a.XCoord);
            Console.WriteLine("y: {0}", a.YCoord);
        }

        public BigInteger RandomNumber(int len)
        {
            SecureRandom random = new SecureRandom();
            byte[] bytes = new byte[len/8];
            random.NextBytes(bytes);
            var bignumber = new BigInteger(1, bytes);
            return bignumber;
        }

        public ECPoint RandomPoint()
        {
            var scalar = RandomNumber(256);
            var point = ScalarMult(scalar, G);
            return point;
        }
    }
}
