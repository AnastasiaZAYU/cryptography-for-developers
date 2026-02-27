using KeyRandomnessValidator;

namespace Fips140.RandomnessTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();
            var validator = new FipsValidator();

            Console.WriteLine("========================================");
            Console.WriteLine("   FIPS-140 RANDOMNESS VALIDATOR    ");
            Console.WriteLine("========================================");

            Console.WriteLine("\n=== [1] Testing BBS Generator (Blum Blum Shub) ===");
            byte[] bbsKey = generator.GenerateKey(2500);
            Console.WriteLine($"Generated Key (first 16 bytes): {generator.ToHexString(bbsKey).Substring(0, 32)}...");
            PrintResult(validator.Validate(bbsKey));

            Console.WriteLine("\n=== [2] Testing Pattern Key ===");
            byte[] badKey = GenerateBadKey();
            Console.WriteLine($"Generated Key (first 16 bytes): {generator.ToHexString(badKey).Substring(0, 32)}...");
            PrintResult(validator.Validate(badKey));
        }

        static void PrintResult((bool IsValid, List<(string TestName, bool Passed)> Details) result)
        {
            if (result.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("RESULT: PASS - The key is valid according to FIPS 140.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("RESULT: FAIL - The key is NOT valid according to FIPS 140.");
            }
            Console.ResetColor();

            Console.WriteLine("Details:");
            foreach (var detail in result.Details)
            {
                Console.WriteLine($"  - {detail.TestName.PadRight(20)}: {(detail.Passed ? "OK" : "FAILED")}");
            }
        }

        static byte[] GenerateBadKey()
        {
            byte[] key = new byte[2500];
            for (int i = 0; i < key.Length; i++) 
                key[i] = 0x55;
            return key;
        }
    }
}
