using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algebra_on_Elliptic_Curves;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestECDH()
        {
            // S = da * Hb = da * ( db * G ) = db * ( da * G ) = db * Ha
            var curve = new AlgebraicOperations();
            var G = curve.BasePointGGet();

            var da = curve.RandomNumber(256);
            var Ha = curve.ScalarMult(da, G);

            var db = curve.RandomNumber(256);
            var Hb = curve.ScalarMult(db, G);
            var Sb = curve.ScalarMult(db, Ha);

            var Sa = curve.ScalarMult(da, Hb);

            Assert.AreEqual(Sa, Sb);
        }

        [TestMethod]
        public void TestPointGeneration()
        {
            var curve = new AlgebraicOperations();
            var expected = curve.BasePointGGet();

            var gx = new BigInteger("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
            var gy = new BigInteger("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
            var actual = curve.ECPointGen(gx, gy);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPointMultiplication()
        {
            // k * ( d * G ) = d * ( k * G )
            var curve = new AlgebraicOperations();
            var G = curve.BasePointGGet();
            var k = curve.RandomNumber(256);
            var d = curve.RandomNumber(256);

            var H1 = curve.ScalarMult(d, G);
            var H2 = curve.ScalarMult(k, H1);

            var H3 = curve.ScalarMult(k, G);
            var H4 = curve.ScalarMult(d, H3);

            Assert.AreEqual(H2, H4);
        }

        [TestMethod]
        public void TestPointDoubling()
        {
            // G + G = 2G
            var curve = new AlgebraicOperations();
            var G = curve.BasePointGGet();

            var H1 = curve.AddECPoints(G, G);

            var H2 = curve.DoubleECPoints(G);

            Assert.AreEqual(H1, H2);
        }

        [TestMethod]
        public void TestAddition()
        {
            // ( A + B ) + C = A + ( B + C )
            var curve = new AlgebraicOperations();
            var A = curve.RandomPoint();
            var B = curve.RandomPoint();
            var C = curve.RandomPoint();

            var H1 = curve.AddECPoints(A, B);
            var H2 = curve.AddECPoints(H1, C);

            var H3 = curve.AddECPoints(B, C);
            var H4 = curve.AddECPoints(A, H3);

            Assert.AreEqual(H2, H4);
        }

        [TestMethod]
        public void TestPointValid()
        {
            // G є CURVE
            var curve = new AlgebraicOperations();
            var G = curve.BasePointGGet();
            var flag = curve.IsOnCurveCheck(G);

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void TestTransformation()
        {
            var curve = new AlgebraicOperations();
            var G = curve.BasePointGGet();

            var str = curve.ECPointToString(G);

            var H1 = curve.StringToECPoint(str);

            Assert.AreEqual(G, H1);
        }
    }
}
