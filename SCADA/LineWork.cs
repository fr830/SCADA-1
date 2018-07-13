using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFID;
using HNC;

namespace SCADA
{
    class LineWork
    {
        private static readonly Lazy<LineWork> lazy = new Lazy<LineWork>(() => new LineWork());

        public static LineWork Instance { get { return lazy.Value; } }

        private MachineTool PLC = My.CoreService.PLC;
        private RFIDItem RFID7 = My.CoreService.RFIDs[7];

        private LineWork()
        {
            for (int i = 2; i < My.CoreService.RFIDs.Count - 1; i++)
            {
                My.CoreService.RFIDs[i].Read_IsRequested += LineWork_Read_IsRequested;
                My.CoreService.RFIDs[i].Write_Process_Success_IsRequested += LineWork_Write_Process_Success_IsRequested;
                My.CoreService.RFIDs[i].Write_Process_Failure_IsRequested += LineWork_Write_Process_Failure_IsRequested;
            }
        }

        void LineWork_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            if (item == null) return;
            var data = item.Read();
            PLC.BitSet(item.Index, 1);
            if (RFIDData.GetProcess(data) == item.Index - 1)
            {
                PLC.BitSet(item.Index, 2);
            }
            else
            {
                PLC.BitSet(item.Index, 3);
            }
            for (int i = 4; i < 9; i++)
            {
                PLC.BitClear(item.Index, i);
            }
            PLC.BitSet(item.Index, (int)RFIDData.GetWorkpiece(data) + 3);
        }

        void LineWork_Write_Process_Success_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            if (item == null) return;
            var data = item.Read();
            RFIDData.SetProcessResult(data, RFIDData.EnumProcessResult.Successed);
            item.Write(data);
            PLC.BitSet(item.Index, 12);
        }

        void LineWork_Write_Process_Failure_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            if (item == null) return;
            var data = item.Read();
            RFIDData.SetProcessResult(data, RFIDData.EnumProcessResult.Failed);
            item.Write(data);
            PLC.BitSet(item.Index, 12);
        }



    }
}
