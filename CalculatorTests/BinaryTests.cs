using Microsoft.VisualStudio.TestTools.UnitTesting;
using Laba1AOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Laba1AOIS.Tests
{
    [TestClass()]
    public class BinaryTests
    {
        [TestMethod()]
        public void BinaryTest()
        {
            
            var binary1 = new Binary(10);
            //var binary2 = new Binary(2);
            List<int> expected = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0 };

            // Act
            List<int> actual = binary1.num;

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DecimalToStraightCodeTest()
        {

            int decimalNumber = -10;
            List<int> expected = new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0 };
            List<int> actual = Binary.DecimalToStraightCode(decimalNumber);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StraightToReverseTest()
        {
            int decimalNumber = -10;
            List<int> onesComplement1 = Binary.DecimalToStraightCode(decimalNumber);
            List<int> expected = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1 };
            List<int> actual = Binary.StraightToReverse(onesComplement1);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReverseToComplementCodeTest()
        {
            int decimalNumber = -10;
            List<int> onesComplement1 = Binary.DecimalToStraightCode(decimalNumber);
            List<int> twosComplement1 = Binary.StraightToReverse(onesComplement1);
            List<int> expected = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0 };
            List<int> actual = Binary.ReverseToComplementCode(twosComplement1);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SumOfComplementTest()
        {
            int decimalNumber1 = -10;
            int decimalNumber2 = 2;
            List<int> straightCode1 = Binary.DecimalToStraightCode(decimalNumber1);
            List<int> onesComplement1 = Binary.StraightToReverse(straightCode1);
            List<int> twosComplement1 = Binary.ReverseToComplementCode(onesComplement1);

            List<int> straightCode2 = Binary.DecimalToStraightCode(decimalNumber2);
            List<int> onesComplement2 = Binary.StraightToReverse(straightCode2);
            List<int> twosComplement2 = Binary.ReverseToComplementCode(onesComplement2);

            List<int> expected = new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 };
            List<int> actual = Binary.SumOfComplement(twosComplement1,twosComplement2);
            CollectionAssert.AreEqual(expected, actual);



        }

        [TestMethod()]
        public void MultiplicationOfStraightTest()
        {
            int decimalNumber1 = 5;
            int decimalNumber2 = 3;
            List<int> straightCode1 = Binary.DecimalToStraightCode(decimalNumber1);
            List<int> onesComplement1 = Binary.StraightToReverse(straightCode1);
            List<int> twosComplement1 = Binary.ReverseToComplementCode(onesComplement1);

            List<int> straightCode2 = Binary.DecimalToStraightCode(decimalNumber2);
            List<int> onesComplement2 = Binary.StraightToReverse(straightCode2);
            List<int> twosComplement2 = Binary.ReverseToComplementCode(onesComplement2);

            List<int> expected = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };
            List<int> actual = Binary.MultiplicationOfStraight(twosComplement1, twosComplement2);
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void DivideTest()
        {
            int decimalNumber1 = 10;
            int decimalNumber2 = 2;
            List<int> straightCode1 = Binary.DecimalToStraightCode(decimalNumber1);

            List<int> straightCode2 = Binary.DecimalToStraightCode(decimalNumber2);
            string expectedIntPart = "0101";
            string expectedFloatPart = "0";
           

           var resultDivide = Binary.Divide(straightCode1, straightCode2);
            Assert.AreEqual(expectedIntPart, resultDivide.IntegerPart);
            Assert.AreEqual(expectedFloatPart, resultDivide.FractionalPart);
        }

        [TestMethod()]
        public void DivideTest2()
        {
            int decimalNumber1 = 7;
            int decimalNumber2 = 2;
            List<int> straightCode1 = Binary.DecimalToStraightCode(decimalNumber1);

            List<int> straightCode2 = Binary.DecimalToStraightCode(decimalNumber2);
            string expectedIntPart = "011";
            string expectedFloatPart = "1";


            var resultDivide = Binary.Divide(straightCode1, straightCode2);
            Assert.AreEqual(expectedIntPart, resultDivide.IntegerPart);
            Assert.AreEqual(expectedFloatPart, resultDivide.FractionalPart);
        }
    }
}