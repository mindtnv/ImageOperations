using System.Drawing;
using ImageOperations.Extensions;

namespace ImageOperations.Effects
{
    public class ContrastEffect : IEffect
    {
        public int Contrast { get; set; }

        public ContrastEffect(int contrast)
        {
            Contrast = contrast;
        }

        public Image Emit(Image source)
        {
            var image = new Bitmap(source);

            var factor = 255.0 * (Contrast + 255) / (255.0 * (255 - Contrast));
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);
                    var red = color.R / 255.0;
                    var green = color.G / 255.0;
                    var blue = color.B / 255.0;
                    var newRed = MathExtensions.Clamp(0, 1, factor * (red - 0.5) + 0.5);
                    var newGreen = MathExtensions.Clamp(0, 1, factor * (green - 0.5) + 0.5);
                    var newBlue = MathExtensions.Clamp(0, 1, factor * (blue - 0.5) + 0.5);

                    var newColor = Color.FromArgb(
                        color.A,
                        (int) (newRed * 255),
                        (int) (newGreen * 255),
                        (int) (newBlue * 255)
                    );

                    image.SetPixel(x, y, newColor);
                }
            }

            return image;
        }
    }
}