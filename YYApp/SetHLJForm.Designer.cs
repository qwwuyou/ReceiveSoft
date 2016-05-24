namespace YYApp
{
    partial class SetHLJForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetHLJForm));
            this.reflectionLabel1 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.button_OK = new DevComponents.DotNetBar.ButtonX();
            this.reflectionLabel3 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.reflectionImage2 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.textBox_PassWord = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_DataBase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Source = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_Close = new DevComponents.DotNetBar.ButtonX();
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
            this.button_OK.Location = new System.Drawing.Point(6, 191);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(63, 23);
            this.button_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_OK.TabIndex = 61;
            this.button_OK.Text = "保 存|测 试";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // reflectionLabel3
            // 
            this.reflectionLabel3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel3.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel3.Location = new System.Drawing.Point(75, 5);
            this.reflectionLabel3.Name = "reflectionLabel3";
            this.reflectionLabel3.Size = new System.Drawing.Size(142, 45);
            this.reflectionLabel3.TabIndex = 64;
            this.reflectionLabel3.Text = "<b><b><font size=\"+3\">黑龙江中间库信息</font></b></b>";
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
            this.reflectionImage2.Location = new System.Drawing.Point(4, -2);
            this.reflectionImage2.Name = "reflectionImage2";
            this.reflectionImage2.Size = new System.Drawing.Size(65, 75);
            this.reflectionImage2.TabIndex = 63;
            // 
            // textBox_PassWord
            // 
            this.textBox_PassWord.Location = new System.Drawing.Point(86, 160);
            this.textBox_PassWord.Name = "textBox_PassWord";
            this.textBox_PassWord.PasswordChar = '*';
            this.textBox_PassWord.Size = new System.Drawing.Size(126, 22);
            this.textBox_PassWord.TabIndex = 72;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 71;
            this.label8.Text = "密  码：";
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Location = new System.Drawing.Point(86, 127);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(126, 22);
            this.textBox_UserName.TabIndex = 70;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 69;
            this.label7.Text = "用户名：";
            // 
            // textBox_DataBase
            // 
            this.textBox_DataBase.Location = new System.Drawing.Point(86, 95);
            this.textBox_DataBase.Name = "textBox_DataBase";
            this.textBox_DataBase.Size = new System.Drawing.Size(126, 22);
            this.textBox_DataBase.TabIndex = 68;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 67;
            this.label6.Text = "数据库名称：";
            // 
            // textBox_Source
            // 
            this.textBox_Source.Location = new System.Drawing.Point(86, 63);
            this.textBox_Source.Name = "textBox_Source";
            this.textBox_Source.Size = new System.Drawing.Size(126, 22);
            this.textBox_Source.TabIndex = 66;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "服务器名称：";
            // 
            // button_Close
            // 
            this.button_Close.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Close.Location = new System.Drawing.Point(149, 191);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(63, 23);
            this.button_Close.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Close.TabIndex = 73;
            this.button_Close.Text = "关  闭";
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // SetHLJForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 226);
            this.ControlBox = false;
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.textBox_PassWord);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_UserName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_DataBase);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_Source);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.reflectionLabel3);
            this.Controls.Add(this.reflectionImage2);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.reflectionLabel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetHLJForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SetHLJForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel1;
        private DevComponents.DotNetBar.ButtonX button_OK;
        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel3;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage2;
        private System.Windows.Forms.TextBox textBox_PassWord;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_DataBase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Source;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.ButtonX button_Close;
    }
}