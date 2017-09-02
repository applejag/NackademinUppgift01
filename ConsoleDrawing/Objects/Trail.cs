using ConsoleDrawing.Structs;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public class Trail : Drawable
    {
        private List<PointInTime> points = new List<PointInTime>();

        public double TimeToLive { get; set; }
        public byte Color { get; set; }
        public bool SelfDestruct { get; set; } = false;

        private Point oldPosition;

        public Trail(double time = 5, byte color = Colors.LIGHT_CYAN) : base()
        {
            TimeToLive = time;
            Color = color;
        }

        public override void Draw()
        {
            int numPoints = points?.Count ?? 0;
            if (numPoints > 1)
            {
                PointInTime last = points[0];
                Drawing.BackgroundColor = Color;

                for (int i = 1; i < numPoints; i++)
                {
                    PointInTime point = points[i];
                    Drawing.FillLine(last.position.x, last.position.y, point.position.x, point.position.y, ' ');
                    last = point;
                }
            }
            else
            {
                Drawing.BackgroundColor = Color;
                Drawing.FillPoint(ApproxPosition, ' ');
            }
        }

        public override void Update()
        {
            Point approx = ApproxPosition;

            double now = Time.Seconds;

            // Add new point if moved
            if (approx.x != oldPosition.x || approx.y != oldPosition.y)
                points.Add(new PointInTime { position = approx, timestamp = now });

            // Remove old points
            points.RemoveAll(p => now - p.timestamp > TimeToLive);

            // Kys
            if (SelfDestruct && points.Count == 0)
                Destroy();

            oldPosition = approx;
        }

        private struct PointInTime
        {
            public Point position;
            public double timestamp;
        }
    }
}
