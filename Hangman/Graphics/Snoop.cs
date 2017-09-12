using System;
using System.Linq;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Helpers;

namespace Hangman.Graphics
{
    public class Snoop : AnimatedText
    {
        private int height;
        private int width;
        
        private bool movingRight = true;
        private float speed = 2;

        public Snoop(Drawable parent = null) : base("Animations/Snoop.txt", parent)
        {
            width = frames.Max(x => x.Split(StringHelper.Newlines, StringSplitOptions.None).Max(j => j.Length));
            height = frames.Max(x => x.Split(StringHelper.Newlines, StringSplitOptions.None).Length);

            foregroundColor = Color.GREEN;
            interval = 0.15f;
            ZDepth = 3;

            Position = new Vector2(-width, Drawing.BufferHeight - height);
        }

        protected override void Update()
        {
            base.Update();

            if (movingRight)
            {
                Position += Vector2.Right * Time.DeltaTime * speed;

                if (Position.x + width >= Drawing.BufferWidth)
                {
                    Position = new Vector2(Drawing.BufferWidth - width, Position.y);
                    movingRight = false;
                }
            }
            else
            {
                Position += Vector2.Left * Time.DeltaTime * speed;

                if (Position.x < 0)
                {
                    Position = new Vector2(0, Position.y);
                    movingRight = true;
                }
            }
        }
    }
}