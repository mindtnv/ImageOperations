using System.Drawing;

namespace ImageOperations.Effects
{
    public class NegativeEffect : IEffect
    {
        public Image Emit(Image source)
        {
            var bitmap = new Bitmap(source);
            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    bitmap.SetPixel(i, j, newColor);
                }
            }

            return bitmap;
        }
    }
}