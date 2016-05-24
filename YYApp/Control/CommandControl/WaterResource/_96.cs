using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;

namespace YYApp.CommandControl
{
    public partial class _96 : UserControl, ICommandControl
    {
        public _96(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Init(Stcds) ;
        }

        private void Init(string[] Stcds) 
        {
            string Where = "where stcd='"+Stcds.First()+"'";
            IList<Service.Model.YY_RTU_Basic> RTU = PublicBD.db.GetRTUList(Where);
            textBox1.Text = RTU.First().PassWord;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy = "";
            int gnm = 0x96;
            CommandCode = "96";
            sjy=Validate();
            if (sjy == null) 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("密码输入有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                return null;
            }
            

            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }

            return commands;
        }


        private string Validate()
        {
            int pwd = 0;
            if (int.TryParse(textBox1.Text.Trim(), out pwd)) 
            {
                if (pwd >= 0 && pwd <= 65535) 
                {
                    return pwd.ToString();
                }
            }
            return null;
        }
    }
}
