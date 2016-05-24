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
    public partial class _1C : UserControl, ICommandControl
    {
        public _1C(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Init(Stcds);
        }

        private void Init(string[] Stcds) 
        {
            string Where = "where stcd='" + Stcds[0]+ "'";
            //IList<Service.Model.YY_RTU_WORK> WORKList = PublicBD.db.GetRTU_WORKList(Where);
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID = '12000000001C'");
            if (CONFIGDATAList.Count > 0)
            {
                int RelayLength=0;
                if (int.TryParse(CONFIGDATAList.First().ConfigVal, out RelayLength))
                {
                    tb1.Text = RelayLength.ToString();
                }
                
            }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "";
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;
            if (rb1.Checked)
            {
                string sjy = Validate();
                if (sjy == null)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("输入转发中继引导码长有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                int gnm = 0x1C;
                CommandCode = "1C";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else 
            {
                int gnm = 0x60;
                CommandCode = "60";
                commands = new string[Stcds.Length];
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
            int A1 = 0;
            if (int.TryParse(tb1.Text.Trim(), out A1))
            {
                if (A1 >= 0 && A1 <= 255)
                {
                    return tb1.Text.Trim();
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
