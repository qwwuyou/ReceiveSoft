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
    public partial class _1B : UserControl, ICommandControl
    {
        public _1B(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Init(Stcds);
        }

        private void Init(string[] Stcds)
        {
            string Where = "where YY_RTU_CONFIGDATA.stcd='" + Stcds[0] + "' and YY_RTU_CONFIGDATA.ConfigID in ('21') and YY_RTU_CONFIGDATA.ItemID like '00002222__'";
            DataTable dt = PublicBD.db.GetRTU_CONFIGDATA(Where );
            string vals = "";
            if (dt != null && dt.Rows.Count > 0) 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vals += dt.Rows[i]["ConfigVal"].ToString() + ",";
                }
            }
            if (vals != "") 
            {
                tb1.Text = vals.Substring(0,vals.Length -1);
            }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "";
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("输入终端站水量的表底（初始）值有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            int gnm = 0x1B;
            CommandCode = "1B";
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
            if (tb1.Text.Trim().Length == 0) 
            {
                return null;
            }
            string[] vals = tb1.Text.Trim().Split(new char[]{','});
            foreach (var item in vals)
            {
                System.Int64 A1 = 0;
                if (System.Int64.TryParse(item, out A1))
                {
                    if (A1 < 0 && A1 > 7999999999)
                    {
                        return null;
                    }
                }
            }


            return tb1.Text.Trim();
        }
    }
}
