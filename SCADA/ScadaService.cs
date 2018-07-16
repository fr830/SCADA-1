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

namespace SCADA
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ScadaService : IScadaService
    {
        public string TestService()
        {
            return new SvResult("服务正在运行", true).ToString();
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
                return new SvResult("请求参数错误！").ToString();
            }
            RFID.EnumWorkpiece wp;
            if (!Enum.TryParse<RFID.EnumWorkpiece>(type, true, out wp))
            {
                return new SvResult("解析参数错误！").ToString();
            }
            if (My.RFIDs[8].Init(wp))
            {
                return SvResult.OK;
            }
            else
            {
                return new SvResult("RFID信息写入失败！").ToString();
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

        public string PutIn()
        {
            var data = My.RFIDs[8].Read();
            if (data == null) return SvResult.Error;
            var state = EnumHelper.GetName(TWorkpieceProcess.EnumState.启动);
            var wpID = My.BLL.TWorkpiece.GetModel(Tool.CreateDict("Name", EnumHelper.GetName(data.Workpiece))).ID;
            var pc = My.BLL.TWorkpieceProcess.GetList(Tool.CreateDict("State", state, "WorkpieceID", wpID)).FirstOrDefault();
            pc.State = EnumHelper.GetName(TWorkpieceProcess.EnumState.完成);
            My.BLL.TWorkpieceProcess.Update(pc, My.AdminID);

            var order = GetExecOrder();
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

        public string PutOut()
        {
            var data = My.RFIDs[9].Read();
            if (data == null) return SvResult.Error;
            var order = GetExecOrder();
            var pc = new TWorkpieceProcess();
            pc.State = EnumHelper.GetName(TWorkpieceProcess.EnumState.启动);
            pc.LocationID = My.LocationID;
            pc.ManufactureID = My.ManufactureID;
            pc.WorkpieceID = My.BLL.TWorkpiece.GetModel(Tool.CreateDict("Name", EnumHelper.GetName(data.Workpiece))).ID;
            pc.OrderID = order.ID;
            My.BLL.TWorkpieceProcess.Insert(pc, My.AdminID);
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
                return JsonConvert.SerializeObject(new SvResult { Success = true });
            }
        }

        public static string Error
        {
            get
            {
                return JsonConvert.SerializeObject(new SvResult { Success = false });
            }
        }
    }

}
