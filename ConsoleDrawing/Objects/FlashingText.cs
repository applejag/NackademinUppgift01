using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public class FlashingText : Text
    {
        public Color[] foregrounds;
        public Color[] backgrounds;

        /// <summary>
        /// Time before a color change in seconds.
        /// </summary>
        public float interval = 1;

        public override void Update()
        {
            int numForegrounds = foregrounds?.Length ?? 0;
            if (numForegrounds > 0)
            {
                int fgIndex = (int)(Time.Seconds / interval) % numForegrounds;
                foregroundColor = foregrounds[fgIndex];
            }

            int numbackgrounds = backgrounds?.Length ?? 0;
            if (numbackgrounds > 0)
            {
                int bgIndex = (int)(Time.Seconds / interval) % numbackgrounds;
                backgroundColor = backgrounds[bgIndex];
            }
        }

        //public override void Draw()
        //{
        //    base.Draw();
        //    Drawing.CursorX = 30;
        //    Drawing.CursorY = 20;
        //    Drawing.Write(Time.Seconds.ToString());
        //}
    }
}
