namespace YYApp.CommandControl
{
    partial class _38
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
            this.label13 = new System.Windows.Forms.Label();
            this.cb_Minute = new System.Windows.Forms.ComboBox();
            this.rb_Minute = new System.Windows.Forms.RadioButton();
            this.cb_Hour = new System.Windows.Forms.ComboBox();
            this.rb_Hour = new System.Windows.Forms.RadioButton();
            this.cb_day = new System.Windows.Forms.ComboBox();
            this.rb_Day = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbl_Item = new System.Windows.Forms.CheckedListBox();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.label13);
            this.panelEx1.Controls.Add(this.cb_Minute);
            this.panelEx1.Controls.Add(this.rb_Minute);
            this.panelEx1.Controls.Add(this.cb_Hour);
            this.panelEx1.Controls.Add(this.rb_Hour);
            this.panelEx1.Controls.Add(this.cb_day);
            this.panelEx1.Controls.Add(this.rb_Day);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.dateTimePicker2);
            this.panelEx1.Controls.Add(this.dateTimePicker1);
            this.panelEx1.Controls.Add(this.label3);
            this.panelEx1.Controls.Add(this.label2);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(319, 125);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(120, 110);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 12);
            this.label13.TabIndex = 61;
            this.label13.Text = "仅能选择1个监测项";
            // 
            // cb_Minute
            // 
            this.cb_Minute.FormattingEnabled = true;
            this.cb_Minute.Location = new System.Drawing.Point(75, 94);
            this.cb_Minute.Name = "cb_Minute";
            this.cb_Minute.Size = new System.Drawing.Size(39, 20);
            this.cb_Minute.TabIndex = 29;
            // 
            // rb_Minute
            // 
            this.rb_Minute.AutoSize = true;
            this.rb_Minute.Location = new System.Drawing.Point(46, 97);
            this.rb_Minute.Name = "rb_Minute";
            this.rb_Minute.Size = new System.Drawing.Size(35, 16);
            this.rb_Minute.TabIndex = 28;
            this.rb_Minute.TabStop = true;
            this.rb_Minute.Text = "分";
            this.rb_Minute.UseVisualStyleBackColor = true;
            this.rb_Minute.CheckedChanged += new System.EventHandler(this.rb_Minute_CheckedChanged);
            // 
            // cb_Hour
            // 
            this.cb_Hour.FormattingEnabled = true;
            this.cb_Hour.Location = new System.Drawing.Point(171, 70);
            this.cb_Hour.Name = "cb_Hour";
            this.cb_Hour.Size = new System.Drawing.Size(39, 20);
            this.cb_Hour.TabIndex = 27;
            // 
            // rb_Hour
            // 
            this.rb_Hour.AutoSize = true;
            this.rb_Hour.Location = new System.Drawing.Point(142, 73);
            this.rb_Hour.Name = "rb_Hour";
            this.rb_Hour.Size = new System.Drawing.Size(35, 16);
            this.rb_Hour.TabIndex = 26;
            this.rb_Hour.TabStop = true;
            this.rb_Hour.Text = "时";
            this.rb_Hour.UseVisualStyleBackColor = true;
            this.rb_Hour.CheckedChanged += new System.EventHandler(this.rb_Hour_CheckedChanged);
            // 
            // cb_day
            // 
            this.cb_day.FormattingEnabled = true;
            this.cb_day.Location = new System.Drawing.Point(75, 71);
            this.cb_day.Name = "cb_day";
            this.cb_day.Size = new System.Drawing.Size(39, 20);
            this.cb_day.TabIndex = 25;
            // 
            // rb_Day
            // 
            this.rb_Day.AutoSize = true;
            this.rb_Day.Location = new System.Drawing.Point(46, 74);
            this.rb_Day.Name = "rb_Day";
            this.rb_Day.Size = new System.Drawing.Size(35, 16);
            this.rb_Day.TabIndex = 24;
            this.rb_Day.TabStop = true;
            this.rb_Day.Text = "日";
            this.rb_Day.UseVisualStyleBackColor = true;
            this.rb_Day.CheckedChanged += new System.EventHandler(this.rb_Day_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "步长：";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy年MM月dd日HH时";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(74, 40);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(136, 21);
            this.dateTimePicker2.TabIndex = 21;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy年MM月dd日HH时";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(74, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(136, 21);
            this.dateTimePicker1.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "结束时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "开始时间：";
            // 
            // cbl_Item
            // 
            this.cbl_Item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbl_Item.FormattingEnabled = true;
            this.cbl_Item.Location = new System.Drawing.Point(0, 125);
            this.cbl_Item.Name = "cbl_Item";
            this.cbl_Item.Size = new System.Drawing.Size(319, 168);
            this.cbl_Item.TabIndex = 10;
            this.cbl_Item.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbl_Item_ItemCheck);
            // 
            // _38
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbl_Item);
            this.Controls.Add(this.panelEx1);
            this.Name = "_38";
            this.Size = new System.Drawing.Size(319, 293);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.CheckedListBox cbl_Item;
        private System.Windows.Forms.ComboBox cb_Minute;
        private System.Windows.Forms.RadioButton rb_Minute;
        private System.Windows.Forms.ComboBox cb_Hour;
        private System.Windows.Forms.RadioButton rb_Hour;
        private System.Windows.Forms.ComboBox cb_day;
        private System.Windows.Forms.RadioButton rb_Day;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
    }
}
