using Fips140.RandomnessTesting;
using Newtonsoft.Json.Bson;

namespace Fips140.Tests
{
    public class PokerTestTests
    {
        private readonly PokerTest _test = new PokerTest();

        [Fact]
        public void Execute_BalancedDistribution_ReturnsTrue()
        {
            // Arrange
            byte[] data = new byte[2500];
            var rng = new Random(42);
            rng.NextBytes(data);

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.True(result, "A sequence from a standard PRNG should fall within valid FIPS ranges.");
        }

        [Fact]
        public void Execute_AllSameNibbles_ReturnsFalse()
        {
            // Arrange
            byte[] data = new byte[2500];
            for (int i = 0; i < data.Length; i++)
                data[i] = 0x00;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, "Sequence with only one type of nibble must fail.");
        }

        [Theory]
        [InlineData(0x55)] // Binary 01010101 (only nibble '5')
        [InlineData(0xAA)] // Binary 10101010 (only nibble '10')
        public void Execute_ConstantPattern_ReturnsFalse(byte pattern)
        {
            // Arrange
            byte[] data = new byte[2500];
            for (int i = 0; i < data.Length; i++)
                data[i] = pattern;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, $"Sequence with repeating pattern 0x{pattern:X2} must fail.");
        }

    }
}
