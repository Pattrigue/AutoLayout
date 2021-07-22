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
        private string QmkMsysPath
        {
            get => qmkMsysPath;
            set
            {
                qmkMsysPath = value;

                if (value == string.Empty) return;

                SelectedQmkMsysPathText.Content = value;
            }
        }
        private string QmkRepoPath
        {
            get => qmkRepoPath;
            set
            {
                qmkRepoPath = value;

                if (value == string.Empty) return;

                SelectedQmkRepoText.Content = value;
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

        private string qmkMsysPath;
        private string qmkRepoPath;
        private string layoutZipFile;
        private string outputPath;

        public MainWindow()
        {
            InitializeComponent();

            QmkMsysPath = Properties.Settings.Default.QmkMsysPath;
            QmkRepoPath = Properties.Settings.Default.QmkRepoPath;
            LayoutZipFile = Properties.Settings.Default.LayoutZipFile;
            OutputPath = Properties.Settings.Default.OutputPath;
        }

        private void SetQmkMsysPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                QmkMsysPath = openFileDialog.FileName;
                Properties.Settings.Default.QmkMsysPath = QmkMsysPath;
            }
        }

        private void SetQmkRepoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                QmkRepoPath = dialog.FileName;
                Properties.Settings.Default.QmkRepoPath = QmkRepoPath;
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
            if (QmkMsysPath == null) return;
            if (OutputPath == null) return;
            if (QmkRepoPath == null) return;

            string directoryName = Path.GetFileNameWithoutExtension(LayoutZipFile);
            string fileDirectory = Path.GetDirectoryName(LayoutZipFile);

            DirectoryInfo unzippedDirectory = Directory.CreateDirectory($"{fileDirectory}/{directoryName}");
            
            if (Directory.Exists(unzippedDirectory.FullName))
            {
                Directory.Delete(unzippedDirectory.FullName, true);
            }

            ZipFile.ExtractToDirectory(LayoutZipFile, unzippedDirectory.FullName);

            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            foreach (FileInfo file in keymapFile.Directory.GetFiles())
            {
                file.CopyTo($"{OutputPath}\\{file.Name}", true);
            }

            LaunchQmkMsys();
        }

        private void LaunchQmkMsys()
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = false,
                FileName = QmkMsysPath,
                Arguments = $"-Dir {QmkRepoPath}"
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