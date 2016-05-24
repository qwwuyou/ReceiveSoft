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
    public partial class _20 : UserControl, ICommandControl
    {
        string[] STCDS = null;
        public _20(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            STCDS = Stcds;
            Init(Stcds);
        }

        private void Init(string[] Stcds) 
        {
            //0~15
            string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速(风向)", "水温", "水质", "土壤含水率", "蒸发量", "水压", "备用1", "备用2", "备用3" };
           foreach (var item in items)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;

            string Where = " where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID='120000000020'";
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList(Where);
            if (CONFIGDATAList.Count > 0) 
            {
                string[] ItemGroup = CONFIGDATAList.First().ConfigVal.Split(new char[]{','});
                for (int i = 0; i < ItemGroup.Length ; i++)
                {
                    string[] temp=ItemGroup[i].Split(new char[] { ':' });
                    if (temp[0] == "1") 
                    {
                        comboBox1.SelectedIndex = i;
                        textBox1.Text = temp[1];
                        textBox2.Text = temp[2];
                        break;
                    }
                }
                
                
            }
        }

        private string Validate()
        {
            int val1 = 0;
            int val2= 0;
            if (int.TryParse(textBox1.Text.Trim(), out val1))
            {
                //if (val1 < 0 || val1 > 255)
                //{
                //    return null;
                //}
            }
            else { return null; }
            if (int.TryParse(textBox2.Text.Trim(), out val2))
            {
                if (val2 < 0 || val2 > 255)
                {
                    return null;
                }
            }
            else { return null; }
            return comboBox1.SelectedIndex+","+val2 + "," + val1;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "";
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;

            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("输入阈值或固态存储时段间隔有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            int gnm = 0x20;
            CommandCode = "20";
            commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }
           

            return commands;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Where = " where STCD='" + STCDS[0] + "' and ItemID='0000000000' and ConfigID='120000000020'";
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList(Where);
            if (CONFIGDATAList.Count > 0)
            {
                string[] ItemGroup = CONFIGDATAList.First().ConfigVal.Split(new char[] { ',' });
                string[] temp = ItemGroup[comboBox1.SelectedIndex].Split(new char[] { ':' });
                textBox1.Text = temp[1];
                textBox2.Text = temp[2];
                
            }
        }
    }
}
