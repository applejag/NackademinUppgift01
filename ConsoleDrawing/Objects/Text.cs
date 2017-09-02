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
        public string text;
        public int maxWidth;
        public byte foregroundColor = Colors.GREY;
        public byte backgroundColor = Colors.BLACK;

        private string[] SplitString()
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
            string[] parts = SplitString();

            foreach (string part in parts)
            {
                Drawing.SetCursorPosition(approx);
                Drawing.Write(part);

                approx.y += 1;
            }
        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
