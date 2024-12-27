using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;

namespace ImageOperations.Effects
{
    public class DetectNumbersEffect : IEffect
    {
        public Image Emit(Image source)
        {
            Cv2.DestroyAllWindows();

            // Func<Image, Bitmap> digitTransform = (Image x) => (Bitmap)new ThresholdEffect(ThresholdEffectType.Otsu,  50).Emit(x);
            Func<Image, Bitmap> digitTransform = (Image x) => (Bitmap) new QuantizationEffect(4).Emit(new ContrastEffect(30).Emit(x));

            var digitRegions = FindDigitRegions(source).OrderBy(x => x.X).ThenBy(x => x.Y);
            var reference = GenerateReferenceDigits(16, 16);

            var detectedDigits = "";
            var i = 0;
            foreach (var region in digitRegions)
            {
                var digitImage = CropImage(source, region);
                digitImage = digitTransform(digitImage);
                var (detectedDigit, a, b) = DetectDigitWithRotation(digitImage, reference, digitTransform);
                detectedDigits += detectedDigit;
                Cv2.ImShow($"{i}_a", a.ToMat());
                Cv2.ImShow($"{i}_b", b.ToMat());
                i++;
            }

            MessageBox.Show(detectedDigits);
            return source;
        }

        private List<Rect> FindDigitRegions(Image source)
        {
            var digitRegions = new List<Rect>();
            using (var mat = BitmapConverter.ToMat(new Bitmap(source)))
            {
                var gray = new Mat();
                Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.AdaptiveThreshold(gray, gray, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 11,
                    2);

                // Поиск контуров
                Cv2.FindContours(gray, out var contours, out _, RetrievalModes.External,
                    ContourApproximationModes.ApproxSimple);
                foreach (var contour in contours)
                {
                    var rect = Cv2.BoundingRect(contour);
                    // Мы можем добавлять условия для фильтрации регионов, например, по размеру
                    if (rect.Width > 5 && rect.Height > 5)
                    {
                        digitRegions.Add(rect);
                    }
                }
            }

            return digitRegions;
        }

        private Bitmap CropImage(Image source, Rect region)
        {
            var bitmap = new Bitmap(source);
            var cropRect = new Rectangle(region.X, region.Y, region.Width, region.Height);
            return bitmap.Clone(cropRect, bitmap.PixelFormat);
        }

        public Dictionary<int, Bitmap> GenerateReferenceDigits(int width, int height)
        {
            var referenceDigits = new Dictionary<int, Bitmap>();
            var fontFamily = new FontFamily("Times New Roman");
            var font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Pixel); // Размер шрифта и стиль

            for (int i = 0; i <= 9; i++)
            {
                var digitImage = CreateReferenceDigit(i, width * 2, height * 2, font);
                if (i != 1)
                {
                    var trimmedImage = CropImage(digitImage, FindDigitRegions(digitImage).First());
                    referenceDigits[i] = trimmedImage;
                }
                else
                {
                    var trimmedImage = CropImage(digitImage, FindDigitRegions(digitImage).First())
                                       .ToMat()
                                       .CopyMakeBorder(1, 1, 3, 3, BorderTypes.Isolated, Scalar.White);
                    referenceDigits[i] = trimmedImage.ToBitmap();
                    Cv2.ImShow("111111111111111111", referenceDigits[i].ToMat());
                }
            }

            return referenceDigits;
        }

