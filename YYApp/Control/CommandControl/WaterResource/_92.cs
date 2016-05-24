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
    public partial class _92 : UserControl, ICommandControl
    {
        string[] Stcds = null;
        public _92(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Stcds = Stcds;
        }

        private void Init(string[] Stcds) 
        {
            string where = "where YY_RTU_CONFIGDATA.stcd in (";
            foreach (var item in Stcds)
            {
                where += "'" + item + "',";
            }
            if (where != "")
            {
                where = where.Substring(0, where.Length - 1);
            }
            where = where + ") and YY_RTU_CONFIGDATA.ConfigID in ('20') and (YY_RTU_CONFIGDATA.ItemID like '00000000__' or YY_RTU_CONFIGDATA.ItemID like '00001111__')";
            DataTable dt= PublicBD.db.GetRTU_CONFIGDATA(where);
            if (dt != null && dt.Rows.Count > 0) 
            {
                if (dt.Rows[0]["ItemID"].ToString().Substring(0, 8) == "00000000")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else { comboBox1.SelectedIndex = 1; }

                int num=0;
                if (int.TryParse(dt.Rows[0]["ItemID"].ToString().Substring(8, 2), out num)) 
                {
                    comboBox2.SelectedIndex = num - 1;
                }

                if (dt.Rows[0]["ConfigVal"].ToString() == "0")
                {
                    rb2.Checked = true;
                }
                else 
                {
                    rb1.Checked = true;
                }
            }
        }

        private void _92_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.Add("水泵");
            this.comboBox1.Items.Add("阀门/闸门");
            this.comboBox1.SelectedIndex = 0;
            for (int i = 1; i < 16; i++)
            {
                this.comboBox2.Items.Add(i.ToString());
                this.comboBox2.SelectedIndex = 0;
            }


            Init(Stcds);
        }



        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy = "";
            int gnm=0;
            if (rb1.Checked)
            {
                gnm = 0x92;
                CommandCode = "92";
            }
            else
            {
                gnm = 0x93;
                CommandCode = "93";
            }
            sjy=comboBox1.SelectedIndex == 0 ? "0000" : "1111";
            sjy += ",";
            sjy += comboBox2.SelectedIndex + 1;

            

            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }

            return commands;
        }
    }
}
