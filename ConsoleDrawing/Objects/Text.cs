using ConsoleDrawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace ConsoleDrawing.Objects
{
    public class Text : Drawable
    {
        private string[] _lines;
        private string _text;
        public string text {
            get => _text;
            set => _lines = StringHelper.SplitString(_text = value, maxWidth);
        }
        public int maxWidth;
        public Color? foregroundColor = Color.GREY;
        public Color? backgroundColor = null;

        public Text(string text, Drawable parent = null) : base(parent)
        {
            this.text = text;
        }

        public Text(Drawable parent = null) : base(parent)
        {
            this.text = string.Empty;
        }

        public override void Draw()
        {
            Drawing.ForegroundColor = foregroundColor;
            Drawing.BackgroundColor = backgroundColor;
            Point approx = ApproxPosition;
            int left = Math.Max(-approx.x, 0);
            if (approx.x < 0) approx.x = 0;

            foreach (string part in _lines)
            {
                if (left <= part.Length) {
                    Drawing.SetCursorPosition(approx);
                    Drawing.Write(part.Substring(left));
                }

                approx.y += 1;
            }
        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
