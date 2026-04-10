using System;
using System.Linq;

namespace InventoryManagementSystem
{
    public static class InputHelper
    {
        public static int GetValidNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please enter a number.");
                    continue;
                }

                if (!input.All(char.IsDigit))
                {
                    Console.WriteLine("Invalid input. Numbers only allowed.");
                    continue;
                }

                if (int.TryParse(input, out int result) && result >= 0)
                {
                    return result;
                }

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        public static decimal GetValidDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                if (decimal.TryParse(input, out decimal result) && result >= 0)
                    return result;

                Console.WriteLine("Invalid decimal.");
            }
        }

        public static string GetValidLetters(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                if (input.All(c => char.IsLetter(c) || c == ' '))
                    return input.Trim();

                Console.WriteLine("Letters only.");
            }
        }

        public static string GetValidLettersOrDigits(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Invalid input.");
            }
        }
    }
}