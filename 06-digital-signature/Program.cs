using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Digital_Signature
{
    class Program
    {
        static void Main(string[] args)
        {
            ECDSA ecdsa = new ECDSA();
            bool prog = true;
            int r = 0;
            string message;
            AsymmetricKeyParameter sk, pk;
            while (prog == true)
            {
                Console.WriteLine("Натиснiть “0” - щоб згенерувати пару ключiв;" +
                    "\nнатиснiть “1” - щоб отримати геш повiдомлення;" +
                    "\nнатиснiть “2” - щоб пiдписати повiдомлення;" +
                    "\nнатиснiть “3” - щоб перевiрити пiдпис повiдомлення;" +
                    "\nнатиснiсть будь-який iнший символ - щоб завершити роботу.");
                r = int.Parse(Console.ReadLine());
                switch (r)
                {
                    case 0:
                        (sk, pk) = ecdsa.KeyGen();
                        Console.WriteLine(ecdsa.ConvertToPEM(sk));
                        Console.WriteLine(ecdsa.ConvertToPEM(pk));
                        break;
                    case 1:
                        Console.Write("Введiть повiдомлення: ");
                        message = Console.ReadLine();
                        var hash = ecdsa.Hash(message);
                        Console.WriteLine("Геш: {0}", Convert.ToBase64String(hash));
                        break;
                    case 2:
                        Console.Write("Введiть повiдомлення: ");
                        message = Console.ReadLine();
                        Console.Write("Введiть приватний ключ: ");
                        sk = ecdsa.InputKey();
                        var signature = ecdsa.Sign(message, sk);
                        Console.WriteLine("Signature: {0}", Convert.ToBase64String(signature));
                        break;
                    case 3:
                        Console.Write("Введiть повiдомлення: ");
                        message = Console.ReadLine();
                        Console.Write("Введiть вiдкритий ключ: ");
                        pk = ecdsa.InputKey();
                        Console.Write("Введiть пiдпис: ");
                        signature = Convert.FromBase64String(Console.ReadLine());
                        var valid = ecdsa.Verify(message, signature, pk);
                        Console.WriteLine(valid.ToString());
                        break;
                    default:
                        prog = false;
                        break;
                }
            }
        }
    }
}
