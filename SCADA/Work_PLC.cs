using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RFID;

namespace SCADA
{
    class Work_PLC
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_PLC> lazy = new Lazy<Work_PLC>(() => new Work_PLC());

        public static Work_PLC Instance { get { return lazy.Value; } }

        private Work_PLC()
        {
        }

        public static Dictionary<EnumPSite, int> SiteIndexDict = new Dictionary<EnumPSite, int>
        {
            {EnumPSite.S1,2},{EnumPSite.S2,3},{EnumPSite.S3,4},{EnumPSite.S4,5},{EnumPSite.S5_Assemble,6},{EnumPSite.S6_Alignment,7},{EnumPSite.S7_Up,0},{EnumPSite.S8_Down,0},{EnumPSite.S9_Manual,0}
        };

        public static Dictionary<EnumWorkpiece, int> WpBitDict = new Dictionary<EnumWorkpiece, int>
        {
            {EnumWorkpiece.A,4},{EnumWorkpiece.B,5},{EnumWorkpiece.C,6},{EnumWorkpiece.D,7},{EnumWorkpiece.E,8},
        };


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
                return task != null && DateTime.Now - lastTime < TimeSpan.FromSeconds(20);
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
                logger.Error(ex);
            }
            finally
            {
                cts = null;
                task = null;
            }
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
                    lastTime = DateTime.Now;
                    //logger.Info("PLC连接正常");
                    Thread.Sleep(1000);
                    if (My.PLC.Trigger(1, 0))//请求相机拍照
                    {
                        logger.Info("触发B1.0:PLC请求相机拍照");
                        OnPhotograph(EnumPSite.S8_Down, 1);//Work_Vision
                    }
                    if (My.PLC.Trigger(1, 10))//请求扫码器扫码
                    {
                        logger.Info("触发B1.10:PLC请求扫码器扫码");
                        OnScan(EnumPSite.S8_Down, 1);//Work_QRCode
                    }
                    foreach (EnumPSite site in Enum.GetValues(typeof(EnumPSite)))
                    {
                        if (site < EnumPSite.S1 || site > EnumPSite.S4)
                        {
                            continue;
                        }
                        var i = SiteIndexDict[site];
                        if (My.PLC.Trigger(i, 0))//加工请求读
                        {
                            logger.Info("触发B{0}.0:PLC加工请求读", i);
                            OnProcessRead(site, i);//Work_RFID
                        }
                        if (My.PLC.Trigger(i, 10))//加工请求写成功
                        {
                            logger.Info("触发B{0}.0:PLC加工请求写成功", i);
                            OnProcessWriteSuccess(site, i);//Work_RFID
                        }
                        if (My.PLC.Trigger(i, 11))//加工请求写失败
                        {
                            logger.Info("触发B{0}.0:PLC加工请求写失败", i);
                            OnProcessWriteFailure(site, i);//Work_RFID
                        }
                    }
                    if (My.PLC.Trigger(SiteIndexDict[EnumPSite.S5_Assemble], 0))//装配台请求读
                    {
                        logger.Info("触发B{0}.0:PLC装配台请求读", SiteIndexDict[EnumPSite.S5_Assemble]);
                        OnAssembleRead(SiteIndexDict[EnumPSite.S5_Assemble]);//Work_RFID
                    }
                    if (My.PLC.Trigger(SiteIndexDict[EnumPSite.S5_Assemble], 10))//装配台请求写成功
                    {
                        logger.Info("触发B{0}.0:PLC装配台请求写成功", SiteIndexDict[EnumPSite.S5_Assemble]);
                        OnAssembleWriteSuccess(SiteIndexDict[EnumPSite.S5_Assemble]);//Work_RFID
                    }
                    if (My.PLC.Trigger(SiteIndexDict[EnumPSite.S5_Assemble], 11))//装配台请求写失败
                    {
                        logger.Info("触发B{0}.0:PLC装配台请求写失败", SiteIndexDict[EnumPSite.S5_Assemble]);
                        OnAssembleWriteFailure(SiteIndexDict[EnumPSite.S5_Assemble]);//Work_RFID
                    }
                    if (My.PLC.Trigger(SiteIndexDict[EnumPSite.S6_Alignment], 0))//定位台请求读
                    {
                        logger.Info("触发B{0}.0:PLC定位台请求读", SiteIndexDict[EnumPSite.S6_Alignment]);
                        OnAlignmentRead(SiteIndexDict[EnumPSite.S6_Alignment]);//Work_RFID
                    }
                    if (My.PLC.Trigger(SiteIndexDict[EnumPSite.S6_Alignment], 10))//请求打印二维码
                    {
                        logger.Info("触发B{0}.0:PLC请求打印二维码", SiteIndexDict[EnumPSite.S6_Alignment]);
                        OnPrintQRCode(SiteIndexDict[EnumPSite.S6_Alignment]);//Work_QRCode
                    }
                    if (My.PLC.Trigger(11, 1))//出库许可
                    {
                        logger.Info("触发B11.1:PLC出库许可");
                        OnPermitOut(EnumPSite.S8_Down, 11);//Work_WMS
                    }
                    if (My.PLC.Trigger(11, 2))//请求入库
                    {
                        logger.Info("触发B11.2:PLC请求入库");
                        OnRequestIn(EnumPSite.S7_Up, 11);//Work_WMS
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

        private void OnPrintQRCode(int index, EnumPSite site = EnumPSite.S6_Alignment)
        {
            if (PrintQRCode != null)
            {
                PrintQRCode(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 请求相机拍照
        /// </summary>
        public event EventHandler<PLCEventArgs> Photograph;

        private void OnPhotograph(EnumPSite site, int index)
        {
            if (Photograph != null)
            {
                Photograph(this, new PLCEventArgs(site, index));
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
        /// 请求入库
        /// </summary>
        public event EventHandler<PLCEventArgs> RequestIn;

        private void OnRequestIn(EnumPSite site, int index)
        {
            if (RequestIn != null)
            {
                RequestIn(this, new PLCEventArgs(site, index));
            }
        }

        /// <summary>
        /// 出库许可
        /// </summary>
        public event EventHandler<PLCEventArgs> PermitOut;

        private void OnPermitOut(EnumPSite site, int index)
        {
            if (PermitOut != null)
            {
                PermitOut(this, new PLCEventArgs(site, index));
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
