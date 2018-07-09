using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNC.MES.Model;
using HNC.MES.Common;

namespace SCADA
{
    public partial class _Layout : Form
    {
        List<Type> TabPageFormTypes = new List<Type>() { typeof(Home) };

        public _Layout()
        {
            InitializeComponent();
#if DEBUG
            FormClosing -= _Layout_FormClosing;
#endif
        }

        private void _Layout_Load(object sender, EventArgs e)
        {
            menuStrip.Visible = false;//暂时不启用菜单栏
            InitStatus();
            InitInfo();
            InitTabPage();
        }

        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitStatus()
        {
            toolStripStatusLabelDateTime.Alignment = ToolStripItemAlignment.Right;
            toolStripStatusLabelRunningTime.Alignment = ToolStripItemAlignment.Right;
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (s, e) =>
            {
                timer.Stop();
                statusStrip.InvokeEx(c =>
                {
                    toolStripStatusLabelDateTime.Text = "当前时间：" + DateTime.Now.ToString();
                    toolStripStatusLabelRunningTime.Text = "系统运行时间：" + (DateTime.Now - My.StartTime).ToString(@"d\天hh\:mm\:ss");
                });
                timer.Start();
            };
            timer.Start();
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void InitInfo()
        {
            richTextBoxInfo.SelectionColor = Color.Black;
            richTextBoxInfo.AppendText("名称：" + Environment.NewLine);
            richTextBoxInfo.SelectionColor = Color.Red;
            richTextBoxInfo.AppendText(My.LocationName + Environment.NewLine);
            //richTextBoxInfo.SelectionColor = Color.Black;
            //richTextBoxInfo.AppendText(new string('─', 14) + Environment.NewLine);
            //richTextBoxInfo.SelectionColor = Color.Black;
            //richTextBoxInfo.AppendText("提示：" + Environment.NewLine);
            //richTextBoxInfo.SelectionColor = Color.Black;
            //richTextBoxInfo.AppendText("查看产线运行状态，请单击下方按钮" + Environment.NewLine);
        }

        /// <summary>
        /// 初始化TabPage
        /// </summary>
        /// <param name="tabPage"></param>
        private void InitTabPage()
        {
            for (int i = 0; i < TabPageFormTypes.Count; i++)
            {
                TabPage page = new TabPage();
                Form frm = Activator.CreateInstance(TabPageFormTypes[i]) as Form;
                if (frm == null)
                {
                    continue;
                }
                page.Text = frm.Text;
                frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm.TopLevel = false;
                frm.Parent = page;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                tabControl.TabPages.Add(page);
            }
        }

        private void _Layout_FormClosing(object sender, FormClosingEventArgs e)
        {
            var r = MessageBox.Show("是否退出系统？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void buttonMES_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://localhost/");
        }

        private void buttonService_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://localhost:41150/ScadaService");
        }

        private void richTextBoxInfo_Enter(object sender, EventArgs e)
        {
            buttonRun.Focus();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            buttonRun.Enabled = false;
            if (My.CoreService.IsRunning)
            {
                My.CoreService.Stop();
            }
            else
            {
                My.CoreService.Start();
            }
            Color c = My.CoreService.IsRunning ? Color.Green : Color.Red;
            pictureBoxStatus.Image = new Bitmap(pictureBoxStatus.Width, pictureBoxStatus.Height);
            var graph = Graphics.FromImage(pictureBoxStatus.Image);
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graph.FillEllipse(new SolidBrush(c), 10, 10, pictureBoxStatus.Width - 20, pictureBoxStatus.Height - 20);
            graph.Save();
            labelStatus.ForeColor = c;
            labelStatus.Text = My.CoreService.IsRunning ? "运行" : "停止";
            buttonRun.Text = My.CoreService.IsRunning ? "停止" : "启动";
            buttonRun.Enabled = true;
        }
    }
}
