using ConsoleDrawing.Objects;

namespace Hangman
{
    public class LoseScreen : Drawable
    {
        protected SecretWord secretWord;

        public LoseScreen(SecretWord secretWord) : base(null)
        {
            this.secretWord = secretWord;
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