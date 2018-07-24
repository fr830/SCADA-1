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
            IList<PLCContent>[] ls = { PLCContent.listS1, PLCContent.listS2, PLCContent.listS3, PLCContent.listS4, PLCContent.listS5, PLCContent.listS6, PLCContent.listS7, PLCContent.listS8, PLCContent.listS9, PLCContent.listS10, };
            IList<PLCContent>[] lj = { PLCContent.listJ1, PLCContent.listJ2, PLCContent.listJ3, PLCContent.listJ4, PLCContent.listJ5, PLCContent.listJ6, PLCContent.listJ7, PLCContent.listJ8, PLCContent.listJ9, PLCContent.listJ10, };
            IList<PLCContent>[] lc = { PLCContent.listC1, PLCContent.listC2, PLCContent.listC3, PLCContent.listC4, PLCContent.listC5, PLCContent.listC6, PLCContent.listC7, PLCContent.listC8, PLCContent.listC9, PLCContent.listC10, };
            for (int i = 0; i < ls.Length; i++)
            {
                flowLayoutPanel.Controls.Add(new UserControl_Area((i + 1).ToString(), ls[i], lj[i], lc[i]));
            }
        }

    }
}
