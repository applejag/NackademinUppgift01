using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleDrawing.Objects;

namespace ConsoleDrawing
{
    public static class Time
    {
        private static readonly object locker = new object();
        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();
        private static float deltaTime = 0;
        private static long millisecondsPerFrame = 0;
        private static long lastFrameTime = 0;

        public delegate void FrameEvent();

        /// <summary>
        /// Get or set the target amount of time between each frame.
        /// To get the actual time elapsed, use <see cref="DeltaTime"/>
        /// </summary>
        public static float FramesPerSecond
        {
            get => millisecondsPerFrame * 0.001f;
            set => millisecondsPerFrame = (long) value * 1000;
        }

        /// <summary>
        /// Time elapsed since start of program in seconds.
        /// </summary>
        public static float Seconds => (float)stopwatch.Elapsed.TotalSeconds;

        /// <summary>
        /// Time elapsed since start of program in milliseconds.
        /// </summary>
        public static long Milliseconds => stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// Time elapsed since last frame in seconds.
        /// </summary>
        public static float DeltaTime => deltaTime;

        public static void RunFrameTimer()
        {
            while (true)
            {
                long now = stopwatch.ElapsedMilliseconds;
                long elapedTime = now - lastFrameTime;
                
                if (elapedTime >= millisecondsPerFrame)
                {
                    deltaTime = elapedTime * 0.001f;

                    FrameCallback();

                    lastFrameTime = now;
                }
            }
        }

        private static void FrameCallback()
        {
            lock (locker)
            {
                // Fix size
                if (Drawing.FixedSize == false)
                {
                    if (Console.WindowWidth != Drawing.BufferWidth || Console.WindowHeight != Drawing.BufferHeight)
                        Drawing.SetWindowSize(Console.WindowWidth, Drawing.BufferHeight);
                }

                lock (Drawable.all)
                {
                    // Update
                    Input.AnalyzeInput();
                    for (int i = Drawable.all.Count - 1; i >= 0; i--)
                    {
                        // GC & Update each one
                        Drawable drawable = Drawable.all[i];
                        if (drawable?.Destroyed ?? true)
                            Drawable.all.RemoveAt(i);
                        else if (drawable.Enabled)
                            drawable.Update();
                    }

                    // Draw
                    Drawing.ResetColor();
                    Drawing.Clear();

                    Drawable.all.Sort((a, b) => b.ZDepth.CompareTo(a.ZDepth));
                    foreach (Drawable drawable in Drawable.all)
                        if (drawable?.Enabled ?? false)
                            drawable.Draw();

                    Drawing.Render();
                }
            }
        }
    }
}