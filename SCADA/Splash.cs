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
            My.LoadCompleted += LoadCompleted;
        }

        void LoadCompleted(object sender, MyInitializeEventArgs e)
        {
            this.InvokeEx(c =>
            {
                richTextBox.AppendText(e.ToString());
                richTextBox.ScrollToCaret();
                if (e.Value < progressBar.Maximum)
                {
                    progressBar.Value = e.Value;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                }
            });
        }

    }
}
