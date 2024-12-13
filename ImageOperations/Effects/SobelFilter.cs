using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageOperations.Effects
{
    public class SobelFilter : IEffect
    {
        private readonly int[,] sobelX = new int[,]
        {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
        };

        private readonly int[,] sobelY = new int[,]
        {
            { -1, -2, -1 },
            { 0, 0, 0 },
            { 1, 2, 1 }
        };

        public Image Emit(Image source)
        {
            Bitmap grayImage = ConvertToGrayscale((Bitmap)source);
            Bitmap resultImage = new Bitmap(grayImage.Width, grayImage.Height);

            for (int x = 1; x < grayImage.Width - 1; x++)
            {
                for (int y = 1; y < grayImage.Height - 1; y++)
                {
                    int gx = ApplyKernel(grayImage, sobelX, x, y);
                    int gy = ApplyKernel(grayImage, sobelY, x, y);

                    int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
                    magnitude = Math.Min(255, Math.Max(0, magnitude));

                    resultImage.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
                }
            }

            return resultImage;
        }

        private Bitmap ConvertToGrayscale(Bitmap source)
        {
            Bitmap grayImage = new Bitmap(source.Width, source.Height);

            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    Color originalColor = source.GetPixel(x, y);
                    int grayColor = (int)((originalColor.R * 0.299) + (originalColor.G * 0.587) + (originalColor.B * 0.114));
                    grayImage.SetPixel(x, y, Color.FromArgb(grayColor, grayColor, grayColor));
                }
            }

            return grayImage;
        }

        private int ApplyKernel(Bitmap image, int[,] kernel, int x, int y)
        {
            int result = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Color pixel = image.GetPixel(x + i, y + j);
                    int grayValue = pixel.R; // Since the image is grayscale, R=G=B
                    result += grayValue * kernel[i + 1, j + 1];
                }
            }
            return result;
        }
    }
}