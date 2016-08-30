namespace CenterApp
{
    partial class LogInControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogInControl));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.button_LogInAnd = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.button_LogIn = new DevComponents.DotNetBar.ButtonX();
            this.textBox_password = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBox_username = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.button_LogInAnd);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.button_LogIn);
            this.panelEx1.Controls.Add(this.textBox_password);
            this.panelEx1.Controls.Add(this.textBox_username);
            this.panelEx1.Controls.Add(this.reflectionImage1);
            this.panelEx1.Location = new System.Drawing.Point(20, 63);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(430, 225);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // button_LogInAnd
            // 
            this.button_LogInAnd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_LogInAnd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.button_LogInAnd.Location = new System.Drawing.Point(283, 148);
            this.button_LogInAnd.Name = "button_LogInAnd";
            this.button_LogInAnd.Size = new System.Drawing.Size(80, 23);
            this.button_LogInAnd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.button_LogInAnd.TabIndex = 9;
            this.button_LogInAnd.Text = "登录并访问";
            this.button_LogInAnd.Click += new System.EventHandler(this.button_LogInAnd_Click);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(159, 93);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "密  码：";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(159, 50);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(66, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "用户名：";
            // 
            // button_LogIn
            // 
            this.button_LogIn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_LogIn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.button_LogIn.Location = new System.Drawing.Point(164, 148);
            this.button_LogIn.Name = "button_LogIn";
            this.button_LogIn.Size = new System.Drawing.Size(61, 23);
            this.button_LogIn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.button_LogIn.TabIndex = 6;
            this.button_LogIn.Text = "登   录";
            this.button_LogIn.Click += new System.EventHandler(this.button_LogIn_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.textBox_password.Border.Class = "TextBoxBorder";
            this.textBox_password.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBox_password.ForeColor = System.Drawing.Color.Black;
            this.textBox_password.Location = new System.Drawing.Point(239, 93);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '★';
            this.textBox_password.Size = new System.Drawing.Size(124, 21);
            this.textBox_password.TabIndex = 5;
            // 
            // textBox_username
            // 
            this.textBox_username.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.textBox_username.Border.Class = "TextBoxBorder";
            this.textBox_username.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBox_username.ForeColor = System.Drawing.Color.Black;
            this.textBox_username.Location = new System.Drawing.Point(238, 50);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(125, 21);
            this.textBox_username.TabIndex = 4;
            // 
            // reflectionImage1
            // 
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage1.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage1.Image")));
            this.reflectionImage1.Location = new System.Drawing.Point(45, 12);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(89, 159);
            this.reflectionImage1.TabIndex = 3;
            // 
            // LogInControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelEx1);
            this.Name = "LogInControl";
            this.Size = new System.Drawing.Size(542, 353);
            this.SizeChanged += new System.EventHandler(this.LogInControl_SizeChanged);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX button_LogIn;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox_password;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox_username;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        public DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX button_LogInAnd;
    }
}
