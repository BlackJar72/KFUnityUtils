using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace kfutils {
    public class LandmassMaker {
        SpatialHash random;
        int regx, regz, size;
        SizeScaleData scale;
        double currentScale;
        BasinNode[] basins;
        int xoff, zoff;


        LandmassMaker(int rx, int ry, SpatialHash rand,
                BasinNode[] basinAr, ESizeScale sc, int startW,
                int xoffIn, int zoffIn) {
            random = rand;
            scale = SizeScale.Get(sc);
            size = startW * scale.whole;
            regx = rx;
            regz = ry;
            currentScale = 1.0;
            basins = basinAr;
            xoff = xoffIn;
            zoff = zoffIn;
        }


        public ChunkTile[] generate(ClimaticWorldSettings settings) {
            double beachThreshold = 0.70;
            ChunkTile[] output = new ChunkTile[size * size];
            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++) {
                    output[(i * size) + j]
                    = new ChunkTile(i, j, xoff, zoff);
                }
            NoiseMap2D heightmaker
                = new NoiseMap2D(random, size, 16 * scale.whole, 1.0, regx, regz);
            double[,] heights = heightmaker.Process(0);
            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++) {
                    output[(i * size) + j].height
                    = edgeFix(output[(i * size) + j],
                            BasinNode.summateEffect(basins,
                                    output[(i * size) + j],
                                    scale.inv));
                    output[(i * size) + j].val = (int)output[(i * size) + j].height;
                    output[(i * size) + j].height /= 10.0;
                    output[(i * size) + j].height = ((output[(i * size) + j].height
                    + (heights[i,j] / 2.0) + 0.5)
                    * output[(i * size) + j].height)
                    + heights[i,j];
                }

            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++) {
                    if(output[(i * size) + j].height > 0.6) {
                        output[(i * size) + j].rlBiome = 1;
                        output[(i * size) + j].beach = true;
                    } else {
                        output[(i * size) + j].rlBiome = 0;
                    }
                }

            return output;
        }


        protected double edgeFix(ChunkTile t, double val) {
            if(t.x < (10 * scale.whole)) {
                val += ((t.x - (10 * scale.whole)) / (2 * scale.whole));
            } else if(t.x >= (size - (10 * scale.whole))) {
                val -= ((t.x - size + (10 * scale.whole)) / (2 * scale.whole));
            }
            if(t.z < (10 * scale.whole)) {
                val += ((t.z - (10 * scale.whole)) /  (2 * scale.whole));
            } else if(t.z >= (size - (10 * scale.whole))) {
                val -= ((t.z - size + (10 * scale.whole)) / (2 * scale.whole));
            }
            return val;
        }


    }
}