using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RFID;

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

            comboBoxRFIDs.DataSource = new BindingSource(My.CoreService.RFIDs, "");
            comboBoxRFIDs.DisplayMember = "Key";
            comboBoxRFIDs.ValueMember = "Value";

            var dictNo = Enum.GetValues(typeof(EnumNo))
               .Cast<EnumNo>()
               .ToDictionary(t => t.ToString(), t => t);
            comboBoxNo.DataSource = new BindingSource(dictNo, "");
            comboBoxNo.DisplayMember = "Key";
            comboBoxNo.ValueMember = "Value";

            var dictWorkpiece = Enum.GetValues(typeof(EnumWorkpiece))
                .Cast<EnumWorkpiece>()
                .ToDictionary(t => t.ToString(), t => t);
            comboBoxWorkpiece.DataSource = new BindingSource(dictWorkpiece, "");
            comboBoxWorkpiece.DisplayMember = "Key";
            comboBoxWorkpiece.ValueMember = "Value";

            var dictClean = Enum.GetValues(typeof(EnumClean))
               .Cast<EnumClean>()
               .ToDictionary(t => t.ToString(), t => t);
            comboBoxClean.DataSource = new BindingSource(dictClean, "");
            comboBoxClean.DisplayMember = "Key";
            comboBoxClean.ValueMember = "Value";

            var dictGauge = Enum.GetValues(typeof(EnumGauge))
               .Cast<EnumGauge>()
               .ToDictionary(t => t.ToString(), t => t);
            comboBoxGauge.DataSource = new BindingSource(dictGauge, "");
            comboBoxGauge.DisplayMember = "Key";
            comboBoxGauge.ValueMember = "Value";

            var dictGaugeResult = Enum.GetValues(typeof(EnumGaugeResult))
               .Cast<EnumGaugeResult>()
               .ToDictionary(t => t.ToString(), t => t);
            comboBoxGaugeResult.DataSource = new BindingSource(dictGaugeResult, "");
            comboBoxGaugeResult.DisplayMember = "Key";
            comboBoxGaugeResult.ValueMember = "Value";
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

        private IList<KeyValuePair<byte, EnumProcessResult>> process = new List<KeyValuePair<byte, EnumProcessResult>>();

        private void buttonRFIDRead_Click(object sender, EventArgs e)
        {
            var item = comboBoxRFIDs.SelectedValue as RFIDItem;
            if (item == null) return;
            var data = item.ReadBytes();
            if (data == null) return;
            var str = RFIDItem.BytesToHexString(data);
            for (int i = 2; i < str.Length; i += 3)
            {
                str = str.Insert(i, i == 17 ? "+" : "_");
            }
            var rData = RFIDData.Deserialize(data);
            textBoxRFIDData.Text = str;
            comboBoxNo.SelectedValue = rData.No;
            comboBoxWorkpiece.SelectedValue = rData.Workpiece;
            comboBoxClean.SelectedValue = rData.Clean;
            comboBoxGauge.SelectedValue = rData.Gauge;
            comboBoxGaugeResult.SelectedValue = rData.GaugeResult;
        }

        private void buttonRFIDWrite_Click(object sender, EventArgs e)
        {
            var item = comboBoxRFIDs.SelectedValue as RFIDItem;
            if (item == null) return;
            var data = new RFIDData((EnumNo)comboBoxNo.SelectedValue, (EnumWorkpiece)comboBoxWorkpiece.SelectedValue, (EnumClean)comboBoxClean.SelectedValue, (EnumGauge)comboBoxGauge.SelectedValue, (EnumGaugeResult)comboBoxGaugeResult.SelectedValue, null);
            item.Write(data);
            buttonRFIDRead.PerformClick();
        }

        private void buttonRFIDInit_Click(object sender, EventArgs e)
        {
            var item = comboBoxRFIDs.SelectedValue as RFIDItem;
            if (item == null) return;
            item.Init((EnumNo)comboBoxNo.SelectedValue, (EnumWorkpiece)comboBoxWorkpiece.SelectedValue);
            buttonRFIDRead.PerformClick();
        }
    }
}
