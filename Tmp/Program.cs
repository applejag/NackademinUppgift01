using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmp
{
    class Program
    {
        const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

        static void Main(string[] args)
        {
            Console.Title = "Kalles flygande-rut akvarium";
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);

            Drawing.CursorVisible = false;

            /*
            int length = 2400;
            StringBuilder builder = new StringBuilder(length, length);
            Random generator = new Random();
            int numLetters = LETTERS.Length;
            for (int i=0; i<builder.MaxCapacity; i++)
            {
                builder.Append(LETTERS[generator.Next(numLetters)]);
            }

            string text = builder.ToString();
            bool rendered = false;

            while (true)
            {
                if (Drawing.Width != Console.WindowWidth || Drawing.Height != Console.WindowHeight || !rendered)
                {
                    Drawing.SetWindowSize(Console.WindowWidth, Console.WindowHeight);

                    Drawing.Clear();
                    Drawing.WriteLine("Hello world!");
                    Drawing.WriteLine("Foo bar");
                    Drawing.WriteLine("Moo doo");
                    Drawing.WriteLine(text);

                    Drawing.Render();
                    rendered = true;
                }
            }//*/
            
            Block block1 = new Block(5, 3, bgColor: Drawing.COLOR_BLACK);
            Block block2 = new Block(41, 25, bgColor: Drawing.COLOR_LIGHT_CYAN);
            Block block3 = new Block(100, 27, bgColor: Drawing.COLOR_LIGHT_MAGENTA);
            Block block4 = new Block(Drawing.Width, Drawing.Height-3, bgColor: Drawing.COLOR_LIGHT_GREEN);

            while (true)
            {
                if (Drawing.Width != Console.WindowWidth || Drawing.Height != Console.WindowHeight)
                    Drawing.SetWindowSize(Console.WindowWidth, Console.WindowHeight);

                Drawing.BackgroundColor = Drawing.COLOR_RED;
                Drawing.Fill(' ');

                //con.BackgroundColor = Drawing.COLOR_LIGHT_RED;
                //con.FillLine(block1.X, block1.Y, block2.X, block2.Y, ' ');
                //con.FillLine(block2.X, block2.Y, block3.X, block3.Y, ' ');
                //con.FillLine(block3.X, block3.Y, block4.X, block4.Y, ' ');
                //con.FillLine(block4.X, block4.Y, block1.X, block1.Y, ' ');


                block1.trail.Draw();
                block2.trail.Draw();
                block3.trail.Draw();
                block4.trail.Draw();
                block1.Update();
                block2.Update();
                block3.Update();
                block4.Update();

                Drawing.Render();

                System.Threading.Thread.Sleep(1000 / 45);

            }

#pragma warning disable CS0162 // Unreachable code detected
            Console.ReadKey();
#pragma warning restore CS0162 // Unreachable code detected

        }

        public class Block
        {
            static Random random = new Random();
            static List<Block> allBlocks = new List<Block>();
            
            public byte backgroundColor;

            Rect rect;
            int xVel = 1;
            int yVel = 1;
            public readonly Trail trail;

            public Block(int x, int y, byte bgColor)
            {
                backgroundColor = bgColor;
                rect = new Rect (x, y, 10, 3);

                xVel = random.Next(2) == 1 ? 1 : -1;
                yVel = random.Next(2) == 1 ? 1 : -1;

                allBlocks.Add(this);

                trail = new Trail(2, bgColor);

                trail.Position = rect.Center;
            }

            ~Block()
            {
                allBlocks.Remove(this);
            }

            public void Update()
            {
                // Move rect
                rect.x += (short)xVel;
                rect.y += (short)yVel;

                // Check collision with other blocks
                var predict = rect;

                foreach (Block other in allBlocks)
                {
                    if (other == this) continue;

                    predict.x = (short)(rect.x + xVel);
                    predict.y = rect.y;

                    if (predict.IsColliding(other.rect))
                    {
                        if (other.rect.x > rect.x)
                            xVel = -1;
                        else
                            xVel = 1;
                    }

                    predict.x = rect.x;
                    predict.y = (short)(rect.y + yVel);

                    if (predict.IsColliding(other.rect))
                    {
                        if (other.rect.y > rect.y)
                            yVel = -1;
                        else
                            yVel = 1;
                    }
                }

                // Check collision with walls
                if (rect.x + rect.width >= Drawing.Width)
                    xVel = -Math.Abs(xVel);
                else if (rect.x <= 0)
                    xVel = Math.Abs(xVel);

                if (rect.y + rect.height >= Drawing.Height)
                    yVel = -Math.Abs(yVel);
                else if (rect.y <= 0)
                    yVel = Math.Abs(yVel);

                // Draw
                trail.Position = rect.Center;
                trail.Update();

                Drawing.BackgroundColor = backgroundColor;
                Drawing.FillRect(rect, ' ');

                Drawing.SetCursorPosition(rect.x, rect.y);
                Drawing.Write(" Hello :) ");
            }
        }
    }
}
