using Microsoft.Win32;
using System.IO;
using System.IO.Compression;

namespace AutoLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string filePath;
        private string fileDirectory;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileDirectory = Path.GetDirectoryName(filePath);
                SelectedFileText.Text = Path.GetFileName(filePath);
            }
        }

        private void MakeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (filePath == null) return;

            string directoryName = Path.GetFileNameWithoutExtension(filePath);
            DirectoryInfo outputDirectory = Directory.CreateDirectory($"{fileDirectory}/{directoryName}");

            ZipFile.ExtractToDirectory(filePath, outputDirectory.FullName);

            var file = new KeymapFileGetter().GetKeymapFile(outputDirectory);
            KeymapModifier.InsertCode(file);
        }
    }
}