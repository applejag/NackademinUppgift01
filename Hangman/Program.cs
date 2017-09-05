using ConsoleDrawing;
using ConsoleDrawing.Objects;
using Hangman.Graphics;
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
            Drawing.SetWindowSize(120, 35);
            Drawing.FixedSize = true;
            Drawing.CursorVisible = false;

            Console.WriteLine("Den som ska spela måste titta bort nu!");
            Console.Write("Skriv in ett ord: ");

            var hemligaOrdet = new SecretWord(Console.ReadLine(), ANTAL_GISSNINGAR);
            
            new Controller(hemligaOrdet);
            Time.RunFrameTimer();
            
            /*
            Console.WriteLine("Ord: {0}", hemligaOrdet.RenderWord());

            if (hemligaOrdet.Finished)
                Console.WriteLine("Grattis!! Du fann ordet med hela {0} fel!", hemligaOrdet.Errors);
            else
                Console.WriteLine("Tyvärr, men du lyckades inte finna ordet..");
                */
            ConsoleHelper.WaitBeforeExit();

        }

        

    }
}
