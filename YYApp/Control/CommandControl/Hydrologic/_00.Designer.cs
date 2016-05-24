namespace YYApp.CommandControl
{
    partial class _00
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.BTselectAll = new DevComponents.DotNetBar.ButtonX();
            this.BTReadDB = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.BTWriteAndReset = new DevComponents.DotNetBar.ButtonX();
            this.cbl_Item = new System.Windows.Forms.CheckedListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.label13);
            this.panelEx1.Controls.Add(this.BTReadDB);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(246, 45);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 15;
            // 
            // BTselectAll
            // 
            this.BTselectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTselectAll.Location = new System.Drawing.Point(5, 11);
            this.BTselectAll.Name = "BTselectAll";
            this.BTselectAll.Size = new System.Drawing.Size(49, 23);
            this.BTselectAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.BTselectAll.TabIndex = 62;
            this.BTselectAll.Text = "全 选";
            this.BTselectAll.Click += new System.EventHandler(this.BTselectAll_Click);
            // 
            // BTReadDB
            // 
            this.BTReadDB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTReadDB.Location = new System.Drawing.Point(7, 12);
            this.BTReadDB.Name = "BTReadDB";
            this.BTReadDB.Size = new System.Drawing.Size(74, 23);
            this.BTReadDB.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.BTReadDB.TabIndex = 63;
            this.BTReadDB.Text = "读取数据库";
            this.BTReadDB.Click += new System.EventHandler(this.BTReadDB_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.BTWriteAndReset);
            this.panelEx2.Controls.Add(this.BTselectAll);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx2.Location = new System.Drawing.Point(0, 251);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(246, 45);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 17;
            // 
            // BTWriteAndReset
            // 
            this.BTWriteAndReset.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTWriteAndReset.Location = new System.Drawing.Point(66, 11);
            this.BTWriteAndReset.Name = "BTWriteAndReset";
            this.BTWriteAndReset.Size = new System.Drawing.Size(158, 23);
            this.BTWriteAndReset.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.BTWriteAndReset.TabIndex = 63;
            this.BTWriteAndReset.Text = "写入数据库并更新服务信息";
            this.BTWriteAndReset.Click += new System.EventHandler(this.BTWriteAndReset_Click);
            // 
            // cbl_Item
            // 
            this.cbl_Item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbl_Item.FormattingEnabled = true;
            this.cbl_Item.Location = new System.Drawing.Point(0, 45);
            this.cbl_Item.Name = "cbl_Item";
            this.cbl_Item.Size = new System.Drawing.Size(246, 206);
            this.cbl_Item.TabIndex = 18;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(83, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(149, 12);
            this.label13.TabIndex = 64;
            this.label13.Text = "信道选择与召测按钮无作用";
            // 
            // _00
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbl_Item);
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.panelEx1);
            this.Name = "_00";
            this.Size = new System.Drawing.Size(246, 296);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.panelEx2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX BTReadDB;
        private DevComponents.DotNetBar.ButtonX BTselectAll;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX BTWriteAndReset;
        private System.Windows.Forms.CheckedListBox cbl_Item;
        private System.Windows.Forms.Label label13;
    }
}
