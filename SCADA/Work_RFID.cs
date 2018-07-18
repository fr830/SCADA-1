using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFID;
using HNC;

namespace SCADA
{
    class Work_RFID
    {
        private static readonly Lazy<Work_RFID> lazy = new Lazy<Work_RFID>(() => new Work_RFID());

        public static Work_RFID Instance { get { return lazy.Value; } }

        private Work_RFID()
        {
            My.Work_PLC.Read_IsRequested += Read_IsRequested;
            My.Work_PLC.Write_Process_Success_IsRequested += Write_Process_Success_IsRequested;
            My.Work_PLC.Write_Process_Failure_IsRequested += Write_Process_Failure_IsRequested;
            My.Work_PLC.Entry_IsRequested += Entry_IsRequested;
        }

        void Read_IsRequested(object sender, RFIDEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            My.PLC.BitSet(e.Index, 1);
            if (data.GetProcessSite() == e.Site)
            {
                My.PLC.BitSet(e.Index, 2);
            }
            else
            {
                My.PLC.BitSet(e.Index, 3);
            }
            for (int i = 4; i < 9; i++)
            {
                My.PLC.BitClear(e.Index, i);
            }
            My.PLC.BitSet(e.Index, Work_PLC.WpBitDict[data.Workpiece]);
        }

        void Write_Process_Success_IsRequested(object sender, RFIDEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Successed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.BitSet(e.Index, 12);
        }

        void Write_Process_Failure_IsRequested(object sender, RFIDEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Failed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.BitSet(e.Index, 12);
        }

        void Entry_IsRequested(object sender, RFIDEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            My.PLC.BitSet(e.Index, 1);
            if (data.GetProcessSite() != EnumPSite.None)
            {
                My.PLC.BitSet(e.Index, 4);//非入库料盘
            }
            else if (data.Assemble == EnumAssemble.Wanted)
            {
                My.PLC.BitSet(e.Index, 4);//非入库料盘
            }
            else if (data.Assemble == EnumAssemble.Unwanted)
            {
                My.PLC.BitSet(e.Index, 5);//入库料盘_清洗模式
            }
            else if (data.Assemble == EnumAssemble.Successed || data.Assemble == EnumAssemble.Failed)
            {
                if (data.Workpiece == EnumWorkpiece.E)
                {
                    My.PLC.BitSet(e.Index, 3);//入库装配体_装配模式
                }
                else
                {
                    My.PLC.BitSet(e.Index, 2);//入库空盘_装配模式
                }
            }
        }


    }
}
