using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sygole.HFReader;

namespace RFID
{
    /// <summary>
    /// RFID读写器
    /// </summary>
    public class RFIDReader
    {
        public EnumPSite Site { get; private set; }

        public HFReader HFReader { get; private set; }

        public string IPAddress { get; private set; }

        public int Port { get; private set; }

        public RFIDReader(EnumPSite site, string ip, int port = 3001)
        {
            Site = site;
            IPAddress = ip;
            Port = port;
            HFReader = new HFReader();
            HFReader.CommEvent = new CommEventCS();
            ConnectAsync();
        }

        public async Task<bool> ConnectAsync()
        {
            return await Task.Run(() => HFReader.Connect(IPAddress, Port));
        }

        public async Task DisconnectAsync()
        {
            await Task.Run(() => HFReader.DisConnect());
        }

        byte ReaderID = 0;
        Opcode_enum Opcode = Opcode_enum.NON_ADDRESS_MODE;
        byte[] UID = new byte[8];
        byte[] Data = new byte[64];
        Antenna_enum Antenna = Antenna_enum.ANT_1;
        byte StartBlock = 0;
        byte BlockCnt = 8;
        byte DataLen = 0;
        byte BlockSize = 4;

        public bool Write(RFIDData data)
        {
            return WriteBytes(data.Serialize());
        }

        public RFIDData Read()
        {
            return RFIDData.Deserialize(ReadBytes());
        }

        /// <summary>
        /// 写数据（32 Byte）
        /// </summary>
        /// <param name="data">32 Byte</param>
        /// <returns></returns>
        public bool WriteBytes(byte[] data)
        {
            return Status_enum.SUCCESS == HFReader.WriteMBlock(ReaderID, Opcode, UID, StartBlock, BlockCnt, (int)BlockSize, data, Antenna);
        }

        public byte[] ReadBytes()
        {
            return Status_enum.SUCCESS == HFReader.ReadMBlock(ReaderID, Opcode, UID, StartBlock, BlockCnt, ref Data, ref DataLen, Antenna) ? Data.Take(DataLen).ToArray() : null;
        }

        public string ReadHexString()
        {
            return Status_enum.SUCCESS == HFReader.ReadMBlock(ReaderID, Opcode, UID, StartBlock, BlockCnt, ref Data, ref DataLen, Antenna) ? BytesToHexString(Data, DataLen) : string.Empty;
        }

        /// <summary>
        /// 初始化RFID信息
        /// </summary>
        /// <param name="workpiece">工件类型</param>
        /// <returns>是否成功</returns>
        public bool Init(Guid guid, EnumWorkpiece workpiece)
        {
            var data = RFIDData.GetDefaut(guid, workpiece).Serialize();
            return WriteBytes(data);
        }

        public static string BytesToHexString(byte[] data, int length = -1, int start = 0)
        {
            if (length < 0)
            {
                length = data.Length;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < start + length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 请求读
        /// </summary>
        public event EventHandler Read_IsRequested;

        public void OnRead_IsRequested()
        {
            if (Read_IsRequested != null)
            {
                Read_IsRequested(this, new EventArgs());
            }
        }

        /// <summary>
        /// 请求写（加工成功）
        /// </summary>
        public event EventHandler Write_Process_Success_IsRequested;

        public void OnWrite_Process_Success_IsRequested()
        {
            if (Write_Process_Success_IsRequested != null)
            {
                Write_Process_Success_IsRequested(this, new EventArgs());
            }
        }

        /// <summary>
        /// 请求写（加工失败）
        /// </summary>
        public event EventHandler Write_Process_Failure_IsRequested;

        public void OnWrite_Process_Failure_IsRequested()
        {
            if (Write_Process_Failure_IsRequested != null)
            {
                Write_Process_Failure_IsRequested(this, new EventArgs());
            }
        }

        /// <summary>
        /// 请求打印二维码
        /// </summary>
        public event EventHandler Print_QR_Code_IsRequested;

        public void OnPrint_QR_Code_IsRequested()
        {
            if (Print_QR_Code_IsRequested != null)
            {
                Print_QR_Code_IsRequested(this, new EventArgs());
            }
        }

    }
}
