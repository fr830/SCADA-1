using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SCADA
{
    class Work_Vision
    {
        private static readonly Lazy<Work_Vision> lazy = new Lazy<Work_Vision>(() => new Work_Vision());

        public static Work_Vision Instance { get { return lazy.Value; } }

        public TcpListener TcpListener { get; private set; }

        public TcpClient TcpClient { get; private set; }

        public RFID.RFIDData RFIDData { get; set; }

        private Work_Vision()
        {
            //ListenerConnect();
            ClientConnect();
        }

        private async void ClientConnect()
        {
            TcpClient = new TcpClient();
            await TcpClient.ConnectAsync(IPAddress.Parse("192.168.1.144"), 41160);
            My.Work_PLC.Camera += Camera_IsRequested;
        }

        private async void ListenerConnect()
        {
            TcpListener = new TcpListener(IPAddress.Any, 41160);
            TcpListener.Start();
            TcpClient = await TcpListener.AcceptTcpClientAsync();
            My.Work_PLC.Camera += Camera_IsRequested;
        }

        public void Camera_IsRequested(object sender, PLCEventArgs e)
        {
            byte[] buffer = new byte[16];
            if (TcpClient == null || !TcpClient.Connected)
            {
                return;
            }
            TcpClient.Client.Send(Encoding.UTF8.GetBytes("1"));
            int count = TcpClient.Client.Receive(buffer);
            var text = Encoding.UTF8.GetString(buffer);
            My.PLC.BitSet(e.Index, 1);
            if (RFIDData == null || count != 8)
            {
                return;
            }
            switch (RFIDData.Workpiece)
            {
                case RFID.EnumWorkpiece.A:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else
                    {
                        My.PLC.BitSet(e.Index, 3);
                    }
                    break;
                case RFID.EnumWorkpiece.B:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else
                    {
                        My.PLC.BitSet(e.Index, 3);
                    }
                    break;
                case RFID.EnumWorkpiece.C:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else
                    {
                        My.PLC.BitSet(e.Index, 3);
                    }
                    break;
                case RFID.EnumWorkpiece.D:
                    if (RFIDData.IsRough && text == Rough_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else if (!RFIDData.IsRough && text == Semi_A)
                    {
                        My.PLC.BitSet(e.Index, 2);
                    }
                    else
                    {
                        My.PLC.BitSet(e.Index, 3);
                    }
                    break;
                case RFID.EnumWorkpiece.E:
                    break;
                default:
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
