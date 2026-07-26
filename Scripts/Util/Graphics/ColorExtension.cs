using System.Diagnostics.Contracts;
using UnityEngine;



namespace kfutils.graphics
{

    public static class ColorExtension
    {

        [Pure] public static Vector3 Vec3RGB(this Color color) => new Vector3(color.r, color.g, color.b);
        [Pure] public static Vector4 Vec4RGB(this Color color) => new Vector4(color.r, color.g, color.b, color.a);
        [Pure] public static Color AsRGB(this Vector3 vector) => new Color(vector.x, vector.y, vector.z, 1.0f);
        [Pure] public static Color AsRGB(this Vector4 vector) => new Color(vector.x, vector.y, vector.z, vector.w);
        public static Color AsHSV(this Vector3 vector, bool hdr = false) => Color.HSVToRGB(vector.x, vector.y, vector.z, hdr);


        public static Vector3 Vec3HSV(this Color color) {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return new Vector3(h, s, v);
        }
        
        
        public static Vector4 Vec4HSV(this Color color) {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return new Vector4(h, s, v, color.a);
        }


        [Pure] public static float RGBDistanceSQ(this Color color, Color other)
        {
            float dr = color.r - other.r;
            float dg = color.g - other.g;
            float db = color.b - other.b;
            return (dr * dr) + (dg * dg) + (db * db);
        }


        [Pure] public static float RGBDistance(this Color color, Color other)
        {
            float dr = color.r - other.r;
            float dg = color.g - other.g;
            float db = color.b - other.b;
            return Mathf.Sqrt((dr * dr) + (dg * dg) + (db * db));
        }


        public static float HSVDistanceSQ(this Color color, Color other)
        {
            Vector3 colorv = color.Vec3HSV();
            Vector3 otherv = other.Vec3HSV();
            float dr = colorv.x - otherv.x;
            float dg = colorv.y - otherv.y;
            float db = colorv.z - otherv.z;
            return Mathf.Sqrt((dr * dr) + (dg * dg) + (db * db));
        }


        public static float HSVDistance(this Color color, Color other)
        {
            Vector3 colorv = color.Vec3HSV();
            Vector3 otherv = other.Vec3HSV();
            return Vector3.Distance(colorv, otherv);
        }


        
    }


}
