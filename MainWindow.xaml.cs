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
        private AppSetting qmkMsys;
        private AppSetting qmkRepo;
        private AppSetting keymapOutput;
        private AppSetting layoutZipFile;

        public MainWindow()
        {
            InitializeComponent();

            qmkMsys = new AppSetting(SelectedQmkMsysPathText, "QmkMsysPath", Properties.Settings.Default.QmkMsysPath);
            qmkRepo = new AppSetting(SelectedQmkRepoText, "QmkRepoPath", Properties.Settings.Default.QmkRepoPath);
            layoutZipFile = new AppSetting(SelectedZipFileText, "LayoutZipFile", Properties.Settings.Default.LayoutZipFile);
            keymapOutput = new AppSetting(SelectedKeymapOutputText, "OutputPath", Properties.Settings.Default.OutputPath);
        }

        private void SetQmkMsysPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                qmkMsys.Value = openFileDialog.FileName;
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
                qmkRepo.Value = dialog.FileName;
            }
        }

        private void FileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                layoutZipFile.Value = openFileDialog.FileName;
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
                keymapOutput.Value = dialog.FileName;
            }
        }

        private void MakeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (layoutZipFile.Value == null) return;
            if (qmkMsys.Value == null) return;
            if (keymapOutput.Value == null) return;
            if (qmkRepo.Value == null) return;

            DirectoryInfo unzippedDirectory = Unzipper.UnzipLayoutFile(layoutZipFile.Value);
            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            QmkMsysManager.CopyFiles(keymapFile.Directory, keymapOutput.Value);
            QmkMsysManager.Launch(qmkMsys.Value, qmkRepo.Value);
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.Save(); 
        }
    }
}