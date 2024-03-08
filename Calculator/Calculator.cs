using Laba1AOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Laba1AOIS
{
   public class Binary
    {
        public List<int> num;


        public Binary(int decimalNumber)
        {
            num = DecimalToBinary(decimalNumber);
        }

        // Метод для перевода числа из десятичной системы счисления в двоичный код 
        private List<int> DecimalToBinary(int decimalNumber)
        {
            List<int> binary = new List<int>(32);

            // Проверяем, является ли число отрицательным
            bool isNegative = decimalNumber < 0;

            if (isNegative)
            {
                // Преобразуем число в его абсолютное значение и добавляем к списку битов его двоичное представление
                decimalNumber = Math.Abs(decimalNumber);
            }

            // Переводим число в двоичный код
            for (int i = 0; i < 32; i++)
            {
                binary.Insert(0, decimalNumber % 2); // Вставляем бит в начало списка
                decimalNumber /= 2; // Делим на 2 для перехода к следующему биту
            }


            return binary;
        }
        public static List<int> DecimalToStraightCode(int decimalNumber)
        {
            List<int> straightCode = new List<int>();

            // Добавляем знаковый бит
            if (decimalNumber < 0)
            {
                straightCode.Add(1); // если число отрицательное, первый бит равен 1
                decimalNumber = Math.Abs(decimalNumber); // переводим число в его абсолютное значение
            }
            else
            {
                straightCode.Add(0); // если число положительное, первый бит равен 0
            }

            // Преобразуем число в его двоичное представление (без знакового бита)
            for (int i = 0; i < 31; i++)
            {
                straightCode.Insert(1, decimalNumber % 2); // вставляем бит вторым элементом списка (после знакового бита)
                decimalNumber /= 2; // делим на 2 для перехода к следующему биту
            }

            return straightCode;
        }

        public static List<int> StraightToReverse(List<int> straightCode)
        {
            List<int> reverse = new List<int>(straightCode);

            int firstIndex = straightCode[0];
            if (firstIndex == 0)
            {
                return straightCode;
            }
            else
            {
                // Проходим по всем битам, начиная со второго 
                for (int i = 1; i < reverse.Count; i++)
                {
                    // Если бит равен 1, заменяем его на 0, иначе на 1
                    reverse[i] = reverse[i] == 1 ? 0 : 1;
                }

                return reverse;
            }
        }
        public static List<int> ReverseToComplementCode(List<int> reverseCode)
        {
            List<int> complement = new List<int>(reverseCode);

            int firstIndex = reverseCode[0];
            if (firstIndex == 0)
            {
                return reverseCode;
            }
            else
            {
                // Добавляем 1 к последнему биту
                int lastIndex = complement.Count - 1;
                complement[lastIndex] += 1;

                // Если последний бит стал равен 2, устанавливаем его в 0 и передаем перенос к предыдущему биту
                int carry = complement[lastIndex] / 2;
                complement[lastIndex] %= 2;
                for (int i = lastIndex - 1; i >= 0 && carry > 0; i--)
                {
                    complement[i] += carry;
                    carry = complement[i] / 2;
                    complement[i] %= 2;
                }
            }
            return complement;
        }

        public static List<int> SumOfComplement(List<int> complement1, List<int> complement2)
        {
            List<int> result = new List<int>(32);
            int carry = 0;
            for (int i = complement1.Count - 1; i >= 0; i--)
            {
                int sum = complement1[i] + complement2[i] + carry;
                if (sum == 2)
                {
                    carry = 1;
                    result.Insert(0, 0);
                }
                else if (sum == 3)
                {
                    carry = 1;
                    result.Insert(0, 1);
                }
                else
                {
                    carry = 0;
                    result.Insert(0, sum);
                }
            }
            if (result[0] == 1 && result.Count == 32)
            {
                //Если складываем отрицательные числа или результат отрицателен
                //или по окончании 32 битов у нас остается 1, то инвертируем результат и добавляем 1
                for (int i = 1; i < result.Count; i++)
                {
                    result[i] = result[i] == 0 ? 1 : 0;
                }
                carry = 1;
                for (int i = result.Count - 1; i >= 0 && carry > 0; i--)
                {
                    result[i] += carry;
                    carry = result[i] / 2;
                    result[i] %= 2;
                }
            }
            return result;
        }


        public static List<int> MultiplicationOfStraight(List<int> straightCode1, List<int> straightCode2)
        {
            List<int> intermediateResults = new List<int>(new int[straightCode1.Count + straightCode2.Count]);


            // Умножаем каждый бит одного числа на бит другого числа
            for (int i = straightCode1.Count - 1; i >= 0; i--)
            {
                int carry = 0;
                for (int j = straightCode2.Count - 1; j >= 0; j--)
                {

                    int product = straightCode1[i] * straightCode2[j] + carry + intermediateResults[i + j + 1];
                    intermediateResults[i + j + 1] = product % 2; // Записываем остаток от деления на 2 (0 или 1)
                    carry = product / 2; // Получаем перенос для следующего разряда
                }
                intermediateResults[i] += carry;
            }

            List<int> result = intermediateResults.GetRange(intermediateResults.Count - 32, 32);

            if ((straightCode1[0] == 1 && straightCode2[0] == 0) || (straightCode1[0] == 0 && straightCode2[0] == 1))
            {
                result[0] = 1; // Если одно из чисел отрицательное, первый бит результата должен быть равен 1
            }
            else
            {
                result[0] = 0; // Иначе первый бит результата равен 0
            }
            return result;

        }



        public static FixedPoint Divide(List<int> dividend, List<int> divider)
        {
            
            string divisionResultInt, divisionResultFloat = "0";
            string divisibleNum = string.Join("", dividend);
            string dividerNum = string.Join("", divider);
            string toDiv;

            char signResult = divisibleNum[0] == dividerNum[0] ? '0' : '1';
            divisionResultInt = signResult.ToString();
            divisibleNum = divisibleNum.Substring(1);
            dividerNum = dividerNum.Substring(1);

            while (divisibleNum[0] != '1')
            {
                divisibleNum = divisibleNum.Substring(1);
            }
            while (dividerNum[0] != '1')
            {
                dividerNum = dividerNum.Substring(1);
            }
            if (!CompareBinaryIntNorm(divisibleNum, dividerNum))
            {
                divisionResultInt = "0";
            }
            else
            {
                toDiv = divisibleNum.Substring(0, dividerNum.Length - 1) ;

                for (int i = dividerNum.Length - 1; i < divisibleNum.Length; i++)
                {
                    toDiv += divisibleNum[i];
                    char element = CompareBinaryIntNorm(toDiv, dividerNum) ? '1' : '0';
                    divisionResultInt += element;
                    if (element == '1')
                    {
                        int result = DirectToDecimal(ConvertToList(toDiv.Insert(0, "0"))) - DirectToDecimal(ConvertToList(dividerNum.Insert(0, "0")));
                        toDiv = string.Join("", DecimalToStraightCode(result));
                        toDiv = toDiv.Substring(1);
                        while (toDiv[0] != '1' && toDiv.Length != 1)
                        {
                            toDiv = toDiv.Substring(1);
                        }
                    }
                }

                if (DirectToDecimal(ConvertToList(toDiv.Insert(0, "0"))) != 0)
                {
                    divisionResultFloat = "0";
                    while (DirectToDecimal(ConvertToList(toDiv.Insert(0, "0"))) != 0 && divisionResultFloat.Length <= 5)
                    {
                        toDiv += '0';
                        char element = CompareBinaryIntNorm(toDiv.Insert(0, "0"), dividerNum.Insert(0, "0")) ? '1' :
                            Math.Abs(DirectToDecimal(ConvertToList(toDiv.Insert(0, "0")))) == Math.Abs(DirectToDecimal(ConvertToList(dividerNum.Insert(0, "0")))) ? '1' : '0';
                        divisionResultFloat += element;
                        if (element == '1')
                        {
                            int result = DirectToDecimal(ConvertToList(toDiv.Insert(0, "0"))) - DirectToDecimal(ConvertToList(dividerNum.Insert(0, "0")));
                            toDiv = string.Join("", DecimalToStraightCode(result));
                            toDiv = toDiv.Substring(1);
                        }
                    }
                    divisionResultFloat = divisionResultFloat.Substring(1);
                }
                else
                {
                    divisionResultFloat = "0";
                }
            }
            return new FixedPoint(divisionResultInt, divisionResultFloat);
        }

        public static bool CompareBinaryIntNorm(string a, string b)
        {
            int decimalA = DirectToDecimal(ConvertToList(a));
            int decimalB = DirectToDecimal(ConvertToList(b));

            return decimalA >= decimalB;
        }
        public static List<int> ConvertToList(string binaryString)
        {
            List<int> result = new List<int>();
            foreach (char c in binaryString)
            {
                result.Add(int.Parse(c.ToString()));
            }
            return result;
        }

        public static int DirectToDecimal(List<int> binaryNumber)
        {
            int decimalValue = 0;
            int power = binaryNumber.Count - 1; // Степень двойки, начиная с самого старшего бита

            foreach (int bit in binaryNumber)
            {
                decimalValue += bit * (int)Math.Pow(2, power); // Добавляем значение бита, умноженное на 2 в степени power
                power--; // Уменьшаем степень для следующего бита
            }

            return decimalValue;
        }

        
    }



    class Calculator
    {
        static void Main(string[] args)
        {
            //        Console.WriteLine("Введите два целых числа:");
            //        int decimalNumber1 = int.Parse(Console.ReadLine());
            //        int decimalNumber2 = int.Parse(Console.ReadLine());

            //        Console.WriteLine($"Двоичное представление числа {decimalNumber1}:");
            //        Binary binary1 = new Binary(decimalNumber1);
            //        foreach (int bit in binary1.num)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Прямой код числа {decimalNumber1}:");
            //        List<int> straightCode1 = Binary.DecimalToStraightCode(decimalNumber1);
            //        foreach (int bit in straightCode1)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Обратный код числа {decimalNumber1}:");
            //        List<int> onesComplement1 = Binary.StraightToReverse(straightCode1);
            //        foreach (int bit in onesComplement1)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();
            //        Console.WriteLine($"Дополнительный код числа {decimalNumber1}:");
            //        List<int> twosComplement1 = Binary.ReverseToComplementCode(onesComplement1);
            //        foreach (int bit in twosComplement1)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Двоичное представление числа {decimalNumber2}:");
            //        Binary binary2 = new Binary(decimalNumber2);
            //        foreach (int bit in binary2.num)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Прямой код числа {decimalNumber2}:");
            //        List<int> straightCode2 = Binary.DecimalToStraightCode(decimalNumber2);
            //        foreach (int bit in straightCode2)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();



            //        Console.WriteLine($"Обратный код числа {decimalNumber2}:");
            //        List<int> onesComplement2 = Binary.StraightToReverse(straightCode2);
            //        foreach (int bit in onesComplement2)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Дополнительный код числа {decimalNumber2}:");
            //        List<int> twosComplement2 = Binary.ReverseToComplementCode(onesComplement2);
            //        foreach (int bit in twosComplement2)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine("\t");

            //        Console.WriteLine($"Сумма {decimalNumber1} и {decimalNumber2} в прямом коде: ");
            //        List<int> sum = Binary.SumOfComplement(twosComplement1, twosComplement2);
            //        foreach (int bit in sum)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();


            //        // Выводим результат
            //        Console.WriteLine($"Результат умножения в прямом коде:");
            //        List<int> resultStraightCode = Binary.MultiplicationOfStraight(straightCode1, straightCode2);
            //        foreach (int bit in resultStraightCode)
            //        {
            //            Console.Write(bit);
            //        }
            //        Console.WriteLine();

            //        Console.WriteLine($"Результат деления ");

            //        FixedPoint resultDivide=Binary.Divide(straightCode1, straightCode2);

            //        Console.WriteLine($"{resultDivide.IntegerPart}.{resultDivide.FractionalPart} ");


            float num1 = 7.25f;
            float num2 = 23.5f;
            FloatingPoint floatingPoint1 = new FloatingPoint(num1);
            floatingPoint1.PrintNumber();
            FloatingPoint floatingPoint2 = new FloatingPoint(num2);
            floatingPoint2.PrintNumber();
            //FloatingPoint result = floatingPoint1.Add(floatingPoint2);

            //result.PrintNumber();

            Console.ReadLine();
        }
    }


}