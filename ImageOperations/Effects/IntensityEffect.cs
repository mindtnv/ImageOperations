using System;
using System.Drawing;
using ImageOperations.Extensions;

namespace ImageOperations.Effects
{
    public class IntensityEffect : IEffect
    {
        public double Factor { get; set; }
        public IntensityEffectAction Action { get; set; }

        public IntensityEffect(IntensityEffectAction action, double factor)
        {
            Action = action;
            Factor = factor;
        }

        public Image Emit(Image source)
        {
            if (Action == IntensityEffectAction.Increase)
                return IncreaseIntensity(source);

            return DecreaseIntensity(source);
        }

        private Image DecreaseIntensity(Image source)
        {
            var image = new Bitmap(source);
            var sumR = 0.0;
            var sumG = 0.0;
            var sumB = 0.0;
            var totalPixels = image.Width * image.Height;

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;
                }
            }

            // Вычисляем среднее значение интенсивности пикселей
            var meanR = sumR / totalPixels;
            var meanG = sumG / totalPixels;
            var meanB = sumB / totalPixels;

            var varianceR = 0.0;
            var varianceG = 0.0;
            var varianceB = 0.0;
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    varianceR += Math.Pow(pixel.R - meanR, 2);
                    varianceG += Math.Pow(pixel.G - meanR, 2);
                    varianceB += Math.Pow(pixel.B - meanR, 2);
                }
            }

            // Вычисляем стандартное отклонение интенсивности пикселей
            var stdDevR = Math.Sqrt(varianceR / totalPixels);
            var stdDevG = Math.Sqrt(varianceG / totalPixels);
            var stdDevB = Math.Sqrt(varianceB / totalPixels);

            // Применяем повышение контрастности
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var newR = MathExtensions.Clamp(0, 255,
                        (int) (meanR - stdDevR * Factor * (pixel.R - meanR))
                    );
                    var newG = MathExtensions.Clamp(0, 255,
                        (int) (meanG - stdDevG * Factor * (pixel.G - meanG))
                    );
                    var newB = MathExtensions.Clamp(0, 255,
                        (int) (meanB - stdDevB * Factor * (pixel.B - meanB))
                    );
                    image.SetPixel(x, y, Color.FromArgb(pixel.A, newR, newG, newB));
                }
            }

            return image;
        }

        private Image IncreaseIntensity(Image source)
        {
            var image = new Bitmap(source);
            var sumR = 0.0;
            var sumG = 0.0;
            var sumB = 0.0;
            var totalPixels = image.Width * image.Height;

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;
                }
            }

            // Вычисляем среднее значение интенсивности пикселей
            var meanR = sumR / totalPixels;
            var meanG = sumG / totalPixels;
            var meanB = sumB / totalPixels;

            var varianceR = 0.0;
            var varianceG = 0.0;
            var varianceB = 0.0;
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    varianceR += Math.Pow(pixel.R - meanR, 2);
                    varianceG += Math.Pow(pixel.G - meanR, 2);
                    varianceB += Math.Pow(pixel.B - meanR, 2);
                }
            }

            // Вычисляем стандартное отклонение интенсивности пикселей
            var stdDevR = Math.Sqrt(varianceR / totalPixels);
            var stdDevG = Math.Sqrt(varianceG / totalPixels);
            var stdDevB = Math.Sqrt(varianceB / totalPixels);

            // Применяем повышение контрастности
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var newR = MathExtensions.Clamp(0, 255,
                        (int) (stdDevR * Factor * (pixel.R - meanR) + meanR)
                    );
                    var newG = MathExtensions.Clamp(0, 255,
                        (int) (stdDevG * Factor * (pixel.G - meanG) + meanG)
                    );
                    var newB = MathExtensions.Clamp(0, 255,
                        (int) (stdDevB * Factor * (pixel.B - meanB) + meanB)
                    );
                    image.SetPixel(x, y, Color.FromArgb(pixel.A, newR, newG, newB));
                }
            }

            return image;
        }
    }

    public enum IntensityEffectAction
    {
        Increase,
        Descrease,
    }
}