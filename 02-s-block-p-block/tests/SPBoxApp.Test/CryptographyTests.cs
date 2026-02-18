namespace SPBoxApp.Test
{
    public class CryptographyTests
    {
        private readonly Functions _fn = new Functions();

        [Theory]
        [InlineData("0000")]
        [InlineData("FFFF")]
        [InlineData("0F")]
        [InlineData("F0")]
        [InlineData("123456789ABCDEF0")]
        [InlineData("A5A5A5A5")]
        public void SBox_Transformation_ShouldBeReversible(string hexInput)
        {
            // Arrange
            byte[] original = _fn.Input(hexInput);
            byte[] data = (byte[])original.Clone();

            // Act
            _fn.S_box(data); // Forward
            _fn.S_box(data, inverse: true); // Inverse

            // Assert
            Assert.Equal(original, data);
        }

        [Theory]
        [InlineData("01")]
        [InlineData("80")]
        [InlineData("00FF")]
        [InlineData("55AA")]
        [InlineData("A1B2C3D4")]
        [InlineData("123456789ABCDEF0")]
        public void PBox_Transformation_ShouldBeReversible(string hexInput)
        {
            // Arrange
            byte[] original = _fn.Input(hexInput);
            byte[] data = (byte[])original.Clone();

            // Act
            _fn.P_box(data); // Forward
            _fn.P_box(data, inverse: true); // Inverse

            // Assert
            Assert.Equal(original, data);
        }

        [Fact]
        public void FullSPCycle_ShouldRestoreOriginalData()
        {
            // Arrange
            byte[] original = { 0x1F, 0x2E, 0x3D, 0x4C, 0x5B, 0x6A, 0x79, 0x80 };
            byte[] data = (byte[])original.Clone();

            // Act: Forward SP-network
            _fn.S_box(data);
            _fn.P_box(data);

            // Act: Inverse SP-network (Reverse order is critical!)
            _fn.P_box(data, inverse: true);
            _fn.S_box(data, inverse: true);

            // Assert
            Assert.Equal(original, data);
        }

        [Fact]
        public void Input_InvalidHexLength_ShouldThrowFormatException()
        {
            // Arrange
            string invalidHex = "ABC"; // Odd length

            // Assert
            Assert.Throws<FormatException>(() => _fn.Input(invalidHex));
        }

        [Fact]
        public void Input_NonHexCharacters_ShouldThrowFormatException()
        {
            // Arrange
            string invalidHex = "GHIJKL"; // Non-hex characters

            // Assert
            Assert.Throws<FormatException>(() => _fn.Input(invalidHex));
        }

        [Fact]
        public void Input_EmptyString_ShouldThrowArgumentException()
        {
            // Arrange
            string emptyHex = "";

            // Assert
            Assert.Throws<ArgumentException>(() => _fn.Input(emptyHex));
        }
    }
}
