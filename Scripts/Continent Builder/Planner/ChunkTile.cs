namespace kfutils {
    public class ChunkTile {
        public const int size = 16;
        public readonly int x, z, tx, tz;
        public int val = 0;
        public int rlBiome;
        public int temp = 0, wet = 0;
        public int biomeSeed = 0;
        public int noiseVal = 0;
        public int river;
        public double height;
        public double centrality;
        public float scale;
        public bool beach;
        TerrainType terrainType = TerrainType.OCEAN;


        public ChunkTile(int x, int z, int xoff, int zoff) {
            this.x = x;
            this.z = z;
            tx = x + xoff;
            tz = z + zoff;
        }

        public static int getSize() {
            return size;
        }

        public int X => x;

        public int Z => z;


        public int TX => tx;

        public int TZ => tz;

        public int Val => val;

        public int RlBiome => rlBiome;

        public int Temp => temp;

        public int Wet => wet;

        public double Height => height;

        public int BiomeSeed => biomeSeed;

        public int Noise => noiseVal;

        public double Centrality => centrality;

        public int CentNoise => noiseVal + (int)(centrality * 10.0);

        public bool IsBeach => beach;

        public TerrainType TerrainType => terrainType;

        public bool IsRiver => river > 0;

        public void BeRiver(int id) {
            river = id;
        }

        public void SetTerrainType(TerrainType type) {
            terrainType = type;
        }

        public void SetMountainous() {
            terrainType = TerrainType.MOUNTAIN;
        }


        public ChunkTile nextBiomeSeed() {
            biomeSeed ^= biomeSeed << 13;
            biomeSeed ^= biomeSeed >> 19;
            biomeSeed ^= biomeSeed << 7;
            biomeSeed &= 0x7fffffff;
            return this;
        }

    }


    public enum TerrainType {
        OCEAN,
        LAND,
        MOUNTAIN
    }
}