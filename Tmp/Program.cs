using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Hangman.Graphics;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmp
{
    class Program
    {
        const string HANGMAN =
              "                   ,,,,,,,,,,,\n"
            + "                   ,,,,,,,,,,,\n"
            + "                    ,,,,,,,,,,\n"
            + "  +@@@@@@@@@@        ,,,,,,,,,\n"
            + "  +    '#   |           ,,,,,,\n"
            + "  +  '#     |               ,,\n"
            + "  +'#       |                 \n"
            + "  +         |                 \n"
            + "  +  @'+ ;@; ## '@            \n"
            + "  +    +........@ ;           \n"
            + "  ++  ;. +...+...+ #          \n"
            + "  ++  #....;.....@ +          \n"
            + "  +;   ..........  +          \n"
            + "  +     @.'.,..#   +          \n"
            + "  + ;      '                  \n"
            + "  +        @                  \n"
            + "  +       @' @                \n"
            + "  +      + '  ;               \n"
            + "  +     ;  '    @             \n"
            + "  +   ;    '     ;            \n"
            + "  +        '                  \n"
            + "  +        '                  \n"
            + "  +        '                  \n"
            + "  +        '                  \n"
            + "  +        ;'                 \n"
            + "  +       # '                 \n"
            + "  +      #   +                \n"
            + "  +     ;    '                \n"
            + "  +     ;     #    ,,,, ,,,,,.\n"
            + "  +    @          ;     ,..,. \n"
            + "  +                         , \n"
            + "  @@       ;;'''' ,. .  `.,.,'\n"
            + " @; @'''''''''''''''''''''''''\n"
            + "+''''@''''''''''''''''''''''''";

        const string YOULOST =
              "██╗   ██╗ ██████╗ ██╗   ██╗    ██╗      ██████╗ ███████╗████████╗\n"
            + "╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║     ██╔═══██╗██╔════╝╚══██╔══╝\n"
            + " ╚████╔╝ ██║   ██║██║   ██║    ██║     ██║   ██║███████╗   ██║   \n"
            + "  ╚██╔╝  ██║   ██║██║   ██║    ██║     ██║   ██║╚════██║   ██║   \n"
            + "   ██║   ╚██████╔╝╚██████╔╝    ███████╗╚██████╔╝███████║   ██║   \n"
            + "   ╚═╝    ╚═════╝  ╚═════╝     ╚══════╝ ╚═════╝ ╚══════╝   ╚═╝   ";


        const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

        static void Main(string[] args)
        {
            Fireworks(args);
            ConsoleHelper.WaitBeforeExit();
        }

        static void Fireworks(string[] args)
        {
            Console.Title = "Kalles fyverkerier!";
            Console.SetWindowSize(100, 34);
            Drawing.SetWindowSize();

            Drawing.FixedSize = true;
            Drawing.CursorVisible = false;

            PlayFireworkLaunchSound();
            new Firework(PlayFireworkExplosionSound);

            new Text { text = HANGMAN };

            var flashingText = new FlashingText
            {
                text = YOULOST,
                foregrounds = new byte[] {Colors.LIGHT_RED, Colors.GREY},
                Position = new Vector2(30, 10)
            };

            void FireworkOnSpace()
            {
                if (Input.GetKeyDown(ConsoleKey.Spacebar))
                {
                    PlayFireworkLaunchSound();
                    new Firework(PlayFireworkExplosionSound);
                }
            }

            var dummy = new Dummy
            {
                OnUpdate = FireworkOnSpace,
            };
            
            Time.RunFrameTimer();
        }

        public static string soundFireworkLaunch = @"C:\Users\kalle\Downloads\firework_launch_sfx_01.wav";

        public static string[] soundFireworkExplosion =
        {
            @"C:\Users\kalle\Downloads\firework_sfx_01.wav",
            @"C:\Users\kalle\Downloads\firework_sfx_02.wav",
        };

        public static void PlayFireworkLaunchSound()
        {
            PlaySound(soundFireworkLaunch);
        }

        public static void PlayFireworkExplosionSound()
        {
            var uri = soundFireworkExplosion[RandomHelper.Range(soundFireworkExplosion.Length)];
            PlaySound(uri);
        }

        public static void PlaySound(string uri)
        {
            var sound = new System.Windows.Media.MediaPlayer();
            sound.Open(new Uri(uri));
            sound.Play();
        }

        private static void Hängagubbe(string[] args)
        {
            var försök = 0; //Antal spel
            bool vunnit = false;

            Console.WriteLine("Den som ska spela måste titta bort nu! ");
            Console.Write("Skriv in ett ord: ");
            var rättSvar = Console.ReadLine().ToUpper(); //Facit för spelet att jämföra med

            char[] rättSvariArray = rättSvar.ToCharArray(); //Stoppar in svaret i en charArray
            char[] gissadeBokstäver = new char[rättSvar.Length]; // gissningar ska sparas och ersättas när det är rätt

            for (int i = 0; i < gissadeBokstäver.Length; i++)
            {
                gissadeBokstäver[i] = '*';
            }

            Console.WriteLine("Enter för att fortsätta...");
            Console.ReadLine();
            Console.Clear();

            //Gissningarna
            while (försök < 6 && vunnit == false)
            {
                string input;
                försök++;
                string testaGissnigar;

                //Information till spelare
                for (int i = 0; i < gissadeBokstäver.Length; i++)
                {
                    Console.Write(gissadeBokstäver[i]);
                }
                Console.WriteLine("\nFörsök: {0}/6", försök);

                Console.WriteLine("\nGissa en bokstav eller hela ord patron");
                input = Console.ReadLine().ToUpper();   //För att testa hela ordet


                if (input == rättSvar)  //Testar om ordet är korrekt i sin helhet
                {
                    Console.WriteLine("Grattis du har gissat rätt");
                    Console.WriteLine("Gissat ord är {0}", rättSvar);
                    Console.WriteLine("Du gissade på {0} försök", försök);
                    Console.ReadLine();
                    vunnit = true;
                }

                else if (input.Length == 1)  //Testar nybokstav
                {
                    var inputIChars = char.Parse(input); //För att testa bokstaven om gissaren har testat en bokstav. 


                    for (int i = 0; i < rättSvariArray.Length; i++)
                    {
                        if (inputIChars == gissadeBokstäver[i])
                        {
                            Console.WriteLine("Du har redan gissat denna bokstav");
                            försök++;
                            Console.WriteLine("Du har nu {0} försök kvar", 6 - försök);
                            Console.ReadLine();
                        }
                        else if (rättSvariArray[i] == inputIChars)
                        {
                            gissadeBokstäver[i] = inputIChars;
                            Console.WriteLine("Bra gissat, {0} är rätt", input);
                            försök++;
                            Console.ReadLine();
                        }

                        testaGissnigar = new string(gissadeBokstäver); //För att testa om bokstäverna blivit mitt ord

                        if (testaGissnigar == rättSvar)
                        {
                            vunnit = true;
                        }
                    }
                }

                else  //borde hantera alla felskrivningar. eller?
                {
                    Console.WriteLine("FEL!!!\nDu har {0} försök kvar", 6 - försök);
                    Console.ReadLine();
                }

                Console.Clear();

            }

            //Avslutningsmeddelande

            if (försök < 6)
            {
                Console.WriteLine("GRATTIS!!! DU ÄR EN VINNARE\n Du klarade det på {0} försök", försök);
            }
            else
            {
                Console.WriteLine("Du förlorade");
            }

        }
    }
}
