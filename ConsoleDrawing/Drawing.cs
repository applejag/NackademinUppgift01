﻿using ConsoleDrawing.Structs;
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
        private const string ERR_NOT_INITIALIZED = "The drawing library has not yet been initialized! Please refer to the " + nameof(SetWindowSize) + " function";
        #endregion

        #region Private fields
        private static SafeFileHandle fileHandler;

        private static CharInfo[] buffer;
        private static int bufferSize;
        internal static SmallRect bufferRect;

        private static short bufferWidth;
        private static short bufferHeight;

        private static int currentBufferIndex = 0;
        private static byte currentAttribute = Colors.GREY & Colors.P_FOREGROUND;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the width of the drawing buffer
        /// </summary>
        public static int BufferWidth => bufferWidth;
        /// <summary>
        /// Gets the height of the drawing buffer
        /// </summary>
        public static int BufferHeight => bufferHeight;

        /// <summary>
        /// Gets or sets the cursors current visible state. Default is true
        /// </summary>
        public static bool CursorVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the setting wether or not the renderer shall force the windows size to be the same as the buffer size.
        /// </summary>
        public static bool FixedSize { get; set; } = false;

        /// <summary>
        /// Get or sets the current cursor position on the x-axis
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current cursor position on the y-axis
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current color attribute for foreground coloring.
        /// </summary>
        public static byte ForegroundColor
        {
            get => (byte)(currentAttribute & Colors.P_FOREGROUND);
            set { currentAttribute = (byte)((value & Colors.P_FOREGROUND) | (currentAttribute & Colors.P_BACKGROUND)); }
        }

        /// <summary>
        /// Gets or sets the current color attribute for background coloring.
        /// </summary>
        public static byte BackgroundColor
        {
            get => (byte)(currentAttribute & Colors.P_BACKGROUND);
            set { currentAttribute = (byte)((value & Colors.P_BACKGROUND) | (currentAttribute & Colors.P_FOREGROUND)); }
        }
        #endregion

        #region Drawing methods

        /// <summary>
        /// Writes a string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void WriteWrap(string text)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            int length = text.Length;

            for (int b = currentBufferIndex, i = 0; b < bufferSize && i < length; b++, i++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = text[i];
            }

            currentBufferIndex += length;
        }

        /// <summary>
        /// Writes a formatted string of text at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/>, <seealso cref="string.Format"/></para>
        /// </summary>
        /// <param name="format">The string format to be used.</param>
        /// <param name="args">The list of arguments for the string formatting</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        /// <summary>
        /// Writes a single character at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Write(string text)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

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
        
        /// <summary>
        /// Writes a single character at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="letter">The ASCII/Unicode character to be written.</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Write(char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            if (currentBufferIndex >= 0 && currentBufferIndex < bufferSize)
            {
                buffer[currentBufferIndex].Char.UnicodeChar = letter;
                buffer[currentBufferIndex].Attributes = currentAttribute;
            }
            currentBufferIndex++;
        }

        /// <summary>
        /// Writes a formatted string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line, similar to <see cref="WriteWrap"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/>, <seealso cref="string.Format"/></para>
        /// </summary>
        /// <param name="format">The string format to be used.</param>
        /// <param name="args">The list of arguments for the string formatting</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// Writes a string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line, similar to <see cref="WriteWrap"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void WriteLine(string text)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            WriteWrap(text);
            currentBufferIndex = (CursorY + 1) * bufferWidth;
        }

        /// <summary>
        /// Clears the entire drawing buffer.
        /// <para>See also: <seealso cref="BackgroundColor"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Clear()
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            currentBufferIndex = 0;

            Fill(' ');
        }

        /// <summary>
        /// Clears the current horizontal line and resets the cursor to the start of the line.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void ClearLine()
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            int y = currentBufferIndex / bufferWidth;
            currentBufferIndex = y * bufferWidth;

            int stopAt = Math.Min(currentBufferIndex + bufferWidth, bufferSize);
            for (int b = currentBufferIndex; b < stopAt; b++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = ' ';
            }
        }
        
        /// <summary>
        /// Fills in the entire drawing buffer using the assigned color attribute and the given <paramref name="letter"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Fill(char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            for (int b = 0; b < bufferSize; b++)
            {
                buffer[b].Attributes = currentAttribute;
                buffer[b].Char.UnicodeChar = letter;
            }
        }
        
        /// <summary>
        /// Fills in a rectangle using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<see cref="Rect"/> <paramref name="rect"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="rect">The rectangle area to fill in</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillRect(Rect rect, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            FillRect(rect.x, rect.y, rect.width, rect.height, letter);
        }

        /// <summary>
        /// Fills in a rectangle using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<paramref name="left"/>, <paramref name="top"/>, <paramref name="width"/>, <paramref name="height"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="left">The position of the rectangle from the left edge</param>
        /// <param name="top">The position of the rectangle from the top edge</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillRect(int left, int top, int width, int height, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

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

        /// <summary>
        /// Fills in a character on a single point using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<see cref="Point"/> <paramref name="point"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="point">The 2D position of the point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillPoint(Point point, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            FillPoint(point.x, point.y, letter);
        }

        /// <summary>
        /// Fills in a character on a single point using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<paramref name="x"/>, <paramref name="y"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="x">The x component of the point</param>
        /// <param name="y">The y component of the point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillPoint(int x, int y, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            if (x < 0 || x >= bufferWidth || y < 0 || y >= bufferHeight)
                return;

            int index = y * bufferWidth + x;
            if (index >= 0 && index < bufferSize)
            {
                buffer[index].Char.UnicodeChar = letter;
                buffer[index].Attributes = currentAttribute;
            }
        }

        /// <summary>
        /// Draws a line between two points using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked between (<see cref="Point"/> <paramref name="point1"/>) and (<see cref="Point"/> <paramref name="point2"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="point1">The 2D position of the first point</param>
        /// <param name="point2">The 2D position of the second point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillLine(Point point1, Point point2, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            FillLine(point1.x, point1.y, point2.x, point2.y, letter);
        }

        /// <summary>
        /// Draws a line between two points using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked between (<paramref name="x1"/>, <paramref name="y1"/>) and (<paramref name="x2"/>, <paramref name="y2"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="x1">The x component of the first point</param>
        /// <param name="y1">The y component of the first point</param>
        /// <param name="x2">The x component of the second point</param>
        /// <param name="y2">The y component of the second point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void FillLine(int x1, int y1, int x2, int y2, char letter)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

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

        /// <summary>
        /// Renders the drawing buffer onto the console window.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void Render()
        {
            if (buffer == null || fileHandler == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            try
            {
                if (FixedSize)
                {
                    Console.SetWindowSize(bufferWidth, bufferHeight);
                }

                bufferRect.Width = bufferWidth;
                bufferRect.Height = bufferHeight;

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
        /// <summary>
        /// Change the window size of the drawing buffer to equal the <see cref="Console.WindowWidth"/> and <see cref="Console.WindowHeight"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void SetWindowSize()
        {
            SetWindowSize(Console.WindowWidth, Console.WindowHeight);
        }

        /// <summary>
        /// Change the window size of the drawing buffer.
        /// </summary>
        /// <param name="width">The new <paramref name="width"/> of the buffer</param>
        /// <param name="height">The new <paramref name="height"/> of the buffer</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void SetWindowSize(int width, int height)
        {
            fileHandler?.Dispose();
            fileHandler = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            short oldWidth = bufferWidth;
            short oldHeight = bufferHeight;
            int oldSize = bufferSize;
            var oldBuffer = buffer;

            bufferWidth = (short)width;
            bufferHeight = (short)height;
            bufferSize = bufferWidth * bufferHeight;
            buffer = CharInfo.NewBuffer(width, height);
            bufferRect = new SmallRect { Left = 0, Top = 0, Width = bufferWidth, Height = bufferHeight };

            // Copy old buffer
            if (oldBuffer != null)
            {
                if (oldSize > bufferSize) oldSize = bufferSize;

                for (int b = 0; b < oldSize; b++)
                {
                    int x = b % bufferWidth;
                    int y = b / bufferWidth;

                    if (x < oldWidth && y < oldHeight)
                    {
                        int i = y * oldWidth + x;
                        buffer[b] = oldBuffer[i];
                    }
                }
            }
        }

        /// <summary>
        /// Sets the cursor position on the buffer window.
        /// </summary>
        /// <param name="point">The 2D position</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void SetCursorPosition(Point point)
        {
            SetCursorPosition(point.x, point.y);
        }

        /// <summary>
        /// Sets the cursor position on the buffer window.
        /// </summary>
        /// <param name="x">The <paramref name="x"/> position</param>
        /// <param name="y">The <paramref name="y"/> position</param>
        /// <exception cref="InvalidOperationException">Thrown if called before <seealso cref="Initialize"/></exception>
        public static void SetCursorPosition(int x, int y)
        {
            if (buffer == null) throw new InvalidOperationException(ERR_NOT_INITIALIZED);

            int index = y * bufferWidth + x;
            currentBufferIndex = index;
        }

        /// <summary>
        /// Sets the color attribute to be used on following drawings. This sets both foreground and background color at the same time.
        /// <para>See: <see cref="Colors"/></para>
        /// </summary>
        /// <param name="attribute">The raw <seealso cref="byte"/> attribute.</param>
        public static void SetColorAttribute(byte attribute)
        {
            currentAttribute = attribute;
        }

        /// <summary>
        /// Resets the color attribute to be used on following drawing to default color, 
        /// <seealso cref="Colors.GREY"/> for foreground and <seealso cref="Colors.BLACK"/> for background.
        /// </summary>
        public static void ResetColorAttribute()
        {
            currentAttribute = Colors.P_DEFAULT;
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

            public static CharInfo NewAsciiChar(byte ascii, short attributes = Colors.P_DEFAULT)
            {
                var info = new CharInfo();
                info.Char.AsciiChar = ascii;
                info.Attributes = attributes;
                return info;
            }

            public static CharInfo NewUnicodeChar(char unicode, short attributes = Colors.P_DEFAULT)
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
        internal struct SmallRect
        {
            public short Left;
            public short Top;
            public short Width;
            public short Height;
        }

        #endregion
    }
}
