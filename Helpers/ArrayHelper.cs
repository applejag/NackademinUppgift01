using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ArrayHelper
    {

        private static Random randomGen = new Random();

        private static void TheShuffler<T>(T[] array, Random randomGen)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = randomGen.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Shuffles the existing array using the Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(this T[] array, int seed)
        {
            TheShuffler(array, new Random(seed));
        }

        /// <summary>
        /// Shuffles the existing array using the Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            TheShuffler(array, randomGen);
        }

        /// <summary>
        /// Returns a new string that is shuffled using the Fisher-Yates algorithm.
        /// </summary>
        public static string Shuffle(this string str, int seed)
        {
            char[] array = str.ToCharArray();
            array.Shuffle(seed);
            return new string(array);
        }

        /// <summary>
        /// Returns a new string that is shuffled using the Fisher-Yates algorithm.
        /// </summary>
        public static string Shuffle(this string str)
        {
            char[] array = str.ToCharArray();
            array.Shuffle();
            return new string(array);
        }

    }
}
