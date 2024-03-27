using System;
using System.Collections.Generic;
using System.Linq;

namespace maxim
{
    internal class Program
    {
        static string StringProcess(string str, out Dictionary<char, int> characterCounts, out string longestVowelSubstring)
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
            longestVowelSubstring = LongestSubstring(processedString);
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

        static string LongestSubstring(string text)
        {
            string vowels = "aeiouy";
            int maxLength = 0;
            string longestSubstring = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (vowels.Contains(text[i]))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (vowels.Contains(text[j]))
                        {
                            int substringLength = j - i + 1;
                            if (substringLength > maxLength)
                            {
                                maxLength = substringLength;
                                longestSubstring = text.Substring(i, substringLength);
                            }
                        }
                    }
                }
            }
            return longestSubstring;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку:");
            string input = Console.ReadLine();
            Dictionary<char, int> charCount;
            string longestVowelSubstring = "";
            if (CheckString(input))
            {
                string processedString = StringProcess(input, out charCount, out longestVowelSubstring);
                Console.WriteLine($"Обработанная строка: {processedString}\n");
                
                Console.WriteLine("Информация о количестве повторений каждого символа:");
                foreach (var kvp in charCount)
                {
                    Console.WriteLine($"Символ '{kvp.Key}': {kvp.Value} раз(а)");
                }

                
                Console.WriteLine($"Самая длинная подстрока, начинающаяся и заканчивающаяся на гласную: {longestVowelSubstring}");
                
            }
            Console.ReadKey();
        }
    }
}
