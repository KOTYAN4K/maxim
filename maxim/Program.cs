using System;
using System.Collections.Generic;
using System.Linq;

namespace maxim
{
    internal class Program
    {
        static string StringProcess(string str, out Dictionary<char, int> characterCounts)
        {
            string processedString = "";
            if (str.Length % 2 == 0)
            {
                int halfString = str.Length / 2;
                string firstSubString = str.Substring(0, halfString);
                string secondSubString = str.Substring(halfString);
                char[] firstHalfArray = firstSubString.ToCharArray();
                Array.Reverse(firstHalfArray);
                char[] secondHalfArray = secondSubString.ToCharArray();
                Array.Reverse(secondHalfArray);
                processedString = new string(firstHalfArray) + new string(secondHalfArray);
            }
            else
            {
                char[] strArray = str.ToCharArray();
                Array.Reverse(strArray);
                processedString = new string(strArray) + str;
            }

            characterCounts = CountChars(processedString);
            return processedString;
        }

        static bool CheckString(string str)
        {
            try
            {
                
                var invalidChars = str.Where(c => !char.IsLower(c) || c < 'a' || c > 'z').Distinct().ToArray();
                if (invalidChars.Any())
                {
                    throw new Exception($"Ошибка: введены неподходящие символы: {string.Join(", ", invalidChars)}. Допустимы только буквы английского алфавита в нижнем регистре.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

        static Dictionary<char, int> CountChars(string text)
        {
            Dictionary<char, int> characterCounts = new Dictionary<char, int>();
            foreach (char c in text)
            {
                if (characterCounts.ContainsKey(c))
                    characterCounts[c]++;
                else characterCounts[c] = 1;
            }
            return characterCounts;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку:");
            string input = Console.ReadLine();
            Dictionary<char, int> charCount;
            if (CheckString(input))
            {
                string processedString = StringProcess(input, out charCount);
                Console.WriteLine($"Обработанная строка: {processedString}\n");
                
                Console.WriteLine("Информация о количестве повторений каждого символа:");
                foreach (var kvp in charCount)
                {
                    Console.WriteLine($"Символ '{kvp.Key}': {kvp.Value} раз(а)");
                }
                
            }
            Console.ReadKey();
        }
    }
}
