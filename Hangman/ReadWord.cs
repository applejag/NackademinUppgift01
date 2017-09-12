using System;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Hangman.Graphics;

namespace Hangman
{
    public class ReadWord : Drawable
    {
        const int ANTAL_GISSNINGAR = 5;

        protected Text titleText;
        protected Text promptText;
        protected TextField inputField;

        public ReadWord() : base(null)
        {
            titleText = new Text("Den som ska spela måste titta bort nu!", parent: this);
            promptText = new Text("Skriv in ett ord: ", parent: this) { LocalPosition = Point.Down};
            inputField = new TextField(promptText) {LocalPosition = Point.Right * promptText.text.Length};

            inputField.Changed += InputFieldOnChanged;
            inputField.Submitted += InputFieldOnSubmitted;
        }

        private void InputFieldOnSubmitted(TextField field)
        {
            string parsed = SecretWord.Parse(field.text);
            if (string.IsNullOrEmpty(parsed)) return;

            var secretWord = new SecretWord(parsed, ANTAL_GISSNINGAR);
            new Controller(secretWord);

            Destroy();
        }

        private void InputFieldOnChanged(TextField field)
        {
            field.text = SecretWord.Parse(field.text);
        }

        protected override void Update()
        {
            //throw new System.NotImplementedException();
        }

        protected override void Draw()
        {
            //throw new System.NotImplementedException();
        }
    }
}