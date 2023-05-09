using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PropertyAttributes = System.Reflection.PropertyAttributes;

namespace ImageOperations
{
    public partial class Form1 : Form
    {
        private Image _image;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var stream = openFileDialog.OpenFile();
                var image = Image.FromStream(stream);
                _image = image;
                updateImage(_image);
            }
        }

        private void setInitialImageButton_Click(object sender, EventArgs e)
        {
            updateImage(_image);
        }

        private void updateImage(Image image)
        {
            pictureBox.Image = image;
            chart.Series.Clear();
            var rgbSeries = new Series();
            var rSeries = new Series();
            var gSeries = new Series();
            var bSeries = new Series();

            var bitmap = new Bitmap(image);
            var rgb = new int[256];
            var r = new int[256];
            var g = new int[256];
            var b = new int[256];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var grayscaled = (color.R + color.G + color.B) / 3;
                    r[color.R] += 1;
                    g[color.G] += 1;
                    b[color.B] += 1;
                    rgb[grayscaled] += 1;
                }
            }

            for (int i = 0; i < 256; i++)
            {
                rgbSeries.Points.AddXY(i, rgb[i]);
                rSeries.Points.AddXY(i, r[i]);
                gSeries.Points.AddXY(i, g[i]);
                bSeries.Points.AddXY(i, b[i]);
            }

            rgbSeries.Color = Color.Gray;
            rSeries.Color = Color.Red;
            gSeries.Color = Color.Green;
            bSeries.Color = Color.Blue;
            chart.Series.Add(rSeries);
            chart.Series.Add(gSeries);
            chart.Series.Add(bSeries);
            chart.Series.Add(rgbSeries);
        }

        private void applyEffectButton_Click(object sender, EventArgs e)
        {
            var effectType = EffectsWindow.ShowSelectEffectDialog();
            if (effectType == null)
                return;
            var effect = EffectOptionsWindow.ConfigureEffect(effectType);
            if (effect == null)
                return;
            
            updateImage(effect.Emit(pictureBox.Image));
        }
    }
}