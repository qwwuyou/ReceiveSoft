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
    public partial class _40 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        List<Service.Model.YY_RTU_CONFIGDATA> list1 ;
        public _40(string[] Stcds)
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
            list1 = Service.PublicBD.db.GetRTU_CONFIGDATAList(" where STCD in (" + In + ") and ConfigID like '1100________'").ToList<Service.Model.YY_RTU_CONFIGDATA>();
            LV_Init();
            Mode_Init();
            STtype_Init();
            comboBox_0F_Init();
            DropDownList_Init(comboBox_COM_M1, true);
            DropDownList_Init(comboBox_COM_M2, true);
            DropDownList_Init(comboBox_COM_M3, true);
            DropDownList_Init(comboBox_COM_M4, true);
            DropDownList_Init(comboBox_COM_B1, false);
            DropDownList_Init(comboBox_COM_B2, false);
            DropDownList_Init(comboBox_COM_B3, false);
            DropDownList_Init(comboBox_COM_B4, false);

            CB_Stcd_Init(Stcds);
            
            if (Stcds.Length > 1) 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("只能设置单个测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = false;
            }
        }

        #region 初始化
        private void LV_Init()
        {
            string[] item = new string[] { "降水量", "蒸发量", "风向", "风速", "气温", "湿度", "地温", "气压", 
                "水位8", "水位7", "水位6", "水位5", "水位4", "水位3", "水位2", "水位1",
                "地下水埋深", "图片", "波浪", "闸门开度", "水量", "流速", "流量", "水压",
                "水表8",	"水表7",	"水表6",	"水表5",	"水表4",	"水表3",	"水表2",	"水表1",
                "100CM墒情",	"80CM墒情",	"60CM墒情",	"50CM墒情",	"40CM墒情",	"30CM墒情",	"20CM墒情",	"10CM墒情" ,
                "pH值",	"溶解氧",	"电导率",	"浊度",	"氧化还原电位",	"高锰酸盐指数",	"氨氮",	"水温",
                "总有机碳",	"总氮",	"总磷",	"锌",	"硒",	"砷",	"总汞",	"镉",
                "D7",	"D6",	"D5",	"D4",	"D3","叶绿素a",	"铜",	"铅"};
            for (int i = 0; i < item.Length ; i++)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.Text = item[i];
                listView_Item.Items.Add(lvItem); 
            }
            
        }

        private void DropDownList_Init(ComboBox cb, bool b)
        {
            cb.Items.Clear();
            //cb.Items.Add("TCP--GPRS");
            //cb.Items.Add("UDP--GPRS");
            //cb.Items.Add("GSM--短信");
            //cb.Items.Add("COM--卫星");
            cb.Items.Add("禁用");
            cb.Items.Add("短信");
            cb.Items.Add("IPV4");
            cb.Items.Add("北斗");
            cb.Items.Add("海事卫星");
            cb.Items.Add("PSTN");
            cb.Items.Add("超短波");
            cb.Items.Add("IPV4+短信/卫星");
            //if (b)
                cb.SelectedIndex = 0;
        }

        private void Mode_Init()
        {
            comboBox_Mode.Items.Clear();
            comboBox_Mode.Items.Add("自报工作状态");
            comboBox_Mode.Items.Add("自报确认工作状态");
            comboBox_Mode.Items.Add("查询/应答工作状态");
            comboBox_Mode.Items.Add("调试或维修状态");
            comboBox_Mode.SelectedIndex = 1;
        }

        private void STtype_Init() 
        {
            comboBox_STtype.Items.Clear();
            List<Item> types = new List<Item>();
            types.Add(new Item(){Key="降水",Value ="50"});
            types.Add(new Item(){Key="河道",Value = "48"});
            types.Add(new Item(){Key="水库(湖泊)", Value ="4B"});
            types.Add(new Item(){Key="闸坝", Value ="5A"});
            types.Add(new Item(){Key="泵站", Value ="44"});
            types.Add(new Item(){Key="潮汐", Value ="54"});
            types.Add(new Item(){Key="墒情", Value ="4D"});
            types.Add(new Item(){Key="地下水",Value = "47"});
            types.Add(new Item(){Key="水质", Value ="51"});
            types.Add(new Item(){Key="取水口", Value ="49"});
            types.Add(new Item(){Key="排水口", Value ="4F"});
            comboBox_STtype.DataSource =types;
            comboBox_STtype.DisplayMember = "Key";//绑定泛型中类的属性
            comboBox_STtype.ValueMember = "Value"; 
        }

        private void comboBox_0F_Init()
        {
            comboBox_0F.Items.Add("移动通信卡");
            comboBox_0F.Items.Add("卫星通信卡");
            comboBox_0F.SelectedIndex = 0;
        }

        private void CB_Stcd_Init(string[] Stcds) 
        {
            List<Item> stcds = new List<Item>();
            for (int i = 0; i < Stcds.Length ; i++)
            {
                var rtu =from r in list where r.STCD ==Stcds[i] select r;
                if(rtu .Count ()>0)
                {
                    stcds.Add(new Item() { Key=rtu.First().STCD ,Value =rtu.First().NiceName });
                }
            }
            comboBox_stcd.DataSource = stcds;
            comboBox_stcd.DisplayMember = "Value";
            comboBox_stcd.ValueMember = "Key";

            comboBox_stcd.SelectedIndex = 0;
        }

        #region 查询初始化
        private void Tab1_Init(string Stcd)
        {
            Tab1_Clear();
            var rtu = from r in list where r.STCD == Stcd select r;
            if (rtu.Count() > 0)
            {
                textBox_PWD.Text = rtu.First().PassWord;
            }

            var config = from c in list1 where c.STCD ==Stcd && c.ConfigID == "11000000000C" select c;
            if (config.Count() > 0) 
            {
                comboBox_Mode.SelectedIndex = int.Parse(config.First().ConfigVal) - 1; 
            }

            config = from c in list1 where c.STCD == Stcd &&  c.ConfigID == "11000000AA01" select c;
            if (config.Count() > 0) 
            {
                comboBox_STtype.SelectedValue  = config.First().ConfigVal; 
            }

            config = from c in list1 where c.STCD == Stcd && c.ConfigID == "11000000000E" select c;
            if (config.Count() > 0)
            {
                textBox_Range.Text  = config.First().ConfigVal;
            }

            config = from c in list1 where c.STCD == Stcd && c.ConfigID == "11000000000F" select c;
            if (config.Count() > 0)
            {
                string Val = config.First().ConfigVal;
                string[] temps = Val.Split(new string[] { "," }, StringSplitOptions.None);
                if (temps.Count() > 1)
                {
                    int type = 0;
                    if (int.TryParse(temps[0], out type)) 
                    {
                        comboBox_0F.SelectedIndex = type - 1;
                        textBox_0F.Text = temps[1].Trim();
                    }
                }
            }
        }

        private void Tab1_Clear() 
        {
            textBox_PWD.Text = "1234";
            comboBox_Mode.SelectedIndex = 1;
            comboBox_STtype.SelectedIndex = 0;
            textBox_Range .Text = "";
            comboBox_0F.SelectedIndex = 0;
            textBox_0F.Text = "";
        }

        private void Tab2_Init(string Stcd)
        {
            IList<Service.Model.YY_RTU_WRES> RTU_WRESList = Service.PublicBD.db.GetRTU_WRESList("where STCD='" + Stcd + "'");
            Tab2_Clear();
            foreach (var item in RTU_WRESList)
            {
                if (item.CODE == 1)
                {
                    textBox_ADR_ZX1.Text = item.ADR_ZX.ToString();
                    if (item.COM_M.HasValue == true)
                    {
                        comboBox_COM_M1.SelectedIndex = item.COM_M.Value ;
                    }
                    textBox_ADR_M1.Text = item.ADR_M;
                    if (item.PORT_M.HasValue == true)
                    {
                        textBox_PORT_M1.Text = item.PORT_M.Value.ToString();
                    }
                    if (item.COM_B.HasValue == true)
                    {
                        comboBox_COM_B1.SelectedIndex = item.COM_B.Value ;
                    }
                    else
                    { comboBox_COM_B1.SelectedIndex = 0; }
                    textBox_ADR_B1.Text = item.ADR_B;
                    if (item.PORT_B.HasValue == true)
                    {
                        textBox_PORT_B1.Text = item.PORT_B.Value.ToString();
                    }

                    textBox_Phone1.Text = item.PhoneNum;
                    textBox_Satellite1.Text = item.SatelliteNum;

                }
                else if (item.CODE == 2)
                {
                    textBox_ADR_ZX2.Text = item.ADR_ZX.ToString();
                    if (item.COM_M.HasValue == true)
                    {
                        comboBox_COM_M2.SelectedIndex = item.COM_M.Value ;
                    }
                    textBox_ADR_M2.Text = item.ADR_M;
                    if (item.PORT_M.HasValue == true)
                    {
                        textBox_PORT_M2.Text = item.PORT_M.Value.ToString();
                    }
                    if (item.COM_B.HasValue == true)
                    {
                        comboBox_COM_B2.SelectedIndex = item.COM_B.Value ;
                    }
                    else
                    { comboBox_COM_B2.SelectedIndex = 0; }
                    textBox_ADR_B2.Text = item.ADR_B;

                    if (item.PORT_B.HasValue == true)
                    {
                        textBox_PORT_B2.Text = item.PORT_B.Value.ToString();
                    }
                    textBox_Phone2.Text = item.PhoneNum;
                    textBox_Satellite2.Text = item.SatelliteNum;
                }
                else if (item.CODE == 3)
                {
                    textBox_ADR_ZX3.Text = item.ADR_ZX.ToString();
                    if (item.COM_M.HasValue == true)
                    {
                        comboBox_COM_M3.SelectedIndex = item.COM_M.Value ;
                    }
                    textBox_ADR_M3.Text = item.ADR_M;
                    if (item.PORT_M.HasValue == true)
                    {
                        textBox_PORT_M3.Text = item.PORT_M.Value.ToString();
                    }
                    if (item.COM_B.HasValue == true)
                    {
                        comboBox_COM_B3.SelectedIndex = item.COM_B.Value;
                    }
                    else
                    { comboBox_COM_B3.SelectedIndex = 0; }
                    textBox_ADR_B3.Text = item.ADR_B;

                    if (item.PORT_B.HasValue == true)
                    {
                        tabControl2.Text = item.PORT_B.Value.ToString();
                    }
                    textBox_Phone3.Text = item.PhoneNum;
                    textBox_Satellite3.Text = item.SatelliteNum;
                }
                else if (item.CODE == 4)
                {
                    textBox_ADR_ZX4.Text = item.ADR_ZX.ToString();
                    if (item.COM_M.HasValue == true)
                    {
                        comboBox_COM_M3.SelectedIndex = item.COM_M.Value ;
                    }
                    textBox_ADR_M4.Text = item.ADR_M;
                    if (item.PORT_M.HasValue == true)
                    {
                        textBox_PORT_M4.Text = item.PORT_M.Value.ToString();
                    }
                    if (item.COM_B.HasValue == true)
                    {
                        comboBox_COM_B4.SelectedIndex = item.COM_B.Value ;
                    }
                    else
                    { comboBox_COM_B4.SelectedIndex = 0; }
                    textBox_ADR_B4.Text = item.ADR_B;
                    if (item.PORT_B.HasValue == true)
                    {
                        textBox_PORT_B4.Text = item.PORT_B.Value.ToString();
                    }
                    textBox_Phone4.Text = item.PhoneNum;
                    textBox_Satellite4.Text = item.SatelliteNum;
                }
            }
        }

        private void Tab2_Clear() 
        {
            Clear1();
            Clear2();
            Clear3();
            Clear4();
        }

        private void Tab3_Init(string Stcd)
        {
            Tab3_Clear();
            var config = from c in list1 where c.STCD == Stcd && c.ConfigID == "11000000000D" select c;
            if (config.Count() > 0)
            {
                string val=config.First().ConfigVal;
                for (int i = 0; i < val.Length ; i++)
                {
                    if (val[i] == '0')
                    { listView_Item.Items[i].Checked = false; }
                    else
                    { listView_Item.Items[i].Checked = true; }
                }
            }
        }

        private void Tab3_Clear() 
        {
            for (int i = 0; i < listView_Item.Items.Count ; i++)
            {
                listView_Item.Items[i].Checked = false; 
            }
        }
        #endregion
        #endregion

        #region Tab2 Clear
        private void Clear1()
        {
            foreach (var item in tabPage4.Controls )
            {
                if (item is TextBox) 
                {
                    (item as TextBox).Text = ""; 
                }
            }

            DropDownList_Init(comboBox_COM_M1, true);
            DropDownList_Init(comboBox_COM_B1, false);
        }
        private void Clear2()
        {
            foreach (var item in tabPage5.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
            }

            DropDownList_Init(comboBox_COM_M2, true);
            DropDownList_Init(comboBox_COM_B2, false);
        }
        private void Clear3()
        {
            foreach (var item in tabPage6.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
            }

            DropDownList_Init(comboBox_COM_M3, true);
            DropDownList_Init(comboBox_COM_B3, false);
        }
        private void Clear4()
        {
            foreach (var item in tabPage7.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
            }

            DropDownList_Init(comboBox_COM_M4, true);
            DropDownList_Init(comboBox_COM_B4, false);
        }
        #endregion

        string[] ICommandControl.GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "40";

            if (Stcds.Length > 1)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择单个测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            string[] commands = new string[1];
            if (tabControl1.SelectedTab == tabPage1) 
            {
                Dictionary<string, string> Dic = new Dictionary<string, string>();

                string STCD = comboBox_stcd.SelectedValue.ToString();
                
                var model = from rtu in list where rtu.STCD == STCD select rtu;
                if (model.Count() > 0)
                {
                    string msg=ValidateTab1(out Dic);
                    if (msg != "")
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    else
                    {
                        Package package = Package.Create_0x40Package(STCD, 1, UInt16.Parse(model.First().PassWord), Dic);
                        byte[] vBuffer = package.GetFrames()[1].ToBytes();
                        string s = ByteHelper.ByteToHexStr(vBuffer);

                        commands[0] = s;                    
                    }
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                Dictionary<string, string> Dic = new Dictionary<string, string>();

                string STCD = comboBox_stcd.SelectedValue.ToString();
                var model = from rtu in list where rtu.STCD == STCD select rtu;
                if (model.Count() > 0)
                {
                    string msg = ValidateTab2(out Dic);
                    if (msg != "")
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    else
                    {
                        Package package = Package.Create_0x40Package(STCD, 1, UInt16.Parse(model.First().PassWord), Dic);
                        commands[0] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                    }
                }
            }
            else 
            {
                Dictionary<string, string> Dic = new Dictionary<string, string>();

                string STCD = comboBox_stcd.SelectedValue.ToString();
                var model = from rtu in list where rtu.STCD == STCD select rtu;
                if (model.Count() > 0)
                {
                    string msg = ValidateTab3(out Dic);
                    if (msg != "")
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                    else
                    {
                        Package package = Package.Create_0x40Package(STCD, 1, UInt16.Parse(model.First().PassWord), Dic);
                        commands[0] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());
                    }
                }
            }
            return commands;
        }

        #region 验证
        private string ValidateTab1(out  Dictionary<string, string> DIC)
        {
            string msg = "";
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            if (CB1.Checked)
            {
                if (textBox_PWD.Text.Trim() != "")
                {
                    Dic.Add("03", textBox_PWD.Text.Trim());
                }
                else 
                {
                    msg += "密码不能为空！\n";
                }
            }
            if (CB2.Checked) 
            {
                Dic.Add("0C", (comboBox_Mode.SelectedIndex +1).ToString());
            }
            if (CB3.Checked)
            {
                if (textBox_Range.Text.Trim() != "")
                {
                    Dic.Add("0E", textBox_Range.Text.Trim());
                }
                else 
                {
                    msg += "中继站（转发）服务地址范围不能为空！\n";
                }
            }
            if (CB4.Checked) 
            {
                if (textBox_0F.Text.Trim() != "")
                {
                    Dic.Add("0F", (comboBox_0F.SelectedIndex + 1).ToString() + "," + textBox_0F.Text.Trim());
                }
                else 
                {
                    msg += "通讯设备识别号不能为空！\n";
                }
            }
            if (!CB1.Checked && !CB2.Checked && !CB3.Checked && !CB4.Checked) 
            {
                msg += "至少选择一个设置项！";
            }
            DIC = Dic;
            return msg;
        }
        private string ValidateTab2(out  Dictionary<string, string> DIC)
        {
            string msg = "";
            Dictionary<string, string> Dic = new Dictionary<string, string>();

            #region 中心站地址1次下发4个
            int adr1 = 0, adr2 = 0, adr3 = 0, adr4 = 0;

            if (textBox_ADR_ZX1.Text.Trim() != "")
            {
                if (!int.TryParse(textBox_ADR_ZX1.Text.Trim(), out adr1))
                {
                    msg += "[中心站1]中心站地址输入有误!" + "\n";
                }
                else
                {
                    if (adr1 < 1 || adr1 > 255)
                    {
                        msg += "[中心站1]中心站地址输入有误!" + "\n";
                    }
                }
            }

            if (textBox_ADR_ZX2.Text.Trim() != "")
            {
                if (!int.TryParse(textBox_ADR_ZX2.Text.Trim(), out adr2))
                {
                    msg += "[中心站2]中心站地址输入有误!" + "\n";
                }
                else
                {
                    if (adr2 < 1 || adr2 > 255)
                    {
                        msg += "[中心站2]中心站地址输入有误!" + "\n";
                    }
                }
            }

            if (textBox_ADR_ZX3.Text.Trim() != "")
            {
                if (!int.TryParse(textBox_ADR_ZX3.Text.Trim(), out adr3))
                {
                    msg += "[中心站3]中心站地址输入有误!" + "\n";
                }
                else
                {
                    if (adr3 < 1 || adr3 > 255)
                    {
                        msg += "[中心站3]中心站地址输入有误!" + "\n";
                    }
                }
            }

            if (textBox_ADR_ZX4.Text.Trim() != "")
            {
                if (!int.TryParse(textBox_ADR_ZX4.Text.Trim(), out adr4))
                {
                    msg += "[中心站4]中心站地址输入有误!" + "\n";
                }
                else
                {
                    if (adr4 < 1 || adr4 > 255)
                    {
                        msg += "[中心站4]中心站地址输入有误!" + "\n";
                    }
                }
            }
            if (msg == "") 
            {
                Dic.Add("01", adr1 + "," + adr2 + "," + adr3 + "," + adr4);
            }
            #endregion
            
            //cb.Items.Add("禁用");
            //cb.Items.Add("短信");
            //cb.Items.Add("IPV4");
            //cb.Items.Add("北斗");
            //cb.Items.Add("海事卫星");
            //cb.Items.Add("PSTN");
            //cb.Items.Add("超短波");
            
            if (checkBox_Sed1.Checked) 
            {
                #region 中心站1主信道
                string IPv4_Phone_Satellite = "";
                if (comboBox_COM_M1.SelectedIndex == 0) 
                {
                    Dic.Add("04", "0");
                }
                else if (comboBox_COM_M1.SelectedIndex == 1) 
                {
                    if (!IsPhone(textBox_Phone1.Text.Trim())) 
                    {
                        msg += "[中心站1]主信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("04","1,"+ textBox_Phone1.Text.Trim());
                    }
                }
                else if (comboBox_COM_M1.SelectedIndex == 2 || comboBox_COM_M1.SelectedIndex == 7)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_M1.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站1]主信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_M1.Text.Trim(), out port))
                    {
                        msg += "[中心站1]主信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站1]主信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        if ((comboBox_COM_M1.SelectedIndex == 2))
                        { Dic.Add("04", "2," + address.ToString() + "," + port); }
                        if (comboBox_COM_M1.SelectedIndex == 7)
                        {
                            IPv4_Phone_Satellite = "7," + address.ToString() + "," + port;
                        }
                    }
                }
                else if (comboBox_COM_M1.SelectedIndex == 3 || comboBox_COM_M1.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite1.Text.Trim(), out port))
                    {
                        msg += "[中心站1]主信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("04", comboBox_COM_M1.SelectedIndex + "," + textBox_Satellite1.Text.Trim());
                    }
                }
                else 
                {
                    Dic.Add("04", comboBox_COM_M1.SelectedIndex .ToString());
                }

                #region IPV4+短信/卫星  ---黑龙江
                if (comboBox_COM_M1.SelectedIndex == 7)
                {
                    if (textBox_Phone1.Text.Trim() == "" && textBox_Satellite1.Text.Trim() == "")
                    {
                        msg += "手机号码或卫星号码至少输入一个!" + "\n";
                    }

                    if (textBox_Phone1.Text.Trim() != "")
                    {
                        if (!IsPhone(textBox_Phone1.Text.Trim())) //---------------
                        {
                            msg += "手机号输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Phone1.Text.Trim()); //----------
                        }
                    }

                    if (textBox_Satellite1.Text.Trim() != "")
                    {
                        int port = 0;
                        if (!int.TryParse(textBox_Satellite1.Text.Trim(), out port))//---------------
                        {
                            msg += "卫星号码输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Satellite1.Text.Trim()); //----------
                        }
                    }
                    Dic.Add("04", IPv4_Phone_Satellite);
                }
                #endregion
                #endregion

                #region 中心站1备用信道
                if (comboBox_COM_B1.SelectedIndex == 0)
                {
                    Dic.Add("05", "0");
                }
                else if (comboBox_COM_B1.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone1.Text.Trim()))
                    {
                        msg += "[中心站1]备用信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("05", "1," + textBox_Phone1.Text.Trim());
                    }
                }
                else if (comboBox_COM_B1.SelectedIndex == 2)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_B1.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站1]备用信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_B1.Text.Trim(), out port))
                    {
                        msg += "[中心站1]备用信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站1]备用信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        Dic.Add("05", "2," + address.ToString() + "," + port);
                    }
                }
                else if (comboBox_COM_B1.SelectedIndex == 3 || comboBox_COM_B1.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite1.Text.Trim(), out port))
                    {
                        msg += "[中心站1]备用信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("05", comboBox_COM_B1.SelectedIndex + "," + textBox_Satellite1.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("05", comboBox_COM_B1.SelectedIndex.ToString());
                }
                #endregion
            }
            if (checkBox_Sed2.Checked) 
            {
                #region 中心站2主信道
                string IPv4_Phone_Satellite = "";
                if (comboBox_COM_M2.SelectedIndex == 0) 
                {
                    Dic.Add("06", "0");
                }
                else if (comboBox_COM_M2.SelectedIndex == 1) 
                {
                    if (!IsPhone(textBox_Phone2.Text.Trim())) 
                    {
                        msg += "[中心站2]主信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("06","1,"+ textBox_Phone2.Text.Trim());
                    }
                }
                else if (comboBox_COM_M2.SelectedIndex == 2 || comboBox_COM_M2.SelectedIndex == 7)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_M2.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站2]主信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_M2.Text.Trim(), out port))
                    {
                        msg += "[中心站2]主信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站2]主信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        if ((comboBox_COM_M2.SelectedIndex == 2))
                        {
                            Dic.Add("06", "2," + address.ToString() + "," + port);
                        }
                        if (comboBox_COM_M2.SelectedIndex == 7)
                        {
                            IPv4_Phone_Satellite = "7," + address.ToString() + "," + port;
                        }
                        
                    }
                }
                else if (comboBox_COM_M2.SelectedIndex == 3 || comboBox_COM_M2.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite2.Text.Trim(), out port))
                    {
                        msg += "[中心站2]主信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("06", comboBox_COM_M2.SelectedIndex + "," + textBox_Satellite2.Text.Trim());
                    }
                }
                else 
                {
                    Dic.Add("06", comboBox_COM_M2.SelectedIndex .ToString());
                }


                #region IPV4+短信/卫星  ---黑龙江
                if (comboBox_COM_M2.SelectedIndex == 7)
                {
                    if (textBox_Phone2.Text.Trim() == "" && textBox_Satellite2.Text.Trim() == "")
                    {
                        msg += "手机号码或卫星号码至少输入一个!" + "\n";
                    }
                    if (textBox_Phone2.Text.Trim() != "")
                    {
                        if (!IsPhone(textBox_Phone2.Text.Trim())) //---------------
                        {
                            msg += "手机号输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Phone2.Text.Trim()); //----------
                        }
                    }

                    if (textBox_Satellite2.Text.Trim() != "")
                    {
                        int port = 0;
                        if (!int.TryParse(textBox_Satellite2.Text.Trim(), out port))//---------------
                        {
                            msg += "卫星号码输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Satellite2.Text.Trim()); //----------
                        }
                    }
                    Dic.Add("06", IPv4_Phone_Satellite);
                }
                #endregion
                #endregion

                #region 中心站2备用信道
                if (comboBox_COM_B2.SelectedIndex == 0)
                {
                    Dic.Add("07", "0");
                }
                else if (comboBox_COM_B2.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone2.Text.Trim()))
                    {
                        msg += "[中心站2]备用信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("07", "1," + textBox_Phone2.Text.Trim());
                    }
                }
                else if (comboBox_COM_B2.SelectedIndex == 2)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_B2.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站2]备用信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_B2.Text.Trim(), out port))
                    {
                        msg += "[中心站2]备用信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站2]备用信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        Dic.Add("07", "2," + address.ToString() + "," + port);
                    }
                }
                else if (comboBox_COM_B2.SelectedIndex == 3 || comboBox_COM_B2.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite2.Text.Trim(), out port))
                    {
                        msg += "[中心站2]备用信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("07", comboBox_COM_B2.SelectedIndex + "," + textBox_Satellite2.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("07", comboBox_COM_B2.SelectedIndex.ToString());
                }
                #endregion
            }
            if (checkBox_Sed3.Checked)
            {
                #region 中心站3主信道
                string IPv4_Phone_Satellite = "";
                if (comboBox_COM_M3.SelectedIndex == 0)
                {
                    Dic.Add("08", "0");
                }
                else if (comboBox_COM_M3.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone3.Text.Trim()))
                    {
                        msg += "[中心站3]主信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("08", "1," + textBox_Phone3.Text.Trim());
                    }
                }
                else if (comboBox_COM_M3.SelectedIndex == 2 || comboBox_COM_M3.SelectedIndex == 7)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_M3.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站3]主信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_M3.Text.Trim(), out port))
                    {
                        msg += "[中心站3]主信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站3]主信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        if ((comboBox_COM_M3.SelectedIndex == 2))
                        {
                            Dic.Add("08", "2," + address.ToString() + "," + port);
                        }
                        if (comboBox_COM_M3.SelectedIndex == 7)
                        {
                            IPv4_Phone_Satellite = "7," + address.ToString() + "," + port;
                        }
                    }
                }
                else if (comboBox_COM_M3.SelectedIndex == 3 || comboBox_COM_M3.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite3.Text.Trim(), out port))
                    {
                        msg += "[中心站3]主信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("08", comboBox_COM_M3.SelectedIndex + "," + textBox_Satellite3.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("08", comboBox_COM_M3.SelectedIndex.ToString());
                }

                #region IPV4+短信/卫星  ---黑龙江
                if (comboBox_COM_M3.SelectedIndex == 7)
                {
                    if (textBox_Phone3.Text.Trim() == "" && textBox_Satellite3.Text.Trim() == "")
                    {
                        msg += "手机号码或卫星号码至少输入一个!" + "\n";
                    }

                    if (textBox_Phone3.Text.Trim() != "")
                    {
                        if (!IsPhone(textBox_Phone3.Text.Trim())) //---------------
                        {
                            msg += "手机号输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Phone3.Text.Trim()); //----------
                        }
                    }

                    if (textBox_Satellite3.Text.Trim() != "")
                    {
                        int port = 0;
                        if (!int.TryParse(textBox_Satellite3.Text.Trim(), out port))//---------------
                        {
                            msg += "卫星号码输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Satellite3.Text.Trim()); //----------
                        }
                    }
                    Dic.Add("08", IPv4_Phone_Satellite);
                }
                #endregion
                #endregion

                #region 中心站3备用信道
                if (comboBox_COM_B3.SelectedIndex == 0)
                {
                    Dic.Add("09", "0");
                }
                else if (comboBox_COM_B3.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone3.Text.Trim()))
                    {
                        msg += "[中心站3]备用信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("09", "1," + textBox_Phone3.Text.Trim());
                    }
                }
                else if (comboBox_COM_B3.SelectedIndex == 2)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_B3.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站3]备用信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_B3.Text.Trim(), out port))
                    {
                        msg += "[中心站3]备用信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站3]备用信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        Dic.Add("09", "2," + address.ToString() + "," + port);
                    }
                }
                else if (comboBox_COM_B3.SelectedIndex == 3 || comboBox_COM_B3.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite3.Text.Trim(), out port))
                    {
                        msg += "[中心站3]备用信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("09", comboBox_COM_B3.SelectedIndex + "," + textBox_Satellite3.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("09", comboBox_COM_B3.SelectedIndex.ToString());
                }
                #endregion
            }
            if (checkBox_Sed4.Checked)
            {
                #region 中心站4主信道
                string IPv4_Phone_Satellite = "";
                if (comboBox_COM_M4.SelectedIndex == 0)
                {
                    Dic.Add("0A", "0");
                }
                else if (comboBox_COM_M4.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone4.Text.Trim()))
                    {
                        msg += "[中心站4]主信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("0A", "1," + textBox_Phone4.Text.Trim());
                    }
                }
                else if (comboBox_COM_M4.SelectedIndex == 2 || comboBox_COM_M4.SelectedIndex == 7)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_M4.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站4]主信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_M4.Text.Trim(), out port))
                    {
                        msg += "[中心站4]主信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站4]主信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        if ((comboBox_COM_M4.SelectedIndex == 2))
                        {
                            Dic.Add("0A", "2," + address.ToString() + "," + port);
                        }
                        if (comboBox_COM_M4.SelectedIndex == 7)
                        {
                            IPv4_Phone_Satellite = "7," + address.ToString() + "," + port;
                        }
                        
                        
                    }
                }
                else if (comboBox_COM_M4.SelectedIndex == 3 || comboBox_COM_M4.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite4.Text.Trim(), out port))
                    {
                        msg += "[中心站4]主信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("0A", comboBox_COM_M4.SelectedIndex + "," + textBox_Satellite4.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("0A", comboBox_COM_M4.SelectedIndex.ToString());
                }

                #region IPV4+短信/卫星  ---黑龙江
                if (comboBox_COM_M4.SelectedIndex == 7)
                {
                    if (textBox_Phone4.Text.Trim() == "" && textBox_Satellite4.Text.Trim() == "")
                    {
                        msg += "手机号码或卫星号码至少输入一个!" + "\n";
                    }

                    if (textBox_Phone4.Text.Trim() != "")
                    {
                        if (!IsPhone(textBox_Phone4.Text.Trim())) //---------------
                        {
                            msg += "手机号输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Phone4.Text.Trim()); //----------
                        }
                    }

                    if (textBox_Satellite4.Text.Trim() != "")
                    {
                        int port = 0;
                        if (!int.TryParse(textBox_Satellite4.Text.Trim(), out port))//---------------
                        {
                            msg += "卫星号码输入有误!" + "\n";
                        }
                        else
                        {
                            IPv4_Phone_Satellite += ("," + textBox_Satellite4.Text.Trim()); //----------
                        }
                    }
                    Dic.Add("0A", IPv4_Phone_Satellite);
                }
                #endregion
                #endregion

                #region 中心站4备用信道
                if (comboBox_COM_B4.SelectedIndex == 0)
                {
                    Dic.Add("0B", "0");
                }
                else if (comboBox_COM_B4.SelectedIndex == 1)
                {
                    if (!IsPhone(textBox_Phone4.Text.Trim()))
                    {
                        msg += "[中心站4]备用信道手机号输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("0B", "1," + textBox_Phone4.Text.Trim());
                    }
                }
                else if (comboBox_COM_B4.SelectedIndex == 2)
                {
                    bool B = false, b = false;
                    System.Net.IPAddress address;
                    if (!System.Net.IPAddress.TryParse(textBox_ADR_B4.Text.Trim(), out address))//-----------
                    {
                        //失败
                        msg += "[中心站4]备用信道地址输入有误!" + "\n";
                    }
                    else
                    {
                        B = true;
                    }


                    int port = 0;
                    if (!int.TryParse(textBox_PORT_B4.Text.Trim(), out port))
                    {
                        msg += "[中心站4]备用信道端口输入有误!" + "\n";
                    }
                    else
                    {
                        if (port < 0 || port > 65535)
                        { msg += "[中心站4]备用信道端口输入有误!" + "\n"; }
                        else
                        {
                            b = true;
                        }
                    }
                    if (b && B)
                    {
                        Dic.Add("0B", "2," + address.ToString() + "," + port);
                    }
                }
                else if (comboBox_COM_B4.SelectedIndex == 3 || comboBox_COM_B4.SelectedIndex == 4)
                {
                    int port = 0;
                    if (!int.TryParse(textBox_Satellite4.Text.Trim(), out port))
                    {
                        msg += "[中心站4]备用信道卫星号码输入有误!" + "\n";
                    }
                    else
                    {
                        Dic.Add("0B", comboBox_COM_B4.SelectedIndex + "," + textBox_Satellite4.Text.Trim());
                    }
                }
                else
                {
                    Dic.Add("0B", comboBox_COM_B4.SelectedIndex.ToString());
                }
                #endregion
            }
            if (!checkBox_Sed1.Checked && !checkBox_Sed2.Checked && !checkBox_Sed3.Checked && !checkBox_Sed4.Checked) 
            {
                msg += "至少选择一个设置项！";
            }
            DIC = Dic;
            return msg;
        }
        private string ValidateTab3(out  Dictionary<string, string> DIC)
        {
            string msg = "";
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            string val = "";
            foreach (ListViewItem item in listView_Item.Items  )
            {
                if (item.Checked)
                {
                    val += "1";
                }
                else
                {
                    val += "0";
                }
            }
            Dic.Add("0D", val);
            if (val.IndexOf('1') == -1) 
            {
                msg= "至少选择一个设置项！";
            }
            DIC=Dic;
            return msg; 
        }
        private bool IsPhone(string phone)
        {
            if (phone == "" || phone == null)
            {
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"\d{11}"))
            {
                return false;
            }
            return true;
        }
        #endregion

        private void comboBox_stcd_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stcd = (comboBox_stcd.SelectedItem as Item).Key;
            Tab1_Init(stcd);
            Tab2_Init(stcd);
            Tab3_Init(stcd);
        }
    }
}
