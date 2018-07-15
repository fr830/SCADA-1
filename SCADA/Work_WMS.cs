using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HNC.MES.Common;
using HNC.MES.Model;


namespace SCADA
{
    class Work_WMS
    {
        private static readonly Lazy<Work_WMS> lazy = new Lazy<Work_WMS>(() => new Work_WMS());

        public static Work_WMS Instance { get { return lazy.Value; } }

        private Work_WMS()
        {
            RunScadaService();
            Start();
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

        /// <summary>
        /// 请求WMS系统工件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        private void RequestWorkpiece(string name, int count)
        {
            RFID.EnumWorkpiece wp;
            if (Enum.TryParse(name, true, out wp))
            {
                RequestWorkpiece(wp, count);
            }
        }

        /// <summary>
        /// 请求WMS系统工件
        /// </summary>
        /// <param name="wp"></param>
        /// <param name="count"></param>
        private void RequestWorkpiece(RFID.EnumWorkpiece wp, int count)
        {
            switch (wp)
            {
                case RFID.EnumWorkpiece.A:
                    break;
                case RFID.EnumWorkpiece.B:
                    break;
                case RFID.EnumWorkpiece.C:
                    break;
                case RFID.EnumWorkpiece.D:
                    break;
                case RFID.EnumWorkpiece.E:
                    break;
                default:
                    break;
            }
        }


        //public event EventHandler<WorkpiecePutEventArgs> WorkpiecePutIn;

        //public void OnWorkpiecePutIn(RFID.EnumWorkpiece wp)
        //{
        //    if (WorkpiecePutIn != null)
        //    {
        //        WorkpiecePutIn(this, new WorkpiecePutEventArgs(wp));
        //    }
        //}

        //public event EventHandler<WorkpiecePutEventArgs> WorkpiecePutOut;

        //public void OnWorkpiecePutOut(RFID.EnumWorkpiece wp)
        //{
        //    if (WorkpiecePutOut != null)
        //    {
        //        WorkpiecePutOut(this, new WorkpiecePutEventArgs(wp));
        //    }
        //}


        private CancellationTokenSource cts;
        private Task task;

        /// <summary>
        /// 服务运行状态
        /// </summary>
        public bool IsRunning { get { return task != null; } }

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
                    Thread.Sleep(500);
                    foreach (var order in My.BLL.TOrder.GetList())
                    {
                        if (order.State == EnumHelper.GetName(TOrder.EnumState.启动))
                        {
                            order.State = EnumHelper.GetName(TOrder.EnumState.执行);
                            My.BLL.TOrder.Update(order, My.AdminID);
                            foreach (var detail in My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID)))
                            {
                                detail.StartTime = DateTime.Now;
                                My.BLL.TOrderDetail.Update(detail, My.AdminID);
                                var name = My.BLL.GetWorkpieceNameByWorkpieceID(detail.WorkpieceID);
                                RequestWorkpiece(name, detail.QuantityDemanded);
                            }
                        }
                    }

                }
            }, token);
        }


    }

    public class WorkpiecePutEventArgs
    {
        public RFID.EnumWorkpiece Workpiece { get; private set; }

        public WorkpiecePutEventArgs(RFID.EnumWorkpiece wp)
        {
            Workpiece = wp;
        }
    }
}
