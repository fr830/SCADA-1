using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QRCode
{
    public class Client
    {
        private static readonly Lazy<Client> lazy = new Lazy<Client>(() => new Client());

        public static Client Instance { get { return lazy.Value; } }

        private TcpClient tcpClient = new TcpClient();

        private Client() { }

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

    }
}
