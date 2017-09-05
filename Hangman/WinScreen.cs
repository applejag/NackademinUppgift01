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
        private float wordMoveLapsed = 0;
        private float wordMoveSpeed = 0;

        private Drawable oldTextsContainer;
        private float oldTextsMoveSpeed;

        private const float fireworksDelay = 1;
        private float fireworksLapsed = -5;

        public WinScreen(SecretWord secretWord, Text wordText, Text[] otherTexts) : base(null)
        {
            oldTextsContainer = new Dummy();
            foreach (Text text in otherTexts)
            {
                text.SetParent(oldTextsContainer);
            }

            this.secretWord = secretWord;
            this.wordText = wordText;
            
            wordText.Parent = this;
            int blen = wordText.text.Length;
            wordText.text = secretWord.RenderWord();
            int alen = wordText.text.Length;
            wordText.Position += Point.Right * (blen - alen);

            startPos = wordText.Position;

            new YouWonText();
            new Snoop();
        }

        protected override void Update()
        {
            ShootFireworks();
            MoveOldTextObjects();
            MoveWordTextObject();
        }

        protected override void Draw()
        {
            //throw new System.NotImplementedException();
        }

        private void MoveOldTextObjects()
        {
            if ((oldTextsContainer?.Destroyed ?? true) == false)
            {
                oldTextsMoveSpeed += Time.DeltaTime;
                oldTextsContainer.LocalPosition += Vector2.Left * oldTextsMoveSpeed * Time.DeltaTime;
                if (oldTextsContainer.LocalPosition.x < -50)
                    oldTextsContainer.Destroy();
            }
        }

        private void ShootFireworks()
        {
            fireworksLapsed += Time.DeltaTime;
            if (fireworksLapsed > fireworksDelay)
            {
                fireworksLapsed = 0;
                new Firework {ZDepth = 3};
            }
        }

        private void MoveWordTextObject()
        {
            if (wordText is FlashingText) return;

            wordMoveSpeed += Time.DeltaTime * 0.05f;
            wordMoveLapsed += wordMoveSpeed * Time.DeltaTime;

            Vector2 targetPos = new Vector2((Drawing.BufferWidth - wordText.text.Length) * 0.5f,
                Drawing.BufferHeight - 10);

            if (wordMoveLapsed >= 1)
            {
                wordText.Destroy();

                targetPos.x = Drawing.BufferWidth * 0.5f;

                wordText = new FlashingText
                {
                    alignment = Text.Alignment.Center,
                    Position = targetPos,
                    text = $" {secretWord.RenderWord()} ",
                    foregrounds = new[] { Color.LIGHT_YELLOW, Color.WHITE },
                    backgroundColor = Color.BLACK,
                };

                new Text
                {
                    alignment = Text.Alignment.Center,
                    Position = targetPos + Point.Up,
                    text = " word was: ",
                    foregroundColor = Color.GREY,
                    backgroundColor = Color.BLACK,
                };

                new AnimatedText("Animations/Poledancer.txt")
                {
                    alignment = Text.Alignment.Center,
                    Position =  targetPos + Point.Up * 6,
                    foregroundColor = Color.LIGHT_MAGENTA,
                    interval = 0.6f,
                };
                    
                new Airplane();
                new Rainbow { showingAngle = -120, ZDepth = 4};
            }
            else
            {
                Vector2 newPos = Vector2.Lerp(startPos, targetPos, wordMoveLapsed);
                newPos.y = MathHelper.SmoothDamp(newPos.y, startPos.y, targetPos.y);
                wordText.Position = newPos;
            }
        }
    }
}