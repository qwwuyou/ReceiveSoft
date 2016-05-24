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
    public partial class _34 : UserControl, ICommandControl
    {
        public _34(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Init(Stcds);
        }

        private void Init(string[] Stcds) 
        {
            //string Where = "";
            //IList<Service.Model.YY_RTU_WORK> WORKList = PublicBD.db.GetRTU_WORKList(Where);
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID = '120000000034'");
            if (CONFIGDATAList.Count > 0)
            { tb1.Text = CONFIGDATAList.First().ConfigVal; }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "";
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("输入定值量有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            int gnm = 0x34;
            CommandCode = "34";
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
            double A1 = 0;
            if (double.TryParse(tb1.Text.Trim(), out A1))
            {
                if (A1 >= 0 && A1 <=7999999999)
                {
                    return tb1.Text.Trim();
                }
            }
            return null;
        }
    }
}
