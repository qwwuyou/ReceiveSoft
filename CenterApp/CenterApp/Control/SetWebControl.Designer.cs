namespace CenterApp
{
    partial class SetWebControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetWebControl));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.button_Save = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.textBox_Url = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.reflectionImage2 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.reflectionImage3 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.reflectionImage4 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.radioButton_ie = new System.Windows.Forms.RadioButton();
            this.radioButton_Chrome = new System.Windows.Forms.RadioButton();
            this.radioButton_Firefox = new System.Windows.Forms.RadioButton();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.radioButton_Firefox);
            this.panelEx1.Controls.Add(this.radioButton_Chrome);
            this.panelEx1.Controls.Add(this.radioButton_ie);
            this.panelEx1.Controls.Add(this.reflectionImage4);
            this.panelEx1.Controls.Add(this.reflectionImage3);
            this.panelEx1.Controls.Add(this.reflectionImage2);
            this.panelEx1.Controls.Add(this.reflectionImage1);
            this.panelEx1.Controls.Add(this.button_Save);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.textBox_Url);
            this.panelEx1.Location = new System.Drawing.Point(99, 51);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(430, 225);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 28;
            // 
            // reflectionImage1
            // 
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage1.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage1.Image")));
            this.reflectionImage1.Location = new System.Drawing.Point(15, 13);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(103, 131);
            this.reflectionImage1.TabIndex = 22;
            // 
            // button_Save
            // 
            this.button_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.button_Save.Location = new System.Drawing.Point(324, 178);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(61, 23);
            this.button_Save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.button_Save.TabIndex = 21;
            this.button_Save.Text = "保   存";
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(124, 36);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(77, 23);
            this.labelX1.TabIndex = 25;
            this.labelX1.Text = "Web网址：";
            // 
            // textBox_Url
            // 
            this.textBox_Url.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.textBox_Url.Border.Class = "TextBoxBorder";
            this.textBox_Url.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBox_Url.ForeColor = System.Drawing.Color.Black;
            this.textBox_Url.Location = new System.Drawing.Point(124, 71);
            this.textBox_Url.Name = "textBox_Url";
            this.textBox_Url.Size = new System.Drawing.Size(261, 21);
            this.textBox_Url.TabIndex = 23;
            // 
            // reflectionImage2
            // 
            // 
            // 
            // 
            this.reflectionImage2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage2.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage2.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage2.Image")));
            this.reflectionImage2.Location = new System.Drawing.Point(148, 104);
            this.reflectionImage2.Name = "reflectionImage2";
            this.reflectionImage2.Size = new System.Drawing.Size(53, 72);
            this.reflectionImage2.TabIndex = 26;
            // 
            // reflectionImage3
            // 
            // 
            // 
            // 
            this.reflectionImage3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage3.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage3.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage3.Image")));
            this.reflectionImage3.Location = new System.Drawing.Point(234, 104);
            this.reflectionImage3.Name = "reflectionImage3";
            this.reflectionImage3.Size = new System.Drawing.Size(53, 72);
            this.reflectionImage3.TabIndex = 27;
            // 
            // reflectionImage4
            // 
            // 
            // 
            // 
            this.reflectionImage4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage4.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage4.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage4.Image")));
            this.reflectionImage4.Location = new System.Drawing.Point(313, 104);
            this.reflectionImage4.Name = "reflectionImage4";
            this.reflectionImage4.Size = new System.Drawing.Size(53, 72);
            this.reflectionImage4.TabIndex = 28;
            // 
            // radioButton_ie
            // 
            this.radioButton_ie.AutoSize = true;
            this.radioButton_ie.Checked = true;
            this.radioButton_ie.Location = new System.Drawing.Point(136, 125);
            this.radioButton_ie.Name = "radioButton_ie";
            this.radioButton_ie.Size = new System.Drawing.Size(14, 13);
            this.radioButton_ie.TabIndex = 29;
            this.radioButton_ie.UseVisualStyleBackColor = true;
            // 
            // radioButton_Chrome
            // 
            this.radioButton_Chrome.AutoSize = true;
            this.radioButton_Chrome.Location = new System.Drawing.Point(224, 125);
            this.radioButton_Chrome.Name = "radioButton_Chrome";
            this.radioButton_Chrome.Size = new System.Drawing.Size(14, 13);
            this.radioButton_Chrome.TabIndex = 30;
            this.radioButton_Chrome.UseVisualStyleBackColor = true;
            // 
            // radioButton_Firefox
            // 
            this.radioButton_Firefox.AutoSize = true;
            this.radioButton_Firefox.Location = new System.Drawing.Point(302, 125);
            this.radioButton_Firefox.Name = "radioButton_Firefox";
            this.radioButton_Firefox.Size = new System.Drawing.Size(14, 13);
            this.radioButton_Firefox.TabIndex = 31;
            this.radioButton_Firefox.UseVisualStyleBackColor = true;
            // 
            // SetWebControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "SetWebControl";
            this.Size = new System.Drawing.Size(629, 327);
            this.SizeChanged += new System.EventHandler(this.SetWebControl_SizeChanged);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
        private DevComponents.DotNetBar.ButtonX button_Save;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox_Url;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage4;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage3;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage2;
        private System.Windows.Forms.RadioButton radioButton_Firefox;
        private System.Windows.Forms.RadioButton radioButton_Chrome;
        private System.Windows.Forms.RadioButton radioButton_ie;
    }
}
