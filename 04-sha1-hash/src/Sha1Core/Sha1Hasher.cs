using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Formats.Asn1;
using System.Numerics;

namespace Sha1Core
{
    public class Sha1Hasher
    {
        private uint[] _h = new uint[5];

        private readonly byte[] _buffer = new byte[64];
        private int _bufferLength = 0;
        private long _totalLength = 0;

        public Sha1Hasher() => Reset();

        // Static helper method for hashing data in one go
        public static byte[] HashData(ReadOnlySpan<byte> data)
        {
            var hasher = new Sha1Hasher();
            hasher.AppendData(data);
            return hasher.GetHashAndReset();
        }

        // Static helper method for hashing a file
        public static byte[] HashFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            var hasher = new Sha1Hasher();
            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hasher.AppendData(buffer.AsSpan(0, bytesRead));
            }
            return hasher.GetHashAndReset();
        }

        public static string ToHexString(byte[] hash) => Convert.ToHexString(hash).ToLower();

        public void Reset()
        {
            _h[0] = 0x67452301;
            _h[1] = 0xEFCDAB89;
            _h[2] = 0x98BADCFE;
            _h[3] = 0x10325476;
            _h[4] = 0xC3D2E1F0;
            _bufferLength = 0;
            _totalLength = 0;
        }

        public void AppendData(ReadOnlySpan<byte> data)
        {
            _totalLength += data.Length;

            while (data.Length > 0)
            {
                int toCopy = Math.Min(64 - _bufferLength, data.Length);
                data[..toCopy].CopyTo(_buffer.AsSpan(_bufferLength));
                _bufferLength += toCopy;
                data = data[toCopy..];

                if (_bufferLength == 64)
                {
                    ProcessBlock(_buffer);
                    _bufferLength = 0;
                }
            }
        }

        public byte[] GetHashAndReset()
        {
            PadMessage();

            byte[] hash = new byte[20];
            for (int i = 0; i < 5; i++)
            {
                BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(i * 4), _h[i]);
            }
            Reset();
            return hash;
        }

        private void ProcessBlock(ReadOnlySpan<byte> block)
        {
            Span<uint> w = stackalloc uint[80];

            for (int t = 0; t < 16; t++)
            {
                w[t] = BinaryPrimitives.ReadUInt32BigEndian(block.Slice(t * 4));
            }
            for (int t = 16; t < 80; t++)
            {
                w[t] = BitOperations.RotateLeft(w[t - 3] ^ w[t - 8] ^ w[t - 14] ^ w[t - 16], 1);
            }

            uint a = _h[0];
            uint b = _h[1];
            uint c = _h[2];
            uint d = _h[3];
            uint e = _h[4];

            for (int i = 0; i < 80; i++)
            {
                uint f, k;
                if (i < 20)
                {
                    f = (b & c) | ((~b) & d);
                    k = 0x5A827999;
                }
                else if (i < 40)
                {
                    f = b ^ c ^ d;
                    k = 0x6ED9EBA1;
                }
                else if (i < 60)
                {
                    f = (b & c) | (b & d) | (c & d);
                    k = 0x8F1BBCDC;
                }
                else
                {
                    f = b ^ c ^ d;
                    k = 0xCA62C1D6;
                }

                uint temp = BitOperations.RotateLeft(a, 5) + f + e + k + w[i];
                e = d;
                d = c;
                c = BitOperations.RotateLeft(b, 30);
                b = a;
                a = temp;
            }
            _h[0] += a;
            _h[1] += b;
            _h[2] += c;
            _h[3] += d;
            _h[4] += e;
        }

        private void PadMessage()
        {
            long bitLength = _totalLength * 8;
            Span<byte> firstPadByte = stackalloc byte[1] { 0x80 };
            AppendData(firstPadByte);
            while (_bufferLength != 56)
            {
                AppendData(stackalloc byte[1] { 0x00 });
            }
            Span<byte> lengthBytes = stackalloc byte[8];
            BinaryPrimitives.WriteInt64BigEndian(lengthBytes, bitLength);
            AppendData(lengthBytes);
        }
    }
}
