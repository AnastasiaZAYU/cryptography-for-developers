using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace _SHA_1
{
    public class SHA_1
    {
        private List<uint> h;

        public void new_H()
        {
            h = new List<uint> { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476, 0xC3D2E1F0 };
        }

        public byte[] CalculateSHA1(string input, bool flag)
        {
            byte[] arr = Encoding.UTF8.GetBytes(input);
            if (flag)
            {
                new_H();
                return my_SHA1(arr);
            }
            return library_SHA1(arr);
        }

        public byte[] CalculateSHA1(StreamReader sr, bool flag)
        {
            var str = sr.ReadToEnd();
            byte[] arr = Encoding.UTF8.GetBytes(str);
            if (flag)
            {
                new_H();
                return my_SHA1(arr);
            }
            return library_SHA1(arr);
        }

        public byte[] CalculateSHA1(BinaryReader br, long len, bool flag)
        {
            var arr = br.ReadBytes((int)len);
            if (flag)
            {
                new_H();
                return my_SHA1(arr);
            }
            return library_SHA1(arr);
        }

        private byte[] my_SHA1(byte[] arr)
        {
            var message = Padding(arr);
            for (int i = 0; i < message.Count; i += 64)
                SHA(message.GetRange(i, 64).ToArray());

            byte[] hash = new byte[20];
            for (int i = 0; i < h.Count; i++)
            {
                var bytes = BitConverter.GetBytes(h[i]);
                Array.Reverse(bytes);
                Array.Copy(bytes, 0, hash, 4 * i, 4);
            }
            return hash;
        }
        
        private void SHA(byte[] list)
        {
            uint[] w = new uint[80];
            for (int t = 0; t < 80; t++)
            {
                if (t < 16)
                {
                    Array.Reverse(list, t * 4, 4);
                    w[t] = BitConverter.ToUInt32(list, t * 4);
                }
                else
                    w[t] = ShiftLeft(w[t - 3] ^ w[t - 8] ^ w[t - 14] ^ w[t - 16], 1);
            }

            var a = h[0];
            var b = h[1];
            var c = h[2];
            var d = h[3];
            var e = h[4];

            uint temp;
            for (int i = 0; i < 80; i++)
            {
                temp = ShiftLeft(a, 5) + F(i, b, c, d) + e + w[i];
                e = d;
                d = c;
                c = ShiftLeft(b, 30);
                b = a;
                a = temp;
            }

            h[0] += a;
            h[1] += b;
            h[2] += c;
            h[3] += d;
            h[4] += e;
        }

        private uint ShiftLeft(uint word, int n)
        {
            return (word << n) | (word >> (32 - n));
        }

        private uint F(int t, uint m, uint l, uint k)
        {
            if (t < 20)
                return ((m & l) | ((~m) & k)) + 0x5A827999;
            else if (t < 40)
                return (m ^ l ^ k) + 0x6ED9EBA1;
            else if (t < 60)
                return ((m & l) | (m & k) | (l & k)) + 0x8F1BBCDC;
            else
                return (m ^ l ^ k) + 0xCA62C1D6;
        }

        private List<byte> Padding(byte[] array)
        {
            byte[] len = BitConverter.GetBytes((ulong)array.Length * 8);
            Array.Reverse(len);
            List<byte> padbyte = new List<byte>(array);
            padbyte.Add(0x80);
            while (padbyte.Count % 64 != 56)
                padbyte.Add(0x0);
            padbyte.AddRange(len);
            return padbyte;
        }

        private byte[] library_SHA1(byte[] arr)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] hash = sha1.ComputeHash(arr);
                return hash;
            }
        }

        public string GetHex(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
