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
    public partial class _B1 : UserControl, ICommandControl
    {
        public _B1()
        {
            InitializeComponent(); 
            this.Dock = DockStyle.Fill;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;

            if (rb5.Checked)
            {

                int gnm = 0xB0;
                CommandCode = "B0";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, "", int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }

            }
            else {
                int gnm = 0xB1;
                CommandCode = "B1";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, string.Format("{0},{1},{2}", comboBox1.SelectedIndex, dateTimePicker1.Value, dateTimePicker2.Value), int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            return commands;
        }

        private void _B1_Load(object sender, EventArgs e)
        {

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;
            foreach (var item in panelEx2.Controls)
            {
                (item as Control).Enabled = false;
            }


            string[] items = new string[] { "雨量", "水位", "流量（水量）", "流速", "闸位", "功率", "气压", "风速（风向）", "水温", "水质", "土壤含水率", "水压", "备用1", "备用2", "备用3" };
            foreach (var item in items)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void rb5_CheckedChanged(object sender, EventArgs e)
        {
            if (rb5.Checked)
            {
                foreach (var item in panelEx2.Controls)
                {
                    (item as Control).Enabled = false;
                }
            }
            else
            {
                foreach (var item in panelEx2.Controls)
                {
                    (item as Control).Enabled = true;
                }
            }
        }
    }
}
