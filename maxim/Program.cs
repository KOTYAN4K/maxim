using System;

namespace maxim
{
    internal class Program
    {
        static string StringProcess(string str)
        {
            if (str.Length % 2 == 0)
            {
                int halfString = str.Length / 2;
                string firstSubString = str.Substring(0, halfString);
                string secondSubString = str.Substring(halfString);
                char[] firstHalfArray = firstSubString.ToCharArray();
                Array.Reverse(firstHalfArray);
                char[] secondHalfArray = secondSubString.ToCharArray();
                Array.Reverse(secondHalfArray);
                return new string(firstHalfArray) + new string(secondHalfArray);
            }
            else
            {
                char[] strArray = str.ToCharArray();
                Array.Reverse(strArray);
                return new string(strArray) + str;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку:");
            string input = Console.ReadLine();
            string processedString = StringProcess(input);
            Console.WriteLine("Обработанная строка: " + processedString);
        }
    }
}
