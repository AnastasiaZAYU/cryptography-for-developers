using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _SHA_1;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var _sha = new SHA_1();
            var str = "Hello World!";
            var expected = _sha.CalculateSHA1(str, false);
            var actual = _sha.CalculateSHA1(str, true);

            Assert.AreEqual(_sha.GetHex(expected), _sha.GetHex(actual));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var _sha = new SHA_1();
            var str = "Secure Hash Algorithm 1 — алгоритм криптографічного хешування.";
            var expected = _sha.CalculateSHA1(str, false);
            var actual = _sha.CalculateSHA1(str, true);

            Assert.AreEqual(_sha.GetHex(expected), _sha.GetHex(actual));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var _sha = new SHA_1();

            var sr = File.OpenText("text.txt");
            var expected = _sha.CalculateSHA1(sr, false);
            sr.Close();

            sr = File.OpenText("text.txt");
            var actual = _sha.CalculateSHA1(sr, true);
            sr.Close();

            Assert.AreEqual(_sha.GetHex(expected), _sha.GetHex(actual));
        }
    }
}
