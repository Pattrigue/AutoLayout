using System.Diagnostics;
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
            Process qmkMsysProcess = CreateQmkProcess(qmkMsysPath, qmkRepoPath);

            Thread.Sleep(1000); // we partake in a little tomfoolery

            qmkMsysProcess.WaitForInputIdle();
            IntPtr h = qmkMsysProcess.MainWindowHandle;
            SetForegroundWindow(h);
            SendKeys.SendWait($"{command}\n");

            qmkMsysProcess.WaitForExit();
        }

        private static Process CreateQmkProcess(string qmkMsysPath, string qmkRepoPath)
        {
            Process qmkMsysProcess;

            Process[] existingProcess = Process.GetProcessesByName("ConEmu64");

            if (existingProcess.Length > 0)
            {
                qmkMsysProcess = existingProcess[0];
            }
            else
            {
                ProcessStartInfo startInfo = new()
                {
                    UseShellExecute = false,
                    FileName = qmkMsysPath,
                    Arguments = $"-Dir {qmkRepoPath}"
                };

                qmkMsysProcess = Process.Start(startInfo);
            }

            return qmkMsysProcess;
        }
    }
}