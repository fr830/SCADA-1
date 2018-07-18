using RFID;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_PLC
    {
        private static readonly Lazy<Work_PLC> lazy = new Lazy<Work_PLC>(() => new Work_PLC());

        public static Work_PLC Instance { get { return lazy.Value; } }

        private Work_PLC()
        {
        }

        private static Dictionary<EnumPSite, int> SiteIndexDict = new Dictionary<EnumPSite, int>
        {
            {EnumPSite.S1,2},{EnumPSite.S2,3},{EnumPSite.S3,4},{EnumPSite.S4,5},{EnumPSite.S5_Assemble,6},{EnumPSite.S6_Alignment,7}
        };

        public static Dictionary<EnumWorkpiece, int> WpBitDict = new Dictionary<EnumWorkpiece, int>
        {
            {EnumWorkpiece.A,4},{EnumWorkpiece.B,5},{EnumWorkpiece.C,6},{EnumWorkpiece.D,7},{EnumWorkpiece.E,8},
        };


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
                    foreach (EnumPSite site in Enum.GetValues(typeof(EnumPSite)))
                    {
                        if (site < EnumPSite.S1 || site > EnumPSite.S5_Assemble)
                        {
                            continue;
                        }
                        var i = SiteIndexDict[site];
                        if (My.PLC.BitExist(i, 0))
                        {
                            My.PLC.BitClear(i, 0);
                            OnRead_IsRequested(site, i);
                        }
                        if (My.PLC.BitExist(i, 10))
                        {
                            My.PLC.BitClear(i, 10);
                            OnWrite_Process_Success_IsRequested(site, i);
                        }
                        if (My.PLC.BitExist(i, 11))
                        {
                            My.PLC.BitClear(i, 11);
                            OnWrite_Process_Failure_IsRequested(site, i);
                        }
                    }
                    {
                        if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S6_Alignment], 0))
                        {
                            My.PLC.BitClear(SiteIndexDict[EnumPSite.S6_Alignment], 0);
                            OnEntry_IsRequested(SiteIndexDict[EnumPSite.S6_Alignment]);
                        }
                        if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S6_Alignment], 10))
                        {
                            My.PLC.BitClear(SiteIndexDict[EnumPSite.S6_Alignment], 10);
                            OnPrint_QR_Code_IsRequested(SiteIndexDict[EnumPSite.S6_Alignment]);
                        }
                    }
                }
            }, token);
        }

        /// <summary>
        /// 请求读
        /// </summary>
        public event EventHandler<RFIDEventArgs> Read_IsRequested;

        private void OnRead_IsRequested(EnumPSite site, int index)
        {
            if (Read_IsRequested != null)
            {
                Read_IsRequested(this, new RFIDEventArgs(site, index));
            }
        }

        /// <summary>
        /// 定位台请求读
        /// </summary>
        public event EventHandler<RFIDEventArgs> Entry_IsRequested;

        private void OnEntry_IsRequested(int index, EnumPSite site = EnumPSite.S6_Alignment)
        {
            if (Entry_IsRequested != null)
            {
                Entry_IsRequested(this, new RFIDEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求写（加工成功）
        /// </summary>
        public event EventHandler<RFIDEventArgs> Write_Process_Success_IsRequested;

        private void OnWrite_Process_Success_IsRequested(EnumPSite site, int index)
        {
            if (Write_Process_Success_IsRequested != null)
            {
                Write_Process_Success_IsRequested(this, new RFIDEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求写（加工失败）
        /// </summary>
        public event EventHandler<RFIDEventArgs> Write_Process_Failure_IsRequested;

        private void OnWrite_Process_Failure_IsRequested(EnumPSite site, int index)
        {
            if (Write_Process_Failure_IsRequested != null)
            {
                Write_Process_Failure_IsRequested(this, new RFIDEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求打印二维码
        /// </summary>
        public event EventHandler Print_QR_Code_IsRequested;

        private void OnPrint_QR_Code_IsRequested(int index)
        {
            if (Print_QR_Code_IsRequested != null)
            {
                Print_QR_Code_IsRequested(this, new EventArgs());
            }
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

    public class RFIDEventArgs : EventArgs
    {
        public EnumPSite Site { get; private set; }

        public int Index { get; private set; }

        public RFIDEventArgs(EnumPSite site, int index)
        {
            Site = site;
            Index = index;
        }
    }

}
