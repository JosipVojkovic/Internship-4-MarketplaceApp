using MarketplaceApp.Domain;
using System;

namespace MarketplaceApp.Presentation.Helpers
{
    public static class InputValidator
    {
        public static string ValidateString(string text)
        {
            string input;
            do
            {
                Console.Write(text);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Clear();
                    Console.WriteLine("Unos ne moze biti prazan. Pokusajte ponovno.\n");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }
        public static string ValidateEmail(string text)
        {
            string input;
            do
            {
                Console.Write(text);
                input = Console.ReadLine();
                if (!input.Contains("@") || !input.Contains("."))
                {
                    Console.Clear();
                    Console.WriteLine("Unesite valjanu email adresu. Pokusajte ponovno.\n");
                    input = null;
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }
        public static decimal ValidateDecimal(string text)
        {
            decimal result;
            string input;
            do
            {
                Console.Write(text);
                input = Console.ReadLine();
                if (!decimal.TryParse(input, out result))
                {
                    Console.Clear();
                    Console.WriteLine("Unos mora biti valjani decimalni broj. Pokusajte ponovno.\n");
                }
                else if(decimal.Parse(input) <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Unos mora biti decimalni broj veci od 0. Pokusajte ponovno.\n");
                }
            } while (!decimal.TryParse(input, out result) || decimal.Parse(input) <= 0);

            return Math.Round(result, 2);
        }

        public static DateTime ValidateDate(string text)
        {
            DateTime result;
            string input;
            do
            {
                Console.Write(text);
                input = Console.ReadLine();

                if (!DateTime.TryParse(input, out result))
                {
                    Console.Clear();
                    Console.WriteLine("Unos mora biti valjani datum (format: dd.MM.yyyy ili yyyy-MM-dd). Pokusajte ponovno.\n");
                }
                else if (result > DateTime.Now)
                {
                    Console.Clear();
                    Console.WriteLine("Unos ne smije biti datum u budućnosti. Pokusajte ponovno.\n");
                }
            } while (!DateTime.TryParse(input, out result) || result > DateTime.Now);

            return result.Date;
        }
    }
}
