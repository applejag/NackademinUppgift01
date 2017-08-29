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
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result) == false)
                {
                    Console.WriteLine($"\"{input}\" är ett ogiltigt heltal! Försök igen.");
                }
                else
                {
                    return result;
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
