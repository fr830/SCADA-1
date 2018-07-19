using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADA;
using RFID;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(new string('0', 8));
            Console.WriteLine(buffer.Length);

            //var item = new RFIDReader(EnumPSite.S3, "192.168.1.133");
            //Console.ReadKey(true);
            //Console.WriteLine(item.HFReader.ConnectStatus);
            //if (item.HFReader.ConnectStatus == Sygole.HFReader.ConnectStatusEnum.CONNECTED)
            //{
            //    var data = Enumerable.Repeat<byte>(0xFF, 32).ToArray();
            //    item.WriteBytes(data);
            //    Console.WriteLine(item.ReadHexString());
            //}
            Console.ReadKey(true);
        }
    }
}
