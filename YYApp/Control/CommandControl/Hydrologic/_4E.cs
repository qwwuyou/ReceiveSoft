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
    public partial class _4E : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _4E()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
        }

        public string Get4E()
        {
            string _4E = "";
            if (RB1_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB2_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB3_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB4_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB5_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB6_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB7_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            if (RB8_open.Checked)
            { _4E += "1"; }
            else
            { _4E += "0"; }
            return _4E;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "4E";
            string[] commands = new string[Stcds.Length];
            string _11000000AA06 = Get4E();
            for (int i = 0; i < Stcds.Length; i++)
            {
                var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                if (model.Count() > 0)
                {
                    Package package = Package.Create_0x4EPackage(Stcds[i], 1, UInt16.Parse(model.First().PassWord), _11000000AA06);
                    commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                }
            }

            return commands;
        }
    }
}
