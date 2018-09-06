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
using System.Collections;

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
            SetComboboxDataSource(comboBoxRFIDs, My.RFIDs);
            SetComboboxDataSource<EnumWorkpiece>(comboBoxWorkpiece);
            SetComboboxDataSource<EnumClean>(comboBoxClean);
            SetComboboxDataSource<EnumGauge>(comboBoxGauge);
            SetComboboxDataSource<EnumAssemble>(comboBoxAssemble);
        }

        private void SetComboboxDataSource<TEnum>(ComboBox cb) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var dict = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToDictionary(t => t.ToString(), t => t);
            SetComboboxDataSource(cb, dict);
        }

        private void SetComboboxDataSource(ComboBox cb, IDictionary dict)
        {
            cb.DataSource = new BindingSource(dict, "");
            cb.DisplayMember = "Key";
            cb.ValueMember = "Value";
        }

        private HNC.HncRegType RegType
        {
            get
            {
                HNC.HncRegType result = HNC.HncRegType.REG_TYPE_B;
                Enum.TryParse<HNC.HncRegType>(comboBoxRegType.Text, out result);
                return result;
            }
        }

        private int Index
        {
            get
            {
                int index = 0;
                int.TryParse(textBoxIndex.Text, out index);
                return index;
            }
        }

        private int Bit
        {
            get
            {
                int bit = 0;
                int.TryParse(textBoxBit.Text, out bit);
                return bit;
            }
        }

        private async Task SetPLCResultAsync(object result)
        {
            if (result != null)
            {
                textBoxResult.Text = result.ToString();
            }
            await Task.Delay(1000);
            textBoxResult.Text = string.Empty;
        }

        private async void buttonRead_Click(object sender, EventArgs e)
        {
            buttonRead.Enabled = false;
            await SetPLCResultAsync(My.PLC.Exist(Index, Bit, RegType));
            buttonRead.Enabled = true;
        }

        private async void buttonWrite_Click(object sender, EventArgs e)
        {
            buttonWrite.Enabled = false;
            await SetPLCResultAsync(My.PLC.Set(Index, Bit, RegType));
            buttonWrite.Enabled = true;
        }

        private async void buttonClear_Click(object sender, EventArgs e)
        {
            buttonClear.Enabled = false;
            await SetPLCResultAsync(My.PLC.Clear(Index, Bit, RegType));
            buttonClear.Enabled = true;
        }

        private RFIDData Data { get; set; }

        private void buttonRFIDRead_Click(object sender, EventArgs e)
        {
            buttonRFIDRead.Enabled = false;
            var item = comboBoxRFIDs.SelectedValue as RFIDReader;
            if (item != null)
            {
                var data = item.ReadBytes();
                if (data != null)
                {
                    var str = RFIDReader.BytesToHexString(data);
                    for (int i = 2; i < str.Length; i += 3)
                    {
                        str = str.Insert(i, i == 65 ? "+" : "_");
                    }
                    Data = RFIDData.Deserialize(data);
                    textBoxRFIDData.Text = str;
                    comboBoxWorkpiece.SelectedValue = Data.Workpiece;
                    comboBoxClean.SelectedValue = Data.Clean;
                    comboBoxGauge.SelectedValue = Data.Gauge;
                    comboBoxAssemble.SelectedValue = Data.Assemble;
                }
            }
            buttonRFIDRead.Enabled = true;
        }

        private void buttonRFIDWrite_Click(object sender, EventArgs e)
        {
            buttonRFIDWrite.Enabled = false;
            var item = comboBoxRFIDs.SelectedValue as RFIDReader;
            if (item != null)
            {
                Data.Clean = (EnumClean)comboBoxClean.SelectedValue;
                Data.Gauge = (EnumGauge)comboBoxGauge.SelectedValue;
                Data.Assemble = (EnumAssemble)comboBoxAssemble.SelectedValue;
                item.Write(Data);
                buttonRFIDRead.PerformClick();
            }
            buttonRFIDWrite.Enabled = true;
        }

        private void buttonFake_Click(object sender, EventArgs e)
        {
            buttonFake.Enabled = false;
            var item = comboBoxRFIDs.SelectedValue as RFIDReader;
            if (item != null)
            {
                var data = item.Read();
                if (data != null)
                {
                    data.SetFake(true);
                    item.Write(data);
                }
                buttonRFIDRead.PerformClick();
            }
            buttonFake.Enabled = true;
        }

        private void buttonRFIDInit_Click(object sender, EventArgs e)
        {
            buttonRFIDInit.Enabled = false;
            var item = comboBoxRFIDs.SelectedValue as RFIDReader;
            if (item != null)
            {
                item.Init(Guid.NewGuid(), (EnumWorkpiece)comboBoxWorkpiece.SelectedValue);
                buttonRFIDRead.PerformClick();
            }
            buttonRFIDInit.Enabled = true;
        }

        private void buttonSpinIn_Click(object sender, EventArgs e)
        {
            buttonSpinIn.Enabled = false;
            My.Work_WMS.SpinIn();
            buttonSpinIn.Enabled = true;
        }

        private void buttonSpinOut_Click(object sender, EventArgs e)
        {
            buttonSpinOut.Enabled = false;
            My.Work_WMS.SpinOut();
            buttonSpinOut.Enabled = true;
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            buttonIn.Enabled = false;
            My.Work_WMS.In(new WMSData("A", 1));
            buttonIn.Enabled = true;
        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            buttonOut.Enabled = false;
            My.Work_WMS.Out(new List<WMSData> { new WMSData("A", 1), new WMSData("B", 1), new WMSData("C", 1), new WMSData("D", 1), new WMSData("E", 0) });
            buttonOut.Enabled = true;
        }

        private void buttonRS8_Click(object sender, EventArgs e)
        {
            buttonRS8.Enabled = false;
            My.Work_Vision.Photograph(null, new PLCEventArgs(EnumPSite.S8_Down, 1));
            buttonRS8.Enabled = true;
        }

        private void buttonCKX_Click(object sender, EventArgs e)
        {
            buttonCKX.Enabled = false;
            var data = RFIDData.GetDefaut(Guid.NewGuid(), EnumWorkpiece.A);
            My.Work_Simulation.Send(new CKX(data, CKX.EnumActionType.出库线转移物料至出库检测位));
            My.Work_Simulation.Send(new CKX(data, CKX.EnumActionType.出库检测位转移物料至定位台1));
            buttonCKX.Enabled = true;
        }

        private void buttonRKX_Click(object sender, EventArgs e)
        {
            buttonRKX.Enabled = false;
            var data = RFIDData.GetDefaut(Guid.NewGuid(), EnumWorkpiece.A);
            My.Work_Simulation.Send(new RKX(data, RKX.EnumActionType.定位台2转移物料至入库检测位));
            My.Work_Simulation.Send(new RKX(data, RKX.EnumActionType.入库检测位转移物料至入库位));
            buttonRKX.Enabled = true;
        }

        private void buttonGSend_Click(object sender, EventArgs e)
        {
            buttonGSend.Enabled = false;
            My.PLC.HNC_NetFileSend(GCodeFile.Dict[EnumPSite.S1][GCodeFile.EnumFile.加工A料程序], "Otest");
            buttonGSend.Enabled = true;
        }

        private void buttonPrintQRCode_Click(object sender, EventArgs e)
        {
            buttonPrintQRCode.Enabled = false;
            //My.Work_QRCode.Print();
            My.PLC.Set(Work_PLC.SiteIndexDict[EnumPSite.S6_Alignment], 10);
            buttonPrintQRCode.Enabled = true;
        }
    }
}
