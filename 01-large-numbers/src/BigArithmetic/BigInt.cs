using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Numerics;

namespace BigArithmetic
{
    public class BigInt
    {
        private readonly uint[] _data;

        public BigInt(uint[] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public BigInt() : this([0]) { }

        // Implementation of setHex and getHex methods

        public static BigInt FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return new BigInt();

            ReadOnlySpan<char> hexSpan = hex.AsSpan().TrimStart('0');
            if (hexSpan.Length == 0)
                return new BigInt();

            int blockSize = 8;
            int numBlocks = (hexSpan.Length + blockSize - 1) / blockSize;
            uint[] data = new uint[numBlocks];

            for (int i = 0; i < numBlocks; i++)
            {
                int end = hexSpan.Length - i * blockSize;
                int start = Math.Max(0, end - blockSize);
                data[i] = uint.Parse(hexSpan.Slice(start, end - start), NumberStyles.HexNumber);
            }
            return new BigInt(data);
        }

        public string ToHex()
        {
            if (IsZero(this))
                return "0";

            StringBuilder sb = new();
            for (int i = _data.Length - 1; i >= 0; i--)
            {
                string format = (i == _data.Length - 1) ? "X" : "X8";
                sb.Append(_data[i].ToString(format));
            }
            return sb.ToString();
        }

        // Implementation of bitwise operations

        public static BigInt operator ^(BigInt a, BigInt b) => BitwiseOp(a, b, (x, y) => x ^ y);
        public static BigInt operator |(BigInt a, BigInt b) => BitwiseOp(a, b, (x, y) => x | y);
        public static BigInt operator &(BigInt a, BigInt b) => BitwiseOp(a, b, (x, y) => x & y);
        public static BigInt operator ~(BigInt a)
        {
            uint[] resultData = new uint[a._data.Length];
            for (int i = 0; i < a._data.Length; i++)
            {
                resultData[i] = ~a._data[i];
            }
            return Trim(new BigInt(resultData));
        }

        private static BigInt BitwiseOp(BigInt a, BigInt b, Func<uint, uint, uint> op)
        {
            int maxLength = Math.Max(a._data.Length, b._data.Length);
            uint[] resultData = new uint[maxLength];
            for (int i = 0; i < maxLength; i++)
            {
                uint aVal = i < a._data.Length ? a._data[i] : 0;
                uint bVal = i < b._data.Length ? b._data[i] : 0;
                resultData[i] = op(aVal, bVal);
            }
            return Trim(new BigInt(resultData));
        }

        public static BigInt operator <<(BigInt a, int count)
        {
            if (count == 0 || IsZero(a))
                return a;
            int blockShift = count / 32;
            int bitShift = count % 32;

            uint[] resultData = new uint[a._data.Length + blockShift + 1];
            for (int i = 0; i < a._data.Length; i++)
            {
                ulong shifted = (ulong)a._data[i] << bitShift;
                resultData[i + blockShift] |= (uint)(shifted & uint.MaxValue);
                resultData[i + blockShift + 1] |= (uint)(shifted >> 32);
            }
            return Trim(new BigInt(resultData));
        }

        public static BigInt operator >>(BigInt a, int count)
        {
            if (count == 0 || IsZero(a))
                return a;
            int blockShift = count / 32;
            int bitShift = count % 32;
            if (blockShift >= a._data.Length)
                return new BigInt();

            uint[] resultData = new uint[a._data.Length - blockShift];
            for (int i = blockShift; i < a._data.Length; i++)
            {
                resultData[i - blockShift] = a._data[i] >> bitShift;
                if (bitShift > 0 && i + 1 < a._data.Length)
                {
                    resultData[i - blockShift] |= a._data[i + 1] << (32 - bitShift);
                }
            }
            return Trim(new BigInt(resultData));
        }

        // Implementation of arithmetic operations

        public static BigInt operator +(BigInt a, BigInt b)
        {
            int maxLength = Math.Max(a._data.Length, b._data.Length);
            uint[] resultData = new uint[maxLength + 1];
            ulong carry = 0;

            for (int i = 0; i < maxLength; i++)
            {
                ulong aVal = i < a._data.Length ? a._data[i] : 0;
                ulong bVal = i < b._data.Length ? b._data[i] : 0;
                ulong sum = aVal + bVal + carry;
                resultData[i] = (uint)(sum);
                carry = sum >> 32;
            }
            resultData[maxLength] = (uint)carry;
            return Trim(new BigInt(resultData));
        }

