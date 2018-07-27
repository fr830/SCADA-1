using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SCADA
{
    public enum EnumMonitorItems
    {
        提升机皮带, 区域, 机器人, 分料机构, 提升机, 顶升机构, 取料状态, 装配台, 刻标机, 装配机器人, 刻标机器人, 翻转机构, 三轴, 五轴, 车床, 钻攻中心, 台1, 台2, 台3, 台4
    }

    public partial class UserControl_Monitor : UserControl
    {
        private UserControl_Monitor()
        {
            InitializeComponent();
        }

        public IDictionary<EnumMonitorItems, IList<Signal>> Dict { get; private set; }

        public UserControl_Monitor(string title, IDictionary<EnumMonitorItems, IList<Signal>> dict)
            : this()
        {
            label1.Text = title;
            Dict = new Dictionary<EnumMonitorItems, IList<Signal>>(dict);
        }

        private void UserControl_Monitor_Load(object sender, EventArgs e)
        {
            foreach (var item in Dict)
            {
                if (item.Key == EnumMonitorItems.提升机皮带)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        var s = new UserControl_SignalPB(item.Value[i]);
                        flowLayoutPanel2.Controls.Add(s);
                    }
                }
                else if (item.Key == EnumMonitorItems.区域)
                {
                    #region 区域
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(1000);
                            var zd = item.Value.FirstOrDefault(p => p.Text == "自动");
                            var sd = item.Value.FirstOrDefault(p => p.Text == "手动");
                            var jt = item.Value.FirstOrDefault(p => p.Text == "急停");
                            var gz = item.Value.FirstOrDefault(p => p.Text == "故障");
                            var yx = item.Value.FirstOrDefault(p => p.Text == "运行");

                            label3.InvokeEx(c =>
                            {
                                c.Text = zd.IsExpected ? zd.Text : sd.Text;
                                c.ForeColor = zd.IsExpected ? zd.Color : sd.Color;
                            });

                            label5.InvokeEx(c =>
                            {
                                Signal signal = null;
                                if (jt.IsExpected)
                                {
                                    signal = jt;
                                }
                                else if (gz.IsExpected)
                                {
                                    signal = gz;
                                }
                                else if (yx.IsExpected)
                                {
                                    signal = yx;
                                }
                                if (signal == null)
                                {
                                    c.Text = "停止";
                                    c.ForeColor = Color.LightGray;
                                }
                                else
                                {
                                    c.Text = signal.Text;
                                    c.ForeColor = signal.Color;
                                }
                            });

                        }
                    });
                    #endregion
                }
                else
                {
                    flowLayoutPanel1.Controls.Add(new UserControl_SignalTB(Enum.GetName(typeof(EnumMonitorItems), item.Key) + "：", item.Value));
                }
            }
        }


    }
}
