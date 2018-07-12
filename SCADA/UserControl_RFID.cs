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
using RFID;

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
            Item.HFReader.AutoReadHandler += HFReader_AutoReadHandler;
            Item.HFReader.CommEvent.CommReceiveHandler += CommEvent_CommReceiveHandler;
            Item.HFReader.CommEvent.CommSendHandler += CommEvent_CommSendHandler;
            HeartBeatAsync();
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
        
        private void richTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            richTextBoxLog.ScrollToCaret();
        }


    }
}
