using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class HoughLineHighlightEffect : IEffect
    {
        public HoughLineHighlightEffect()
        {
        }

        public Image Emit(Image source)
        {
            var image = new Bitmap(source);

            var grayscaleImage = new Bitmap(new GrayscaleEffect().Emit(image));
            // Применяем фильтр для нахождения краев (например, Собеля)
            var edgesImage = new Bitmap(new SobelFilter().Emit(grayscaleImage));
            // Применяем преобразование Хафа
            var (houghMap, thetaValues, maxDistance) = HoughTransform(edgesImage);
            // Находим пик линии в пространстве Хафа
            var (maxThetaIndex, maxRadiusIndex) = FindHoughPeak(houghMap);

            // Подсвечиваем найденную линию на изображении
            HighlightLine(image, thetaValues[maxThetaIndex], maxRadiusIndex - maxDistance);

            return image;
        }

        private (int[,], double[], int) HoughTransform(Bitmap edgesImage)
        {
            int width = edgesImage.Width;
            int height = edgesImage.Height;
            int maxDistance = (int) Math.Sqrt(width * width + height * height);
            int[,] houghMap = new int[180, maxDistance * 2];
            double[] thetas = new double[180];

            for (int t = 0; t < 180; t++)
            {
                thetas[t] = (Math.PI / 180.0) * t;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (edgesImage.GetPixel(x, y).R > 128) // порог для фильтрации слабых линий
                    {
                        for (int theta = 0; theta < 180; theta++)
                        {
                            int r = (int) (x * Math.Cos(thetas[theta]) + y * Math.Sin(thetas[theta]));
                            r += maxDistance;
                            if (r >= 0 && r < maxDistance * 2)
                            {
                                houghMap[theta, r]++;
                            }
                        }
                    }
                }
            }

            return (houghMap, thetas, maxDistance);
        }

        private (int, int) FindHoughPeak(int[,] houghMap)
        {
            int maxTheta = 0, maxR = 0, maxVotes = 0;

            for (int theta = 0; theta < houghMap.GetLength(0); theta++)
            {
                for (int r = 0; r < houghMap.GetLength(1); r++)
                {
                    if (houghMap[theta, r] > maxVotes)
                    {
                        maxVotes = houghMap[theta, r];
                        maxTheta = theta;
                        maxR = r;
                    }
                }
            }

            return (maxTheta, maxR);
        }

        private void HighlightLine(Bitmap image, double theta, int r)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int width = image.Width;
                int height = image.Height;

                double cosT = Math.Cos(theta);
                double sinT = Math.Sin(theta);

                if (sinT != 0)
                {
                    int y1 = (int) ((r - (0 * cosT)) / sinT);
                    int y2 = (int) ((r - (width * cosT)) / sinT);
                    g.DrawLine(new Pen(Color.Red, 2), new Point(0, y1), new Point(width, y2));
                }
                else
                {
                    int x = (int) (r / cosT);
                    g.DrawLine(new Pen(Color.Red, 2), new Point(x, 0), new Point(x, height));
                }
            }
        }
    }
}