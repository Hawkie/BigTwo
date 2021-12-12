using System;

namespace BIg_Two
{
    public static class FisherYatesShuffler
    {
        private static Random r = new Random();
        public static void Shuffle<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int idx = r.Next(i, array.Length);
                T temp = array[idx];
                array[idx] = array[i];
                array[i] = temp;
            }
        }
    }
}
