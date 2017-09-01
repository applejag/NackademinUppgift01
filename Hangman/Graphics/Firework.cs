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
        protected Trail trail;
        protected float start;
        protected float lifetime;

        public Firework(float lifetime = 5) : base()
        {
            Position = new Vector2(RandomHelper.Range(0f, Drawing.Width), Drawing.Height);
            trail = new Trail(2, Drawing.COLOR_LIGHT_BLUE);
            velocity = new Vector2(-20, RandomHelper.Range(-2f, 2f));

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

            if (Time.Seconds - start > lifetime)
            {
                var particle = new Particle
                {
                    Position = new Vector2()
                    
                };

                Destroy();
            }
        }

        public class Particle : Moving
        {
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
                velocity += Vector2.Down * Time.DeltaTime;
            }
        }
    }
}
