using System.Numerics;

namespace EllipticCurveWrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Elliptic Curve Cryptography Wrapper ===");
            var ecc = new BouncyCastleWrapper("secp256k1");
            var G = ecc.BasePointGGet();

            Console.WriteLine("\nElliptic Curve: secp256k1");
            Console.WriteLine($"Generator point G:\n{G}");
        }
    }
}
