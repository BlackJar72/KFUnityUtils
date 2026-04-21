using System;
using UnityEngine;

namespace kfutils
{

    public static class TextureDraw
    {
        public static Texture2D GetMainTexture(this GameObject go)
        {            
            Renderer renderer = go.GetComponent<Renderer>();
            return (Texture2D)renderer.sharedMaterial.mainTexture;
        }


        /// <summary>
        /// Draws a pixel in a texture;  mostly pointless, but keeps a reminder of 
        /// how to do it.  Really, you should probably just use SetPixel() directly 
        /// instead of this in a real code base in the vast majority of situations.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        [Obsolete("You probably want SetPixel(x, y, color), which this wraps as a reminder")]
        public static void DrawPixel(this Texture2D texture, int x, int y, Color color)
        {
            texture.SetPixel(x, y, color);
            // This should only be done once per operation, thus this method should 
            // not be the basis of other, more complex drawing methods.  Then, this 
            // is mostly a dummy method to help remember the methods called.
            texture.Apply(); 
        }
        
    }


}