using Fips140.RandomnessTesting;

namespace Fips140.Tests
{
    public class MonobitTestTests
    {
        private readonly MonobitTest _test = new MonobitTest();

        private byte[] CreateDataWithOnes(int totalOnes)
        {
            byte[] data = new byte[2500];
            int currentOnes = 0;

            for (int i = 0; i < data.Length && currentOnes < totalOnes; i++)
            {
                for (int bit = 0; bit < 8 && currentOnes < totalOnes; bit++)
                {
                    data[i] |= (byte)(1 << bit);
                    currentOnes++;
                }
            }
            return data;
        }

        [Theory]
        [InlineData(9654)] // Lower limit
        [InlineData(10000)] // Perfect balance
        [InlineData(10346)] // Upper limit
        public void Execute_ValidNumberOfOnes_ReturnTrue(int onesCount)
        {
            // Arrange
            byte[] data = CreateDataWithOnes(onesCount);

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.True(result, $"Test should pass for {onesCount} ones.");
        }

        [Theory]
        [InlineData(9653)] // 1 less than the limit
        [InlineData(10347)] // 1 more than the limit
        [InlineData(0)] // All zeros
        [InlineData(20000)] // All ones
        public void Execute_InvalidNumberOfOnes_ReturnFalse(int onesCount)
        {
            byte[] data = CreateDataWithOnes(onesCount);

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, $"Test should fail for {onesCount} ones.");
        }

        [Fact]
        public void Execute_NullData_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _test.Execute(null!));
        }
    }
}
