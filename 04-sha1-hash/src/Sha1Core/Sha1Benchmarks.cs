using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Order;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace Sha1Core
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class Sha1Benchmarks
    {
        private byte[] _smallData;
        private byte[] _largeData;
        
        [GlobalSetup]
        public void Setup()
        {
            _smallData = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");
            _largeData = new byte[10 * 1024 * 1024]; // 10 MB of random bytes
            Random.Shared.NextBytes(_largeData);
        }

        // --- Small Data Tests ---
        [Benchmark(Description = "Library SHA-1 (Small)")]
        public byte[] LibrarySmall() => SHA1.HashData(_smallData);

        [Benchmark(Description = "Custom SHA-1 (Small)")]
        public byte[] CustomSmall() => Sha1Hasher.HashData(_smallData);

        // --- Large Data Tests ---
        [Benchmark(Description = "Library SHA-1 (10 MB)")]
        public byte[] LibraryLarge() => SHA1.HashData(_largeData);
            
        [Benchmark(Description = "Custom SHA-1 (10 MB)")]
        public byte[] CustomLarge() => Sha1Hasher.HashData(_largeData);
    }
}
