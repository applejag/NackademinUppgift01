using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace Hangman.Graphics
{
    public class Airplane : AnimatedText
    {
        public float speed = 6;
        private int width;

        public Airplane() : base("Animations/Airplane.txt")
        {
            GotoSpawnPos();
            foregroundColor = Color.LIGHT_YELLOW;
            ZDepth = -2;
            interval = 0.2f;

            width = frames.Max(x => x.Split(StringHelper.Newlines, StringSplitOptions.None).Max(j => j.Length));
        }

        protected override void Update()
        {
            base.Update();

            Position += Vector2.Left * Time.DeltaTime * speed;
            if (Position.x < -width) GotoSpawnPos();
        }

        protected void GotoSpawnPos()
        {
            Position = new Vector2(Drawing.BufferWidth + RandomHelper.Range(4, 10), RandomHelper.Range(14, 17));
        }

    }
}
