namespace YYApp.CommandControl
{
    partial class _43
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
            this.cbl_Item = new System.Windows.Forms.CheckedListBox();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.comboBox_stcd = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbl_Item
            // 
            this.cbl_Item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbl_Item.FormattingEnabled = true;
            this.cbl_Item.Location = new System.Drawing.Point(0, 45);
            this.cbl_Item.Name = "cbl_Item";
            this.cbl_Item.Size = new System.Drawing.Size(358, 336);
            this.cbl_Item.TabIndex = 16;
            this.cbl_Item.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbl_Item_ItemCheck);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.label13);
            this.panelEx1.Controls.Add(this.comboBox_stcd);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(358, 45);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 15;
            // 
            // comboBox_stcd
            // 
            this.comboBox_stcd.FormattingEnabled = true;
            this.comboBox_stcd.Location = new System.Drawing.Point(86, 6);
            this.comboBox_stcd.Name = "comboBox_stcd";
            this.comboBox_stcd.Size = new System.Drawing.Size(117, 20);
            this.comboBox_stcd.TabIndex = 63;
            this.comboBox_stcd.SelectedIndexChanged += new System.EventHandler(this.comboBox_stcd_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 62;
            this.label1.Text = "遥测站地址：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(3, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 12);
            this.label13.TabIndex = 64;
            this.label13.Text = "最多能选择5个选择项";
            // 
            // _43
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbl_Item);
            this.Controls.Add(this.panelEx1);
            this.Name = "_43";
            this.Size = new System.Drawing.Size(358, 381);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cbl_Item;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.ComboBox comboBox_stcd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
    }
}
