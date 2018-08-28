using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using HNC.MES.Common;
using HNC.MES.Model;
using RFID;

namespace SCADA
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ScadaService : IScadaService
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 测试服务状态
        /// </summary>
        /// <returns></returns>
        public string TestService()
        {
            return SvResult.IsRunning;
        }

        private IDictionary<string, object> ParseQueryString(Stream stream)
        {
            var str = string.Empty;
            using (StreamReader sr = new StreamReader(stream))
            {
                str = sr.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
        }

        /// <summary>
        /// 初始化RFID信息
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string InitRFID(Stream stream)
        {
            logger.Info("InitRFID:请求初始化RFID信息");
            var dict = ParseQueryString(stream);
            if (!dict.ContainsKey("type"))
            {
                logger.Warn("InitRFID:请求参数错误");
                return SvResult.ParameterError;
            }
            var type = dict["type"] as string;
            RFID.EnumWorkpiece workpiece;
            if (!Enum.TryParse<RFID.EnumWorkpiece>(type, true, out workpiece))
            {
                logger.Warn("InitRFID:解析参数错误");
                return SvResult.AnalyticError;
            }
            else
            {
                var pc = new TWorkpieceProcess();
                pc.State = "启动"; //EnumHelper.GetName(TWorkpieceProcess.EnumState.启动);
                pc.LocationID = My.LocationID;
                pc.ManufactureID = My.ManufactureID;
                pc.WorkpieceID = My.WorkpieceIDs[workpiece];
                My.BLL.TWorkpieceProcess.Insert(pc, My.AdminID);
                var guid = new Guid(pc.ID);
                if (My.RFIDs[EnumPSite.S9_Manual].Init(guid, workpiece))
                {
                    logger.Info("InitRFID:初始化RFID信息完成");
                    return SvResult.OK;
                }
                else
                {
                    My.BLL.TWorkpieceProcess.DeleteReal(pc);
                    logger.Warn("InitRFID:RFID信息写入失败");
                    return SvResult.RFIDWriteFail;
                }
            }
        }

        /// <summary>
        /// 请求读取RFID信息
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string Read(Stream stream)
        {
            logger.Info("Read:请求读取RFID信息");
            var dict = ParseQueryString(stream);
            if (!dict.ContainsKey("site"))
            {
                logger.Warn("Read:请求参数错误");
                return SvResult.ParameterError;
            }
            int site = 0;
            if (!int.TryParse(dict["site"].ToString(), out site))
            {
                logger.Warn("Read:解析参数错误");
                return SvResult.AnalyticError;
            }
            else
            {
                var PSite = site == 1 ? EnumPSite.S7_Up : EnumPSite.S8_Down;
                if (PSite == EnumPSite.S7_Up)
                {
                    return RFID_In();
                }
                else if (PSite == EnumPSite.S8_Down)
                {
                    return RFID_Out();
                }
                else
                {
                    return SvResult.Fail;
                }
            }
        }

        /// <summary>
        /// 自动入库口允许入库
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string PermitIn(Stream stream)
        {
            logger.Info("PermitIn:允许入库");
            var dict = ParseQueryString(stream);
            if (!dict.ContainsKey("site"))
            {
                logger.Warn("PermitIn:请求参数错误");
                return SvResult.ParameterError;
            }
            int site = 0;
            if (!int.TryParse(dict["site"].ToString(), out site))
            {
                logger.Warn("PermitIn:解析参数错误");
                return SvResult.AnalyticError;
            }
            else
            {
                if (site == 1)
                {
                    My.PLC.Set(11, 3);
                    logger.Info("设置B11.3:允许入库");
                    return SvResult.OK;
                }
                else
                {
                    return SvResult.Fail;
                }
            }
        }

        private TOrder GetExecOrder()
        {
            return My.BLL.TOrder.GetModel(Tool.CreateDict("State", "执行")); //EnumHelper.GetName(TOrder.EnumState.执行)));
        }

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <returns></returns>
        public string RFID_In()
        {
            logger.Info("RFID_In:入库处RFID读取");
            var data = My.RFIDs[EnumPSite.S7_Up].Read();
            if (data == null)
            {
                logger.Warn("RFID_In:RFID信息读取失败");
                return SvResult.RFIDReadFail;
            }
            try
            {
                #region 修改订单明细，增加完成数量
                var pc = My.BLL.TWorkpieceProcess.GetModel(Tool.CreateDict("ID", data.Guid.ToString()));
                pc.State = "完成"; //EnumHelper.GetName(TWorkpieceProcess.EnumState.完成);
                My.BLL.TWorkpieceProcess.Update(pc, My.AdminID);
                if (data.IsRough)
                {
                    throw new Exception("当前入库的工件未完成所有加工工序");
                }
                var order = GetExecOrder();
                if (order == null)
                {
                    logger.Warn("RFID_In:未查询到相关订单");
                    return SvResult.OrderNullError;
                }
                foreach (var detail in My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID)))
                {
                    if (My.WorkpieceIDs[data.Workpiece] == detail.WorkpieceID
                        && detail.QuantityCompletion < detail.QuantityDemanded)
                    {
                        detail.EndTime = DateTime.Now;
                        detail.QuantityCompletion++;
                        My.BLL.TOrderDetail.Update(detail, My.AdminID);
                        break;
                    }
                }
                #endregion
                #region 检查订单是否已完成
                var allFinish = true;
                foreach (var detail in My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID)))
                {
                    if (detail.QuantityCompletion < detail.QuantityDemanded)
                    {
                        allFinish = false;
                        break;
                    }
                }
                if (allFinish)
                {
                    order.State = "完成"; //EnumHelper.GetName(TOrder.EnumState.完成);
                    order.EndTime = DateTime.Now;
                    My.BLL.TOrder.Update(order, My.AdminID);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            Task.Run(() =>
            {
                //毛坯的name为A，半成品的name为A1
                var name = Enum.GetName(typeof(EnumWorkpiece), data.Workpiece) + (data.IsRough ? "" : "1");
                var wmsData = new WMSData(name, data.Assemble == EnumAssemble.Unwanted ? 1 : 0, data.Guid.ToString());
                if (data.Workpiece == EnumWorkpiece.E)
                {
                    wmsData.quantity = data.Assemble == EnumAssemble.Successed ? 1 : 0;
                }
                logger.Info("请求入库:" + wmsData.ToString());
                My.Work_WMS.In(wmsData);
                My.Work_Simulation.Send(new RKX(data, RKX.EnumActionType.入库检测位转移物料至入库位));
            });
            logger.Info("RFID_In:入库处RFID读取完成");
            return SvResult.OK;
        }

        /// <summary>
        /// 自动出库
        /// </summary>
        /// <returns></returns>
        public string RFID_Out()
        {
            logger.Info("RFID_Out:出库处RFID读取");
            var data = My.RFIDs[EnumPSite.S8_Down].Read();
            if (data == null)
            {
                logger.Warn("RFID_Out:RFID信息读取失败");
                return SvResult.RFIDReadFail;
            }
            else
            {
                My.Work_Vision.DataQueue.Enqueue(data);
            }
            try
            {
                #region 半成品修改为演示模式
                data.SetFake();
                #endregion
                #region 根据订单内容，修改标签数据，是否装配
                var order = GetExecOrder();
                if (order == null)
                {
                    logger.Warn("RFID_Out:未查询到相关订单");
                    return SvResult.OrderNullError;
                }
                foreach (var detail in My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID)))
                {
                    if (My.BLL.GetWorkpieceNameByWorkpieceID(detail.WorkpieceID) == "E")//EnumHelper.GetName(EnumWorkpiece.E))
                    {
                        if (data.Assemble == EnumAssemble.Unwanted)
                        {
                            data.Assemble = EnumAssemble.Wanted;
                            break;
                        }
                    }
                    else
                    {
                        if (data.Assemble == EnumAssemble.Wanted)
                        {
                            data.Assemble = EnumAssemble.Unwanted;
                            break;
                        }
                    }
                }
                #endregion
                #region 写入信息
                if (!My.RFIDs[EnumPSite.S8_Down].Write(data))
                {
                    logger.Warn("RFID_Out:RFID信息写入失败");
                    return SvResult.RFIDWriteFail;
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            My.PLC.SetForce(11, 0);
            logger.Info("设置B11.0:向PLC请求出库");
            My.Work_Simulation.Send(new CKX(data, CKX.EnumActionType.出库检测位转移物料至定位台1));
            logger.Info("RFID_Out:出库处RFID读取完成");
            return SvResult.OK;
        }
    }

    public class SvResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public SvResult(string message = "", bool success = false)
        {
            Success = success;
            Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string OK
        {
            get
            {
                return new SvResult("成功", true).ToString();
            }
        }

        public static string Fail
        {
            get
            {
                return new SvResult("失败", false).ToString();
            }
        }

        public static string IsRunning
        {
            get
            {
                return new SvResult("服务正在运行", true).ToString();
            }
        }

        public static string ParameterError
        {
            get
            {
                return new SvResult("请求参数错误！").ToString();
            }
        }

        public static string AnalyticError
        {
            get
            {
                return new SvResult("解析参数错误！").ToString();
            }
        }

        public static string RFIDReadFail
        {
            get
            {
                return new SvResult("RFID信息读取失败！").ToString();
            }
        }

        public static string RFIDWriteFail
        {
            get
            {
                return new SvResult("RFID信息写入失败！").ToString();
            }
        }

        public static string OrderNullError
        {
            get
            {
                return new SvResult("未查询到相关订单！").ToString();
            }
        }

    }

}
