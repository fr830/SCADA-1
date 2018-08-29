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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（加工请求读）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
            My.PLC.Set(e.Index, 1);//读取成功
            logger.Info("B{0}.1:RFID读取成功（加工请求读）", e.Index);
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            if (data.Workpiece == EnumWorkpiece.D && data.IsFake)
            {
                My.PLC.Set(e.Index, 9);//D1设置为B5.9——D半成品
                logger.Info("B{0}.{1}:设置工件类型（加工请求读），物料类型{2}", e.Index, 9, "D半成品");
            }
            else
            {
                My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
                logger.Info("B{0}.{1}:设置工件类型（加工请求读），物料类型{2}", e.Index, Work_PLC.WpBitDict[data.Workpiece], Enum.GetName(typeof(EnumWorkpiece), data.Workpiece));
            }
            if (data.GetProcessSite() == e.Site)
            {
                #region GCode
                if (false)//关闭G代码下发
                {
                    var dict = GCodeFile.Dict[e.Site];
                    var mt = My.MachineTools[e.Site];
                    var file = GCodeFile.EnumFile.联调试验程序;
                    if (!data.IsFake)
                    {
                        switch (data.Workpiece)
                        {
                            case EnumWorkpiece.A:
                                file = GCodeFile.EnumFile.加工A料程序;
                                break;
                            case EnumWorkpiece.B:
                                file = GCodeFile.EnumFile.加工B料程序;
                                break;
                            case EnumWorkpiece.C:
                                file = GCodeFile.EnumFile.加工C料程序;
                                break;
                            default:
                                break;
                        }
                    }
                    if (!mt.HNC_NetFileSend(dict[file], "O999"))
                    {
                        throw new ArgumentException("G代码下发失败，请检查后重试！");
                    }
                }
                #endregion
                My.PLC.Set(e.Index, 2);//工件符合当前工位
                logger.Info("B{0}.2:工件符合当前工位（加工请求读）", e.Index);
                SimulationDS(data, e.Site);
            }
            else
            {
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
                logger.Info("B{0}.3:工件不符合当前工位（加工请求读）", e.Index);
                SimulationZD(data, e.Site);
            }
        }

        void ProcessWriteSuccess(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（加工请求写入成功）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
            data.SetProcessResult(EnumPResult.Successed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//RFID写入完成
            logger.Info("B{0}.12:RFID写入完成（加工请求写入成功）", e.Index);
        }

        void ProcessWriteFailure(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（加工请求写入失败）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
            data.SetProcessResult(EnumPResult.Failed);
            My.RFIDs[e.Site].Write(data);
            My.PLC.Set(e.Index, 12);//RFID写入完成
            logger.Info("B{0}.12:RFID写入完成（加工请求写入失败）", e.Index);
        }

        void AssembleRead(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（装配台请求读）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
            My.PLC.Set(e.Index, 1);//读取成功
            logger.Info("B{0}.1:RFID读取成功（装配台请求读）", e.Index);
            for (int i = 4; i < 9; i++)
            {
                My.PLC.Clear(e.Index, i);
            }
            My.PLC.Set(e.Index, Work_PLC.WpBitDict[data.Workpiece]);//工件类型
            logger.Info("B{0}.{1}:设置工件类型（装配台请求读），物料类型{2}", e.Index, Work_PLC.WpBitDict[data.Workpiece], Enum.GetName(typeof(EnumWorkpiece), data.Workpiece));
            if (data.IsRough || data.IsFake)
            {
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
                logger.Info("B{0}.3:工件不符合当前工位（装配台请求读）", e.Index);
                SimulationZD(data, e.Site);
            }
            else if (data.Assemble == EnumAssemble.Wanted && data.Clean == EnumClean.Wanted)
            {
                My.PLC.Set(10, 0);//装配
                My.PLC.Set(10, 1);//清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
                logger.Info("B{0}.2:装配、清洗，工件符合当前工位（装配台请求读）", e.Index);
                SimulationDS(data, e.Site);
            }
            else if (data.Assemble == EnumAssemble.Unwanted && data.Clean == EnumClean.Wanted)
            {
                My.PLC.Clear(10, 0);//不装配
                My.PLC.Set(10, 1);//清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
                logger.Info("B{0}.2:不装配、清洗，工件符合当前工位（装配台请求读）", e.Index);
                SimulationDS(data, e.Site);
            }
            else if (data.Assemble == EnumAssemble.Wanted && data.Clean == EnumClean.Unwanted)
            {
                My.PLC.Set(10, 0);//装配
                My.PLC.Clear(10, 1);//不清洗
                My.PLC.Set(e.Index, 2);//工件符合当前工位
                logger.Info("B{0}.2:装配、不清洗，工件符合当前工位（装配台请求读）", e.Index);
                SimulationDS(data, e.Site);
            }
            else
            {
                My.PLC.Clear(10, 0);//不装配
                My.PLC.Clear(10, 1);//不清洗
                My.PLC.Set(e.Index, 3);//工件不符合当前工位
                logger.Info("B{0}.3:不装配、不清洗，工件不符合当前工位（装配台请求读）", e.Index);
                SimulationZD(data, e.Site);
            }
        }

        void AssembleWriteSuccess(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（装配台请求写入装配、清洗成功）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
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
            logger.Info("B{0}.12:RFID写入完成（装配台请求写入装配、清洗成功）", e.Index);
        }

        void AssembleWriteFailure(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（装配台请求写入装配、清洗失败）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
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
            logger.Info("B{0}.12:RFID写入完成（装配台请求写入装配、清洗失败）", e.Index);
        }

        void AlignmentRead(object sender, PLCEventArgs e)
        {
            var data = My.RFIDs[e.Site].Read();
            if (data == null)
            {
                logger.Warn("{0}:RFID读取失败（定位台请求读）", Enum.GetName(typeof(EnumPSite), e.Site));
                return;
            }
            My.PLC.Set(e.Index, 1);//读取成功
            logger.Info("B{0}.1:RFID读取成功（定位台请求读）", e.Index);
            if (data.Assemble == EnumAssemble.Wanted || data.IsRough || data.IsFake)
            {
                My.PLC.Set(e.Index, 4);//非入库料盘
                logger.Info("B{0}.4:非入库料盘（定位台请求读）", e.Index);
                SimulationZD(data, e.Site);
            }
            else
            {
                My.PLC.Set(e.Index, data.Workpiece == EnumWorkpiece.E ? 3 : 2);//入库料盘
                logger.Info("B{0}.{1}:入库料盘（定位台请求读），物料类型{2}", e.Index, data.Workpiece == EnumWorkpiece.E ? 3 : 2, Enum.GetName(typeof(EnumWorkpiece), data.Workpiece));
                SimulationDS(data, e.Site);
            }
        }

        /// <summary>
        /// 顶升
        /// </summary>
        /// <param name="data"></param>
        /// <param name="site"></param>
        private void SimulationDS(RFIDData data, EnumPSite site)
        {
            switch (site)
            {
                case EnumPSite.S1:
                    My.Work_Simulation.Send(new DSJ01(data, DSJ01.EnumActionType.物料顶升));
                    break;
                case EnumPSite.S2:
                    My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.物料顶升));
                    break;
                case EnumPSite.S3:
                    My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.物料顶升));
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
        }

        /// <summary>
        /// 阻挡
        /// </summary>
        /// <param name="data"></param>
        /// <param name="site"></param>
        private void SimulationZD(RFIDData data, EnumPSite site)
        {
            switch (site)
            {
                case EnumPSite.S1:
                    My.Work_Simulation.Send(new DSJ01(data, DSJ01.EnumActionType.正阻挡位转移物料至顶升机2前阻挡位));
                    My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.前阻挡位到位));
                    break;
                case EnumPSite.S2:
                    My.Work_Simulation.Send(new DSJ02(data, DSJ02.EnumActionType.正阻挡位转移物料至顶升机3前阻挡位));
                    My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.前阻挡位到位));
                    break;
                case EnumPSite.S3:
                    My.Work_Simulation.Send(new DSJ03(data, DSJ03.EnumActionType.正阻挡位转移物料至顶升机4前阻挡位));
                    My.Work_Simulation.Send(new DSJ04(data, DSJ04.EnumActionType.前阻挡位到位));
                    break;
                case EnumPSite.S4:
                    My.Work_Simulation.Send(new DSJ04(data, DSJ04.EnumActionType.正阻挡位转移物料至顶升机5前阻挡位));
                    My.Work_Simulation.Send(new DSJ05(data, DSJ05.EnumActionType.前阻挡位到位));
                    break;
                case EnumPSite.S5_Assemble:
                    My.Work_Simulation.Send(new DSJ05(data, DSJ05.EnumActionType.正阻挡位转移物料至下料位前阻挡位));
                    My.Work_Simulation.Send(new XLW(data, XLW.EnumActionType.前阻挡位到位));
                    break;
                case EnumPSite.S6_Alignment:
                    My.Work_Simulation.Send(new XLW(data, XLW.EnumActionType.正阻挡位转移物料至升降机2));
                    break;
                default:
                    break;
            }
        }


    }
}
