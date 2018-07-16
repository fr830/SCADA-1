namespace SCADA
{
    partial class Debug
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonWrite = new System.Windows.Forms.Button();
            this.textBoxIndex = new System.Windows.Forms.TextBox();
            this.textBoxBit = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonRead = new System.Windows.Forms.Button();
            this.comboBoxRegType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRFIDInit = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxGaugeResult = new System.Windows.Forms.ComboBox();
            this.comboBoxGauge = new System.Windows.Forms.ComboBox();
            this.comboBoxClean = new System.Windows.Forms.ComboBox();
            this.comboBoxNo = new System.Windows.Forms.ComboBox();
            this.comboBoxWorkpiece = new System.Windows.Forms.ComboBox();
            this.textBoxRFIDData = new System.Windows.Forms.TextBox();
            this.buttonRFIDRead = new System.Windows.Forms.Button();
            this.buttonRFIDWrite = new System.Windows.Forms.Button();
            this.comboBoxRFIDs = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonWrite
            // 
            this.buttonWrite.Location = new System.Drawing.Point(87, 103);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(75, 23);
            this.buttonWrite.TabIndex = 0;
            this.buttonWrite.Text = "Write";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // textBoxIndex
            // 
            this.textBoxIndex.Location = new System.Drawing.Point(79, 49);
            this.textBoxIndex.Name = "textBoxIndex";
            this.textBoxIndex.Size = new System.Drawing.Size(42, 21);
            this.textBoxIndex.TabIndex = 1;
            this.textBoxIndex.Text = "1";
            // 
            // textBoxBit
            // 
            this.textBoxBit.Location = new System.Drawing.Point(162, 49);
            this.textBoxBit.Name = "textBoxBit";
            this.textBoxBit.Size = new System.Drawing.Size(42, 21);
            this.textBoxBit.TabIndex = 2;
            this.textBoxBit.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxResult);
            this.groupBox1.Controls.Add(this.buttonRead);
            this.groupBox1.Controls.Add(this.comboBoxRegType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxIndex);
            this.groupBox1.Controls.Add(this.buttonWrite);
            this.groupBox1.Controls.Add(this.textBoxBit);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 149);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PLC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Result:";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(79, 76);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(164, 21);
            this.textBoxResult.TabIndex = 8;
            // 
            // buttonRead
            // 
            this.buttonRead.Location = new System.Drawing.Point(6, 103);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(75, 23);
            this.buttonRead.TabIndex = 7;
            this.buttonRead.Text = "Read";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // comboBoxRegType
            // 
            this.comboBoxRegType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegType.FormattingEnabled = true;
            this.comboBoxRegType.Items.AddRange(new object[] {
            "REG_TYPE_X",
            "REG_TYPE_Y",
            "REG_TYPE_F",
            "REG_TYPE_G",
            "REG_TYPE_R",
            "REG_TYPE_W",
            "REG_TYPE_D",
            "REG_TYPE_B",
            "REG_TYPE_P"});
            this.comboBoxRegType.Location = new System.Drawing.Point(79, 23);
            this.comboBoxRegType.Name = "comboBoxRegType";
            this.comboBoxRegType.Size = new System.Drawing.Size(164, 20);
            this.comboBoxRegType.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "RegType:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(127, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Bit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Index:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRFIDInit);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBoxGaugeResult);
            this.groupBox2.Controls.Add(this.comboBoxGauge);
            this.groupBox2.Controls.Add(this.comboBoxClean);
            this.groupBox2.Controls.Add(this.comboBoxNo);
            this.groupBox2.Controls.Add(this.comboBoxWorkpiece);
            this.groupBox2.Controls.Add(this.textBoxRFIDData);
            this.groupBox2.Controls.Add(this.buttonRFIDRead);
            this.groupBox2.Controls.Add(this.buttonRFIDWrite);
            this.groupBox2.Controls.Add(this.comboBoxRFIDs);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(283, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(587, 149);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RFID";
            // 
            // buttonRFIDInit
            // 
            this.buttonRFIDInit.Location = new System.Drawing.Point(25, 78);
            this.buttonRFIDInit.Name = "buttonRFIDInit";
            this.buttonRFIDInit.Size = new System.Drawing.Size(156, 23);
            this.buttonRFIDInit.TabIndex = 27;
            this.buttonRFIDInit.Text = "Init";
            this.buttonRFIDInit.UseVisualStyleBackColor = true;
            this.buttonRFIDInit.Click += new System.EventHandler(this.buttonRFIDInit_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(414, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "Result:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(247, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "Gauge:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(420, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "Clean:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(438, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "Wp:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(265, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "No:";
            // 
            // comboBoxGaugeResult
            // 
            this.comboBoxGaugeResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGaugeResult.FormattingEnabled = true;
            this.comboBoxGaugeResult.Location = new System.Drawing.Point(467, 74);
            this.comboBoxGaugeResult.Name = "comboBoxGaugeResult";
            this.comboBoxGaugeResult.Size = new System.Drawing.Size(94, 20);
            this.comboBoxGaugeResult.TabIndex = 17;
            // 
            // comboBoxGauge
            // 
            this.comboBoxGauge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGauge.FormattingEnabled = true;
            this.comboBoxGauge.Location = new System.Drawing.Point(294, 74);
            this.comboBoxGauge.Name = "comboBoxGauge";
            this.comboBoxGauge.Size = new System.Drawing.Size(94, 20);
            this.comboBoxGauge.TabIndex = 16;
            // 
            // comboBoxClean
            // 
            this.comboBoxClean.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClean.FormattingEnabled = true;
            this.comboBoxClean.Location = new System.Drawing.Point(467, 48);
            this.comboBoxClean.Name = "comboBoxClean";
            this.comboBoxClean.Size = new System.Drawing.Size(94, 20);
            this.comboBoxClean.TabIndex = 15;
            // 
            // comboBoxNo
            // 
            this.comboBoxNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNo.FormattingEnabled = true;
            this.comboBoxNo.Location = new System.Drawing.Point(294, 23);
            this.comboBoxNo.Name = "comboBoxNo";
            this.comboBoxNo.Size = new System.Drawing.Size(94, 20);
            this.comboBoxNo.TabIndex = 14;
            // 
            // comboBoxWorkpiece
            // 
            this.comboBoxWorkpiece.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkpiece.FormattingEnabled = true;
            this.comboBoxWorkpiece.Location = new System.Drawing.Point(467, 23);
            this.comboBoxWorkpiece.Name = "comboBoxWorkpiece";
            this.comboBoxWorkpiece.Size = new System.Drawing.Size(94, 20);
            this.comboBoxWorkpiece.TabIndex = 13;
            // 
            // textBoxRFIDData
            // 
            this.textBoxRFIDData.Location = new System.Drawing.Point(25, 107);
            this.textBoxRFIDData.Name = "textBoxRFIDData";
            this.textBoxRFIDData.ReadOnly = true;
            this.textBoxRFIDData.Size = new System.Drawing.Size(536, 21);
            this.textBoxRFIDData.TabIndex = 12;
            // 
            // buttonRFIDRead
            // 
            this.buttonRFIDRead.Location = new System.Drawing.Point(25, 49);
            this.buttonRFIDRead.Name = "buttonRFIDRead";
            this.buttonRFIDRead.Size = new System.Drawing.Size(75, 23);
            this.buttonRFIDRead.TabIndex = 11;
            this.buttonRFIDRead.Text = "Read";
            this.buttonRFIDRead.UseVisualStyleBackColor = true;
            this.buttonRFIDRead.Click += new System.EventHandler(this.buttonRFIDRead_Click);
            // 
            // buttonRFIDWrite
            // 
            this.buttonRFIDWrite.Location = new System.Drawing.Point(106, 49);
            this.buttonRFIDWrite.Name = "buttonRFIDWrite";
            this.buttonRFIDWrite.Size = new System.Drawing.Size(75, 23);
            this.buttonRFIDWrite.TabIndex = 10;
            this.buttonRFIDWrite.Text = "Write";
            this.buttonRFIDWrite.UseVisualStyleBackColor = true;
            this.buttonRFIDWrite.Click += new System.EventHandler(this.buttonRFIDWrite_Click);
            // 
            // comboBoxRFIDs
            // 
            this.comboBoxRFIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRFIDs.FormattingEnabled = true;
            this.comboBoxRFIDs.Location = new System.Drawing.Point(106, 23);
            this.comboBoxRFIDs.Name = "comboBoxRFIDs";
            this.comboBoxRFIDs.Size = new System.Drawing.Size(75, 20);
            this.comboBoxRFIDs.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "RFIDNo:";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(168, 103);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 397);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Debug";
            this.Text = "Debug";
            this.Load += new System.EventHandler(this.Debug_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.TextBox textBoxIndex;
        private System.Windows.Forms.TextBox textBoxBit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxRegType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonRFIDRead;
        private System.Windows.Forms.Button buttonRFIDWrite;
        private System.Windows.Forms.ComboBox comboBoxRFIDs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxRFIDData;
        private System.Windows.Forms.ComboBox comboBoxWorkpiece;
        private System.Windows.Forms.ComboBox comboBoxNo;
        private System.Windows.Forms.ComboBox comboBoxClean;
        private System.Windows.Forms.ComboBox comboBoxGauge;
        private System.Windows.Forms.ComboBox comboBoxGaugeResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonRFIDInit;
        private System.Windows.Forms.Button buttonClear;
    }
}