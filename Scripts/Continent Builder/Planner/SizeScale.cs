using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace kfutils {
    public enum ESizeScale {
        X1, X2, X3, X4
    }


    public struct SizeScaleData {
        public readonly int whole;
        public readonly double fract;
        public readonly double inv;
        public readonly int log; // determines fractal iterations, mostly
        public readonly int sq;
        public readonly int width;

        public const  ESizeScale defaultSize = ESizeScale.X1;
        public static ESizeScale setting = defaultSize;

        public SizeScaleData(int s, int l) {
            whole = s;
            fract = s;
            inv = 1.0 / fract;
            log = l;
            sq = whole * whole;
            width = 256 * s;
        }
    }


    public class SizeScale {
        public static readonly SizeScaleData[] sizes = new SizeScaleData[4]
                {new SizeScaleData(1, 0), new SizeScaleData(2, 1),
                 new SizeScaleData(3, 2), new SizeScaleData(4, 2)};
        public static SizeScaleData Get(ESizeScale scale) => sizes[(int)scale];
        public static SizeScaleData Get(int scale) => sizes[scale];
    }



}