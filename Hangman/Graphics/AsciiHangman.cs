using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing;
using ConsoleDrawing.Objects;
using ConsoleDrawing.Structs;
using Helpers;

namespace Hangman.Graphics
{
    public class AsciiHangman : AnimatedText
    {
        protected static readonly string[] curses =
        {
            "VA F*N! Håller ju på o dör här!",
            "SKÄRP DIG! Du tar kål på mig!",
            "NEJ #£$@*%!! Mitt liv är på linan här ju",
            "Nämen det är lungt, inte så att bara MITT LIV STÅR PÅ SPEL!!",
        };

        protected static readonly string[] scores =
        {
            "Woho! Fortsätt så!",
            "Fett jobbat! They can take my life, but they can never take my freedom!",
            "DU ÄR BÄST! Du kommer ta det här!",
            "Så jäkla grymt!",
            "Du klarar det här så galant",
            "\"I belive I'll survive, I belive I'll leave all alive \"",
        };

        protected const float messageDuration = 4;

        protected readonly Text messageText;
        protected readonly Text messageLabelText;
        protected float messageTimeLeft;

        protected bool win = false;
        protected float winTime = 0;
        protected Vector2 winStartPos;

        public AsciiHangman(Drawable parent = null) : base("Animations/Hanging.txt", parent)
        {
            ZDepth = 1;
            interval = -1;

            messageLabelText = new Text("<", parent: this) {LocalPosition = new Point(15,0), Enabled = false, localZDepth = -1};
            messageText = new Text(parent: messageLabelText) {LocalPosition = new Point(2,0)};

            foregroundColor = Color.LIGHT_CYAN;
            backgroundColor = Color.BLACK;
        }

        protected override void Update()
        {
            base.Update();

            if (messageTimeLeft > 0)
            {
                messageTimeLeft -= Time.DeltaTime;

                if (messageTimeLeft <= 0)
                {
                    messageText.text = string.Empty;
                    messageLabelText.Enabled = false;
                    if (!win)
                        FrameIndex = 0;
                }
            }

            if (win)
            {
                winTime += Time.DeltaTime / 5;
                winTime = MathHelper.Clamp01(winTime);

                Vector2 targetPos = new Vector2(Drawing.BufferWidth - 24, Drawing.BufferHeight - 7);
                Position = Vector2.Lerp(winStartPos, targetPos, MathHelper.SmoothDamp01(winTime));
            }
        }

        public async Task AnimationWin()
        {
            FrameIndex = 2;
            Say("YAAAY DU KLARADE DET!!");
            await Task.Delay(3000);
            FrameIndex = 4;
            Say("*kick*");
            await Task.Delay(500);
            FrameIndex = 5;
            await Task.Delay(1000);
            FrameIndex = 6;
            await Task.Delay(1000);
            Say("JAG ÄR FRIiiiII!!");

            frames = LoadFromFile("Animations/HangmanDance.txt");
            FrameIndex = 0;
            interval = 0.6f;

            winStartPos = Position;
            win = true;

            await Task.Delay((int)(messageDuration * 1000));
        }

        public async Task AnimationLoose()
        {
            FrameIndex = 1;
            Say("NEJ JAG VILL INTE DÖÖÖÖ!!");
            await Task.Delay(3000);
            FrameIndex = 3;
            Say("*BLERGGHGHGHG*");

            frames = LoadFromFile("Animations/HangmanHung.txt");
            for(int i = 1; i<10;i++)
            {
                FrameIndex = i % 2;
                await Task.Delay(1000);
            }

            FrameIndex = 2;

            await Task.Delay((int)(messageDuration * 1000));
        }

        public void Say(string message)
        {
            messageTimeLeft = messageDuration;
            messageLabelText.Enabled = true;
            messageText.text = message ?? "null";
        }

        public void SayCurse()
        {
            string curse;
            do curse = curses[RandomHelper.Range(curses.Length)];
            while (messageText.text == curse);

            FrameIndex = 1;
            Say(curse);
        }

        public void SayScore()
        {
            string score;
            do score = scores[RandomHelper.Range(scores.Length)];
            while (messageText.text == score);

            FrameIndex = 2;
            Say(score);
        }

    }
}
