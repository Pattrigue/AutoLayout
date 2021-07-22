﻿using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace AutoLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string qmkPath;
        private string filePath;
        private string fileDirectory;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileDirectory = Path.GetDirectoryName(filePath);
                SelectedFileText.Content = Path.GetFileName(filePath);
            }
        }

        private void SetQmkPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                qmkPath = openFileDialog.FileName;
                SelectedQmkPathText.Content = qmkPath;
            }
        }

        private void MakeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LaunchQMK();
            if (filePath == null) return;
            if (qmkPath == null) return;

            string directoryName = Path.GetFileNameWithoutExtension(filePath);
            DirectoryInfo outputDirectory = Directory.CreateDirectory($"{fileDirectory}/{directoryName}");

            ZipFile.ExtractToDirectory(filePath, outputDirectory.FullName);

            FileInfo keymapFile = KeymapFileGetter.GetKeymapFile(outputDirectory);
            KeymapModifier.InsertCode(keymapFile);

        }

        private void LaunchQMK()
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = false,
                FileName = qmkPath,
                Arguments = "-cd Documents"
            };

            using Process qmkProcess = Process.Start(startInfo);
            qmkProcess.WaitForExit();
        }
    }
}