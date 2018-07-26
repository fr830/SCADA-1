using System;
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
    public partial class UserControl_SignalTB : UserControl
    {
        private UserControl_SignalTB()
        {
            InitializeComponent();
        }

        public UserControl_SignalTB(string text, params Signal[] signals)
            : this()
        {
            ExplainText = text;
            Signals = signals.ToList();
        }

        public UserControl_SignalTB(string text, IList<Signal> signals)
            : this()
        {
            ExplainText = text;
            Signals = new List<Signal>(signals);
        }

        public string ExplainText
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public IList<Signal> Signals { get; private set; }

        private System.Timers.Timer timer = new System.Timers.Timer(500);

        private void UserControl_SignalTB_Load(object sender, EventArgs e)
        {
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            bool flag = false;
            foreach (var item in Signals)
            {
                if (item.IsExpected)
                {
                    flag = true;
                    SetInfo(item.Text, item.Color);
                }
            }
            if (!flag)
            {
                SetInfo(string.Empty, Color.Black);
            }
            timer.Start();
        }

        private void SetInfo(string text, Color color)
        {
            label2.InvokeEx(c =>
            {
                c.ForeColor = color;
                c.Text = text;
            });
        }
    }
}
