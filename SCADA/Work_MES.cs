﻿using HNC.MES.Common;
using HNC.MES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_MES
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_MES> lazy = new Lazy<Work_MES>(() => new Work_MES());

        public static Work_MES Instance { get { return lazy.Value; } }

        private Work_MES()
        {
            Start();
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
            try
            {
                if (cts != null && !cts.IsCancellationRequested)
                {
                    cts.Cancel();
                }
                task.Wait();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                cts = null;
                task = null;
            }
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
                    Thread.Sleep(2000);
                    OrderToWMS();
                }
            }, token);
        }

        private TOrder GetOrder(TOrder.EnumState state)
        {
            return My.BLL.TOrder.GetModel(Tool.CreateDict("State", Enum.GetName(typeof(TOrder.EnumState), state)));
        }

        public void OrderToWMS()
        {
            if (GetOrder(TOrder.EnumState.执行) != null)
            {
                return;
            }
            else
            {
                var order = GetOrder(TOrder.EnumState.启动);
                if (order == null)
                {
                    return;
                }
                else
                {
                    order.State = "执行";//EnumHelper.GetName(TOrder.EnumState.执行);
                    order.StartTime = DateTime.Now;
                    My.BLL.TOrder.Update(order, My.AdminID);
                    var orderDetails = My.BLL.TOrderDetail.GetList(Tool.CreateDict("OrderID", order.ID));
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        var list = new List<WMSData>();
                        foreach (var detail in orderDetails)
                        {
                            detail.StartTime = DateTime.Now;
                            My.BLL.TOrderDetail.Update(detail, My.AdminID);
                            var name = My.BLL.GetWorkpieceNameByWorkpieceID(detail.WorkpieceID);
                            if (name == "E")//EnumHelper.GetName(RFID.EnumWorkpiece.E))
                            {
                                for (int i = 0; i < detail.QuantityDemanded; i++)
                                {
                                    //list.Add(new WMSData(EnumHelper.GetName(RFID.EnumWorkpiece.A), 1));
                                    //list.Add(new WMSData(EnumHelper.GetName(RFID.EnumWorkpiece.B), 1));
                                    //list.Add(new WMSData(EnumHelper.GetName(RFID.EnumWorkpiece.C), 1));
                                    //list.Add(new WMSData(EnumHelper.GetName(RFID.EnumWorkpiece.D), 1));
                                    //list.Add(new WMSData(EnumHelper.GetName(RFID.EnumWorkpiece.E), 0));
                                    list.Add(new WMSData("A", 1));
                                    list.Add(new WMSData("B", 1));
                                    list.Add(new WMSData("C", 1));
                                    list.Add(new WMSData("D1", 1));
                                    list.Add(new WMSData("E", 0));
                                }
                            }
                            else
                            {
                                for (int i = 0; i < detail.QuantityDemanded; i++)
                                {
                                    list.Add(new WMSData(name, 1));
                                }
                            }
                        }
                        var sb = new StringBuilder();
                        sb.Append("请求出库:");
                        foreach (var item in list)
                        {
                            sb.Append(item.ToString());
                        }
                        logger.Info(sb.ToString());
                        My.Work_WMS.Out(list);
                    }
                }
            }
        }

    }
}
