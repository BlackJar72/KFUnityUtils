using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kfutils {

    public static class KFMath {

        /// <summary>
        /// This is will produce an always positive modulus,
        /// that is, a remainder from the next lower number
        /// even when negative.  Many situations require this,
        /// such as when locating a value in a 2D grid stored
        /// as a 1D array.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ModRight(int a, int b) {
            return (a & 0x7fffffff) % b;
        }


        /// <summary>
        /// Causes the input to start asymptoting at the number given as start, 
        /// such that the output would start + rate at n = infinity, and each 
        /// having of the difference between the outpur and start + rate and will 
        /// reqiore doubling the difference between start and n.
        /// 
        /// For value of n less than start the output will be n, with n = start 
        /// producing the same results for either formula so as to create a 
        /// continuous curve.
        /// 
        /// This was first developed to avoid sudden height cut-off on precedurally 
        /// generated mountains that might otherwise produce conspicuously unnatural 
        /// looking fast tops if exceeding a height limit.
        /// 
        /// It has since been used effectively to soft-cap armor in the health and 
        /// damage system included in the "Damage System" folder.  It was also used 
        /// to soft cap difficulty in the Cavern Of Evil "NG+" levels. It could be  
        /// useful for applying a soft cap to anything, really.
        ///
        /// n is the number being converted to an asymptopic form.
        /// start is the place where the output should start to curve.
        /// rate is the reciprical of the value it should approach minus the start.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="start"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static float Asymptote(float n, float start, float rate) {
            if(n > start) {
                float output = (n - start) / rate;
                output = 1 - (1 / (output + 1));
                output = (output * rate) + start;
                return output;
            }
            return n;
        }


        /// <summary>
        /// Causes the input to start asymptoting at the number given as start, 
        /// such that the output would start + rate at n = infinity, and each 
        /// having of the difference between the outpur and start + rate and will 
        /// reqiore doubling the difference between start and n.
        /// 
        /// For value of n less than start the output will be n, with n = start 
        /// producing the same results for either formula so as to create a 
        /// continuous curve.
        /// 
        /// This was first developed to avoid sudden height cut-off on precedurally 
        /// generated mountains that might otherwise produce conspicuously unnatural 
        /// looking fast tops if exceeding a height limit.
        /// 
        /// It has since been used effectively to soft-cap armor in the health and 
        /// damage system included in the "Damage System" folder.  It was also used 
        /// to soft cap difficulty in the Cavern Of Evil "NG+" levels. It could be  
        /// useful for applying a soft cap to anything, really. 
        ///
        /// n is the number being converted to an asymptopic form.
        /// start is the place where the output should start to curve.
        /// rate is the reciprical of the value it should approach minus the start.
        ///
        /// </summary>
        /// <param name="n"></param>
        /// <param name="start"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static double Asymptote(double n, double start, double rate) {
            if(n > start) {
                double output = (n - start) / rate;
                output = 1 - (1 / (output + 1));
                output = (output * rate) + start;
                return output;
            }
            return n;
        }


        /// <summary>
        /// This will convert a string to a long for use as a seed for random number generation.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long GetLongSeed(this string str) {
            string strSeed = str.Trim();
            long output;
            try
            {
                output = long.Parse(strSeed);
            }
            catch (System.FormatException)
            {
                output = strSeed.GetHashCode();
                output |= output << 32;
            }
            return output;
        }


        /// <summary>
        /// This will convert a string to a ulong for use as a seed for random number generation.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ulong GetULongSeed(this string str) {
            string strSeed = str.Trim();
            ulong output = 0;
            try
            {
                output = ulong.Parse(strSeed);
            }
            catch (System.FormatException)
            {
                output |= (ulong)((uint)strSeed.GetHashCode());
                output |= output << 32;
            }
            return output;
        }


        /// <summary>
        /// This will convert a string to an int for use as a seed for random number generation.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetIntSeed(this string str) {
            string strSeed = str.Trim();
            int output;
            try
            {
                output = int.Parse(strSeed);
            }
            catch (System.FormatException)
            {
                output = strSeed.GetHashCode();
            }
            return output;
        }

    }

}
