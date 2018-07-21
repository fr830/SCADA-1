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
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss:fffZ"));

            Console.WriteLine(new CKX(RFIDData.GetDefaut(Guid.NewGuid(), EnumWorkpiece.A)));

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
