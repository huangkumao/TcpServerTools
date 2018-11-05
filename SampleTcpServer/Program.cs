using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SampleTcpServer
{
    internal static class Program
    {
        // 启动控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool AllocConsole();

        // 释放控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeConsole();

        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}