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
                    if (My.PLC.BitExist(1, 0))
                    {
                        My.PLC.BitClear(1, 0);
                        OnCamera(EnumPSite.S8_Down, 1);
                    }
                    if (My.PLC.BitExist(1, 10))
                    {
                        My.PLC.BitClear(1, 10);
                        OnScan(EnumPSite.S8_Down, 1);
                    }
                    foreach (EnumPSite site in Enum.GetValues(typeof(EnumPSite)))
                    {
                        if (site < EnumPSite.S1 || site > EnumPSite.S4)
                        {
                            continue;
                        }
                        var i = SiteIndexDict[site];
                        if (My.PLC.BitExist(i, 0))
                        {
                            My.PLC.BitClear(i, 0);
                            OnProcessRead(site, i);
                        }
                        if (My.PLC.BitExist(i, 10))
                        {
                            My.PLC.BitClear(i, 10);
                            OnProcessWriteSuccess(site, i);
                        }
                        if (My.PLC.BitExist(i, 11))
                        {
                            My.PLC.BitClear(i, 11);
                            OnProcessWriteFailure(site, i);
                        }
                    }
                    if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S5_Assemble], 0))
                    {
                        My.PLC.BitClear(SiteIndexDict[EnumPSite.S5_Assemble], 0);
                        OnAssembleRead(SiteIndexDict[EnumPSite.S5_Assemble]);
                    }
                    if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S5_Assemble], 10))
                    {
                        My.PLC.BitClear(SiteIndexDict[EnumPSite.S5_Assemble], 10);
                        OnAssembleWriteSuccess(SiteIndexDict[EnumPSite.S5_Assemble]);
                    }
                    if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S5_Assemble], 11))
                    {
                        My.PLC.BitClear(SiteIndexDict[EnumPSite.S5_Assemble], 11);
                        OnAssembleWriteFailure(SiteIndexDict[EnumPSite.S5_Assemble]);
                    }
                    if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S6_Alignment], 0))
                    {
                        My.PLC.BitClear(SiteIndexDict[EnumPSite.S6_Alignment], 0);
                        OnAlignmentRead(SiteIndexDict[EnumPSite.S6_Alignment]);
                    }
                    if (My.PLC.BitExist(SiteIndexDict[EnumPSite.S6_Alignment], 10))
                    {
                        My.PLC.BitClear(SiteIndexDict[EnumPSite.S6_Alignment], 10);
                        OnPrintQRCode(EnumPSite.S6_Alignment, SiteIndexDict[EnumPSite.S6_Alignment]);
                    }
                }
            }, token);
        }

        /// <summary>
        /// 加工请求读
        /// </summary>
        public event EventHandler<PLCEventArgs> ProcessRead;

        private void OnProcessRead(EnumPSite site, int index)
        {
            if (ProcessRead != null)
            {
                ProcessRead(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 加工请求写成功
        /// </summary>
        public event EventHandler<PLCEventArgs> ProcessWriteSuccess;

        private void OnProcessWriteSuccess(EnumPSite site, int index)
        {
            if (ProcessWriteSuccess != null)
            {
                ProcessWriteSuccess(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 加工请求写失败
        /// </summary>
        public event EventHandler<PLCEventArgs> ProcessWriteFailure;

        private void OnProcessWriteFailure(EnumPSite site, int index)
        {
            if (ProcessWriteFailure != null)
            {
                ProcessWriteFailure(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 装配台请求读
        /// </summary>
        public event EventHandler<PLCEventArgs> AssembleRead;

        private void OnAssembleRead(int index, EnumPSite site = EnumPSite.S5_Assemble)
        {
            if (AssembleRead != null)
            {
                AssembleRead(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 装配台请求写成功
        /// </summary>
        public event EventHandler<PLCEventArgs> AssembleWriteSuccess;

        private void OnAssembleWriteSuccess(int index, EnumPSite site = EnumPSite.S5_Assemble)
        {
            if (AssembleWriteSuccess != null)
            {
                AssembleWriteSuccess(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 装配台请求写失败
        /// </summary>
        public event EventHandler<PLCEventArgs> AssembleWriteFailure;

        private void OnAssembleWriteFailure(int index, EnumPSite site = EnumPSite.S5_Assemble)
        {
            if (AssembleWriteFailure != null)
            {
                AssembleWriteFailure(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 定位台请求读
        /// </summary>
        public event EventHandler<PLCEventArgs> AlignmentRead;

        private void OnAlignmentRead(int index, EnumPSite site = EnumPSite.S6_Alignment)
        {
            if (AlignmentRead != null)
            {
                AlignmentRead(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求打印二维码
        /// </summary>
        public event EventHandler<PLCEventArgs> PrintQRCode;

        private void OnPrintQRCode(EnumPSite site, int index)
        {
            if (PrintQRCode != null)
            {
                PrintQRCode(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求相机拍照
        /// </summary>
        public event EventHandler<PLCEventArgs> Camera;

        private void OnCamera(EnumPSite site, int index)
        {
            if (Camera != null)
            {
                Camera(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求扫码器扫码
        /// </summary>
        public event EventHandler<PLCEventArgs> Scan;

        private void OnScan(EnumPSite site, int index)
        {
            if (Scan != null)
            {
                Scan(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 入库请求
        /// </summary>
        public event EventHandler<PLCEventArgs> WorkpieceIn;

        private void OnWorkpieceIn(EnumPSite site, int index)
        {
            if (WorkpieceIn != null)
            {
                WorkpieceIn(this, new PLCEventArgs(site, index));
            }
        }


    }

    public class PLCEventArgs : EventArgs
    {
        public EnumPSite Site { get; private set; }

        public int Index { get; private set; }

        public PLCEventArgs(EnumPSite site, int index)
        {
            Site = site;
            Index = index;
        }
    }

}
