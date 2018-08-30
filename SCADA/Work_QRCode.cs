using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_QRCode
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Work_QRCode> lazy = new Lazy<Work_QRCode>(() => new Work_QRCode());

        public static Work_QRCode Instance { get { return lazy.Value; } }

        public IPAddress IP { get { return IPAddress.Parse(ConfigurationManager.AppSettings["QRCodeIP"]); } }

        public int Port { get { return int.Parse(ConfigurationManager.AppSettings["QRCodePort"]); } }

        private TcpClient tcpClient = new TcpClient();

        private Work_QRCode()
        {
            My.Work_PLC.Scan += Scan;
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
                        logger.Error("打印二维码设备连接失败");
                    }
                }
                My.Work_PLC.PrintQRCode += PrintQRCode;
            });
        }

        async void PrintQRCode(object sender, PLCEventArgs e)
        {
            logger.Info("启动打印二维码");
            Print();
            await Task.Delay(60 * 1000);
            My.PLC.Set(e.Index, 12);//打印二维码完成
            logger.Info("B{0}.12:打印二维码完成", e.Index);
        }

        async void Scan(object sender, PLCEventArgs e)
        {
            logger.Info("启动扫描二维码");
            await Task.Delay(2 * 1000);
            My.PLC.Set(e.Index, 12);//扫码器扫码完成
            logger.Info("B{0}.12:扫码器扫码完成", e.Index);
        }

        public void Print()
        {
            try
            {
                var data = GetData(EnumCommand.Print);
                tcpClient.Client.Send(data);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public enum EnumCommand : byte
        {
            Query = 0x01,
            Start = 0x02,
            Stop = 0x03,
            Text = 0x05,
            Setting = 0x06,
            Count = 0x07,
            Report = 0x08,
            Print = 0x09
        }

        private byte Head = 0xFF;
        private byte Sequence = 0x00;//序列号，每发送一个报文，序列号自动加1，响应的报文需要带上这个序列号.
        private byte[] Length = { 0x00, 0x00 };//当前包的长度，即五部分总共占用字节数.低字节代表长度的高8bit.高字节代表长度的低8bit.
        private byte Command;//命令定义
        private byte[] Data = new byte[0];//传输数据（分行0x0A0x0A）用两个连接着的字符表示分行，以免会有歧义.
        private byte Checksum = 0x00;//前四部分的字节和

        public enum EnumState : byte { Start = 0x01, Stop = 0x02 }

        private byte[] GetData(EnumCommand cmd, string data = "")
        {
            Sequence++;
            Command = (byte)cmd;
            if (!string.IsNullOrEmpty(data))
            {
                Data = Encoding.UTF8.GetBytes(data);
            }
            int DataLength = Data.Length;
            Length[1] = (byte)(0x06 + DataLength);
            Checksum = (byte)(Sequence + Length[0] + Length[1] + Command - 0x01);
            byte[] result = { Head, Sequence, Length[0], Length[1], Command, Checksum };
            return result;
        }

        public EnumState Query()
        {
            try
            {
                var data = GetData(EnumCommand.Query);
                if (!tcpClient.Connected)
                {
                    return EnumState.Stop;
                }
                tcpClient.Client.Send(data);
                var reply = new byte[data.Length + 1];
                tcpClient.Client.Receive(reply);
                return (EnumState)reply[6];
            }
            catch (Exception)
            {
                return EnumState.Stop;
            }
        }
    }
}
