using System;
using System.Windows.Forms;

namespace SCADA
{
    public static class ControlExtensions
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static TResult InvokeEx<TControl, TResult>(this TControl control, Func<TControl, TResult> func) where TControl : Control
        {
            try
            {
                return control.InvokeRequired ? (TResult)control.Invoke(func, control) : func(control);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return default(TResult);
            }
        }

        public static void InvokeEx<TControl>(this TControl control, Action<TControl> action) where TControl : Control
        {
            control.InvokeEx(c => { action(c); return c; });
        }

        public static void InvokeEx<TControl>(this TControl control, Action action) where TControl : Control
        {
            control.InvokeEx(c => action());
        }
    }
}
