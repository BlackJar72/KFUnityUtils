using UnityEngine;
using kfutils;
using kfutils.graphics;
using pixelut;
using System.IO;



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
