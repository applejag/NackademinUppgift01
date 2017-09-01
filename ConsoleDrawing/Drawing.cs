using ConsoleDrawing.Structs;
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
    public static class Drawing
    {
        #region Constants
        public const byte COLOR_BLACK = 0x00;
        public const byte COLOR_INTENSITY = 0x88;
        public const byte COLOR_RED = 0x44;
        public const byte COLOR_GREEN = 0x22;
        public const byte COLOR_BLUE = 0x11;
        public const byte COLOR_YELLOW = COLOR_GREEN | COLOR_RED;
        public const byte COLOR_MAGENTA = COLOR_BLUE | COLOR_RED;
        public const byte COLOR_CYAN = COLOR_BLUE | COLOR_GREEN;
        public const byte COLOR_GREY = COLOR_RED | COLOR_GREEN | COLOR_BLUE;

        public const byte COLOR_LIGHT_RED = COLOR_RED | COLOR_INTENSITY;
        public const byte COLOR_LIGHT_GREEN = COLOR_GREEN | COLOR_INTENSITY;
        public const byte COLOR_LIGHT_BLUE = COLOR_BLUE | COLOR_INTENSITY;
        public const byte COLOR_LIGHT_YELLOW = COLOR_YELLOW | COLOR_INTENSITY;
        public const byte COLOR_LIGHT_MAGENTA = COLOR_MAGENTA | COLOR_INTENSITY;
        public const byte COLOR_LIGHT_CYAN = COLOR_CYAN | COLOR_INTENSITY;
        public const byte COLOR_WHITE = COLOR_GREY | COLOR_INTENSITY;

        private const byte P_COLOR_FOREGROUND = 0x0F;
        private const byte P_COLOR_BACKGROUND = 0xF0;
        private const byte P_COLOR_DEFAULT = (COLOR_GREY & P_COLOR_FOREGROUND) | (COLOR_BLACK & P_COLOR_BACKGROUND);
        #endregion

        #region Private fields
        private static SafeFileHandle fileHandler = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        private static CharInfo[] buffer = CharInfo.NewBuffer(Console.WindowWidth, Console.WindowHeight);
        private static int bufferSize = Console.WindowWidth * Console.WindowHeight;
        private static SmallRect bufferRect = new SmallRect {
            Left = 0, Top = 0, Width = (short)Console.WindowWidth, Height = (short)Console.WindowHeight
        };

        private static short bufferWidth = (short)Console.WindowHeight;
        private static short bufferHeight = (short)Console.WindowWidth;

        private static int currentBufferIndex = 0;
        private static byte currentAttribute = COLOR_GREY & P_COLOR_FOREGROUND;
        #endregion

        #region Properties
        public static int Width => bufferWidth;
        public static int Height => bufferHeight;

        public static bool CursorVisible { get; set; } = true;

        public static int CursorX
        {
            get => currentBufferIndex % bufferWidth;
            set
            {
                int y = CursorY;
                int index = y * bufferWidth + value;
                currentBufferIndex = index;
            }
        }

        public static int CursorY
        {
            get => currentBufferIndex / bufferWidth;
            set
            {
                int x = CursorX;
                int index = value * bufferWidth + x;
                currentBufferIndex = index;
            }
        }

        public static byte ForegroundColor
        {
            get => (byte)(currentAttribute & P_COLOR_FOREGROUND);
            set { currentAttribute = (byte)((value & P_COLOR_FOREGROUND) | (currentAttribute & P_COLOR_BACKGROUND)); }
        }

        public static byte BackgroundColor
        {
            get => (byte)(currentAttribute & P_COLOR_BACKGROUND);
            set { currentAttribute = (byte)((value & P_COLOR_BACKGROUND) | (currentAttribute & P_COLOR_FOREGROUND)); }
        }
        #endregion

        #region Drawing methods

        public static void WriteWrap(string text)
        {
            int length = text.Length;

            for (int b = currentBufferIndex, i = 0; b < bufferSize && i < length; b++, i++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = text[i];
            }

            currentBufferIndex += length;
        }

        public static void Write(string text)
        {
            int length = text.Length;
            for (int b = currentBufferIndex, i = 0; b < bufferSize && i < length; b++, i++)
            {
                currentBufferIndex++;

                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = text[i];

                int x = b % bufferWidth;
                if (x == bufferWidth - 1)
                    break;
            }
        }

        public static void Write(char letter)
        {
            if (currentBufferIndex >= 0 && currentBufferIndex < bufferSize)
            {
                buffer[currentBufferIndex].Char.UnicodeChar = letter;
                buffer[currentBufferIndex].Attributes = currentAttribute;
            }
            currentBufferIndex++;
        }

        public static void WriteLine(string text)
        {
            WriteWrap(text);
            currentBufferIndex = (CursorY + 1) * bufferWidth;
        }

        public static void Clear()
        {
            currentBufferIndex = 0;

            Fill(' ');
        }

        public static void ClearLine()
        {
            int y = currentBufferIndex / bufferWidth;
            currentBufferIndex = y * bufferWidth;

            int stopAt = Math.Min(currentBufferIndex + bufferWidth, bufferSize);
            for (int b = currentBufferIndex; b < stopAt; b++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = ' ';
            }
        }

        public static void Fill(char letter)
        {
            for (int b = 0; b < bufferSize; b++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = letter;
            }
        }

        public static void FillRect(Rect rect, char letter)
        {
            FillRect(rect.x, rect.y, rect.width, rect.height, letter);
        }

        public static void FillRect(int left, int top, int width, int height, char letter)
        {
            int right = left + width;
            int bottom = top + height;
            for (int x = left; x < right; x++)
            {
                for (int y = top; y < bottom; y++)
                {
                    FillPoint(x, y, letter);
                }
            }
        }

        public static void FillPoint(Point point, char letter)
        {
            FillPoint(point.x, point.y, letter);
        }

        public static void FillPoint(int x, int y, char letter)
        {
            if (x < 0 || x >= bufferWidth || y < 0 || y >= bufferHeight)
                return;

            int index = y * bufferWidth + x;
            if (index >= 0 && index < bufferSize)
            {
                buffer[index].Char.UnicodeChar = letter;
                buffer[index].Attributes = currentAttribute;
            }
        }

        public static void FillLine(Point point1, Point point2, char letter)
        {
            FillLine(point1.x, point1.y, point2.x, point2.y, letter);
        }

        public static void FillLine(int x1, int y1, int x2, int y2, char letter)
        {
            // Generalized Bresenham's Line Drawing Algorithm
            int x = x1;
            int y = y1;
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = Math.Sign(x2 - x1);
            int sy = Math.Sign(y2 - y1);
            bool swap = false;

            if (dy > dx)
            {
                int tmp = dx;
                dx = dy;
                dy = tmp;
                swap = true;
            }

            int D = 2 * dy - dx;
            for (int i = 0; i < dx; i++)
            {
                FillPoint(x, y, letter);
                while (D >= 0)
                {
                    D -= 2 * dx;
                    if (swap) x += sx;
                    else y += sy;
                }
                D += 2 * dy;
                if (swap) y += sy;
                else x += sx;
            }
        }

        public static void Render()
        {
            try
            {
                Console.SetBufferSize(
                    Math.Max(Console.WindowWidth, bufferWidth),
                    Math.Max(Console.WindowHeight, bufferHeight));
            } catch { }

            bool b = WriteConsoleOutput(fileHandler, buffer,
                new Coord(bufferWidth, bufferHeight),
                new Coord(0, 0),
                ref bufferRect);

            int x = CursorX;
            int y = CursorY;
            bool inWindow = x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight;
            if (inWindow && CursorVisible)
            {
                Console.SetCursorPosition(x, y);
                Console.CursorVisible = true;
            }
            else
                Console.CursorVisible = false;

        }

        #endregion

        #region Setters methods

        public static void SetWindowSize(int width, int height)
        {
            short oldWidth = bufferWidth;
            short oldHeight = bufferHeight;
            int oldSize = bufferSize;
            var oldBuffer = buffer;

            bufferWidth = (short)width;
            bufferHeight = (short)height;
            bufferSize = bufferWidth * bufferHeight;
            buffer = new CharInfo[bufferSize];
            bufferRect = new SmallRect { Left = 0, Top = 0, Width = bufferWidth, Height = bufferHeight };

            // Copy old buffer
            var defaultChar = CharInfo.NewUnicodeChar(' ');

            for (int b = 0; b < bufferSize; b++)
            {
                int x = b % bufferWidth;
                int y = b / bufferWidth;

                if (x < oldWidth && y < oldHeight)
                {
                    int i = y * oldWidth + x;
                    buffer[b] = oldBuffer[i];
                }
                else
                    buffer[b] = defaultChar;
            }

        }

        public static void SetCursorPosition(Point point)
        {
            SetCursorPosition(point.x, point.y);
        }

        public static void SetCursorPosition(int x, int y)
        {
            int index = y * bufferWidth + x;
            currentBufferIndex = index;
        }

        public static void SetColorAttribute(byte attribute)
        {
            currentAttribute = attribute;
        }

        public static void ResetColorAttribute()
        {
            currentAttribute = P_COLOR_DEFAULT;
        }
        #endregion

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
        private struct Coord
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
        private struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;

            public static CharInfo NewAsciiChar(byte ascii, short attributes = P_COLOR_DEFAULT)
            {
                var info = new CharInfo();
                info.Char.AsciiChar = ascii;
                info.Attributes = attributes;
                return info;
            }

            public static CharInfo NewUnicodeChar(char unicode, short attributes = P_COLOR_DEFAULT)
            {
                var info = new CharInfo();
                info.Char.UnicodeChar = unicode;
                info.Attributes = attributes;
                return info;
            }

            public static CharInfo[] NewBuffer(int width, int height)
            {
                int bufferSize = width * height;
                CharInfo[] buffer = new CharInfo[bufferSize];

                // Fill buffer
                var defaultChar = NewUnicodeChar(' ');

                for (int b = 0; b < bufferSize; b++)
                {
                    buffer[b] = defaultChar;
                }

                return buffer;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRect
        {
            public short Left;
            public short Top;
            public short Width;
            public short Height;
        }

        #endregion
    }
}
