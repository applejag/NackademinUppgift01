using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCipher
{
    class Program
    {
        const short CHAR_FIRST = (short) 'A';
        const short CHAR_LAST = (short) 'Z';
        const short CHAR_COUNT = CHAR_LAST - CHAR_FIRST + 1;

        static void Main(string[] args)
        {
            Console.WriteLine("KalleCryptor x0.002");
            Console.WriteLine();

            while (true)
            {
                // Läs input
                Console.Write("Skriv en text: ");
                string input = Console.ReadLine();

                int key = AskForInt("Skriv in ett tal som nyckel: ");

                Console.WriteLine();

                // Printa encryptat meddelande
                string output = EncryptString(input, key);
                Console.Write("Resultat: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(output);
                Console.ResetColor();

                Console.WriteLine();
            }
        }

        static string EncryptString(string input, int key)
        {
            input = input.ToUpper();

            string output = "";

            foreach (char c in input)
            {
                // Hoppa över ogiltiga karaktärer
                if (c < CHAR_FIRST || c > CHAR_LAST) continue;

                output += EncryptChar(c, key);
            }

            return output;
        }

        static char EncryptChar(char input, int key)
        {
            int output = input + key;

            // Fortsätt försöka flytta om output till rätt intervall
            while (output < CHAR_FIRST) output += CHAR_COUNT;
            while (output > CHAR_LAST) output -= CHAR_COUNT;

            return (char)output;
        }
        
        static int AskForInt(string prompt)
        {
            string input = null;
            int result;

            do {
                if (input != null)
                    Console.WriteLine($"\"{input}\" kunde inte omvandlas till en siffra! Försök igen.");

                Console.Write(prompt);
                input = Console.ReadLine()
                    ?.Trim() ?? "";

            } while (!int.TryParse(input, out result));

            return result;
        }

    }
}
