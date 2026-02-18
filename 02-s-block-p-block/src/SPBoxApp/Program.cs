using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SPBoxApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fn = new Functions();
            byte[] message = Array.Empty<byte>();

            Console.WriteLine("========== Cryptography SP-Box Tool ==========" +
                "\n0 - Input message (HEX)" +
                "\n1 - Forward S-Box transformation" +
                "\n2 - Inverse S-Box transformation" +
                "\n3 - Forward P-Box transformation" +
                "\n4 - Inverse P-Box transformation" +
                "\nAny other key - Exit");

            while (true)
            {

                Console.Write("\nSelect option: ");
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    if (new[] { "1", "2", "3", "4" }.Contains(input) && message.Length == 0)
                        throw new InvalidOperationException("No message loaded. Please input a message first.");

                    switch (input)
                    {
                        case "0":
                            Console.Write("Enter HEX string: ");
                            message = fn.Input(Console.ReadLine());
                            fn.Print(message, "Loaded");
                            break;

                        case "1":
                            fn.S_box(message);
                            fn.Print(message, "S-Box Forward");
                            break;

                        case "2":
                            fn.S_box(message, inverse: true);
                            fn.Print(message, "S-Box Inverse");
                            break;

                        case "3":
                            fn.P_box(message);
                            fn.Print(message, "P-Box Forward");
                            break;

                        case "4":
                            fn.P_box(message, inverse: true);
                            fn.Print(message, "P-Box Inverse");
                            break;

                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
