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
    public class SecretWordText : Text
    {
        private SecretWord word;

        public SecretWordText(SecretWord word) : base()
        {
            this.word = word;
            ZDepth = -1;
        }

        public override void Update()
        {
            base.Update();

            text = word.RenderWord();
            Position = new Vector2((Drawing.BufferWidth - text.Length) * 0.5f, Drawing.BufferHeight * 0.4f);
        }

        public override void Draw()
        {
            int width = text.Length;
            Point approx = ApproxPosition;
            Rect rect = new Rect
            {
                x = approx.x - 3, y = approx.y - 2,
                width = width + 6, height = 5,
            };

            Drawing.BackgroundColor = Color.BLUE;
            Drawing.FillRect(rect, ' ');

            //rect.width -= 2;
            //rect.height -= 2;
            //rect.x += 1;
            //rect.y += 1;
            //Drawing.BackgroundColor = Color.BLUE;
            //Drawing.FillRect(rect, ' ');

            base.Draw();
        }
    }
}
