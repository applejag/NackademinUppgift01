using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDrawing.Objects;

namespace Hangman.Graphics
{
    public class AsciiHangman : AnimatedText
    {

        public AsciiHangman() : base("Animations/Hanging.txt")
        {
            
        }

    }
}
