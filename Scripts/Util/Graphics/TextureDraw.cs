using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace kfutils.graphics
{

    public static class TextureDraw
    {
        public const float TWO_PI = Mathf.PI * 2.0f; 


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


        /// <summary>
        /// Saves the texture at the give path, as either a PNG or JPEG, based on the file extension. 
        /// If the file extension is missing or incorrect, the file will be saved as a PNG, and a .png 
        /// extension will be added.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="path"></param>
        public static void SaveAsAsset(this Texture2D texture, string path)
        {
            if(Path.GetExtension(path).ToLower().Contains("jp")) 
                SaveAsJPG(texture, Directory.GetParent((Application.dataPath)).FullName + path);
            else SaveAsPNG(texture, Directory.GetParent((Application.dataPath)).FullName + path);
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Saves the texture asset to the file system, replacing the existing file.
        /// 
        /// This saves a texture at its current path, over wrighting the current file. It will save 
        /// as PNG or JPEG base on file extension.  If the exenstion is missing or incorrect 
        /// (for either PNG or JPEG) it will default to the PNG format, but will not change the 
        /// file name as this would not replace the existing file (with is the purpose of this 
        /// method). 
        /// </summary>
        /// <param name="texture"></param>
        public static void SaveAsset(this Texture2D texture)
        {
            string path = Directory.GetParent((Application.dataPath)).FullName 
                        + Path.DirectorySeparatorChar + AssetDatabase.GetAssetPath(texture);
            if(Path.GetExtension(path).ToLower().EndsWith(".jpg") || Path.GetExtension(path).ToLower().EndsWith(".jpeg")) 
            {
                byte[] bytes = ImageConversion.EncodeToJPG(texture);
                if(Path.IsPathRooted(path)) File.WriteAllBytes(path, bytes);
                else File.WriteAllBytes(Application.dataPath + path, bytes);
            }
            else
            {
                byte[] bytes = ImageConversion.EncodeToPNG(texture);
                if(Path.IsPathRooted(path)) File.WriteAllBytes(path, bytes);
                else File.WriteAllBytes(Application.dataPath + path, bytes);
            }
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Saves the texture as a PNG at the given path. If the file extension 
        /// is absent or not .png, it will add .png to the file name.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="path"></param>
        public static void SaveAsPNG(this Texture2D texture, string path)
        {
            byte[] bytes = ImageConversion.EncodeToPNG(texture);
            if(!path.ToLower().EndsWith(".png")) path += ".png";
            if(Path.IsPathRooted(path)) File.WriteAllBytes(path, bytes);
            else File.WriteAllBytes(Application.dataPath + path, bytes);
        }


        /// <summary>
        /// Saves the texture as a JPEG at the given path.  If the file extension is absent 
        /// or not either .jpg or .jpeg, it will add .jpg to the end of the file name.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="path"></param>
        public static void SaveAsJPG(this Texture2D texture, string path)
        {
            byte[] bytes = ImageConversion.EncodeToJPG(texture);
            if(!(path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg"))) path += ".jpg";
            if(Path.IsPathRooted(path)) File.WriteAllBytes(path, bytes);
            else File.WriteAllBytes(Application.dataPath + path, bytes);
        }


        public static void DrawRectangleSolid(this Texture2D texture, RectInt rect, Color color)
        {
            for(int i = rect.xMin; i < rect.xMax; i++)
                for(int j = rect.yMin; j < rect.yMax; j++)
                {
                    texture.SetPixel(i, j, color);
                } 
            texture.Apply();
        }


        public static void DrawRectangleHollow(this Texture2D texture, RectInt rect, Color color)
        {
            for(int i = rect.xMin; i < rect.xMax; i++)
            {
                texture.SetPixel(i, rect.yMin, color);
                texture.SetPixel(i, rect.yMax, color);
            }
            for(int j = rect.yMin; j < rect.yMax; j++)
            {
                texture.SetPixel(rect.xMin, j, color);
                texture.SetPixel(rect.xMax, j, color);
            } 
            texture.Apply();
        }


        public static void DrawCircleSolid(this Texture2D texture, int x, int y, int r, Color color)
        {
            int rsq = r * r;
            for(int i = 0; i < r; i++)
                for(int j = 0; j < r; j++)
                {
                    if(((i * i) + (j * j)) < rsq) {
                        texture.SetPixel(x + i, y + j, color);
                        texture.SetPixel(x + i, y - j, color);
                        texture.SetPixel(x - i, y - j, color);
                        texture.SetPixel(x - i, y + j, color);
                    }
                } 
            texture.DrawCircleHollow(x, y, r, color);
            texture.Apply();
        }


        public static void DrawCircleHollow(this Texture2D texture, int x, int y, int r, Color color)
        {
            int j = 0; 
            int rsq = r * r;
            for(int i = 0; i <= r; i++)
            {
                j = Mathf.RoundToInt(Mathf.Sqrt(rsq - (i * i)));
                texture.SetPixel(x + i, y + j, color);
                texture.SetPixel(x - i, y - j, color);
                texture.SetPixel(x + i, y - j, color);
                texture.SetPixel(x - i, y + j, color);
                texture.SetPixel(x + j, y + i, color);
                texture.SetPixel(x - j, y - i, color);
                texture.SetPixel(x + j, y - i, color);
                texture.SetPixel(x - j, y + i, color);
            }
            texture.Apply();
        }


        public static void DrawLineSegment(this Texture2D texture, Vector2Int start, Vector2Int end, Color color)
        {    
            bool steep = Math.Abs(end.y - start.y) > Math.Abs(end.x - start.x);
            if (steep)
            {
                int t = start.x; start.x = start.y; start.y = t;
                t = end.x; end.x = end.y; end.y = t;
            }
            if (start.x > end.x)
            {
                int t = start.x; start.x = end.x; end.x = t;
                t = start.y; start.y = end.y; end.y = t;
            }
            
            int dx = end.x - start.x;
            int dy = Math.Abs(end.y - start.y);
            int error = dx / 2;
            int ystep = (start.y < end.y) ? 1 : -1;
            int y = start.y;

            for (int x = start.x; x <= end.x; x++)
            {
                if (steep)
                {
                    texture.SetPixel(y, x, color);
                }
                else
                {
                    texture.SetPixel(x, y, color);
                }
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }


        public static void PlotLineHigh(Texture2D texture, int x1, int y1, int x2, int y2, Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int xi = 1;
            if(dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            float d = (2 * dx) - dy;
            int x = x1;
            for(int j = y1; j <= y2; j++)
            {
                texture.SetPixel(x, j, color);
                if(d > 0)
                {
                    x += xi;
                    d += 2 * (dx - dy);
                }
                else
                {
                    d += d * dx;
                }
            }
            texture.Apply();
        }


        private static void PlotLineLow(Texture2D texture, int x1, int y1, int x2, int y2, Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int yi = 1;
            if(dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            float d = (2 * dy) - dx;
            int y = y1;
            for(int i = x1; i <= x2; i++)
            {
                texture.SetPixel(i, y, color);
                if(d > 0)
                {
                    y += yi;
                    d += 2 * (dy - dx);
                }
                else
                {
                    d += d * dy;
                }
            }
            texture.Apply();
        }


        public static void Copy(this Texture2D texture, Texture2D source)
        {
            int endX = Mathf.Min(texture.width, source.width);
            int endY = Mathf.Min(texture.height, source.height);
            for(int i = 0; i < endX; i++)
                for(int j = 0; j < endY; j++)
                {
                    texture.SetPixel(i, j, source.GetPixel(i, j));
                } 
            texture.Apply();
        }


        public static void CopyTextureRectangle(this Texture2D texture, RectInt rect, Texture2D source)
        {
            for(int i = rect.xMin; i < rect.xMax; i++)
                for(int j = rect.yMin; j < rect.yMax; j++)
                {
                    texture.SetPixel(i, j, source.GetPixel(i, j));
                } 
            texture.Apply();
        }


        public static void DrawRectangleTexture(this Texture2D texture, RectInt rect, Texture2D source, int sourceX, int sourceY)
        {
            int startX = Math.Max(0, rect.xMin);
            int startY = Math.Max(0, rect.yMin);
            int rangeX = Math.Min(Math.Min(rect.xMax - startX, source.width - sourceX), texture.width - startX);
            int rangeY = Math.Min(Math.Min(rect.yMax - startY, source.height - sourceY), texture.height - startY);
            for(int i = 0; i < rangeX; i++)
                for(int j = 0; j < rangeY; j++)
                {
                    texture.SetPixel(i + startX, j + startY, source.GetPixel(i + sourceX, j + sourceY));
                } 
            texture.Apply();
        }


        public static void ColorFill(this Texture2D texture, Color color)
        {
            for(int i = 0; i < texture.width; i++)
                for(int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, color);
                } 
            texture.Apply();
        }


        public static void ClearToBlack(this Texture2D texture)
        {
            for(int i = 0; i < texture.width; i++)
                for(int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, Color.black);
                } 
            texture.Apply();
        }


        public static void ClearToWhite(this Texture2D texture)
        {
            for(int i = 0; i < texture.width; i++)
                for(int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, Color.white);
                } 
            texture.Apply();
        }


        public static void DrawNGonHollow(this Texture2D texture, Color color, Vector2Int center, int radius, int sides, float rotation = 0.0f)
        {
            if(sides < 3) throw new Exception("NGon must have at least 3 sides!");
            float sideAngle = TWO_PI / sides;
            float currentAngle, nextAngle;
            currentAngle = nextAngle = rotation;
            Vector2Int start = new(), end = new();
            for(int i = 1; i < sides; i++)
            {
                nextAngle += sideAngle;
                start.Set((int)(Mathf.Cos(currentAngle) * radius) + center.x, (int)(Mathf.Sin(currentAngle) * radius) + center.y);
                end.Set((int)(Mathf.Cos(nextAngle) * radius) + center.x, (int)(Mathf.Sin(nextAngle) * radius) + center.y);
                texture.DrawLineSegment(start, end, color);
                currentAngle = nextAngle;
            }
            nextAngle = rotation;
            start.Set((int)(Mathf.Cos(currentAngle) * radius) + center.x, (int)(Mathf.Sin(currentAngle) * radius) + center.y);
            end.Set((int)(Mathf.Cos(nextAngle) * radius) + center.x, (int)(Mathf.Sin(nextAngle) * radius) + center.y);
            texture.DrawLineSegment(start, end, color);
        }


        public static void DrawNGonSolid(this Texture2D texture, Color color, Vector2Int center, int radius, int sides, float rotation = 0.0f)
        {
            texture.DrawNGonHollow(color, center, radius, sides, rotation);
            texture.FloodFillToMatch(center, color);
        }


        public static void FloodFill(this Texture2D texture, Vector2Int start, Color color)
        {
            Color startColor = texture.GetPixel(start.x, start.y);
            if(startColor == color) return;
            List<Vector2Int> currentPixels = new();
            List<Vector2Int> nextPixels = new();
            List<Vector2Int> dummyList;
            currentPixels.Add(start);
            while(currentPixels.Count > 0)
            {
                for(int i = 0; i < currentPixels.Count; i++)
                {
                    texture.SetPixel(currentPixels[i].x, currentPixels[i].y, color);
                }
                for(int i = 0; i < currentPixels.Count; i++)
                {
                    if(texture.GetPixel(currentPixels[i].x + 1, currentPixels[i].y) == startColor) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x + 1, currentPixels[i].y));

                    if(texture.GetPixel(currentPixels[i].x - 1, currentPixels[i].y) == startColor) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x - 1, currentPixels[i].y));

                    if(texture.GetPixel(currentPixels[i].x, currentPixels[i].y + 1) == startColor) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x, currentPixels[i].y + 1));
                            
                    if(texture.GetPixel(currentPixels[i].x, currentPixels[i].y - 1) == startColor) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x, currentPixels[i].y - 1));
                }
                dummyList = currentPixels;
                currentPixels = nextPixels;
                nextPixels = dummyList;
                nextPixels.Clear();
            }
            texture.Apply();
        }


        public static void FloodFillToMatch(this Texture2D texture, Vector2Int start, Color color)
        {
            List<Vector2Int> currentPixels = new();
            List<Vector2Int> nextPixels = new();
            List<Vector2Int> dummyList;
            currentPixels.Add(start);
            while(currentPixels.Count > 0)
            {
                for(int i = 0; i < currentPixels.Count; i++)
                {
                    texture.SetPixel(currentPixels[i].x, currentPixels[i].y, color);
                }
                for(int i = 0; i < currentPixels.Count; i++)
                {
                    if(texture.GetPixel(currentPixels[i].x + 1, currentPixels[i].y) != color) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x + 1, currentPixels[i].y));

                    if(texture.GetPixel(currentPixels[i].x - 1, currentPixels[i].y) != color) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x - 1, currentPixels[i].y));

                    if(texture.GetPixel(currentPixels[i].x, currentPixels[i].y + 1) != color) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x, currentPixels[i].y + 1));
                            
                    if(texture.GetPixel(currentPixels[i].x, currentPixels[i].y - 1) != color) 
                            nextPixels.Add(new Vector2Int(currentPixels[i].x, currentPixels[i].y - 1));
                }
                dummyList = currentPixels;
                currentPixels = nextPixels;
                nextPixels = dummyList;
                nextPixels.Clear();
            }
            texture.Apply();
        }


    }


}