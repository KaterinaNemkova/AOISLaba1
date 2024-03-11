using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1AOIS
{
    public class FloatingPoint
    {
        public float Value { get; private set; }
        public int Sign { get; private set; }
        public List<int> Exponent { get; private set; }
        public List<int> Mantissa { get; private set; }

        public FloatingPoint(float num)
        {
            Value = num;
            Sign = num < 0 ? 1 : 0;
            Exponent = new List<int>();
            Mantissa = new List<int>();

            if (num == 0)
            {
                Exponent.AddRange(new int[8]);
                Mantissa.AddRange(new int[23]);
                return;
            }

            // Convert float to binary
            int intPart = (int)Math.Abs(num);
            float fracPart = Math.Abs(num) - intPart;

            // Convert integer part to binary
            List<int> intBinary = ConvertToBinary(intPart);
            Exponent = CalculateExponent(intBinary);

            // Convert fractional part to binary
            List<int> fractBinary = ConvertFractionToBinary(fracPart);
            Mantissa = CalculateMantissa(intBinary,fractBinary);
        }

        private List<int> ConvertToBinary(int num)
        {
            List<int> binary = new List<int>();
            while (num > 0)
            {
                binary.Insert(0, num % 2);
                num /= 2;
            }
            return binary;
        }

        private List<int> CalculateExponent(List<int> binary)
        {
            List<int> exponent = new List<int>();
            int bias = 127;
            int expValue = binary.Count - 1 + bias;

            while (expValue > 0)
            {
                exponent.Insert(0, expValue % 2);
                expValue /= 2;
            }

            // Add leading zeros if necessary
            while (exponent.Count < 8)
            {
                exponent.Insert(0, 0);
            }

            return exponent;
        }

        private List<int> ConvertFractionToBinary(float fracPart)
        {
            List<int> fractbinary = new List<int>();
            int maxBits = 23;

            while (fracPart != 0 && fractbinary.Count < maxBits)
            {
                fracPart *= 2;
                if (fracPart >= 1)
                {
                    fractbinary.Add(1);
                    fracPart -= 1;
                  
                }
                else
                {
                    fractbinary.Add(0);
                }
            }

            return fractbinary;
        }
        private List<int> CalculateMantissa(List<int> intBinary,List<int> fractBinary)
        {
            List<int> mantissa = new List<int>(24);
            int maxBits = 24;

            mantissa.AddRange(intBinary);

            // Добавляем дробную часть мантиссы
            mantissa.AddRange(fractBinary);
            while (mantissa.Count != maxBits)
            {
                mantissa.Add(0);
            }

            return mantissa;
        }
        public void PrintNumber()
        {
            string mantissaStr = string.Join("", Mantissa);

            // Выводим мантиссу, начиная со второго символа
            Console.WriteLine($"Floating point: {Sign} | {string.Join("", Exponent)} | {mantissaStr.Substring(1)}");
        }

        public FloatingPoint Add(FloatingPoint other)
        {
            FloatingPoint sum = new FloatingPoint(0);

            // Если одно из чисел равно нулю, просто возвращаем другое число
            if (IsZero(this))
            {
                return other;
            }
            else if (IsZero(other))
            {
                return this;
            }

            // Определяем, какое из чисел имеет больший экспонент
            int expComparison = CompareExponents(other);

            // Выравниваем мантиссы по экспоненте
            
            List<int> alignedMantissaThis = AlignMantissas(this, other, expComparison);
            
            List<int> resultMantissa = new List<int>(25);
            // Складываем мантиссы
            if (expComparison > 0)
            {
                resultMantissa = Add(alignedMantissaThis, this.Mantissa);
            }
            else
            {
                resultMantissa = Add(alignedMantissaThis, other.Mantissa);
            }

            // При необходимости корректируем знак и экспоненту
            if (expComparison < 0)
            {
                sum.Sign = other.Sign;
                sum.Exponent = other.Exponent;
            }
            else
            {
                sum.Sign = this.Sign;
                sum.Exponent = this.Exponent;
            }
            if (resultMantissa[0] == 1)
            { 
                int carry = 1;
                for (int i = sum.Exponent.Count-1; i>=0; i--)
                {
                        sum.Exponent[i] += carry;
                        carry=sum.Exponent[i] / 2;
                        sum.Exponent[i] %= 2;
                   
                }
                if (resultMantissa.Count > 24)
                {
                    while (resultMantissa.Count > 24)
                    {
                       resultMantissa.RemoveAt(resultMantissa.Count - 1);
                    }
                }
                
            }
            else
            {
                if (resultMantissa.Count > 24)
                {
                    while (resultMantissa.Count > 24)
                    {
                        resultMantissa.RemoveAt(resultMantissa.Count - 1);
                    }
                }
                resultMantissa.RemoveAt(0);
            }
            
            // Обновляем мантиссу результата
            sum.Mantissa = resultMantissa;

            return sum;
        }

        private bool IsZero(FloatingPoint num)
        {
            // Проверяем, является ли число нулем (мантисса и экспонента равны нулю)
            return Enumerable.SequenceEqual(num.Mantissa, Enumerable.Repeat(0, 23)) &&
                   Enumerable.SequenceEqual(num.Exponent, Enumerable.Repeat(0, 8));
        }
        private int CompareExponents(FloatingPoint other)
        {
            // Возвращаем разницу между суммами значений экспонент и 127
            return ExponentSum() - other.ExponentSum();
        }

        private int ExponentSum()
        {
            // Вычисляем сумму значений экспоненты и вычитаем 127
            int sum = 0;
            foreach (int expValue in Exponent)
            {
                sum = sum * 2 + expValue;
            }
            return sum - 127;
        }

        private List<int> AlignMantissas(FloatingPoint target, FloatingPoint source, int expComparison)
        {
            if (expComparison > 0)
            {
                // Если значение target больше, добавляем нули к мантиссе source
                List<int> alignedMantissa = new List<int>(source.Mantissa);
                alignedMantissa.InsertRange(0, new int[expComparison]);
                if (alignedMantissa.Count > 24)
                {
                    while (alignedMantissa.Count > 24)
                    {
                        alignedMantissa.RemoveAt(alignedMantissa.Count - 1);
                    }
                }
                return alignedMantissa;
            }
            else 
            {
                // Если значение source больше, добавляем нули к мантиссе target
                List<int> alignedMantissa = new List<int>(target.Mantissa);
                alignedMantissa.InsertRange(0, new int[Math.Abs(expComparison)]);
                while (alignedMantissa.Count >24)
                {
                    alignedMantissa.RemoveAt(alignedMantissa.Count - 1);
                }
                return alignedMantissa;
            }

        }
        public static List<int> Add(List<int> mantissa1, List<int> mantissa2)
        {
            List<int> result = new List<int>(25);
            int carry = 0;
            for (int i = mantissa1.Count-1; i >= 0; --i)
            {
                int sum = mantissa1[i] + mantissa2[i] + carry;

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
            if (carry == 1)
            {
                result.Insert(0, 1);
            }
            else
            {
                result.Insert(0, 0);
            }
           
                return result;
        }
    }
}