        public static BigInt operator -(BigInt a, BigInt b)
        {
            if (Compare(a, b) < 0)
                throw new InvalidOperationException("Subtraction would result in a negative value.");

            uint[] resultData = new uint[a._data.Length];
            long borrow = 0;

            for (int i = 0; i < a._data.Length; i++)
            {
                long aVal = a._data[i];
                long bVal = i < b._data.Length ? b._data[i] : 0;
                long sub = aVal - bVal - borrow;

                if (sub < 0)
                {
                    sub += 0x100000000L;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                resultData[i] = (uint)sub;
            }
            return Trim(new BigInt(resultData));
        }

        public static BigInt operator *(BigInt a, BigInt b)
        {
            if (IsZero(a) || IsZero(b))
                return new BigInt();
            uint[] resultData = new uint[a._data.Length + b._data.Length];

            for (int i = 0; i < a._data.Length; i++)
            {
                ulong carry = 0;
                for (int j = 0; j < b._data.Length; j++)
                {
                    ulong product = (ulong)a._data[i] * b._data[j] + resultData[i + j] + carry;
                    resultData[i + j] = (uint)(product);
                    carry = product >> 32;
                }
                resultData[i + b._data.Length] = (uint)carry;
            }
            return Trim(new BigInt(resultData));
        }

        public static (BigInt Quotient, BigInt Remainder) DivMod(BigInt a, BigInt b)
        {
            if (IsZero(b))
                throw new DivideByZeroException();
            if (Compare(a, b) < 0)
                return (new BigInt(), a);

            BigInt quotient = new BigInt();
            BigInt remainder = new BigInt();

            int bitCount = a._data.Length * 32;
            for (int i = bitCount; i >= 0; i--)
            {
                remainder <<= 1;
                if (GetBit(a, i))
                    remainder._data[0] |= 1;
                if (Compare(remainder, b) >= 0)
                {
                    remainder -= b;
                    quotient = SetBit(quotient, i);
                }
            }
            return (Trim(quotient), Trim(remainder));
        }

        public static BigInt operator /(BigInt a, BigInt b) => DivMod(a, b).Quotient;

        public static BigInt operator %(BigInt a, BigInt b) => DivMod(a, b).Remainder;

        public static BigInt PowMod(BigInt @base, BigInt exponent, BigInt modulus)
        {
            if (IsZero(modulus))
                throw new DivideByZeroException();

            BigInt result = FromHex("1");
            BigInt b = @base % modulus;
            BigInt e = exponent;

            while (!IsZero(e))
            {
                if (GetBit(e, 0))
                    result = (result * b) % modulus;
                b = (b * b) % modulus;
                e >>= 1;
            }
            return result;
        }

        // Implementation of helper methods

        public static int Compare(BigInt a, BigInt b)
        {
            if (a._data.Length != b._data.Length)
                return a._data.Length.CompareTo(b._data.Length);
            for (int i = a._data.Length - 1; i >= 0; i--)
            {
                if (a._data[i] != b._data[i])
                    return a._data[i].CompareTo(b._data[i]);
            }
            return 0;
        }

        private static BigInt Trim(BigInt a)
        {
            int lastNonZeroIndex = a._data.Length - 1;
            while (lastNonZeroIndex > 0 && a._data[lastNonZeroIndex] == 0)
            {
                lastNonZeroIndex--;
            }
            if (lastNonZeroIndex == a._data.Length - 1)
                return a;
            uint[] trimmed = new uint[lastNonZeroIndex + 1];
            Array.Copy(a._data, trimmed, lastNonZeroIndex + 1);
            return new BigInt(trimmed);
        }

        public static bool IsZero(BigInt a) => a._data.Length == 0 || (a._data.Length == 1 && a._data[0] == 0);

        private static bool GetBit(BigInt a, int bitIndex)
        {
            int blockIndex = bitIndex / 32;
            if (blockIndex >= a._data.Length)
                return false;
            uint bitMask = 1u << (bitIndex % 32);
            return (a._data[blockIndex] & bitMask) != 0;
        }

        private static BigInt SetBit(BigInt a, int bitIndex)
        {
            int blockIndex = bitIndex / 32;
            uint[] data = a._data;
            if (blockIndex >= data.Length)
            {
                Array.Resize(ref data, blockIndex + 1);
            }
            data[blockIndex] |= 1u << (bitIndex % 32);
            return new BigInt(data);
        }
    }
}
