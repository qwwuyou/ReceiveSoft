using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp.CommandControl
{
    public partial class _10 : UserControl, ICommandControl
    {
        public _10()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }


        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "";
            string[] commands = null;
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            if (rb1.Checked) //设置
            {

                if (Stcds.Length > 1)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("请选择单个测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                
                string sjy = Validate();

                if (sjy == null)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据输入不正确！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                int gnm = 0x10;
                CommandCode = "10";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else  //查询
            {
                commands = new string[Stcds.Length];
                int gnm = 0x50;
                CommandCode = "50";
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, "", int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }

            return commands;
        }


        private string Validate() 
        {
            int A1=0;
            int A2 = 0;
            if (int.TryParse(tb1.Text.Trim(), out A1) || tb1.Text.Trim().Length ==6) 
            {
                if (A1>=0)
                if (int.TryParse(tb2.Text.Trim(), out A2)) 
                {
                    if (A2>=0 && A2 <= 65535) 
                    {
                        return tb1.Text.Trim() + tb2.Text.Trim();
                    }
                }
            }

            return null;
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            if (rb2.Checked)
            {
                foreach (var item in groupBox1.Controls)
                {
                    (item as Control).Enabled = false;
                }
            }
            else 
            {
                foreach (var item in groupBox1.Controls)
                {
                    (item as Control).Enabled = true;
                }
            }
        }
    }
}
