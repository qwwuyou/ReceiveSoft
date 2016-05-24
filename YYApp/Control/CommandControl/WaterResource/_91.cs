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
    public partial class _91 : UserControl, ICommandControl
    {
        public _91()
        {
            InitializeComponent(); 
            this.Dock = DockStyle.Fill;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            int gnm = 0x91;
            CommandCode = "91";
            if (Stcds.Length > 1)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择单个测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }


            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("至少选择一项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            
            byte[] bt = new byte[sjy.Length / 8];
            for (int i = 0; i < bt.Length; i++)
                bt[i] = Convert.ToByte(sjy.Substring(i * 8, 8), 2);


            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, ByteArrayToHexStr(bt), int.Parse(RTU.First().PWD));

                commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }

            return commands;
        }


        /// <summary>
        /// byte[] 转 Hex字符串
        /// </summary>
        /// <param name="Data">byte数组</param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] Data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Data.Length; i++)
            {
                sb.Append(Data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private string Validate()
        {
            string sjy="";
            sjy += cb8.Checked ? "1" : "0";
            sjy += cb7.Checked ? "1" : "0";
            sjy += cb6.Checked ? "1" : "0";
            sjy += cb5.Checked ? "1" : "0";
            sjy += cb4.Checked ? "1" : "0";
            sjy += cb3.Checked ? "1" : "0";
            sjy += cb2.Checked ? "1" : "0";
            sjy += cb1.Checked ? "1" : "0";

            if (sjy != "00000000")
            {
                return sjy;
            }
            return null;
        }
    }
}
