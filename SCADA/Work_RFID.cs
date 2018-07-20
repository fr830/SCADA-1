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
            My.Work_PLC.ProcessRead += ProcessRead;
            My.Work_PLC.ProcessWriteSuccess += ProcessWriteSuccess;
            My.Work_PLC.ProcessWriteFailure += ProcessWriteFailure;
            My.Work_PLC.AssembleRead += AssembleRead;
            My.Work_PLC.AssembleWriteSuccess += AssembleWriteSuccess;
            My.Work_PLC.AssembleWriteFailure += AssembleWriteFailure;
            My.Work_PLC.AlignmentRead += AlignmentRead;
        }

        void ProcessRead(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            My.PLC.Set(e.Index, 1);//读取成功
            if (data.GetProcessSite() == e.Site)
            {
                My.PLC.Set(e.Index, 2);//工件符合当前工位
            }
            else
            {
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
            }
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
        }

        void ProcessWriteSuccess(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Successed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//写入完成
        }

        void ProcessWriteFailure(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Failed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//写入完成
        }

        void AssembleRead(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            My.PLC.Set(e.Index, 1);//读取成功
            if (data.IsRough)
            {
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
            }
            else if (data.Assemble == EnumAssemble.Wanted && data.Clean == EnumClean.Wanted)
            {
                My.PLC.Set(10, 0);//装配
                My.PLC.Set(10, 1);//清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
            }
            else if (data.Assemble == EnumAssemble.Unwanted && data.Clean == EnumClean.Wanted)
            {
                My.PLC.Clear(10, 0);//不装配
                My.PLC.Set(10, 1);//清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
            }
            else if (data.Assemble == EnumAssemble.Wanted && data.Clean == EnumClean.Unwanted)
            {
                My.PLC.Set(10, 0);//装配
                My.PLC.Clear(10, 1);//不清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
            }
            else
            {
                My.PLC.Clear(10, 0);//不装配
                My.PLC.Clear(10, 1);//不清洗
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
            }
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
        }

        void AssembleWriteSuccess(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            if (data.Assemble == EnumAssemble.Wanted)
            {
                data.Assemble = EnumAssemble.Successed;
            }
            if (data.Clean == EnumClean.Wanted)
            {
                data.Clean = EnumClean.Successed;
            }
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//写入完成
        }

        void AssembleWriteFailure(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            if (data.Assemble == EnumAssemble.Wanted)
            {
                data.Assemble = EnumAssemble.Failed;
            }
            if (data.Clean == EnumClean.Wanted)
            {
                data.Clean = EnumClean.Failed;
            }
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//写入完成
        }

        void AlignmentRead(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            My.PLC.Set(e.Index, 1);//读取成功
            if (data.IsRough)
            {
                My.PLC.Set(e.Index, 4);//非入库料盘
            }
            else if (data.Assemble == EnumAssemble.Wanted)
            {
                My.PLC.Set(e.Index, 4);//非入库料盘
            }
            else if (data.Workpiece == EnumWorkpiece.E)
            {
                My.PLC.Set(e.Index, 3);//入库装配体
            }
            else
            {
                My.PLC.Set(e.Index, 2);//入库料盘
            }
        }


    }
}
