using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sygole.HFReader;
using System.Threading;
using System.Reflection;

namespace SCADA
{
    public partial class UserControl_RFID : UserControl
    {
        private UserControl_RFID()
        {
            InitializeComponent();
        }

        public RFIDItem Item { get; private set; }

        public UserControl_RFID(RFIDItem item)
        {
            Item = item;
            InitializeComponent();
        }

        private void UserControl_RFID_Load(object sender, EventArgs e)
        {
            labelIP.Text = Item.IPAddress;
            labelPort.Text = Item.Port.ToString();
            HeartBeatAsync();
            Item.HFReader.AutoReadHandler += HFReader_AutoReadHandler;
            Item.HFReader.CommEvent.CommReceiveHandler += CommEvent_CommReceiveHandler;
            Item.HFReader.CommEvent.CommSendHandler += CommEvent_CommSendHandler;
        }

        private void WriteLog(string text)
        {
            richTextBoxLog.InvokeEx(c => c.AppendText(string.Format("{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, text, Environment.NewLine)));
        }

        void HFReader_AutoReadHandler(object sender, AutoReadEventArgs e)
        {
            var text = string.Format("自动读取，天线{0}，UID：{1}{2}", ((int)(e.ant + 1)).ToString(), ConvertHelper.BytesToHexString(e.UID), Environment.NewLine);
            //WriteLog(text);
        }

        void CommEvent_CommSendHandler(object sender, CommEventArgs e)
        {
            var text = "请求数据:" + Environment.NewLine + ConvertHelper.BytesToHexString(e.CommDatas, e.CommDatasLen);
            WriteLog(text);
        }

        void CommEvent_CommReceiveHandler(object sender, CommEventArgs e)
        {
            var text = "响应数据:" + Environment.NewLine + ConvertHelper.BytesToHexString(e.CommDatas, e.CommDatasLen);
            WriteLog(text);
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            await Item.ConnectAsync();
            buttonConnect.Enabled = true;
        }

        private async void buttonDisconnect_Click(object sender, EventArgs e)
        {
            buttonDisconnect.Enabled = false;
            await Item.DisconnectAsync();
            buttonDisconnect.Enabled = true;
        }

        private async Task HeartBeatAsync()
        {
            await Task.Run(() =>
            {
                while (!this.IsDisposed)
                {
                    Thread.Sleep(500);
                    labelStatus.InvokeEx(c =>
                    {
                        switch (Item.HFReader.ConnectStatus)
                        {
                            case ConnectStatusEnum.CONNECTED:
                                c.Text = "通讯正常";
                                buttonConnect.InvokeEx(bc => bc.Enabled = false);
                                buttonDisconnect.InvokeEx(bd => bd.Enabled = true);
                                break;
                            case ConnectStatusEnum.CONNECTING:
                                c.Text = "正在连接";
                                buttonConnect.InvokeEx(bc => bc.Enabled = false);
                                buttonDisconnect.InvokeEx(bd => bd.Enabled = true);
                                break;
                            case ConnectStatusEnum.CONNECTLOST:
                                c.Text = "通讯中断";
                                buttonConnect.InvokeEx(bc => bc.Enabled = true);
                                buttonDisconnect.InvokeEx(bd => bd.Enabled = false);
                                break;
                            case ConnectStatusEnum.DISCONNECTED:
                                c.Text = "连接断开";
                                buttonConnect.InvokeEx(bc => bc.Enabled = true);
                                buttonDisconnect.InvokeEx(bd => bd.Enabled = false);
                                break;
                            default:
                                break;
                        }
                    });
                }
            });
        }

        /// <summary>
        /// 工件[{0,无},{1,A小圆},{2,B中圆},{3,C大圆},{4,D底座},{5,E装配成品}]
        /// </summary>
        enum EnumWorkpiece { None = 0, A, B, C, D, E }

        /// <summary>
        /// 清洗[{0,不清洗},{1,清洗},{2,清洗完成},{3,清洗失败}]
        /// </summary>
        enum EnumClean { Unwanted = 0, Wanted, Finished, Failed }

        /// <summary>
        /// 检测[{0,不检测},{1,检测},{2,检测完成},{3,检测失败}]
        /// </summary>
        enum EnumGauge { Unwanted = 0, Wanted, Finished, Failed }

        /// <summary>
        /// 检测结果[{0,待检测},{1,检测合格},{2,检测不合格}]
        /// </summary>
        enum EnumGaugeResult { Waiting = 0, Qualified, Unqualified }

        /*
         * RFID标签数据格式
         * 33为思谷标签固定标识
         * 第三位（工件类型）：[{0,无},{1,A小圆},{2,B中圆},{3,C大圆},{4,D底座},{5,E装配成品}]
         * 第四位（清洗状态）：[{0,不清洗},{1,清洗},{2,清洗完成},{3,清洗失败}]
         * 第五位（检测状态）：[{0,不检测},{1,检测},{2,检测完成},{3,检测失败}]
         * 第六位（检测结果）：[{0,待检测},{1,检测合格},{2,检测不合格}]
         * 第七位（完成工序）：数值表示已顺序完成n道工序
         * 第八位开始，表示要进行的工序：数值表示第n道工序
         */

        /// <summary>
        /// 默认标签数据：E装配成品，清洗，检测，待检测，完成0道工序，加工-进入5
        /// </summary>
        const string DefaultRFIDData = "33" + "5" + "1" + "1" + "0" + "0" + "500000000";

        private void richTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            richTextBoxLog.ScrollToCaret();
        }


    }
}
