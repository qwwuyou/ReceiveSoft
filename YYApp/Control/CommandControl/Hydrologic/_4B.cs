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
    public partial class _4B : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        List<Service.Model.YY_RTU_CONFIGDATA> list1;
        public _4B(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();

            string In = "";
            if (Stcds.Length > 0)
            {
                for (int i = 0; i < Stcds.Length; i++)
                {
                    In += "'" + Stcds[i] + "',";
                }
                In = In.Substring(0, In.Length - 1);
            }
            list1 = Service.PublicBD.db.GetRTU_CONFIGDATAList(" where STCD in (" + In + ") and ConfigID like '1100____AA02'").ToList<Service.Model.YY_RTU_CONFIGDATA>();
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "4B";
            string[] commands = new string[Stcds.Length];


            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    string _11000000AA02 = "111111111" + "11111111111111111111111"; //状态和报警信息
                    var configdata = from c in list1 where c.STCD == Stcds[i] select c;
                    if (configdata.Count() > 0)
                    {
                        _11000000AA02 = configdata.First().ConfigVal;
                    }
                    if (RB1.Checked)
                    {
                        _11000000AA02 = _11000000AA02.Substring(0, 8) + "1" + _11000000AA02.Substring(10, 22);
                    }
                    else 
                    {
                        _11000000AA02 = _11000000AA02.Substring(0, 9) + "0" + _11000000AA02.Substring(10, 22);
                    }


                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        Package package = Package.Create_0x4BPackage(Stcds[i], 1, UInt16.Parse(model.First().PassWord), _11000000AA02);
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                    }
                }
            }

            return commands;
        }
    }
}
