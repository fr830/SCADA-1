using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SCADA
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Process p = GetRunningInstance();
            if (p != null)
            {
                Win32Api.ShowWindowAsync(p.MainWindowHandle, 1);
                Win32Api.SetForegroundWindow(p.MainWindowHandle);
            }
            else
            {
                Application.Run(new _Layout());
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "线程异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Environment.Exit(2);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception).Message, "未处理的异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Environment.Exit(1);
        }

        /// <summary>
        /// 获取正在运行的实例，没有运行的实例返回null
        /// </summary>
        public static System.Diagnostics.Process GetRunningInstance()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "//") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
    }
}
