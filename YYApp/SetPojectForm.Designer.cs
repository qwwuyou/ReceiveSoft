namespace YYApp
{
    partial class SetPojectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPojectForm));
            this.reflectionLabel1 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.button_OK = new DevComponents.DotNetBar.ButtonX();
            this.textBox_projectname = new System.Windows.Forms.TextBox();
            this.reflectionLabel3 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.reflectionImage2 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.SuspendLayout();
            // 
            // reflectionLabel1
            // 
            this.reflectionLabel1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel1.Font = new System.Drawing.Font("黑体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reflectionLabel1.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel1.Location = new System.Drawing.Point(62, 12);
            this.reflectionLabel1.Name = "reflectionLabel1";
            this.reflectionLabel1.Size = new System.Drawing.Size(46, 57);
            this.reflectionLabel1.TabIndex = 1;
            this.reflectionLabel1.Text = "<b><font size=\"+6\"></font></b>";
            // 
            // button_OK
            // 
            this.button_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_OK.Location = new System.Drawing.Point(152, 106);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(63, 23);
            this.button_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_OK.TabIndex = 61;
            this.button_OK.Text = "确 定";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // textBox_projectname
            // 
            this.textBox_projectname.BackColor = System.Drawing.Color.White;
            this.textBox_projectname.ForeColor = System.Drawing.Color.Black;
            this.textBox_projectname.Location = new System.Drawing.Point(51, 66);
            this.textBox_projectname.MaxLength = 20;
            this.textBox_projectname.Name = "textBox_projectname";
            this.textBox_projectname.Size = new System.Drawing.Size(146, 22);
            this.textBox_projectname.TabIndex = 62;
            // 
            // reflectionLabel3
            // 
            this.reflectionLabel3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel3.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel3.Location = new System.Drawing.Point(72, -3);
            this.reflectionLabel3.Name = "reflectionLabel3";
            this.reflectionLabel3.Size = new System.Drawing.Size(142, 68);
            this.reflectionLabel3.TabIndex = 64;
            this.reflectionLabel3.Text = "<b><b><font size=\"+4\">请输入项目名称</font></b></b>";
            // 
            // reflectionImage2
            // 
            this.reflectionImage2.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionImage2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage2.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage2.ForeColor = System.Drawing.Color.Black;
            this.reflectionImage2.Image = ((System.Drawing.Image)(resources.GetObject("reflectionImage2.Image")));
            this.reflectionImage2.Location = new System.Drawing.Point(8, -3);
            this.reflectionImage2.Name = "reflectionImage2";
            this.reflectionImage2.Size = new System.Drawing.Size(65, 75);
            this.reflectionImage2.TabIndex = 63;
            // 
            // SetPojectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 135);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_projectname);
            this.Controls.Add(this.reflectionLabel3);
            this.Controls.Add(this.reflectionImage2);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.reflectionLabel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetPojectForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SetPojectForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel1;
        private DevComponents.DotNetBar.ButtonX button_OK;
        public System.Windows.Forms.TextBox textBox_projectname;
        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel3;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage2;
    }
}