using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private async void Splash_Shown(object sender, EventArgs e)
        {
            await RunProgress();
            this.Close();
        }

        private readonly IList<string> Logs = new List<string> {
            "系统正在加载中","连接数据库","获取基础数据","连接PLC","连接机床",
            "加载核心服务","加载Web服务","加载外部服务","系统加载完成","进入控制界面",
        };

        public event EventHandler LoadingComplete;

        public async Task RunProgress()
        {
            await Task.Run(() =>
            {
                for (int i = 0; this.Visible; i++)
                {
                    Thread.Sleep(300);
                    this.InvokeEx(() =>
                    {
                        if (progressBar.Value < progressBar.Maximum)
                        {
                            richTextBox.AppendText(DateTime.Now.ToString() + "\t" + Logs[i] + Environment.NewLine);
                            progressBar.Value += progressBar.Step;
                        }
                        else
                        {
                            this.Visible = false;
                            if (LoadingComplete != null)
                            {
                                LoadingComplete(null, new EventArgs());
                            }
                        }
                    });
                }
            });

        }
    }
}
