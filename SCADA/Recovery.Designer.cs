namespace SCADA
{
    partial class Recovery
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.labelRFID = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxRFIDs = new System.Windows.Forms.ComboBox();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "仓储";
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.Color.DodgerBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Transparent;
            this.button2.Location = new System.Drawing.Point(146, 67);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "出库许可";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.DodgerBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(65, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "请求入库";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.labelRFID);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(318, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 300);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "相机";
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.BackColor = System.Drawing.Color.DodgerBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.Transparent;
            this.button4.Location = new System.Drawing.Point(25, 67);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 24);
            this.button4.TabIndex = 5;
            this.button4.Text = "获取RFID信息";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // labelRFID
            // 
            this.labelRFID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRFID.Location = new System.Drawing.Point(120, 67);
            this.labelRFID.Name = "labelRFID";
            this.labelRFID.Size = new System.Drawing.Size(160, 23);
            this.labelRFID.TabIndex = 4;
            this.labelRFID.Text = "无";
            this.labelRFID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.Color.DodgerBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.Transparent;
            this.button3.Location = new System.Drawing.Point(25, 97);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "请求重新拍照";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(624, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(300, 300);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "三维仿真";
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.BackColor = System.Drawing.Color.DodgerBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.Transparent;
            this.button5.Location = new System.Drawing.Point(201, 67);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(58, 54);
            this.button5.TabIndex = 6;
            this.button5.Text = "刷新";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(35, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "待发送消息数量";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(35, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "服务状态";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button9);
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Location = new System.Drawing.Point(12, 318);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(300, 300);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "MES";
            // 
            // button9
            // 
            this.button9.AutoSize = true;
            this.button9.BackColor = System.Drawing.Color.DodgerBlue;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.ForeColor = System.Drawing.Color.Transparent;
            this.button9.Location = new System.Drawing.Point(160, 85);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(89, 24);
            this.button9.TabIndex = 2;
            this.button9.Text = "重置数据库";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.BackColor = System.Drawing.Color.DodgerBlue;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ForeColor = System.Drawing.Color.Transparent;
            this.button6.Location = new System.Drawing.Point(65, 86);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 24);
            this.button6.TabIndex = 1;
            this.button6.Text = "重新启动订单";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(318, 318);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 300);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PLC";
            // 
            // button7
            // 
            this.button7.AutoSize = true;
            this.button7.BackColor = System.Drawing.Color.DodgerBlue;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ForeColor = System.Drawing.Color.Transparent;
            this.button7.Location = new System.Drawing.Point(191, 86);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(89, 24);
            this.button7.TabIndex = 2;
            this.button7.Text = "刷新";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(25, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "服务状态";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboBoxRFIDs);
            this.groupBox6.Controls.Add(this.button8);
            this.groupBox6.Location = new System.Drawing.Point(624, 318);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(300, 300);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "RFID";
            // 
            // comboBoxRFIDs
            // 
            this.comboBoxRFIDs.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBoxRFIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRFIDs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxRFIDs.ForeColor = System.Drawing.Color.Transparent;
            this.comboBoxRFIDs.FormattingEnabled = true;
            this.comboBoxRFIDs.Location = new System.Drawing.Point(35, 88);
            this.comboBoxRFIDs.Name = "comboBoxRFIDs";
            this.comboBoxRFIDs.Size = new System.Drawing.Size(129, 20);
            this.comboBoxRFIDs.TabIndex = 4;
            this.comboBoxRFIDs.TabStop = false;
            // 
            // button8
            // 
            this.button8.AutoSize = true;
            this.button8.BackColor = System.Drawing.Color.DodgerBlue;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ForeColor = System.Drawing.Color.Transparent;
            this.button8.Location = new System.Drawing.Point(170, 85);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(89, 24);
            this.button8.TabIndex = 3;
            this.button8.Text = "请求读";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // Recovery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(949, 651);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Recovery";
            this.Text = "故障恢复";
            this.Load += new System.EventHandler(this.Recovery_Load);
            this.Shown += new System.EventHandler(this.Recovery_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label labelRFID;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox comboBoxRFIDs;
        private System.Windows.Forms.Button button9;
    }
}