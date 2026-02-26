using System.Numerics;
using System.Runtime.CompilerServices;

namespace ElGamalCryptoTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _service = new ElGamalService();

            var parameters = _service.GenerateParameters(3072);
            Console.WriteLine(parameters);

            var keys = _service.GenerateKeyPair(parameters);
            Console.WriteLine(keys);

            string originalMessage = "The quick brown fox jumps over the lazy dog.";

            var signature = _service.Sign(originalMessage, parameters, keys.PrivateKey);
            bool isValid = _service.Verify(originalMessage, signature, parameters, keys.PublicKey);
            Console.WriteLine($"Signature valid: {isValid}\n");

            var cipher = _service.Encrypt(originalMessage, parameters, keys.PublicKey);
            string decrypted = _service.Decrypt(cipher, parameters, keys.PrivateKey);
            Console.WriteLine($"Decrypted text: {decrypted}\n");
        }
    }
}
