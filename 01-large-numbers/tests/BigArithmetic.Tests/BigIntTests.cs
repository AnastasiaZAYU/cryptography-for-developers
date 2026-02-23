using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace BigArithmetic.Tests
{
    public class BigIntTests
    {
        private static readonly Random _random = new();

        private string GenerateRandomHex(int byteLength)
        {
            byte[] buffer = new byte[byteLength];
            _random.NextBytes(buffer);
            return "00" + Convert.ToHexString(buffer);
        }

        private string NormalizeHex(string hex)
        {
            string normalized = hex.TrimStart('0').ToUpper();
            return string.IsNullOrEmpty(normalized) ? "0" : normalized;
        }

        [Theory]
        [InlineData(0)] // Test no shift
        [InlineData(1)] // Test single bit shift
        [InlineData(31)] // Test shift just before block boundary
        [InlineData(32)] // Boundary test for 32-bit blocks
        [InlineData(65)] // Test shift across multiple blocks
        public void Shifts_ShouldMatchSystemBigInteger(int shiftAmount)
        {
            // Arrange
            string hex = GenerateRandomHex(32);
            var custom = BigInt.FromHex(hex);
            var system = BigInteger.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            // Act & Assert for Left Shift
            Assert.Equal(
                NormalizeHex((system << shiftAmount).ToString("X")),
                NormalizeHex((custom << shiftAmount).ToHex()),
                ignoreCase: true
                );

            // Act & Assert for Right Shift
            Assert.Equal(
                NormalizeHex((system >> shiftAmount).ToString("X")),
                NormalizeHex((custom >> shiftAmount).ToHex()),
                ignoreCase: true
                );
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void RandomOperations_ShouldMatchSystemBigInteger(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                // Arrange
                string hexA = GenerateRandomHex(_random.Next(16, 128));
                string hexB = GenerateRandomHex(_random.Next(16, 128));

                var customA = BigInt.FromHex(hexA);
                var customB = BigInt.FromHex(hexB);

                var systemA = BigInteger.Parse(hexA, System.Globalization.NumberStyles.HexNumber);
                var systemB = BigInteger.Parse(hexB, System.Globalization.NumberStyles.HexNumber);

                // --- Bitwise Operations ---

                // Act & Assert for XOR
                Assert.Equal(
                    NormalizeHex((systemA ^ systemB).ToString("X")), 
                    NormalizeHex((customA ^ customB).ToHex()),
                    ignoreCase: true
                    );

                // Act & Assert for OR
                Assert.Equal(
                    NormalizeHex((systemA | systemB).ToString("X")),
                    NormalizeHex((customA | customB).ToHex()),
                    ignoreCase: true
                    );

                // Act & Assert for AND
                Assert.Equal(
                    NormalizeHex((systemA & systemB).ToString("X")),
                    NormalizeHex((customA & customB).ToHex()),
                    ignoreCase: true
                    );

                // Act & Assert for NOT
                var invA = ~customA;
                var backA = ~invA;
                Assert.Equal(customA.ToHex(), backA.ToHex(), ignoreCase: true);

                // --- Arithmetic Operations ---

                // Act & Assert for Addition
                Assert.Equal(
                    NormalizeHex((systemA + systemB).ToString("X")),
                    NormalizeHex((customA + customB).ToHex()),
                    ignoreCase: true
                    );

                // Act & Assert for Multiplication
                Assert.Equal(
                    NormalizeHex((systemA * systemB).ToString("X")),
                    NormalizeHex((customA * customB).ToHex()),
                    ignoreCase: true
                    );

                // Act & Assert for Subtraction (only if result is non-negative)
                if (systemA >= systemB)
                {
                    Assert.Equal(
                        NormalizeHex((systemA - systemB).ToString("X")),
                        NormalizeHex((customA - customB).ToHex()),
                        ignoreCase: true
                        );
                }

                // Act & Assert for Modulo
                if (!systemB.IsZero)
                {
                    Assert.Equal(
                        NormalizeHex((systemA % systemB).ToString("X")),
                        NormalizeHex((customA % customB).ToHex()),
                        ignoreCase: true
                    );
                }

                // Act & Assert for Division
                if (!systemB.IsZero)
                {
                    Assert.Equal(
                        NormalizeHex((systemA / systemB).ToString("X")),
                        NormalizeHex((customA / customB).ToHex()),
                        ignoreCase: true
                        );
                }
            }
        }

        [Fact]
        public void PowMod_Random_ShouldMatchSystemBigInteger()
        {
            // Arrange
            string hexBase = GenerateRandomHex(8);
            string hexExp = GenerateRandomHex(4);
            string hexMod = GenerateRandomHex(8);

            // Act
            var customResult = BigInt.PowMod(
                BigInt.FromHex(hexBase),
                BigInt.FromHex(hexExp),
                BigInt.FromHex(hexMod)
            );

            var systemBase = BigInteger.Parse(hexBase, System.Globalization.NumberStyles.HexNumber);
            var systemExp = BigInteger.Parse(hexExp, System.Globalization.NumberStyles.HexNumber);
            var systemMod = BigInteger.Parse(hexMod, System.Globalization.NumberStyles.HexNumber);

            var systemResult = BigInteger.ModPow(systemBase, systemExp, systemMod);

            // Assert
            Assert.Equal(
                NormalizeHex(systemResult.ToString("X")),
                NormalizeHex(customResult.ToHex()),
                ignoreCase: true
                );
        }
    }
}
