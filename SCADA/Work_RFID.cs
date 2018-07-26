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
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
            if (data.GetProcessSite() == e.Site)
            {
                switch (e.Site)
                {
                    case EnumPSite.S1:
                        My.Work_Simulation.Send(new DSJ01(data, DSJ01.EnumActionType.物料顶升));
                        switch (data.Workpiece)
                        {
                            case EnumWorkpiece.A:
                                My.MachineTools[e.Site].HNC_NetFileSend(ProcessProgram.dictS1[ProcessProgram.EnumS1File.加工A料程序], "O9901");
                                break;
                            case EnumWorkpiece.B:
                                My.PLC.HNC_NetFileSend(ProcessProgram.dictS1[ProcessProgram.EnumS1File.加工B料程序], "O9902");
                                break;
                            case EnumWorkpiece.C:
                                My.PLC.HNC_NetFileSend(ProcessProgram.dictS1[ProcessProgram.EnumS1File.加工C料程序], "O9903");
                                break;
                            default:
                                break;
                        }
                        break;
                    case EnumPSite.S2:
                        My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.物料顶升));
                        break;
                    case EnumPSite.S3:
                        My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.物料顶升));
                        switch (data.Workpiece)
                        {
                            case EnumWorkpiece.A:
                                My.PLC.HNC_NetFileSend(ProcessProgram.dictS3[ProcessProgram.EnumS3File.加工A料程序], "O9901");
                                break;
                            case EnumWorkpiece.B:
                                My.PLC.HNC_NetFileSend(ProcessProgram.dictS3[ProcessProgram.EnumS3File.加工B料程序], "O9902");
                                break;
                            case EnumWorkpiece.C:
                                My.PLC.HNC_NetFileSend(ProcessProgram.dictS3[ProcessProgram.EnumS3File.加工C料程序], "O9903");
                                break;
                            default:
                                break;
                        }
                        break;
                    case EnumPSite.S4:
                        My.Work_Simulation.Send(new DSJ04(data, DSJ04.EnumActionType.物料顶升));
                        break;
                    case EnumPSite.S5_Assemble:
                        My.Work_Simulation.Send(new DSJ05(data, DSJ05.EnumActionType.物料顶升));
                        break;
                    case EnumPSite.S6_Alignment:
                        My.Work_Simulation.Send(new XLW(data, XLW.EnumActionType.物料顶升));
                        break;
                    default:
                        break;
                }
                My.PLC.Set(e.Index, 2);//工件符合当前工位
            }
            else
            {
                switch (e.Site)
                {
                    case EnumPSite.S1:
                        My.Work_Simulation.Send(new DSJ01(data, DSJ01.EnumActionType.正阻挡位转移物料至顶升机2前阻挡位));
                        break;
                    case EnumPSite.S2:
                        My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.正阻挡位转移物料至顶升机3前阻挡位));
                        break;
                    case EnumPSite.S3:
                        My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.正阻挡位转移物料至顶升机4前阻挡位));
                        break;
                    case EnumPSite.S4:
                        My.Work_Simulation.Send(new DSJ04(data, DSJ04.EnumActionType.正阻挡位转移物料至顶升机5前阻挡位));
                        break;
                    case EnumPSite.S5_Assemble:
                        My.Work_Simulation.Send(new DSJ05(data, DSJ05.EnumActionType.正阻挡位转移物料至下料位前阻挡位));
                        break;
                    case EnumPSite.S6_Alignment:
                        My.Work_Simulation.Send(new XLW(data, XLW.EnumActionType.正阻挡位转移物料至升降机2));
                        break;
                    default:
                        break;
                }
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
            }
        }

        void ProcessWriteSuccess(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null) return;
            data.SetProcessResult(EnumPResult.Successed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//写入完成
            switch (e.Site)
            {
                case EnumPSite.S1:
                    My.Work_Simulation.Send(new DSJ01(data, DSJ01.EnumActionType.物料下降));
                    break;
                case EnumPSite.S2:
                    My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.物料下降));
                    break;
                case EnumPSite.S3:
                    My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.物料下降));
                    break;
                case EnumPSite.S4:
                    My.Work_Simulation.Send(new DSJ04(data, DSJ04.EnumActionType.物料下降));
                    break;
                case EnumPSite.S5_Assemble:
                    My.Work_Simulation.Send(new DSJ05(data, DSJ05.EnumActionType.物料下降));
                    break;
                case EnumPSite.S6_Alignment:
                    My.Work_Simulation.Send(new XLW(data, XLW.EnumActionType.物料下降));
                    break;
                default:
                    break;
            }
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
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
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
            else
            {
                My.PLC.Set(e.Index, data.Workpiece == EnumWorkpiece.E ? 3 : 2);//入库料盘
                My.Work_Simulation.Send(new DWT04(data));
                My.Work_Simulation.Send(new AGV(data, AGV.EnumActionType.AGV从定位台4运动至定位台2));
            }
        }


    }
}
