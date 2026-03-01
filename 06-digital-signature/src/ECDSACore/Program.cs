using ECDSACore.Infrastructure;

namespace ECDSACore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var ecService = new BouncyCastleWrapper();
            var hashService = new Sha256HashService();
            var ecdsa = new EcdsaService(ecService, hashService);

            Console.WriteLine("======= ECDSA Digital Signature Demo  =======");
                
            var (privateKey, publicKey) = ecdsa.GenerateKeyPair();
            string message = "The quick brown fox jumps over the lazy dog!";

            var (r, s) = ecdsa.Sign(message, privateKey);
            string signature = ecdsa.SerializeSignature(r, s);

            Console.WriteLine($"\nMessage: {message}");
            Console.WriteLine($"Public Key: {ecService.ECPointToString(publicKey)}");
            Console.WriteLine($"Signature: {signature}");
            
            bool isValid = ecdsa.Verify(message, r, s, publicKey);
            Console.WriteLine($"\nVerification result: {(isValid ? "SUCCESS" : "FAILED")}");            
        }
    }
}
