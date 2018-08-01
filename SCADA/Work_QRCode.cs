﻿using System;
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
        private static readonly Lazy<Work_QRCode> lazy = new Lazy<Work_QRCode>(() => new Work_QRCode());

        public static Work_QRCode Instance { get { return lazy.Value; } }

        public IPAddress IP { get { return IPAddress.Parse(ConfigurationManager.AppSettings["QRCodeIP"]); } }

        public int Port { get { return int.Parse(ConfigurationManager.AppSettings["QRCodePort"]); } }

        private TcpClient tcpClient = new TcpClient();

        private Work_QRCode()
        {
            My.Work_PLC.Scan += Scan;
            My.Work_PLC.PrintQRCode += PrintQRCode;
            tcpClient.ConnectAsync(IP, Port);
        }

        async void PrintQRCode(object sender, PLCEventArgs e)
        {
            Print();
            await Task.Delay(60 * 1000);
            My.PLC.Set(e.Index, 12);//打印二维码完成
        }

        async void Scan(object sender, PLCEventArgs e)
        {
            await Task.Delay(2 * 1000);
            My.PLC.Set(e.Index, 12);//扫码器扫码完成
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
                //throw;
            }
        }

        public void Print()
        {
            try
            {
                var data = GetData(EnumCommand.Print);
                if (!tcpClient.Connected)
                {
                    tcpClient.Connect(IP, Port);
                }
                tcpClient.Client.Send(data);
            }
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
