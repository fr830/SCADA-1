using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID
{
    /// <summary>
    /// RFID数据格式定义
    /// 从第7个Byte开始
    /// 每2Byte代表一道工序
    /// 1B代表工序位置[1,n]
    /// 2B代表完成结果EnumMachineResult
    /// </summary>
    public static class RFID_Data_Define
    {
        /// <summary>
        /// 数据长度32Byte
        /// </summary>
        public const int DataLength = 32;

        /// <summary>
        /// LOGO(1)
        /// </summary>
        public enum EnumLOGO : byte { HNC = 59 }

        /// <summary>
        /// 位置(2)
        /// </summary>
        public enum EnumLocation : byte { SJZ = 130 }

        /// <summary>
        /// 工件类型(3)
        /// </summary>
        public enum EnumWorkpiece : byte
        {
            /// <summary>
            /// 小圆
            /// </summary>
            A = 1,
            /// <summary>
            /// 中圆
            /// </summary>
            B = 2,
            /// <summary>
            /// 大圆
            /// </summary>
            C = 4,
            /// <summary>
            /// 底座
            /// </summary>
            D = 8,
            /// <summary>
            /// 装配
            /// </summary>
            E = 16
        }

        /// <summary>
        /// 清洗(4)
        /// </summary>
        public enum EnumClean : byte
        {
            /// <summary>
            /// 不需要
            /// </summary>
            Unwanted = 1,
            /// <summary>
            /// 需要
            /// </summary>
            Wanted = 2,
            /// <summary>
            /// 成功
            /// </summary>
            Successed = 4,
            /// <summary>
            /// 失败
            /// </summary>
            Failed = 8
        }

        /// <summary>
        /// 检测(5)
        /// </summary>
        public enum EnumGauge : byte
        {
            /// <summary>
            /// 不需要
            /// </summary>
            Unwanted = 1,
            /// <summary>
            /// 需要
            /// </summary>
            Wanted = 2,
            /// <summary>
            /// 成功
            /// </summary>
            Successed = 4,
            /// <summary>
            /// 失败
            /// </summary>
            Failed = 8
        }

        /// <summary>
        /// 检测结果(6)
        /// </summary>
        public enum EnumGaugeResult : byte
        {
            /// <summary>
            /// 等待中
            /// </summary>
            Waiting = 1,
            /// <summary>
            /// 合格
            /// </summary>
            Qualified = 4,
            /// <summary>
            /// 不合格
            /// </summary>
            Unqualified = 8
        }

        /// <summary>
        /// 当前工序加工结果
        /// </summary>
        public enum EnumMachineResult : byte
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,
            /// <summary>
            /// 等待中
            /// </summary>
            Waiting = 1,
            /// <summary>
            /// 成功
            /// </summary>
            Successed = 4,
            /// <summary>
            /// 失败
            /// </summary>
            Failed = 8
        }

        /// <summary>
        /// 获取RFID数据
        /// </summary>
        /// <param name="workpiece">工件类型</param>
        /// <param name="clean">清洗</param>
        /// <param name="gauge">检测</param>
        /// <param name="gaugeResult">检测结果</param>
        /// <param name="sites">工序</param>
        /// <returns>RFID数据</returns>
        public static byte[] GetDataCustom(
            EnumWorkpiece workpiece = EnumWorkpiece.E,
            EnumClean clean = EnumClean.Wanted,
            EnumGauge gauge = EnumGauge.Wanted,
            EnumGaugeResult gaugeResult = EnumGaugeResult.Waiting,
            params byte[] sites)
        {
            var data = new byte[DataLength];
            data[0] = (byte)EnumLOGO.HNC;
            data[1] = (byte)EnumLocation.SJZ;
            data[2] = (byte)workpiece;
            data[3] = (byte)clean;
            data[4] = (byte)gauge;
            data[5] = (byte)gaugeResult;
            for (int i = 0; i < sites.Length; i++)
            {
                data[i * 2 + 6] = sites[i];
                data[i * 2 + 7] = (byte)EnumMachineResult.Waiting;
            }
            return data;
        }

        /// <summary>
        /// 获取RFID数据
        /// </summary>
        /// <param name="workpiece">工件类型</param>
        /// <returns>RFID数据</returns>
        public static byte[] GetData(EnumWorkpiece workpiece)
        {
            switch (workpiece)
            {
                case EnumWorkpiece.A:
                    return GetDataCustom(workpiece, EnumClean.Wanted, EnumGauge.Wanted, EnumGaugeResult.Waiting, 1, 3, 2, 5);
                case EnumWorkpiece.B:
                    return GetDataCustom(workpiece, EnumClean.Wanted, EnumGauge.Wanted, EnumGaugeResult.Waiting, 1, 3, 5);
                case EnumWorkpiece.C:
                    return GetDataCustom(workpiece, EnumClean.Wanted, EnumGauge.Wanted, EnumGaugeResult.Waiting, 1, 3, 5);
                case EnumWorkpiece.D:
                    return GetDataCustom(workpiece, EnumClean.Wanted, EnumGauge.Wanted, EnumGaugeResult.Waiting, 4, 5);
                case EnumWorkpiece.E:
                    return GetDataCustom(workpiece, EnumClean.Wanted, EnumGauge.Wanted, EnumGaugeResult.Waiting, 5, 6);
                default:
                    return GetDataCustom();
            }
        }

        public static byte GetNextProcess(byte[] data)
        {
            var d = data.Take(6).ToArray();
            for (int i = 0; i < d.Length / 2; i++)
            {
                if (d[i + 1] == 0)
                {
                    return 0;
                }
                else
                {

                }
            }
            return 0;
        }
    }
}
