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
        private static readonly Lazy<Work_MES> lazy = new Lazy<Work_MES>(() => new Work_MES());

        public static Work_MES Instance { get { return lazy.Value; } }

        private Work_MES()
        {
            Start();
        }

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
                    Thread.Sleep(5000);
                    OrderToWMS();
                }
            }, token);
        }

        private TOrder GetOrder(TOrder.EnumState state)
        {
            return My.BLL.TOrder.GetModel(Tool.CreateDict("State", EnumHelper.GetName(state)));
        }

        public void OrderToWMS()
        {
            if (GetOrder(TOrder.EnumState.执行) != null)
            {
                return;//TODO
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
                    order.State = EnumHelper.GetName(TOrder.EnumState.执行);
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
                            list.Add(new WMSData(name, detail.QuantityDemanded));
                        }
                        My.Work_WMS.Down(list);
                    }
                }
            }
        }

    }
}