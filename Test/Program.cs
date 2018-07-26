using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCADA;
using RFID;
using System.Net.Sockets;
using System.Net;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(DateTime.UtcNow);

            

            //var messages = new List<string>
            //{
            //    "&2018-07-05T05:45:09.116Z,1,CKX,1,123|4|A1#",
            //    "&2018-07-05T05:45:09.116Z,1,AGV,2,123|4|A1#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR01,1,123|4|A1#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR04,1,A1#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR04,2,A2#",
            //    "&2018-07-05T05:45:09.116Z,1,DSJ02,2,123|4|A2#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR04,3,A2#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR04,4,A3#",
            //    "&2018-07-05T05:45:09.116Z,1,DSJ03,4,123|4|A3#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR05,1,A3#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR05,4,A4#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR02,8,A4#",
            //    "&2018-07-05T05:45:09.116Z,1,JQR02,7,A4#",
            //};

            //var tcpClient = new TcpClient();
            //tcpClient.Connect(IPAddress.Parse("58.49.21.230"), 50001);
            //Console.WriteLine("Connected");
            //while (true)
            //{
            //    for (int i = 0; i < messages.Count; i++)
            //    {
            //        tcpClient.Client.Send(Encoding.UTF8.GetBytes(messages[i]));
            //    }
            //    Console.WriteLine("Send OK");
            //    Console.ReadKey(true);
            //}



            Console.ReadKey(true);
        }
    }
}
