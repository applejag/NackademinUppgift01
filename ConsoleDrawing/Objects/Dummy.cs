using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public class Dummy : Drawable
    {
        public Action OnUpdate;
        public Action OnDraw;

        public override void Update()
        {
            OnUpdate?.Invoke();
        }

        public override void Draw()
        {
            OnDraw?.Invoke();
        }
    }
}
