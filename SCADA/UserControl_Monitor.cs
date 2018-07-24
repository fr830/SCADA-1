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
    public partial class UserControl_Monitor : UserControl
    {
        private UserControl_Monitor()
        {
            InitializeComponent();
        }

        public UserControl_Monitor(params Signal[] signals)
            : this()
        {
            Signals = signals.ToList();
        }

        public UserControl_Monitor(IList<Signal> signals)
            : this()
        {
            Signals = new List<Signal>(signals);
        }

        public IList<Signal> Signals { get; private set; }

        private System.Timers.Timer timer = new System.Timers.Timer(500);

        private void UserControl_Monitor_Load(object sender, EventArgs e)
        {
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            foreach (var item in Signals)
            {
                if (item.IsExpected)
                {
                    DrawInfo(item.Text, item.Color);
                }
            }
            timer.Start();
        }

        private void DrawInfo(string text, Color color)
        {
            pictureBox.InvokeEx(c =>
            {
                pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
                var graph = Graphics.FromImage(pictureBox.Image);
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.FillEllipse(new SolidBrush(color), 10, 10, pictureBox.Width - 20, pictureBox.Height - 20);
                graph.Save();
            });
            label.InvokeEx(c =>
            {
                label.ForeColor = color;
                label.Text = text;
            });
        }

    }

}
