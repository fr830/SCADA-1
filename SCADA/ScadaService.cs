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
            var dict = ParseQueryString(stream);
            if (!dict.ContainsKey("type"))
            {
                return SvResult.ParameterError;
            }
            var type = dict["type"] as string;
            RFID.EnumWorkpiece workpiece;
            if (!Enum.TryParse<RFID.EnumWorkpiece>(type, true, out workpiece))
            {
                return SvResult.AnalyticError;
            }
            else
            {
                var pc = new TWorkpieceProcess();
                pc.State = EnumHelper.GetName(TWorkpieceProcess.EnumState.启动);
                pc.LocationID = My.LocationID;
                pc.ManufactureID = My.ManufactureID;
                pc.WorkpieceID = My.WorkpieceIDs[workpiece];
                My.BLL.TWorkpieceProcess.Insert(pc, My.AdminID);
                var guid = new Guid(pc.ID);
                if (My.RFIDs[EnumPSite.S9_Manual].Init(guid, workpiece))
                {
                    return SvResult.OK;
                }
                else
                {
                    My.BLL.TWorkpieceProcess.DeleteReal(pc);
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
            var dict = ParseQueryString(stream);
            if (!dict.ContainsKey("site"))
            {
                return SvResult.ParameterError;
            }
            int site = 0;
            if (!int.TryParse(dict["site"].ToString(), out site))
            {
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

        private TOrder GetExecOrder()
        {
            return My.BLL.TOrder.GetModel(Tool.CreateDict("State", EnumHelper.GetName(TOrder.EnumState.执行)));
        }

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <returns></returns>
        public string RFID_In()
        {
            var data = My.RFIDs[EnumPSite.S7_Up].Read();
            if (data == null)
            {
                return SvResult.RFIDReadFail;
            }
            try
            {
                #region 修改订单明细，增加完成数量
                var pc = My.BLL.TWorkpieceProcess.GetModel(Tool.CreateDict("ID", data.Guid.ToString()));
                pc.State = EnumHelper.GetName(TWorkpieceProcess.EnumState.完成);
                My.BLL.TWorkpieceProcess.Update(pc, My.AdminID);
                var order = GetExecOrder();
                if (order == null)
                {
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
                    order.State = EnumHelper.GetName(TOrder.EnumState.完成);
                    order.EndTime = DateTime.Now;
                    My.BLL.TOrder.Update(order, My.AdminID);
                }
                #endregion
            }
            catch (Exception)
            {
                //TODO
                //throw;
            }
            Task.Run(() =>
            {
                var wmsData = new WMSData(Enum.GetName(typeof(EnumWorkpiece), data.Workpiece), data.Assemble == EnumAssemble.Unwanted ? 1 : 0, data.Guid.ToString());
                if (data.Workpiece == EnumWorkpiece.E)
                {
                    wmsData.quantity = data.Assemble == EnumAssemble.Successed ? 1 : 0;
                }
                My.Work_WMS.In(wmsData);
                My.Work_Simulation.Send(new RKX(data, RKX.EnumActionType.入库检测位转移物料至入库位));
            });
            return SvResult.OK;
        }

        /// <summary>
        /// 自动出库
        /// </summary>
        /// <returns></returns>
        public string RFID_Out()
        {
            var data = My.RFIDs[EnumPSite.S8_Down].Read();
            if (data == null)
            {
                return SvResult.RFIDReadFail;
            }
            else
            {
                My.Work_Vision.RFIDData = data;
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
                    return SvResult.OrderNullError;
                }
                foreach (var detail in My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID)))
                {
                    if (My.BLL.GetWorkpieceNameByWorkpieceID(detail.WorkpieceID) == EnumHelper.GetName(EnumWorkpiece.E))
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
                    return SvResult.RFIDWriteFail;
                }
                #endregion
            }
            catch (Exception)
            {
                //TODO
                //throw;
            }
            My.PLC.Set(11, 0);
            My.Work_Simulation.Send(new CKX(data, CKX.EnumActionType.出库检测位转移物料至定位台1));
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
                return new SvResult("", true).ToString();
            }
        }

        public static string Fail
        {
            get
            {
                return new SvResult("", false).ToString();
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
