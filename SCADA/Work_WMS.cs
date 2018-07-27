using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HNC.MES.Common;
using HNC.MES.Model;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Configuration;


namespace SCADA
{
    class Work_WMS
    {
        private static readonly Lazy<Work_WMS> lazy = new Lazy<Work_WMS>(() => new Work_WMS());

        public static Work_WMS Instance { get { return lazy.Value; } }

        public string WMSDownUri { get { return ConfigurationManager.AppSettings["WMSDown"]; } }

        public string WMSUpUri { get { return ConfigurationManager.AppSettings["WMSUp"]; } }

        public string WMSSpinUri { get { return ConfigurationManager.AppSettings["WMSSpin"]; } }

        private Work_WMS()
        {
            RunScadaService();
            //Start();
            My.Work_PLC.RequestIn += RequestIn;
            My.Work_PLC.PermitOut += PermitOut;
        }

        void RequestIn(object sender, PLCEventArgs e)
        {
            if (!SpinIn().IsOK)
            {
                //TODO：提示用户，重新发起 请求入库B11.2
            }
            else
            {
                My.PLC.Set(11, 3);
            }
        }

        void PermitOut(object sender, PLCEventArgs e)
        {
            if (!SpinOut().IsOK)
            {
                //TODO：提示用户，重新发起 出库许可B11.1
            }
        }

        /// <summary>
        /// 运行Scada服务（供WMS系统调用）
        /// </summary>
        private void RunScadaService()
        {
            IScadaService service = new ScadaService();
            WebServiceHost host = new WebServiceHost(service, new Uri(@"http://localhost:41150/ScadaService"));
            host.Open();
        }

        private WMSResult WMSPost(string uri, string json)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(json);
                var request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = "POST";
                request.Timeout = 10 * 60 * 1000;
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
                var response = request.GetResponse() as HttpWebResponse;
                var responseStream = response.GetResponseStream();
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string str = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<WMSResult>(str);
                }
            }
            catch (Exception)
            {
                return WMSResult.Error;
                //throw;
            }
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public WMSResult Out(IList<WMSData> list)
        {
            return WMSPost(WMSDownUri, JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public WMSResult In(WMSData data)
        {
            return WMSPost(WMSUpUri, JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 皮带线运行
        /// </summary>
        /// <returns></returns>
        public WMSResult Spin(SpinData data)
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



        private CancellationTokenSource cts;
        private Task task;

        private DateTime lastTime = DateTime.Now;

        /// <summary>
        /// 服务运行状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return task != null && DateTime.Now - lastTime < TimeSpan.FromSeconds(10);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            Stop();
            cts = new CancellationTokenSource();
            task = GetServiceTask(cts.Token);
            task.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (task == null) return;
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
            task.Wait();
            cts = null;
            task = null;
        }

        /// <summary>
        /// 获取服务Task
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task GetServiceTask(CancellationToken token)
        {
            return new Task(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    lastTime = DateTime.Now;
                    Thread.Sleep(1000);
                }
            }, token);
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

        public WMSData(string name, int count, string guid = null)
        {
            code = name;
            quantity = count;
            trayId = guid;
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
                    msg = "操作成功！"
                };
            }
        }
    }

}
