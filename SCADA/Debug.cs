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
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void Debug_Load(object sender, EventArgs e)
        {
            comboBoxRegType.SelectedIndex = 7;
        }

        HNC.MachineTool PLC = My.CoreService.PLC;

        HNC.HncRegType RegType
        {
            get
            {
                HNC.HncRegType result = HNC.HncRegType.REG_TYPE_B;
                Enum.TryParse<HNC.HncRegType>(comboBoxRegType.Text, out result);
                return result;
            }
        }

        int Index
        {
            get
            {
                int index = 0;
                int.TryParse(textBoxIndex.Text, out index);
                return index;
            }
        }

        int Bit
        {
            get
            {
                int bit = 0;
                int.TryParse(textBoxBit.Text, out bit);
                return bit;
            }
        }

        async void SetPLCResultAsync(object result)
        {
            if (result != null)
            {
                textBoxResult.Text = result.ToString();
            }
            await Task.Delay(1000);
            textBoxResult.Text = string.Empty;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            SetPLCResultAsync(PLC.BitSet(Index, Bit, RegType));
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            SetPLCResultAsync(PLC.BitExist(Index, Bit, RegType));
        }
    }
}
