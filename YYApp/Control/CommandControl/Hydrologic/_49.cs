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
    public partial class _49 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _49(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            textBox_PWD_Init(Stcds);
        }

        private void textBox_PWD_Init(string[] Stcds) 
        {
            if (Stcds.Count() == 1) 
            {
                var rtu = from r in list where r.STCD == Stcds[0] select r;
                textBox_PWD.Text = rtu.First().PassWord;
            }
        }
        
        private string Validate() 
        {
            int pwd=0;
            if (!int.TryParse(textBox_PWD.Text.Trim(), out pwd)) 
            {
                return null;
            }
            return textBox_PWD.Text.Trim();
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "49";
            string[] commands = new string[Stcds.Length];


            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("密码输入有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        Package package = Package.Create_0x49Package(Stcds[i], 1, UInt16.Parse(model.First().PassWord), UInt16.Parse(sjy));
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                    }
                }
            }

            return commands;
        }
    }
}
