using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplessCipher
{
    class Program
    {
        static readonly string LETTER_TABLE = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ -_:;.,?!0123456789".Shuffle(0);
        static readonly int LETTER_COUNT = LETTER_TABLE.Length;

        static void Main(string[] args)
        {
            Console.WriteLine("KalleCryptor X2000");
            Console.WriteLine();

            while (true)
            {
                // Läs input
                Console.Write("Skriv en text: ");
                string input = Console.ReadLine();

                int key = ConsoleHelper.AskForInt("Skriv in ett tal som nyckel: ");

                Console.WriteLine();

                // Printa encryptat meddelande
                string output = EncryptString(input, key);
                Console.Write("Resultat: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine(output);
                Console.ResetColor();

                Console.WriteLine();
            }
        }

        static string EncryptString(string input, int key)
        {
            string output = "";

            foreach (char c in input)
            {
                int index = LETTER_TABLE.IndexOf(c);

                // Hoppa över ogiltiga karaktärer
                if (index == -1) continue;

                output += EncryptChar(index, key);
            }

            return output;
        }

        static char EncryptChar(int index, int key)
        {
            int output = index + key;

            // Fortsätt försöka flytta om output till rätt intervall
            while (output < 0) output += LETTER_COUNT;
            output %= LETTER_COUNT;

            return LETTER_TABLE[output];
        }

    }
}
