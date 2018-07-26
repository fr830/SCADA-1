using RFID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    static class GCodeFile
    {
        public enum EnumFile { 联调试验程序 = 0, 主程序, 加工A料程序, 加工B料程序, 加工C料程序, 加工D料程序 }

        public static IDictionary<EnumPSite, Dictionary<EnumFile, string>> Dict = new Dictionary<EnumPSite, Dictionary<EnumFile, string>>
        {
            { EnumPSite.S1,new Dictionary<EnumFile,string>{
            {EnumFile.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O3344")},
            {EnumFile.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O3355")},
            {EnumFile.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O80")},
            {EnumFile.加工B料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O86")},
            {EnumFile.加工C料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O110")},}},
            {EnumPSite.S2,new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O5566")},
            {EnumFile.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O5577")},
            {EnumFile.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O0001")},
            }},
            {EnumPSite.S3, new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O7788")},
            {EnumFile.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O7799")},
            {EnumFile.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O001")},
            {EnumFile.加工B料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O002")},
            {EnumFile.加工C料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O003")},
            }},
            {EnumPSite.S4,new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O9911")},
            {EnumFile.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O9900")},
            {EnumFile.加工D料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O005-05")},
            }},
        };
    }
}
