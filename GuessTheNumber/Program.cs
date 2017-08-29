using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheNumber
{
    class Program
    {
        static void Main(string[] args)
        {

            bool fortsättSpela = true;
            Random slumpGenerator = new Random();

            while (fortsättSpela)
            {
                Console.WriteLine("Gissa ett tal mellan 1 och 100.");

                int antalGissningar = 0;
                int slumpatTal = slumpGenerator.Next(1, 101); // 1 INCLUSIVE -> 101 EXCLUSIVE

            GissaEttTal:

                antalGissningar++;
                int gissning = ConsoleHelper.AskForInt($"Gissning {antalGissningar}: ");
                
                if (gissning == slumpatTal)
                {
                    Console.WriteLine($"Rätt! Du gissade rätt på {antalGissningar} försök.");

                    string svar = ConsoleHelper.AskForSpecificString("Vill du spela igen (Ja/Nej)? ", "ja", "nej");
                    if (svar == "nej")
                    {
                        fortsättSpela = false;
                    }
                }
                else
                {
                    // Fel gissning
                    if (slumpatTal > gissning)
                    {
                        Console.WriteLine("Talet är större.");
                    }
                    else
                    {
                        Console.WriteLine("Talet är mindre.");
                    }

                    goto GissaEttTal;
                }
                
            }

            Console.WriteLine("Tack för den här gången!");
            ConsoleHelper.WaitBeforeExit();
        }
    }
}
