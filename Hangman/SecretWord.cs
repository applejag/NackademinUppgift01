using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class SecretWord
    {
        public const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
        public const char CHAR_LOCKED = '_';
        public const char CHAR_SPACING = ' ';

        private readonly string word;
        private readonly int maxErrors;

        private string unlocked = " ";
        private string guesses = string.Empty;
        private string misses = string.Empty;

        public SecretWord(string input, int maxErrors)
        {
            this.maxErrors = maxErrors;
            word = Parse(input);
            Finished = IsFinished();
        }

        public bool Finished { get; private set; } = false;
        public bool GameOver { get => Finished || TriesLeft == 0; }
        public int Errors { get; private set; } = 0;
        public int TriesLeft { get => Math.Max(maxErrors - Errors, 0); }
        public bool AnyMisses { get => misses.Length > 0; }

        /// <summary>
        /// <para>Returnerar <seealso cref="true"/> när en ny karaktär är upplåst.</para>
        /// <para>Returnerar <seealso cref="false"/> ifall spelet redan är över, karaktären är ogiltig, den redan är upplåst, eller den inte finns i det gömda ordet.</para>
        /// </summary>
        public bool GuessLetter(char guess)
        {
            if (Finished == true) return false;
            if (IsAllowed(guess) == false) return false;
            if (IsUnlocked(guess) == true) return false;
            if (IsGuessed(guess) == true) return false;

            guesses += guess;

            bool bokstavFinnsIOrd = word.IndexOf(guess) != -1;

            if (bokstavFinnsIOrd)
            {
                unlocked += guess;

                Finished = IsFinished();
            }
            else
            {
                Errors++;
                misses += guess;
            }

            return bokstavFinnsIOrd;
        }

        /// <summary>
        /// <para>Returnerar <seealso cref="true"/> om det var det rätta order.</para>
        /// <para>Returnerar <seealso cref="false"/> ifall spelet redan är över, eller ifall gissningen var fel.</para>
        /// </summary>
        public bool GuessWord(string guess)
        {
            if (Finished == true) return false;

            guess = Parse(guess);

            bool rättGissat = guess == word;

            if (rättGissat)
            {
                Finished = true;
            }
            else
            {
                Errors++;
            }

            return rättGissat;
        }

        public string RenderWord()
        {
            string output = string.Empty;
            int length = word.Length;

            for (int i = 0; i < length; i++)
            {
                if (Finished || IsUnlocked(word[i]))
                    output += word[i];
                else
                    output += CHAR_LOCKED;

                output += CHAR_SPACING;
            }

            return output.TrimEnd();
        }

        public string RenderMisses()
        {
            string output = string.Empty;
            int length = misses.Length;

            for (int i = 0; i < length; i++)
            {
                output += misses[i];
                output += CHAR_SPACING;
            }

            return output.TrimEnd();
        }

        private bool IsFinished()
        {
            foreach (char c in word)
                if (IsUnlocked(c) == false)
                    return false;

            return true;
        }

        public bool IsUnlocked(char letter)
        {
            return unlocked.IndexOf(letter) != -1;
        }

        public bool IsGuessed(char letter)
        {
            return guesses.IndexOf(letter) != -1;
        }

        public bool IsMissed(char letter)
        {
            return misses.IndexOf(letter) != -1;
        }

        public static bool IsAllowed(char letter)
        {
            return ALLOWED_CHARS.IndexOf(letter) != -1;
        }

        public static string Parse(string input)
        {
            input = input?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string word = string.Empty;

            foreach (char c in input)
            {
                if (IsAllowed(c) || c == CHAR_SPACING)
                {
                    word += c;
                }
            }

            return word;
        }

    }
}
