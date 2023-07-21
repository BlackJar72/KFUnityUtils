using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace kfutils {
    [Serializable]
    public class ClimaticWorldSettings {
        // Core settings
        public bool addIslands;
        public bool extraBeaches;
        public bool forceWhole;
        public bool rockyScrub;
        public bool deepSand;
        public bool volcanicIslands;
        public bool hasRivers;
        public bool hasCoasts;

        public bool bigMountains;

        public int biomeSize;
        public SizeScale regionSize;
        public int mode;
        public double sisize;
    }
}