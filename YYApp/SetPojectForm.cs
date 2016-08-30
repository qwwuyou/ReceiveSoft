using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace YYApp
{
    public partial class SetPojectForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public SetPojectForm()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (textBox_projectname.Text.Trim() != "")
            {
                OperateXML.ProjectInfo pi = new OperateXML.ProjectInfo();
                pi.Project = textBox_projectname.Text.Trim();
                pi.Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
                Program.wrx.XMLObj.projects.Clear();
                Program.wrx.XMLObj.projects.Add(pi);
                Program.wrx.WriteXML();

                this.Close();
            }
            else 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请输入项目名称！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void SetPojectForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                button_OK_Click(null,null);
            }
        }


    }
}