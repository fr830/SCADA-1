using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sygole.HFReader;

namespace SCADA
{
    public class RFIDItem
    {
        public HFReader HFReader { get; private set; }

        public string IPAddress { get; private set; }

        public int Port { get; private set; }

        public RFIDItem(string ip, int port = 3001)
        {
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

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data">最大32B</param>
        /// <returns></returns>
        public bool Write(byte[] data)
        {
            return Status_enum.SUCCESS == HFReader.WriteMBlock(ReaderID, Opcode, UID, StartBlock, BlockCnt, (int)BlockSize, data, Antenna);
        }

        public string Read()
        {
            return Status_enum.SUCCESS == HFReader.ReadMBlock(ReaderID, Opcode, UID, StartBlock, BlockCnt, ref Data, ref DataLen, Antenna) ? ConvertHelper.BytesToHexString(Data, DataLen) : string.Empty;
        }
    }
}
