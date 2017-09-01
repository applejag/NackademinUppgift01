using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Time
    {
        static Stopwatch stopwatch = Stopwatch.StartNew();
        static Timer timer = new Timer(FrameCallback, null, 0, millisecondsPerFrame);
        private static float deltaTime = 0;
        private static long deltaMilliseconds = 0;
        private static long millisecondsPerFrame = (long)(1000 / framesPerSecond);
        private static float framesPerSecond = 30;
        private static long lastFrameTime = 0;

        public delegate void FrameEvent();
        public static event FrameEvent OnEventUpdate;
        public static event FrameEvent OnEventDraw;

        public static float FramesPerSecond
        {
            get => framesPerSecond;
            set
            {
                framesPerSecond = value;
                millisecondsPerFrame = (long)(1000 / framesPerSecond);
                timer.Change(0, millisecondsPerFrame);
            }
        }
        
        static void FrameCallback(object state)
        {
            long now = stopwatch?.ElapsedMilliseconds ?? 0;
            deltaMilliseconds = now - lastFrameTime;
            deltaTime = deltaMilliseconds * 0.001f;

            OnEventUpdate?.Invoke();
            OnEventDraw?.Invoke();

            lastFrameTime = now;
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