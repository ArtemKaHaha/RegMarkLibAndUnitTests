using System.Text.RegularExpressions;

namespace REG_MARK_LIB
{
    public class Class1
    {
        public static bool CheckMark(string mark)
        {
            if (string.IsNullOrEmpty(mark)) return false;
            string pattern = "^[A-Za-z]\\d{3}[A-Za-z]{2}\\d{3}$";
            if (!Regex.IsMatch(mark, pattern)) return false;
            string regionString = mark.Substring(mark.Length - 3);
            if (!int.TryParse(regionString, out int region)) return false;
            return region >= 1 && region <= 199;
        }
        public static string GetNextMarkAfter(string mark)
        {
            if (string.IsNullOrEmpty(mark) || mark.Length != 9)
            {
                throw new ArgumentException("Некорректный формат номерного знака.");
            }

            char firstLetter = mark[0];
            int number = int.Parse(mark.Substring(1, 3));
            string twoLetters = mark.Substring(4, 2);
            int region = int.Parse(mark.Substring(6, 3));

            // Увеличиваем номер
            number++;

            // Проверяем, не превышает ли номер 999
            if (number > 999)
            {
                number = 1; // Сбрасываем номер
                twoLetters = IncrementLetters(twoLetters);

                // Проверяем, нужно ли увеличивать первую букву
                if (twoLetters == null)
                {
                    twoLetters = "AA";
                    firstLetter = IncrementLetter(firstLetter);
                    if (firstLetter == '0')
                    {
                        throw new Exception("Все возможные номера исчерпаны.");
                    }
                }

                // Увеличиваем регион только если номер был сброшен
                if (region < 199)
                {
                    region++;
                }
                else
                {
                    region = 1; // Сброс региона, если он достиг максимума
                }
            }

            return $"{firstLetter}{number:D3}{twoLetters}{region:D3}";
        }

        private static char IncrementLetter(char letter)
        {
            if (letter == 'z') return 'A';
            if (letter == 'Z') return '0';
            return (char)(letter + 1);
        }

        private static string IncrementLetters(string letters)
        {
            char letter2 = letters[1];
            char letter1 = letters[0];

            letter2 = IncrementLetter(letter2);
            if (letter2 == '0')
            {
                letter2 = 'A';
                letter1 = IncrementLetter(letter1);
                if (letter1 == '0') return null;
            }
            return $"{letter1}{letter2}";
        }

        public static string GetNextMarkAfterInRange(string prevMark, string rangeStart, string rangeEnd)
        {
            if (string.IsNullOrEmpty(prevMark) || prevMark.Length != 9 ||
                string.IsNullOrEmpty(rangeStart) || rangeStart.Length != 9 ||
                string.IsNullOrEmpty(rangeEnd) || rangeEnd.Length != 9)
            {
                throw new ArgumentException("Некорректный формат номерного знака или диапазона.");
            }

            try
            {
                string nextMark = GetNextMarkAfter(prevMark); 

                if (string.Compare(nextMark, rangeStart) >= 0 && string.Compare(nextMark, rangeEnd) <= 0)
                {
                    return nextMark;
                }
                else
                {
                    return "out of stock";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static int GetCombinationsCountInRange(string mark1, string mark2)
        {
            if (string.IsNullOrEmpty(mark1) || string.IsNullOrEmpty(mark2))
            {
                throw new ArgumentException("Номера не могут быть пустыми.");
            }

            long num1 = ConvertMarkToNumber(mark1);
            long num2 = ConvertMarkToNumber(mark2);

            if (num1 > num2)
            {
                long temp = num1;
                num1 = num2;
                num2 = temp;
            }

            return (int)(num2 - num1 + 1);
        }

        private static long ConvertMarkToNumber(string mark)
        {
            var regex = new Regex(@"([A-Z]+)(d{3})([A-Z]{2})(d{3})");
            var match = regex.Match(mark);

            if (!match.Success)
            {
                throw new ArgumentException("Неверный формат номера.");
            }

            string letters1 = match.Groups[1].Value; 
            string digits1 = match.Groups[2].Value;  
            string letters2 = match.Groups[3].Value; 
            string digits2 = match.Groups[4].Value;  

            long number = 0;

            number += (long)(letters1[0] - 'A') * 1000000000; 
            number += long.Parse(digits1) * 10000;          
            number += (long)(letters2[0] - 'A') * 1000;     
            number += long.Parse(digits2);

            return number;
        }

        public static void Main(string[] args)
        {
            // Для первого метода
            string[] licensePlates = { "A123AA123", "a456bb789", "B789CC101", "Z999ZZ999", "A123aa123", "A123AA1234", "A123AA200", "A123BB001" };

            foreach (string plate in licensePlates)
            {
                Console.WriteLine($"{plate}: {CheckMark(plate)}");
            }

            // Для второго метода
            string[] marks = { "A999ZZ199", "A999ZZ198", "Z999ZZ199", "A000AA001" };

            foreach (string mark in marks)
            {
                try
                {
                    string nextMark = GetNextMarkAfter(mark);
                    Console.WriteLine($"Следующий номер после {mark}: {nextMark}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка для {mark}: {ex.Message}");
                }
            }

            // Для третьего метода
            string rangeStart = "A001AA001";
            string rangeEnd = "A999ZZ199";

            string[] prevMarks = { "A001AA001", "A999ZZ198", "A999ZZ199", "Z999ZZ199", "A000AA001", "A123AA100" };

            foreach (string prevMark in prevMarks)
            {
                string nextMark = GetNextMarkAfterInRange(prevMark, rangeStart, rangeEnd);
                Console.WriteLine($"Следующий номер после {prevMark}: {nextMark}");
            }

            // Для четвёртого метода
            string mark1 = "A999AA999";
            string mark2 = "B000AB000";
            int count = GetCombinationsCountInRange(mark1, mark2);
            Console.WriteLine($"Количество возможных номеров между {mark1} и {mark2}: {count}");
        }
    }
}
