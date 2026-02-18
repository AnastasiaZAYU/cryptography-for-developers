using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace SPBoxApp
{
    public class Functions
    {
        // Substitution tables (S-Boxes) for high and low nibbles (4 bits each)
        private readonly byte[] _s0 = new byte[16];
        private readonly byte[] _s1 = new byte[16];
        // Permutation table (P-Box) for 8-bit block
        private readonly byte[] _pBox = new byte[8];

        //Inverse lookup tables for decryption
        private readonly byte[] _s0Inv = new byte[16];
        private readonly byte[] _s1Inv = new byte[16];
        private readonly byte[] _pBoxInv = new byte[8];

        public Functions()
        {
            // Initialize and shuffle transformation tables
            InitializeTables(_s0);
            InitializeTables(_s1);
            InitializeTables(_pBox);

            // Generate inverse tables for reverse transformations
            CreateInverseTable(_s0,_s0Inv);
            CreateInverseTable(_s1, _s1Inv);
            CreateInverseTable(_pBox, _pBoxInv);
        }

        private void InitializeTables(byte[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = (byte)i;
            }
            // Fisher-Yates shuffle algorithm for unbiased permutation
            for (int i = table.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (table[i], table[j]) = (table[j], table[i]);
            }
        }

        private void CreateInverseTable(byte[] source, byte[] destination)
        {
            for (int i = 0; i < source.Length; i++)
            {
                destination[source[i]] = (byte)i;
            }
        }

        public byte[] Input(string? hex)
        { 
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Input cannot be empty. Please enter a valid HEX string.");
            return Convert.FromHexString(hex);
        }

        public void Print(ReadOnlySpan<byte> data, string label = "State") =>
            Console.WriteLine($"{label}: {Convert.ToHexString(data)}");
        
        public void S_box(Span<byte> data, bool inverse = false)
        {
            ReadOnlySpan<byte> s0Table = inverse ? _s0Inv : _s0;
            ReadOnlySpan<byte> s1Table = inverse ? _s1Inv : _s1;

            for (int i = 0; i < data.Length; i++)
            {
                byte b = data[i];
                // Hign nibble (bits 7-4) and low nibble (bits 3-0) substitution
                data[i] = (byte)((s0Table[b >> 4] << 4) | s1Table[b & 0x0F]);
            }
        }

        public void P_box(Span<byte> data, bool inverse = false)
        {
            ReadOnlySpan<byte> currentPBox = inverse ? _pBoxInv : _pBox;

            for (int i = 0; i < data.Length; i ++)
            {
                byte b = data[i];
                byte shuffled = 0;

                // Bit-by-bit permutation logic
                for (int j = 0; j < 8; j++)
                {
                    // Extract the bit at specified position and shift it to new position
                    int bit = (b >> (7 - currentPBox[j])) & 1;
                    shuffled |= (byte)(bit << (7 - j));
                }
                data[i] = shuffled;
            }
        }
    }
}
