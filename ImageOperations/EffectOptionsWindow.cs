using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using ImageOperations.Effects;

namespace ImageOperations
{
    public static class EffectOptionsWindow
    {
        public static IEffect ConfigureEffect(Type effectType)
        {
            var constructor = effectType.GetConstructors().FirstOrDefault();
            if (constructor == null)
                throw new Exception($"Provide constructor for effect {effectType.Name}");

            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
                return Activator.CreateInstance(effectType) as IEffect;

            var prompt = new Form
            {
                Width = 280,
                Height = parameters.Length * 120 + 60,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Введите параметры эффекта",
                StartPosition = FormStartPosition.CenterScreen,
            };
            var confirmation = new Button {Text = "Применить", DialogResult = DialogResult.OK, Dock = DockStyle.Top};
            var mainPanel = new Panel {Left = 30, Top = 30, Height = parameters.Length * 120 + 30};
            mainPanel.Controls.Add(confirmation);
            var values = new Dictionary<string, string>();

            for (var i = parameters.Length - 1; i >= 0; i--)
            {
                var parameter = parameters[i];
                if (parameter.ParameterType.IsEnum)
                {
                    var textLabel = new Label
                    {
                        Name = $"{parameter.Name}Label",
                        Text = parameter.Name,
                        Dock = DockStyle.Top
                    };
                    var box = new ComboBox
                    {
                        Dock = DockStyle.Top,
                    };
                    var enumValues = Enum.GetValues(parameter.ParameterType);
                    foreach (var enumValue in enumValues)
                    {
                        box.Items.Add(enumValue.ToString());
                    }

                    box.Text = enumValues.GetValue(0).ToString();
                    values[parameter.Name] = enumValues.GetValue(0).ToString();
                    box.SelectedValueChanged += (sender, args) =>
                    {
                        values[parameter.Name] = (sender as ComboBox).Text; 
                    };
                    mainPanel.Controls.Add(box);
                    mainPanel.Controls.Add(textLabel);
                }
                if (parameter.ParameterType == typeof(int) || parameter.ParameterType == typeof(double))
                {
                    var textLabel = new Label
                    {
                        Name = $"{parameter.Name}Label",
                        Text = parameter.Name,
                        Dock = DockStyle.Top
                    };
                    var textBox = new TextBox
                    {
                        Name = $"{parameter.Name}Input",
                        Width = 30,
                        Dock = DockStyle.Top,
                    };
                    var defaultValue = GetDefaultValue(parameter.ParameterType).ToString();
                    textBox.Text = defaultValue;
                    values[parameter.Name] = defaultValue;
                    mainPanel.Controls.Add(textBox);
                    mainPanel.Controls.Add(textLabel);
                    textBox.TextChanged += (sender, args) => { values[parameter.Name] = (sender as TextBox).Text; };
                }
            }

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(mainPanel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();

            var @params = parameters.Select(x =>
                                        CastFromString(values[x.Name], x.ParameterType)
                                    )
                                    .ToArray();

            return Activator.CreateInstance(effectType, @params) as IEffect;
        }

        private static object CastFromString(string s, Type destType)
        {
            if (destType.IsEnum)
                return Enum.Parse(destType, s);
            if (destType == typeof(int))
                return Int32.Parse(s); 
            if (destType == typeof(double))
                return double.Parse(s);

            if (destType == typeof(string))
                return s;

            return GetDefaultValue(destType);
        }

        private static object GetDefaultValue(Type destType)
        {
            if (destType.IsValueType)
                return Activator.CreateInstance(destType);

            return null;
        }
    }
}