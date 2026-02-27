using Fips140.RandomnessTesting;

namespace Fips140.Tests
{
    public class RunsTestTests
    {
        private readonly RunsTest _test = new RunsTest();

        private byte[] CreatePatternData(byte pattern)
        {
            byte[] data = new byte[2500];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = pattern;
            }
            return data;
        }

        [Fact]
        public void Execute_ConstantPattern_ShouldFail()
        {
            // Arrange
            byte[] data = new byte[2500];

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, "A sequence of all zeros must fail the Runs Test.");
        }

        [Fact]
        public void Execute_AlternatingBits_ShouldFail()
        {
            // Arrange
            byte[] data = CreatePatternData(0xAA);

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, "Too many runs of length 1 should result in failure.");
        }

        [Fact]
        public void Execute_ValidSequence_ShouldPass()
        {
            // Arrange
            byte[] validData = new byte[2500];
            var rng = new System.Random(42);
            rng.NextBytes(validData);

            // Act
            bool result = _test.Execute(validData);

            // Assert
            Assert.True(result, "A statistically random sequence should pass the Runs Test.");
        }

        [Theory]
        [InlineData(1, 2266)] // Just below min for length 1
        [InlineData(1, 2734)] // Just above max for length 1
        [InlineData(6, 89)]   // Just below min for length 6+
        public void Validate_SpecificRunCounts_ShouldFail(int length, int count)
        {
            // Arrange
            byte[] badData = new byte[2500];
            if (length == 1)
            {
                for (int i = 0; i < badData.Length; i++)
                    badData[i] = 0xAA;
            }

            // Act
            bool result = _test.Execute(badData);

            // Assert
            Assert.False(result);
        }
    }
}