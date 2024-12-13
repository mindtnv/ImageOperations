using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class ScaleEffect : IEffect
    {
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }

        public ScaleEffect(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        public Image Emit(Image source)
        {
            var sourceWidth = source.Width;
            var sourceHeight = source.Height;

            // Определяем размеры нового изображения
            var newWidth = (int) (sourceWidth * ScaleX);
            var newHeight = (int) (sourceHeight * ScaleY);
            var scaledImage = new Bitmap(newWidth, newHeight);

            for (var x = 0; x < newWidth; x++)
            {
                for (var y = 0; y < newHeight; y++)
                {
                    // Переводим координаты из нового изображения в координаты исходного изображения
                    var sourceX = x / ScaleX;
                    var sourceY = y / ScaleY;
                    var nearestX = (int) Math.Min(Math.Max(Math.Round(sourceX), 0), sourceWidth - 1);
                    var nearestY = (int) Math.Min(Math.Max(Math.Round(sourceY), 0), sourceHeight - 1);

                    var color = ((Bitmap) source).GetPixel(nearestX, nearestY);
                    scaledImage.SetPixel(x, y, color);
                }
            }

            return scaledImage;
        }
    }
}