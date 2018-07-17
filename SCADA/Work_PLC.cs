using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HNC.MES.Common;
using HNC.MES.Model;
using HNC.MES.DAL;
using HNC.MES.BLL;
using System.ServiceModel.Web;
using System.Collections.Concurrent;
using Sygole.HFReader;
using System.Reflection;
using RFID;
using HNC;

namespace SCADA
{
    class Work_PLC
    {
        private static readonly Lazy<Work_PLC> lazy = new Lazy<Work_PLC>(() => new Work_PLC());

        public static Work_PLC Instance { get { return lazy.Value; } }

        private Work_PLC()
        {
        }


        private CancellationTokenSource cts;
        private Task task;

        /// <summary>
        /// 核心服务运行状态
        /// </summary>
        public bool IsRunning { get { return task != null; } }

        /// <summary>
        /// 启动核心服务
        /// </summary>
        public void Start()
        {
            Stop();
            cts = new CancellationTokenSource();
            task = GetServiceTask(cts.Token);
            task.Start();
        }

        /// <summary>
        /// 停止核心服务
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
        /// 获取核心服务Task
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
                    if (My.PLC.BitExist(0, 0))
                    {
                        My.PLC.BitClear(0, 0);
                        OnCamera_IsRequested();
                    }
                    if (My.PLC.BitExist(0, 10))
                    {
                        My.PLC.BitClear(0, 10);
                        OnScan_IsRequested();
                    }
                    foreach (var item in My.RFIDs)
                    {
                        var i = Work_RFID.SiteIndexDict[item.Key];
                        var RFID = item.Value;
                        if (item.Key == EnumPSite.S6_Alignment)
                        {
                            if (My.PLC.BitExist(i, 0))
                            {
                                My.PLC.BitClear(i, 0);
                                RFID.OnRead_IsRequested();
                            }
                            if (My.PLC.BitExist(i, 10))
                            {
                                My.PLC.BitClear(i, 10);
                                RFID.OnPrint_QR_Code_IsRequested();
                            }
                        }
                        else if (item.Key == EnumPSite.S7_Up)
                        {

                        }
                        else if (item.Key == EnumPSite.S8_Down)
                        {

                        }
                        else if (item.Key == EnumPSite.S9_Manual)
                        {

                        }
                        else
                        {
                            if (My.PLC.BitExist(i, 0))
                            {
                                My.PLC.BitClear(i, 0);
                                RFID.OnRead_IsRequested();
                            }
                            if (My.PLC.BitExist(i, 10))
                            {
                                My.PLC.BitClear(i, 10);
                                RFID.OnWrite_Process_Success_IsRequested();
                            }
                            if (My.PLC.BitExist(i, 11))
                            {
                                My.PLC.BitClear(i, 11);
                                RFID.OnWrite_Process_Failure_IsRequested();
                            }
                        }
                    }
                }
            }, token);
        }

        /// <summary>
        /// 请求相机拍照
        /// </summary>
        public event EventHandler Camera_IsRequested;

        private void OnCamera_IsRequested()
        {
            if (Camera_IsRequested != null)
            {
                Camera_IsRequested(this, new EventArgs());
            }
        }

        /// <summary>
        /// 请求扫码器扫码
        /// </summary>
        public event EventHandler Scan_IsRequested;

        private void OnScan_IsRequested()
        {
            if (Scan_IsRequested != null)
            {
                Scan_IsRequested(this, new EventArgs());
            }
        }


    }
}
