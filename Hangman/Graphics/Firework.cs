using ConsoleDrawing.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing;
using Helpers;
using ConsoleDrawing.Structs;

namespace Hangman.Graphics
{
    public class Firework : Moving
    {
        public const float GRAVITY = 9.81f;
        protected Trail trail;
        protected float start;
        protected float lifetime;

        public Firework(float lifetime = 5) : base()
        {
            Position = new Vector2(Drawing.Width * 0.5f, Drawing.Height * .5f);
            trail = new Trail(2, Drawing.COLOR_LIGHT_BLUE);
            trail.SetParent(this);
            Velocity = new Vector2(RandomHelper.Range(-2f, 2f), -20);

            start = Time.Seconds;
            this.lifetime = lifetime;
        }

        public override void Draw()
        {
            Drawing.BackgroundColor = Drawing.COLOR_LIGHT_BLUE;
            Drawing.FillPoint(ApproxPosition, ' ');
        }

        public override void Update()
        {
            base.Update();

            Velocity += Vector2.Down * Time.DeltaTime * GRAVITY;

            if (Time.Seconds - start > lifetime)
            {
                var particle = new Particle
                {
                    Position = Position
                };

                Destroy();
            }
        }

        public class Particle : Moving
        {
            public const float GRAVITY = 9.81f;

            public Particle() : base()
            {
                var trail = new Trail(0.2, Drawing.COLOR_CYAN);

                trail.SetParent(this);

                Destroy(RandomHelper.Range(3.5f, 5f));
            }

            public override void Draw()
            {
                Drawing.BackgroundColor = Drawing.COLOR_LIGHT_CYAN;
                Drawing.FillPoint(ApproxPosition, ' ');
            }

            public override void Update()
            {
                base.Update();

                // Gravity
                Velocity += Vector2.Down * Time.DeltaTime * GRAVITY;
            }
        }
    }
}
