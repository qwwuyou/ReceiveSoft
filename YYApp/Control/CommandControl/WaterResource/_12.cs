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
    public partial class _12 : UserControl, ICommandControl
    {
        public _12(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Init(Stcds);
        }

        public void Init(string[] Stcds) 
        {
            //string Where="where stcd='"+Stcds[0]+"'";
            //IList<Service.Model.YY_RTU_WORK>  WORKList=PublicBD.db.GetRTU_WORKList(Where);
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID = '120000000012'");
            if (CONFIGDATAList.Count > 0)
            {
                int mode = 0;
                if (int.TryParse(CONFIGDATAList.First().ConfigVal, out mode))
                {
                    if (mode == 0)
                    { rb1.Checked = true; }
                    else if (mode == 1)
                    { rb2.Checked = true; }
                    else if (mode == 2)
                    { rb3.Checked = true; }
                    else
                    { rb4.Checked = true; }
                }
            }

        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();

            string[] commands = null;
           
            if (rb5.Checked)
            {
                int gnm = 0x12;
                CommandCode = "12";
                commands = new string[Stcds.Length];

                string sjy = "";
                if (rb1.Checked) { sjy = "00"; }
                else if (rb2.Checked) { sjy = "01"; }
                else if (rb3.Checked) { sjy = "02"; }
                else { sjy = "03"; }

                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else
            {
                int gnm = 0x52;
                CommandCode = "52";
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

        private void rb6_CheckedChanged(object sender, EventArgs e)
        {
            if (rb6.Checked)
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
