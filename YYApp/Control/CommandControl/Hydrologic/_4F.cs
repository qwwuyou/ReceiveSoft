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
    public partial class _4F : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _4F()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
        }
        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "4F";
            string[] commands = new string[Stcds.Length];


            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        Package package = Package.Create_0x4FPackage(Stcds[i], 1, UInt16.Parse(model.First().PassWord), RB_open.Checked );
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                    }
                }
            }

            return commands;
        }

      
    }
}

