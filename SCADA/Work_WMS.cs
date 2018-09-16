using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;


namespace SCADA
{
    class Work_WMS
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_WMS> lazy = new Lazy<Work_WMS>(() => new Work_WMS());

        public static Work_WMS Instance { get { return lazy.Value; } }

        public string WMSDownUri { get { return ConfigurationManager.AppSettings["WMSDown"]; } }

        public string WMSUpUri { get { return ConfigurationManager.AppSettings["WMSUp"]; } }

        public string WMSSpinUri { get { return ConfigurationManager.AppSettings["WMSSpin"]; } }

        public string WMSUpWait { get { return ConfigurationManager.AppSettings["WMSUpWait"]; } }

        private Work_WMS()
        {
            RunScadaService();
            My.Work_PLC.RequestIn += RequestIn;
            My.Work_PLC.PermitOut += PermitOut;
        }

        void RequestIn(object sender, PLCEventArgs e)
        {
            logger.Info("通知WMS请求入库");
            if (UpWait().IsOK)
            {
                logger.Info("WMS接收入库通知成功");
            }
            else
            {
                logger.Warn("WMS接收入库通知失败，重新发起 请求入库B11.2");
            }
        }

        void PermitOut(object sender, PLCEventArgs e)
        {
            logger.Info("调用出库皮带线");
            if (SpinOut().IsOK)
            {
                RFID.RFIDData data = null;
                My.Work_Vision.DataQueue.TryPeek(out data);
                My.Work_Simulation.Send(new CKX(data, CKX.EnumActionType.出库检测位转移物料至定位台1));
                logger.Info("调用出库皮带线成功");
            }
            else
            {
                logger.Warn("出库皮带线调用失败，重新发起 请求出库B11.0");
            }
        }

        /// <summary>
        /// 运行Scada服务（供WMS系统调用）
        /// </summary>
        private void RunScadaService()
        {
            IScadaService service = new ScadaService();
            WebServiceHost host = new WebServiceHost(service, new Uri(@"http://localhost:41150/ScadaService"));
#if !OFFLINE
            host.Open();
            logger.Info("启动ScadaService服务");
#endif
        }

        private WMSResult WMSPost(string uri, string json, int seconds = 10)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(json);
                var request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = "POST";
                request.Timeout = seconds * 1000;
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
                logger.Info("Post:{0}", json);
                var response = request.GetResponse() as HttpWebResponse;
                var responseStream = response.GetResponseStream();
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string str = sr.ReadToEnd();
                    logger.Info("Receivce:{0}", str);
                    return JsonConvert.DeserializeObject<WMSResult>(str);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(uri);
                logger.Error(json);
                return WMSResult.Error;
            }
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public WMSResult Out(IEnumerable<WMSData> list)
        {
            return WMSPost(WMSDownUri, JsonConvert.SerializeObject(list), 30 * 60);
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public WMSResult In(WMSData data)
        {
            return WMSPost(WMSUpUri, JsonConvert.SerializeObject(data), 10 * 60);
        }

        /// <summary>
        /// 皮带线运行
        /// </summary>
        /// <returns></returns>
        private WMSResult Spin(SpinData data)
        {
            return WMSPost(WMSSpinUri, JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 入库皮带线运行
        /// </summary>
        /// <returns></returns>
        public WMSResult SpinIn()
        {
            return Spin(new SpinData { site = "1" });
        }

        /// <summary>
        /// 出库皮带线运行
        /// </summary>
        /// <returns></returns>
        public WMSResult SpinOut()
        {
            return Spin(new SpinData { site = "2" });
        }

        /// <summary>
        /// 通知WMS有料要入库
        /// </summary>
        /// <returns></returns>
        public WMSResult UpWait()
        {
            return WMSPost(WMSUpWait, string.Empty);
        }

    }

    /// <summary>
    /// 皮带线数据
    /// </summary>
    public class SpinData
    {
        /// <summary>
        /// 1为入库，2为出库
        /// </summary>
        public string site { get; set; }
    }

    /// <summary>
    /// 入库数据
    /// </summary>
    public class WMSData
    {
        /// <summary>
        /// A-E
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        public string trayId { get; set; }

        /// <summary>
        /// 0空盘 1有料
        /// </summary>
        public int quantity { get; set; }

        public WMSData() { }

        public WMSData(string name, int count, string guid = null)
        {
            code = name;
            quantity = count;
            trayId = guid;
        }

        public override string ToString()
        {
            return string.Format("|ID:{0} 类型:{1}-{2}|", trayId, code, quantity == 0 ? "空盘" : "有料");
        }
    }

    public class WMSResult
    {
        public long code { get; set; }

        public string msg { get; set; }

        public object data { get; set; }

        public bool IsOK
        {
            get { return code == 0; }
        }

        public static WMSResult Error
        {
            get
            {
                return new WMSResult
                {
                    code = -1,
                    msg = "请求解析错误"
                };
            }
        }

        public static WMSResult OK
        {
            get
            {
                return new WMSResult
                {
                    code = 0,
                    msg = "操作成功"
                };
            }
        }
    }

}
