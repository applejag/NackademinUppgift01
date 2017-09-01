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

        public Trail(double time = 5, byte color = Drawing.COLOR_LIGHT_CYAN) : base()
        {
            TimeToLive = time;
            Color = color;
        }

        public override void Draw()
        {
            if (points == null) return;

            int numPoints = points.Count;
            if (numPoints == 1)
            {
                Drawing.BackgroundColor = Color;
                Drawing.FillPoint(ApproxPosition, ' ');
            }
            else if (numPoints > 1)
            {
                PointInTime last = points[0];
                Drawing.BackgroundColor = Color;

                for (int i = 1; i < numPoints; i++)
                {
                    PointInTime point = points[i];
                    Drawing.FillLine(last.position, point.position, ' ');
                    last = point;
                }
            }
        }

        public override void Update()
        {
            double now = Time.Seconds;
            points.Add(new PointInTime { position = ApproxPosition, timestamp = now });
            points.RemoveAll(p => now - p.timestamp > TimeToLive);
        }

        private struct PointInTime
        {
            public Point position;
            public double timestamp;
        }
    }
}
