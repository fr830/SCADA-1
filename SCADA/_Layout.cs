using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNC.MES.Common;
using HNC.MES.Model;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using NLog.Windows.Forms;

namespace SCADA
{
    public partial class _Layout : Form
    {
        private static Logger logger;

        public _Layout()
        {
            InitializeComponent();
        }

        private IList<Type> TabPages = new List<Type>
        { 
            typeof(Home),typeof(RFIDPage),typeof(DebugPLC),typeof(Recovery)
        };

        private void _Layout_Load(object sender, EventArgs e)
        {
            panelLog.Visible = false;//默认不显示日志
            InitLog();
            logger.Info("系统启动");
#if DEBUG
            TabPages.Add(typeof(Debug));
            FormClosing -= _Layout_FormClosing;
#endif
            menuStrip.Visible = false;//暂时不启用菜单栏
            this.Visible = false;
            InitInfo();
            InitStatus();
            var splash = new Splash();
            splash.Show();
            My.LoadCompleted += async (ms, me) =>
            {
                if (me.Value >= 100)
                {
                    this.InvokeEx(c => c.InitTabPage());
                    await Task.Delay(1000);
                    splash.InvokeEx(c => c.Close());
                    this.InvokeEx(c => c.Visible = true);
                    GetSignalStateAsync();
                }
            };
            My.InitializeAsync();
        }

        private void InitLog()
        {
            var config = new LoggingConfiguration();
            var logfile = new FileTarget("logfile") { FileName = "${basedir}/logs/${shortdate}.log" };
            var logconsole = new ConsoleTarget("logconsole");
            var logrichtextbox = new RichTextBoxTarget();

            logrichtextbox.Name = "RichTextBox";
            //target.Layout = "${longdate} ${level:uppercase=true} ${logger} ${message}";
            logrichtextbox.Layout = "${longdate} ${uppercase:${level}} ${message}";
            logrichtextbox.ControlName = "richTextBoxLog";
            logrichtextbox.FormName = "_Layout";
            logrichtextbox.AutoScroll = true;
            logrichtextbox.MaxLines = 10000;
            logrichtextbox.UseDefaultRowColoringRules = false;
            var getRule = new Func<string, string, RichTextBoxRowColoringRule>((condition, fontColor) =>
            {
                return new RichTextBoxRowColoringRule(condition, fontColor, "Transparent", FontStyle.Bold);
            });
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Trace", "DarkGray"));
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Debug", "Gray"));
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Info", "Black"));
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Warn", "Orange"));
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Error", "Red"));
            logrichtextbox.RowColoringRules.Add(getRule("level == LogLevel.Fatal", "Maroon"));

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRuleForAllLevels(logconsole);
            config.AddRuleForAllLevels(logfile);
            config.AddRuleForAllLevels(logrichtextbox);
            LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();
            //logger.Trace("This is a Trace message");
            //logger.Debug("This is a Debug message");
            //logger.Info("This is an Info message");
            //logger.Warn("This is a Warn message");
            //logger.Error("This is an Error message");
            //logger.Fatal("This is a Fatal error message");
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
        }

        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitStatus()
        {
            toolStripStatusLabelDateTime.Alignment = ToolStripItemAlignment.Right;
            toolStripStatusLabelRunningTime.Alignment = ToolStripItemAlignment.Right;
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = sender as System.Timers.Timer;
            timer.Stop();
            statusStrip.InvokeEx(c =>
            {
                toolStripStatusLabelDateTime.Text = "当前时间：" + DateTime.Now.ToString();
                toolStripStatusLabelRunningTime.Text = "系统运行时间：" + (DateTime.Now - My.StartTime).ToString(@"d\天hh\:mm\:ss");
            });
            timer.Start();
        }

        /// <summary>
        /// 初始化TabPage
        /// </summary>
        private void InitTabPage()
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                var page = new TabPage();
                var form = Activator.CreateInstance(TabPages[i]) as Form;
                if (form != null)
                {
                    page.Text = form.Text;
                    form.Width = page.Width;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.TopLevel = false;
                    form.Parent = page;
                    form.Dock = DockStyle.Fill;
                    form.Show();
                    tabControl.TabPages.Add(page);
                }
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

        private async void buttonRun_Click(object sender, EventArgs e)
        {
            buttonRun.Enabled = false;
            if (My.Work_PLC.IsRunning)
            {
                My.Work_PLC.Stop();
            }
            else
            {
                My.Work_PLC.Start();
            }
            await Task.Delay(3000);
            buttonRun.Enabled = true;
        }

        private async Task GetSignalStateAsync(CancellationToken token = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                var lastState = true;
                while (!token.IsCancellationRequested)
                {
                    #region buttonRun
                    try
                    {
                        var isRunning = My.Work_PLC.IsRunning;
                        if (isRunning != lastState)
                        {
                            lastState = isRunning;
                            Color color = isRunning ? Color.Green : Color.Red;
                            pictureBoxStatus.Image = new Bitmap(pictureBoxStatus.Width, pictureBoxStatus.Height);
                            var graph = Graphics.FromImage(pictureBoxStatus.Image);
                            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graph.FillEllipse(new SolidBrush(color), 10, 10, pictureBoxStatus.Width - 20, pictureBoxStatus.Height - 20);
                            graph.Save();
                            labelStatus.InvokeEx(c => { c.ForeColor = color; c.Text = isRunning ? "运行" : "停止"; });
                            buttonRun.InvokeEx(c => c.Text = isRunning ? "断开PLC" : "连接PLC");
                        }
                    }
                    catch (Exception)
                    {
                        //TODO
                        //throw;
                    }
                    #endregion
                    #region checkBoxProtect
                    if (signalProtect.Exist())
                    {
                        buttonProtect.InvokeEx(c => { c.Text = "连锁保护"; });
                    }
                    else
                    {
                        buttonProtect.InvokeEx(c => { c.Text = "连锁解除"; });
                    }
                    #endregion
                    #region checkBoxQuiet
                    if (signalQuiet.Exist())
                    {
                        buttonQuiet.InvokeEx(c => { c.Text = "取消警报静音"; });
                    }
                    else
                    {
                        buttonQuiet.InvokeEx(c => { c.Text = "警报静音"; });
                    }
                    #endregion
                    Thread.Sleep(1500);
                }
            }, token);
        }

        private Signal signalProtect = new Signal(339, 0, "连锁保护", Signal.EnumSignalType.手动控制, HNC.HncRegType.REG_TYPE_R, false);

        private async void buttonProtect_Click(object sender, EventArgs e)
        {
            buttonProtect.Enabled = false;
            if (signalProtect.Exist())
            {
                signalProtect.Clear();
            }
            else
            {
                signalProtect.Set();
            }
            await Task.Delay(3000);
            buttonProtect.Enabled = true;
        }

        private Signal signalQuiet = new Signal(339, 1, "警报静音", Signal.EnumSignalType.手动控制, HNC.HncRegType.REG_TYPE_R, false);

        private async void buttonQuiet_Click(object sender, EventArgs e)
        {
            buttonQuiet.Enabled = false;
            if (signalQuiet.Exist())
            {
                signalQuiet.Clear();
            }
            else
            {
                signalQuiet.Set();
            }
            await Task.Delay(3000);
            buttonQuiet.Enabled = true;
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            panelLog.Visible = !panelLog.Visible;
        }
    }
}
