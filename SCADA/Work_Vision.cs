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
        private static readonly Lazy<Work_Vision> lazy = new Lazy<Work_Vision>(() => new Work_Vision());

        public static Work_Vision Instance { get { return lazy.Value; } }

        public IPAddress IP { get { return IPAddress.Parse(ConfigurationManager.AppSettings["VisionIP"]); } }

        public int Port { get { return int.Parse(ConfigurationManager.AppSettings["VisionPort"]); } }

        private TcpClient tcpClient = new TcpClient();

        public RFID.RFIDData RFIDData { get; set; }

        private Work_Vision()
        {
            ClientConnectAsync();
            My.Work_PLC.Photograph += Photograph;
        }

        private async void ClientConnectAsync()
        {
            await Task.Run(async () =>
            {
                while (!tcpClient.Connected)
                {
                    try
                    {
                        await tcpClient.ConnectAsync(IP, Port);
                    }
                    catch (Exception)
                    {
                        //TODO提示用户连接失败
                        //throw;
                    }
                    await Task.Delay(2000);
                }
            });
        }

        public async void Photograph(object sender, PLCEventArgs e)
        {
            if (!tcpClient.Connected)
            {
                await tcpClient.ConnectAsync(IP, Port);
                if (!tcpClient.Connected)
                {
                    //TODO提示连接失败
                    return;
                }
            }
            if (RFIDData == null)
            {
                //TODO提示RFID读取失败
                return;
            }
            byte[] buffer = new byte[16];
            try
            {
                tcpClient.Client.Send(Encoding.UTF8.GetBytes("1"));
                tcpClient.Client.Receive(buffer);
            }
            catch (Exception)
            {
                //TODO提示用户拍照失败
                return;
                //throw;
            }
            var text = Encoding.UTF8.GetString(buffer);
            My.PLC.Set(e.Index, 1);//拍照完成
            switch (RFIDData.Workpiece)
            {
                case RFID.EnumWorkpiece.A:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else
                    {
                        My.PLC.Set(e.Index, 3);//工件不匹配
                    }
                    break;
                case RFID.EnumWorkpiece.B:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else
                    {
                        My.PLC.Set(e.Index, 3);//工件不匹配
                    }
                    break;
                case RFID.EnumWorkpiece.C:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else
                    {
                        My.PLC.Set(e.Index, 3);//工件不匹配
                    }
                    break;
                case RFID.EnumWorkpiece.D:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.Set(e.Index, 2);//工件匹配
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
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
        }

        /// <summary>
        /// 毛坯小圆
        /// </summary>
        public const string Rough_A = "0,0,0,0,1,0,0,0,";
        /// <summary>
        /// 毛坯中圆
        /// </summary>
        public const string Rough_B = "0,0,1,0,0,0,0,0,";

        /// <summary>
        /// 毛坯大圆
        /// </summary>
        public const string Rough_C = "1,0,0,0,0,0,0,0,";

        /// <summary>
        /// 毛坯底座
        /// </summary>
        public const string Rough_D = "0,0,0,0,0,0,1,0,";

        /// <summary>
        /// 半成品小圆
        /// </summary>
        public const string Semi_A = "0,0,0,0,0,1,0,0,";

        /// <summary>
        /// 半成品中圆
        /// </summary>
        public const string Semi_B = "0,0,0,1,0,0,0,0,";

        /// <summary>
        /// 半成品大圆
        /// </summary>
        public const string Semi_C = "1,1,0,0,0,0,0,0,";

        /// <summary>
        /// 半成品底座
        /// </summary>
        public const string Semi_D = "0,0,0,0,0,0,0,1,";

    }
}