        private static Bitmap CreateReferenceDigit(int digit, int width, int height, Font font)
        {
            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    g.DrawString(digit.ToString(), font, Brushes.Black, new RectangleF(2, 2, width, height),
                        stringFormat);
                }
            }

            return bitmap;
        }

        private static double ComputeNCC(Image img1, Image img2)
        {
            using (Mat mat1 = BitmapConverter.ToMat(new Bitmap(img1)))
                using (Mat mat2 = BitmapConverter.ToMat(new Bitmap(img2)))
                {
                    // Convert to grayscale
                    if (mat1.Channels() > 1)
                        Cv2.CvtColor(mat1, mat1, ColorConversionCodes.BGR2GRAY);
                    if (mat2.Channels() > 1)
                        Cv2.CvtColor(mat2, mat2, ColorConversionCodes.BGR2GRAY);

                    // Equalize the size
                    Cv2.Resize(mat2, mat2, mat1.Size());

                    // Compute the normalized cross-correlation
                    Mat ncc = new Mat();
                    Cv2.MatchTemplate(mat1, mat2, ncc, TemplateMatchModes.CCoeffNormed);
                    return ncc.At<float>(0, 0); // Value between -1 and 1
                }
        }

        private static (int, Bitmap, Bitmap) DetectDigitWithRotation(Bitmap digit,
            Dictionary<int, Bitmap> referenceDigits, Func<Image, Bitmap> digitTransform = default)
        {
            int bestMatch = -1;
            double minDifference = double.MaxValue;
            Bitmap minDifferenceImage = null;
            Bitmap minDifferenceReference = null;

            for (double angle = -35; angle <= 35; angle += 0.4)
            {
                var rotatedDigit = RotateImage(digit, angle);

                foreach (var kvp in referenceDigits)
                {
                    // Нормализуем размер изображений перед сравнением
                    var resizedReference = ResizeImage(kvp.Value, rotatedDigit.Width, rotatedDigit.Height);
                    var referenceResized = digitTransform != null ? digitTransform(resizedReference) : resizedReference;
                    // Вычисляем разницу с эталонной цифрой
                    var difference = ComputeMSE(
                        rotatedDigit, referenceResized);
                    if (difference < minDifference)
                    {
                        minDifference = difference;
                        bestMatch = kvp.Key;
                        minDifferenceImage = rotatedDigit;
                        minDifferenceReference = referenceResized;
                    }
                }
            }

            return (bestMatch, minDifferenceImage, minDifferenceReference);
        }

        private static Bitmap RotateImage(Bitmap bitmap, double angle)
        {
            using (Mat src = BitmapConverter.ToMat(bitmap))
            {
                var center = new Point2f(src.Width / 2, src.Height / 2);
                var rotationMat = Cv2.GetRotationMatrix2D(center, angle, 1.0);
                var dst = new Mat();
                Cv2.WarpAffine(src, dst, rotationMat, src.Size(), borderValue: Scalar.White);
                return dst.ToBitmap();
            }
        }

        private static Bitmap ResizeImage(Bitmap bitmap, int width, int height)
        {
            using (Mat src = BitmapConverter.ToMat(bitmap))
            {
                var dst = new Mat();
                Cv2.Resize(src, dst, new OpenCvSharp.Size(width, height), 0, 0, InterpolationFlags.Area);
                return dst.ToBitmap();
            }
        }

        private static double ComputeMSE(Bitmap img1, Bitmap img2)
        {
            double mse = 0;
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    var pixel1 = img1.GetPixel(x, y);
                    var pixel2 = img2.GetPixel(x, y);
                    mse += Math.Pow(pixel1.R - pixel2.R, 2);
                    mse += Math.Pow(pixel1.G - pixel2.G, 2);
                    mse += Math.Pow(pixel1.B - pixel2.B, 2);
                }
            }

            return mse / (img1.Width * img1.Height * 3);
        }

        private static double ComputeImageDifference(Bitmap img1, Bitmap img2)
        {
            double totalDifference = 0;

            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    var pixel1 = img1.GetPixel(x, y);
                    var pixel2 = img2.GetPixel(x, y);

                    totalDifference += Math.Abs(pixel1.R - pixel2.R) +
                                       Math.Abs(pixel1.G - pixel2.G) +
                                       Math.Abs(pixel1.B - pixel2.B);
                }
            }

            return totalDifference;
        }
    }
}