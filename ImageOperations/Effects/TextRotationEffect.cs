using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Size = OpenCvSharp.Size;

namespace ImageOperations.Effects
{
    public class TextRotationEffect : IEffect
    {
        public Image Emit(Image source)
        {
            MessageBox.Show(DetectRotationAngle(source).ToString() + "°");
            return source;
        }

        private double DetectRotationAngle(Image source)
        {
            using (var mat = BitmapConverter.ToMat(new Bitmap(source)))
            {
                // Конвертируем изображение в оттенки серого
                var gray = new Mat();
                Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

                // Размываем изображение
                var blurred = new Mat();
                Cv2.GaussianBlur(gray, blurred, new Size(3, 3), 0);

                // Определяем границы
                var edges = new Mat();
                Cv2.Canny(blurred, edges, 100, 200);

                // Поиск линий с использованием преобразования Хафа
                LineSegmentPolar[] lines = Cv2.HoughLines(edges, 1, Math.PI / 180.0, 50);

                double totalAngle = 0;
                int detectedLines = 0;

                foreach (var line in lines)
                {
                    double angle = line.Theta * 180.0 / Math.PI;

                    // Преобразуем радианы в градусы и фильтруем углы
                    if (Math.Abs(angle) < 45 || Math.Abs(angle) > 135)
                    {
                        totalAngle += angle;
                        detectedLines++;
                    }
                }

                return detectedLines > 0 ? totalAngle / detectedLines : 0;
            }
        }
    }
}