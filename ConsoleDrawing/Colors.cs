using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing
{
    public static class Colors
    {
        public const byte BLACK = 0x00;
        public const byte INTENSITY = 0x88;
        public const byte RED = 0x44;
        public const byte GREEN = 0x22;
        public const byte BLUE = 0x11;
        public const byte YELLOW = GREEN | RED;
        public const byte MAGENTA = BLUE | RED;
        public const byte CYAN = BLUE | GREEN;
        public const byte GREY = RED | GREEN | BLUE;

        public const byte LIGHT_RED = RED | INTENSITY;
        public const byte LIGHT_GREEN = GREEN | INTENSITY;
        public const byte LIGHT_BLUE = BLUE | INTENSITY;
        public const byte LIGHT_YELLOW = YELLOW | INTENSITY;
        public const byte LIGHT_MAGENTA = MAGENTA | INTENSITY;
        public const byte LIGHT_CYAN = CYAN | INTENSITY;
        public const byte WHITE = GREY | INTENSITY;

        public const byte P_FOREGROUND = 0x0F;
        public const byte P_BACKGROUND = 0xF0;
        public const byte P_DEFAULT = (GREY & P_FOREGROUND) | (BLACK & P_BACKGROUND);
    }
}
