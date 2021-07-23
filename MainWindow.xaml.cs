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
        private readonly AppSetting qmkMsys;
        private readonly AppSetting qmkRepo;
        private readonly AppSetting keymapOutput;
        private readonly AppSetting downloadPath;
        private readonly AppSetting command;

        public MainWindow()
        {
            InitializeComponent();

            qmkMsys = new AppSetting(SelectedQmkMsysPathText, "QmkMsysPath", Properties.Settings.Default.QmkMsysPath);
            qmkRepo = new AppSetting(SelectedQmkRepoText, "QmkRepoPath", Properties.Settings.Default.QmkRepoPath);
            downloadPath = new AppSetting(SelectedDownloadPathText, "DownloadPath", Properties.Settings.Default.DownloadPath);
            keymapOutput = new AppSetting(SelectedKeymapOutputText, "OutputPath", Properties.Settings.Default.OutputPath);
            command = new AppSetting(CommandTextBox, "Command", Properties.Settings.Default.Command);
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
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                downloadPath.Value = dialog.FileName;
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
            if (downloadPath.Value == null) return;
            if (qmkMsys.Value == null) return;
            if (keymapOutput.Value == null) return;
            if (qmkRepo.Value == null) return;

            FileManager.DownloadLayout(DownloadTextBox.Text, downloadPath.Value, out string layoutZipPath);

            DirectoryInfo unzippedDirectory = Unzipper.UnzipLayoutFile(layoutZipPath);
            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            FileManager.CopyFiles(keymapFile.Directory, keymapOutput.Value);
            FileManager.CleanUpDownloadedFiles(layoutZipPath, unzippedDirectory.FullName);

            QmkMsysManager.Launch(qmkMsys.Value, qmkRepo.Value, CommandTextBox.Text);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            command.Value = CommandTextBox.Text;
            Properties.Settings.Default.Save(); 
        }
    }
}