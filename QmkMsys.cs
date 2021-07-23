using System.Diagnostics;
using System.IO;
using System.Threading;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoLayout
{
    public static class QmkMsys
    {
        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);

        public static void Launch(string qmkMsysPath, string qmkRepoPath, string command)
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = false,
                FileName = qmkMsysPath,
                Arguments = $"-Dir {qmkRepoPath}"
            };

            using Process qmkMsysProcess = Process.Start(startInfo);

            Thread.Sleep(1000); // we partake in a little tomfoolery

            qmkMsysProcess.WaitForInputIdle();
            IntPtr h = qmkMsysProcess.MainWindowHandle;
            SetForegroundWindow(h);
            SendKeys.SendWait($"{command}\n");

            qmkMsysProcess.WaitForExit();
        }
    }
}