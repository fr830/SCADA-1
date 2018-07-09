﻿using System;
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
        /// Mac（Key：IP地址，Value：dbNo）
        /// </summary>
        public Dictionary<string, int> MacDict = new Dictionary<string, int>();

        /// <summary>
        /// 核心服务初始化，获取PLC与机床的连接
        /// </summary>
        private void Initialize()
        {
            string[] ipArray = My.BLL.SettingGet(My.AdminID, "MacIP").ToString().Split(';');
            foreach (var ip in ipArray)
            {
                int t = -1;
                My.MacDataService.GetMachineDbNo(ip, ref t);
                MacDict.Add(ip.Split('.').Last(), t);
            }
        }

        private void RunScadaService()
        {
            IScadaService service = new ScadaService();
            WebServiceHost host = new WebServiceHost(service, new Uri(@"http://localhost:41150/ScadaService"));
            host.Open();
        }

        private CancellationTokenSource cts;
        private Task task;

        public bool IsRunning { get { return task != null; } }

        public void Start()
        {
            Stop();
            cts = new CancellationTokenSource();
            task = GetServiceTask(cts.Token);
            task.Start();
        }

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

        private Task GetServiceTask(CancellationToken token)
        {
            return new Task(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(500);
                }
            }, token);
        }
    }
}
