using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class CircleDetectionEffect : IEffect
    {

        public Image Emit(Image source)
        {
            var bitmap = new Bitmap(new ThresholdEffect(ThresholdEffectType.Otsu, 128).Emit(source));
            var width = bitmap.Width;
            var height = bitmap.Height;

            var houghSpace = new int[width, height, Math.Min(width, height) / 2];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    // Проверяем, соответствует ли пиксель порогу для обработки
                    if (IsEdgePixel(bitmap, x, y))
                    {
                        for (var a = 0; a < width; a++)
                        {
                            for (var b = 0; b < height; b++)
                            {
                                var r = Math.Sqrt(Math.Pow(x - a, 2) + Math.Pow(y - b, 2));
                                var radius = (int)Math.Round(r);
                                if (radius >= 0 && radius < houghSpace.GetLength(2))
                                {
                                    houghSpace[a, b, radius]++;
                                }
                            }
                        }
                    }
                }
            }

            int maxA = 0, maxB = 0, maxR = 0;
            for (var a = 0; a < width; a++)
            {
                for (var b = 0; b < height; b++)
                {
                    for (var r = 0; r < houghSpace.GetLength(2); r++)
                    {
                        if (houghSpace[maxA, maxB, maxR] < houghSpace[a, b, r])
                        {
                            maxA = a;
                            maxB = b;
                            maxR = r;
                        }
                    }
                }
            }

            var result = new Bitmap(source);
            using (var g = Graphics.FromImage(result))
            {
                var pen = new Pen(Color.Green, 4);
                g.DrawEllipse(pen, maxA - maxR, maxB - maxR, maxR * 2, maxR * 2);
            }

            return result;
        }

        private bool IsEdgePixel(Bitmap bitmap, int x, int y)
        {
            // Простой порог - черные пиксели считаем границей
            Color pixel = bitmap.GetPixel(x, y);
            return pixel.R < 128;
        }
    }
}