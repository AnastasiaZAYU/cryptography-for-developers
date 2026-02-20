using System.Security.Cryptography;
using System.Text;

namespace Sha1Core.Test
{
    public class Sha1AlgorithmTests
    {
        [Theory]
        [InlineData("")] // Empty string (0 bytes)
        [InlineData("world")] // Less than block size (5 bytes)
        [InlineData("This string is exactly fifty-five characters long total")] // 55 bytes - a single 64-byte block with 9 bytes of padding
        [InlineData("This string is exactly fifty-six characters long total!!")] // 56 bytes - the threshold where padding MUST overflow into an additional block
        [InlineData("1234567890123456789012345678901234567890123456789012345678901234")] // 64 bytes - a single block with no padding
        [InlineData("The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.")] // More than one block (89 bytes)
        [InlineData("1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890ab")] // multiple blocks (192 bytes)
        public void Hash_MatchesLibraryImplementation(string input)
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes(input);

            // Act
            byte[] customHash = Sha1Hasher.HashData(data);
            byte[] libraryHash = SHA1.HashData(data);

            // Assert
            Assert.Equal(libraryHash, customHash);
        }

        [Fact]
        public void Hash_MultiBlockData_MatchesLibrary()
        {
            // Arrange
            byte[] largeData = new byte[1024 * 1024]; // 1 MB of data
            Random.Shared.NextBytes(largeData);

            // Act
            byte[] customHash = Sha1Hasher.HashData(largeData);
            byte[] libraryHash = SHA1.HashData(largeData);

            // Assert
            Assert.Equal(libraryHash, customHash);
        }

        [Fact]
        public void HashFile_LargeBinaryFile_MatchesLibrary()
        {
            // Arrange
            string filePath = Path.Combine(AppContext.BaseDirectory, "data", "testfile.bin");

            if (!File.Exists(filePath))
            {
                string? directory = Path.GetDirectoryName(filePath);
                if (directory != null)
                    Directory.CreateDirectory(directory);

                byte[] dummyData = new byte[10 * 1024 * 1024]; // 10 MB of data
                new Random().NextBytes(dummyData);
                File.WriteAllBytes(filePath, dummyData);
            }

            // Act
            byte[] customHash = Sha1Hasher.HashFile(filePath);
            using var fs = File.OpenRead(filePath);
            byte[] libraryHash = SHA1.HashData(fs);

            // Assert
            Assert.Equal(libraryHash, customHash);
        }
    }
}
