using HNC;
using HNC.MES.BLL;
using HNC.MES.Common;
using HNC.MES.Model;
using RFID;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool Initialized { get; private set; }

        public static event EventHandler<MyInitializeEventArgs> PartCompleted;

        private static void OnPartCompleted(string message, int value)
        {
            if (PartCompleted != null)
            {
                PartCompleted(null, new MyInitializeEventArgs(message, value));
            }
        }

        public static event EventHandler<MyInitializeEventArgs> AllCompleted;

        private static void OnAllCompleted(string message)
        {
            if (AllCompleted != null)
            {
                AllCompleted(null, new MyInitializeEventArgs(message, 100));
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
        /// 数据系统字典
        /// Key：编号，从0开始，0表示PLC
        /// Value：MachineTool
        /// </summary>
        public static Dictionary<int, MachineTool> MachineTools { get; private set; }

        public static MachineTool PLC
        {
            get
            {
                if (MachineTools != null && MachineTools.Count > 0)
                {
                    return MachineTools[0];
                }
                return null;
            }
        }

        /// <summary>
        /// RFID读写器字典
        /// Key：加工位置
        /// Value：RFID读写器
        /// </summary>
        public static Dictionary<EnumPSite, RFIDReader> RFIDs { get; private set; }

        /// <summary>
        /// 产线
        /// </summary>
        public static Work_RFID Work_RFID { get; private set; }

        /// <summary>
        /// PLC
        /// </summary>
        public static Work_PLC Work_PLC { get; private set; }

        /// <summary>
        /// 三维仿真
        /// </summary>
        public static Work_Simulation Work_Simulation { get; private set; }

        /// <summary>
        /// WMS
        /// </summary>
        public static Work_WMS Work_WMS { get; private set; }

        public static Work_MES Work_MES { get; private set; }

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
                Type = EnumHelper.GetName(TLocation.EnumType.默认),
                State = EnumHelper.GetName(TLocation.EnumState.正常),
                Description = LocationName,
            };
            BLL.TLocation.Insert(location, AdminID);
            LocationID = location.ID;
            BLL.SettingAdd(AdminID, "SelectedLocation", LocationID);
            string[] wpNames = { "A", "B", "C", "D", "E" };
            string[] wpDescriptions = { "小圆", "中圆", "大圆", "底座", "装配成品" };
            var workpieceIDs = new List<string>();
            for (int i = 0; i < wpNames.Length; i++)
            {
                var workpiece = new TWorkpiece
                {
                    LocationID = LocationID,
                    Name = wpNames[i],
                    Type = EnumHelper.GetName(TWorkpiece.EnumType.默认),
                    State = EnumHelper.GetName(TWorkpiece.EnumState.正常),
                    Description = wpDescriptions[i],
                };
                BLL.TWorkpiece.Insert(workpiece, AdminID);
                workpieceIDs.Add(workpiece.ID);
            }
            var manufacture = new TManufacture
            {
                LocationID = LocationID,
                Name = ManufactureName,
                Type = EnumHelper.GetName(TManufacture.EnumType.默认),
                State = EnumHelper.GetName(TManufacture.EnumState.停止),
                Description = ManufactureName,
                WorkpieceID = workpieceIDs.LastOrDefault(),
            };
            BLL.TManufacture.Insert(manufacture, AdminID);
            ManufactureID = manufacture.ID;
            foreach (var wpid in workpieceIDs)
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
                Type = EnumHelper.GetName(TEquipment.EnumType.机床),
                State = EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.121",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc1, AdminID);
            var jc2 = new TEquipment
            {
                Name = "高速钻工中心",
                Type = EnumHelper.GetName(TEquipment.EnumType.机床),
                State = EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.122",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc2, AdminID);
            var jc3 = new TEquipment
            {
                Name = "铣床",
                Type = EnumHelper.GetName(TEquipment.EnumType.机床),
                State = EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.123",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc3, AdminID);
            var jc4 = new TEquipment
            {
                Name = "五轴加工中心",
                Type = EnumHelper.GetName(TEquipment.EnumType.机床),
                State = EnumHelper.GetName(TEquipment.EnumState.停止),
                Description = "IP地址：192.168.1.124",
                ManufactureID = manufacture.ID,
            };
            BLL.TEquipment.Insert(jc4, AdminID);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static async void InitializeAsync()
        {
            await Task.Run(async () =>
            {
                BLL = BLLCustom.Instance;
                AdminID = BLL.GetUserIDByUsername("admin");
                OnPartCompleted("数据库连接成功", 20);
                LocationID = BLL.GetLocationIDByLocationName(LocationName);
                if (string.IsNullOrWhiteSpace(LocationID))
                {
                    InitializeDB();
                }
                else
                {
                    ManufactureID = BLL.TManufacture.GetModel(Tool.CreateDict("Name", ManufactureName)).ID;
                }
                if (!string.IsNullOrWhiteSpace(LocationID))
                {
                    OnPartCompleted("数据库初始化成功", 30);
                }
                else
                {
                    OnPartCompleted("数据库初始化失败！", 30);
                }
                var macIPs = BLL.SettingGet(AdminID, "MacIP").ToString().Split(';');
                MachineTools = new Dictionary<int, MachineTool>();
                for (int i = 0; i < macIPs.Length; i++)
                {
                    try
                    {
                        MachineTools.Add(i, new MachineTool(macIPs[i]));
                    }
                    catch (Exception)
                    {
                        if (i == 0)
                        {
                            throw new Exception("PLC连接失败，IP：" + macIPs[i]);
                        }
                        else
                        {
                            OnPartCompleted("数控系统" + macIPs[i] + "连接失败！", 40);
                        }
                        break;
                    }
                }
                OnPartCompleted("数控系统连接成功", 40);
                var rfidIPs = BLL.SettingGet(AdminID, "RFIDIP").ToString().Split(';');
                RFIDs = new Dictionary<EnumPSite, RFIDReader>();
                RFIDs.Add(EnumPSite.S1, new RFIDReader(EnumPSite.S1, rfidIPs[0]));
                RFIDs.Add(EnumPSite.S2, new RFIDReader(EnumPSite.S2, rfidIPs[1]));
                RFIDs.Add(EnumPSite.S3, new RFIDReader(EnumPSite.S3, rfidIPs[2]));
                RFIDs.Add(EnumPSite.S4, new RFIDReader(EnumPSite.S4, rfidIPs[3]));
                RFIDs.Add(EnumPSite.S5_Assemble, new RFIDReader(EnumPSite.S5_Assemble, rfidIPs[4]));
                RFIDs.Add(EnumPSite.S6_Alignment, new RFIDReader(EnumPSite.S6_Alignment, rfidIPs[5]));
                RFIDs.Add(EnumPSite.S7_Up, new RFIDReader((EnumPSite.S7_Up), rfidIPs[6]));
                RFIDs.Add(EnumPSite.S8_Down, new RFIDReader((EnumPSite.S8_Down), rfidIPs[7]));
                RFIDs.Add(EnumPSite.S9_Manual, new RFIDReader((EnumPSite.S9_Manual), rfidIPs[8]));
                OnPartCompleted("RFID连接成功", 60);
                Work_PLC = Work_PLC.Instance;
                Work_RFID = Work_RFID.Instance;
                Work_Simulation = Work_Simulation.Instance;
                Work_WMS = Work_WMS.Instance;
                Work_MES = Work_MES.Instance;
                OnPartCompleted("后台服务连接成功", 80);
                Initialized = true;
                OnPartCompleted("系统加载完成", 98);
                await Task.Delay(1000);
                OnAllCompleted("即将进入总控界面");
            });
        }

    }
}
