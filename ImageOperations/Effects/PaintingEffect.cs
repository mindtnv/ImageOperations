using System.Drawing;

namespace ImageOperations.Effects
{
    public class PaintingEffect : IEffect
    {
        public Image Emit(Image source)
        {
            var image = new Bitmap(source);
            var palette = new[]
            {
                Color.Navy,
                Color.Blue,
                Color.Purple,
                Color.MediumPurple,
                Color.MediumPurple,
            };

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var c = image.GetPixel(x, y);
                    var gray = (int) (c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
                    var index = (int) (gray / 255.0 * (palette.Length - 1));
                    image.SetPixel(x, y, Color.FromArgb(c.A, palette[index].R, palette[index].G, palette[index].B));
                }
            }

            return image;
        }
    }
}