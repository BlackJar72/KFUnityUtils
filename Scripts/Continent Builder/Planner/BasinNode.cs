using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace kfutils {
    public class BasinNode {
        readonly int x, z, value;
        readonly double decay;
        private static readonly double[] LOGTABLE = makeLogTable();
        private static readonly double[] logtable = LOGTABLE;


        public BasinNode(int x, int y, int value, double decay) {
            this.x = x;
            this.z = y;
            this.value = value;
            this.decay = decay;
        }


        public double getRelativeWeakness(int range) {
            double effect = ((range) * decay);
            return range * range;
        }


        public double getWeaknessAt(double atx, double aty) {
            double xdisplace = ((((double)x) - atx) * decay);
            double ydisplace = ((((double)z) - aty) * decay);
            return Mathf.Min((float)((xdisplace * xdisplace) + (ydisplace * ydisplace)), 1.0f);
        }


        public static int summateEffect(BasinNode[] n, ChunkTile t) {
            double effect = 0.0;
            double sum    = 0.0;
            double power, weakness;
            for(int i = 0; i < n.Length; i++) {
                if((n[i].x == t.tx) && (n[i].z == t.tz)) {
                    return n[i].value;
                }
                weakness = n[i].getWeaknessAt(t.tx, t.tz);
                power = 1.0 / (weakness * weakness);
                sum += power;
                effect += Mathf.Max((float)((n[i].value) * power), 0);
            }
            return (int)(effect / sum);
        }


        public static double summateEffect(BasinNode[] n, ChunkTile t, double scale) {
            double effect = 0.0;
            double sum    = 0.0;
            double power, weakness;
            for(int i = 0; i < n.Length; i++) {
                double x = ((double)t.tx) * scale;
                double z = ((double)t.tz) * scale;
                if((n[i].x == (int)x) && (n[i].z == (int)z)) {
                    return (int)n[i].value;
                }
                weakness = n[i].getWeaknessAt(x, z);
                power = (1.0 / (weakness * weakness));
                sum += power;
                effect += Mathf.Max((float)((n[i].value) * power), 0f);
            }
            return (effect / sum);
        }


        private static double[] makeLogTable() {
            double[] output = new double[31];
            for(int i = 0; i < output.Length; i++) {
                output[i] = Mathf.Pow(10, ((float)(i - 15)) / 10);
            }
            return output;
        }


        public static double getLogScaled(int input) {
            return logtable[input + 15];
        }



        override public string ToString() {
            return "    [x=" + x + ", z=" + z + ", val=" + value + ", decay=" + decay + "] ";
        }


        public string BriefString() {
            return "    [x=" + x + ", z=" + z + "] ";
        }

    }

}