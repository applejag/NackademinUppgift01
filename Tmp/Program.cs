using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Hangman.Graphics;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmp
{
    class Program
    {
        const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

        static void Main(string[] args)
        {
            Console.Title = "Kalles flygande-rut akvarium";
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);

            Drawing.CursorVisible = false;
            Time.StartFrameTimer();
            var firework = new Firework();
            
            List<TestThingie> myThingies = new List<TestThingie>
            {
                new TestThingie(),
                new TestThingie(),
                new TestThingie(),
            };

            var otherThingie = new TestThingie();
            otherThingie.SetParent(myThingies[2]);

            var line1 = new Line(new Point(), new Point((int)(Drawing.Width * 0.5f), Drawing.Height));
            var line2 = new Line(new Point(Drawing.Width, 0), new Point((int)(Drawing.Width * 0.5f), Drawing.Height));

            while (true)
            {
                var info = Console.ReadKey(true);

                if (info.Key == ConsoleKey.Spacebar)
                {
                    myThingies.ForEach(t => t.Destroy());
                    myThingies = new List<TestThingie>
                    {
                        new TestThingie(),
                        new TestThingie(),
                        new TestThingie(),
                        new TestThingie(),
                        new TestThingie(),
                    };
                }
            }

        }

        public class TestThingie : Moving
        {
            public TestThingie()
            {
                Position = new Vector2(RandomHelper.Range(Drawing.Width), RandomHelper.Range(Drawing.Height));
            }

            public override void Draw()
            {
                Drawing.BackgroundColor = Drawing.COLOR_CYAN;
                Drawing.FillLine(new Point(), ApproxPosition, 'a');
            }
        }

        public class Line : Drawable
        {
            public Point point1;
            public Point point2;

            public Line(Point point1, Point point2) : base()
            {
                this.point1 = point1;
                this.point2 = point2;
            }

            public override void Draw()
            {
                Drawing.BackgroundColor = Drawing.COLOR_LIGHT_GREEN;
                Drawing.FillLine(point1, point2, ' ');
            }

            public override void Update()
            {
                //
            }
        }
    }
}
