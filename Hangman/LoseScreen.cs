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

        public override void Update()
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw()
        {
            //throw new System.NotImplementedException();
        }
    }
}