using System.Windows.Controls;

namespace AutoLayout
{
    public sealed class AppSetting
    {
        public string Value
        {
            get => value;
            set
            {
                this.value = value;

                if (value == string.Empty) return;

                Properties.Settings.Default[key] = value;

                switch (content)
                {
                    case Label label:
                        label.Content = value;
                        break;
                    case TextBox textBox:
                        textBox.Text = value;
                        break;
                }
            }
        }

        private readonly Control content;
        private readonly string key;

        private string value;

        public AppSetting(Control label, string key, string value)
        {
            this.content = label;
            this.key = key;
            Value = value;
        }
    }
}