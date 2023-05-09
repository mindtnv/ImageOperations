using System;
using System.Drawing;
using ImageOperations.Extensions;

namespace ImageOperations.Effects
{
    public class NegativeWithClampEffect : IEffect
    {
        public NegativeWithClampEffect(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; set; }
        public int Max { get; set; }

        public Image Emit(Image source)
        {
            var bitmap = new Bitmap(source);
            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var r = MathExtensions.Clamp(Min, Max, 255 - color.R);
                    var g = MathExtensions.Clamp(Min, Max, 255 - color.G);
                    var b = MathExtensions.Clamp(Min, Max, 255 - color.B);
                    var newColor = Color.FromArgb(r, g, b);
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            return bitmap;
        }
    }
}