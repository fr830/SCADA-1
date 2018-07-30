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

        private static string GetFilePath(string folder, string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "加工程序", folder, filename);
        }

        private static string GetFilePath(EnumPSite site, string filename)
        {
            switch (site)
            {
                case EnumPSite.S1:
                    return GetFilePath("车床", filename);
                case EnumPSite.S2:
                    return GetFilePath("高速钻工中心", filename);
                case EnumPSite.S3:
                    return GetFilePath("三轴加工中心", filename);
                case EnumPSite.S4:
                    return GetFilePath("五轴加工中心", filename);
                default:
                    return string.Empty;
            }
        }

        public static IDictionary<EnumPSite, Dictionary<EnumFile, string>> Dict = new Dictionary<EnumPSite, Dictionary<EnumFile, string>>
        {
            {EnumPSite.S1,new Dictionary<EnumFile,string>
            {
            {EnumFile.联调试验程序,GetFilePath(EnumPSite.S1,"O3344")},
            {EnumFile.主程序,GetFilePath(EnumPSite.S1,"O3355")},
            {EnumFile.加工A料程序,GetFilePath(EnumPSite.S1,"O80")},
            {EnumFile.加工B料程序,GetFilePath(EnumPSite.S1,"O86")},
            {EnumFile.加工C料程序,GetFilePath(EnumPSite.S1,"O110")},
            }},
            {EnumPSite.S2,new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,GetFilePath(EnumPSite.S2,"O5566")},
            {EnumFile.主程序,GetFilePath(EnumPSite.S2,"O5577")},
            {EnumFile.加工A料程序,GetFilePath(EnumPSite.S2,"O0001")},
            }},
            {EnumPSite.S3, new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,GetFilePath(EnumPSite.S3,"O7788")},
            {EnumFile.主程序,GetFilePath(EnumPSite.S3,"O7799")},
            {EnumFile.加工A料程序,GetFilePath(EnumPSite.S3,"O001")},
            {EnumFile.加工B料程序,GetFilePath(EnumPSite.S3,"O002")},
            {EnumFile.加工C料程序,GetFilePath(EnumPSite.S3,"O003")},
            }},
            {EnumPSite.S4,new Dictionary<EnumFile, string>
            {
            {EnumFile.联调试验程序,GetFilePath(EnumPSite.S4,"O9911")},
            {EnumFile.主程序,GetFilePath(EnumPSite.S4,"O9900")},
            {EnumFile.加工D料程序,GetFilePath(EnumPSite.S4,"O005-05")},
            }},
        };
    }
}
