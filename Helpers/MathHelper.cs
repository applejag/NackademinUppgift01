using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class MathHelper
    {

        public const float PI = 3.14159265358979f;
        public const float Deg2Rad = (PI * 2f) / 360f;
        public const float Rad2Deg = 360f / (PI * 2f);

        public static int Clamp(int value, int min, int max)
        {
            return value > max ? max : (value < min ? min : value);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value > max ? max : (value < min ? min : value);
        }

        public static int Clamp01(int value)
        {
            return value > 1 ? 1 : (value < 0 ? 0 : value);
        }

        public static float Clamp01(float value)
        {
            return value > 1 ? 1 : (value < 0 ? 0 : value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from <paramref name="min"/> (INCLUSIVE) to <paramref name="max"/> (EXCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The lower bound. (INCLUSIVE)</param>
        /// <param name="max">The upper bound. (EXCLUSIVE)</param>
        public static int Wrap(int value, int min, int max)
        {
            int range = max - min;
            value = (value - min) % range;
            return value < 0 ? (max + value) : (min + value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from <paramref name="min"/> (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The lower bound. (INCLUSIVE)</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static float Wrap(float value, float min, float max)
        {
            float range = max - min + 1;
            value = (value - min) % range;
            return value < 0 ? (max + 1 + value) : (min + value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from 0 (INCLUSIVE) to <paramref name="max"/> (EXCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="max">The upper bound. (EXCLUSIVE)</param>
        public static int Wrap(int value, int max)
        {
            int range = max;
            value = value % range;
            return value < 0 ? (max + value) : value;
        }

        /// <summary>
        /// Wraps a value in a specified interval from 0 (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static float Wrap(float value, float max)
        {
            float range = max + 1;
            value = value % range;
            return value < 0 ? (max + 1 + value) : value;
        }

    }
}
