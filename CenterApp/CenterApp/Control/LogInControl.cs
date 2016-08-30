using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace CenterApp
{
    public partial class LogInControl : UserControl
    {
        public LogInControl(StyleManager stylemanager)
        {
            InitializeComponent();
            SetStyle(stylemanager);
        }

        public void SetStyle(StyleManager stylemanager) 
        {
            panelEx1.Style.BackColor1.Color = stylemanager.MetroColorParameters.CanvasColor;
            panelEx1.Style.BorderColor.Color = stylemanager.MetroColorParameters.BaseColor;
        }

        public void SetControlEnabled() 
        {
            if (TcpControl.UserID != "")
            {
                textBox_username.Enabled = false;
                textBox_password.Enabled = false;
                button_LogInAnd.Enabled = false;

                button_LogIn.Text = "注   销";
            }
            else 
            {
                textBox_username.Enabled = true ;
                textBox_password.Enabled = true;
                button_LogInAnd.Enabled = true;

                button_LogIn.Text = "登   录";
            }
        }

        private void LogInControl_SizeChanged(object sender, EventArgs e)
        {
            panelEx1.Location = new Point((panelEx1.Parent.Width - panelEx1.Width) / 2, (panelEx1.Parent.Height - panelEx1.Height) / 2);
        }

        private void button_LogInAnd_Click(object sender, EventArgs e)
        {
            string TimeTtamp =DateTime.Now.ToString("yyyyMMddHHmmssfff");
            TcpControl.UserID = "";// DataAccess.LogIn(textBox_username.Text.Trim(), textBox_password.Text.Trim()) + TimeTtamp;
            if (TcpControl.UserID != "")
            {

                try
                {
                    //System.Diagnostics.Process.Start(WriteReadXML.Browser, WriteReadXML.Url + "?t=l&n=" + textBox_username.Text.Trim() + "&p=" + textBox_password.Text.Trim() + "&ti=" + TimeTtamp);
                }
                catch 
                {
                    //System.Diagnostics.Process.Start(WriteReadXML.Url + "?t=l&n=" + textBox_username.Text.Trim() + "&p=" + textBox_password.Text.Trim() + "&ti=" + TimeTtamp);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("登录失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox_username.Text = "";
                textBox_password.Text = "";
            }
            SetControlEnabled();
        }

        public void button_LogIn_Click(object sender, EventArgs e)
        {
            if (button_LogIn.Text == "登   录")
            {
                TcpControl.UserID = "";// DataAccess.LogIn(textBox_username.Text.Trim(), textBox_password.Text.Trim());
                if (TcpControl.UserID != "")
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show(textBox_username.Text.Trim() + "登录成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("登录失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_username.Text = "";
                    textBox_password.Text = "";
                }
            }
            else
            {
                TcpControl.UserID = "";
                textBox_username.Text = "";
                textBox_password.Text = "";
            }
            SetControlEnabled();
        }
    }
}
