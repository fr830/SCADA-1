using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace SCADA
{
    static class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            bool createdNew;
            Mutex mutex = new Mutex(true, Application.ProductName, out createdNew);
            if (createdNew)
            {
                Application.Run(new _Layout());
            }
            else
            {
                Process p = GetRunningInstance();
                if (p != null)
                {
                    Win32Api.ShowWindowAsync(p.MainWindowHandle, 1);
                    Win32Api.SetForegroundWindow(p.MainWindowHandle);
                }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            logger.Fatal(e.Exception);
            MessageBox.Show(e.Exception.ToString(), "线程异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
            Environment.Exit(2);
#endif
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Fatal(e.ExceptionObject);
            MessageBox.Show((e.ExceptionObject as Exception).ToString(), "未处理的异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
            Environment.Exit(1);
#endif
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
