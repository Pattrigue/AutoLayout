using System.Diagnostics;
using System.IO;
using System.Threading;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoLayout
{
    public static class QmkMsysManager
    {
        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);

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

            using Process qmkMsysProcess = Process.Start(startInfo);

            Thread.Sleep(1000); // maut

            qmkMsysProcess.WaitForInputIdle();
            IntPtr h = qmkMsysProcess.MainWindowHandle;
            SetForegroundWindow(h);
            SendKeys.SendWait("make moonlander:pattrigue\n");

            qmkMsysProcess.WaitForExit();
        }
    }
}