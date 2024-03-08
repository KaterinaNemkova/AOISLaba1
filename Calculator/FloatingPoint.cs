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
            List<int> mantissa = new List<int>();
            //int maxBits = 23;

            mantissa.AddRange(intBinary);

            // Добавляем дробную часть мантиссы
            mantissa.AddRange(fractBinary);

            // Убираем первую единицу, если она есть
            //if (mantissa.Count > 0 && mantissa[0] == 1)
            //{
            //    mantissa.RemoveAt(0);
            //}
            //mantissa.Add(0);

            return mantissa;
        }
        public void PrintNumber()
        {
            string mantissaStr = string.Join("", Mantissa);
            int firstOneIndex = mantissaStr.IndexOf('1');

            // Если первая единица найдена, выводим мантиссу без нее
            if (firstOneIndex != -1)
            {
                mantissaStr = mantissaStr.Substring(firstOneIndex + 1);
            }
            Console.WriteLine($"Floating point: {Sign} | {string.Join("", Exponent)} | {string.Join("", mantissaStr)}");
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
            List<int> alignedMantissaOther = AlignMantissasRight(this, other, expComparison);

            // Складываем мантиссы
            List<int> resultMantissa = Binary.SumOfComplement(alignedMantissaThis, alignedMantissaOther);

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

            // Обновляем мантиссу результата
            sum.Mantissa = resultMantissa;

            // Нормализуем результат
            sum.Normalize();

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
           // List<int> alignedMantissa = new List<int>(target.Mantissa);

            if (expComparison > 0)
            {
                // Если значение target больше, добавляем нули к мантиссе source
                List<int> alignedMantissa = new List<int>(source.Mantissa);
                alignedMantissa.InsertRange(0, new int[expComparison]);
                return alignedMantissa;
            }
            else 
            {
                // Если значение source больше, добавляем нули к мантиссе target
                List<int> alignedMantissa = new List<int>(target.Mantissa);
                alignedMantissa.InsertRange(0, new int[Math.Abs(expComparison)]);
                return alignedMantissa;
            }

            
        }
        private List<int> AlignMantissasRight(FloatingPoint target, FloatingPoint source, int expComparison)
        {
            // List<int> alignedMantissa = new List<int>(target.Mantissa);

            if (expComparison > 0)
            {
                // Если значение target больше, добавляем нули к мантиссе source
                List<int> alignedMantissa = new List<int>(target.Mantissa);
                for (int i = 0; i < expComparison; i++)
                {
                    alignedMantissa.Add(0);
                }
                return alignedMantissa;
            }
            else
            {
                // Если значение source больше, добавляем нули к мантиссе target
                List<int> alignedMantissa = new List<int>(source.Mantissa);
                for (int i = 0; i < Math.Abs(expComparison); i++)
                {
                    alignedMantissa.Add(0);
                }
                return alignedMantissa;
            }


        }
       
       
        private void Normalize()
        {
            // Нормализуем результат, обновляя экспоненту и мантиссу при необходимости
            int index = 0;
            // Ищем первый ненулевой бит в мантиссе
            while (index < Mantissa.Count && Mantissa[index] == 0)
            {
                index++;
            }

            // Если индекс выходит за пределы размера мантиссы, значит число равно нулю
            if (index == Mantissa.Count)
            {
                // Сбрасываем экспоненту
                Exponent = new List<int>(new int[Exponent.Count]);
                return;
            }

            // Обновляем экспоненту, вычитая из нее разницу между текущим индексом и индексом, с которого начинается нормализованная мантисса
            Exponent = SubtractExponents(Exponent, index);

            // Обрезаем мантиссу, оставляя только нормализованную часть
            Mantissa = Mantissa.GetRange(index, Mantissa.Count - index);
        }

        private List<int> SubtractExponents(List<int> exponent, int subtractValue)
        {
            List<int> result = new List<int>(exponent);
            // Вычитаем значение из каждого бита экспоненты
            for (int i = result.Count - 1; i >= 0 && subtractValue > 0; i--)
            {
                if (result[i] == 0)
                {
                    result[i] = 1;
                    subtractValue--;
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }
    }
}
