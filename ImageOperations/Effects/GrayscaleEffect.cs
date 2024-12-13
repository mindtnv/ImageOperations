using System.Drawing;

namespace ImageOperations.Effects
{
    public class GrayscaleEffect : IEffect
    {
        public Image Emit(Image source)
        {
            var bitmap = new Bitmap(source);
            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var x = (int) (0.299 * color.R) +
                            (int) (0.587 * color.G) +
                            (int) (0.114 * color.B);
                    var newColor = Color.FromArgb(x, x, x);
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            return bitmap;
        }
    }
}