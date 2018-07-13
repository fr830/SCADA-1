using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFID;
using HNC;

namespace SCADA
{
    class Work_Line
    {
        private static readonly Lazy<Work_Line> lazy = new Lazy<Work_Line>(() => new Work_Line());

        public static Work_Line Instance { get { return lazy.Value; } }
        
        private Work_Line()
        {
            for (int i = 2; i < My.RFIDs.Count - 1; i++)
            {
                My.RFIDs[i].Read_IsRequested += LineWork_Read_IsRequested;
                My.RFIDs[i].Write_Process_Success_IsRequested += LineWork_Write_Process_Success_IsRequested;
                My.RFIDs[i].Write_Process_Failure_IsRequested += LineWork_Write_Process_Failure_IsRequested;
            }
            My.RFIDs[7].Read_IsRequested += Entry_Read_IsRequested;
        }

        void Entry_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            My.PLC.BitSet(item.Index, 1);
        }

        void LineWork_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            My.PLC.BitSet(item.Index, 1);
            if (data.GetProcessSite() == item.Index - 1)
            {
                My.PLC.BitSet(item.Index, 2);
            }
            else
            {
                My.PLC.BitSet(item.Index, 3);
            }
            for (int i = 4; i < 9; i++)
            {
                My.PLC.BitClear(item.Index, i);
            }
            My.PLC.BitSet(item.Index, (int)data.Workpiece + 3);
        }

        void LineWork_Write_Process_Success_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            data.SetProcessResult(EnumProcessResult.Successed);
            item.Write(data);
            My.PLC.BitSet(item.Index, 12);
        }

        void LineWork_Write_Process_Failure_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            data.SetProcessResult(EnumProcessResult.Failed);
            item.Write(data);
            My.PLC.BitSet(item.Index, 12);
        }



    }
}
