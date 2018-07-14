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

        private void Splash_Load(object sender, EventArgs e)
        {
            My.PartCompleted += My_PartCompleted;
            My.AllCompleted += My_AllCompleted;
        }

        void My_PartCompleted(object sender, MyInitializeEventArgs e)
        {
            this.InvokeEx(c =>
            {
                richTextBox.AppendText(e.ToString());
                if (e.Value < progressBar.Maximum)
                {
                    progressBar.Value = e.Value;
                }
            });
        }

        void My_AllCompleted(object sender, MyInitializeEventArgs e)
        {
            this.InvokeEx(c =>
            {
                richTextBox.AppendText(e.ToString());
                progressBar.Value = e.Value;
            });
        }

    }
}
