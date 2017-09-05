using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Hangman.Graphics;
using Helpers;

namespace Hangman
{
    public class WinScreen : Drawable
    {
        protected SecretWord secretWord;
        protected Text wordText;

        private Vector2 startPos;
        private float moveLapsed = 0;
        private const float moveDuration = 3;

        private Drawable container;
        private float moveAwaySpeed;

        public WinScreen(SecretWord secretWord, Text wordText, Text[] otherTexts) : base(null)
        {
            container = new Dummy();
            foreach (Text text in otherTexts)
            {
                text.SetParent(container);
            }

            this.secretWord = secretWord;
            this.wordText = wordText;
            
            wordText.Parent = this;
            int blen = wordText.text.Length;
            wordText.text = secretWord.RenderWord();
            int alen = wordText.text.Length;
            wordText.Position += Point.Right * (blen - alen);

            startPos = wordText.Position;
        }

        public override void Update()
        {
            if ((container?.Destroyed ?? true) == false)
            {
                moveAwaySpeed += Time.DeltaTime;
                container.LocalPosition += Vector2.Left * moveAwaySpeed * Time.DeltaTime;
                if (container.LocalPosition.x < -50)
                    container.Destroy();
            }

            if (moveLapsed < moveDuration)
            {
                moveLapsed += Time.DeltaTime;
                Vector2 targetPos = new Vector2((Drawing.BufferWidth - wordText.text.Length) * 0.5f,
                    Drawing.BufferHeight - 10);

                if (moveLapsed >= moveDuration)
                {
                    wordText.Destroy();
                    
                    wordText = new FlashingText
                    {
                        Position = targetPos,
                        text = secretWord.RenderWord(),
                        foregrounds = new[] {Color.LIGHT_YELLOW, Color.WHITE},
                    };

                    new Firework();
                    new Airplane();
                    new Rainbow {showingAngle = -180,};
                }
                else
                {
                    float t = moveLapsed / moveDuration;
                    Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
                    newPos.y = MathHelper.SmoothDamp(newPos.y, startPos.y, targetPos.y);
                    wordText.Position = newPos;
                }
            }
        }

        public override void Draw()
        {
            //throw new System.NotImplementedException();
        }
    }
}