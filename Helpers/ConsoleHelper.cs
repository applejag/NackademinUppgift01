using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ConsoleHelper
    {

        public static int AskForInt(string question)
        {
            while (true)
            {
                Console.Write(question);
                string input = Console.ReadLine()
                    .Trim();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                Console.WriteLine($"\"{input}\" kunde inte omvandlas till en siffra! Försök igen.");

            }
        }
        
        public static string AskForSpecificString(string question, params string[] alternatives)
        {
            while (true)
            {
                Console.Write(question);
                string input = Console.ReadLine()
                    .Trim();

                foreach(string alt in alternatives)
                {
                    if (input.ToLower() == alt.ToLower())
                        return alt;
                }
            }
        }

        public static void WaitBeforeExit()
        {
            Console.WriteLine("\nTryck valfri knapp för att avsluta..");
            Console.ReadKey();
        }

    }
}
