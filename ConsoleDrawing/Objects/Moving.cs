using ConsoleDrawing.Structs;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public abstract class Moving : Drawable
    {
        public Vector2 Velocity { get; set; }

        public override void Update()
        {
            LocalPosition += Velocity * Time.DeltaTime;
        }
    }
}
