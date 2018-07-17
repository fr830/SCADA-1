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

        public static Dictionary<EnumPSite, int> SiteIndexDict = new Dictionary<EnumPSite, int>
        {
            {EnumPSite.S1,2},{EnumPSite.S2,3},{EnumPSite.S3,4},{EnumPSite.S4,5},{EnumPSite.S5_Assemble,6},{EnumPSite.S6_Alignment,7}
        };

        public static Dictionary<EnumWorkpiece, int> WpBitDict = new Dictionary<EnumWorkpiece, int>
        {
            {EnumWorkpiece.A,4},{EnumWorkpiece.B,5},{EnumWorkpiece.C,6},{EnumWorkpiece.D,7},{EnumWorkpiece.E,8},
        };

        private Work_RFID()
        {
            foreach (var item in My.RFIDs.Where(p => p.Key != EnumPSite.S6_Alignment))
            {
                var RFID = item.Value;
                RFID.Read_IsRequested += RFID_Read_IsRequested;
                RFID.Write_Process_Success_IsRequested += RFID_Write_Process_Success_IsRequested;
                RFID.Write_Process_Failure_IsRequested += RFID_Write_Process_Failure_IsRequested;
            }
            My.RFIDs[EnumPSite.S6_Alignment].Read_IsRequested += Alignment_Read_IsRequested;
            My.RFIDs[EnumPSite.S6_Alignment].Print_QR_Code_IsRequested += Alignment_Print_QR_Code_IsRequested;
        }

        void Alignment_Print_QR_Code_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            //TODO
            My.PLC.BitSet(SiteIndexDict[item.Site], 11);
        }


        void Alignment_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            if (data == null) return;
            My.PLC.BitSet(SiteIndexDict[item.Site], 1);
            //TODO
            My.PLC.BitSet(SiteIndexDict[item.Site], 4);
            //TODO
        }

        void RFID_Read_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            if (data == null) return;
            My.PLC.BitSet(SiteIndexDict[item.Site], 1);
            if (data.GetProcessSite() == item.Site)
            {
                My.PLC.BitSet(SiteIndexDict[item.Site], 2);
            }
            else
            {
                My.PLC.BitSet(SiteIndexDict[item.Site], 3);
            }
            for (int i = 4; i < 9; i++)
            {
                My.PLC.BitClear(SiteIndexDict[item.Site], i);
            }
            My.PLC.BitSet(SiteIndexDict[item.Site], WpBitDict[data.Workpiece]);
        }

        void RFID_Write_Process_Success_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Successed);
            item.Write(data);
            My.PLC.BitSet(SiteIndexDict[item.Site], 12);
        }

        void RFID_Write_Process_Failure_IsRequested(object sender, EventArgs e)
        {
            var item = sender as RFIDReader;
            if (item == null) return;
            var data = item.Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Failed);
            item.Write(data);
            My.PLC.BitSet(SiteIndexDict[item.Site], 12);
        }



    }
}
