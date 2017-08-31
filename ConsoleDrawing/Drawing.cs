using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing
{
    public class Drawing
    {
        public const byte FG_BLACK = 0x00;
        public const byte FG_INTENSITY = 0x08;
        public const byte FG_RED = 0x04;
        public const byte FG_GREEN = 0x02;
        public const byte FG_BLUE = 0x01;
        public const byte FG_YELLOW = FG_GREEN | FG_RED;
        public const byte FG_MAGENTA = FG_BLUE | FG_RED;
        public const byte FG_CYAN = FG_BLUE | FG_GREEN;
        public const byte FG_GREY = FG_RED | FG_GREEN | FG_BLUE;

        public const byte FG_LIGHT_RED = FG_RED | FG_INTENSITY;
        public const byte FG_LIGHT_GREEN = FG_GREEN | FG_INTENSITY;
        public const byte FG_LIGHT_BLUE = FG_BLUE | FG_INTENSITY;
        public const byte FG_LIGHT_YELLOW = FG_YELLOW | FG_INTENSITY;
        public const byte FG_LIGHT_MAGENTA = FG_MAGENTA | FG_INTENSITY;
        public const byte FG_LIGHT_CYAN = FG_CYAN | FG_INTENSITY;
        public const byte FG_WHITE = FG_GREY | FG_INTENSITY;

        public const byte BG_BLACK = 0x00;
        public const byte BG_INTENSITY = 0x80;
        public const byte BG_RED = 0x40;
        public const byte BG_GREEN = 0x20;
        public const byte BG_BLUE = 0x10;
        public const byte BG_YELLOW = BG_GREEN | BG_RED;
        public const byte BG_MAGENTA = BG_BLUE | BG_RED;
        public const byte BG_CYAN = BG_BLUE | BG_GREEN;
        public const byte BG_GREY = BG_RED | BG_GREEN | BG_BLUE;

        public const byte BG_LIGHT_RED = BG_RED | BG_INTENSITY;
        public const byte BG_LIGHT_GREEN = BG_GREEN | BG_INTENSITY;
        public const byte BG_LIGHT_BLUE = BG_BLUE | BG_INTENSITY;
        public const byte BG_LIGHT_YELLOW = BG_YELLOW | BG_INTENSITY;
        public const byte BG_LIGHT_MAGENTA = BG_MAGENTA | BG_INTENSITY;
        public const byte BG_LIGHT_CYAN = BG_CYAN | BG_INTENSITY;
        public const byte BG_WHITE = BG_GREY | BG_INTENSITY;

        private SafeFileHandle fileHandler;

        private readonly CharInfo[] buffer;
        private readonly int bufferSize;
        private SmallRect bufferRect;

        private readonly short windowWidth;
        private readonly short windowHeight;
        private int currentBufferIndex;
        private byte currentAttribute = FG_GREY | BG_BLACK;

        public short Width => windowWidth;
        public short Height => windowHeight;

        public int CursorX
        {
            get => currentBufferIndex % windowWidth;
            set
            {
                int y = CursorY;
                int index = y * windowWidth + value;
                if (index < 0 || index >= bufferSize)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Position is outside window!");
                else
                    currentBufferIndex = index;
            }
        }

        public int CursorY
        {
            get => currentBufferIndex / windowWidth;
            set
            {
                int x = CursorX;
                int index = value * windowWidth + x;
                if (index < 0 || index >= bufferSize)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Position is outside window!");
                else
                    currentBufferIndex = index;
            }
        }

        public byte ForegroundColor
        {
            get => (byte)(currentAttribute & 0x0F);
            // Only take foreground bits from value, combine with previous background bits
            set { currentAttribute = (byte)((value & 0x0F) | (currentAttribute & 0xF0)); }
        }

        public byte BackgroundColor
        {
            get => (byte)(currentAttribute & 0xF0);
            // Only take background bits from value, combine with previous foreground bits
            set { currentAttribute = (byte)((value & 0xF0) | (currentAttribute & 0x0F)); }
        }


        public Drawing()
        {
            fileHandler = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            windowHeight = (short)Console.WindowHeight;
            windowWidth = (short)Console.WindowWidth;

            bufferSize = windowWidth * windowHeight;
            buffer = new CharInfo[bufferSize];
            bufferRect = new SmallRect { Left = 0, Top = 0, Width = windowWidth, Height = windowHeight };
        }

        ~Drawing()
        {
            fileHandler?.Dispose();
        }

        public void Write(string text)
        {
            int length = text.Length;
            for (int b = currentBufferIndex, i = 0; b < bufferSize && i < length; b++, i++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = text[i];
            }
        }

        public void Fill(char letter)
        {
            for (int b = 0; b < bufferSize; b++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = letter;
            }
        }

        public void FillRect(SmallRect rect, char letter)
        {
            int start = rect.Top * windowWidth + rect.Left;
            int stop = (rect.Top + rect.Height - 1) * windowWidth + rect.Left + rect.Width;
            stop = Math.Min(stop, bufferSize);
            for (int b = start; b < stop; b++)
            {
                int x = b % windowWidth;
                if (x >= rect.Left && x < rect.Left + rect.Width)
                {
                    buffer[b].Attributes = currentAttribute;
                    buffer[b].Char.UnicodeChar = letter;
                }
            }
        }

        public void Render()
        {
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            bool b = WriteConsoleOutput(fileHandler, buffer,
                new Coord(windowWidth, windowHeight),
                new Coord(0, 0),
                ref bufferRect);
        }

        public void SetCursorPosition(int x, int y)
        {
            int index = y * windowWidth + x;
            if (index < 0 || index >= bufferSize)
                throw new ArgumentOutOfRangeException($"x:{nameof(x)}, y:{nameof(y)}", $"x:{x}, y:{y}", "Position is outside window!");
            else
                currentBufferIndex = index;
        }

        public void SetColorAttribute(byte attribute)
        {
            currentAttribute = (byte)attribute;
        }

        #region External functions

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        #endregion

        #region Datastructures

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Width;
            public short Height;

            public bool IsColliding(SmallRect other)
            {
                return Left < other.Left + other.Width
                    && Left + Width > other.Left
                    && Top < other.Top + other.Height
                    && Top + Height > other.Top;
            }
        }

        #endregion
    }
}
