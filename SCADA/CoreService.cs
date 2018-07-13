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
    class CoreService
    {
        private static readonly Lazy<CoreService> lazy = new Lazy<CoreService>(() => new CoreService());

        public static CoreService Instance { get { return lazy.Value; } }

        private CoreService()
        {
            Initialize();
            RunScadaService();
        }

        /// <summary>
        /// Mac（Key：IP地址最后一位，Value：MachineTool）
        /// </summary>
        public SortedDictionary<int, MachineTool> MachineTools { get; private set; }

        public MachineTool PLC
        {
            get
            {
                if (MachineTools != null && MachineTools.Count > 0)
                {
                    return MachineTools[0];
                }
                return null;
            }
        }

        /// <summary>
        /// RFID（Key：IP地址最后一位，Value：RFID）
        /// </summary>
        public SortedDictionary<int, RFIDItem> RFIDs { get; private set; }

        /// <summary>
        /// 核心服务初始化，获取PLC、机床、RFID的连接
        /// </summary>
        private void Initialize()
        {
            MachineTools = new SortedDictionary<int, MachineTool>();
            var macIPs = My.BLL.SettingGet(My.AdminID, "MacIP").ToString().Split(';');
            for (int i = 0; i < macIPs.Length; i++)
            {
                MachineTools.Add(i, new MachineTool(macIPs[i]));
            }
            RFIDs = new SortedDictionary<int, RFIDItem>();
            var rfidIPs = My.BLL.SettingGet(My.AdminID, "RFIDIP").ToString().Split(';');
            for (int i = 0; i < rfidIPs.Length; i++)
            {
                RFIDs.Add(i + 2, new RFIDItem(i + 2, rfidIPs[i]));
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
                    if (PLC.BitExist(0, 0))
                    {
                        PLC.BitClear(0, 0);
                        OnCamera_IsRequested();
                    }
                    if (PLC.BitExist(0, 10))
                    {
                        PLC.BitClear(0, 10);
                        OnScan_IsRequested();
                    }
                    for (int i = 2; i < 8; i++)
                    {
                        if (PLC.BitExist(i, 0))
                        {
                            PLC.BitClear(i, 0);
                            RFIDs[i].OnRead_IsRequested();
                        }
                        if (i < 7)
                        {
                            if (PLC.BitExist(i, 10))
                            {
                                PLC.BitClear(i, 10);
                                RFIDs[i].OnWrite_Process_Success_IsRequested();
                            }
                            if (PLC.BitExist(i, 11))
                            {
                                PLC.BitClear(i, 11);
                                RFIDs[i].OnWrite_Process_Failure_IsRequested();
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
