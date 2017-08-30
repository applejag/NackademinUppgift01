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

        public static bool PointInBounds(int x, int y)
        {
            return x >= 0 && x < Console.WindowWidth
                && y >= 0 && y < Console.WindowHeight;
        }

    }

    public class ConsoleBuffer
    {
        private Pixel[,] pixels;

        public int CursorX { get; set; }
        public int CursorY { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int BufferWidth { get; }
        public int BufferHeight { get; }

        public ConsoleBuffer(int width, int height)
        {
            BufferWidth = width;
            BufferHeight = height;

            pixels = new Pixel[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixels[x,y] = new Pixel();
                }
            }
        }

        public void ApplyRow(int row)
        {
            //Pixel.DrawRow(OffsetX, OffsetY, pixels[row]);
        }

        public class Pixel
        {
            public char character = ' ';
            public ConsoleColor foregroundColor = ConsoleColor.Gray;
            public ConsoleColor backgroundColor = ConsoleColor.Black;

            public void DrawAt(int x, int y)
            {
                if (ConsoleHelper.PointInBounds(x,y))
                {
                    Console.CursorLeft = x;
                    Console.CursorTop = y;
                    Console.ForegroundColor = foregroundColor;
                    Console.BackgroundColor = backgroundColor;
                    Console.Write(character);
                }
            }

            public static void DrawRow(int x, int y, params Pixel[] pixels)
            {
                if (ConsoleHelper.PointInBounds(Math.Max(x, 0), y) == false)
                    return;


                Console.CursorLeft = x;
                Console.CursorTop = y;

                int length = pixels.Length;
                int startAt = x < 0 ? -x : 0;
                int stopAt = Math.Max(Console.WindowWidth - x, length);

                for (int i = startAt; i < stopAt; i++)
                {
                    Pixel pixel = pixels[i];
                    Console.ForegroundColor = pixel.foregroundColor;
                    Console.BackgroundColor = pixel.backgroundColor;
                    Console.Write(pixel.character);
                }
            }
        }
    }
}
