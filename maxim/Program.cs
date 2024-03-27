using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace maxim
{
    internal class Program
    {
        static string StringProcess(string str, out Dictionary<char, int> characterCounts, 
            out string longestVowelSubstring,out string sortedString, string sortAlgorithm)
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

            // Подсчет количества символов
            characterCounts = CountChars(processedString);

            // Поиск самой длинной подстроки, начинающейся и заканчивающейся на гласную
            longestVowelSubstring = LongestSubstring(processedString);

            // Выбор алгоритма сортировки
            if (sortAlgorithm.ToLower() == "quicksort")
                sortedString = QuickSort(processedString);
            else if (sortAlgorithm.ToLower() == "treesort")
                sortedString = TreeSort(processedString);
            else
                sortedString = processedString; // Если алгоритм сортировки неизвестен, возвращаем исходную строку

            //возврат обработанной строки
            return processedString;
        }

        private static string TreeSort(string processedString)
        {
            SortedSet<char> charSet = new SortedSet<char>(processedString);
            return string.Join("", charSet);
        }

        private static string QuickSort(string processedString)
        {
            char[] charArray = processedString.ToCharArray();
            Array.Sort(charArray);
            return new string(charArray);
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

        static async Task<int> GetRandomNumber(int max)
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, max);

            // Если удаленный API недоступен, возвращаем случайное число средствами .NET
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={max}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    randomIndex = int.Parse(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении случайного числа через удаленный API: {ex.Message}");
            }

            return randomIndex;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите строку:");
            string input = Console.ReadLine();
            Console.WriteLine("Выберите алгоритм сортировки (Quicksort или Treesort):");
            string sortAlgorithm = Console.ReadLine();

            Dictionary<char, int> charCount;
            string longestVowelSubstring = "";
            string sortedString = "";

            if (CheckString(input))
            {
                string processedString = StringProcess(input, out charCount, out longestVowelSubstring,
                    out sortedString, sortAlgorithm);
                Console.WriteLine($"Обработанная строка: {processedString}\n");
                
                Console.WriteLine("Информация о количестве повторений каждого символа:");
                if (charCount != null)
                {
                    foreach (var kvp in charCount)
                    {
                        Console.WriteLine($"Символ '{kvp.Key}': {kvp.Value} раз(а)");
                    }
                }

                Console.WriteLine($"\nСамая длинная подстрока, начинающаяся и заканчивающаяся на гласную: {longestVowelSubstring}\n");

                Console.WriteLine($"Отсортированная обработанная строка: {sortedString}\n");

                // Получение случайного числа
                int randomIndex = await GetRandomNumber(processedString.Length);

                // Урезание обработанной строки
                string trimmedString = processedString.Remove(randomIndex, 1);

                Console.WriteLine("Урезанная обработанная строка: " + trimmedString);
            }
            Console.ReadKey();
        }
    }
}