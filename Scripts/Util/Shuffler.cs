using System.Collections.Generic;
using UnityEngine;


namespace kfutils {

    public static class Shuffler
    {
        /// <summary>
        /// A simple exension method for Lists to shuffle them using a 
        /// provided xorshift RMG. 
        /// 
        /// The use of the custum RNG is for situarion when you need to 
        /// the number generation separate from that used for game play, 
        /// for when you want to assign specific seed (or let playesr 
        /// choose a seed), and similar situations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="random"></param>
        public static void Shuffle<T>(this IList<T> list, Xorshift random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.NextInt(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        /// <summary>
        /// A simple extension method for Lists to shuffle them using the 
        /// statndard Random object from the UnityEngine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
