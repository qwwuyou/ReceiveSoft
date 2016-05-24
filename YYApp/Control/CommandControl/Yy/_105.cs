using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service.Model;
using Service;

namespace YYApp.CommandControl
{
    public partial class _105 : UserControl, ICommandControl
    {
        public _105(string[] Stcds)
        {
            InitializeComponent(); 
            this.Dock = DockStyle.Fill;

            RtuList = PublicBD.db.GetRTUList("").ToList<YY_RTU_Basic>();
            string In = "";
            if (Stcds.Length > 0) 
            {
                for (int i = 0; i < Stcds.Length; i++)
                {
                    In += "'" + Stcds[i] + "',";
                }
                In = In.Substring(0, In.Length - 1);
            }
            CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList(" where STCD in (" + In + ") AND ConfigID like '1000________'").ToList<YY_RTU_CONFIGDATA>();
            Combox_Init(Stcds);
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YYPack.Pack pack = new YYPack.Pack();
            string[] commands = null;
            CommandCode = "0069";

            if (radioButton1.Checked)
            {

                string sjy = Validate();
                if (sjy == null)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("输入开始或结束时间有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }


                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    string Auto = Convert.ToInt32(GetAuto(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string CenterAddress = Convert.ToInt32(GetCenterAddress(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string RainfallOrder = Convert.ToInt32(GetRainfallOrder(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string WaterLevelOrder = Convert.ToInt32(GetWaterLevelOrder(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string PowerMode = Convert.ToInt32(GetPowerMode(Stcds[i]), 16).ToString().ToString().PadLeft(4, '0');
                    if (NFOINDEX.ToLower() != "短信") //短信
                    {
                        //打召测包
                        commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, CenterAddress, sjy, pack.GetBeginOrEndTime(dateTimePicker1.Value), pack.GetBeginOrEndTime(dateTimePicker2.Value));
                    }
                    else
                    {
                        if (radTX_A.Checked)
                            commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, CenterAddress, sjy, pack.GetBeginOrEndTime(dateTimePicker1.Value), pack.GetBeginOrEndTime(dateTimePicker2.Value), "gsm");
                        else
                            commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, CenterAddress, sjy, pack.GetBeginOrEndTime(dateTimePicker1.Value), pack.GetBeginOrEndTime(dateTimePicker2.Value), "gprs");
                    }
                }

                return commands;
            }
            else 
            {
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {

                    string Auto = Convert.ToInt32(GetAuto(Stcds[i]),16).ToString().PadLeft(4, '0'); ;
                    string CenterAddress = Convert.ToInt32(GetCenterAddress(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string RainfallOrder = Convert.ToInt32(GetRainfallOrder(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string WaterLevelOrder = Convert.ToInt32(GetWaterLevelOrder(Stcds[i]), 16).ToString().PadLeft(4, '0'); ;
                    string PowerMode = Convert.ToInt32(GetPowerMode(Stcds[i]), 16).ToString().ToString().PadLeft(4, '0');
                    if (NFOINDEX.ToLower() != "短信") //短信
                    {
                        //打召测包
                        commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, "0000", "FFFF", "FFFF", "FFFF");
                    }
                    else
                    {
                        if (radTX_C.Checked)
                            commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, "0000", "FFFF", "FFFF", "FFFF", "gsm");
                        else
                            commands[i] = pack.GetAnswer("0001", CommandCode, Auto, WaterLevelOrder, RainfallOrder, PowerMode, "0000", "FFFF", "FFFF", "FFFF", "gprs");
                    }
                }

                return commands;
            }
        }

        private void Combox_Init(string[] Stcds) 
        {
            string[] items = new string[] { "雨量", "水位1", "水位2", "墒情"};
            foreach (var item in items)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;

            foreach (var item in Stcds)
            {
                comboBox2.Items.Add(item);
            }
            comboBox2.SelectedIndex = 0;
        }

        private string Validate() 
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
                return null;

            if (comboBox1.SelectedIndex == 0)
                return "0001";
            else if (comboBox1.SelectedIndex == 1)
                return "0002";
            else if (comboBox1.SelectedIndex == 2)
                return "0004";
            else
                return "0040";
        }

        #region  从数据库里得到回复用的数据
        public List<YY_RTU_Basic> RtuList = null;
        public List<YY_RTU_CONFIGDATA> CONFIGDATAList = null;
        /// <summary>
        /// 得到设备自报间隔D7  (默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        private string GetAuto(string STCD)
        {
            var rtu=from r in CONFIGDATAList where r.STCD ==STCD && r.ConfigID =="100000000007"  select r;
            if ( rtu.Count() > 0)
            {
                return rtu.First().ConfigVal.ToString().PadLeft(4, '0');
            }

            return "0001";
        }

        /// <summary>
        /// 得到雨量量级D10（默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetRainfallOrder(string STCD)
        {

            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000010" select r;
            if (rtu.Count() > 0)
            {
                return rtu.First().ConfigVal.ToString().PadLeft(4, '0');
            }

            return "0001";
        }

        /// <summary>
        /// 得到水位量级D8（默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetWaterLevelOrder(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000008" select r;
            if (rtu.Count() > 0)
            {
                return rtu.First().ConfigVal.ToString().PadLeft(4, '0');
            }

            return "0001";
        }

        /// <summary>
        /// 得到电源供电模式D11（默认值0 =0000自动掉电  1=0001保持长供电）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetPowerMode(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000011" select r;
            if (rtu.Count() > 0)
            {
                return rtu.First().ConfigVal.ToString().PadLeft(4, '0');
            }

            return "0000";
        }

        /// <summary>
        ///得到能够远程提取历史数据的中心站号D13(默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        ///// <returns></returns>
        private string GetCenterAddress(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000013" select r;
            if (rtu.Count() > 0)
            {
                return rtu.First().ConfigVal.ToString().PadLeft(4, '0');
            }
            
            return "0001";

        }
        #endregion


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Point p = new Point(3,42);
                groupBox1.Location = p;

                p = new Point(-1000, 42);
                groupBox2.Location = p;
            }
            else 
            {
                Point p = new Point(3, 42);
                groupBox2.Location = p;

                p = new Point(-1000, 42);
                groupBox1.Location = p;
                
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string STCD=comboBox2.SelectedItem.ToString() ;
            var rtu = from r in RtuList where r.STCD ==STCD select r;
            if (rtu != null && rtu.Count() > 0)
                label1_name.Text = rtu.First().NiceName;

            label_auto.Text = Convert.ToInt32(GetAuto(STCD), 10).ToString();
            label1_CenterAddress.Text = Convert.ToInt32(GetCenterAddress(STCD), 10).ToString();
            label1_Rainfall.Text = Convert.ToInt32(GetRainfallOrder(STCD), 10).ToString();
            label1_WaterLevel.Text = Convert.ToInt32(GetWaterLevelOrder(STCD), 10).ToString();


            if (GetPowerMode(STCD) == "0000")
            { label1_PowerMode.Text = "自动掉电"; }
            else
            { label1_PowerMode.Text = "长供电"; }

        }

    }
}
