using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Graphics
{
    public class Airplane : AnimatedText
    {
        public float speed = 6;

        public Airplane() : base()
        {
            frames = LoadFromFile("Animations/Airplane.txt");
            Position = new Vector2(Drawing.BufferWidth, 15);
            foregroundColor = Color.LIGHT_YELLOW;
            ZDepth = -2;
            interval = 0.2f;
        }

        public override void Update()
        {
            base.Update();

            Position += Vector2.Left * Time.DeltaTime * speed;
            if (Position.x < -51) Destroy();
        }

    }
}
