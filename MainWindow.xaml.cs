using Microsoft.Win32;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Net;

namespace AutoLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string LayoutZipFileName = "MyLayout.zip";

        private string LayoutZipPath => $"{downloadPath.Value}\\{LayoutZipFileName}";

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

            DownloadLayout();

            DirectoryInfo unzippedDirectory = Unzipper.UnzipLayoutFile(LayoutZipPath);
            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(unzippedDirectory);
            KeymapModifier.InsertCode(keymapFile);

            QmkMsysManager.CopyFiles(keymapFile.Directory, keymapOutput.Value);
            QmkMsysManager.Launch(qmkMsys.Value, qmkRepo.Value, CommandTextBox.Text);
        }

        private void DownloadLayout()
        {
            string downloadUrl = DownloadTextBox.Text;

            using WebClient client = new();
            client.DownloadFile(downloadUrl, LayoutZipPath);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            command.Value = CommandTextBox.Text;
            Properties.Settings.Default.Save(); 
        }
    }
}