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
            TestAsync();
            Console.WriteLine(DateTime.Now);


            Console.ReadKey(true);
        }

        static async Task TestAsync()
        {
            await Task.Run(async () => await Task.Delay(2000));
        }
    }
}
