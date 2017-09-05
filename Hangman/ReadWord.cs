using System;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;

namespace Hangman
{
    public class ReadWord : Drawable
    {
        const int ANTAL_GISSNINGAR = 5;

        protected Text titleText;
        protected Text promptText;
        protected TextField inputField;
        protected Text previewLabelText;
        protected Text previewText;

        public ReadWord() : base(null)
        {
            titleText = new Text("Den som ska spela måste titta bort nu!", parent: this);
            promptText = new Text("Skriv in ett ord: ", parent: this) { LocalPosition = Point.Down};
            inputField = new TextField(promptText) {LocalPosition = Point.Right * promptText.text.Length};
            previewLabelText = new Text("Preview: ", parent: this) {LocalPosition = Point.Down * 3};
            previewText = new Text(parent: previewLabelText)
            {
                LocalPosition = Point.Right * previewLabelText.text.Length,
                foregroundColor = Color.WHITE,
            };

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
            string preview = SecretWord.RenderPreview(field.text);
            previewText.text = preview;
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