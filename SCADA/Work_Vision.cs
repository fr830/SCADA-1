using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using RFID;

namespace SCADA
{
    class Work_Vision
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_Vision> lazy = new Lazy<Work_Vision>(() => new Work_Vision());

        public static Work_Vision Instance { get { return lazy.Value; } }

        public IPAddress IP { get { return IPAddress.Parse(ConfigurationManager.AppSettings["VisionIP"]); } }

        public int Port { get { return int.Parse(ConfigurationManager.AppSettings["VisionPort"]); } }

        private TcpClient tcpClient = new TcpClient();

        public Queue<RFIDData> DataQueue { get; set; }

        private Work_Vision()
        {
            DataQueue = new Queue<RFIDData>();
#if !OFFLINE
            ClientConnectAsync();
#endif
        }

        private async void ClientConnectAsync()
        {
            await Task.Run(async () =>
            {
                while (!tcpClient.Connected)
                {
                    try
                    {
                        tcpClient.Close();
                        tcpClient = new TcpClient();
                        await tcpClient.ConnectAsync(IP, Port);
                    }
                    catch (Exception)
                    {
                        logger.Error("出库视觉相机连接失败");
                    }
                }
                My.Work_PLC.Photograph += Photograph;
            });
        }

        public void Photograph(object sender, PLCEventArgs e)
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    My.Work_PLC.Photograph -= Photograph;
                    ClientConnectAsync();
                    throw new Exception("出库视觉相机连接失败，正在尝试重连");
                }
                if (DataQueue.Count < 1)
                {
                    logger.Error("出库口无RFID信息记录，无法进行比对");
                    logger.Error("请手动将料盘放回出库口RFID读写位，并使用故障恢复里的<获取RFID信息>与<请求出库>的功能");
                    return;
                }
                var data = DataQueue.Peek();
                byte[] buffer = new byte[16];
                tcpClient.Client.Send(Encoding.UTF8.GetBytes("1"));
                tcpClient.ReceiveTimeout = 5000;
                tcpClient.Client.Receive(buffer);
                var text = Encoding.UTF8.GetString(buffer);
                My.PLC.Set(e.Index, 1);//拍照完成
                logger.Info("B{0}.1:拍照完成", e.Index);
                #region PLC
                if (data.Workpiece == EnumWorkpiece.E)
                {
                    logger.Info("RFID数据:{0}{1}", data.Assemble == EnumAssemble.Successed ? "成品" : "空盘", Enum.GetName(typeof(EnumWorkpiece), data.Workpiece));
                }
                else
                {
                    logger.Info("RFID数据:{0}{1}", data.IsRough ? "毛坯" : "半成品", Enum.GetName(typeof(EnumWorkpiece), data.Workpiece));
                }
                logger.Info("Vision数据:{0}|{1}", text, VisionDict.ContainsKey(text) ? VisionDict[text] : string.Empty);
                switch (data.Workpiece)
                {
                    case EnumWorkpiece.A:
                        if (data.IsRough && text == AR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else if (!data.IsRough && text == AS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                            logger.Info("B{0}.3:工件不匹配", e.Index);
                        }
                        break;
                    case EnumWorkpiece.B:
                        if (data.IsRough && text == BR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else if (!data.IsRough && text == BS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                            logger.Info("B{0}.3:工件不匹配", e.Index);
                        }
                        break;
                    case EnumWorkpiece.C:
                        if (data.IsRough && text == CR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else if (!data.IsRough && text == CS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                            logger.Info("B{0}.3:工件不匹配", e.Index);
                        }
                        break;
                    case EnumWorkpiece.D:
                        if (data.IsRough && text == DR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else if (!data.IsRough && text == DS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                            logger.Info("B{0}.2:工件匹配", e.Index);
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                            logger.Info("B{0}.3:工件不匹配", e.Index);
                        }
                        break;
                    case EnumWorkpiece.E:
                        My.PLC.Set(e.Index, 2);//工件匹配
                        logger.Info("B{0}.2:工件匹配", e.Index);
                        break;
                    default:
                        My.PLC.Set(e.Index, 3);//工件不匹配
                        logger.Info("B{0}.3:工件不匹配", e.Index);
                        break;
                }
                #endregion
                DataQueue.Dequeue();
            }
            catch (Exception ex)
            {
                logger.Error("出库相机拍照失败");
                logger.Error(ex);
            }
        }

        private static IReadOnlyDictionary<string, string> VisionDict = new Dictionary<string, string>
        {
            {AR,"毛坯A"},{AS,"半成品A"},
            {BR,"毛坯B"},{BS,"半成品B"},
            {CR,"毛坯C"},{CS,"半成品C"},
            {DR,"毛坯D"},{DS,"半成品D"},
        };

        /// <summary>
        /// 毛坯小圆
        /// </summary>
        public static string AR { get { return ConfigurationManager.AppSettings["VisionAR"]; } }

        /// <summary>
        /// 毛坯中圆
        /// </summary>
        public static string BR { get { return ConfigurationManager.AppSettings["VisionBR"]; } }

        /// <summary>
        /// 毛坯大圆
        /// </summary>
        public static string CR { get { return ConfigurationManager.AppSettings["VisionCR"]; } }

        /// <summary>
        /// 毛坯底座
        /// </summary>
        public static string DR { get { return ConfigurationManager.AppSettings["VisionDR"]; } }

        /// <summary>
        /// 半成品小圆
        /// </summary>
        public static string AS { get { return ConfigurationManager.AppSettings["VisionAS"]; } }

        /// <summary>
        /// 半成品中圆
        /// </summary>
        public static string BS { get { return ConfigurationManager.AppSettings["VisionBS"]; } }

        /// <summary>
        /// 半成品大圆
        /// </summary>
        public static string CS { get { return ConfigurationManager.AppSettings["VisionCS"]; } }

        /// <summary>
        /// 半成品底座
        /// </summary>
        public static string DS { get { return ConfigurationManager.AppSettings["VisionDS"]; } }

    }
}
