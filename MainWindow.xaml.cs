using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace AutoLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string QmkPath
        {
            get => qmkPath;
            set
            {
                qmkPath = value;

                if (value == string.Empty) return;

                SelectedQmkPathText.Content = value;
            }
        }

        private string LayoutZipFile
        {
            get => layoutZipFile;
            set
            {
                layoutZipFile = value;

                if (value == string.Empty) return;

                SelectedFileText.Content = Path.GetFileName(layoutZipFile);
            }
        }

        private string OutputPath
        {
            get => outputPath;
            set
            {
                outputPath = value;
                
                if (value == string.Empty) return;

                SelectedOuputDirectoryText.Content = value;
            }
        }

        private string qmkPath;
        private string layoutZipFile;
        private string outputPath;

        public MainWindow()
        {
            InitializeComponent();

            QmkPath = Properties.Settings.Default.QMKPath;
            LayoutZipFile = Properties.Settings.Default.LayoutZipFile;
            OutputPath = Properties.Settings.Default.OutputPath;
        }

        private void SetQmkPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                QmkPath = openFileDialog.FileName;
                Properties.Settings.Default.QMKPath = QmkPath;
            }
        }

        private void FileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                LayoutZipFile = openFileDialog.FileName;
                Properties.Settings.Default.LayoutZipFile = LayoutZipFile;
            }
        }

        private void OutputSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputPath = dialog.FileName;
                Properties.Settings.Default.OutputPath = OutputPath;
            }
        }

        private void MakeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LayoutZipFile == null) return;
            if (QmkPath == null) return;
            if (OutputPath == null) return;

            string directoryName = Path.GetFileNameWithoutExtension(LayoutZipFile);
            string fileDirectory = Path.GetDirectoryName(LayoutZipFile); 

            DirectoryInfo unzippedDirectory = Directory.CreateDirectory($"{fileDirectory}/{directoryName}");

            ZipFile.ExtractToDirectory(LayoutZipFile, unzippedDirectory.FullName);

            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            foreach (FileInfo file in keymapFile.Directory.GetFiles())
            {
                file.CopyTo($"{OutputPath}\\{file.Name}", true);
            }

            LaunchQMK();
        }

        private void LaunchQMK()
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = false,
                FileName = QmkPath,
                Arguments = $"-Dir {OutputPath}"
            };

            using Process qmkProcess = Process.Start(startInfo);
            qmkProcess.WaitForExit();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.Save(); 
        }
    }
}