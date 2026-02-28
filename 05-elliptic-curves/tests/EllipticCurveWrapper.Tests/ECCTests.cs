using System.Numerics;
using System.Security.Cryptography;

namespace EllipticCurveWrapper.Tests
{
    public class ECCTests
    {
        private readonly IEllipticCurveService _ecc;

        public ECCTests()
        {
            _ecc = new BouncyCastleWrapper("secp256k1");
        }

        private BigInteger GenerateRandomBigInteger(int bytesCount)
        {
            byte[] data = new byte[bytesCount];
            RandomNumberGenerator.Fill(data);
            return BigInteger.Abs(new BigInteger(data));
        }

        [Fact]
        public void BasePoint_ShouldBeOnCurve()
        {
            // Arrange
            var g = _ecc.BasePointGGet();

            // Act
            var isOnCurve = _ecc.IsOnCurveCheck(g);

            // Assert
            Assert.True(isOnCurve, "Base point G must be on the curve.");
            Assert.NotNull(g.X);
            Assert.NotNull(g.Y);
        }

        [Fact]
        public void ScalarMultiplication_CommutativeProperty_Test()
        {
            // k * (d * G) = d * (k * G)
            // Arrange
            var g = _ecc.BasePointGGet();
            var k = GenerateRandomBigInteger(32);
            var d = GenerateRandomBigInteger(32);

            // Act
            var dG = _ecc.ScalarMult(d, g);
            var leftSide = _ecc.ScalarMult(k, dG);

            var kG = _ecc.ScalarMult(k, g);
            var rightSide = _ecc.ScalarMult(d, kG);

            // Assert
            Assert.Equal(leftSide, rightSide);
            Assert.True(_ecc.IsOnCurveCheck(leftSide));
        }

        [Fact]
        public void Serialization_ShouldRestoreOriginalPoint()
        {
            // Arrange
            var g = _ecc.BasePointGGet();
            var k = GenerateRandomBigInteger(32);
            var originalPoint = _ecc.ScalarMult(k, g);

            // Act
            var hexString = _ecc.ECPointToString(originalPoint);
            var restoredPoint = _ecc.StringToECPoint(hexString);

            // Assert
            Assert.NotNull(restoredPoint.X);
            Assert.NotNull(restoredPoint.Y);
            Assert.Equal(originalPoint, restoredPoint);
            Assert.True(_ecc.IsOnCurveCheck(restoredPoint));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("12345678901234567890")]
        public void DoublePoint_ShouldEqual_ScalarMultByTwo(string scalarStr)
        {
            // P + P = 2 * P
            // Arrange
            var g = _ecc.BasePointGGet();
            var p = _ecc.ScalarMult(BigInteger.Parse(scalarStr) + GenerateRandomBigInteger(8), g);

            // Act
            var doubled = _ecc.DoubleECPoints(p);
            var scalarTwo = _ecc.ScalarMult(new BigInteger(2), p);

            // Assert
            Assert.Equal(doubled, scalarTwo);
        }

        [Fact]
        public void InfinityPoint_ShouldBeRecognized()
        {
            // Arrange
            var inf = ECPoint.Infinity;

            // Act
            var isInf = inf.IsInfinity;

            // Assert
            Assert.True(isInf);
            Assert.Null(inf.X);
        }
    }
}
