using Microsoft.VisualStudio.TestTools.UnitTesting;
using Laba1AOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1AOIS.Tests
{
    [TestClass()]
    public class FloatingPointTests
    {
        [TestMethod()]
        public void FloatingPointTest()
        {
            float num = 7.25f;
            int expectedSign = 0;
            string expectedExponent = "10000001"; // Двоичное представление числа 129 (127 + 2)
            string expectedMantissa = "11101";


            FloatingPoint floatingPoint = new FloatingPoint(num);
            //List<int> actualExponent = floatingPoint.Exponent;
            //List<int> actualMantissa= floatingPoint.Mantissa;

            Assert.AreEqual(expectedSign, floatingPoint.Sign);
            Assert.AreEqual(expectedExponent, string.Join("", floatingPoint.Exponent));
            Assert.AreEqual(expectedMantissa, string.Join("", floatingPoint.Mantissa));

        }

        [TestMethod()]
        public void PrintNumberTest()
        {
            float num = 7.25f;

         
            int expectedSign = 0;
            List<int> expectedExponent = new List<int> { 1, 0, 0, 0, 0, 0, 0, 1 };
            List<int> expectedMantissa = new List<int> { 1, 1, 0, 1 };
            FloatingPoint floatingPoint = new FloatingPoint(num);

            // Act
            string printedNumber = CaptureConsoleOutput(() => floatingPoint.PrintNumber());

            // Assert
            Assert.IsTrue(printedNumber.Contains($"Floating point: {expectedSign} | {string.Join("", expectedExponent)} | {string.Join("", expectedMantissa)}"));

             string CaptureConsoleOutput(Action action)
            {
                // Redirect console output to a StringWriter
                using (var consoleOutput = new System.IO.StringWriter())
                {
                    Console.SetOut(consoleOutput);

                    // Perform action that will write to console
                    action();

                    // Return the console output as a string
                    return consoleOutput.ToString();
                }
            }
        }

        [TestMethod()]
        public void AddTest()
        {
            FloatingPoint fp1 = new FloatingPoint(6.125f);
            FloatingPoint fp2 = new FloatingPoint(23.5f);
            //FloatingPoint floatingPoint = new FloatingPoint(num);
            // Act
            FloatingPoint actualSum = fp1.Add(fp2);

            // Assert
            string printedNumber = CaptureConsoleOutput(() => actualSum.PrintNumber());

            // Assert
            Assert.IsTrue(printedNumber.Contains($"Floating point: 0 | 10000011 | 1101101"));

            string CaptureConsoleOutput(Action action)
            {
                // Redirect console output to a StringWriter
                using (var consoleOutput = new System.IO.StringWriter())
                {
                    Console.SetOut(consoleOutput);

                    // Perform action that will write to console
                    action();

                    // Return the console output as a string
                    return consoleOutput.ToString();
                }
            }
        }
    }
}