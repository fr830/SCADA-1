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

namespace SCADA
{
    public partial class UserControl_RFID : UserControl
    {
        private UserControl_RFID()
        {
            InitializeComponent();
        }

        public UserControl_RFID(string ip, HFReader hfReader)
        {
            IPAddress = "192.168.1." + ip;
            HFReader = hfReader;
            InitializeComponent();
        }

        private void UserControl_RFID_Load(object sender, EventArgs e)
        {
            labelIP.Text = IPAddress;
            HeartBeat();
            HFReader.AutoReadHandler += HFReader_AutoReadHandler;
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            HFReader.Connect(IPAddress, 3001);
            buttonConnect.Enabled = true;
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            buttonDisconnect.Enabled = false;
            HFReader.DisConnect();
            buttonDisconnect.Enabled = true;
        }

        private void HeartBeat()
        {
            Task.Run(() =>
            {
                while (!this.IsDisposed)
                {
                    Thread.Sleep(500);
                    labelStatus.InvokeEx(c =>
                    {
                        switch (HFReader.ConnectStatus)
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

        void HFReader_AutoReadHandler(object sender, AutoReadEventArgs e)
        {
            var data = e.UID;
        }

        public string IPAddress { get; private set; }

        public HFReader HFReader { get; private set; }

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
         * 第七位开始表示要进入
         */
        const string DefaultRFIDData = "3351100200000000";//默认标签数据：E装配成品，需要清洗和检测


    }
}
