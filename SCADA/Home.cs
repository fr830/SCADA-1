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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            var table1 = new UserControl_Monitor(new Signal(397, 0, "台1就绪", Color.Green), new Signal(397, 1, "台1故障", Color.Red));
            var table2 = new UserControl_Monitor(new Signal(397, 2, "台2就绪", Color.Green), new Signal(397, 3, "台2故障", Color.Red));
            var table3 = new UserControl_Monitor(new Signal(397, 4, "台3就绪", Color.Green), new Signal(397, 5, "台3故障", Color.Red));
            var table4 = new UserControl_Monitor(new Signal(397, 6, "台4就绪", Color.Green), new Signal(397, 7, "台4故障", Color.Red));
            flowLayoutPanelTable.Controls.Add(table1);
            flowLayoutPanelTable.Controls.Add(table2);
            flowLayoutPanelTable.Controls.Add(table3);
            flowLayoutPanelTable.Controls.Add(table4);
        }

        private void Home_Shown(object sender, EventArgs e)
        {
            ;
        }
    }
}
