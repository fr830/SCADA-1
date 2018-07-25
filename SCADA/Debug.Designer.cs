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
            this.buttonClear = new System.Windows.Forms.Button();
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
            this.comboBoxAssemble = new System.Windows.Forms.ComboBox();
            this.comboBoxGauge = new System.Windows.Forms.ComboBox();
            this.comboBoxClean = new System.Windows.Forms.ComboBox();
            this.comboBoxWorkpiece = new System.Windows.Forms.ComboBox();
            this.textBoxRFIDData = new System.Windows.Forms.TextBox();
            this.buttonRFIDRead = new System.Windows.Forms.Button();
            this.buttonRFIDWrite = new System.Windows.Forms.Button();
            this.comboBoxRFIDs = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonOut = new System.Windows.Forms.Button();
            this.buttonSpinOut = new System.Windows.Forms.Button();
            this.buttonIn = new System.Windows.Forms.Button();
            this.buttonSpinIn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonRS8 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonRKX = new System.Windows.Forms.Button();
            this.buttonCKX = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonWrite
            // 
            this.buttonWrite.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWrite.ForeColor = System.Drawing.Color.Transparent;
            this.buttonWrite.Location = new System.Drawing.Point(106, 103);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(75, 23);
            this.buttonWrite.TabIndex = 1;
            this.buttonWrite.Text = "Write";
            this.buttonWrite.UseVisualStyleBackColor = false;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // textBoxIndex
            // 
            this.textBoxIndex.Location = new System.Drawing.Point(98, 49);
            this.textBoxIndex.Name = "textBoxIndex";
            this.textBoxIndex.Size = new System.Drawing.Size(42, 21);
            this.textBoxIndex.TabIndex = 4;
            this.textBoxIndex.Text = "1";
            // 
            // textBoxBit
            // 
            this.textBoxBit.Location = new System.Drawing.Point(181, 49);
            this.textBoxBit.Name = "textBoxBit";
            this.textBoxBit.Size = new System.Drawing.Size(42, 21);
            this.textBoxBit.TabIndex = 5;
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
            this.groupBox1.Size = new System.Drawing.Size(290, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PLC";
            // 
            // buttonClear
            // 
            this.buttonClear.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.ForeColor = System.Drawing.Color.Transparent;
            this.buttonClear.Location = new System.Drawing.Point(187, 103);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Result:";
            // 
            // textBoxResult
            // 
            this.textBoxResult.BackColor = System.Drawing.Color.White;
            this.textBoxResult.Location = new System.Drawing.Point(98, 76);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(164, 21);
            this.textBoxResult.TabIndex = 6;
            // 
            // buttonRead
            // 
            this.buttonRead.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRead.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRead.Location = new System.Drawing.Point(25, 103);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(75, 23);
            this.buttonRead.TabIndex = 0;
            this.buttonRead.Text = "Read";
            this.buttonRead.UseVisualStyleBackColor = false;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // comboBoxRegType
            // 
            this.comboBoxRegType.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxRegType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxRegType.ForeColor = System.Drawing.Color.Transparent;
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
            this.comboBoxRegType.Location = new System.Drawing.Point(98, 23);
            this.comboBoxRegType.Name = "comboBoxRegType";
            this.comboBoxRegType.Size = new System.Drawing.Size(164, 20);
            this.comboBoxRegType.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "RegType:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Bit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 52);
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
            this.groupBox2.Controls.Add(this.comboBoxAssemble);
            this.groupBox2.Controls.Add(this.comboBoxGauge);
            this.groupBox2.Controls.Add(this.comboBoxClean);
            this.groupBox2.Controls.Add(this.comboBoxWorkpiece);
            this.groupBox2.Controls.Add(this.textBoxRFIDData);
            this.groupBox2.Controls.Add(this.buttonRFIDRead);
            this.groupBox2.Controls.Add(this.buttonRFIDWrite);
            this.groupBox2.Controls.Add(this.comboBoxRFIDs);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(308, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(606, 149);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RFID";
            // 
            // buttonRFIDInit
            // 
            this.buttonRFIDInit.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRFIDInit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRFIDInit.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRFIDInit.Location = new System.Drawing.Point(25, 73);
            this.buttonRFIDInit.Name = "buttonRFIDInit";
            this.buttonRFIDInit.Size = new System.Drawing.Size(163, 23);
            this.buttonRFIDInit.TabIndex = 3;
            this.buttonRFIDInit.Text = "Init";
            this.buttonRFIDInit.UseVisualStyleBackColor = false;
            this.buttonRFIDInit.Click += new System.EventHandler(this.buttonRFIDInit_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(319, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "Assemble:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(337, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "Gauge:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(337, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "Clean:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "Workpiece:";
            // 
            // comboBoxAssemble
            // 
            this.comboBoxAssemble.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxAssemble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAssemble.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxAssemble.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxAssemble.FormattingEnabled = true;
            this.comboBoxAssemble.Location = new System.Drawing.Point(384, 76);
            this.comboBoxAssemble.Name = "comboBoxAssemble";
            this.comboBoxAssemble.Size = new System.Drawing.Size(75, 20);
            this.comboBoxAssemble.TabIndex = 6;
            // 
            // comboBoxGauge
            // 
            this.comboBoxGauge.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxGauge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGauge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxGauge.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxGauge.FormattingEnabled = true;
            this.comboBoxGauge.Location = new System.Drawing.Point(384, 47);
            this.comboBoxGauge.Name = "comboBoxGauge";
            this.comboBoxGauge.Size = new System.Drawing.Size(75, 20);
            this.comboBoxGauge.TabIndex = 5;
            // 
            // comboBoxClean
            // 
            this.comboBoxClean.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxClean.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxClean.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxClean.FormattingEnabled = true;
            this.comboBoxClean.Location = new System.Drawing.Point(384, 18);
            this.comboBoxClean.Name = "comboBoxClean";
            this.comboBoxClean.Size = new System.Drawing.Size(75, 20);
            this.comboBoxClean.TabIndex = 4;
            // 
            // comboBoxWorkpiece
            // 
            this.comboBoxWorkpiece.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxWorkpiece.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkpiece.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWorkpiece.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxWorkpiece.FormattingEnabled = true;
            this.comboBoxWorkpiece.Location = new System.Drawing.Point(94, 46);
            this.comboBoxWorkpiece.Name = "comboBoxWorkpiece";
            this.comboBoxWorkpiece.Size = new System.Drawing.Size(94, 20);
            this.comboBoxWorkpiece.TabIndex = 2;
            // 
            // textBoxRFIDData
            // 
            this.textBoxRFIDData.BackColor = System.Drawing.Color.White;
            this.textBoxRFIDData.Location = new System.Drawing.Point(25, 107);
            this.textBoxRFIDData.Name = "textBoxRFIDData";
            this.textBoxRFIDData.ReadOnly = true;
            this.textBoxRFIDData.Size = new System.Drawing.Size(547, 21);
            this.textBoxRFIDData.TabIndex = 0;
            // 
            // buttonRFIDRead
            // 
            this.buttonRFIDRead.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRFIDRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRFIDRead.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRFIDRead.Location = new System.Drawing.Point(485, 74);
            this.buttonRFIDRead.Name = "buttonRFIDRead";
            this.buttonRFIDRead.Size = new System.Drawing.Size(87, 23);
            this.buttonRFIDRead.TabIndex = 8;
            this.buttonRFIDRead.Text = "Read";
            this.buttonRFIDRead.UseVisualStyleBackColor = false;
            this.buttonRFIDRead.Click += new System.EventHandler(this.buttonRFIDRead_Click);
            // 
            // buttonRFIDWrite
            // 
            this.buttonRFIDWrite.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRFIDWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRFIDWrite.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRFIDWrite.Location = new System.Drawing.Point(485, 16);
            this.buttonRFIDWrite.Name = "buttonRFIDWrite";
            this.buttonRFIDWrite.Size = new System.Drawing.Size(87, 52);
            this.buttonRFIDWrite.TabIndex = 7;
            this.buttonRFIDWrite.Text = "Write";
            this.buttonRFIDWrite.UseVisualStyleBackColor = false;
            this.buttonRFIDWrite.Click += new System.EventHandler(this.buttonRFIDWrite_Click);
            // 
            // comboBoxRFIDs
            // 
            this.comboBoxRFIDs.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxRFIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRFIDs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxRFIDs.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxRFIDs.FormattingEnabled = true;
            this.comboBoxRFIDs.Location = new System.Drawing.Point(94, 17);
            this.comboBoxRFIDs.Name = "comboBoxRFIDs";
            this.comboBoxRFIDs.Size = new System.Drawing.Size(94, 20);
            this.comboBoxRFIDs.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Site:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonOut);
            this.groupBox3.Controls.Add(this.buttonSpinOut);
            this.groupBox3.Controls.Add(this.buttonIn);
            this.groupBox3.Controls.Add(this.buttonSpinIn);
            this.groupBox3.Location = new System.Drawing.Point(12, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 171);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WMS";
            // 
            // buttonOut
            // 
            this.buttonOut.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOut.ForeColor = System.Drawing.Color.Transparent;
            this.buttonOut.Location = new System.Drawing.Point(106, 20);
            this.buttonOut.Name = "buttonOut";
            this.buttonOut.Size = new System.Drawing.Size(75, 23);
            this.buttonOut.TabIndex = 1;
            this.buttonOut.Text = "Out";
            this.buttonOut.UseVisualStyleBackColor = false;
            this.buttonOut.Click += new System.EventHandler(this.buttonOut_Click);
            // 
            // buttonSpinOut
            // 
            this.buttonSpinOut.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonSpinOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSpinOut.ForeColor = System.Drawing.Color.Transparent;
            this.buttonSpinOut.Location = new System.Drawing.Point(106, 49);
            this.buttonSpinOut.Name = "buttonSpinOut";
            this.buttonSpinOut.Size = new System.Drawing.Size(75, 23);
            this.buttonSpinOut.TabIndex = 3;
            this.buttonSpinOut.Text = "SpinOut";
            this.buttonSpinOut.UseVisualStyleBackColor = false;
            this.buttonSpinOut.Click += new System.EventHandler(this.buttonSpinOut_Click);
            // 
            // buttonIn
            // 
            this.buttonIn.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIn.ForeColor = System.Drawing.Color.Transparent;
            this.buttonIn.Location = new System.Drawing.Point(25, 49);
            this.buttonIn.Name = "buttonIn";
            this.buttonIn.Size = new System.Drawing.Size(75, 23);
            this.buttonIn.TabIndex = 2;
            this.buttonIn.Text = "In";
            this.buttonIn.UseVisualStyleBackColor = false;
            this.buttonIn.Click += new System.EventHandler(this.buttonIn_Click);
            // 
            // buttonSpinIn
            // 
            this.buttonSpinIn.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonSpinIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSpinIn.ForeColor = System.Drawing.Color.Transparent;
            this.buttonSpinIn.Location = new System.Drawing.Point(25, 20);
            this.buttonSpinIn.Name = "buttonSpinIn";
            this.buttonSpinIn.Size = new System.Drawing.Size(75, 23);
            this.buttonSpinIn.TabIndex = 0;
            this.buttonSpinIn.Text = "SpinIn";
            this.buttonSpinIn.UseVisualStyleBackColor = false;
            this.buttonSpinIn.Click += new System.EventHandler(this.buttonSpinIn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonRS8);
            this.groupBox4.Location = new System.Drawing.Point(308, 167);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(300, 171);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vision";
            // 
            // buttonRS8
            // 
            this.buttonRS8.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRS8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRS8.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRS8.Location = new System.Drawing.Point(25, 20);
            this.buttonRS8.Name = "buttonRS8";
            this.buttonRS8.Size = new System.Drawing.Size(75, 23);
            this.buttonRS8.TabIndex = 0;
            this.buttonRS8.Text = "RS8";
            this.buttonRS8.UseVisualStyleBackColor = false;
            this.buttonRS8.Click += new System.EventHandler(this.buttonRS8_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonRKX);
            this.groupBox5.Controls.Add(this.buttonCKX);
            this.groupBox5.Location = new System.Drawing.Point(614, 167);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 171);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Simulation";
            // 
            // buttonRKX
            // 
            this.buttonRKX.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRKX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRKX.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRKX.Location = new System.Drawing.Point(96, 20);
            this.buttonRKX.Name = "buttonRKX";
            this.buttonRKX.Size = new System.Drawing.Size(75, 23);
            this.buttonRKX.TabIndex = 1;
            this.buttonRKX.Text = "RKX";
            this.buttonRKX.UseVisualStyleBackColor = false;
            this.buttonRKX.Click += new System.EventHandler(this.buttonRKX_Click);
            // 
            // buttonCKX
            // 
            this.buttonCKX.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonCKX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCKX.ForeColor = System.Drawing.Color.Transparent;
            this.buttonCKX.Location = new System.Drawing.Point(15, 20);
            this.buttonCKX.Name = "buttonCKX";
            this.buttonCKX.Size = new System.Drawing.Size(75, 23);
            this.buttonCKX.TabIndex = 0;
            this.buttonCKX.Text = "CKX";
            this.buttonCKX.UseVisualStyleBackColor = false;
            this.buttonCKX.Click += new System.EventHandler(this.buttonCKX_Click);
            // 
            // Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(970, 397);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Debug";
            this.Text = "调试";
            this.Load += new System.EventHandler(this.Debug_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox comboBoxClean;
        private System.Windows.Forms.ComboBox comboBoxGauge;
        private System.Windows.Forms.ComboBox comboBoxAssemble;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonRFIDInit;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSpinIn;
        private System.Windows.Forms.Button buttonIn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonRS8;
        private System.Windows.Forms.Button buttonSpinOut;
        private System.Windows.Forms.Button buttonOut;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonCKX;
        private System.Windows.Forms.Button buttonRKX;
    }
}