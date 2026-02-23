namespace BigArithmetic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = BigInt.FromHex("51bf608414ad5726a3c1bec098f77b1b54ffb2787f8d528a74c1d7fde6470ea4");
            var b = BigInt.FromHex("403db8ad88a3932a0b7e8189aed9eeffb8121dfac05c3512fdb396dd73f6331c");
            Console.WriteLine($"A: {a.ToHex()}\nB: {b.ToHex()}");

            ExecuteSafe("A XOR B", () => Console.WriteLine((a ^ b).ToHex()));

            a = BigInt.FromHex("36f028580bb02cc8272a9a020f4200e346e276ae664e45ee80745574e2f5ab80");
            b = BigInt.FromHex("70983d692f648185febe6d6fa607630ae68649f7e6fc45b94680096c06e4fadb");
            Console.WriteLine($"A: {a.ToHex()}\nB: {b.ToHex()}");

            ExecuteSafe("A + B", () => Console.WriteLine((a + b).ToHex()));

            a = BigInt.FromHex("33ced2c76b26cae94e162c4c0d2c0ff7c13094b0185a3c122e732d5ba77efebc");
            b = BigInt.FromHex("22e962951cb6cd2ce279ab0e2095825c141d48ef3ca9dabf253e38760b57fe03");
            Console.WriteLine($"A: {a.ToHex()}\nB: {b.ToHex()}");

            ExecuteSafe("A - B", () => Console.WriteLine((a - b).ToHex()));

            ExecuteSafe("B - A", () => Console.WriteLine((b - a).ToHex()));

            a = BigInt.FromHex("7d7deab2affa38154326e96d350deee1");
            b = BigInt.FromHex("97f92a75b3faf8939e8e98b96476fd22");
            Console.WriteLine($"A: {a.ToHex()}\nB: {b.ToHex()}");

            ExecuteSafe("A * B", () => Console.WriteLine((a * b).ToHex()));
        }

        static void ExecuteSafe(string label, Action action)
        {
            Console.Write($"{label} = ");
            try
            {
                action();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[!] Error: {ex.Message}\n");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
