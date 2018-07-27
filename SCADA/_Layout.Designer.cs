namespace SCADA
{
    partial class _Layout
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_Layout));
            this.panelInfo = new System.Windows.Forms.Panel();
            this.richTextBoxInfo = new System.Windows.Forms.RichTextBox();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.panelButton = new System.Windows.Forms.Panel();
            this.checkBoxQuiet = new System.Windows.Forms.CheckBox();
            this.checkBoxProtect = new System.Windows.Forms.CheckBox();
            this.buttonService = new System.Windows.Forms.Button();
            this.buttonMES = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelRunningTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelC = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.panelInfo.SuspendLayout();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelButton.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelInfo.Controls.Add(this.richTextBoxInfo);
            this.panelInfo.Controls.Add(this.panelStatus);
            this.panelInfo.Controls.Add(this.pictureBoxLogo);
            this.panelInfo.Controls.Add(this.panelButton);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInfo.Location = new System.Drawing.Point(0, 24);
            this.panelInfo.MinimumSize = new System.Drawing.Size(200, 100);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(200, 704);
            this.panelInfo.TabIndex = 1;
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.BackColor = System.Drawing.Color.White;
            this.richTextBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInfo.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxInfo.ForeColor = System.Drawing.Color.Black;
            this.richTextBoxInfo.Location = new System.Drawing.Point(0, 100);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.ReadOnly = true;
            this.richTextBoxInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBoxInfo.Size = new System.Drawing.Size(200, 150);
            this.richTextBoxInfo.TabIndex = 0;
            this.richTextBoxInfo.TabStop = false;
            this.richTextBoxInfo.Text = "";
            this.richTextBoxInfo.Enter += new System.EventHandler(this.richTextBoxInfo_Enter);
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.labelStatus);
            this.panelStatus.Controls.Add(this.pictureBoxStatus);
            this.panelStatus.Controls.Add(this.buttonRun);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 250);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(200, 134);
            this.panelStatus.TabIndex = 3;
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStatus.Location = new System.Drawing.Point(60, 6);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(3);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(134, 42);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(54, 54);
            this.pictureBoxStatus.TabIndex = 3;
            this.pictureBoxStatus.TabStop = false;
            // 
            // buttonRun
            // 
            this.buttonRun.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonRun.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRun.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRun.ForeColor = System.Drawing.Color.Transparent;
            this.buttonRun.Location = new System.Drawing.Point(0, 54);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(200, 80);
            this.buttonRun.TabIndex = 5;
            this.buttonRun.Text = "连接PLC";
            this.buttonRun.UseVisualStyleBackColor = false;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(200, 100);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 1;
            this.pictureBoxLogo.TabStop = false;
            // 
            // panelButton
            // 
            this.panelButton.AutoSize = true;
            this.panelButton.BackColor = System.Drawing.Color.Transparent;
            this.panelButton.Controls.Add(this.checkBoxQuiet);
            this.panelButton.Controls.Add(this.checkBoxProtect);
            this.panelButton.Controls.Add(this.buttonService);
            this.panelButton.Controls.Add(this.buttonMES);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 384);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(200, 320);
            this.panelButton.TabIndex = 2;
            // 
            // checkBoxQuiet
            // 
            this.checkBoxQuiet.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxQuiet.BackColor = System.Drawing.Color.DodgerBlue;
            this.checkBoxQuiet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxQuiet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxQuiet.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxQuiet.ForeColor = System.Drawing.Color.Transparent;
            this.checkBoxQuiet.Location = new System.Drawing.Point(0, 0);
            this.checkBoxQuiet.Name = "checkBoxQuiet";
            this.checkBoxQuiet.Size = new System.Drawing.Size(200, 80);
            this.checkBoxQuiet.TabIndex = 9;
            this.checkBoxQuiet.Text = "警报静音";
            this.checkBoxQuiet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxQuiet.UseVisualStyleBackColor = false;
            this.checkBoxQuiet.CheckedChanged += new System.EventHandler(this.checkBoxQuiet_CheckedChanged);
            // 
            // checkBoxProtect
            // 
            this.checkBoxProtect.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxProtect.BackColor = System.Drawing.Color.DodgerBlue;
            this.checkBoxProtect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxProtect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxProtect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxProtect.ForeColor = System.Drawing.Color.Transparent;
            this.checkBoxProtect.Location = new System.Drawing.Point(0, 80);
            this.checkBoxProtect.Name = "checkBoxProtect";
            this.checkBoxProtect.Size = new System.Drawing.Size(200, 80);
            this.checkBoxProtect.TabIndex = 8;
            this.checkBoxProtect.Text = "连锁解除";
            this.checkBoxProtect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxProtect.UseVisualStyleBackColor = false;
            this.checkBoxProtect.CheckedChanged += new System.EventHandler(this.checkBoxProtect_CheckedChanged);
            // 
            // buttonService
            // 
            this.buttonService.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonService.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonService.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonService.ForeColor = System.Drawing.Color.Transparent;
            this.buttonService.Location = new System.Drawing.Point(0, 160);
            this.buttonService.Name = "buttonService";
            this.buttonService.Size = new System.Drawing.Size(200, 80);
            this.buttonService.TabIndex = 1;
            this.buttonService.Text = "WMS服务测试";
            this.buttonService.UseVisualStyleBackColor = false;
            this.buttonService.Click += new System.EventHandler(this.buttonService_Click);
            // 
            // buttonMES
            // 
            this.buttonMES.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonMES.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonMES.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMES.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonMES.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMES.Location = new System.Drawing.Point(0, 240);
            this.buttonMES.Name = "buttonMES";
            this.buttonMES.Size = new System.Drawing.Size(200, 80);
            this.buttonMES.TabIndex = 0;
            this.buttonMES.Text = "访问MES系统";
            this.buttonMES.UseVisualStyleBackColor = false;
            this.buttonMES.Click += new System.EventHandler(this.buttonMES_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(887, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "菜单栏";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDateTime,
            this.toolStripStatusLabelRunningTime,
            this.toolStripStatusLabelC});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 728);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(887, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "状态栏";
            // 
            // toolStripStatusLabelDateTime
            // 
            this.toolStripStatusLabelDateTime.Name = "toolStripStatusLabelDateTime";
            this.toolStripStatusLabelDateTime.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabelDateTime.Text = "当前时间";
            // 
            // toolStripStatusLabelRunningTime
            // 
            this.toolStripStatusLabelRunningTime.Name = "toolStripStatusLabelRunningTime";
            this.toolStripStatusLabelRunningTime.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabelRunningTime.Text = "累计时间";
            // 
            // toolStripStatusLabelC
            // 
            this.toolStripStatusLabelC.Name = "toolStripStatusLabelC";
            this.toolStripStatusLabelC.Size = new System.Drawing.Size(199, 17);
            this.toolStripStatusLabelC.Text = "© 2018 HNC. All rights reserved.";
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl.Location = new System.Drawing.Point(200, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(687, 704);
            this.tabControl.TabIndex = 4;
            // 
            // _Layout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(887, 750);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "_Layout";
            this.Text = "华中数控总控系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._Layout_FormClosing);
            this.Load += new System.EventHandler(this._Layout_Load);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelButton.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDateTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRunningTime;
        private System.Windows.Forms.RichTextBox richTextBoxInfo;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelC;
        private System.Windows.Forms.Button buttonMES;
        private System.Windows.Forms.Button buttonService;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.CheckBox checkBoxQuiet;
        private System.Windows.Forms.CheckBox checkBoxProtect;
    }
}

