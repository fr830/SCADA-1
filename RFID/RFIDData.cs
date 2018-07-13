using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID
{
    /// <summary>
    /// LOGO(1)
    /// </summary>
    public enum EnumLOGO : byte { HNC = 59 }

    /// <summary>
    /// 编号(2)
    /// </summary>
    public enum EnumNo : byte { No0 = 0, No1 = 1, No2, No3, No4, No5, No6, No7, No8, No9, No10 }

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
        None = 0,
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
    public enum EnumProcessResult : byte
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
    /// RFID数据格式定义
    /// 从第7个Byte开始
    /// 每2Byte代表一道工序
    /// 1B代表工序位置[1,n]
    /// 2B代表完成结果EnumMachineResult
    /// </summary>
    public class RFIDData
    {
        public const int DataLength = 32;
        public EnumLOGO LOGO { get; private set; }
        public EnumNo No { get; private set; }
        public EnumWorkpiece Workpiece { get; private set; }
        public EnumClean Clean { get; private set; }
        public EnumGauge Gauge { get; private set; }
        public EnumGaugeResult GaugeResult { get; private set; }
        public IList<KeyValuePair<byte, EnumProcessResult>> Processes { get; private set; }

        private RFIDData()
        {
            LOGO = EnumLOGO.HNC;
            Processes = new List<KeyValuePair<byte, EnumProcessResult>>();
        }

        /// <summary>
        /// 获取RFID数据
        /// </summary>
        /// <param name="no"></param>
        /// <param name="workpiece">工件类型</param>
        /// <param name="clean">清洗</param>
        /// <param name="gauge">检测</param>
        /// <param name="gaugeResult">检测结果</param>
        /// <param name="sites">工序</param>
        public RFIDData(EnumNo no, EnumWorkpiece workpiece, EnumClean clean, EnumGauge gauge, EnumGaugeResult gaugeResult, IList<byte> sites = null)
        {
            LOGO = EnumLOGO.HNC;
            No = no;
            Workpiece = workpiece;
            Clean = clean;
            Gauge = gauge;
            GaugeResult = gaugeResult;
            if (sites == null)
            {
                sites = new List<byte>();
            }
            Processes = ToProcesses(sites);
        }

        public byte GetProcessSite()
        {
            for (int i = 0; i < Processes.Count; i++)
            {
                if (Processes[i].Value == EnumProcessResult.Waiting)
                {
                    return Processes[i].Key;
                }
            }
            return 0;
        }

        public bool SetProcessResult(EnumProcessResult result = EnumProcessResult.Successed)
        {
            for (int i = 0; i < Processes.Count; i++)
            {
                if (Processes[i].Value == EnumProcessResult.Waiting)
                {
                    Processes[i] = new KeyValuePair<byte, EnumProcessResult>(Processes[i].Key, result);
                    return true;
                }
            }
            return false;
        }

        private static IList<KeyValuePair<byte, EnumProcessResult>> ToProcesses(IList<byte> sites)
        {
            return sites.Select(s => new KeyValuePair<byte, EnumProcessResult>(s, EnumProcessResult.Waiting)).ToList();
        }

        private static IList<KeyValuePair<byte, EnumProcessResult>> ToProcesses(params byte[] sites)
        {
            return ToProcesses(sites.ToList());
        }

        /// <summary>
        /// 获取RFID数据
        /// </summary>
        /// <param name="no">工件编号</param>
        /// <param name="workpiece">工件类型</param>
        /// <returns>RFID数据</returns>
        public static RFIDData GetDefaut(EnumNo no, EnumWorkpiece workpiece)
        {
            var data = new RFIDData();
            data.No = no;
            data.Workpiece = workpiece;
            switch (workpiece)
            {
                case EnumWorkpiece.A:
                    data.Clean = EnumClean.Wanted;
                    data.Gauge = EnumGauge.Wanted;
                    data.GaugeResult = EnumGaugeResult.Waiting;
                    data.Processes = ToProcesses(1, 3, 2, 5);
                    break;
                case EnumWorkpiece.B:
                    data.Clean = EnumClean.Wanted;
                    data.Gauge = EnumGauge.Wanted;
                    data.GaugeResult = EnumGaugeResult.Waiting;
                    data.Processes = ToProcesses(1, 3, 5);
                    break;
                case EnumWorkpiece.C:
                    data.Clean = EnumClean.Wanted;
                    data.Gauge = EnumGauge.Wanted;
                    data.GaugeResult = EnumGaugeResult.Waiting;
                    data.Processes = ToProcesses(1, 3, 5);
                    break;
                case EnumWorkpiece.D:
                    data.Clean = EnumClean.Wanted;
                    data.Gauge = EnumGauge.Wanted;
                    data.GaugeResult = EnumGaugeResult.Waiting;
                    data.Processes = ToProcesses(4, 5);
                    break;
                case EnumWorkpiece.E:
                    data.Clean = EnumClean.Wanted;
                    data.Gauge = EnumGauge.Wanted;
                    data.GaugeResult = EnumGaugeResult.Waiting;
                    data.Processes = ToProcesses(5, 6);
                    break;
                default:
                    break;
            }
            return data;
        }

        public byte[] Serialize()
        {
            return RFIDData.Serialize(this);
            //var data = new byte[DataLength];
            //data[0] = (byte)LOGO;
            //data[1] = (byte)No;
            //data[2] = (byte)Workpiece;
            //data[3] = (byte)Clean;
            //data[4] = (byte)Gauge;
            //data[5] = (byte)GaugeResult;
            //for (int i = 0; i < Processes.Count; i++)
            //{
            //    data[i * 2 + 6] = Processes[i].Key;
            //    data[i * 2 + 7] = (byte)Processes[i].Value;
            //}
            //return data;
        }

        public static byte[] Serialize(RFIDData d)
        {
            if (d == null) return null;
            var data = new byte[DataLength];
            data[0] = (byte)d.LOGO;
            data[1] = (byte)d.No;
            data[2] = (byte)d.Workpiece;
            data[3] = (byte)d.Clean;
            data[4] = (byte)d.Gauge;
            data[5] = (byte)d.GaugeResult;
            for (int i = 0; i < d.Processes.Count; i++)
            {
                data[i * 2 + 6] = d.Processes[i].Key;
                data[i * 2 + 7] = (byte)d.Processes[i].Value;
            }
            return data;
        }

        public static RFIDData Deserialize(byte[] data)
        {
            if (data == null || data.Length != DataLength) return null;
            var r = new RFIDData();
            r.LOGO = (EnumLOGO)data[0];
            r.No = (EnumNo)data[1];
            r.Workpiece = (EnumWorkpiece)data[2];
            r.Clean = (EnumClean)data[3];
            r.Gauge = (EnumGauge)data[4];
            r.GaugeResult = (EnumGaugeResult)data[5];
            for (int i = 0; i < data.Length; i += 2)
            {
                if (data[i + 6] == 0)
                {
                    break;
                }
                else
                {
                    r.Processes.Add(new KeyValuePair<byte, EnumProcessResult>(data[i + 6], (EnumProcessResult)data[i + 7]));
                }
            }
            return r;
        }
    }

}
