using Microsoft.Win32;
using System.IO;
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

        private string KeymapOutputPath
        {
            get => keymapOutputPath;
            set
            {
                keymapOutputPath = value;
                
                if (value == string.Empty) return;

                SelectedKeymapOutputText.Content = value;
            }
        }

        private string qmkMsysPath;
        private string qmkRepoPath;
        private string layoutZipFile;
        private string keymapOutputPath;

        public MainWindow()
        {
            InitializeComponent();

            QmkMsysPath = Properties.Settings.Default.QmkMsysPath;
            QmkRepoPath = Properties.Settings.Default.QmkRepoPath;
            LayoutZipFile = Properties.Settings.Default.LayoutZipFile;
            KeymapOutputPath = Properties.Settings.Default.OutputPath;
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
            CommonOpenFileDialog dialog = new()
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
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                KeymapOutputPath = dialog.FileName;
                Properties.Settings.Default.OutputPath = KeymapOutputPath;
            }
        }

        private void MakeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LayoutZipFile == null) return;
            if (QmkMsysPath == null) return;
            if (KeymapOutputPath == null) return;
            if (QmkRepoPath == null) return;

            DirectoryInfo unzippedDirectory = Unzipper.UnzipLayoutFile(LayoutZipFile);
            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            QmkMsysManager.CopyFiles(keymapFile.Directory, KeymapOutputPath);
            QmkMsysManager.Launch(QmkMsysPath, QmkRepoPath);
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.Save(); 
        }
    }
}