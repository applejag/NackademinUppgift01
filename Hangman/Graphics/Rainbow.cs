using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Graphics
{
    public class Rainbow : Drawable
    {
        public float showingAngle = 0;
        public float duration = 10;

        public Rainbow() : base()
        {
            ZDepth = 5;
        }

        protected override void Update()
        {
            if (showingAngle < 180)
            {
                showingAngle += (180 / 10f) * Time.DeltaTime;
            }
        }

        protected override void Draw()
        {
            int width = Drawing.BufferWidth;
            int height = Drawing.BufferHeight;
            Vector2 offset = new Vector2(width * 0.5f, height + 7);
            Vector2 size = new Vector2(width * 0.65f, height);
            
            for (float angle = 0; angle <= showingAngle; angle += 0.5f)
            {
                Vector2 vec = Vector2.Scale(Vector2.FromDegrees(angle), size) + offset;
                DrawRainbowCol((int)(vec.x + 0.5f), (int)(vec.y + 0.5f));
            }
        }

        private void DrawRainbowCol(int x, int y)
        {
            Drawing.BackgroundColor = Color.MAGENTA;
            Drawing.FillPoint(x, y, ' ');
            Drawing.BackgroundColor = Color.LIGHT_BLUE;
            Drawing.FillPoint(x, --y, ' ');
            Drawing.BackgroundColor = Color.LIGHT_GREEN;
            Drawing.FillPoint(x, --y, ' ');
            Drawing.BackgroundColor = Color.LIGHT_YELLOW;
            Drawing.FillPoint(x, --y, ' ');
            Drawing.BackgroundColor = Color.LIGHT_RED;
            Drawing.FillPoint(x, --y, ' ');
            Drawing.BackgroundColor = Color.RED;
            Drawing.FillPoint(x, --y, ' ');
        }

    }
}
