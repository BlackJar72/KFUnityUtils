using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace kfutils {
    public class TerrainBulder : MonoBehaviour {
        [SerializeField] int worldSize;
        [SerializeField] int terrainSize;
        [SerializeField] int terrainsPerArea;
        [SerializeField] int heightScale;
        [SerializeField] string assetPath;
        [SerializeField] string seedString;

        public List<GameObject> terrainsObjects;
        public List<Terrain> terrains;

        private int terrainDetail;
        private int terrainsAcross;
        public long seed;


        void Start() {
            worldSize = Mathf.ClosestPowerOfTwo(worldSize);
            terrainSize = Mathf.ClosestPowerOfTwo(terrainSize);
            terrainDetail = Mathf.ClosestPowerOfTwo(terrainSize) + 1;

            if((seedString == null) || (seedString.Length < 1)) seed = (int)(Random.value * int.MaxValue);
            else if(!long.TryParse(seedString, out seed)) {
                seed = seedString.GetHashCode();
            }

            terrainsAcross = worldSize / terrainSize;
            terrainsObjects = new List<GameObject>();
            terrains = new List<Terrain>();

            MakeTerrains();
            SetHeights();
        }


        private void MakeTerrains() {
            Vector3 terrainDimensions = new Vector3(terrainSize, heightScale, terrainSize);
            Vector3 terrainPos;
            for(int i = 0; i <= terrainsAcross - 1; i++)
                for(int j = 0; j <= terrainsAcross - 1; j++) {
                    string tname = "Terrain-" + (i / terrainsPerArea) + "." + (i % terrainsPerArea) + "-"
                    + (j / terrainsPerArea) + "." + (j % terrainsPerArea);
                    terrainPos  = new Vector3(i * terrainSize, 0, j * terrainSize);
                    TerrainData terrainData = new TerrainData();
                    terrainData.baseMapResolution = terrainSize;
                    terrainData.heightmapResolution = terrainDetail + 1;
                    terrainData.alphamapResolution = terrainSize;
                    terrainData.SetDetailResolution(terrainSize, 1);
                    terrainData.size = terrainDimensions;
                    terrainData.name = tname;

                    GameObject terrain = Terrain.CreateTerrainGameObject(terrainData);

                    terrain.name = tname;
                    terrainsObjects.Add(terrain);
                    terrain.transform.SetParent(transform);
                    terrain.transform.position = terrainPos;
                    terrains.Add(terrain.GetComponent<Terrain>());

                    //AssetDatabase.CreateAsset(terrainData, "Assets/" + assetPath + tname + ".asset");
                }
        }


        private void SetHeights() {
            float[,] tmesh = new float[worldSize + 1, worldSize + 1];
            SpatialHash srandom = new SpatialHash((ulong)seed);

            HeightNoiseMap noise = new HeightNoiseMap(worldSize, worldSize, 256, 4);
            float[,] noisemap = noise.Process(srandom, 0, 0);
            float[,] localmap = new float[terrainDetail, terrainDetail];

            //TODO: Make a continent!
            for(int i = 0; i < worldSize; i++)
                for(int j = 0; j < worldSize; j++) {
                    tmesh[i, j] = (noisemap[i, j] + 0.25f) * 1.33333333333f;
                    // Would normally be done afterward.
                    tmesh[i, j] = tmesh[i, j] * 0.5f;
                }

            for(int i = 0; i < terrainsAcross; i++)
                for(int j = 0; j < terrainsAcross ; j++) {
                    for(int ii = 0; ii < terrainDetail; ii++)
                        for(int jj = 0; jj < terrainDetail; jj++) {
                            localmap[ii, jj] = tmesh[i * terrainSize + ii,
                                    j * terrainSize + jj];
                        }
                    terrains[i + (j * terrainsAcross)].terrainData.SetHeights(0, 0, localmap);
                }
        }


    }
}