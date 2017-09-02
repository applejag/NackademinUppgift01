using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDrawing
{
    public static class Time
    {
        static Stopwatch stopwatch = Stopwatch.StartNew();
        static ManualResetEvent timerStopper = new ManualResetEvent(false);
        static RegisteredWaitHandle timerWaitHandle;
        private static float deltaTime = 0;
        private static long deltaMilliseconds = 0;
        private static long millisecondsPerFrame;
        private static float framesPerSecond = 30;
        private static long lastFrameTime = 0;

        public delegate void FrameEvent();
        public static event FrameEvent OnEventUpdate;
        public static event FrameEvent OnEventDraw;

        private static object locker = new object();

        public static float FramesPerSecond
        {
            get => framesPerSecond;
            set
            {
                framesPerSecond = value;
                StartFrameTimer();
            }
        }
        
        public static void StartFrameTimer()
        {
            timerStopper.Set();
            timerStopper = new ManualResetEvent(false);
            
            millisecondsPerFrame = (long)(1000 / framesPerSecond);

            timerWaitHandle = ThreadPool.RegisterWaitForSingleObject(timerStopper, FrameCallback, null, millisecondsPerFrame, false);
        }

        static void FrameCallback(object state, bool timedOut)
        {
            lock (locker)
            {
                long now = stopwatch?.ElapsedMilliseconds ?? 0;
                deltaMilliseconds = now - lastFrameTime;
                deltaTime = deltaMilliseconds * 0.001f;

                // Fix size
                if (Console.WindowWidth != Drawing.BufferWidth || Console.WindowHeight != Drawing.BufferHeight)
                    Drawing.SetWindowSize(Console.WindowWidth, Drawing.BufferHeight);

                // Update
                OnEventUpdate?.Invoke();

                // Draw
                Drawing.ResetColorAttribute();
                Drawing.Clear();
                OnEventDraw?.Invoke();
                Drawing.Render();

                // Garbage collect
                Objects.Drawable.all?.RemoveAll(p => p?.Destroyed ?? true);

                lastFrameTime = now;
            }
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

        /// <summary>
        /// Time elapsed since last frame in milliseconds.
        /// </summary>
        public static double DeltaMilliseconds => deltaMilliseconds;
    }
}