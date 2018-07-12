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
        private RFIDItem RFID2 = My.CoreService.RFIDs[2];
        private RFIDItem RFID3 = My.CoreService.RFIDs[3];
        private RFIDItem RFID4 = My.CoreService.RFIDs[4];
        private RFIDItem RFID5 = My.CoreService.RFIDs[5];
        private RFIDItem RFID6 = My.CoreService.RFIDs[6];
        private RFIDItem RFID7 = My.CoreService.RFIDs[7];

        private LineWork()
        {
            RFID2.Read_IsRequested += RFID2_Read_IsRequested;
            RFID2.Write_Process_Success_IsRequested += RFID2_Write_Process_Success_IsRequested;
            RFID2.Write_Process_Failure_IsRequested += RFID2_Write_Process_Failure_IsRequested;
            RFID4.Read_IsRequested += RFID4_Read_IsRequested;
            RFID4.Write_Process_Success_IsRequested += RFID4_Write_Process_Success_IsRequested;
            RFID4.Write_Process_Failure_IsRequested += RFID4_Write_Process_Failure_IsRequested;
        }

        void RFID2_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            var data = item.Read();
        }

        void RFID2_Write_Process_Success_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            item.Write(Enumerable.Repeat<byte>(0x10, 32).ToArray());
        }

        void RFID2_Write_Process_Failure_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            item.Write(Enumerable.Repeat<byte>(0x20, 32).ToArray());
        }

        void RFID4_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            var data = item.Read();
        }

        void RFID4_Write_Process_Success_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            item.Write(Enumerable.Repeat<byte>(0x10, 32).ToArray());
        }

        void RFID4_Write_Process_Failure_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDItem;
            item.Write(Enumerable.Repeat<byte>(0x20, 32).ToArray());
        }


    }
}
