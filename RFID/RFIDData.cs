using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID
{
    #region Enums
    /// <summary>
    /// LOGO(1)
    /// </summary>
    public enum EnumLOGO : byte { HNC = 59 }

    /// <summary>
    /// 预留定义(2)
    /// </summary>
    public enum EnumObligate : byte { None = byte.MaxValue }

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
        Unwanted = 0,
        /// <summary>
        /// 需要
        /// </summary>
        Wanted = 1,
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
        Unwanted = 0,
        /// <summary>
        /// 需要
        /// </summary>
        Wanted = 1,
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
    /// 装配(6)
    /// </summary>
    public enum EnumAssemble : byte
    {
        /// <summary>
        /// 不需要
        /// </summary>
        Unwanted = 0,
        /// <summary>
        /// 需要
        /// </summary>
        Wanted = 1,
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
    /// 位置
    /// </summary>
    public enum EnumPSite : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 1号顶升机构：车床
        /// </summary>
        S1,
        /// <summary>
        /// 2号顶升机构：钻攻中心
        /// </summary>
        S2,
        /// <summary>
        /// 3号顶升机构：铣床，三轴加工中心（850B）
        /// </summary>
        S3,
        /// <summary>
        /// 4号顶升机构：五轴加工中心
        /// </summary>
        S4,
        /// <summary>
        /// 5号顶升机构：装配台
        /// </summary>
        S5_Assemble,
        /// <summary>
        /// 6号顶升机构：定位台
        /// </summary>
        S6_Alignment,
        /// <summary>
        /// 7号WMS入库皮带线
        /// </summary>
        S7_Up,
        /// <summary>
        /// 8号WMS出库皮带线
        /// </summary>
        S8_Down,
        /// <summary>
        /// 9号WMS手动皮带线
        /// </summary>
        S9_Manual
    }

    /// <summary>
    /// 当前工序加工结果
    /// </summary>
    public enum EnumPResult : byte
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
    #endregion

    /// <summary>
    /// 加工数据
    /// </summary>
    public class ProcessData
    {
        public EnumPSite Site { get; private set; }

        public EnumPResult Result { get; set; }

        public ProcessData(EnumPSite site, EnumPResult result = EnumPResult.Waiting)
        {
            Site = site;
            Result = result;
        }
    }

    /// <summary>
    /// RFID数据格式定义
    /// 从第23个Byte开始
    /// 每2Byte代表一道工序
    /// 1B代表工序位置EnumPSite
    /// 2B代表工序结果EnumPResult
    /// </summary>
    public class RFIDData
    {
        public const int DataLength = 32;
        public Guid Guid { get; private set; }
        public EnumLOGO LOGO { get; private set; }
        public EnumObligate Obligate { get; private set; }
        public EnumWorkpiece Workpiece { get; private set; }
        public EnumClean Clean { get; set; }
        public EnumGauge Gauge { get; set; }
        public EnumAssemble Assemble { get; set; }
        public IList<ProcessData> ProcessDataList { get; set; }

        /// <summary>
        /// 是否为毛坯
        /// </summary>
        public bool IsRough { get { return GetProcessSite() != EnumPSite.None; } }

        private RFIDData(Guid guid)
        {
            Guid = guid;
            LOGO = EnumLOGO.HNC;
            Obligate = EnumObligate.None;
            ProcessDataList = new List<ProcessData>();
        }

        public EnumPSite GetProcessSite()
        {
            if (ProcessDataList == null || ProcessDataList.Count == 0)
            {
                return EnumPSite.None;
            }
            for (int i = 0; i < ProcessDataList.Count; i++)
            {
                if (ProcessDataList[i].Result == EnumPResult.Waiting)
                {
                    return ProcessDataList[i].Site;
                }
            }
            return EnumPSite.None;
        }

        public bool SetProcessResult(EnumPResult result = EnumPResult.Successed)
        {
            if (ProcessDataList == null || ProcessDataList.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < ProcessDataList.Count; i++)
            {
                if (ProcessDataList[i].Result == EnumPResult.Waiting)
                {
                    ProcessDataList[i].Result = result;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取默认RFID数据
        /// </summary>
        /// <param name="guid">工件编号</param>
        /// <param name="workpiece">工件类型</param>
        /// <param name="clean">清洗</param>
        /// <param name="gauge">检测</param>
        /// <param name="assemble">装配</param>
        /// <returns>RFIDData对象</returns>
        public static RFIDData GetDefaut(Guid guid, EnumWorkpiece workpiece, EnumClean clean = EnumClean.Wanted, EnumGauge gauge = EnumGauge.Wanted, EnumAssemble assemble = EnumAssemble.Wanted)
        {
            var data = new RFIDData(guid);
            data.Workpiece = workpiece;
            data.Clean = clean;
            data.Gauge = gauge;
            data.Assemble = assemble;
            switch (workpiece)
            {
                case EnumWorkpiece.A:
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S1));
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S3));
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S2));
                    break;
                case EnumWorkpiece.B:
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S1));
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S3));
                    break;
                case EnumWorkpiece.C:
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S1));
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S3));
                    break;
                case EnumWorkpiece.D:
                    data.ProcessDataList.Add(new ProcessData(EnumPSite.S4));
                    break;
                case EnumWorkpiece.E:
                    break;
                default:
                    break;
            }
            return data;
        }

        public byte[] Serialize()
        {
            return RFIDData.Serialize(this);
        }

        public static byte[] Serialize(RFIDData d)
        {
            if (d == null) return null;
            var data = new byte[DataLength];
            var guidBytes = d.Guid.ToByteArray();
            for (int i = 0; i < guidBytes.Length; i++)
            {
                data[i] = guidBytes[i];
            }
            data[16] = (byte)d.LOGO;
            data[17] = (byte)d.Obligate;
            data[18] = (byte)d.Workpiece;
            data[19] = (byte)d.Clean;
            data[20] = (byte)d.Gauge;
            data[21] = (byte)d.Assemble;
            for (int i = 0; i < d.ProcessDataList.Count; i++)
            {
                data[i * 2 + 22] = (byte)d.ProcessDataList[i].Site;
                data[i * 2 + 23] = (byte)d.ProcessDataList[i].Result;
            }
            return data;
        }

        public static RFIDData Deserialize(byte[] data)
        {
            if (data == null || data.Length != DataLength)
            {
                return null;
            }
            var r = new RFIDData(new Guid(data.Take(16).ToArray()));
            r.LOGO = (EnumLOGO)data[16];
            r.Obligate = (EnumObligate)data[17];
            r.Workpiece = (EnumWorkpiece)data[18];
            r.Clean = (EnumClean)data[19];
            r.Gauge = (EnumGauge)data[20];
            r.Assemble = (EnumAssemble)data[21];
            for (int i = 0; i < data.Length; i += 2)
            {
                if (data[i + 22] == 0)
                {
                    break;
                }
                else
                {
                    var processData = new ProcessData((EnumPSite)data[i + 22], (EnumPResult)data[i + 23]);
                    r.ProcessDataList.Add(processData);
                }
            }
            return r;
        }
    }

}
