using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_QRCode
    {
        private static readonly Lazy<Work_QRCode> lazy = new Lazy<Work_QRCode>(() => new Work_QRCode());

        public static Work_QRCode Instance { get { return lazy.Value; } }

        private Work_QRCode()
        {
            My.Work_PLC.Scan += Scan;
        }

        void Scan(object sender, PLCEventArgs e)
        {
            My.PLC.Set(1, 12);//扫码器扫码完成
        }
    }
}
