using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceSim
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till tärningssimulatorn!");

            int antalOmgångar = ConsoleHelper.AskForInt("Antal omgångar? ");
            int antalTärningar = ConsoleHelper.AskForInt("Antal tärningar? ");
            int antalSidor = ConsoleHelper.AskForInt("Antal sidor? ");

            Random slumpgenerator = new Random();
            int totalSumma = 0;

            for (int omgång = 1; omgång <= antalOmgångar; omgång++)
            {
                int omgångSumma = 0;

                Console.Write($"Omgång {omgång}: ");

                for (int tärning = 0; tärning < antalTärningar; tärning++)
                {
                    // +1 för att räkna från 1 istället för 0
                    int slag = slumpgenerator.Next(antalSidor) + 1;

                    omgångSumma += slag;
                    totalSumma += slag;

                    Console.Write(slag + " ");

                }

                Console.WriteLine("= " + omgångSumma);
            }

            double medelvärde = totalSumma / (double)antalOmgångar;

            Console.WriteLine($"Total summa: {totalSumma}");
            Console.WriteLine($"Medelvärde per omgång: {medelvärde}");

            ConsoleHelper.WaitBeforeExit();
        }

    }
}
