using System.IO;
using System.Net;

namespace AutoLayout
{
    public static class FileManager
    {
        private const string LayoutZipFileName = "MyLayout.zip";

        public static void DownloadLayout(string downloadUrl, string downloadPath, out string LayoutZipPath)
        {
            LayoutZipPath = $"{downloadPath}\\{LayoutZipFileName}";

            using WebClient client = new();
            client.DownloadFile(downloadUrl, LayoutZipPath);
        }

        public static void CopyFiles(DirectoryInfo directory, string outputPath)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                file.CopyTo($"{outputPath}\\{file.Name}", true);
            }
        }

        public static void CleanUpDownloadedFiles(string layoutZipPath, string unzippedDirectory)
        {
            if (File.Exists(layoutZipPath))
            {
                File.Delete(layoutZipPath);
            }

            if (Directory.Exists(unzippedDirectory))
            {
                Directory.Delete(unzippedDirectory, true);
            }
        }
    }
}