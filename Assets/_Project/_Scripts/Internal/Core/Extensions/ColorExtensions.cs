using UnityEngine;

namespace Internal.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetAnalogous(this Color color, float hueOffset = 30f)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            h = (h + hueOffset / 360f) % 1f;
            return Color.HSVToRGB(h, s, v);
        }
        
        public static Color GetComplementary(this Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            h = (h + 0.5f) % 1f;
            return Color.HSVToRGB(h, s, v);
        }

        public static Color GenerateRandomColor(this Color color)
        {
            color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            color.a = 1f;

            return color;
        }
    }
}