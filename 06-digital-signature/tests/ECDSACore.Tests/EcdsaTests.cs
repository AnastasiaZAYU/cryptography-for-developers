using ECDSACore.Infrastructure;
using System.Numerics;

namespace ECDSACore.Tests
{
    public class EcdsaTests
    {
        private readonly EcdsaService _service;
        private readonly IEllipticCurveService _ecService;

        public EcdsaTests()
        {
            _ecService = new BouncyCastleWrapper();
            var hashService = new Sha256HashService();
            _service = new EcdsaService(_ecService, hashService);
        }

        [Fact]
        public void Verify_ValidSignature_ReturnsTrue()
        {
            // Arrange
            var (privateKey, publicKey) = _service.GenerateKeyPair();
            string message = "The quick brown fox jumps over the lazy dog!";
            var (r, s) = _service.Sign(message, privateKey);

            // Act
            bool result = _service.Verify(message, r, s, publicKey);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Verify_CorruptedMessage_ReturnsFalse()
        {
            // Arrange
            var (privateKey, publicKey) = _service.GenerateKeyPair();
            var (r, s) = _service.Sign("Original message", privateKey);

            // Act
            bool result = _service.Verify("Modified message", r, s, publicKey);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Verify_CorruptedSignature_ReturnsFalse()
        {
            // Arrange
            var (privateKey, publicKey) = _service.GenerateKeyPair();
            string message = "Correct message";
            var (r, s) = _service.Sign(message, privateKey);

            // Act
            bool result = _service.Verify(message, r + 1, s, publicKey);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Verify_WrongPublicKey_ReturnsFalse()
        {
            // Arrange
            var (privateKey1, publicKey1) = _service.GenerateKeyPair();
            var (privateKey2, publicKey2) = _service.GenerateKeyPair();
            string message = "Test message";

            // Act
            var (r, s) = _service.Sign(message, privateKey1); 
            bool result = _service.Verify(message, r, s, publicKey2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Sign_SameMessageTwice_ProducesDifferentSignatures()
        {
            // Arrange
            var (privateKey, _) = _service.GenerateKeyPair();
            string message = "Same message";

            // Act
            var signature1 = _service.Sign(message, privateKey);
            var signature2 = _service.Sign(message, privateKey);

            // Assert
            Assert.NotEqual(signature1.r, signature2.r);
            Assert.NotEqual(signature1.s, signature2.s);
        }

        [Fact]
        public void Serialize_Deserialize_ShouldBeEqual()
        {
            // Arrange
            var (privateKey, _) = _service.GenerateKeyPair();
            var (r, s) = _service.Sign("Random test message", privateKey);

            // Act
            string hex = _service.SerializeSignature(r, s);
            var (dr, ds) = _service.DeserializeSignature(hex);

            // Assert
            Assert.Equal(r, dr);
            Assert.Equal(s, ds);
            Assert.False(string.IsNullOrWhiteSpace(hex), "Hex string should not be empty");
        }

        [Theory]
        [InlineData("0:0")]
        [InlineData("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF:FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        public void Deserialize_EdgeCases_ShouldNotThrow(string hex)
        {
            // Act
            var exception = Record.Exception(() => _service.DeserializeSignature(hex));
            
            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void GenerateKeyPair_PublicKey_ShouldBeOnCurve()
        {
            // Arrange
            var (_, publicKey) = _service.GenerateKeyPair();

            // Act
            bool isOnCurve = _ecService.IsOnCurveCheck(publicKey);

            // Assert
            Assert.True(isOnCurve);
        }
    }
}
