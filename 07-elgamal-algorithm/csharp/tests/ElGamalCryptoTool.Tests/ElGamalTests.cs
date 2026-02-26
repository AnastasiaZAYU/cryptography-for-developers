using System.Numerics;

namespace ElGamalCryptoTool.Tests
{
    public class ElGamalTests
    {
        private readonly ElGamalService _service;
        private readonly ElGamalParameters _parameters;
        private readonly ElGamalKeyPair _keyPair;

        public ElGamalTests()
        {
            _service = new ElGamalService();
            _parameters = _service.GenerateParameters(512);
            _keyPair = _service.GenerateKeyPair(_parameters);
        }

        [Fact]
        public void Signature_ShouldBeValid_ForOriginalMessage()
        {
            // Arrange
            string message = "Standard test message";

            // Act
            var signature = _service.Sign(message, _parameters, _keyPair.PrivateKey);
            bool isValid = _service.Verify(message, signature, _parameters, _keyPair.PublicKey);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Signature_ShouldBeInvalid_ForModifiedMessage()
        {
            // Arrange
            string message = "Original message";
            var signature = _service.Sign(message, _parameters, _keyPair.PrivateKey);

            // Act
            bool isValid = _service.Verify("Modified message", signature, _parameters, _keyPair.PublicKey);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Short")]
        [InlineData("Very long message that will definitely be split into multiple blocks because its length exceeds the key size significantly...")]
        [InlineData("   Message with spaces   ")]
        [InlineData("!2#$%^&*()_+ symbols")]
        public void Encryption_ShouldRestoreOriginalText_ForVariousInputs(string originalMessage)
        {
            // Act
            var ciphertext = _service.Encrypt(originalMessage, _parameters, _keyPair.PublicKey);
            string decryptedMessage = _service.Decrypt(ciphertext, _parameters, _keyPair.PrivateKey);

            // Assert
            Assert.Equal(originalMessage, decryptedMessage);
        }

        [Fact]
        public void Decryption_WithWrongKey_ShouldFail()
        {
            // Arrange
            string message = "Secret data";
            var secondKeyPair = _service.GenerateKeyPair(_parameters);
            var ciphertext = _service.Encrypt(message, _parameters, _keyPair.PublicKey);

            // Act
            string decryptedWithWrongKey = _service.Decrypt(ciphertext, _parameters, secondKeyPair.PrivateKey);

            // Assert
            Assert.NotEqual(message, decryptedWithWrongKey);
        }

        [Fact]
        public void Math_ModInverse_ShouldThrow_WhenNoInverseExists()
        {
            // Arrange 
            BigInteger a = 10;
            BigInteger n = 20;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ElGamalMath.ModInverse(a, n));
         }
    }
}
