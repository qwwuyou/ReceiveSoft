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
    public partial class _A0 : UserControl, ICommandControl
    {
        string[] Stcds = null;
        public _A0(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Stcds = Stcds;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;
            if (rb6.Checked)
            {
                int gnm = 0x54;
                CommandCode = "54";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, "", int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else
            {
                string sjy = "";
                int gnm = 0xA0;
                CommandCode = "A0";
                sjy = Validate();


                byte[] bt = new byte[sjy.Length / 8];
                for (int i = 0; i < bt.Length; i++)
                    bt[i] = Convert.ToByte(sjy.Substring(i * 8, 8), 2);


                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, ByteArrayToHexStr(bt), int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            return commands;
        }

        private void _A0_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;

            string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "终端内存", "固态存储", "上报水压", "备用" };

            string Where = " where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID='1200000000A0'";
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList(Where);
            if (CONFIGDATAList.Count > 0 && CONFIGDATAList.First().ConfigVal.Length ==16)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (CONFIGDATAList.First().ConfigVal[i] == '1')
                    {
                        checkedListBox1.Items.Add(items[i], true);
                    }
                    else 
                    {
                        checkedListBox1.Items.Add(items[i], false);
                    }
                }
            }
            else 
            {
                foreach (var item in items)
                {
                    checkedListBox1.Items.Add(item, false);
                }
            }
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
            string temp = "";

            for (int i = checkedListBox1.Items.Count-1; i >= 0; i--)
            {
                temp += checkedListBox1.GetItemChecked(i)? "1" : "0";
            }
            
            ////取消了验证
            //if (temp != "000000000000000") 
            //{
                return temp;
            //}

            //return null;
        }

        private void rb6_CheckedChanged(object sender, EventArgs e)
        {
            if (rb6.Checked)
            {
                checkedListBox1.Enabled = false;
            }
            else
            {
                checkedListBox1.Enabled = true;
            }
        }
    }
}
