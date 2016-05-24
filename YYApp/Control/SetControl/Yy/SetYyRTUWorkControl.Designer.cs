namespace YYApp.SetControl
{
    partial class SetYyRTUWorkControl
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
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.comboBox_STCD = new System.Windows.Forms.ComboBox();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.button_ReadRTU = new DevComponents.DotNetBar.ButtonX();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_MODE = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBox_SatelliteNum = new System.Windows.Forms.TextBox();
            this.textBox_PhoneNum = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Interval = new System.Windows.Forms.TextBox();
            this.textBox_WaterLevel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_Rainfall = new System.Windows.Forms.TextBox();
            this.button_ReBootService = new DevComponents.DotNetBar.ButtonX();
            this.button_Del = new DevComponents.DotNetBar.ButtonX();
            this.button_Set = new DevComponents.DotNetBar.ButtonX();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Address = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.label38);
            this.panelEx1.Controls.Add(this.label37);
            this.panelEx1.Controls.Add(this.comboBox_STCD);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(901, 45);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 22;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.ForeColor = System.Drawing.Color.Red;
            this.label38.Location = new System.Drawing.Point(361, 15);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(173, 12);
            this.label38.TabIndex = 24;
            this.label38.Text = "编辑内容后回车，进行模糊查询";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(20, 15);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(59, 12);
            this.label37.TabIndex = 21;
            this.label37.Text = "测   站：";
            // 
            // comboBox_STCD
            // 
            this.comboBox_STCD.FormattingEnabled = true;
            this.comboBox_STCD.Location = new System.Drawing.Point(85, 12);
            this.comboBox_STCD.Name = "comboBox_STCD";
            this.comboBox_STCD.Size = new System.Drawing.Size(256, 20);
            this.comboBox_STCD.TabIndex = 20;
            this.comboBox_STCD.SelectedIndexChanged += new System.EventHandler(this.comboBox_STCD_SelectedIndexChanged);
            this.comboBox_STCD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_STCD_KeyPress);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.groupBox5);
            this.panelEx2.Controls.Add(this.button_ReadRTU);
            this.panelEx2.Controls.Add(this.groupBox4);
            this.panelEx2.Controls.Add(this.groupBox3);
            this.panelEx2.Controls.Add(this.button_ReBootService);
            this.panelEx2.Controls.Add(this.button_Del);
            this.panelEx2.Controls.Add(this.button_Set);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(0, 45);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(901, 371);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 23;
            // 
            // button_ReadRTU
            // 
            this.button_ReadRTU.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_ReadRTU.Location = new System.Drawing.Point(645, 253);
            this.button_ReadRTU.Name = "button_ReadRTU";
            this.button_ReadRTU.Size = new System.Drawing.Size(75, 23);
            this.button_ReadRTU.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_ReadRTU.TabIndex = 63;
            this.button_ReadRTU.Text = "重置信息";
            this.button_ReadRTU.Tooltip = "修改参数后使服务重新读取RTU信息";
            this.button_ReadRTU.Click += new System.EventHandler(this.button_ReadRTU_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.comboBox_MODE);
            this.groupBox4.Controls.Add(this.textBox_SatelliteNum);
            this.groupBox4.Controls.Add(this.textBox_PhoneNum);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Location = new System.Drawing.Point(12, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(240, 125);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 44;
            this.label1.Text = "工作模式：";
            // 
            // comboBox_MODE
            // 
            this.comboBox_MODE.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox_MODE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MODE.FormattingEnabled = true;
            this.comboBox_MODE.Items.AddRange(new object[] {
            "自报工作状态(01)",
            "查询/应答工作状态(02)",
            "调试/维修状态(03)"});
            this.comboBox_MODE.Location = new System.Drawing.Point(99, 23);
            this.comboBox_MODE.Name = "comboBox_MODE";
            this.comboBox_MODE.Size = new System.Drawing.Size(126, 22);
            this.comboBox_MODE.TabIndex = 43;
            // 
            // textBox_SatelliteNum
            // 
            this.textBox_SatelliteNum.Location = new System.Drawing.Point(99, 86);
            this.textBox_SatelliteNum.MaxLength = 10;
            this.textBox_SatelliteNum.Name = "textBox_SatelliteNum";
            this.textBox_SatelliteNum.Size = new System.Drawing.Size(126, 21);
            this.textBox_SatelliteNum.TabIndex = 48;
            // 
            // textBox_PhoneNum
            // 
            this.textBox_PhoneNum.Location = new System.Drawing.Point(99, 56);
            this.textBox_PhoneNum.MaxLength = 11;
            this.textBox_PhoneNum.Name = "textBox_PhoneNum";
            this.textBox_PhoneNum.Size = new System.Drawing.Size(126, 21);
            this.textBox_PhoneNum.TabIndex = 47;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 45;
            this.label11.Text = "手机号：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 89);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 46;
            this.label12.Text = "卫星号：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_Interval);
            this.groupBox3.Controls.Add(this.textBox_WaterLevel);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox_Rainfall);
            this.groupBox3.Location = new System.Drawing.Point(290, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 125);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(221, 93);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 16;
            this.label16.Text = "厘米";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(221, 61);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 15;
            this.label15.Text = "斗";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "自报间隔：";
            // 
            // textBox_Interval
            // 
            this.textBox_Interval.Location = new System.Drawing.Point(88, 23);
            this.textBox_Interval.MaxLength = 2;
            this.textBox_Interval.Name = "textBox_Interval";
            this.textBox_Interval.Size = new System.Drawing.Size(126, 21);
            this.textBox_Interval.TabIndex = 10;
            // 
            // textBox_WaterLevel
            // 
            this.textBox_WaterLevel.Location = new System.Drawing.Point(88, 90);
            this.textBox_WaterLevel.MaxLength = 5;
            this.textBox_WaterLevel.Name = "textBox_WaterLevel";
            this.textBox_WaterLevel.Size = new System.Drawing.Size(127, 21);
            this.textBox_WaterLevel.TabIndex = 14;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "水位量级：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 61);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 11;
            this.label14.Text = "雨量量级：";
            // 
            // textBox_Rainfall
            // 
            this.textBox_Rainfall.Location = new System.Drawing.Point(88, 58);
            this.textBox_Rainfall.MaxLength = 3;
            this.textBox_Rainfall.Name = "textBox_Rainfall";
            this.textBox_Rainfall.Size = new System.Drawing.Size(127, 21);
            this.textBox_Rainfall.TabIndex = 12;
            // 
            // button_ReBootService
            // 
            this.button_ReBootService.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_ReBootService.Location = new System.Drawing.Point(762, 253);
            this.button_ReBootService.Name = "button_ReBootService";
            this.button_ReBootService.Size = new System.Drawing.Size(75, 23);
            this.button_ReBootService.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_ReBootService.TabIndex = 60;
            this.button_ReBootService.Text = "重启服务";
            this.button_ReBootService.Tooltip = "修改数据后请重启通讯服务软件";
            this.button_ReBootService.Click += new System.EventHandler(this.button_ReBootService_Click);
            // 
            // button_Del
            // 
            this.button_Del.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Del.Location = new System.Drawing.Point(645, 217);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(75, 23);
            this.button_Del.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Del.TabIndex = 59;
            this.button_Del.Text = "删除";
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // button_Set
            // 
            this.button_Set.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Set.Location = new System.Drawing.Point(762, 217);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(75, 23);
            this.button_Set.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Set.TabIndex = 57;
            this.button_Set.Text = "配 置";
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.textBox_Address);
            this.groupBox5.Location = new System.Drawing.Point(589, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(260, 125);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "中心站地址：";
            // 
            // textBox_Address
            // 
            this.textBox_Address.Location = new System.Drawing.Point(88, 23);
            this.textBox_Address.MaxLength = 2;
            this.textBox_Address.Name = "textBox_Address";
            this.textBox_Address.Size = new System.Drawing.Size(126, 21);
            this.textBox_Address.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(217, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "1~255";
            // 
            // SetYyRTUWorkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.panelEx1);
            this.Name = "SetYyRTUWorkControl";
            this.Size = new System.Drawing.Size(901, 416);
            this.Load += new System.EventHandler(this.SetRTUWorkControl_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.panelEx2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox comboBox_STCD;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX button_Set;
        private DevComponents.DotNetBar.ButtonX button_Del;
        private DevComponents.DotNetBar.ButtonX button_ReBootService;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Interval;
        private System.Windows.Forms.TextBox textBox_WaterLevel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_Rainfall;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBox_MODE;
        private System.Windows.Forms.TextBox textBox_SatelliteNum;
        private System.Windows.Forms.TextBox textBox_PhoneNum;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private DevComponents.DotNetBar.ButtonX button_ReadRTU;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Address;
    }
}
