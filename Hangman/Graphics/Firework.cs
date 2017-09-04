using ConsoleDrawing.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing;
using Helpers;
using ConsoleDrawing.Structs;
using System.Media;

namespace Hangman.Graphics
{
    public class Firework : Moving
    {
        public static readonly byte[] colors = { Colors.CYAN, Colors.GREEN, Colors.RED, Colors.YELLOW };

        public const float GRAVITY = 9.81f;
        protected Trail trail;
        protected float start;
        protected float lifetime;

        byte color;
        byte lightColor;

        Action onExplosion;

        public Firework(Action onExplosion) : base()
        {
            this.onExplosion = onExplosion;

            color = colors[RandomHelper.Range(colors.Length)];
            lightColor = (byte)(color | Colors.INTENSITY);

            Position = new Vector2(Drawing.BufferWidth * RandomHelper.Float, Drawing.BufferHeight);
            trail = new Trail(0.5f, color);
            trail.SetParent(this, false);
            Velocity = new Vector2(RandomHelper.Range(-5f, 5f), -RandomHelper.Range(0.5f, 0.8f) * Drawing.BufferHeight);

            start = Time.Seconds;
            lifetime = RandomHelper.Range(1.5f, 2.2f);
        }

        public override void Draw()
        {
            Drawing.BackgroundColor = lightColor;
            Drawing.FillPoint(ApproxPosition, ' ');
        }

        public override void Update()
        {
            base.Update();

            Velocity += Vector2.Down * Time.DeltaTime * GRAVITY;

            if (Time.Seconds - start > lifetime)
            {
                // Create explosion
                for (int i = 0; i < 50; i++)
                {
                    var particle = new Particle
                    {
                        Position = Position,
                        color = RandomHelper.Float >= 0.3f ? lightColor : color,
                    };
                }

                // Drop trail
                trail.SetParent(null);
                trail.SelfDestruct = true;
                
                try
                {
                    onExplosion?.Invoke();
                } catch { /* Failed to play sound */ }

                Destroy();
            }
        }

        public class Particle : Moving
        {
            public const float GRAVITY = 9.81f;
            public byte color;

            private bool isForeground;

            public Particle() : base()
            {
                float lifetime = RandomHelper.Float;

                Velocity = Vector2.FromDegrees(RandomHelper.Range(360f), 2 + 8 * lifetime);

                Destroy(1 + lifetime * 1);

                isForeground = RandomHelper.Boolean;
            }

            public override void Draw()
            {
                if (isForeground)
                {
                    Drawing.BackgroundColor = Colors.BLACK;
                    Drawing.ForegroundColor = color;
                    Drawing.FillPoint(ApproxPosition, '*');
                }
                else
                {
                    Drawing.BackgroundColor = color;
                    Drawing.FillPoint(ApproxPosition, ' ');
                }
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
