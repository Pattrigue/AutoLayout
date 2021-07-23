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

                label.Content = value;
                Properties.Settings.Default[key] = value;
            }
        }

        private readonly Label label;
        private readonly string key;

        private string value;

        public AppSetting(Label label, string key, string value)
        {
            this.label = label;
            this.key = key;
            Value = value;
        }
    }
}