using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp
{
    public partial class LoginForm :  DevComponents.DotNetBar.Metro.MetroAppForm
    {
        MainForm MF = null;
        public LoginForm()
        {
            InitializeComponent();
            List<string> pros = Program.wrx.GetProjects();
            if (pros.Count > 0 && pros[0] == "未知") 
            {
                SetPojectForm spf = new SetPojectForm();
                spf.ShowDialog();
            }
        }
 
        private void button_login_Click(object sender, EventArgs e)
        {

            string username = "";
            string password = "";
            Program.wrx.ReadLoginXML(out username, out password);
            if (textBox_username.Text.Trim() == username && textBox_password.Text.Trim() == password)
            {
                Program.LoginState = true;
                this.Visible = false;
                if (MF == null)
                {
                    MF = new MainForm(this,comboBox_projects.SelectedValue.ToString());
                    MF.Show();
                }
                MF.buttonItem_Login.Enabled = false;
                MF.buttonItem_ReBootService.Enabled = true;
                MF.buttonItem_ReadRTU.Enabled = true;
                MF.buttonItem_SetCommand.Enabled = true;
                MF.buttonItem6.Enabled = true;
                MF.buttonItem_Rem.Enabled = true;
                MF.buttonItem_Manual.Enabled = true;
                MF.buttonItem_RealTime.Enabled = true;
                MF.buttonItem_SendMail.Enabled = true;
                MF.buttonItem_DataResave.Enabled = true;
                comboBox_projects.Enabled = false;

                if (MF.pageSliderPage3.Controls.Count > 0 && MF.pageSliderPage3.Controls[0] is YYApp.SetControl.SetSystemControl)
                {
                    (MF.pageSliderPage3.Controls[0] as YYApp.SetControl.SetSystemControl).button_Restar.Enabled = true;
                    (MF.pageSliderPage3.Controls[0] as YYApp.SetControl.SetSystemControl).button_Set1 .Enabled = true;
                    (MF.pageSliderPage3.Controls[0] as YYApp.SetControl.SetSystemControl).button_Set2.Enabled = true;
                    (MF.pageSliderPage3.Controls[0] as YYApp.SetControl.SetSystemControl).button_SetLocal4.Enabled = true;
                }
            }

            
            textBox_username.Text = "";
            textBox_password.Text = "";
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            comboBox_projects.Enabled = false;
            if (MF == null)
            {
                MF = new MainForm(this,comboBox_projects.SelectedValue.ToString());
                MF.Show();
            }
            MF.buttonItem_Login.Enabled = true;
            MF.buttonItem_ReBootService.Enabled = false;
            MF.buttonItem_ReadRTU.Enabled = false;
            MF.buttonItem_SetCommand.Enabled = false;
            MF.buttonItem6.Enabled = false;
            MF.buttonItem_Rem.Enabled = false;
            MF.buttonItem_Manual.Enabled = false;
            MF.buttonItem_RealTime.Enabled = false;
            MF.buttonItem_SendMail.Enabled = false;
            MF.buttonItem_DataResave.Enabled = false;

        }


        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='\r')
            {
                if (textBox_username.Text.Trim() == "" && textBox_password.Text.Trim() == "")
                {
                    button_cancel_Click(null, null);
                }
                else
                {
                    button_login_Click(null, null);
                }
            }
        }

        private void LoginForm_Activated(object sender, EventArgs e)
        {
            textBox_username.Focus();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            List<string> pros = Program.wrx.GetProjects();
            comboBox_projects.DataSource = pros;
            comboBox_projects.SelectedIndex = 0;

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            VerificationRegistration();
            Registration();
        }

        private void comboBox_projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.wrx.SetPath(Program.wrx.GetXmlPath(comboBox_projects.SelectedValue.ToString()));
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        #region 
        private void Registration()
        {
            string text = EnDe.DESDecrypt(EnDe.ReadAk(), "1q2w3e4r", "11111111");
            string EnCPU = EnDe.GetCPU();

            if (EnCPU != text)
            {
                TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks); ; 
                try
                {
                    ts2 = new TimeSpan(DateTime.Parse(text).Ticks);
                }
                catch
                {
                    //MessageBox.Show("注册文件错误，请购买正版！");
                    System.Environment.Exit(0);
                }

                TimeSpan tSpan = ts1 - ts2;
                int Day = (int)tSpan.TotalDays;
                if (Day > 300)
                {
                    //MessageBox.Show("系统试用结束，请购买正版！");
                    System.Environment.Exit(0);
                }
                else if (Day < 0) //可能用户向后调整了时间
                {
                    EnDe.WriteAk(EnDe.DESEncrypt("asdf", "1q2w3e4r", "11111111"));
                    //MessageBox.Show("注册文件错误，请购买正版！");
                    System.Environment.Exit(0);
                }
                else
                {
                    //MessageBox.Show("系统还可试用" + (30 - Day) + "天！");
                }
            }
        }
        private void VerificationRegistration()
        {
            if (EnDe.ReadAk() == null)
            {
                EnDe.WriteAk(EnDe.DESEncrypt(DateTime.Now.ToString(), "1q2w3e4r", "11111111"));
            }
        } 
        #endregion
    }
}
