using System.Diagnostics;
using System.IO;

namespace AutoLayout
{
    public static class QmkMsysManager
    {
        public static void CopyFiles(DirectoryInfo directory, string outputPath)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                file.CopyTo($"{outputPath}\\{file.Name}", true);
            }
        }

        public static void Launch(string QmkMsysPath, string QmkRepoPath)
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
    }
}