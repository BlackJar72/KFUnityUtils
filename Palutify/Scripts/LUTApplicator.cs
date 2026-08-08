using UnityEngine;
using kfutils;
using kfutils.graphics;
using kfutils.graphics.palutify;
using System.IO;


namespace kfutils.graphics.palutify {


    public class LUTApplicator : MonoBehaviour
    {
        [SerializeField] Texture2D texture;
        [SerializeField] Texture2D lut;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            texture.ApplyLUT(lut);
        }
    }


}