﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class UserControl_Area : UserControl
    {
        private UserControl_Area()
        {
            InitializeComponent();
        }

        public UserControl_Area(string title, IList<Signal> listS, IList<Signal> listJ, IList<Signal> listC)
        {
            InitializeComponent();
            groupBox.Text = title;
            groupBox.Controls.Add(CreateGroupBox("按钮", listC));
            groupBox.Controls.Add(CreateGroupBox("监控", listJ));
            groupBox.Controls.Add(CreateGroupBox("监控", listS));
        }

        private System.Timers.Timer timer = new System.Timers.Timer(1000);

        private void UserControl_Area_Load(object sender, EventArgs e)
        {
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            RefreshAllButtons(groupBox);
            timer.Start();
        }

        private GroupBox CreateGroupBox(string groupBoxText, IList<Signal> list)
        {
            var box = new GroupBox();
            box.Dock = DockStyle.Top;
            box.Height = 200;
            box.Text = groupBoxText;
            var panel = new FlowLayoutPanel();
            box.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    panel.Controls.Add(CreateButton(list[i]));
                }
            }
            return box;
        }

        private Button CreateButton(Signal c)
        {
            var btn = new Button();
            btn.AutoSize = true;
            btn.FlatStyle = FlatStyle.Flat;
            //btn.Font = new Font("微软雅黑", 15, FontStyle.Regular);
            btn.Tag = c;
            btn.Text = c.Text;
            switch (c.SignalType)
            {
                case Signal.EnumSignalType.状态监控:
                    btn.Enabled = false;
                    btn.BackColor = My.PLC.Exist(c.Index, c.Bit) ? Color.Lime : Color.Transparent;
                    break;
                case Signal.EnumSignalType.手动控制:
                    btn.Enabled = true;
                    btn.BackColor = Color.DodgerBlue;
                    btn.ForeColor = Color.Transparent;
                    btn.MouseDown += (ds, de) => My.PLC.Set(c.Index, c.Bit);
                    btn.MouseUp += (us, ue) => My.PLC.Clear(c.Index, c.Bit);
                    break;
                default:
                    btn.Enabled = false;
                    break;
            }
            return btn;
        }

        private void RefreshAllButtons(Control c)
        {
            foreach (Control item in c.Controls)
            {
                if (item.HasChildren)
                {
                    RefreshAllButtons(item);
                }
                else if (item is Button)
                {
                    var btn = item as Button;
                    var content = (Signal)btn.Tag;
                    if (content.SignalType == Signal.EnumSignalType.状态监控)
                    {
                        btn.InvokeEx(b =>
                        {
                            b.BackColor = My.PLC.Exist(content.Index, content.Bit) ? Color.Lime : Color.Transparent;
                        });
                    }
                }
            }
        }
    }
}
