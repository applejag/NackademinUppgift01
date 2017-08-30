using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        const int ANTAL_GISSNINGAR = 5;

        static void Main(string[] args)
        {
            Console.WriteLine("Den som ska spela måste titta bort nu!");
            Console.Write("Skriv in ett ord: ");

            SecretWord hemligaOrdet = new SecretWord(Console.ReadLine(), ANTAL_GISSNINGAR);
            
            Console.Clear();

            while (hemligaOrdet.GameOver == false)
            {
                Console.WriteLine("Ord: {0}", hemligaOrdet.RenderWord());

                if (hemligaOrdet.AnyMisses)
                    Console.WriteLine("Missar: {0}", hemligaOrdet.RenderMisses());

                Console.Write("Gissa en bokstav eller ordet? ");

                string input = Console.ReadLine().Trim().ToUpper();
                int length = input.Length;

                Console.Clear();

                if (length == 1)
                {
                    char guess = input[0];

                    if (hemligaOrdet.IsGuessed(guess))
                        Console.Write("Du har redan försökt med {0}! ", guess);
                    else
                    {
                        if (hemligaOrdet.GuessLetter(guess))
                            Console.Write("Rätt! ");
                        else
                            Console.Write("Fel! ");
                    }
                }
                else if (length != 0)
                {
                    string guess = input;

                    if (hemligaOrdet.GuessWord(guess) == false)
                        Console.Write("Fel ord! ");
                    
                }

                Console.WriteLine("Du har {0} gissningar kvar.", hemligaOrdet.TriesLeft);
                Console.WriteLine();
            }

            Console.WriteLine("Ord: {0}", hemligaOrdet.RenderWord());

            if (hemligaOrdet.Finished)
                Console.WriteLine("Grattis!! Du fann ordet med hela {0} fel!", hemligaOrdet.Errors);
            else
                Console.WriteLine("Tyvärr, men du lyckades inte finna ordet..");

            ConsoleHelper.WaitBeforeExit();

        }

        

    }
}
