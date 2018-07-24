using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{

    public class PLCContent
    {
        public enum EnumSignalType { 状态监控 = 0, 手动控制 }

        public EnumSignalType SignalType { get; private set; }

        public HNC.HncRegType HncRegType { get; private set; }

        public int Index { get; private set; }

        public int Bit { get; private set; }

        public string Text { get; private set; }

        public PLCContent(int index, int bit, string text, EnumSignalType signalType = PLCContent.EnumSignalType.状态监控, HNC.HncRegType hncRegType = HNC.HncRegType.REG_TYPE_R)
        {
            Index = index;
            Bit = bit;
            Text = text;
            SignalType = signalType;
            HncRegType = hncRegType;
        }


        #region 01

        public static IList<PLCContent> listS1 = new List<PLCContent>
        {  
            new PLCContent(12,0,"提升机皮带正转"),
            new PLCContent(12,1,"提升机皮带反转"),
            new PLCContent(301,0,"区域运行"),
            new PLCContent(301,1,"区域手动"),
            new PLCContent(301,2,"区域自动"),
            new PLCContent(301,4,"区域急停"),
            new PLCContent(343,0,"区域故障"),
            new PLCContent(343,1,"ROB就绪"),
            new PLCContent(343,2,"ROB运行"),
            new PLCContent(343,3,"ROB故障"),
            new PLCContent(343,4,"分料机构就绪"),
            new PLCContent(130,7,"提升机就绪"),
        };

        public static IList<PLCContent> listJ1 = new List<PLCContent>
        {
            new PLCContent(12,4,"分料阻挡伸"),
            new PLCContent(12,5,"分料阻挡缩"),
            new PLCContent(12,6,"定位阻挡伸"),
            new PLCContent(12,7,"定位阻挡缩"),
            new PLCContent(12,2,"提升机伸"),
            new PLCContent(12,3,"提升机缩"),
        };

        public static IList<PLCContent> listC1 = new List<PLCContent>
        {
            new PLCContent(340,0,"分料阻挡伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(340,1,"分料阻挡缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(340,2,"定位阻挡伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(340,3,"定位阻挡缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(340,4,"提升机伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(340,5,"提升机缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(341,0,"复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(341,1,"故障复位",  PLCContent.EnumSignalType.手动控制),
        };

        #endregion

        #region 02

        public static IList<PLCContent> listS2 = new List<PLCContent>
        {
            new PLCContent(21,0,"提升机皮带正转"),
            new PLCContent(302,0,"区域运行"),
            new PLCContent(302,1,"区域手动"),
            new PLCContent(302,2,"区域自动"),
            new PLCContent(302,4,"区域急停"),
            new PLCContent(347,0,"区域故障"),
            new PLCContent(120,7,"顶升就绪"),
            new PLCContent(147,1,"顶升运行"),
            new PLCContent(347,2,"顶升故障"),
            new PLCContent(106,0,"顶升1允许取A料"),
            new PLCContent(106,1,"顶升1允许取B料"),
            new PLCContent(106,2,"顶升1允许取C料"),
            new PLCContent(106,3,"顶升1允许放A料"),
            new PLCContent(106,4,"顶升1允许放B料"),
            new PLCContent(106,5,"顶升1允许放C料"),
        };

        public static IList<PLCContent> listJ2 = new List<PLCContent>
        {
            new PLCContent(21,2 ,"定位阻挡伸到位"),
            new PLCContent(21,3,"定位阻挡缩到位"),
            new PLCContent(21,4,"分料阻挡伸到位"),
            new PLCContent(21,5,"分料阻挡缩到位"),
            new PLCContent(21,6,"托举气缸伸到位"),
            new PLCContent(21,7,"托举气缸缩到位"),
            new PLCContent(22,0,"左夹紧气缸伸到位"),
            new PLCContent(22,1,"左夹紧气缸缩到位"),
            new PLCContent(22,2,"右夹紧气缸伸到位"),
            new PLCContent(22,3,"右夹紧气缸缩到位"),
            new PLCContent(22,4,"左顶升气缸伸到位"),
            new PLCContent(22,5,"左顶升气缸缩到位"),
            new PLCContent(22,6,"右顶升气缸伸到位"),
            new PLCContent(22,7,"右顶升气缸缩到位"),
        };

        public static IList<PLCContent> listC2 = new List<PLCContent>
        {
            new PLCContent(344,0,"定位阻挡伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,1,"定位阻挡缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,2,"分料阻挡伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,3,"分料阻挡缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,4,"托举气缸伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,5,"托举气缸缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,6,"左夹紧气缸伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(344,7,"左夹紧气缸缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,0,"右夹紧气缸伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,1,"右夹紧气缸缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,2,"左顶升气缸伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,3,"左顶升气缸缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,4,"右顶升气缸伸", PLCContent.EnumSignalType.手动控制),
            new PLCContent(345,5,"右顶升气缸缩", PLCContent.EnumSignalType.手动控制),
            new PLCContent(346,0,"复位", PLCContent.EnumSignalType.手动控制),
            new PLCContent(346,1,"故障复位", PLCContent.EnumSignalType.手动控制),
            new PLCContent(346,2,"加工失败", PLCContent.EnumSignalType.手动控制),
        };

        #endregion

        #region 03

        public static IList<PLCContent> listS3 = new List<PLCContent>
        {
            new PLCContent(31,0,"提升机皮带正转"),
            new PLCContent(303,0,"区域运行"),
            new PLCContent(303,1,"区域手动"),
            new PLCContent(303,2,"区域自动"),
            new PLCContent(303,4,"区域急停"),
            new PLCContent(353,0,"区域故障"),
            new PLCContent(140,7,"顶升就绪"),
            new PLCContent(353,1,"顶升运行"),
            new PLCContent(353,2,"顶升故障"),
            new PLCContent(106,6,"顶升允许取A料"),
            new PLCContent(106,7,"顶升允许放A料"),
        };

        public static IList<PLCContent> listJ3 = new List<PLCContent>
        {
            new PLCContent(31,2,"定位阻挡伸到位"),
            new PLCContent(31,3,"定位阻挡缩到位"),
            new PLCContent(31,4,"分料阻挡伸到位"),
            new PLCContent(31,5,"分料阻挡缩到位"),
            new PLCContent(31,6,"托举气缸伸到位"),
            new PLCContent(31,7,"托举气缸缩到位"),
            new PLCContent(32,0,"左夹紧气缸伸到位"),
            new PLCContent(32,1,"左夹紧气缸缩到位"),
            new PLCContent(32,2,"右夹紧气缸伸到位"),
            new PLCContent(32,3,"右夹紧气缸缩到位"),
            new PLCContent(32,4,"左顶升气缸伸到位"),
            new PLCContent(32,5,"左顶升气缸缩到位"),
            new PLCContent(32,6,"右顶升气缸伸到位"),
            new PLCContent(32,7,"右顶升气缸缩到位"),
        };

        public static IList<PLCContent> listC3 = new List<PLCContent>
        {
            new PLCContent(350,0,"定位阻挡伸", EnumSignalType.手动控制),
            new PLCContent(350,1,"定位阻挡缩", EnumSignalType.手动控制),
            new PLCContent(350,2,"分料阻挡伸", EnumSignalType.手动控制),
            new PLCContent(350,3,"分料阻挡缩", EnumSignalType.手动控制),
            new PLCContent(350,4,"托举气缸伸", EnumSignalType.手动控制),
            new PLCContent(350,5,"托举气缸缩", EnumSignalType.手动控制),
            new PLCContent(350,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(350,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(351,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(351,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(351,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(351,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(351,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(351,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(352,0,"复位", EnumSignalType.手动控制),
            new PLCContent(352,1,"故障复位", EnumSignalType.手动控制),
            new PLCContent(352,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 04

        public static IList<PLCContent> listS4 = new List<PLCContent>
        {
            new PLCContent(41,0,"提升机皮带正转"),
            new PLCContent(304,0,"区域运行"),
            new PLCContent(304,1,"区域手动"),
            new PLCContent(304,2,"区域自动"),
            new PLCContent(304,4,"区域急停"),
            new PLCContent(357,0,"区域故障"),
            new PLCContent(150,7,"顶升就绪"),
            new PLCContent(357,1,"顶升运行"),
            new PLCContent(357,2,"顶升故障"),
            new PLCContent(96,0,"顶升允许取A料"),
            new PLCContent(96,1,"顶升允许取B料"),
            new PLCContent(96,2,"顶升允许取C料"),
            new PLCContent(96,3,"顶升允许放A料"),
            new PLCContent(96,4,"顶升允许放B料"),
            new PLCContent(96,5,"顶升允许放C料"),
        };

        public static IList<PLCContent> listJ4 = new List<PLCContent>
        {
            new PLCContent(41,2,"定位阻挡伸到位"),
            new PLCContent(41,3,"定位阻挡缩到位"),
            new PLCContent(41,4,"分料阻挡伸到位"),
            new PLCContent(41,5,"分料阻挡缩到位"),
            new PLCContent(41,6,"托举气缸伸到位"),
            new PLCContent(41,7,"托举气缸缩到位"),
            new PLCContent(42,0,"左夹紧气缸伸到位"),
            new PLCContent(42,1,"左夹紧气缸缩到位"),
            new PLCContent(42,2,"右夹紧气缸伸到位"),
            new PLCContent(42,3,"右夹紧气缸缩到位"),
            new PLCContent(42,4,"左顶升气缸伸到位"),
            new PLCContent(42,5,"左顶升气缸缩到位"),
            new PLCContent(42,6,"右顶升气缸伸到位"),
            new PLCContent(42,7,"右顶升气缸缩到位"),
        };

        public static IList<PLCContent> listC4 = new List<PLCContent>
        {
            new PLCContent(354,0,"定位阻挡伸", EnumSignalType.手动控制),
            new PLCContent(354,1,"定位阻挡缩", EnumSignalType.手动控制),
            new PLCContent(354,2,"分料阻挡伸", EnumSignalType.手动控制),
            new PLCContent(354,3,"分料阻挡缩", EnumSignalType.手动控制),
            new PLCContent(354,4,"托举气缸伸", EnumSignalType.手动控制),
            new PLCContent(354,5,"托举气缸缩", EnumSignalType.手动控制),
            new PLCContent(354,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(354,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(355,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(355,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(355,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(355,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(355,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(355,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(356,0,"复位", EnumSignalType.手动控制),
            new PLCContent(356,1,"故障复位", EnumSignalType.手动控制),
            new PLCContent(356,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 05
        public static IList<PLCContent> listS5 = new List<PLCContent>
        {
            new PLCContent(51,0,"提升机皮带正转"),
            new PLCContent(305,0,"区域运行"),
            new PLCContent(305,1,"区域手动"),
            new PLCContent(305,2,"区域自动"),
            new PLCContent(305,4,"区域急停"),
            new PLCContent(363,0,"区域故障"),
            new PLCContent(160,7,"顶升就绪"),
            new PLCContent(363,1,"顶升运行"),
            new PLCContent(363,2,"顶升故障"),
            new PLCContent(96,6,"顶升允许取D料"),
            new PLCContent(96,7,"顶升允许放D料"),
        };

        public static IList<PLCContent> listJ5 = new List<PLCContent>
        {
            new PLCContent(51,2,"定位阻挡伸到位"),
            new PLCContent(51,3,"定位阻挡缩到位"),
            new PLCContent(51,4,"分料阻挡伸到位"),
            new PLCContent(51,5,"分料阻挡缩到位"),
            new PLCContent(51,6,"托举气缸伸到位"),
            new PLCContent(51,7,"托举气缸缩到位"),
            new PLCContent(52,0,"左夹紧气缸伸到位"),
            new PLCContent(52,1,"左夹紧气缸缩到位"),
            new PLCContent(52,2,"右夹紧气缸伸到位"),
            new PLCContent(52,3,"右夹紧气缸缩到位"),
            new PLCContent(52,4,"左顶升气缸伸到位"),
            new PLCContent(52,5,"左顶升气缸缩到位"),
            new PLCContent(52,6,"右顶升气缸伸到位"),
            new PLCContent(52,7,"右顶升气缸缩到位"),
        };

        public static IList<PLCContent> listC5 = new List<PLCContent>
        {
            new PLCContent(360,0,"定位阻挡伸", EnumSignalType.手动控制),
            new PLCContent(360,1,"定位阻挡缩", EnumSignalType.手动控制),
            new PLCContent(360,2,"分料阻挡伸", EnumSignalType.手动控制),
            new PLCContent(360,3,"分料阻挡缩", EnumSignalType.手动控制),
            new PLCContent(360,4,"托举气缸伸", EnumSignalType.手动控制),
            new PLCContent(360,5,"托举气缸缩", EnumSignalType.手动控制),
            new PLCContent(360,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(360,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(361,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(361,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(361,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(361,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(361,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(361,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(361,7,"复位", EnumSignalType.手动控制),
            new PLCContent(362,0,"故障复位", EnumSignalType.手动控制),
            new PLCContent(362,1,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 06

        public static IList<PLCContent> listS6 = new List<PLCContent>
        {
            new PLCContent(61,0,"倍速链1正转"),
            new PLCContent(61,1,"倍速链2正转"),
            new PLCContent(306,0,"区域运行"),
            new PLCContent(306,1,"区域手动"),
            new PLCContent(306,2,"区域自动"),
            new PLCContent(306,4,"区域急停"),
            new PLCContent(367,0,"区域故障"),
            new PLCContent(170,7,"顶升就绪"),
            new PLCContent(367,1,"顶升运行"),
            new PLCContent(367,2,"顶升故障"),
            new PLCContent(116,0,"允许取工件1"),
            new PLCContent(116,1,"允许取工件2"),
            new PLCContent(116,2,"允许取工件3"),
            new PLCContent(116,3,"允许取工件4"),
            new PLCContent(116,4,"允许放装配件"),
            new PLCContent(116,5,"允许放工件1"),
            new PLCContent(116,6,"允许放工件2"),
            new PLCContent(116,7,"允许放工件3"),
        };

        public static IList<PLCContent> listJ6 = new List<PLCContent>
        {
            new PLCContent(61,2,"定位阻挡伸到位"),
            new PLCContent(61,3,"定位阻挡缩到位"),
            new PLCContent(61,4,"分料阻挡伸到位"),
            new PLCContent(61,5,"分料阻挡缩到位"),
            new PLCContent(61,6,"托举气缸伸到位"),
            new PLCContent(61,7,"托举气缸缩到位"),
            new PLCContent(62,0,"左夹紧气缸伸到位"),
            new PLCContent(62,1,"左夹紧气缸缩到位"),
            new PLCContent(62,2,"右夹紧气缸伸到位"),
            new PLCContent(62,3,"右夹紧气缸缩到位"),
            new PLCContent(62,4,"左顶升气缸伸到位"),
            new PLCContent(62,5,"左顶升气缸缩到位"),
            new PLCContent(62,6,"右顶升气缸伸到位"),
            new PLCContent(62,7,"右顶升气缸缩到位"),
        };

        public static IList<PLCContent> listC6 = new List<PLCContent>
        {
            new PLCContent(364,0,"定位阻挡伸", EnumSignalType.手动控制),
            new PLCContent(364,1,"定位阻挡缩", EnumSignalType.手动控制),
            new PLCContent(364,2,"分料阻挡伸", EnumSignalType.手动控制),
            new PLCContent(364,3,"分料阻挡缩", EnumSignalType.手动控制),
            new PLCContent(364,4,"托举气缸伸", EnumSignalType.手动控制),
            new PLCContent(364,5,"托举气缸缩", EnumSignalType.手动控制),
            new PLCContent(364,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(364,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(365,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new PLCContent(365,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new PLCContent(365,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(365,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(365,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new PLCContent(365,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new PLCContent(366,0,"复位", EnumSignalType.手动控制),
            new PLCContent(366,1,"故障复位", EnumSignalType.手动控制),
            new PLCContent(366,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 07

        public static IList<PLCContent> listS7 = new List<PLCContent>
        {
            new PLCContent(72,0,"提升机皮带正转"),
            new PLCContent(72,1,"提升机皮带反转"),
            new PLCContent(307,0,"区域运行"),
            new PLCContent(307,1,"区域手动"),
            new PLCContent(307,2,"区域自动"),
            new PLCContent(307,4,"区域急停"),
            new PLCContent(372,0,"区域故障"),
            new PLCContent(180,7,"顶升就绪"),
            new PLCContent(372,1,"顶升运行"),
            new PLCContent(372,2,"顶升故障"),
            new PLCContent(180,6,"提升机就绪"),
            new PLCContent(372,3,"提升机工作"),
            new PLCContent(372,4,"提升机故障"),
            new PLCContent(372,5,"ROB就绪"),
            new PLCContent(372,6,"ROB运行"),
            new PLCContent(372,7,"ROB故障"),
        };

        public static IList<PLCContent> listJ7 = new List<PLCContent>
        {
            new PLCContent(72,2,"分料阻挡伸到位"),
            new PLCContent(72,3,"分料阻挡缩到位"),
            new PLCContent(72,4,"定位阻挡伸到位"),
            new PLCContent(72,5,"定位阻挡缩到位"),
            new PLCContent(72,6,"托举气缸伸到位"),
            new PLCContent(72,7,"托举气缸缩到位"),
            new PLCContent(73,0,"提升机伸到位"),
            new PLCContent(73,1,"提升机缩到位"),
        };

        public static IList<PLCContent> listC7 = new List<PLCContent>
        {
            new PLCContent(370,0,"分料阻挡伸", EnumSignalType.手动控制),
            new PLCContent(370,1,"分料阻挡缩", EnumSignalType.手动控制),
            new PLCContent(370,2,"定位阻挡伸", EnumSignalType.手动控制),
            new PLCContent(370,3,"定位阻挡缩", EnumSignalType.手动控制),
            new PLCContent(370,4,"托举气缸伸", EnumSignalType.手动控制),
            new PLCContent(370,5,"托举气缸缩", EnumSignalType.手动控制),
            new PLCContent(370,6,"提升机伸", EnumSignalType.手动控制),
            new PLCContent(370,7,"提升机缩", EnumSignalType.手动控制),
            new PLCContent(371,0,"复位", EnumSignalType.手动控制),
            new PLCContent(371,1,"故障复位", EnumSignalType.手动控制),
        };

        #endregion

        #region 08

        public static IList<PLCContent> listS8 = new List<PLCContent>
        {
            new PLCContent(308,0,"区域运行"),
            new PLCContent(308,1,"区域手动"),
            new PLCContent(308,2,"区域自动"),
            new PLCContent(308,4,"区域急停"),
            new PLCContent(383,0,"区域故障"),
            new PLCContent(383,1,"装配台就绪"),
            new PLCContent(383,2,"装配台故障"),
            new PLCContent(383,3,"清洗机就绪"),
            new PLCContent(383,4,"清洗机故障"),
        };

        public static IList<PLCContent> listJ8 = new List<PLCContent>
        {
            new PLCContent(83,0,"A工件定位气缸伸到位"),
            new PLCContent(83,1,"A工件定位气缸缩到位"),
            new PLCContent(83,2,"B工件定位气缸伸到位"),
            new PLCContent(83,3,"B工件定位气缸缩到位"),
            new PLCContent(83,4,"C工件定位气缸伸到位"),
            new PLCContent(83,5,"C工件定位气缸缩到位"),
            new PLCContent(83,6,"D工件定位气缸伸到位"),
            new PLCContent(83,7,"D工件定位气缸缩到位"),
            new PLCContent(84,0,"移料气缸伸到位"),
            new PLCContent(84,1,"移料气缸缩到位"),
            new PLCContent(250,6,"移料定位气缸伸到位"),
            new PLCContent(250,7,"移料定位气缸缩到位"),
        };

        public static IList<PLCContent> listC8 = new List<PLCContent>
        {
            new PLCContent(380,0,"A工件定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,1,"A工件定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,2,"B工件定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,3,"B工件定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,4,"C工件定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,5,"C工件定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,6,"D工件定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(380,7,"D工件定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,0,"移料气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,1,"移料气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,2,"移料定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,3,"移料定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,5,"清洗机复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,6,"装配台复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(381,7,"故障复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(382,0,"工件A复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(382,1,"工件B复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(382,2,"工件C复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(382,3,"工件D复位",  PLCContent.EnumSignalType.手动控制),
        };

        #endregion

        #region 09

        public static IList<PLCContent> listS9 = new List<PLCContent>
        {
            new PLCContent(309,0,"区域运行"),
            new PLCContent(309,1,"区域手动"),
            new PLCContent(309,2,"区域自动"),
            new PLCContent(309,4,"区域急停"),
            new PLCContent(392,0,"区域故障"),
            new PLCContent(200,0,"翻转机构就绪"),
            new PLCContent(392,1,"翻转机构运行"),
            new PLCContent(392,2,"翻转机构故障"),
            new PLCContent(392,3,"ROB就绪"),
            new PLCContent(392,4,"ROB运行"),
            new PLCContent(392,5,"ROB故障"),
            new PLCContent(92,0,"三轴循环运行"),
            new PLCContent(92,1,"三轴空闲"),
            new PLCContent(92,2,"三轴故障"),
            new PLCContent(92,3,"三轴允许上料"),
            new PLCContent(92,4,"三轴允许下料"),
            new PLCContent(93,0,"五轴轴循环运行"),
            new PLCContent(93,1,"五轴空闲"),
            new PLCContent(93,2,"五轴故障"),
            new PLCContent(93,3,"五轴允许上料"),
            new PLCContent(93,4,"五轴允许下料"),
        };

        public static IList<PLCContent> listJ9 = new List<PLCContent>
        {
            new PLCContent(93,0,"翻转气缸伸到位"),
            new PLCContent(93,1,"翻转气缸缩到位"),
            new PLCContent(93,2,"放料夹紧气缸伸到位"),
            new PLCContent(93,3,"放料夹紧气缸缩到位"),
            new PLCContent(93,4,"取料气缸伸到位"),
            new PLCContent(93,5,"取料气缸伸到位"),
        };

        public static IList<PLCContent> listC9 = new List<PLCContent>
        {
            new PLCContent(390,0,"翻转气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(390,1,"翻转气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(390,2,"放料夹紧气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(390,3,"放料夹紧气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(390,4,"取料气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(390,5,"取料气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(391,0,"复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(391,1,"故障复位",  PLCContent.EnumSignalType.手动控制),
        };

        #endregion

        #region 10
        public static IList<PLCContent> listS10 = new List<PLCContent>
        {
            new PLCContent(310,0,"区域运行"),
            new PLCContent(310,1,"区域手动"),
            new PLCContent(310,2,"区域自动"),
            new PLCContent(310,4,"区域急停"),
            new PLCContent(396,4,"区域故障"),
            new PLCContent(396,5,"ROB就绪"),
            new PLCContent(396,6,"ROB运行"),
            new PLCContent(396,7,"ROB故障"),
            new PLCContent(102,0,"车床循环运行"),
            new PLCContent(102,1,"车床空闲"),
            new PLCContent(102,2,"车床故障"),
            new PLCContent(102,3,"车床允许上料"),
            new PLCContent(102,4,"车床允许下料"),
            new PLCContent(103,0,"钻攻中心循环运行"),
            new PLCContent(103,1,"钻攻中心空闲"),
            new PLCContent(103,2,"钻攻中心故障"),
            new PLCContent(103,3,"钻攻中心允许上料"),
            new PLCContent(103,4,"钻攻中心允许下料"),
            new PLCContent(397,0,"台1就绪"),
            new PLCContent(397,1,"台1故障"),
            new PLCContent(397,2,"台2就绪"),
            new PLCContent(397,3,"台2故障"),
            new PLCContent(397,4,"台3就绪"),
            new PLCContent(397,5,"台3故障"),
            new PLCContent(397,6,"台4就绪"),
            new PLCContent(397,7,"台4故障"),
        };

        public static IList<PLCContent> listJ10 = new List<PLCContent>
        {
            new PLCContent(260,0,"台1定位气缸伸"),
            new PLCContent(260,1,"台1定位气缸缩"),
            new PLCContent(1,6,"台1阻挡气缸伸到位"),
            new PLCContent(1,7,"台1阻挡气缸缩到位"),
            new PLCContent(264,2,"台2定位气缸伸"),
            new PLCContent(264,3,"台2定位气缸缩"),
            new PLCContent(2,6,"台2阻挡气缸伸"),
            new PLCContent(2,7,"台2阻挡气缸缩"),
            new PLCContent(220,0,"台3定位气缸伸"),
            new PLCContent(220,1,"台3定位气缸缩"),
            new PLCContent(224,0,"台4定位气缸伸"),
            new PLCContent(224,1,"台4定位气缸缩"),
        };

        public static IList<PLCContent> listC10 = new List<PLCContent>
        {
            new PLCContent(394,0,"台1定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,1,"台1定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,2,"台1阻挡气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,3,"台1阻挡气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,4,"台2定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,5,"台2定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,6,"台2阻挡气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(394,7,"台2阻挡气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,0,"台3定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,1,"台3定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,2,"台4定位气缸伸",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,3,"台4定位气缸缩",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,5,"台1复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,6,"台2复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(395,7,"台3复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(396,0,"台4复位",  PLCContent.EnumSignalType.手动控制),
            new PLCContent(396,1,"区域10故障复位",  PLCContent.EnumSignalType.手动控制),
        };

        #endregion
    }

}