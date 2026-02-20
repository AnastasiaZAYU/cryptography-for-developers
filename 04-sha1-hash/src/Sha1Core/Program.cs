using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Sha1Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("========= SHA-1 Hahsing Tool =========" +
                "\n1 - Hash text string" +
                "\n2 - Hash file" +
                "\nAny other key - Exit");

            while (true)
            {
                Console.Write("Select option: ");
                string input = Console.ReadLine() ?? string.Empty;

                try 
                {
                    if (input == "1")
                    {
                        Console.Write("Enter text: ");
                        string text = Console.ReadLine() ?? string.Empty;
                        byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
                        byte[] hash = Sha1Hasher.HashData(data);
                        Console.WriteLine($"SHA-1 Hash: {Sha1Hasher.ToHexString(hash)}");
                    }
                    else if (input == "2")
                    {
                        Console.Write("Enter file path: ");
                        string filePath = Console.ReadLine() ?? string.Empty;
                        if (!File.Exists(filePath))
                            throw new FileNotFoundException("File not found!");
                        byte[] hash = Sha1Hasher.HashFile(filePath);
                        Console.WriteLine($"SHA-1 Hash: {Sha1Hasher.ToHexString(hash)}");
                    }
                    else
                    {
                        break;
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
