using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YanYu.Protocol.HIMR;

namespace YYApp.CommandControl
{
    public partial class _4C : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _4C()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
        }

        public string Get4C() 
        {
            string _4C = "";
            if (RB1_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB2_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB3_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; } 
            if (RB4_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB5_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB6_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB7_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            if (RB8_open.Checked)
            { _4C += "1"; }
            else
            { _4C += "0"; }
            return _4C;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "4C";
            string[] commands = new string[Stcds.Length];
            string _11000000AA04 = Get4C();
            for (int i = 0; i < Stcds.Length; i++)
            {
                var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                if (model.Count() > 0)
                {
                    Package package = Package.Create_0x4CPackage(Stcds[i], 1, UInt16.Parse(model.First().PassWord), _11000000AA04);
                    commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                }
            }

            return commands;
        }
    }
}
