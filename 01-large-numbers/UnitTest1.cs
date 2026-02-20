using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigNumbers;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestINV()
        {
            var _big = new BigNumber();
            var a = _big.setHex("70983d692f648185febe6d6fa607630ae68649f7e6fc45b94680096c06e4fadb");
            var actual = _big.INV(a);
            var expected = "f67c296d09b7e7a0141929059f89cf51979b6081903ba46b97ff693f91b0524";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestXOR()
        {
            var _big = new BigNumber();
            var a = _big.setHex("51bf608414ad5726a3c1bec098f77b1b54ffb2787f8d528a74c1d7fde6470ea4");
            var b = _big.setHex("403db8ad88a3932a0b7e8189aed9eeffb8121dfac05c3512fdb396dd73f6331c");
            var actual = _big.XOR(a, b);
            var expected = "1182d8299c0ec40ca8bf3f49362e95e4ecedaf82bfd167988972412095b13db8";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestOR()
        {
            var _big = new BigNumber();
            var a = _big.setHex("51bf608414ad5726a3c1bec098f77b1b54ffb2787f8d528a74c1d7fde6470ea4");
            var b = _big.setHex("403db8ad88a3932a0b7e8189aed9eeffb8121dfac05c3512fdb396dd73f6331c");
            var actual = _big.OR(a, b);
            var expected = "51bff8ad9cafd72eabffbfc9befffffffcffbffaffdd779afdf3d7fdf7f73fbc";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestAND()
        {
            var _big = new BigNumber();
            var a = _big.setHex("51bf608414ad5726a3c1bec098f77b1b54ffb2787f8d528a74c1d7fde6470ea4");
            var b = _big.setHex("403db8ad88a3932a0b7e8189aed9eeffb8121dfac05c3512fdb396dd73f6331c");
            var actual = _big.AND(a, b);
            var expected = "403d208400a113220340808088d16a1b10121078400c1002748196dd62460204";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestADD()
        {
            var _big = new BigNumber();
            var a = _big.setHex("36f028580bb02cc8272a9a020f4200e346e276ae664e45ee80745574e2f5ab80");
            var b = _big.setHex("70983d692f648185febe6d6fa607630ae68649f7e6fc45b94680096c06e4fadb");
            var actual = _big.ADD(a, b);
            var expected = "a78865c13b14ae4e25e90771b54963ee2d68c0a64d4a8ba7c6f45ee0e9daa65b";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestSUB()
        {
            var _big = new BigNumber();
            var a = _big.setHex("33ced2c76b26cae94e162c4c0d2c0ff7c13094b0185a3c122e732d5ba77efebc");
            var b = _big.setHex("22e962951cb6cd2ce279ab0e2095825c141d48ef3ca9dabf253e38760b57fe03");
            var actual = _big.SUB(a, b);
            var expected = "10e570324e6ffdbc6b9c813dec968d9bad134bc0dbb061530934f4e59c2700b9";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestMOD()
        {
            var _big = new BigNumber();
            var a = _big.setHex("14545fb14964c0c62e5d79272fd2fc69ff0faa54b8ced63fc9cf302864d55761ddc49dd7ab851edcd908ec803b8fa40f70d2f5ac257a9a3702bc340a36f701ac2");
            var b = _big.setHex("38cdd88155e5e68a2b66fc28861fb57657e27a1d41d3e61730fab712fb0e55728443d1a18c27de41a5c3caafe43de9484f48d282f29f8505f4bdf734d492b484");
            var actual = _big.MOD(a, b);
            var expected = "2940c08de8ce8bb00cd4a5a85e913b50398e42b943c9e588a80d6f27660dcae146f6c552fb8a968553bbd29443c4b28d80c33e339a8c0a52640d6c9b4892942e";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestMUL()
        {
            var _big = new BigNumber();
            var a = _big.setHex("7d7deab2affa38154326e96d350deee1");
            var b = _big.setHex("97f92a75b3faf8939e8e98b96476fd22");
            var actual = _big.MUL(a, b);
            var expected = "4a7f69b908e167eb0dc9af7bbaa5456039c38359e4de4f169ca10c44d0a416e2";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestDIV()
        {
            var _big = new BigNumber();
            var a = _big.setHex("D4D2110984907B5625309D956521BAB4157B8B1ECE04043249A3D379AC112E5B9AF44E721E148D88A942744CF56A06B92D28A0DB950FE4CED2B41A0BD38BCE7D0BE1055CF5DE38F2A588C2C9A79A75011058C320A7B661C6CE1C36C7D870758307E5D2CF07D9B6E8D529779B6B2910DD17B6766A7EFEE215A98CAC300F2827DB");
            var b = _big.setHex("3A7EF2554E8940FA9B93B2A5E822CC7BB262F4A14159E4318CAE3ABF5AEB1022EC6D01DEFAB48B528868679D649B445A753684C13F6C3ADBAB059D635A2882090FC166EA9F0AAACD16A062149E4A0952F7FAAB14A0E9D3CB0BE9200DBD3B0342496421826919148E617AF1DB66978B1FCD28F8408506B79979CCBCC7F7E5FDE7");
            var actual = _big.DIV(a, b);
            var expected = "3";
            Assert.AreEqual(expected, _big.getHex(actual));
        }

        [TestMethod]
        public void TestPOWMOD()
        {
            var _big = new BigNumber();
            var a = _big.setHex("7d7deab2affa38154326e96d350deee1");
            var b = _big.setHex("a");
            var n = _big.setHex("97f92a75b3faf8939e8e98b96476fd22");
            var actual = _big.POWMOD(a, b, n);
            var expected = "9140c47be5548c844c880842fd583f2f";
            Assert.AreEqual(expected, _big.getHex(actual));
        }
    }
}
