namespace CenterApp
{
    partial class NetworkControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkControl));
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.textBox_Port = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBox_IP = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.button_Save = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.reflectionImage2 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(124, 95);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(77, 23);
            this.labelX4.TabIndex = 26;
            this.labelX4.Text = "端   口：";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(124, 42);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(77, 23);
            this.labelX1.TabIndex = 25;
            this.labelX1.Text = "IP地址：";
            // 
            // textBox_Port
            // 
            this.textBox_Port.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.textBox_Port.Border.Class = "TextBoxBorder";
            this.textBox_Port.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBox_Port.ForeColor = System.Drawing.Color.Black;
            this.textBox_Port.Location = new System.Drawing.Point(204, 95);
            this.textBox_Port.MaxLength = 7;
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(139, 21);
            this.textBox_Port.TabIndex = 24;
            // 
            // textBox_IP
            // 
            this.textBox_IP.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBox_IP.Border.Class = "TextBoxBorder";
            this.textBox_IP.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBox_IP.ForeColor = System.Drawing.Color.Black;
            this.textBox_IP.Location = new System.Drawing.Point(204, 42);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(139, 21);
            this.textBox_IP.TabIndex = 23;
            // 
            // button_Save
            // 
            this.button_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.button_Save.Location = new System.Drawing.Point(263, 139);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(80, 23);
            this.button_Save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.button_Save.TabIndex = 21;
            this.button_Save.Text = "保存并连接";
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.reflectionImage2);
            this.panelEx1.Controls.Add(this.reflectionImage1);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.button_Save);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.textBox_IP);
            this.panelEx1.Controls.Add(this.textBox_Port);
            this.panelEx1.Location = new System.Drawing.Point(116, 41);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(430, 225);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 27;
            // 
            // reflectionImage1
            // 
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage1.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage1.Image")));
            this.reflectionImage1.Location = new System.Drawing.Point(15, 19);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(103, 131);
            this.reflectionImage1.TabIndex = 22;
            // 
            // reflectionImage2
            // 
            // 
            // 
            // 
            this.reflectionImage2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage2.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage2.Image = global::CenterApp.Properties.Resources.no;
            this.reflectionImage2.Location = new System.Drawing.Point(354, 137);
            this.reflectionImage2.Name = "reflectionImage2";
            this.reflectionImage2.Size = new System.Drawing.Size(35, 50);
            this.reflectionImage2.TabIndex = 27;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // NetworkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "NetworkControl";
            this.Size = new System.Drawing.Size(644, 294);
            this.SizeChanged += new System.EventHandler(this.NetworkControl_SizeChanged);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox_Port;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox_IP;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
        private DevComponents.DotNetBar.ButtonX button_Save;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage2;
        private System.Windows.Forms.Timer timer1;
    }
}
