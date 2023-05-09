using System;
using System.Drawing;

namespace ImageOperations.Effects
{
    public class SolarizeEffect : IEffect
    {
        public SolarizeEffect(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        
        private static int Solarize(int colorValue, int value)
        {
            var c = Math.Pow(colorValue / 255.0, 1);
            var solarizedValue = c * 255.0;
            return (int) (solarizedValue < value ? 255.0 - solarizedValue : solarizedValue);
        }

        public Image Emit(Image source)
        {
            var image = new Bitmap(source);

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);
                    var red = Solarize(color.R, Value);
                    var green = Solarize(color.G, Value);
                    var blue = Solarize(color.B, Value);
                    var newColor = Color.FromArgb(red, green, blue);
                    image.SetPixel(x, y, newColor);
                }
            }

            return image;
        }
    }
}