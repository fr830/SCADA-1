using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private IList<UserControl_Monitor> monitors = new List<UserControl_Monitor>
        {
            new UserControl_Monitor("01单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(12,0,"正转"),
                    new Signal(12,1,"反转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(301,0,"运行"),
                    new Signal(301,1,"手动"),
                    new Signal(301,2,"自动"),
                    new Signal(301,4,"急停",Color.Red,false),
                    new Signal(343,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.机器人,new List<Signal>{
                    new Signal(343,1,"就绪"),
                    new Signal(343,2,"运行"),
                    new Signal(343,3,"故障",Color.Red),
                }},
                {EnumMonitorItems.分料机构,new List<Signal>{
                    new Signal(343,4,"就绪"),
                    new Signal(343,5,"运行"),
                    new Signal(343,6,"故障",Color.Red),
                }},
                {EnumMonitorItems.提升机,new List<Signal>{
                    new Signal(130,7,"就绪"),
                    new Signal(342,0,"运行"),
                    new Signal(342,1,"故障",Color.Red),
                }},
            }),
            new UserControl_Monitor("02单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(21,0,"正转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(302,0,"运行"),
                    new Signal(302,1,"手动"),
                    new Signal(302,2,"自动"),
                    new Signal(302,4,"急停",Color.Red,false),
                    new Signal(347,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(120,7,"就绪"),
                    new Signal(347,1,"运行"),
                    new Signal(347,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.取料状态,new List<Signal>{
                    new Signal(106,0,"允许取A料"),
                    new Signal(106,1,"允许取B料"),
                    new Signal(106,2,"允许取C料"),
                    new Signal(106,3,"允许放A料"),
                    new Signal(106,4,"允许放B料"),
                    new Signal(106,5,"允许放C料"),
                }},
            }),
            new UserControl_Monitor("03单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(31,0,"正转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(303,0,"运行"),
                    new Signal(303,1,"手动"),
                    new Signal(303,2,"自动"),
                    new Signal(303,4,"急停",Color.Red,false),
                    new Signal(353,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(140,7,"就绪"),
                    new Signal(353,1,"运行"),
                    new Signal(353,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.取料状态,new List<Signal>{
                    new Signal(106,6,"允许取A料"),
                    new Signal(106,7,"允许放A料"),
                }},
            }),
            new UserControl_Monitor("04单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(41,0,"正转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(304,0,"运行"),
                    new Signal(304,1,"手动"),
                    new Signal(304,2,"自动"),
                    new Signal(304,4,"急停",Color.Red,false),
                    new Signal(357,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(150,7,"就绪"),
                    new Signal(357,1,"运行"),
                    new Signal(357,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.取料状态,new List<Signal>{
                    new Signal(96,0,"允许取A料"),
                    new Signal(96,1,"允许取B料"),
                    new Signal(96,2,"允许取C料"),
                    new Signal(96,3,"允许放A料"),
                    new Signal(96,4,"允许放B料"),
                    new Signal(96,5,"允许放C料"),
                }},
            }),
            new UserControl_Monitor("05单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(51,0,"正转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(305,0,"运行"),
                    new Signal(305,1,"手动"),
                    new Signal(305,2,"自动"),
                    new Signal(305,4,"急停",Color.Red,false),
                    new Signal(363,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(160,7,"就绪"),
                    new Signal(363,1,"运行"),
                    new Signal(363,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.取料状态,new List<Signal>{
                    new Signal(96,6,"允许取D料"),
                    new Signal(96,7,"允许放D料"),
                }},
            }),
            new UserControl_Monitor("06单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(61,0,"倍速链1正转"),
                    new Signal(61,1,"倍速链2正转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(306,0,"运行"),
                    new Signal(306,1,"手动"),
                    new Signal(306,2,"自动"),
                    new Signal(306,4,"急停",Color.Red,false),
                    new Signal(367,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(170,7,"就绪"),
                    new Signal(367,1,"运行"),
                    new Signal(367,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.取料状态,new List<Signal>{
                    new Signal(116,0,"允许取A料"),
                    new Signal(116,1,"允许取B料"),
                    new Signal(116,2,"允许取C料"),
                    new Signal(116,3,"允许取D料"),
                    new Signal(116,4,"允许放装配件"),
                    new Signal(116,5,"允许放A料"),
                    new Signal(116,6,"允许放B料"),
                    new Signal(116,7,"允许放C料"),
                }},
            }),
            new UserControl_Monitor("07单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.提升机皮带,new List<Signal>{
                    new Signal(72,0,"正转"),
                    new Signal(72,1,"反转"),
                }},
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(307,0,"运行"),
                    new Signal(307,1,"手动"),
                    new Signal(307,2,"自动"),
                    new Signal(307,4,"急停",Color.Red,false),
                    new Signal(372,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.顶升机构,new List<Signal>{
                    new Signal(180,7,"就绪"),
                    new Signal(372,1,"运行"),
                    new Signal(372,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.提升机,new List<Signal>{
                    new Signal(180,6,"就绪"),
                    new Signal(372,3,"运行"),
                    new Signal(372,4,"故障",Color.Red),
                }},
                {EnumMonitorItems.机器人,new List<Signal>{
                    new Signal(372,5,"就绪"),
                    new Signal(372,6,"运行"),
                    new Signal(372,7,"故障",Color.Red),
                }},
            }),
            new UserControl_Monitor("08单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(308,0,"运行"),
                    new Signal(308,1,"手动"),
                    new Signal(308,2,"自动"),
                    new Signal(308,4,"急停",Color.Red,false),
                    new Signal(383,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.装配台,new List<Signal>{
                    new Signal(383,1,"就绪"),
                    new Signal(384,4,"运行"),
                    new Signal(383,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.刻标机,new List<Signal>{
                    new Signal(383,3,"就绪"),
                    new Signal(383,4,"运行"),
                    //new Signal(383,4,"故障",Color.Red),
                }},
                {EnumMonitorItems.装配机器人,new List<Signal>{
                    new Signal(383,5,"就绪"),
                    new Signal(383,6,"运行"),
                    new Signal(383,7,"故障",Color.Red),
                }},
                {EnumMonitorItems.刻标机器人,new List<Signal>{
                    new Signal(382,5,"就绪"),
                    new Signal(382,6,"运行"),
                    new Signal(382,7,"故障",Color.Red),
                }},
            }),
            new UserControl_Monitor("09单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(309,0,"运行"),
                    new Signal(309,1,"手动"),
                    new Signal(309,2,"自动"),
                    new Signal(309,4,"急停",Color.Red,false),
                    new Signal(392,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.翻转机构,new List<Signal>{
                    new Signal(200,0,"就绪"),
                    new Signal(392,1,"运行"),
                    new Signal(392,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.机器人,new List<Signal>{
                    new Signal(392,3,"就绪"),
                    new Signal(392,4,"运行"),
                    new Signal(392,5,"故障",Color.Red),
                }},
                {EnumMonitorItems.三轴,new List<Signal>{
                    new Signal(92,0,"循环运行"),
                    new Signal(92,1,"空闲"),
                    new Signal(92,2,"故障",Color.Red),
                    new Signal(92,3,"允许上料"),
                    new Signal(92,4,"允许下料"),
                }},
                {EnumMonitorItems.五轴,new List<Signal>{
                    new Signal(93,0,"循环运行"),
                    new Signal(93,1,"空闲"),
                    new Signal(93,2,"故障",Color.Red),
                    new Signal(93,3,"允许上料"),
                    new Signal(93,4,"允许下料"),
                }},
            }),
            new UserControl_Monitor("10单元",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(310,0,"运行"),
                    new Signal(310,1,"手动"),
                    new Signal(310,2,"自动"),
                    new Signal(310,4,"急停",Color.Red,false),
                    new Signal(396,4,"故障",Color.Red),
                }},
                {EnumMonitorItems.机器人,new List<Signal>{
                    new Signal(396,5,"就绪"),
                    new Signal(396,6,"运行"),
                    new Signal(396,7,"故障",Color.Red),
                }},
                {EnumMonitorItems.车床,new List<Signal>{
                    new Signal(102,0,"循环运行"),
                    new Signal(102,1,"空闲"),
                    new Signal(102,2,"故障",Color.Red),
                    new Signal(102,3,"允许上料"),
                    new Signal(102,4,"允许下料"),
                }},
                {EnumMonitorItems.钻攻中心,new List<Signal>{
                    new Signal(103,0,"循环运行"),
                    new Signal(103,1,"空闲"),
                    new Signal(103,2,"故障",Color.Red),
                    new Signal(103,3,"允许上料"),
                    new Signal(103,4,"允许下料"),
                }},
            }),
            new UserControl_Monitor("10单元(台)",new Dictionary<EnumMonitorItems,IList<Signal>>
            {
                {EnumMonitorItems.区域,new List<Signal>{
                    new Signal(310,0,"运行"),
                    new Signal(310,1,"手动"),
                    new Signal(310,2,"自动"),
                    new Signal(310,4,"急停",Color.Red,false),
                    new Signal(396,4,"故障",Color.Red),
                }},
                {EnumMonitorItems.台1,new List<Signal>{
                    new Signal(336,0,"就绪"),
                    new Signal(336,1,"运行"),
                    new Signal(336,2,"故障",Color.Red),
                }},
                {EnumMonitorItems.台2,new List<Signal>{
                    new Signal(336,3,"就绪"),
                    new Signal(336,4,"运行"),
                    new Signal(336,5,"故障",Color.Red),
                }},
                {EnumMonitorItems.台3,new List<Signal>{
                    new Signal(336,6,"就绪"),
                    new Signal(336,7,"运行"),
                    new Signal(337,0,"故障",Color.Red),
                }},
                {EnumMonitorItems.台4,new List<Signal>{
                    new Signal(337,1,"就绪"),
                    new Signal(337,2,"运行"),
                    new Signal(337,3,"故障",Color.Red),
                }},
            }),
        };

        private void Home_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < monitors.Count; i++)
            {
                flowLayoutPanel.Controls.Add(monitors[i]);
            }
        }

        private void Home_Shown(object sender, EventArgs e)
        {

        }
    }
}
