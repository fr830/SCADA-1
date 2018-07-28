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
    public partial class RFIDPage : Form
    {
        public RFIDPage()
        {
            InitializeComponent();
        }

        private void RFID_Load(object sender, EventArgs e)
        {
            foreach (var item in My.RFIDs.Values)
            {
                flowLayoutPanel.Controls.Add(new UserControl_RFID(item));
            }
        }

        private void RFIDPage_Shown(object sender, EventArgs e)
        {

        }
    }
}
