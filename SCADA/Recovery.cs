using HNC.MES.Common;
using HNC.MES.Model;
using RFID;
using System;
using System.Collections;
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
    public partial class Recovery : Form
    {
        public Recovery()
        {
            InitializeComponent();
        }

        private void Recovery_Load(object sender, EventArgs e)
        {
            SetComboboxDataSource(comboBoxRFIDs, My.RFIDs);
        }

        private void Recovery_Shown(object sender, EventArgs e)
        {
            button4.PerformClick();
            button5.PerformClick();
            button7.PerformClick();
        }

        private void SetComboboxDataSource(ComboBox cb, IDictionary dict)
        {
            cb.DataSource = new BindingSource(dict, "");
            cb.DisplayMember = "Key";
            cb.ValueMember = "Value";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            await My.PLC.SetForceAsync(11, 2);//请求入库
            button1.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            await My.PLC.SetForceAsync(11, 0);//请求出库
            button2.Enabled = true;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            await My.PLC.SetForceAsync(1, 0);//请求相机拍照
            button3.Enabled = true;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            var data = My.RFIDs[EnumPSite.S8_Down].Read();
            if (data == null)
            {
                labelRFID.Text = string.Empty;
            }
            else
            {
                My.Work_Vision.RFIDData = data;
                labelRFID.Text = data.ToString();
                await Task.Delay(1000);
                labelRFID.Text = string.Empty;
            }
            button4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            label1.Text = "服务状态：" + (My.Work_Simulation.IsRunning ? "运行" : "停止");
            label2.Text = "待发送消息数量：" + My.Work_Simulation.WaitForSendingCount;
            button5.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            var order = My.BLL.TOrder.GetModel(Tool.CreateDict("State", EnumHelper.GetName(TOrder.EnumState.执行)));
            if (order != null)
            {
                order.State = EnumHelper.GetName(TOrder.EnumState.启动);
                My.BLL.TOrder.Update(order, My.AdminID);
            }
            button6.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            label3.Text = "服务状态：" + (My.Work_PLC.IsRunning ? "运行" : "停止");
            button7.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            var item = comboBoxRFIDs.SelectedValue as RFIDReader;
            My.PLC.Set(Work_PLC.SiteIndexDict[item.Site], 0);
            button8.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button9.Enabled = false;
            var dr = MessageBox.Show("重置后当前所有的数据将清空并且不可恢复，确认进行重置？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                My.BLL.TLocation.EmptyData();
                MessageBox.Show("软件即将关闭，请重新启动此软件应用新的数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            button9.Enabled = true;
        }
    }
}
