using HNC;
using HNC.MES.BLL;
using HNC.MES.Common;
using HNC.MES.Model;
using RFID;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCADA
{
    static class My
    {
        /// <summary>
        /// 工厂名称
        /// </summary>
        public const string LocationName = "石家庄职业技术学院智能工厂";

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
        /// 数据系统字典
        /// Key：编号，从0开始，0表示PLC
        /// Value：MachineTool
        /// </summary>
        public static SortedDictionary<int, MachineTool> MachineTools { get; private set; }

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
        /// Key：编号，从2开始
        /// Value：RFID读写器
        /// </summary>
        public static SortedDictionary<int, RFIDReader> RFIDs { get; private set; }

        /// <summary>
        /// 产线
        /// </summary>
        public static Work_Line Work_Line { get; private set; }

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
            BLL.SettingAdd(AdminID, "SelectedLocation", LocationID);
            string[] wpNames = { "小圆", "中圆", "大圆", "底座", "装配成品" };
            var workpieceIDs = new List<string>();
            foreach (var name in wpNames)
            {
                var workpiece = new TWorkpiece
                {
                    LocationID = LocationID,
                    Name = name,
                    Type = EnumHelper.GetName(TWorkpiece.EnumType.默认),
                    State = EnumHelper.GetName(TWorkpiece.EnumState.正常),
                    Description = name,
                };
                BLL.TWorkpiece.Insert(workpiece, AdminID);
                workpieceIDs.Add(workpiece.ID);
            }
            var manufacture = new TManufacture
            {
                LocationID = LocationID,
                Name = "加工单元",
                Type = EnumHelper.GetName(TManufacture.EnumType.默认),
                State = EnumHelper.GetName(TManufacture.EnumState.停止),
                Description = "加工单元",
                WorkpieceID = workpieceIDs.LastOrDefault(),
            };
            BLL.TManufacture.Insert(manufacture, AdminID);
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
        public static void Initialize()
        {
            BLL = BLLCustom.Instance;
            AdminID = BLL.GetUserIDByUsername("admin");
            LocationID = BLL.GetLocationIDByLocationName(LocationName);
            if (string.IsNullOrWhiteSpace(LocationID))
            {
                InitializeDB();
                LocationID = BLL.GetLocationIDByLocationName(LocationName);
            }
            var macIPs = BLL.SettingGet(AdminID, "MacIP").ToString().Split(';');
            MachineTools = new SortedDictionary<int, MachineTool>();
            for (int i = 0; i < macIPs.Length; i++)
            {
                MachineTools.Add(i, new MachineTool(macIPs[i]));
            }
            var rfidIPs = BLL.SettingGet(AdminID, "RFIDIP").ToString().Split(';');
            RFIDs = new SortedDictionary<int, RFIDReader>();
            for (int i = 0; i < rfidIPs.Length; i++)
            {
                RFIDs.Add(i + 2, new RFIDReader(i + 2, rfidIPs[i]));
            }
            Work_Line = Work_Line.Instance;
            Work_PLC = Work_PLC.Instance;
            Work_Simulation = Work_Simulation.Instance;
            Work_WMS = Work_WMS.Instance;
        }

    }
}
