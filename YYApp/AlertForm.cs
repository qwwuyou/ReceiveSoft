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
    public partial class AlertForm : DevComponents.DotNetBar.Balloon
    {
        int Count = 0;
        public AlertForm(int count)
        {
            InitializeComponent();
            Count = count;
        }

        private void AlertForm_Load(object sender, EventArgs e)
        {
            labelX1.Text = "<b><font size='+2'>用户您好！</font></b>\n\n";
            labelX2.Text = "系统中有站号和站名一致的测站  <b><font size='+3'>" + Count + "</font></b> \n" +
                           "个,可能是系统接收数据后自动添加的测站信息，\n\n" +
                           "请您在“基础信息”中查看并完善相关信息！";
             
        }
    }
}
