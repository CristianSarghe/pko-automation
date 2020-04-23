using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKO_BOT.Business
{
    public static class ProcessUtilities
    {
        public static Process gameProcess;

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);

        public static void InitializeProcess()
        {
            gameProcess = Process.GetProcessesByName("Game").FirstOrDefault();
        }

        public static void SendKeys(string keys, int pressForSeconds)
        {
            if(gameProcess == null)
            {
                return;
            }

            IntPtr handle = gameProcess.MainWindowHandle;

            for (int i = 0; i <= pressForSeconds * 1000 / 400; ++i)
            {
                Thread.Sleep(400);
                SetForegroundWindow(handle);

                Point cursorPosition;
                GetCursorPos(out cursorPosition);

                System.Windows.Forms.SendKeys.SendWait(keys);
                SetCursorPos(cursorPosition.X, cursorPosition.Y);

                System.Windows.Forms.SendKeys.Flush();
            }
        }
    }
}
