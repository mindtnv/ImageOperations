using System;
using System.Linq;
using System.Windows.Forms;
using ImageOperations.Effects;

namespace ImageOperations
{
    public static class EffectsWindow
    {
        private static Lazy<Type[]> getEffectTypesLazy = new Lazy<Type[]>(() =>
        {
            return typeof(EffectsWindow).Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IEffect))).ToArray();
        });

        public static Type[] EffectTypes => getEffectTypesLazy.Value;

        public static Type ShowSelectEffectDialog()
        {
            var prompt = new Form
            {
                Width = 280,
                Height = EffectTypes.Length * 45,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Выберете эффект",
                StartPosition = FormStartPosition.CenterScreen,
            };
        
            var mainPanel = new Panel {Left = 30, Top = 30, Height = EffectTypes.Length * 45 + 30};
            Type selectedType = null;
        
            for (var i = EffectTypes.Length - 1; i >= 0; i--)
            {
                var effectType = EffectTypes[i];
                var button = new Button
                {
                    Text = effectType.Name,
                    Dock = DockStyle.Top,
                };
                button.Click += (sender, args) =>
                {
                    var btn = sender as Button;
                    selectedType = EffectTypes.First(x => x.Name == btn.Text);
                    prompt.Close();
                };
                mainPanel.Controls.Add(button);
            }
        
            prompt.Controls.Add(mainPanel);
            prompt.ShowDialog();

            return selectedType;
        }
    }
}