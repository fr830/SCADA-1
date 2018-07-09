using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    static class Win32Api
    {
        #region User32.dll

        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态  
        /// </summary>  
        /// <param name="hWnd">窗口句柄</param>  
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>  
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>  
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>  
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。  
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。   
        /// </summary>  
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>  
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 打开剪贴板
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪贴板
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool CloseClipboard();

        /// <summary>
        /// 清空剪贴板
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool EmptyClipboard();

        /// <summary>
        /// 将存放有数据的内存块放入剪切板的资源管理中
        /// </summary>
        [DllImport("User32.dll")]
        public static extern IntPtr SetClipboardData(uint Format, IntPtr hData);

        #endregion
    }
}
