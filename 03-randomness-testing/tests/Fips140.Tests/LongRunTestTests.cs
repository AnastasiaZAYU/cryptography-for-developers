using Fips140.RandomnessTesting;
using Newtonsoft.Json.Bson;

namespace Fips140.Tests
{
    public class LongRunTestTests
    {
        private readonly LongRunTest _test = new LongRunTest();
        [Fact]
        public void Execute_NoRunsLongerThan36_ReturnsTrue()
        {
            // Arrange
            byte[] data = new byte[2500];
            for (int i = 0; i < data.Length; i++) 
                data[i] = 0xAA;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.True(result, "Alternating sequence should pass Long Run Test.");
        }

        [Fact]
        public void Execute_Exactly36BitsRun_ReturnsTrue()
        {
            // Arrange
            byte[] data = new byte[2500];
            for (int i = 0; i < data.Length; i++)
                data[i] = 0xAA;
            data[0] = 0xFF;
            data[1] = 0xFF;
            data[2] = 0xFF;
            data[3] = 0xFF;
            data[4] = 0xF5;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.True(result, "A run of exactly 36 bits should be valid.");
        }

        [Fact]
        public void Execute_37BitsRun_ReturnsFalse()
        {
            // Arrange
            byte[] data = new byte[2500];
            data[0] = 0xFF;
            data[1] = 0xFF;
            data[2] = 0xFF;
            data[3] = 0xFF;
            data[4] = 0xF8;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, "A run of 37 bits must fail the test.");
        }

        [Fact]
        public void Execute_LongRunAtTheVeryEnd_ReturnsFalse()
        {
            // Arrange
            byte[] data = new byte[2500];
            data[2495] = 0x10;
            for (int i = 0; i < 2500; i++) data[i] = 0xFF; 
            data[2495] = 0x07;
            data[2496] = 0x00;
            data[2497] = 0x00;
            data[2498] = 0x00;
            data[2499] = 0x00;

            // Act
            bool result = _test.Execute(data);

            // Assert
            Assert.False(result, "A run of 37 zeros at the end must fail.");
        }

    }
}
