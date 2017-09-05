using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Hangman.Graphics;

namespace Hangman
{
    public class Controller : Drawable
    {
        protected readonly SecretWord secretWord;
        protected readonly Text prompt;
        protected readonly Text missesLabelText;
        protected readonly Text missesText;
        protected readonly Text errorText;
        protected readonly Text triesLeftText;
        protected readonly Text wordText;
        protected readonly TextField inputField;

        protected readonly AsciiHangman hangmanAnimated;

        protected bool win = false;

        public Controller(SecretWord secretWord) : base(null)
        {
            this.secretWord = secretWord;

            prompt = new Text("Gissa en bokstav eller ordet? ", parent: this);
            missesLabelText = new Text("", parent: this);
            missesText = new Text("", parent: missesLabelText);
            errorText = new Text("", parent: this);
            triesLeftText = new Text("", parent: errorText);
            inputField = new TextField(parent: prompt);
            wordText = new Text($"Ord: {secretWord.RenderWord()}", parent: this);

            hangmanAnimated = new AsciiHangman(parent: this) {Position = new Vector2(2,8)};

            missesText.foregroundColor = Color.LIGHT_YELLOW;
            inputField.Submitted += OnInputFieldSubmit;
        }

        private void OnInputFieldSubmit(TextField field)
        {
            string input = field.text.Trim().ToUpper();
            int length = input.Length;

            if (length == 1)
            {
                char guess = input[0];

                if (secretWord.IsGuessed(guess))
                    errorText.text = $"Du har redan försökt med {guess}! ";
                else
                {
                    if (secretWord.GuessLetter(guess))
                    {
                        errorText.foregroundColor = Color.LIGHT_GREEN;
                        errorText.text = "Rätt! ";
                        hangmanAnimated.SayScore();
                    }
                    else
                    {
                        errorText.foregroundColor = Color.LIGHT_RED;
                        errorText.text = "Fel! ";
                        hangmanAnimated.SayCurse();
                    }
                }
            }
            else if (length != 0)
            {
                string guess = input;

                if (secretWord.GuessWord(guess) == false)
                {
                    errorText.foregroundColor = Color.LIGHT_RED;
                    errorText.text = "Fel ord! ";
                    hangmanAnimated.SayCurse();
                }
            }

            if (secretWord.AnyMisses)
            {
                missesLabelText.text = "Missar: ";
                missesText.text = secretWord.RenderMisses();
                missesText.LocalPosition = Point.Right * missesLabelText.text.Length;
            }

            triesLeftText.text = $"Du har {secretWord.TriesLeft} gissningar kvar.";
            triesLeftText.LocalPosition = Point.Right *  errorText.text.Length;
            wordText.text = $"Ord: {secretWord.RenderWord()}";

            if (secretWord.GameOver)
            {
                WinTask();
            }
        }

        protected async void WinTask()
        {
            inputField.Destroy();
            Drawing.CursorVisible = false;
            Update();
            await hangmanAnimated.AnimationWin();
            win = true;
        }

        protected override void Update()
        {
            int row = 0;

            if (!string.IsNullOrEmpty(errorText.text) || !string.IsNullOrEmpty(triesLeftText.text))
            {
                row++;
            }

            wordText.LocalPosition = Point.Down * row * 2;
            row++;

            if (secretWord.AnyMisses)
            {
                missesLabelText.LocalPosition = Point.Down * row * 2;
                row++;
            }
            
            prompt.LocalPosition = Point.Down * row * 2;

            inputField.LocalPosition = Point.Right * prompt.text.Length;


            if (win)
            {
                var wordLabelText = new Text("Ord:", parent: wordText) { Position = wordText.Position };

                Text[] textDrawables = {
                    wordLabelText,
                    errorText,
                    missesLabelText,
                    missesText,
                    triesLeftText,
                    prompt,
                };

                if (secretWord.Finished)
                {
                    var winScreen = new WinScreen(secretWord, wordText, textDrawables);
                    hangmanAnimated.SetParent(winScreen);
                }
                else
                    new LoseScreen(secretWord);

                Drawing.CursorVisible = false;
                Destroy();
            }
        }

        protected override void Draw()
        {
            //throw new NotImplementedException();
        }

    }
}
