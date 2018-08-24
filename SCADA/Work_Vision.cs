using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

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

        public RFID.RFIDData RFIDData { get; set; }

        private Work_Vision()
        {
#if !OFFLINE
            ClientConnectAsync();
#endif
        }

        private async void ClientConnectAsync()
        {
            await Task.Run(() =>
            {
                while (!tcpClient.Connected)
                {
                    try
                    {
                        tcpClient.Connect(IP, Port);
                    }
                    catch (Exception)
                    {
                        var message = "出库视觉相机连接失败";
                        logger.Error(message);
                    }
                }
                My.Work_PLC.Photograph += Photograph;
            });
        }

        public void Photograph(object sender, PLCEventArgs e)
        {
            try
            {
                if (RFIDData == null)
                {
                    throw new Exception("出库口RFID信息获取失败，无法进行比对！");
                }
                byte[] buffer = new byte[16];
                tcpClient.Client.Send(Encoding.UTF8.GetBytes("1"));
                tcpClient.ReceiveTimeout = 5000;
                tcpClient.Client.Receive(buffer);
                var text = Encoding.UTF8.GetString(buffer);
                My.PLC.Set(e.Index, 1);//拍照完成
                logger.Info("B{0}.1:拍照完成", e.Index);
                #region PLC
                switch (RFIDData.Workpiece)
                {
                    case RFID.EnumWorkpiece.A:
                        if (RFIDData.IsRough && text == AR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else if (!RFIDData.IsRough && text == AS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                        }
                        break;
                    case RFID.EnumWorkpiece.B:
                        if (RFIDData.IsRough && text == BR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else if (!RFIDData.IsRough && text == BS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                        }
                        break;
                    case RFID.EnumWorkpiece.C:
                        if (RFIDData.IsRough && text == CR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else if (!RFIDData.IsRough && text == CS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                        }
                        break;
                    case RFID.EnumWorkpiece.D:
                        if (RFIDData.IsRough && text == DR)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else if (!RFIDData.IsRough && text == DS)
                        {
                            My.PLC.Set(e.Index, 2);//工件匹配
                        }
                        else
                        {
                            My.PLC.Set(e.Index, 3);//工件不匹配
                        }
                        break;
                    case RFID.EnumWorkpiece.E:
                        My.PLC.Set(e.Index, 2);//工件匹配
                        break;
                    default:
                        My.PLC.Set(e.Index, 3);//工件不匹配
                        break;
                }
                #endregion
                RFIDData = null;
            }
            catch (Exception ex)
            {
                logger.Error("出库相机拍照失败");
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 毛坯小圆
        /// </summary>
        public string AR { get { return ConfigurationManager.AppSettings["VisionAR"]; } }
        /// <summary>
        /// 毛坯中圆
        /// </summary>
        public string BR { get { return ConfigurationManager.AppSettings["VisionBR"]; } }

        /// <summary>
        /// 毛坯大圆
        /// </summary>
        public string CR { get { return ConfigurationManager.AppSettings["VisionCR"]; } }

        /// <summary>
        /// 毛坯底座
        /// </summary>
        public string DR { get { return ConfigurationManager.AppSettings["VisionDR"]; } }

        /// <summary>
        /// 半成品小圆
        /// </summary>
        public string AS { get { return ConfigurationManager.AppSettings["VisionAS"]; } }

        /// <summary>
        /// 半成品中圆
        /// </summary>
        public string BS { get { return ConfigurationManager.AppSettings["VisionBS"]; } }

        /// <summary>
        /// 半成品大圆
        /// </summary>
        public string CS { get { return ConfigurationManager.AppSettings["VisionCS"]; } }

        /// <summary>
        /// 半成品底座
        /// </summary>
        public string DS { get { return ConfigurationManager.AppSettings["VisionDS"]; } }

    }
}
