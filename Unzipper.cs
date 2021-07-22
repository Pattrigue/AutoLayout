using System.IO;
using System.IO.Compression;

namespace AutoLayout
{
    public static class Unzipper
    {
        public static DirectoryInfo UnzipLayoutFile(string zipFile)
        {
            string directoryName = Path.GetFileNameWithoutExtension(zipFile);
            string fileDirectory = Path.GetDirectoryName(zipFile);

            DirectoryInfo unzippedDirectory = Directory.CreateDirectory($"{fileDirectory}/{directoryName}");

            if (Directory.Exists(unzippedDirectory.FullName))
            {
                Directory.Delete(unzippedDirectory.FullName, true);
            }

            ZipFile.ExtractToDirectory(zipFile, unzippedDirectory.FullName);

            return unzippedDirectory;
        }
    }
}