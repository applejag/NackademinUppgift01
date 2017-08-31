using ConsoleDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Kalles flygande-rut akvarium";
            var con = new Drawing();
            
            Block block1 = new Block(5,3);
            Block block2 = new Block(41,25,Drawing.BG_CYAN);
            Block block3 = new Block(100, 27,Drawing.BG_YELLOW);

            while (true)
            {
                con.BackgroundColor = Drawing.BG_RED;
                con.Fill(' ');

                block1.Update(con);
                block2.Update(con);
                block3.Update(con);

                con.Render();

                System.Threading.Thread.Sleep(1000 / 60);

            }

            Console.ReadKey();

        }

        public class Block
        {
            static Random random = new Random();
            static List<Block> allBlocks = new List<Block>();

            Drawing.SmallRect rect;
            int xVel = 1;
            int yVel = 1;

            public byte backgroundColor;

            public Block(int x, int y, byte color = Drawing.BG_GREEN)
            {
                backgroundColor = color;
                rect = new Drawing.SmallRect
                {
                    Top = (short)y,
                    Left = (short)x,
                    Width = 10,
                    Height = 3,
                };

                xVel = random.Next(2) == 1 ? 1 : -1;
                yVel = random.Next(2) == 1 ? 1 : -1;

                allBlocks.Add(this);
            }

            ~Block()
            {
                allBlocks.Remove(this);
            }

            public void Update(Drawing console)
            {
                // Move rect
                rect.Left += (short)xVel;
                rect.Top += (short)yVel;

                // Check collision with other blocks
                var predict = rect;

                foreach (Block other in allBlocks)
                {
                    if (other == this) continue;

                    predict.Left = (short)(rect.Left + xVel);
                    predict.Top = rect.Top;

                    if (predict.IsColliding(other.rect))
                    {
                        if (other.rect.Left > rect.Left)
                            xVel = -1;
                        else
                            xVel = 1;
                    }

                    predict.Left = rect.Left;
                    predict.Top = (short)(rect.Top + yVel);

                    if (predict.IsColliding(other.rect))
                    {
                        if (other.rect.Top > rect.Top)
                            yVel = -1;
                        else
                            yVel = 1;
                    }
                }

                // Check collision with walls
                if (rect.Left + rect.Width >= console.Width)
                    xVel = -Math.Abs(xVel);
                else if (rect.Left <= 0)
                    xVel = Math.Abs(xVel);

                if (rect.Top + rect.Height >= console.Height)
                    yVel = -Math.Abs(yVel);
                else if (rect.Top <= 0)
                    yVel = Math.Abs(yVel);

                // Draw
                console.BackgroundColor = backgroundColor;
                console.FillRect(rect, ' ');

                console.SetCursorPosition(rect.Left, rect.Top);
                console.Write(" Hello :) ");
            }
        }
    }
}
