using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Helpers;

namespace Hangman.Graphics
{
    public class YouWonText : AnimatedText
    {
        protected float moveSpeed;
        protected int height;

        public YouWonText() : base("Animations/YouWon.txt")
        {
            height = frames.Max(x => x.Split(StringHelper.Newlines, StringSplitOptions.None).Length);
            Position = Vector2.Up * height;
            foregroundColor = Color.LIGHT_GREEN;
            ZDepth = 2;
            alignment = Alignment.Center;
        }

        protected override void Update()
        {
            base.Update();

            Position = new Vector2(Drawing.BufferWidth * 0.5f, Position.y);

            if (moveSpeed < 0)
            {
                Position = new Vector2(Position.x, Drawing.BufferHeight - height);
            }
            else
            {
                moveSpeed += Time.DeltaTime;
                Position += Vector2.Down * moveSpeed * Time.DeltaTime;

                if (Position.y >= Drawing.BufferHeight - height)
                {
                    Position = new Vector2(Position.x, Drawing.BufferHeight - height);
                    moveSpeed = -1;
                }
            }

        }

        protected override void Draw()
        {
            base.Draw();
        }
    }
}
