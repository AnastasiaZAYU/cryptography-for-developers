using Fips140.RandomnessTesting;

namespace Fips140.Tests
{
    public class GeneratorTests
    {
        private readonly Generator _generator = new Generator();

        [Fact]
        public void GenerateKey_DefaultSize_Returns2500Bytes()
        {
            // Act
            byte[] key = _generator.GenerateKey();

            // Assert
            Assert.NotNull(key);
            Assert.Equal(2500, key.Length);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(5000)]
        public void GenerateKey_CustomSize_ReturnsRequestedLength(int size)
        {
            // Act
            byte[] key = _generator.GenerateKey(size);

            // Assert
            Assert.Equal(size, key.Length);
        }

        [Fact]
        public void GenerateKey_ConsecutiveCalls_ProduceDifferentData()
        {
            // Arrange & Act
            byte[] key1 = _generator.GenerateKey(16);
            byte[] key2 = _generator.GenerateKey(16);

            string hex1 = _generator.ToHexString(key1);
            string hex2 = _generator.ToHexString(key2);

            // Assert
            Assert.NotEqual(hex1, hex2);
        }

        [Fact]
        public void ToHexString_ValidInput_ReturnsCorrectFormat()
        {
            // Arrange
            byte[] data = { 0x00, 0xAA, 0xFF, 0x12 };
            string expected = "00AAFF12";

            // Act
            string result = _generator.ToHexString(data);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateKey_DoesNotReturnConstantStream()
        {
            // Arrange
            byte[] key = _generator.GenerateKey(100);

            // Act
            bool allZeros = true;
            bool allOnes = true;
            foreach (byte b in key)
            {
                if (b != 0x00) allZeros = false;
                if (b != 0xFF) allOnes = false;
            }

            // Assert
            Assert.False(allZeros, "Generator should not return only zeros.");
            Assert.False(allOnes, "Generator should not return only ones.");
        }
    }
}