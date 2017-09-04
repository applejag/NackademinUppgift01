using ConsoleDrawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public class Text : Drawable
    {
        private string[] _lines;
        private string _text;
        public string text {
            get => _text;
            set => _lines = SplitString(_text = value, maxWidth);
        }
        public int maxWidth;
        public Color? foregroundColor = Color.GREY;
        public Color? backgroundColor = null;

        private static string[] SplitString(string text, int maxWidth)
        {
            List<string> list = new List<string>(text.Split('\n'));

            if (maxWidth > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length > maxWidth)
                    {
                        if (list[i].Length != maxWidth + 1)
                            list.Insert(i + 1, list[i].Substring(maxWidth + 1));
                        list[i] = list[i].Substring(0, maxWidth);
                    }
                }
            }

            return list.ToArray();
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
