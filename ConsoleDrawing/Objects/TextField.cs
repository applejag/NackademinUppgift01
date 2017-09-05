using System;
using ConsoleDrawing.Structs;

namespace ConsoleDrawing.Objects
{
    public class TextField : Text
    {
        public event Action<TextField> OnSubmit;
        public bool selected = true;

        public TextField(string text, Drawable parent = null) : base(text, parent)
        {
            ZDepth = -5;
        }

        public TextField(Drawable parent = null) : base(parent)
        {}

        public override void Update()
        {
            base.Update();

            if (!selected) return;

            text += Input.InputString;

            if (Input.GetKeyDown(ConsoleKey.Backspace) && text.Length > 0)
                text = text.Substring(0, Math.Max(text.Length - 2, 0));

            if (Input.GetKeyDown(ConsoleKey.Enter))
            {
                OnSubmit?.Invoke(this);
                text = string.Empty;
            }
        }

        public override void Draw()
        {
            base.Draw();

            Drawing.CursorVisible = selected;
            Drawing.SetCursorBlinkPosition(ApproxPosition + Point.Right * text.Length);
        }
    }
}