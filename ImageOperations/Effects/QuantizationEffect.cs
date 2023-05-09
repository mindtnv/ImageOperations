using System.Drawing;
using System.Drawing.Imaging;

namespace ImageOperations.Effects
{
    public class QuantizationEffect : IEffect
    {
        public QuantizationEffect(int levels)
        {
            Levels = levels;
        }

        public int Levels { get; set; }

        public Image Emit(Image source)
        {
            var result = new Bitmap(source);
            var step = 255 / Levels;
            for (var x = 0; x < result.Width; x++)
            {
                for (var y = 0; y < result.Height; y++)
                {
                    var c = result.GetPixel(x, y);
                    var r = c.R / step * step;
                    var g = c.G / step * step;
                    var b = c.B / step * step;
                    result.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return result;
        }
    }
}