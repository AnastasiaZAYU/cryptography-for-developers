using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boxes;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_S_box()
        {
            Random rnd = new Random();
            byte[] randomBytes = new byte[16];
            rnd.NextBytes(randomBytes);
            var expected = BitConverter.ToString(randomBytes).Replace("-", "").ToLower(); //створення довільного HEX-повідомлення довжини 32

            var _f = new Functions();
            var a = _f.S_box(_f.Input(expected)); //застосування прямого перетворення з S-блоком
            var actual = _f.S_box_inv(a); //застосування зворотного перетворення з S-блоком
            Assert.AreEqual(expected, _f.Output(actual)); //порівняння отрисаного набору даних з початковим
        }

        [TestMethod]
        public void Test_P_box()
        {
            Random rnd = new Random();
            byte[] randomBytes = new byte[16];
            rnd.NextBytes(randomBytes);
            var expected = BitConverter.ToString(randomBytes).Replace("-", "").ToLower(); //створення довільного HEX-повідомлення довжини 32

            var _f = new Functions();
            var a = _f.P_box(_f.Input(expected)); //застосування прямого перетворення з P-блоком
            var actual = _f.P_box_inv(a); //застосування зворотного перетворення з P-блоком
            Assert.AreEqual(expected, _f.Output(actual)); //порівняння отрисаного набору даних з початковим
        }

        [TestMethod]
        public void Test_SP_box()
        {
            Random rnd = new Random();
            byte[] randomBytes = new byte[16];
            rnd.NextBytes(randomBytes);
            var expected = BitConverter.ToString(randomBytes).Replace("-", "").ToLower(); //створення довільного HEX-повідомлення довжини 32

            var _f = new Functions();
            var a = _f.S_box(_f.Input(expected)); //застосування прямого перетворення з S-блоком
            a = _f.P_box(a); //застосування прямого перетворення з P-блоком
            a = _f.P_box_inv(a); //застосування зворотного перетворення з P-блоком
            var actual = _f.S_box_inv(a); //застосування зворотного перетворення з S-блоком
            Assert.AreEqual(expected, _f.Output(actual)); //порівняння отрисаного набору даних з початковим
        }

        [TestMethod]
        public void Test_PS_box()
        {
            Random rnd = new Random();
            byte[] randomBytes = new byte[16];
            rnd.NextBytes(randomBytes);
            var expected = BitConverter.ToString(randomBytes).Replace("-", "").ToLower(); //створення довільного HEX-повідомлення довжини 32

            var _f = new Functions();
            var a = _f.P_box(_f.Input(expected)); //застосування прямого перетворення з P-блоком
            a = _f.S_box(a); //застосування прямого перетворення з S-блоком
            a = _f.S_box_inv(a); //застосування зворотного перетворення з S-блоком
            var actual = _f.P_box_inv(a); //застосування зворотного перетворення з P-блоком
            Assert.AreEqual(expected, _f.Output(actual)); //порівняння отрисаного набору даних з початковим
        }
    }
}
