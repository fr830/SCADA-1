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
    public partial class RFID : Form
    {
        public RFID()
        {
            InitializeComponent();
        }

        private async void RFID_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                foreach (var item in My.CoreService.RFIDDict)
                {
                    var rfid = new UserControl_RFID(item.Key, item.Value);
                    flowLayoutPanel.InvokeEx(c => c.Controls.Add(rfid));
                }
            });
        }
    }
}
