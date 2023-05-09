using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class GammaEffect : IEffect
    {
        public GammaEffect(double gamma)
        {
            Gamma = gamma;
        }

        public double Gamma { get; set; }

        public Image Emit(Image source)
        {
            var image = new Bitmap(source);
            var gammaTable = new double[256];

            for (var i = 0; i < 256; i++)
                gammaTable[i] = Math.Pow(i / 255.0, Gamma) * 255.0;

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);
                    var r = (int) gammaTable[color.R];
                    var g = (int) gammaTable[color.G];
                    var b = (int) gammaTable[color.B];
                    image.SetPixel(x, y, Color.FromArgb(
                        color.A,
                        r,
                        g,
                        b)
                    );
                }
            }

            return image;
        }
    }
}