namespace YYApp
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.reflectionLabel1 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.reflectionLabel2 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.button_login = new DevComponents.DotNetBar.ButtonX();
            this.button_cancel = new DevComponents.DotNetBar.ButtonX();
            this.comboBox_projects = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.reflectionLabel3 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.button_close = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // reflectionLabel1
            // 
            this.reflectionLabel1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel1.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel1.Location = new System.Drawing.Point(30, 49);
            this.reflectionLabel1.Name = "reflectionLabel1";
            this.reflectionLabel1.Size = new System.Drawing.Size(68, 37);
            this.reflectionLabel1.TabIndex = 0;
            this.reflectionLabel1.Text = "<b><font size=\"+.5\">用户名：</font></b>";
            // 
            // reflectionLabel2
            // 
            this.reflectionLabel2.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel2.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel2.Location = new System.Drawing.Point(30, 80);
            this.reflectionLabel2.Name = "reflectionLabel2";
            this.reflectionLabel2.Size = new System.Drawing.Size(68, 37);
            this.reflectionLabel2.TabIndex = 1;
            this.reflectionLabel2.Text = "<b><font size=\"+.5\">密 码：</font></b>";
            // 
            // textBox_username
            // 
            this.textBox_username.BackColor = System.Drawing.Color.White;
            this.textBox_username.ForeColor = System.Drawing.Color.Black;
            this.textBox_username.Location = new System.Drawing.Point(104, 51);
            this.textBox_username.MaxLength = 20;
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(124, 21);
            this.textBox_username.TabIndex = 2;
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.Color.White;
            this.textBox_password.ForeColor = System.Drawing.Color.Black;
            this.textBox_password.Location = new System.Drawing.Point(104, 88);
            this.textBox_password.MaxLength = 20;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(124, 21);
            this.textBox_password.TabIndex = 3;
            // 
            // button_login
            // 
            this.button_login.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_login.Location = new System.Drawing.Point(80, 121);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(59, 23);
            this.button_login.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_login.TabIndex = 21;
            this.button_login.Text = "用  户";
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_cancel.Location = new System.Drawing.Point(169, 121);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(59, 23);
            this.button_cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_cancel.TabIndex = 22;
            this.button_cancel.Text = "匿  名";
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // comboBox_projects
            // 
            this.comboBox_projects.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox_projects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_projects.ForeColor = System.Drawing.Color.Black;
            this.comboBox_projects.FormattingEnabled = true;
            this.comboBox_projects.Location = new System.Drawing.Point(104, 13);
            this.comboBox_projects.Name = "comboBox_projects";
            this.comboBox_projects.Size = new System.Drawing.Size(124, 22);
            this.comboBox_projects.TabIndex = 23;
            this.comboBox_projects.SelectedIndexChanged += new System.EventHandler(this.comboBox_projects_SelectedIndexChanged);
            // 
            // reflectionLabel3
            // 
            this.reflectionLabel3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.reflectionLabel3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel3.ForeColor = System.Drawing.Color.Black;
            this.reflectionLabel3.Location = new System.Drawing.Point(30, 11);
            this.reflectionLabel3.Name = "reflectionLabel3";
            this.reflectionLabel3.Size = new System.Drawing.Size(68, 37);
            this.reflectionLabel3.TabIndex = 24;
            this.reflectionLabel3.Text = "<b><font size=\"+.5\">项  目：</font></b>";
            // 
            // button_close
            // 
            this.button_close.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_close.BackColor = System.Drawing.Color.Red;
            this.button_close.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.button_close.Location = new System.Drawing.Point(238, -2);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(20, 23);
            this.button_close.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_close.TabIndex = 25;
            this.button_close.Text = "<b>×</b>";
            this.button_close.TextColor = System.Drawing.Color.White;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 153);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.reflectionLabel3);
            this.Controls.Add(this.comboBox_projects);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.reflectionLabel2);
            this.Controls.Add(this.reflectionLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录窗口";
            this.Activated += new System.EventHandler(this.LoginForm_Activated);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LoginForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel1;
        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel2;
        private System.Windows.Forms.TextBox textBox_password;
        private DevComponents.DotNetBar.ButtonX button_login;
        private DevComponents.DotNetBar.ButtonX button_cancel;
        public System.Windows.Forms.TextBox textBox_username;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBox_projects;
        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel3;
        private DevComponents.DotNetBar.ButtonX button_close;
    }
}