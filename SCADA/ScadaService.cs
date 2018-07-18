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

        public string InitRFID(Stream stream)
        {
            var dict = ParseQueryString(stream);
            var type = dict["type"] as string;
            if (string.IsNullOrWhiteSpace(type))
            {
                return SvResult.ParameterError;
            }
            RFID.EnumWorkpiece wp;
            if (!Enum.TryParse<RFID.EnumWorkpiece>(type, true, out wp))
            {
                return SvResult.AnalyticError;
            }
            else
            {
                var order = GetExecOrder();
                var pc = new TWorkpieceProcess();
                pc.State = EnumHelper.GetName(TWorkpieceProcess.EnumState.启动);
                pc.LocationID = My.LocationID;
                pc.ManufactureID = My.ManufactureID;
                pc.WorkpieceID = My.BLL.TWorkpiece.GetModel(Tool.CreateDict("Name", type.ToUpper())).ID;
                pc.OrderID = order.ID;
                My.BLL.TWorkpieceProcess.Insert(pc, My.AdminID);
                var guid = new Guid(pc.ID);
                if (My.RFIDs[EnumPSite.S9_Manual].Init(guid, wp))
                {
                    return SvResult.OK;
                }
                else
                {
                    My.BLL.TWorkpieceProcess.DeleteReal(pc);
                    return SvResult.RFISWriteFail;
                }
            }
        }

        public string Read(Stream stream)
        {
            var dict = ParseQueryString(stream);
            var obj = dict["site"] as string;
            if (string.IsNullOrWhiteSpace(obj))
            {
                return SvResult.ParameterError;
            }
            int site = 0;
            if (!int.TryParse(obj, out site))
            {
                return SvResult.AnalyticError;
            }
            else
            {
                var PSite = site == 1 ? EnumPSite.S7_Up : EnumPSite.S8_Down;
                if (PSite == EnumPSite.S7_Up)
                {
                    return PutIn();
                }
                else if (PSite == EnumPSite.S8_Down)
                {
                    return PutOut();
                }
                return SvResult.Fail;
            }
        }

        private TOrder GetExecOrder()
        {
            foreach (var order in My.BLL.TOrder.GetList())
            {
                if (order.State == EnumHelper.GetName(TOrder.EnumState.执行))
                {
                    return order;
                }
            }
            return null;
        }

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <returns></returns>
        public string PutIn()
        {
            var data = My.RFIDs[EnumPSite.S7_Up].Read();
            if (data == null) return SvResult.Fail;
            var wpID = My.BLL.TWorkpiece.GetModel(Tool.CreateDict("Name", EnumHelper.GetName(data.Workpiece))).ID;
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
                if (wpID == detail.WorkpieceID
                    && detail.QuantityCompletion < detail.QuantityDemanded)
                {
                    detail.EndTime = DateTime.Now;
                    detail.QuantityCompletion++;
                    My.BLL.TOrderDetail.Update(detail, My.AdminID);
                    break;
                }
            }
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
                My.BLL.TOrder.Update(order, My.AdminID);
            }
            return SvResult.OK;
        }

        /// <summary>
        /// 自动出库
        /// </summary>
        /// <returns></returns>
        public string PutOut()
        {
            var data = My.RFIDs[EnumPSite.S8_Down].Read();
            if (data == null)
            {
                return SvResult.Fail;
            }
            else
            {
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
                            if (My.RFIDs[EnumPSite.S8_Down].Write(data))
                            {
                                return SvResult.OK;
                            }
                            else
                            {
                                return SvResult.RFISWriteFail;
                            }
                        }
                    }
                    else
                    {
                        if(data.Assemble== EnumAssemble.Wanted)
                        {
                            data.Assemble = EnumAssemble.Unwanted;
                            if (My.RFIDs[EnumPSite.S8_Down].Write(data))
                            {
                                return SvResult.OK;
                            }
                            else
                            {
                                return SvResult.RFISWriteFail;
                            }
                        }
                    }
                }
                return SvResult.OK;
            }
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

        public static string RFISWriteFail
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
