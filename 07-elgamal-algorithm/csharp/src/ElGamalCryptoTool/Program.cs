namespace ElGamalCryptoTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new ElGamalService();
            var paramsResult = service.GenerateParameters(1024);

            Console.WriteLine($"P: {paramsResult.P.ToString("X")}");
            Console.WriteLine($"G: {paramsResult.G.ToString("X")}");

            var keyPair = service.GenerateKeyPair(paramsResult);

            Console.WriteLine($"Private Key: {keyPair.PrivateKey.ToString("X")}");
            Console.WriteLine($"Public Key: {keyPair.PublicKey.ToString("X")}");
        }
    }
}
