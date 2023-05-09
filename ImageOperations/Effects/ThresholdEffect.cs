using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class ThresholdEffect : IEffect
    {
        public ThresholdEffect(ThresholdEffectType type, int max)
        {
            Type = type;
            Max = max;
        }

        public int Max { get; set; }
        public ThresholdEffectType Type { get; set; }

        public Image Emit(Image source)
        {
            if (Type == ThresholdEffectType.Simple)
                return SimpleThreshold(source);


            if (Type == ThresholdEffectType.Otsu)
                return OtsuThreshold(source);

            throw new NotSupportedException();
        }

        private Image SimpleThreshold(Image source)
        {
            var image = new Bitmap(source);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int intensity = (int) (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    if (intensity > Max)
                    {
                        image.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return image;
        }

        private Image OtsuThreshold(Image source)
        {
            var image = new Bitmap(source);
            // Вычисляем гистограмму интенсивности пикселей
            int[] histogram = new int[256];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int intensity = (int) (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    histogram[intensity]++;
                }
            }

            // Вычисляем общее среднее значение интенсивности пикселей
            int sum = 0;
            int totalPixels = image.Width * image.Height;
            for (int i = 0; i < 256; i++)
            {
                sum += i * histogram[i];
            }

            double mean = (double) sum / totalPixels;

            // Вычисляем межклассовую дисперсию для каждого порога
            double maxVariance = 0;
            int threshold = 0;
            int sum1 = 0, sum2 = 0, count1 = 0, count2 = 0;
            for (int i = 0; i < 256; i++)
            {
                sum1 += i * histogram[i];
                count1 += histogram[i];
                double w1 = (double) count1 / totalPixels;
                if (count1 == 0) continue;
                double mean1 = (double) sum1 / count1;
                sum2 = sum - sum1;
                count2 = totalPixels - count1;
                if (count2 == 0) break;
                double mean2 = (double) sum2 / count2;
                double betweenVariance = w1 * (1 - w1) * (mean1 - mean2) * (mean1 - mean2);
                if (betweenVariance > maxVariance)
                {
                    maxVariance = betweenVariance;
                    threshold = i;
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int intensity = (int) (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    if (intensity > threshold)
                    {
                        image.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return image;
        }
    }

    public enum ThresholdEffectType
    {
        Simple,
        Otsu,
    }
}