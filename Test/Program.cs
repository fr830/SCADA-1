using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADA;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = new RFIDItem("192.168.1.133");
            Console.ReadKey(true);
            Console.WriteLine(item.HFReader.ConnectStatus);
            if (item.HFReader.ConnectStatus == Sygole.HFReader.ConnectStatusEnum.CONNECTED)
            {
                var data = Enumerable.Repeat<byte>(0xFF, 32).ToArray();
                item.Write(data);
                Console.WriteLine(item.Read());
            }
            Console.ReadKey(true);
        }
    }
}
