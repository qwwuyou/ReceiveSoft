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
    public partial class _02 : UserControl, ICommandControl
    {
        public _02()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        
        public string[] GetCommand(string[] Stcds, string NFOINDEX,out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy = "";
            int gnm = 0x02;
            CommandCode = "02";
            if (rb1.Checked) { sjy = "F0"; }
            else if (rb2.Checked) { sjy = "F1"; }
            else { sjy = "F2"; }

            string[] commands=new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU=from rtu in ExecRTUList.Lrdm where rtu.STCD ==Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                commands[i]=YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }

            return commands;
        }
    }
}
