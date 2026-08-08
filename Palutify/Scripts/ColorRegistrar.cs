using UnityEngine;
using kfutils;
using kfutils.graphics;
using System.Collections.Generic;



namespace pixelut {

    public class ColorRegistrar
    {
        private readonly List<Color> colors = new();

        public int ColorCount => colors.Count;


        public void ClearRegistry() => colors.Clear();


        public void AddColor(Color color)
        {
            if(!colors.Contains(color)) colors.Add(color);
        }


        public void AddColors(Texture2D texture)
        {
            Color[] pixels = texture.GetPixels();
            for(int i = 0; i < pixels.Length; i++) AddColor(pixels[i]);
            /*{
                if(!colors.Contains(pixels[i])) colors.Add(pixels[i]);
                else Debug.Log(pixels[i] + " at " + i);
            }*/
        }


        public void PopulateFromTexture(Texture2D texture)
        {
            ClearRegistry();
            AddColors(texture);
        }


        public Color FindClosestRegisteredRGB(Color source)
        {
            Color result = source;
            float shortest = float.PositiveInfinity;
            for(int i = 0; i < colors.Count; i++)
            {
                float dist = source.RGBDistanceSQ(colors[i]);
                if(dist < shortest)
                {
                    shortest = dist;
                    result = colors[i];
                } 
            }
            return result;
        }


        public Color FindClosestRegisteredHSV(Color source)
        {
            Color result = source;
            float shortest = float.PositiveInfinity;
            for(int i = 0; i < colors.Count; i++)
            {
                float dist = source.HSVDistance(colors[i]);
                if(dist < shortest)
                {
                    shortest = dist;
                    result = colors[i];
                }
            }
            return result;
        }


        public Color FindClosestRegisteredHRGB(Color source)
        {
            Color result = source;
            float shortest = float.PositiveInfinity;
            for(int i = 0; i < colors.Count; i++)
            {
                float dist = source.HRGBDistanceSQ(colors[i]);
                if(dist < shortest)
                {
                    shortest = dist;
                    result = colors[i];
                }
            }
            return result;
        }


        public Color FindClosestRegisteredHybrid(Color source)
        {
            Color result = source;
            float shortest = float.PositiveInfinity;
            for(int i = 0; i < colors.Count; i++)
            {
                float dist = source.RGBDistanceSQ(colors[i]) + source.HSVDistanceSQ(colors[i]);
                if(dist < shortest)
                {
                    shortest = dist;
                    result = colors[i];
                }
            }
            return result;
        }


    }


}
