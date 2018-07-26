using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    static class ProcessProgram
    {
        public enum EnumS1File { 联调试验程序 = 0, 主程序, 加工A料程序, 加工B料程序, 加工C料程序 }

        /// <summary>
        /// 车床
        /// </summary>
        public static IDictionary<EnumS1File, string> dictS1 = new Dictionary<EnumS1File, string>
        {
            {EnumS1File.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O3344")},
            {EnumS1File.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O3355")},
            {EnumS1File.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O80")},
            {EnumS1File.加工B料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O86")},
            {EnumS1File.加工C料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","车床","O110")},
        };

        public enum EnumS2File { 联调试验程序 = 0, 主程序, 加工A料程序 }

        /// <summary>
        /// 钻攻中心
        /// </summary>
        public static IDictionary<EnumS2File, string> dictS2 = new Dictionary<EnumS2File, string>
        {
            {EnumS2File.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O5566")},
            {EnumS2File.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O5577")},
            {EnumS2File.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","高速钻工中心","O0001")},
        };

        public enum EnumS3File { 联调试验程序 = 0, 主程序, 加工A料程序, 加工B料程序, 加工C料程序 }

        /// <summary>
        /// 三轴
        /// </summary>
        public static IDictionary<EnumS3File, string> dictS3 = new Dictionary<EnumS3File, string>
        {
            {EnumS3File.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O7788")},
            {EnumS3File.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O7799")},
            {EnumS3File.加工A料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O001")},
            {EnumS3File.加工B料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O002")},
            {EnumS3File.加工C料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","三轴加工中心","O003")},
        };

        public enum EnumS4File { 联调试验程序 = 0, 主程序, 加工D料程序 }

        /// <summary>
        /// 五轴
        /// </summary>
        public static IDictionary<EnumS4File, string> dictS4 = new Dictionary<EnumS4File, string>
        {
            {EnumS4File.联调试验程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O9911")},
            {EnumS4File.主程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O9900")},
            {EnumS4File.加工D料程序,Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"加工程序","五轴加工中心","O005-05")},
        };
    }
}
