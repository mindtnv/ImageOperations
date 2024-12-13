using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class RotateEffect : IEffect
    {
        public double Angle { get; set; }

        public RotateEffect(double angle)
        {
            Angle = angle;
        }

        public Image Emit(Image source)
        {
            var radians = Angle * Math.PI / 180.0;
            var cosTheta = Math.Cos(radians);
            var sinTheta = Math.Sin(radians);
            var sourceWidth = source.Width;
            var sourceHeight = source.Height;

            // Определяем размеры нового изображения
            var newWidth = (int) (Math.Abs(sourceWidth * cosTheta) + Math.Abs(sourceHeight * sinTheta));
            var newHeight = (int) (Math.Abs(sourceWidth * sinTheta) + Math.Abs(sourceHeight * cosTheta));

            var newImage = new Bitmap(newWidth, newHeight);
            var centerX = sourceWidth / 2.0;
            var centerY = sourceHeight / 2.0;
            var newCenterX = newWidth / 2.0;
            var newCenterY = newHeight / 2.0;

            for (var x = 0; x < newWidth; x++)
            {
                for (var y = 0; y < newHeight; y++)
                {
                    // Переводим координаты в систему центра изображения
                    var sourceX = (x - newCenterX) * cosTheta + (y - newCenterY) * sinTheta + centerX;
                    var sourceY = -(x - newCenterX) * sinTheta + (y - newCenterY) * cosTheta + centerY;

                    // Проверяем, попадают ли вычисленные координаты в пределы исходного изображения
                    if (!(sourceX >= 0) || !(sourceX < sourceWidth) || !(sourceY >= 0) || !(sourceY < sourceHeight))
                        continue;

                    var nearestX = (int) Math.Floor(sourceX);
                    var nearestY = (int) Math.Floor(sourceY);
                    var color = ((Bitmap) source).GetPixel(nearestX, nearestY);
                    newImage.SetPixel(x, y, color);
                }
            }

            return newImage;
        }
    }
}