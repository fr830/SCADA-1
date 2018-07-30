using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{

    /// <summary>
    /// PLC寄存器信号
    /// </summary>
    public class Signal
    {
        public enum EnumSignalType { 状态监控 = 0, 手动控制 }

        /// <summary>
        /// 信号类型
        /// </summary>
        public EnumSignalType SignalType { get; private set; }

        /// <summary>
        /// 寄存器类型
        /// </summary>
        public HNC.HncRegType HncRegType { get; private set; }

        public int Index { get; private set; }

        public int Bit { get; private set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// 期望信号状态
        /// </summary>
        public bool Expect { get; private set; }

        /// <summary>
        /// 信号状态是否符合期望
        /// </summary>
        public bool IsExpected
        {
            get
            {
                return My.PLC.Exist(Index, Bit, HncRegType) == Expect;
            }
        }

        public Signal(int index, int bit, string text, EnumSignalType signalType = Signal.EnumSignalType.状态监控, HNC.HncRegType hncRegType = HNC.HncRegType.REG_TYPE_R, bool expect = true)
        {
            Index = index;
            Bit = bit;
            Text = text;
            SignalType = signalType;
            HncRegType = hncRegType;
            Expect = expect;
            Color = Color.RoyalBlue;
        }

        public Signal(int index, int bit, string text, Color color, bool expect = true, EnumSignalType signalType = Signal.EnumSignalType.状态监控, HNC.HncRegType hncRegType = HNC.HncRegType.REG_TYPE_R)
        {
            Index = index;
            Bit = bit;
            Text = text;
            Color = color;
            Expect = expect;
            SignalType = signalType;
            HncRegType = hncRegType;
        }

        public bool Exist()
        {
            return My.PLC.Exist(Index, Bit, HncRegType);
        }

        public bool Set()
        {
            return My.PLC.Set(Index, Bit, HncRegType);
        }

        public bool Clear()
        {
            return My.PLC.Clear(Index, Bit, HncRegType);
        }

        #region 01

        public static IList<Signal> listS1 = new List<Signal>
        {  
            new Signal(12,0,"提升机皮带正转"),
            new Signal(12,1,"提升机皮带反转"),
            new Signal(301,0,"区域运行"),
            new Signal(301,1,"区域手动"),
            new Signal(301,2,"区域自动"),
            new Signal(301,4,"区域急停"),
            new Signal(343,0,"区域故障"),
            new Signal(343,1,"ROB就绪"),
            new Signal(343,2,"ROB运行"),
            new Signal(343,3,"ROB故障"),
            new Signal(343,4,"分料机构就绪"),
            new Signal(343,5,"分料机构运行"),
            new Signal(343,6,"分料机构故障"),
            new Signal(130,7,"提升机就绪"),
            new Signal(342,0,"提升机运行"),
            new Signal(342,1,"提升机故障"),
        };

        public static IList<Signal> listJ1 = new List<Signal>
        {
            new Signal(12,4,"分料阻挡伸"),
            new Signal(12,5,"分料阻挡缩"),
            new Signal(12,6,"定位阻挡伸"),
            new Signal(12,7,"定位阻挡缩"),
            new Signal(12,2,"提升机伸"),
            new Signal(12,3,"提升机缩"),
        };

        public static IList<Signal> listC1 = new List<Signal>
        {
            new Signal(340,0,"分料阻挡伸",  Signal.EnumSignalType.手动控制),
            new Signal(340,1,"分料阻挡缩",  Signal.EnumSignalType.手动控制),
            new Signal(340,2,"定位阻挡伸",  Signal.EnumSignalType.手动控制),
            new Signal(340,3,"定位阻挡缩",  Signal.EnumSignalType.手动控制),
            new Signal(340,4,"提升机伸",  Signal.EnumSignalType.手动控制),
            new Signal(340,5,"提升机缩",  Signal.EnumSignalType.手动控制),
            new Signal(341,0,"复位",  Signal.EnumSignalType.手动控制),
            //new Signal(341,1,"故障复位",  Signal.EnumSignalType.手动控制),
        };

        #endregion

        #region 02

        public static IList<Signal> listS2 = new List<Signal>
        {
            new Signal(21,0,"提升机皮带正转"),
            new Signal(302,0,"区域运行"),
            new Signal(302,1,"区域手动"),
            new Signal(302,2,"区域自动"),
            new Signal(302,4,"区域急停"),
            new Signal(347,0,"区域故障"),
            new Signal(120,7,"顶升就绪"),
            new Signal(347,1,"顶升运行"),
            new Signal(347,2,"顶升故障"),
            new Signal(106,0,"顶升1允许取A料"),
            new Signal(106,1,"顶升1允许取B料"),
            new Signal(106,2,"顶升1允许取C料"),
            new Signal(106,3,"顶升1允许放A料"),
            new Signal(106,4,"顶升1允许放B料"),
            new Signal(106,5,"顶升1允许放C料"),
        };

        public static IList<Signal> listJ2 = new List<Signal>
        {
            new Signal(21,2 ,"定位阻挡伸"),
            new Signal(21,3,"定位阻挡缩"),
            new Signal(21,4,"分料阻挡伸"),
            new Signal(21,5,"分料阻挡缩"),
            new Signal(21,6,"托举气缸伸"),
            new Signal(21,7,"托举气缸缩"),
            new Signal(22,0,"左夹紧气缸伸"),
            new Signal(22,1,"左夹紧气缸缩"),
            new Signal(22,2,"右夹紧气缸伸"),
            new Signal(22,3,"右夹紧气缸缩"),
            new Signal(22,4,"左顶升气缸伸"),
            new Signal(22,5,"左顶升气缸缩"),
            new Signal(22,6,"右顶升气缸伸"),
            new Signal(22,7,"右顶升气缸缩"),
        };

        public static IList<Signal> listC2 = new List<Signal>
        {
            new Signal(344,0,"定位阻挡伸", Signal.EnumSignalType.手动控制),
            new Signal(344,1,"定位阻挡缩", Signal.EnumSignalType.手动控制),
            new Signal(344,2,"分料阻挡伸", Signal.EnumSignalType.手动控制),
            new Signal(344,3,"分料阻挡缩", Signal.EnumSignalType.手动控制),
            new Signal(344,4,"托举气缸伸", Signal.EnumSignalType.手动控制),
            new Signal(344,5,"托举气缸缩", Signal.EnumSignalType.手动控制),
            new Signal(344,6,"左夹紧气缸伸", Signal.EnumSignalType.手动控制),
            new Signal(344,7,"左夹紧气缸缩", Signal.EnumSignalType.手动控制),
            new Signal(345,0,"右夹紧气缸伸", Signal.EnumSignalType.手动控制),
            new Signal(345,1,"右夹紧气缸缩", Signal.EnumSignalType.手动控制),
            new Signal(345,2,"左顶升气缸伸", Signal.EnumSignalType.手动控制),
            new Signal(345,3,"左顶升气缸缩", Signal.EnumSignalType.手动控制),
            new Signal(345,4,"右顶升气缸伸", Signal.EnumSignalType.手动控制),
            new Signal(345,5,"右顶升气缸缩", Signal.EnumSignalType.手动控制),
            new Signal(346,0,"复位", Signal.EnumSignalType.手动控制),
            //new Signal(346,1,"故障复位", Signal.EnumSignalType.手动控制),
            new Signal(346,2,"加工失败", Signal.EnumSignalType.手动控制),
        };

        #endregion

        #region 03

        public static IList<Signal> listS3 = new List<Signal>
        {
            new Signal(31,0,"提升机皮带正转"),
            new Signal(303,0,"区域运行"),
            new Signal(303,1,"区域手动"),
            new Signal(303,2,"区域自动"),
            new Signal(303,4,"区域急停"),
            new Signal(353,0,"区域故障"),
            new Signal(140,7,"顶升就绪"),
            new Signal(353,1,"顶升运行"),
            new Signal(353,2,"顶升故障"),
            new Signal(106,6,"顶升允许取A料"),
            new Signal(106,7,"顶升允许放A料"),
        };

        public static IList<Signal> listJ3 = new List<Signal>
        {
            new Signal(31,2,"定位阻挡伸"),
            new Signal(31,3,"定位阻挡缩"),
            new Signal(31,4,"分料阻挡伸"),
            new Signal(31,5,"分料阻挡缩"),
            new Signal(31,6,"托举气缸伸"),
            new Signal(31,7,"托举气缸缩"),
            new Signal(32,0,"左夹紧气缸伸"),
            new Signal(32,1,"左夹紧气缸缩"),
            new Signal(32,2,"右夹紧气缸伸"),
            new Signal(32,3,"右夹紧气缸缩"),
            new Signal(32,4,"左顶升气缸伸"),
            new Signal(32,5,"左顶升气缸缩"),
            new Signal(32,6,"右顶升气缸伸"),
            new Signal(32,7,"右顶升气缸缩"),
        };

        public static IList<Signal> listC3 = new List<Signal>
        {
            new Signal(350,0,"定位阻挡伸", EnumSignalType.手动控制),
            new Signal(350,1,"定位阻挡缩", EnumSignalType.手动控制),
            new Signal(350,2,"分料阻挡伸", EnumSignalType.手动控制),
            new Signal(350,3,"分料阻挡缩", EnumSignalType.手动控制),
            new Signal(350,4,"托举气缸伸", EnumSignalType.手动控制),
            new Signal(350,5,"托举气缸缩", EnumSignalType.手动控制),
            new Signal(350,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(350,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(351,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(351,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(351,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new Signal(351,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new Signal(351,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new Signal(351,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new Signal(352,0,"复位", EnumSignalType.手动控制),
            //new Signal(352,1,"故障复位", EnumSignalType.手动控制),
            new Signal(352,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 04

        public static IList<Signal> listS4 = new List<Signal>
        {
            new Signal(41,0,"提升机皮带正转"),
            new Signal(304,0,"区域运行"),
            new Signal(304,1,"区域手动"),
            new Signal(304,2,"区域自动"),
            new Signal(304,4,"区域急停"),
            new Signal(357,0,"区域故障"),
            new Signal(150,7,"顶升就绪"),
            new Signal(357,1,"顶升运行"),
            new Signal(357,2,"顶升故障"),
            new Signal(96,0,"顶升允许取A料"),
            new Signal(96,1,"顶升允许取B料"),
            new Signal(96,2,"顶升允许取C料"),
            new Signal(96,3,"顶升允许放A料"),
            new Signal(96,4,"顶升允许放B料"),
            new Signal(96,5,"顶升允许放C料"),
        };

        public static IList<Signal> listJ4 = new List<Signal>
        {
            new Signal(41,2,"定位阻挡伸"),
            new Signal(41,3,"定位阻挡缩"),
            new Signal(41,4,"分料阻挡伸"),
            new Signal(41,5,"分料阻挡缩"),
            new Signal(41,6,"托举气缸伸"),
            new Signal(41,7,"托举气缸缩"),
            new Signal(42,0,"左夹紧气缸伸"),
            new Signal(42,1,"左夹紧气缸缩"),
            new Signal(42,2,"右夹紧气缸伸"),
            new Signal(42,3,"右夹紧气缸缩"),
            new Signal(42,4,"左顶升气缸伸"),
            new Signal(42,5,"左顶升气缸缩"),
            new Signal(42,6,"右顶升气缸伸"),
            new Signal(42,7,"右顶升气缸缩"),
        };

        public static IList<Signal> listC4 = new List<Signal>
        {
            new Signal(354,0,"定位阻挡伸", EnumSignalType.手动控制),
            new Signal(354,1,"定位阻挡缩", EnumSignalType.手动控制),
            new Signal(354,2,"分料阻挡伸", EnumSignalType.手动控制),
            new Signal(354,3,"分料阻挡缩", EnumSignalType.手动控制),
            new Signal(354,4,"托举气缸伸", EnumSignalType.手动控制),
            new Signal(354,5,"托举气缸缩", EnumSignalType.手动控制),
            new Signal(354,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(354,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(355,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(355,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(355,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new Signal(355,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new Signal(355,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new Signal(355,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new Signal(356,0,"复位", EnumSignalType.手动控制),
            //new Signal(356,1,"故障复位", EnumSignalType.手动控制),
            new Signal(356,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 05
        public static IList<Signal> listS5 = new List<Signal>
        {
            new Signal(51,0,"提升机皮带正转"),
            new Signal(305,0,"区域运行"),
            new Signal(305,1,"区域手动"),
            new Signal(305,2,"区域自动"),
            new Signal(305,4,"区域急停"),
            new Signal(363,0,"区域故障"),
            new Signal(160,7,"顶升就绪"),
            new Signal(363,1,"顶升运行"),
            new Signal(363,2,"顶升故障"),
            new Signal(96,6,"顶升允许取D料"),
            new Signal(96,7,"顶升允许放D料"),
        };

        public static IList<Signal> listJ5 = new List<Signal>
        {
            new Signal(51,2,"定位阻挡伸"),
            new Signal(51,3,"定位阻挡缩"),
            new Signal(51,4,"分料阻挡伸"),
            new Signal(51,5,"分料阻挡缩"),
            new Signal(51,6,"托举气缸伸"),
            new Signal(51,7,"托举气缸缩"),
            new Signal(52,0,"左夹紧气缸伸"),
            new Signal(52,1,"左夹紧气缸缩"),
            new Signal(52,2,"右夹紧气缸伸"),
            new Signal(52,3,"右夹紧气缸缩"),
            new Signal(52,4,"左顶升气缸伸"),
            new Signal(52,5,"左顶升气缸缩"),
            new Signal(52,6,"右顶升气缸伸"),
            new Signal(52,7,"右顶升气缸缩"),
        };

        public static IList<Signal> listC5 = new List<Signal>
        {
            new Signal(360,0,"定位阻挡伸", EnumSignalType.手动控制),
            new Signal(360,1,"定位阻挡缩", EnumSignalType.手动控制),
            new Signal(360,2,"分料阻挡伸", EnumSignalType.手动控制),
            new Signal(360,3,"分料阻挡缩", EnumSignalType.手动控制),
            new Signal(360,4,"托举气缸伸", EnumSignalType.手动控制),
            new Signal(360,5,"托举气缸缩", EnumSignalType.手动控制),
            new Signal(360,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(360,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(361,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(361,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(361,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new Signal(361,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new Signal(361,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new Signal(361,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new Signal(361,7,"复位", EnumSignalType.手动控制),
            //new Signal(362,0,"故障复位", EnumSignalType.手动控制),
            new Signal(362,1,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 06

        public static IList<Signal> listS6 = new List<Signal>
        {
            new Signal(61,0,"倍速链1正转"),
            new Signal(61,1,"倍速链2正转"),
            new Signal(306,0,"区域运行"),
            new Signal(306,1,"区域手动"),
            new Signal(306,2,"区域自动"),
            new Signal(306,4,"区域急停"),
            new Signal(367,0,"区域故障"),
            new Signal(170,7,"顶升就绪"),
            new Signal(367,1,"顶升运行"),
            new Signal(367,2,"顶升故障"),
            new Signal(116,0,"允许取工件1"),
            new Signal(116,1,"允许取工件2"),
            new Signal(116,2,"允许取工件3"),
            new Signal(116,3,"允许取工件4"),
            new Signal(116,4,"允许放装配件"),
            new Signal(116,5,"允许放工件1"),
            new Signal(116,6,"允许放工件2"),
            new Signal(116,7,"允许放工件3"),
        };

        public static IList<Signal> listJ6 = new List<Signal>
        {
            new Signal(61,2,"定位阻挡伸"),
            new Signal(61,3,"定位阻挡缩"),
            new Signal(61,4,"分料阻挡伸"),
            new Signal(61,5,"分料阻挡缩"),
            new Signal(61,6,"托举气缸伸"),
            new Signal(61,7,"托举气缸缩"),
            new Signal(62,0,"左夹紧气缸伸"),
            new Signal(62,1,"左夹紧气缸缩"),
            new Signal(62,2,"右夹紧气缸伸"),
            new Signal(62,3,"右夹紧气缸缩"),
            new Signal(62,4,"左顶升气缸伸"),
            new Signal(62,5,"左顶升气缸缩"),
            new Signal(62,6,"右顶升气缸伸"),
            new Signal(62,7,"右顶升气缸缩"),
        };

        public static IList<Signal> listC6 = new List<Signal>
        {
            new Signal(364,0,"定位阻挡伸", EnumSignalType.手动控制),
            new Signal(364,1,"定位阻挡缩", EnumSignalType.手动控制),
            new Signal(364,2,"分料阻挡伸", EnumSignalType.手动控制),
            new Signal(364,3,"分料阻挡缩", EnumSignalType.手动控制),
            new Signal(364,4,"托举气缸伸", EnumSignalType.手动控制),
            new Signal(364,5,"托举气缸缩", EnumSignalType.手动控制),
            new Signal(364,6,"左夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(364,7,"左夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(365,0,"右夹紧气缸伸", EnumSignalType.手动控制),
            new Signal(365,1,"右夹紧气缸缩", EnumSignalType.手动控制),
            new Signal(365,2,"左顶升气缸伸", EnumSignalType.手动控制),
            new Signal(365,3,"左顶升气缸缩", EnumSignalType.手动控制),
            new Signal(365,4,"右顶升气缸伸", EnumSignalType.手动控制),
            new Signal(365,5,"右顶升气缸缩", EnumSignalType.手动控制),
            new Signal(366,0,"复位", EnumSignalType.手动控制),
            //new Signal(366,1,"故障复位", EnumSignalType.手动控制),
            new Signal(366,2,"加工失败", EnumSignalType.手动控制),
        };

        #endregion

        #region 07

        public static IList<Signal> listS7 = new List<Signal>
        {
            new Signal(72,0,"提升机皮带正转"),
            new Signal(72,1,"提升机皮带反转"),
            new Signal(307,0,"区域运行"),
            new Signal(307,1,"区域手动"),
            new Signal(307,2,"区域自动"),
            new Signal(307,4,"区域急停"),
            new Signal(372,0,"区域故障"),
            new Signal(180,7,"顶升就绪"),
            new Signal(372,1,"顶升运行"),
            new Signal(372,2,"顶升故障"),
            new Signal(180,6,"提升机就绪"),
            new Signal(372,3,"提升机工作"),
            new Signal(372,4,"提升机故障"),
            new Signal(372,5,"ROB就绪"),
            new Signal(372,6,"ROB运行"),
            new Signal(372,7,"ROB故障"),
        };

        public static IList<Signal> listJ7 = new List<Signal>
        {
            new Signal(72,2,"分料阻挡伸"),
            new Signal(72,3,"分料阻挡缩"),
            new Signal(72,4,"定位阻挡伸"),
            new Signal(72,5,"定位阻挡缩"),
            new Signal(72,6,"托举气缸伸"),
            new Signal(72,7,"托举气缸缩"),
            new Signal(73,0,"提升机伸"),
            new Signal(73,1,"提升机缩"),
        };

        public static IList<Signal> listC7 = new List<Signal>
        {
            new Signal(370,0,"分料阻挡伸", EnumSignalType.手动控制),
            new Signal(370,1,"分料阻挡缩", EnumSignalType.手动控制),
            new Signal(370,2,"定位阻挡伸", EnumSignalType.手动控制),
            new Signal(370,3,"定位阻挡缩", EnumSignalType.手动控制),
            new Signal(370,4,"托举气缸伸", EnumSignalType.手动控制),
            new Signal(370,5,"托举气缸缩", EnumSignalType.手动控制),
            new Signal(370,6,"提升机伸", EnumSignalType.手动控制),
            new Signal(370,7,"提升机缩", EnumSignalType.手动控制),
            new Signal(371,0,"复位", EnumSignalType.手动控制),
            //new Signal(371,1,"故障复位", EnumSignalType.手动控制),
        };

        #endregion

        #region 08

        public static IList<Signal> listS8 = new List<Signal>
        {
            new Signal(308,0,"区域运行"),
            new Signal(308,1,"区域手动"),
            new Signal(308,2,"区域自动"),
            new Signal(308,4,"区域急停"),
            new Signal(383,0,"区域故障"),
            new Signal(383,1,"装配台就绪"),
            new Signal(383,2,"装配台故障"),
            new Signal(383,3,"刻标机就绪"),
            new Signal(383,4,"刻标机运行"),
            new Signal(383,5,"装配机器人就绪"),
            new Signal(383,6,"装配机器人运行"),
            new Signal(383,7,"装配机器人故障"),
            new Signal(382,5,"刻标机器人就绪"),
            new Signal(382,6,"刻标机器人运行"),
            new Signal(382,7,"刻标机器人故障"),
        };

        public static IList<Signal> listJ8 = new List<Signal>
        {
            new Signal(83,0,"A工件定位气缸伸"),
            new Signal(83,1,"A工件定位气缸缩"),
            new Signal(83,2,"B工件定位气缸伸"),
            new Signal(83,3,"B工件定位气缸缩"),
            new Signal(83,4,"C工件定位气缸伸"),
            new Signal(83,5,"C工件定位气缸缩"),
            new Signal(83,6,"D工件定位气缸伸"),
            new Signal(83,7,"D工件定位气缸缩"),
            new Signal(84,0,"移料气缸伸"),
            new Signal(84,1,"移料气缸缩"),
            new Signal(250,6,"移料定位气缸伸"),
            new Signal(250,7,"移料定位气缸缩"),
        };

        public static IList<Signal> listC8 = new List<Signal>
        {
            new Signal(380,0,"A工件定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(380,1,"A工件定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(380,2,"B工件定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(380,3,"B工件定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(380,4,"C工件定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(380,5,"C工件定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(380,6,"D工件定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(380,7,"D工件定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(381,0,"移料气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(381,1,"移料气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(381,2,"移料定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(381,3,"移料定位气缸缩",  Signal.EnumSignalType.手动控制),
            //new Signal(381,5,"清洗机复位",  Signal.EnumSignalType.手动控制),
            new Signal(381,6,"装配台复位",  Signal.EnumSignalType.手动控制),
            //new Signal(381,7,"故障复位",  Signal.EnumSignalType.手动控制),
            new Signal(382,0,"工件A复位",  Signal.EnumSignalType.手动控制),
            new Signal(382,1,"工件B复位",  Signal.EnumSignalType.手动控制),
            new Signal(382,2,"工件C复位",  Signal.EnumSignalType.手动控制),
            new Signal(382,3,"工件D复位",  Signal.EnumSignalType.手动控制),
        };

        #endregion

        #region 09

        public static IList<Signal> listS9 = new List<Signal>
        {
            new Signal(309,0,"区域运行"),
            new Signal(309,1,"区域手动"),
            new Signal(309,2,"区域自动"),
            new Signal(309,4,"区域急停"),
            new Signal(392,0,"区域故障"),
            new Signal(200,0,"翻转机构就绪"),
            new Signal(392,1,"翻转机构运行"),
            new Signal(392,2,"翻转机构故障"),
            new Signal(392,3,"ROB就绪"),
            new Signal(392,4,"ROB运行"),
            new Signal(392,5,"ROB故障"),
            new Signal(92,0,"三轴循环运行"),
            new Signal(92,1,"三轴空闲"),
            new Signal(92,2,"三轴故障"),
            new Signal(92,3,"三轴允许上料"),
            new Signal(92,4,"三轴允许下料"),
            new Signal(93,0,"五轴轴循环运行"),
            new Signal(93,1,"五轴空闲"),
            new Signal(93,2,"五轴故障"),
            new Signal(93,3,"五轴允许上料"),
            new Signal(93,4,"五轴允许下料"),
        };

        public static IList<Signal> listJ9 = new List<Signal>
        {
            new Signal(93,0,"翻转气缸伸"),
            new Signal(93,1,"翻转气缸缩"),
            new Signal(93,2,"放料夹紧气缸伸"),
            new Signal(93,3,"放料夹紧气缸缩"),
            new Signal(93,4,"取料气缸伸"),
            new Signal(93,5,"取料气缸伸"),
        };

        public static IList<Signal> listC9 = new List<Signal>
        {
            new Signal(390,0,"翻转气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(390,1,"翻转气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(390,2,"放料夹紧气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(390,3,"放料夹紧气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(390,4,"取料气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(390,5,"取料气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(391,0,"复位",  Signal.EnumSignalType.手动控制),
            //new Signal(391,1,"故障复位",  Signal.EnumSignalType.手动控制),
        };

        #endregion

        #region 10

        public static IList<Signal> listS10 = new List<Signal>
        {
            new Signal(310,0,"区域运行"),
            new Signal(310,1,"区域手动"),
            new Signal(310,2,"区域自动"),
            new Signal(310,4,"区域急停"),
            new Signal(396,4,"区域故障"),
            new Signal(396,5,"ROB就绪"),
            new Signal(396,6,"ROB运行"),
            new Signal(396,7,"ROB故障"),
            new Signal(102,0,"车床循环运行"),
            new Signal(102,1,"车床空闲"),
            new Signal(102,2,"车床故障"),
            new Signal(102,3,"车床允许上料"),
            new Signal(102,4,"车床允许下料"),
            new Signal(103,0,"钻攻中心循环运行"),
            new Signal(103,1,"钻攻中心空闲"),
            new Signal(103,2,"钻攻中心故障"),
            new Signal(103,3,"钻攻中心允许上料"),
            new Signal(103,4,"钻攻中心允许下料"),
            new Signal(336,0,"台1就绪"),
            new Signal(336,1,"台1运行"),
            new Signal(336,2,"台1故障"),
            new Signal(336,3,"台2就绪"),
            new Signal(336,4,"台2运行"),
            new Signal(336,5,"台2故障"),
            new Signal(336,6,"台3就绪"),
            new Signal(336,7,"台3运行"),
            new Signal(337,0,"台3故障"),
            new Signal(337,1,"台4就绪"),
            new Signal(337,2,"台4运行"),
            new Signal(337,3,"台4故障"),
        };

        public static IList<Signal> listJ10 = new List<Signal>
        {
            new Signal(260,0,"台1定位气缸伸"),
            new Signal(260,1,"台1定位气缸缩"),
            new Signal(1,6,"台1阻挡气缸伸"),
            new Signal(1,7,"台1阻挡气缸缩"),
            new Signal(264,2,"台2定位气缸伸"),
            new Signal(264,3,"台2定位气缸缩"),
            new Signal(2,6,"台2阻挡气缸伸"),
            new Signal(2,7,"台2阻挡气缸缩"),
            new Signal(220,0,"台3定位气缸伸"),
            new Signal(220,1,"台3定位气缸缩"),
            new Signal(224,0,"台4定位气缸伸"),
            new Signal(224,1,"台4定位气缸缩"),
        };

        public static IList<Signal> listC10 = new List<Signal>
        {
            new Signal(394,0,"台1定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(394,1,"台1定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(394,2,"台1阻挡气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(394,3,"台1阻挡气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(394,4,"台2定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(394,5,"台2定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(394,6,"台2阻挡气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(394,7,"台2阻挡气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(395,0,"台3定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(395,1,"台3定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(395,2,"台4定位气缸伸",  Signal.EnumSignalType.手动控制),
            new Signal(395,3,"台4定位气缸缩",  Signal.EnumSignalType.手动控制),
            new Signal(395,5,"台1复位",  Signal.EnumSignalType.手动控制),
            new Signal(395,6,"台2复位",  Signal.EnumSignalType.手动控制),
            new Signal(395,7,"台3复位",  Signal.EnumSignalType.手动控制),
            new Signal(396,0,"台4复位",  Signal.EnumSignalType.手动控制),
            new Signal(396,1,"复位",  Signal.EnumSignalType.手动控制),
            new Signal(373,0,"台1滚筒正转",  Signal.EnumSignalType.手动控制),
            new Signal(373,1,"台1滚筒反转",  Signal.EnumSignalType.手动控制),
            new Signal(373,2,"台2滚筒正转",  Signal.EnumSignalType.手动控制),
            new Signal(373,3,"台2滚筒反转",  Signal.EnumSignalType.手动控制),
            new Signal(373,4,"台3滚筒正转",  Signal.EnumSignalType.手动控制),
            new Signal(373,5,"台3滚筒反转",  Signal.EnumSignalType.手动控制),
            new Signal(373,6,"台4滚筒正转",  Signal.EnumSignalType.手动控制),
            new Signal(373,7,"台4滚筒反转",  Signal.EnumSignalType.手动控制),
        };

        #endregion

    }

}