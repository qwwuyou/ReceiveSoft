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
    public partial class _1E : UserControl, ICommandControl
    {
        public _1E(string[] Stcds)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.comboBox1.Items.Add("不切换-(00)");
            this.comboBox1.Items.Add("自动切换-(11)");
            this.comboBox1.SelectedIndex = 0;

            this.comboBox2.Items.Add("不允许-(00)");
            this.comboBox2.Items.Add("允许-(11)");
            this.comboBox2.SelectedIndex = 0;

            Init(Stcds);
        }

        private void Init(string[] Stcds)
        {
            //string Where = " where STCD='" + Stcds[0] + "'";
            //IList<Service.Model.YY_RTU_WORK> WORKList = PublicBD.db.GetRTU_WORKList(Where);
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID like '120000001E__'");

            if (CONFIGDATAList.Count > 0)
            {
                //Service.Model.YY_RTU_WORK model = WORKList.First();
                var list =from d in CONFIGDATAList where d.ConfigID =="120000001ED0"  select d;
                if (list.Count() > 0)
                {
                    int AutoSwitch = 0;
                    if (int.TryParse(list.First().ConfigVal, out AutoSwitch))
                    {
                        if (AutoSwitch == 0)
                            this.comboBox1.SelectedIndex = 0;
                        else
                            this.comboBox1.SelectedIndex = 1;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED2" select d;
                if (list.Count() > 0)
                {
                    int Relaying = 0;
                    if (int.TryParse(list.First().ConfigVal, out Relaying))
                    {
                        if (Relaying == 0)
                            this.comboBox2.SelectedIndex = 0;
                        else
                            this.comboBox2.SelectedIndex = 1;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED4" select d;
                if (list.Count() > 0)
                {
                    int PowerReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out PowerReport))
                    {
                        if (PowerReport == 0)
                            checkBox1.Checked = false; 
                        else
                            checkBox1.Checked = true; 
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED5" select d;
                if (list.Count() > 0)
                {
                    int SwitchReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out SwitchReport))
                    {
                        if (SwitchReport == 0)
                            checkBox2.Checked = false;
                        else
                            checkBox2.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED6" select d;
                if (list.Count() > 0)
                {
                    int FaultReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out FaultReport))
                    {
                        if (FaultReport == 0)
                            checkBox3.Checked = false;
                        else
                            checkBox3.Checked = true;
                    }
                }

            }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;
            if (rb1.Checked)
            {
                int gnm = 0x1E;
                CommandCode = "1E";
                commands = new string[Stcds.Length];


                string strT = "0";
                strT += checkBox3.Checked ? "1" : "0";
                strT += checkBox2.Checked ? "1" : "0";
                strT += checkBox1.Checked ? "1" : "0";
                strT += comboBox2.SelectedIndex == 0 ? "00" : "11";
                strT += comboBox1.SelectedIndex == 0 ? "00" : "11";
                string strData = ((int)Convert.ToByte(strT, 2)).ToString();

                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, strData, int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else 
            {
                int gnm = 0x63;
                CommandCode = "63";
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
