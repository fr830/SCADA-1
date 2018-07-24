namespace SCADA
{
    partial class Home
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
            this.groupBoxTable = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelTable = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTable
            // 
            this.groupBoxTable.AutoSize = true;
            this.groupBoxTable.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxTable.Controls.Add(this.flowLayoutPanelTable);
            this.groupBoxTable.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTable.Name = "groupBoxTable";
            this.groupBoxTable.Size = new System.Drawing.Size(73, 40);
            this.groupBoxTable.TabIndex = 0;
            this.groupBoxTable.TabStop = false;
            this.groupBoxTable.Text = "台";
            // 
            // flowLayoutPanelTable
            // 
            this.flowLayoutPanelTable.AutoSize = true;
            this.flowLayoutPanelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTable.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelTable.Name = "flowLayoutPanelTable";
            this.flowLayoutPanelTable.Size = new System.Drawing.Size(67, 20);
            this.flowLayoutPanelTable.TabIndex = 0;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SCADA.Properties.Resources.微信图片_20180724122322;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(924, 489);
            this.Controls.Add(this.groupBoxTable);
            this.Name = "Home";
            this.Text = "首页";
            this.Load += new System.EventHandler(this.Home_Load);
            this.Shown += new System.EventHandler(this.Home_Shown);
            this.groupBoxTable.ResumeLayout(false);
            this.groupBoxTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTable;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTable;



    }
}