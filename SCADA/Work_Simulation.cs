using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RFID;

namespace SCADA
{
    class Work_Simulation
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_Simulation> lazy = new Lazy<Work_Simulation>(() => new Work_Simulation());

        public static Work_Simulation Instance { get { return lazy.Value; } }

        public IPAddress IP { get { return IPAddress.Parse(ConfigurationManager.AppSettings["SimulationIP"]); } }

        public int Port { get { return int.Parse(ConfigurationManager.AppSettings["SimulationPort"]); } }

        private ConcurrentQueue<byte[]> messages = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// 等待发送的消息数量
        /// </summary>
        public int WaitForSendingCount
        {
            get
            {
                return messages.Count;
            }
        }

        public TcpClient tcpClient = new TcpClient();

        private Work_Simulation()
        {
#if !OFFLINE
            AutoSendAsync();
#endif
            //Start();
        }

        private async void AutoSendAsync()
        {
            await Task.Run(async () =>
            {
                int failedTimes = 0;
                while (true)
                {
                    Thread.Sleep(500);
                    try
                    {
                        if (!tcpClient.Connected)
                        {
                            tcpClient.Close();
                            tcpClient = new TcpClient();
                            await tcpClient.ConnectAsync(IP, Port);
                        }
                        else
                        {
                            byte[] data;
                            if (messages.TryDequeue(out data))
                            {
                                tcpClient.Client.Send(data);
                                logger.Info("三维仿真：{0}", Encoding.UTF8.GetString(data));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        failedTimes++;
                        if (failedTimes >= 30)
                        {
                            failedTimes = 0;
                            logger.Error("连接三维仿真失败");
                        }
                    }
                }
            });
        }

        public void Send<TEquipment>(TEquipment equipment) where TEquipment : Equipment
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("&");
            sb.Append(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            sb.Append(",1,");
            sb.Append(equipment.ToString());
            sb.Append("#");
            var data = Encoding.UTF8.GetBytes(sb.ToString());
            messages.Enqueue(data);
        }

        public async Task SendAsync<TEquipment>(TEquipment equipment) where TEquipment : Equipment
        {
            await Task.Run(() =>
            {
                Send(equipment);
            });
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
                //return task != null && DateTime.Now - lastTime < TimeSpan.FromSeconds(10);
                return tcpClient.Client.Connected;
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
        /// 获取服务Task
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task GetServiceTask(CancellationToken token)
        {
            return new Task(() =>
            {
                var flag_DSJ01_3 = false;
                var flag_DSJ02_3 = false;
                var flag_DSJ03_3 = false;
                var flag_DSJ04_3 = false;
                var flag_DSJ05_3 = false;
                var flag_XLW_3 = false;
                var flag_SJJ01_2 = false;
                while (!token.IsCancellationRequested)
                {
                    lastTime = DateTime.Now;
                    Thread.Sleep(500);
                    //#region DSJ01
                    //if (My.PLC.Exist(24, 1, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(21, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ01(null, DSJ01.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_DSJ01_3 && My.PLC.Exist(24, 2, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(21, 3, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(21, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_DSJ01_3 = true;
                    //    Send(new DSJ01(null, DSJ01.EnumActionType.正阻挡位转移物料至顶升机2前阻挡位));
                    //    Send(new DSJ02(null, DSJ02.EnumActionType.前阻挡位到位));
                    //}
                    //else
                    //{
                    //    flag_DSJ01_3 = false;
                    //}
                    //if (My.PLC.Exist(22, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(22, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ01(null, DSJ01.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region DSJ02
                    //if (My.PLC.Exist(34, 1, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(31, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ02(null, DSJ02.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_DSJ02_3 && My.PLC.Exist(34, 2, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(31, 3, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(31, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_DSJ02_3 = true;
                    //    Send(new DSJ02(null, DSJ02.EnumActionType.正阻挡位转移物料至顶升机3前阻挡位));
                    //    Send(new DSJ03(null, DSJ03.EnumActionType.前阻挡位到位));
                    //}
                    //else
                    //{
                    //    flag_DSJ02_3 = false;
                    //}
                    //if (My.PLC.Exist(32, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(32, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ02(null, DSJ02.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region DSJ03
                    //if (My.PLC.Exist(44, 1, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(41, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ03(null, DSJ03.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_DSJ03_3 && My.PLC.Exist(44, 2, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(41, 3, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(41, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_DSJ03_3 = true;
                    //    Send(new DSJ03(null, DSJ03.EnumActionType.正阻挡位转移物料至顶升机4前阻挡位));
                    //    Send(new DSJ04(null, DSJ04.EnumActionType.前阻挡位到位));
                    //}
                    //else
                    //{
                    //    flag_DSJ03_3 = false;
                    //}
                    //if (My.PLC.Exist(42, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(42, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ03(null, DSJ03.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region DSJ04
                    //if (My.PLC.Exist(54, 1, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(51, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ04(null, DSJ04.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_DSJ04_3 && My.PLC.Exist(54, 2, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(51, 3, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(51, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_DSJ04_3 = true;
                    //    Send(new DSJ04(null, DSJ04.EnumActionType.正阻挡位转移物料至顶升机5前阻挡位));
                    //    Send(new DSJ05(null, DSJ05.EnumActionType.前阻挡位到位));
                    //}
                    //else
                    //{
                    //    flag_DSJ04_3 = false;
                    //}
                    //if (My.PLC.Exist(52, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(52, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ04(null, DSJ04.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region DSJ05
                    //if (My.PLC.Exist(64, 1, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(61, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ05(null, DSJ05.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_DSJ05_3 && My.PLC.Exist(64, 2, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(61, 3, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(61, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_DSJ05_3 = true;
                    //    Send(new DSJ05(null, DSJ05.EnumActionType.正阻挡位转移物料至下料位前阻挡位));
                    //    Send(new XLW(null, XLW.EnumActionType.前阻挡位到位));
                    //}
                    //else
                    //{
                    //    flag_DSJ05_3 = false;
                    //}
                    //if (My.PLC.Exist(62, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(62, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new DSJ05(null, DSJ05.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region XLW
                    //if (My.PLC.Exist(72, 3))
                    //{
                    //    Send(new XLW(null, XLW.EnumActionType.前阻挡位转移物料至正阻挡位));
                    //}
                    //if (!flag_XLW_3 && My.PLC.Exist(72, 5, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(72, 7))
                    //{
                    //    flag_XLW_3 = true;
                    //    Send(new XLW(null, XLW.EnumActionType.正阻挡位转移物料至升降机2));
                    //}
                    //else
                    //{
                    //    flag_XLW_3 = false;
                    //}
                    //if (My.PLC.Exist(72, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new XLW(null, XLW.EnumActionType.物料下降));
                    //}
                    //#endregion
                    //#region SJJ01
                    //if (My.PLC.Exist(12, 5, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.前阻挡转移物料至正阻挡位));
                    //}
                    //if (!flag_SJJ01_2 && My.PLC.Exist(14, 4, HNC.HncRegType.REG_TYPE_X) && My.PLC.Exist(12, 7, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    flag_SJJ01_2 = true;
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.正阻挡位转移物料至升降台));
                    //}
                    //else
                    //{
                    //    flag_SJJ01_2 = false;
                    //}
                    //if (My.PLC.Exist(14, 4, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.升降机上升));
                    //}
                    //else
                    //{
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.升降机1转移物料至顶升机1前阻挡位));
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.前阻挡位到位));
                    //}
                    //if (!My.PLC.Exist(14, 3, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new SJJ01(null, SJJ01.EnumActionType.升降机下降));
                    //}
                    //#endregion
                    //#region SJJ02
                    //if (!My.PLC.Exist(74, 3, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new SJJ02(null, SJJ02.EnumActionType.升降机上升));
                    //}
                    //if (My.PLC.Exist(74, 4, HNC.HncRegType.REG_TYPE_X))
                    //{
                    //    Send(new SJJ02(null, SJJ02.EnumActionType.升降机下降));
                    //}
                    //else
                    //{
                    //    Send(new SJJ02(null, SJJ02.EnumActionType.升降台2转移物料值升降机1前阻挡位));
                    //    Send(new SJJ02(null, SJJ02.EnumActionType.前阻挡位转移物料至升降台));
                    //}
                    //#endregion
                }
            }, token);
        }
    }

    abstract class Equipment
    {
        protected RFIDData RFIDData { get; set; }

        /// <summary>
        /// 动作类型
        /// </summary>
        protected int ActionType { get; set; }

        [Flags]
        protected enum EnumActionParameter { 无 = 0, 料盘信息 = 1, 物料信息 = 2, XYZ = 4 }

        /// <summary>
        /// 动作参数
        /// </summary>
        protected EnumActionParameter ActionParameter { get; set; }

        /// <summary>
        /// 获取动作参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string GetActionParameterString(RFIDData data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
            {
                sb.Append(data.Guid.ToString());
                sb.Append("|");
            }
            switch (data.Workpiece)
            {
                case EnumWorkpiece.A:
                    if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
                    {
                        sb.Append("4|");
                    }
                    switch (data.GetProcessSite())
                    {
                        case EnumPSite.S1:
                            sb.Append("A1");
                            break;
                        case EnumPSite.S3:
                            sb.Append("A2");
                            break;
                        case EnumPSite.S2:
                            sb.Append("A3");
                            break;
                        case EnumPSite.None:
                            sb.Append("A4");
                            break;
                        default:
                            break;
                    }
                    break;
                case EnumWorkpiece.B:
                    if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
                    {
                        sb.Append("3|");
                    }
                    if (data.Assemble == EnumAssemble.Successed)
                    {
                        sb.Append("B4");
                    }
                    else
                    {
                        switch (data.GetProcessSite())
                        {
                            case EnumPSite.S1:
                                sb.Append("B1");
                                break;
                            case EnumPSite.S3:
                                sb.Append("B2");
                                break;
                            case EnumPSite.None:
                                sb.Append("B3");
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EnumWorkpiece.C:
                    if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
                    {
                        sb.Append("2|");
                    }
                    if (data.Assemble == EnumAssemble.Successed)
                    {
                        sb.Append("C4");
                    }
                    else
                    {
                        switch (data.GetProcessSite())
                        {
                            case EnumPSite.S1:
                                sb.Append("C1");
                                break;
                            case EnumPSite.S3:
                                sb.Append("C2");
                                break;
                            case EnumPSite.None:
                                sb.Append("C3");
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EnumWorkpiece.D:
                    if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
                    {
                        sb.Append("1|");
                    }
                    switch (data.GetProcessSite())
                    {
                        case EnumPSite.S4:
                            sb.Append("D1");
                            break;
                        case EnumPSite.None:
                            sb.Append("D2");
                            break;
                        default:
                            break;
                    }
                    break;
                case EnumWorkpiece.E:
                    if (ActionParameter.HasFlag(EnumActionParameter.料盘信息))
                    {
                        sb.Append("5|");
                    }
                    if (data.Assemble == EnumAssemble.Successed)
                    {
                        sb.Append("E1");
                    }
                    else
                    {
                        sb.Append("");//仿真未定义
                    }
                    break;
                default:
                    break;
            }
            return sb.ToString();
        }

        protected Equipment(RFIDData data)
        {
            RFIDData = data;
            ActionType = 1;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", this.GetType().Name, ActionType, GetActionParameterString(RFIDData));
        }
    }

    /// <summary>
    /// 码垛机
    /// </summary>
    class MDJ : Equipment
    {
        public MDJ(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.XYZ | EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            码垛机出库动作_仓库软件发送 = 1,
            码垛机入库动作_仓库软件发送
        }
    }

    /// <summary>
    /// 出库线
    /// </summary>
    class CKX : Equipment
    {
        public CKX(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            出库线转移物料至出库检测位 = 1,
            出库检测位转移物料至定位台1
        }
    }

    /// <summary>
    /// 入库线
    /// </summary>
    class RKX : Equipment
    {
        public RKX(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            定位台2转移物料至入库检测位 = 1,
            入库检测位转移物料至入库位
        }
    }

    /// <summary>
    /// 定位台1
    /// </summary>
    class DWT01 : Equipment
    {
        public DWT01(RFIDData data, EnumActionType type = EnumActionType.定位台1转移物料至AGV)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            定位台1转移物料至AGV = 1
        }
    }

    /// <summary>
    /// 定位台4
    /// </summary>
    class DWT04 : Equipment
    {
        public DWT04(RFIDData data, EnumActionType type = EnumActionType.定位台4转移物料至AGV)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            定位台4转移物料至AGV = 1
        }
    }

    /// <summary>
    /// AGV
    /// </summary>
    class AGV : Equipment
    {
        public AGV(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            AGV从定位台1运动至定位台2 = 1,
            AGV从定位台1运动至定位台3,
            AGV从定位台1运动至定位台4,
            AGV从定位台2运动至定位台1,
            AGV从定位台2运动至定位台3,
            AGV从定位台2运动至定位台4,
            AGV从定位台3运动至定位台1,
            AGV从定位台3运动至定位台2,
            AGV从定位台3运动至定位台4,
            AGV从定位台4运动至定位台1,
            AGV从定位台4运动至定位台2,
            AGV从定位台4运动至定位台3,
            AGV转移物料至定位台3
        }
    }

    /// <summary>
    /// 顶升机1
    /// </summary>
    class DSJ01 : Equipment
    {
        public DSJ01(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至顶升机2前阻挡位,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 顶升机2
    /// </summary>
    class DSJ02 : Equipment
    {
        public DSJ02(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至顶升机3前阻挡位,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 顶升机3
    /// </summary>
    class DSJ03 : Equipment
    {
        public DSJ03(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至顶升机4前阻挡位,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 顶升机4
    /// </summary>
    class DSJ04 : Equipment
    {
        public DSJ04(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至顶升机5前阻挡位,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 顶升机5
    /// </summary>
    class DSJ05 : Equipment
    {
        public DSJ05(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至下料位前阻挡位,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 下料位
    /// </summary>
    class XLW : Equipment
    {
        public XLW(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            前阻挡位转移物料至正阻挡位,
            正阻挡位转移物料至升降机2,
            物料顶升,
            物料下降
        }
    }

    /// <summary>
    /// 升降机1
    /// </summary>
    class SJJ01 : Equipment
    {
        public SJJ01(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位到位 = 1,
            正阻挡位转移物料至升降台,
            升降机上升,
            升降机下降,
            前阻挡转移物料至正阻挡位,
            升降机1转移物料至顶升机1前阻挡位
        }
    }

    /// <summary>
    /// 升降机2
    /// </summary>
    class SJJ02 : Equipment
    {
        public SJJ02(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            前阻挡位转移物料至升降台 = 1,
            升降机上升,
            升降机下降,
            升降台2转移物料值升降机1前阻挡位
        }
    }

    /// <summary>
    /// 固定机器人1
    /// </summary>
    class JQR01 : Equipment
    {
        public JQR01(RFIDData data, EnumActionType type = EnumActionType.抓取定位台物料至流水线上料位)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            抓取定位台物料至流水线上料位 = 1
        }
    }

    /// <summary>
    /// 固定机器人2
    /// </summary>
    class JQR02 : Equipment
    {
        public JQR02(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            抓取顶升机5物料至清洗机上料位1 = 1,
            抓取清洗机上料位1物料至清洗机上料位2,
            抓取清洗机上料位2物料至清洗机上料位3,
            抓取清洗机上料位3物料至清洗机上料位4,
            抓取清洗机上料位4物料至装配台,
            抓取底座至装配台,
            抓取A或B或C安装到底座,
            抓取成品至顶升机5,
            更换为普通物料夹具,
            更换为成品夹具
        }
    }

    /// <summary>
    /// 固定机器人3
    /// </summary>
    class JQR03 : Equipment
    {
        public JQR03(RFIDData data, EnumActionType type = EnumActionType.抓取下料位物料至定位台4)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.料盘信息 | EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            抓取下料位物料至定位台4 = 1
        }
    }

    /// <summary>
    /// 移动机器人1
    /// </summary>
    class JQR04 : Equipment
    {
        public JQR04(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            抓取顶升机1物料至车床上料位 = 1,
            抓取车床上料位至顶升机1,
            抓取顶升机2物料至钻攻上料位,
            抓取钻攻上料位物料至顶升机2
        }
    }

    /// <summary>
    /// 移动机器人2
    /// </summary>
    class JQR05 : Equipment
    {
        public JQR05(RFIDData data, EnumActionType type)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            抓取顶升机3物料至翻转机下位 = 1,
            抓取翻转机上位物料至三轴上料位,
            抓取三轴上料位至翻转机上位,
            抓取翻转机下位物料至顶升机3,
            抓取顶升机4物料至五轴上料位,
            抓取五轴上料位物料至顶升机4
        }
    }

    /// <summary>
    /// 装配台
    /// </summary>
    class ZPT : Equipment
    {
        public ZPT(RFIDData data, EnumActionType type = EnumActionType.激光雕刻)
            : base(data)
        {
            ActionType = (int)type;
            ActionParameter = EnumActionParameter.物料信息;
        }

        public enum EnumActionType
        {
            激光雕刻 = 1
        }
    }

}
