using System;
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
    public partial class DebugPLC : Form
    {
        public DebugPLC()
        {
            InitializeComponent();
        }

        private void DebugPLC_Load(object sender, EventArgs e)
        {
            IList<Signal>[] ls = { Signal.listS1, Signal.listS2, Signal.listS3, Signal.listS4, Signal.listS5, Signal.listS6, Signal.listS7, Signal.listS8, Signal.listS9, Signal.listS10, };
            IList<Signal>[] lj = { Signal.listJ1, Signal.listJ2, Signal.listJ3, Signal.listJ4, Signal.listJ5, Signal.listJ6, Signal.listJ7, Signal.listJ8, Signal.listJ9, Signal.listJ10, };
            IList<Signal>[] lc = { Signal.listC1, Signal.listC2, Signal.listC3, Signal.listC4, Signal.listC5, Signal.listC6, Signal.listC7, Signal.listC8, Signal.listC9, Signal.listC10, };
            for (int i = 0; i < ls.Length; i++)
            {
                flowLayoutPanel.Controls.Add(new UserControl_Area((i + 1).ToString(), ls[i], lj[i], lc[i]));
            }
        }

    }
}
