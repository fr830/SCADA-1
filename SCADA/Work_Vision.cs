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

        public TcpListener TcpListener { get; private set; }

        public TcpClient TcpClient { get; private set; }

        public RFID.RFIDData RFIDData { get; set; }

        private Work_Vision()
        {
            //ListenerConnect();
            ClientConnectAsync();
            My.Work_PLC.Photograph += Photograph;
        }

        private async void ClientConnectAsync()
        {
            TcpClient = new TcpClient();
            await Task.Run(async () =>
            {
                while (!TcpClient.Connected)
                {
                    try
                    {
                        TcpClient.Connect(IP, Port);
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

        private async void ListenerConnect()
        {
            TcpListener = new TcpListener(IPAddress.Any, 41160);
            TcpListener.Start();
            TcpClient = await TcpListener.AcceptTcpClientAsync();
            My.Work_PLC.Photograph += Photograph;
        }

        public void Photograph(object sender, PLCEventArgs e)
        {
            if (TcpClient == null || !TcpClient.Connected)
            {
                return;
            }
            byte[] buffer = new byte[16];
            TcpClient.Client.Send(Encoding.UTF8.GetBytes("1"));
            int count = TcpClient.Client.Receive(buffer);
            var text = Encoding.UTF8.GetString(buffer);
            My.PLC.Set(e.Index, 1);
            if (RFIDData == null || count != 8)
            {
                return;
            }
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
                    My.PLC.Set(e.Index, 2);//工件匹配
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
