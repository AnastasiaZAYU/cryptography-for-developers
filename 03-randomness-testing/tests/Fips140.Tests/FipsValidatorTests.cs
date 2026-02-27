using Fips140.RandomnessTesting;

namespace Fips140.Tests
{
    public class FipsValidatorTests
    {
        private readonly FipsValidator _validator = new FipsValidator();

        [Fact]
        public void Validate_InvalidDataLength_ThrowsArgumentException()
        {
            // Arrange
            byte[] shortData = new byte[100];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _validator.Validate(shortData));
            Assert.Contains("2500 bytes", exception.Message);
        }

        [Fact]
        public void Validate_NullData_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _validator.Validate(null!));
        }

        [Fact]
        public void Validate_BadData_ReturnsIsValidFalseAndDetailedReport()
        {
            // Arrange
            byte[] badData = new byte[2500];
            for (int i = 0; i < badData.Length; i++) badData[i] = 0x00;

            // Act
            var (isValid, details) = _validator.Validate(badData);

            // Assert
            Assert.False(isValid, "Validator should return false for non-random data.");
            Assert.Equal(4, details.Count); 
            Assert.Contains(details, d => d.Passed == false);
        }

        [Fact]
        public void Validate_ReportContainsAllTestNames()
        {
            // Arrange
            byte[] data = new byte[2500];

            // Act
            var (_, details) = _validator.Validate(data);

            // Assert
            var testNames = details.Select(d => d.TestName).ToList();
            Assert.Contains("Monobit Test", testNames);
            Assert.Contains("Poker Test (m = 4)", testNames);
            Assert.Contains("Runs Test", testNames);
            Assert.Contains("Long Run Test", testNames);
        }
    }
}