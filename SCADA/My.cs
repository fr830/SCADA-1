using HNC;
using HNC.MES.BLL;
using HNC.MES.Common;
using HNC.MES.Model;
using RFID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCADA
{
    public class MyInitializeEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }

        public string Message { get; private set; }

        public int Value { get; private set; }

        public MyInitializeEventArgs(string message, int value)
        {
            Time = DateTime.Now;
            Message = message;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}{2}", Time, Message, Environment.NewLine);
        }
    }

    static class My
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool Initialized { get; private set; }

        public static event EventHandler<MyInitializeEventArgs> LoadCompleted;

        private static void OnLoadCompleted(string message, int value)
        {
            if (LoadCompleted != null)
            {
                LoadCompleted(null, new MyInitializeEventArgs(message, value));
            }
        }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public const string LocationName = "石家庄职业技术学院智能工厂";

        /// <summary>
        /// 加工单元名称
        /// </summary>
        public const string ManufactureName = "加工单元";

        /// <summary>
        /// 系统启动时间
        /// </summary>
        public static readonly DateTime StartTime = DateTime.Now;

        public static BLLCustom BLL { get; private set; }

        /// <summary>
        /// Admin用户的ID
        /// </summary>
        public static string AdminID { get; private set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public static string LocationID { get; private set; }

        /// <summary>
        /// 加工单元ID
        /// </summary>
        public static string ManufactureID { get; private set; }

        /// <summary>
        /// 工件ID字典
        /// Key：工件类型
        /// Value：工件类型ID
        /// </summary>
        public static Dictionary<EnumWorkpiece, string> WorkpieceIDs { get; private set; }

        /// <summary>
        /// 数据系统字典
        /// Key：加工位置(None表示PLC)
        /// Value：MachineTool
        /// </summary>
        public static Dictionary<EnumPSite, MachineTool> MachineTools { get; private set; }

        /// <summary>
        /// PLC
        /// </summary>
        public static MachineTool PLC
        {
            get
            {
                try
                {
                    return MachineTools[EnumPSite.None];
                }
                catch (Exception)
                {
                    var message = "系统未检测到PLC！请联系维护人员。";
                    logger.Fatal(message);
                    throw new ArgumentException(message);
                }
            }
        }

        /// <summary>
        /// RFID读写器字典
        /// Key：加工位置
        /// Value：RFID读写器
        /// </summary>
        public static Dictionary<EnumPSite, RFIDReader> RFIDs { get; private set; }

        /// <summary>
        /// PLC
        /// </summary>
        public static Work_PLC Work_PLC { get; private set; }

        /// <summary>
        /// RFID
        /// </summary>
        public static Work_RFID Work_RFID { get; private set; }

        /// <summary>
        /// WMS
        /// </summary>
        public static Work_WMS Work_WMS { get; private set; }

        /// <summary>
        /// MES
        /// </summary>
        public static Work_MES Work_MES { get; private set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public static Work_QRCode Work_QRCode { get; private set; }

        /// <summary>
        /// 视觉
        /// </summary>
        public static Work_Vision Work_Vision { get; private set; }

        /// <summary>
        /// 三维仿真
        /// </summary>
        public static Work_Simulation Work_Simulation { get; private set; }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private static void InitializeDB()
        {
            BLL.TLocation.EmptyData();
            BLL.TManufacture.EmptyData();
            BLL.TManufactureWorkpiece.EmptyData();
            BLL.TEquipment.EmptyData();
            BLL.TWorkpiece.EmptyData();
            BLL.TWorkpieceProcess.EmptyData();
            BLL.TSetting.EmptyData();
            if (!BLL.SettingExist(AdminID, "MacIP"))
            {
                string[] array = { "192.168.1.120", "192.168.1.121", "192.168.1.122", "192.168.1.123", "192.168.1.124" };
                BLL.SettingAdd(AdminID, "MacIP", string.Join(";", array));
            }
            if (!BLL.SettingExist(AdminID, "RFIDIP"))
            {
                string[] array = { "192.168.1.131", "192.168.1.132", "192.168.1.133", "192.168.1.134", "192.168.1.135", "192.168.1.136", "192.168.1.137", "192.168.1.138", "192.168.1.139" };
                BLL.SettingAdd(AdminID, "RFIDIP", string.Join(";", array));
            }
            var location = new TLocation
            {
                Name = LocationName,
                Type = "默认",//EnumHelper.GetName(TLocation.EnumType.默认),
                State = "正常",//EnumHelper.GetName(TLocation.EnumState.正常),
                Description = LocationName,
            };
            BLL.TLocation.Insert(location, AdminID);
            LocationID = location.ID;
            BLL.SettingAdd(AdminID, "SelectedLocation", LocationID, "TLocation");
            var wps = Enum.GetValues(typeof(EnumWorkpiece)).Cast<EnumWorkpiece>().ToArray();
            string[] wpDescriptions = { "小圆", "中圆", "大圆", "底座", "装配成品" };
            for (int i = 0; i < wps.Length; i++)
            {
                var workpiece = new TWorkpiece
                {
                    LocationID = LocationID,
                    Name = Enum.GetName(typeof(EnumWorkpiece), wps[i]),
                    Type = "默认",//EnumHelper.GetName(TWorkpiece.EnumType.默认),
                    State = "正常",//EnumHelper.GetName(TWorkpiece.EnumState.正常),
                    Description = wpDescriptions[i],
                };
                BLL.TWorkpiece.Insert(workpiece, AdminID);
                WorkpieceIDs.Add(wps[i], workpiece.ID);
            }
            var manufacture = new TManufacture
            {
                LocationID = LocationID,
                Name = ManufactureName,
                Type = "默认",//EnumHelper.GetName(TManufacture.EnumType.默认),
                State = "停止",//EnumHelper.GetName(TManufacture.EnumState.停止),
                Description = ManufactureName,
                WorkpieceID = WorkpieceIDs[EnumWorkpiece.E],
            };
            BLL.TManufacture.Insert(manufacture, AdminID);
            ManufactureID = manufacture.ID;
            foreach (var wpid in WorkpieceIDs.Values.ToArray())
            {
                var mw = new TManufactureWorkpiece
                {
                    ManufactureID = manufacture.ID,
                    WorkpieceID = wpid,
                    IsAllowed = true,
                };
                BLL.TManufactureWorkpiece.Insert(mw, AdminID);
            }
            var jc1 = new TEquipment
            {
                Name = "车床",
                Type = "机床",//EnumHelper.GetName(TEquipment.EnumType.机床),
                State = "停止",//EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.121",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc1, AdminID);
            var jc2 = new TEquipment
            {
                Name = "高速钻工中心",
                Type = "机床",//EnumHelper.GetName(TEquipment.EnumType.机床),
                State = "停止",//EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.122",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc2, AdminID);
            var jc3 = new TEquipment
            {
                Name = "铣床",
                Type = "机床",//EnumHelper.GetName(TEquipment.EnumType.机床),
                State = "停止",//EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.123",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc3, AdminID);
            var jc4 = new TEquipment
            {
                Name = "五轴加工中心",
                Type = "机床",//EnumHelper.GetName(TEquipment.EnumType.机床),
                State = "停止",//EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.124",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc4, AdminID);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static async Task InitializeAsync()
        {
            await Task.Run(async () =>
            {
                BLL = BLLCustom.Instance;
                AdminID = BLL.GetUserIDByUsername("admin");
                OnLoadCompleted("数据库连接成功", 20);

                LocationID = BLL.GetLocationIDByLocationName(LocationName);
                WorkpieceIDs = new Dictionary<EnumWorkpiece, string>();
                if (string.IsNullOrWhiteSpace(LocationID))
                {
                    InitializeDB();
                }
                else
                {
                    ManufactureID = BLL.TManufacture.GetModel(Tool.CreateDict("Name", ManufactureName)).ID;
                    var wpList = BLL.TWorkpiece.GetList(Tool.CreateDict("LocationID", LocationID));
                    foreach (var item in wpList)
                    {
                        EnumWorkpiece wp;
                        if (Enum.TryParse<EnumWorkpiece>(item.Name, true, out wp))
                        {
                            WorkpieceIDs.Add(wp, item.ID);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(LocationID))
                {
                    OnLoadCompleted("数据库初始化成功", 30);
                }
                else
                {
                    OnLoadCompleted("数据库初始化失败！", 30);
#if !DEBUG
                    throw new Exception("数据库初始化失败！");
#endif
                }

                var macIPs = BLL.SettingGet(AdminID, "MacIP").ToString().Split(';');
                MachineTools = new Dictionary<EnumPSite, MachineTool>();
                try
                {
                    MachineTools.Add(EnumPSite.None, new MachineTool(macIPs[0]));
                }
                catch (Exception)
                {
                    OnLoadCompleted("PLC连接失败！", 35);
#if !DEBUG
                    throw new Exception("PLC连接失败，IP：" + macIPs[0]);
#endif
                }
                try
                {
                    MachineTools.Add(EnumPSite.S1, new MachineTool(macIPs[1]));
                    MachineTools.Add(EnumPSite.S2, new MachineTool(macIPs[2]));
                    MachineTools.Add(EnumPSite.S3, new MachineTool(macIPs[3]));
                    MachineTools.Add(EnumPSite.S4, new MachineTool(macIPs[4]));
                    OnLoadCompleted("数控系统连接成功", 40);
                }
                catch (Exception)
                {
                    OnLoadCompleted("数控系统连接失败！", 40);
                }

                var rfidIPs = BLL.SettingGet(AdminID, "RFIDIP").ToString().Split(';');
                RFIDs = new Dictionary<EnumPSite, RFIDReader>();
                for (EnumPSite i = EnumPSite.S1; i <= EnumPSite.S9_Manual; i++)
                {
                    RFIDs.Add(i, new RFIDReader(i, rfidIPs[(int)i - 1]));
                }
                OnLoadCompleted("RFID连接成功", 60);

                Work_PLC = Work_PLC.Instance;
                Work_RFID = Work_RFID.Instance;
                Work_WMS = Work_WMS.Instance;
                Work_MES = Work_MES.Instance;
                Work_QRCode = Work_QRCode.Instance;
                Work_Vision = Work_Vision.Instance;
                Work_Simulation = Work_Simulation.Instance;
                OnLoadCompleted("后台服务连接成功", 80);

                Initialized = true;
                OnLoadCompleted("系统加载完成", 98);

                await Task.Delay(1000);
                OnLoadCompleted("即将进入总控界面", 100);
            });
        }

    }
}
