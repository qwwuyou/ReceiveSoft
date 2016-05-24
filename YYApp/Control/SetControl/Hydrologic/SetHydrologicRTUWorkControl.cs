using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;

namespace YYApp.SetControl
{

    ////协议（2） 是否监测项（2）  内部编码（4，如果为0，未归类） 协议编码（4） 例：111100000025

    public partial class SetHydrologicRTUWorkControl : UserControl
    {
        public SetHydrologicRTUWorkControl()
        {
            InitializeComponent();
        }

        public List<Service.Model.YY_RTU_TIME> TIMEList = null;

        #region 初始化
        class Item
        {
            private string _key;
            private string _value;
            public string Key
            {
                get { return _key; }
                set { _key = value; }
            }
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }
        private void STtype_Init()
        {
            comboBox_STtype.Items.Clear();
            List<Item> types = new List<Item>();
            types.Add(new Item() { Key = "降水", Value = "50" });
            types.Add(new Item() { Key = "河道", Value = "48" });
            types.Add(new Item() { Key = "水库(湖泊)", Value = "4B" });
            types.Add(new Item() { Key = "闸坝", Value = "5A" });
            types.Add(new Item() { Key = "泵站", Value = "44" });
            types.Add(new Item() { Key = "潮汐", Value = "54" });
            types.Add(new Item() { Key = "墒情", Value = "4D" });
            types.Add(new Item() { Key = "地下水", Value = "47" });
            types.Add(new Item() { Key = "水质", Value = "51" });
            types.Add(new Item() { Key = "取水口", Value = "49" });
            types.Add(new Item() { Key = "排水口", Value = "4F" });
            comboBox_STtype.DataSource = types;
            comboBox_STtype.DisplayMember = "Key";//绑定泛型中类的属性
            comboBox_STtype.ValueMember = "Value";
        }

        private void comboBox_STCD_Init()
        {
            string Where = "where NiceName like '%" + comboBox_STCD.Text + "%'";
            IList<Service.Model.YY_RTU_Basic> ItemList = PublicBD.db.GetRTUList(Where);
            comboBox_STCD.DataSource = ItemList;
            comboBox_STCD.DisplayMember = "NiceName";
            comboBox_STCD.ValueMember = "STCD";

            if (comboBox_STCD.Items.Count > 0)
            {
                comboBox_STCD.SelectedIndex = 0;
                comboBox_STCD_SelectedIndexChanged(comboBox_STCD, new EventArgs());
                this.comboBox_STCD.SelectedIndexChanged += new System.EventHandler(this.comboBox_STCD_SelectedIndexChanged);
            }
        }

        private void comboBox_MODE_Init() 
        {
            comboBox_MODE.Items.Clear();
            comboBox_MODE.Items.Add("自报工作状态(01)");
            comboBox_MODE.Items.Add("自报确认工作状态(02)");
            comboBox_MODE.Items.Add("查询/应答工作状态(03)");
            comboBox_MODE.Items.Add("调试或维修状态(04)");
            comboBox_MODE.SelectedIndex = 0;
        }

        private void comboBox_20_Init()
        {
            comboBox_20.Items.Clear();
            comboBox_20.Items.Add("1");
            comboBox_20.Items.Add("2");
            comboBox_20.Items.Add("3");
            comboBox_20.Items.Add("4"); 
            comboBox_20.Items.Add("6");
            comboBox_20.Items.Add("8");
            comboBox_20.Items.Add("12");
            comboBox_20.Items.Add("24");
            comboBox_20.SelectedIndex = 0;
            //1,2,3,4,6,8,12,24
        }
        private void comboBox_22_Init()
        {
            comboBox_22.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                comboBox_22.Items.Add(i);
            }
            comboBox_22.SelectedIndex = 0;
        }
        private void comboBox_25_Init()
        {
            comboBox_25.Items.Clear();
            comboBox_25.Items.Add("0");
            comboBox_25.Items.Add("1");
            comboBox_25.Items.Add("0.5");
            comboBox_25.Items.Add("0.2");
            comboBox_25.Items.Add("0.1"); 
            comboBox_25.SelectedIndex = 0;

        }
        private void comboBox_26_Init()
        {
            comboBox_26.Items.Clear();
            comboBox_26.Items.Add("0");
            comboBox_26.Items.Add("1");
            comboBox_26.Items.Add("0.5");
            comboBox_26.Items.Add("0.1");
            comboBox_26.SelectedIndex = 0;
        }
        private void comboBox_0F_Init()
        {
            comboBox_0F.Items.Add("移动通信卡");
            comboBox_0F.Items.Add("卫星通信卡");
            comboBox_0F.SelectedIndex = 0;
        }

        private void TabPage2_Init(string STCD, IList<Service.Model.YY_RTU_CONFIGDATA> list) 
        {
            if (list.Count() > 0) 
            {
                var AA02 = from l in list where l.ConfigID == "11000000AA02" select l;
                if (AA02.Count() > 0)
                {
                    string temp = AA02.First().ConfigVal;
                    if (temp.Substring(0, 1) == "0")
                    { BR1.Checked = true; }
                    else
                    { BR2.Checked = true; }

                    if (temp.Substring(1, 1) == "0")
                    { BR3.Checked = true; }
                    else
                    { BR4.Checked = true; }

                    if (temp.Substring(2, 1) == "0")
                    { BR5.Checked = true; }
                    else
                    { BR6.Checked = true; }

                    if (temp.Substring(3, 1) == "0")
                    { BR7.Checked = true; }
                    else
                    { BR8.Checked = true; }

                    if (temp.Substring(4, 1) == "0")
                    { BR9.Checked = true; }
                    else
                    { BR10.Checked = true; }

                    if (temp.Substring(5, 1) == "0")
                    { BR11.Checked = true; }
                    else
                    { BR12.Checked = true; }

                    if (temp.Substring(6, 1) == "0")
                    { BR13.Checked = true; }
                    else
                    { BR14.Checked = true; }

                    if (temp.Substring(7, 1) == "0")
                    { BR15.Checked = true; }
                    else
                    { BR16.Checked = true; }

                    if (temp.Substring(8, 1) == "0")
                    { BR17.Checked = true; }
                    else
                    { BR18.Checked = true; }

                    if (temp.Substring(9, 1) == "0")
                    { BR19.Checked = true; }
                    else
                    { BR20.Checked = true; }

                    if (temp.Substring(10, 1) == "0")
                    { BR21.Checked = true; }
                    else
                    { BR22.Checked = true; }

                    if (temp.Substring(11, 1) == "0")
                    { BR23.Checked = true; }
                    else
                    { BR24.Checked = true; }
                }
                else 
                {
                    BR1.Checked = true;
                    BR3.Checked = true;
                    BR5.Checked = true;
                    BR7.Checked = true;
                    BR9.Checked = true;
                    BR11.Checked = true;
                    BR13.Checked = true;
                    BR15.Checked = true;
                    BR17.Checked = true;
                    BR19.Checked = true;
                    BR21.Checked = true;
                    BR23.Checked = true;
                }
            }
            else
            {
                BR1.Checked = true;
                BR3.Checked = true;
                BR5.Checked = true;
                BR7.Checked = true;
                BR9.Checked = true;
                BR11.Checked = true;
                BR13.Checked = true;
                BR15.Checked = true;
                BR17.Checked = true;
                BR19.Checked = true;
                BR21.Checked = true;
                BR23.Checked = true;
            }
        }

        private void TabPage3_Init(string STCD, IList<Service.Model.YY_RTU_CONFIGDATA> list)
        {
            if (list.Count() > 0)
            {
                #region AA04
                var AA04 = from l in list where l.ConfigID == "11000000AA04" select l;
                if (AA04.Count() > 0)
                {
                    string temp = AA04.First().ConfigVal;
                    if (temp.Substring(0, 1) == "1")
                    { RB11_open.Checked = true; }
                    else
                    { RB11_close.Checked = true; }

                    if (temp.Substring(1, 1) == "1")
                    { RB12_open.Checked = true; }
                    else
                    { RB12_close.Checked = true; }

                    if (temp.Substring(2, 1) == "1")
                    { RB13_open.Checked = true; }
                    else
                    { RB13_close.Checked = true; }

                    if (temp.Substring(3, 1) == "1")
                    { RB14_open.Checked = true; }
                    else
                    { RB14_close.Checked = true; }

                    if (temp.Substring(4, 1) == "1")
                    { RB15_open.Checked = true; }
                    else
                    { RB15_close.Checked = true; }

                    if (temp.Substring(5, 1) == "1")
                    { RB16_open.Checked = true; }
                    else
                    { RB16_close.Checked = true; }

                    if (temp.Substring(6, 1) == "1")
                    { RB17_open.Checked = true; }
                    else
                    { RB17_close.Checked = true; }

                    if (temp.Substring(7, 1) == "1")
                    { RB18_open.Checked = true; }
                    else
                    { RB18_close.Checked = true; }

                }
                else
                {
                    RB11_close.Checked = true;
                    RB12_close.Checked = true;
                    RB13_close.Checked = true;
                    RB14_close.Checked = true;
                    RB15_close.Checked = true;
                    RB16_close.Checked = true;
                    RB17_close.Checked = true;
                    RB18_close.Checked = true;
                }
                #endregion

                #region AA05
                var AA05 = from l in list where l.ConfigID == "11000000AA05" select l;
                if (AA05.Count() > 0)
                {
                    string temp = AA05.First().ConfigVal;
                    if (temp.Substring(0, 1) == "1")
                    { RB21_open.Checked = true; }
                    else
                    { RB21_close.Checked = true; }

                    if (temp.Substring(1, 1) == "1")
                    { RB22_open.Checked = true; }
                    else
                    { RB22_close.Checked = true; }

                    if (temp.Substring(2, 1) == "1")
                    { RB23_open.Checked = true; }
                    else
                    { RB23_close.Checked = true; }

                    if (temp.Substring(3, 1) == "1")
                    { RB24_open.Checked = true; }
                    else
                    { RB24_close.Checked = true; }

                    if (temp.Substring(4, 1) == "1")
                    { RB25_open.Checked = true; }
                    else
                    { RB25_close.Checked = true; }

                    if (temp.Substring(5, 1) == "1")
                    { RB26_open.Checked = true; }
                    else
                    { RB26_close.Checked = true; }

                    if (temp.Substring(6, 1) == "1")
                    { RB27_open.Checked = true; }
                    else
                    { RB27_close.Checked = true; }

                    if (temp.Substring(7, 1) == "1")
                    { RB28_open.Checked = true; }
                    else
                    { RB28_close.Checked = true; }

                }
                else
                {
                    RB21_close.Checked = true;
                    RB22_close.Checked = true;
                    RB23_close.Checked = true;
                    RB24_close.Checked = true;
                    RB25_close.Checked = true;
                    RB26_close.Checked = true;
                    RB27_close.Checked = true;
                    RB28_close.Checked = true;
                }
                #endregion

                #region AA06
                var AA06 = from l in list where l.ConfigID == "11000000AA06" select l;
                if (AA06.Count() > 0)
                {
                    string temp = AA06.First().ConfigVal;
                    if (temp.Substring(0, 1) == "1")
                    { RB31_open.Checked = true; }
                    else
                    { RB31_close.Checked = true; }

                    if (temp.Substring(1, 1) == "1")
                    { RB32_open.Checked = true; }
                    else
                    { RB32_close.Checked = true; }

                    if (temp.Substring(2, 1) == "1")
                    { RB33_open.Checked = true; }
                    else
                    { RB33_close.Checked = true; }

                    if (temp.Substring(3, 1) == "1")
                    { RB34_open.Checked = true; }
                    else
                    { RB34_close.Checked = true; }

                    if (temp.Substring(4, 1) == "1")
                    { RB35_open.Checked = true; }
                    else
                    { RB35_close.Checked = true; }

                    if (temp.Substring(5, 1) == "1")
                    { RB36_open.Checked = true; }
                    else
                    { RB36_close.Checked = true; }

                    if (temp.Substring(6, 1) == "1")
                    { RB37_open.Checked = true; }
                    else
                    { RB37_close.Checked = true; }

                    if (temp.Substring(7, 1) == "1")
                    { RB38_open.Checked = true; }
                    else
                    { RB38_close.Checked = true; }

                }
                else
                {
                    RB31_close.Checked = true;
                    RB32_close.Checked = true;
                    RB33_close.Checked = true;
                    RB34_close.Checked = true;
                    RB35_close.Checked = true;
                    RB36_close.Checked = true;
                    RB37_close.Checked = true;
                    RB38_close.Checked = true;
                }
                #endregion
            }
            else
            {
                RB11_close.Checked = true;
                RB12_close.Checked = true;
                RB13_close.Checked = true;
                RB14_close.Checked = true;
                RB15_close.Checked = true;
                RB16_close.Checked = true;
                RB17_close.Checked = true;
                RB18_close.Checked = true;

                RB21_close.Checked = true;
                RB22_close.Checked = true;
                RB23_close.Checked = true;
                RB24_close.Checked = true;
                RB25_close.Checked = true;
                RB26_close.Checked = true;
                RB27_close.Checked = true;
                RB28_close.Checked = true;

                RB31_close.Checked = true;
                RB32_close.Checked = true;
                RB33_close.Checked = true;
                RB34_close.Checked = true;
                RB35_close.Checked = true;
                RB36_close.Checked = true;
                RB37_close.Checked = true;
                RB38_close.Checked = true;
            }
        }

        private string GetTabPage2Str() 
        {
            string val = "";
            if (BR1.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR3.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR5.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR7.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR9.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR11.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR13.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR15.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR17.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR19.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR21.Checked)
            { val += "1"; }
            else
            { val += "0"; }

            if (BR23.Checked)
            { val += "1"; }
            else
            { val += "0"; }
            return val + "11111111111111111111";
        }

        private string GetTabPage3Str_1() 
        { 
            string _AA04 = "";
            if (RB11_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB12_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB13_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; } 
            if (RB14_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB15_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB16_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB17_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            if (RB18_open.Checked)
            { _AA04 += "1"; }
            else
            { _AA04 += "0"; }
            return _AA04;
        }
        private string GetTabPage3Str_2()
        {
            string _AA05 = "";
            if (RB21_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB22_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB23_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB24_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB25_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB26_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB27_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            if (RB28_open.Checked)
            { _AA05 += "1"; }
            else
            { _AA05 += "0"; }
            return _AA05;
        }
        private string GetTabPage3Str_3()
        {
            string _AA06 = "";
            if (RB31_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB32_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB33_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB34_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB35_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB36_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB37_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            if (RB38_open.Checked)
            { _AA06 += "1"; }
            else
            { _AA06 += "0"; }
            return _AA06;
        }
        #endregion

        private void comboBox_STCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            #region 水文
            IList<Service.Model.YY_RTU_CONFIGDATA> list = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ConfigID like '1100________'");
            if (list.Count() > 0) 
            {
                var _0C=from l in list where l.ConfigID =="11000000000C" select l;
                if (_0C.Count() > 0) 
                {
                    int k = 0;
                    if (int.TryParse(_0C.First().ConfigVal,out k))
                    { comboBox_MODE.SelectedIndex = k- 1; }
                }



                var _0E = from l in list where l.ConfigID == "11000000000E" select l;
                if (_0E.Count() > 0) 
                {
                    textBox_RelayAddress.Text = _0E.First().ConfigVal;
                }

                var _20 = from l in list where l.ConfigID == "110000000020" select l;
                if (_20.Count() > 0)
                {
                    for (int i = 0; i < comboBox_20.Items.Count; i++)
                    {
                        if (comboBox_20.Items[i].ToString() == _20.First().ConfigVal)
                        {
                            comboBox_20.SelectedIndex = i;
                            break;
                        }
                    }
                }

                var _22 = from l in list where l.ConfigID == "110000000022" select l;
                if (_22.Count() > 0)
                {
                    int index=0;
                    if(int.TryParse(_22.First().ConfigVal,out index))
                    {
                        if(index>=0&& index<24)
                        comboBox_22.SelectedIndex = index; 
                    }
                }

                var _25 = from l in list where l.ConfigID == "110000000025" select l;
                if (_25.Count() > 0)
                {
                    for (int i = 0; i < comboBox_25.Items.Count ; i++)
                    {
                        if (comboBox_25.Items[i].ToString() == _25.First().ConfigVal)
                        {
                            comboBox_25.SelectedIndex = i;
                            break;
                        }
                    }
                    
                }

                var _26 = from l in list where l.ConfigID == "110000000026" select l;
                if (_26.Count() > 0)
                {
                    for (int i = 0; i < comboBox_26.Items.Count; i++)
                    {
                        if (comboBox_26.Items[i].ToString() == _26.First().ConfigVal)
                        {
                            comboBox_26.SelectedIndex = i;
                            break;
                        }
                    }
                }

                var _24 = from l in list where l.ConfigID == "110000000024" select l;
                if (_24.Count() > 0)
                {
                    textBox_24.Text = _24.First().ConfigVal;
                }

                var _21 = from l in list where l.ConfigID == "110000000021" select l;
                if (_21.Count() > 0)
                {
                    textBox_21.Text = _21.First().ConfigVal;
                }

                var _23 = from l in list where l.ConfigID == "110000000023" select l;
                if (_23.Count() > 0)
                {
                    textBox_23.Text = _23.First().ConfigVal;
                }

                var _0F = from l in list where l.ConfigID == "11000000000F" select l;
                if (_0F.Count() > 0)
                {
                    string[] temps = _0F.First().ConfigVal .Split(new string[] { "," }, StringSplitOptions.None);
                    if (temps.Count() ==2)
                    {
                        int type = 0;
                        if (int.TryParse(temps[0], out type)) 
                        {
                            comboBox_0F.SelectedIndex = type - 1;
                            textBox_0F.Text = temps[1];
                        }
                    }
                }

                var _0D = from l in list where l.ConfigID == "11000000000D" select l;
                if (_0D.Count() > 0) 
                {
                    string CheckedStr = _0D.First().ConfigVal;
                    if (CheckedStr.Length == 64) 
                    {
                        for (int i = 1; i < 65; i++)
                        {
                            foreach (var item in tabPage1.Controls)
                            {
                                if (item is CheckBox && (item as CheckBox).Name == "cb" + i)
                                {
                                    if (CheckedStr[i-1]=='0')
                                        (item as CheckBox).Checked=false;
                                    else
                                        (item as CheckBox).Checked=true;
                                }
                            }
                        }
                    }
                }

                var _AA07 = from l in list where l.ConfigID == "11000000AA07" select l;  //手机
                if (_AA07.Count() > 0)
                {
                    textBox_PhoneNum.Text = _AA07.First().ConfigVal;
                }

                var _AA08 = from l in list where l.ConfigID == "11000000AA08" select l;  //卫星
                if (_AA08.Count() > 0)
                {
                    textBox_SatelliteNum.Text = _AA08.First().ConfigVal;
                }
            }

            TabPage2_Init(comboBox_STCD.SelectedValue.ToString(), list);

            TabPage3_Init(comboBox_STCD.SelectedValue.ToString(), list);
            #endregion
        }

        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }
        private void Clear() 
        {
            comboBox_STtype.SelectedIndex = 0;

            comboBox_20.SelectedIndex = 0;
            comboBox_22.SelectedIndex = 0;
            comboBox_25.SelectedIndex = 0;
            comboBox_26.SelectedIndex = 0;
            comboBox_MODE.SelectedIndex = 0;
            comboBox_0F.SelectedIndex = 0;

            textBox_21.Text = "";
            textBox_23.Text = "";
            textBox_24.Text = "";
            textBox_PhoneNum.Text = "";
            textBox_SatelliteNum.Text = "";
            textBox_RelayAddress.Text = "";
            textBox_0F.Text = "";

            foreach (var item in tabPage1.Controls)
            {
                if (item is CheckBox)
                { (item as CheckBox).Checked = false; }
            }

            BR1.Checked = true;
            BR3.Checked = true;
            BR5.Checked = true;
            BR7.Checked = true;
            BR9.Checked = true;
            BR11.Checked = true;
            BR13.Checked = true;
            BR15.Checked = true;
            BR17.Checked = true;
            BR19.Checked = true;
            BR21.Checked = true;
            BR23.Checked = true;

            RB11_close.Checked = true;
            RB12_close.Checked = true;
            RB13_close.Checked = true;
            RB14_close.Checked = true;
            RB15_close.Checked = true;
            RB16_close.Checked = true;
            RB17_close.Checked = true;
            RB18_close.Checked = true;

            RB21_close.Checked = true;
            RB22_close.Checked = true;
            RB23_close.Checked = true;
            RB24_close.Checked = true;
            RB25_close.Checked = true;
            RB26_close.Checked = true;
            RB27_close.Checked = true;
            RB28_close.Checked = true;

            RB31_close.Checked = true;
            RB32_close.Checked = true;
            RB33_close.Checked = true;
            RB34_close.Checked = true;
            RB35_close.Checked = true;
            RB36_close.Checked = true;
            RB37_close.Checked = true;
            RB38_close.Checked = true;
        }
         

        private void SetRTUWorkControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;
            STtype_Init();
            comboBox_MODE_Init();
            comboBox_20_Init();
            comboBox_22_Init(); 
            comboBox_25_Init();
            comboBox_26_Init();
            comboBox_0F_Init();
            comboBox_STCD_Init();

            #region 黑龙江
            //label54.Visible = false;
            //label55.Visible = false;
            //label56.Visible = false;
            //label57.Visible = false;
            //textBox_FF03.Visible = false;
            #endregion
        }


        private string Validate(out List<Service.Model.YY_RTU_CONFIGDATA> list)
        {
            string msg = "";
            list = new List<Service.Model.YY_RTU_CONFIGDATA>();

            Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID ="0000000000";
            Model.ConfigID = "11000000000C";
            Model.ConfigVal =(comboBox_MODE.SelectedIndex+1).ToString();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "110000000020";
            Model.ConfigVal = comboBox_20.Items[comboBox_20.SelectedIndex].ToString();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "110000000022";
            Model.ConfigVal = comboBox_22.Items[comboBox_22.SelectedIndex].ToString();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "110000000025";
            Model.ConfigVal = comboBox_25.Items[comboBox_25.SelectedIndex].ToString();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "110000000026";
            Model.ConfigVal = comboBox_26.Items[comboBox_26.SelectedIndex].ToString();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA01";
            Model.ConfigVal = comboBox_STtype.SelectedValue.ToString();
            list.Add(Model);


            int _24=0;
            if (textBox_24.Text.Trim().Length > 0) 
            {
                if (int.TryParse(textBox_24.Text.Trim(), out _24) && (_24 >= 1 && _24 <= 59))
                {
                    Model = new Service.Model.YY_RTU_CONFIGDATA();
                    Model.STCD = comboBox_STCD.SelectedValue.ToString();
                    Model.ItemID = "0000000000";
                    Model.ConfigID = "110000000024";
                    Model.ConfigVal = _24.ToString();
                    list.Add(Model);
                }
                else
                { msg += "水位数据存储间隔输入有误！" + "\n"; }
            }

            int _21 = 0;
            if (textBox_21.Text.Trim().Length > 0)
            {
                if (int.TryParse(textBox_21.Text.Trim(), out _21) && (_21 >= 0 && _21 <= 59))
                {
                    Model = new Service.Model.YY_RTU_CONFIGDATA();
                    Model.STCD = comboBox_STCD.SelectedValue.ToString();
                    Model.ItemID = "0000000000";
                    Model.ConfigID = "110000000021";
                    Model.ConfigVal = _21.ToString();
                    list.Add(Model);
                }
                else
                { msg += "加报时间间隔输入有误！" + "\n"; }
            }

            int _23 = 0;
            if (textBox_23.Text.Trim().Length > 0)
            {
                if (int.TryParse(textBox_23.Text.Trim(), out _23) && (_23 >= 0 && _23 <= 9999))
                {
                    Model = new Service.Model.YY_RTU_CONFIGDATA();
                    Model.STCD = comboBox_STCD.SelectedValue.ToString();
                    Model.ItemID = "0000000000";
                    Model.ConfigID = "110000000023";
                    Model.ConfigVal = _23.ToString();
                    list.Add(Model);
                }
                else
                { msg += "采样间隔输入有误！" + "\n"; }
            }

            string CheckedStr = "";
            for (int i = 1; i < 65; i++)
            {
                foreach (var item in tabPage1.Controls)
                {
                    if (item is CheckBox && (item as CheckBox).Name == "cb" + i)
                    {
                        if ((item as CheckBox).Checked)
                            CheckedStr += "1";
                        else
                            CheckedStr += "0";
                    }
                }
            }
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000000D";
            Model.ConfigVal = CheckedStr;
            list.Add(Model);

            if (textBox_RelayAddress.Text.Trim().Length > 0) 
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "11000000000E";
                Model.ConfigVal = textBox_RelayAddress.Text.Trim();
                list.Add(Model);
            }

            if (textBox_0F.Text.Trim().Length > 0)
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "11000000000F";
                Model.ConfigVal =(comboBox_0F.SelectedIndex +1)+","+ textBox_0F.Text.Trim();
                list.Add(Model);
            }

            if (textBox_0F.Text.Trim() != "")//1,12345或2,12345
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "11000000AA07";
                Model.ConfigVal = textBox_PhoneNum.Text.Trim();
                list.Add(Model);
            }

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA02";
            Model.ConfigVal = GetTabPage2Str();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA04";
            Model.ConfigVal = GetTabPage3Str_1();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA05";
            Model.ConfigVal = GetTabPage3Str_2();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA06";
            Model.ConfigVal = GetTabPage3Str_3();
            list.Add(Model);

            Model = new Service.Model.YY_RTU_CONFIGDATA();  
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA07";
            Int64 PhoneNum = 0;
            if (textBox_PhoneNum.Text.Trim().Length > 0)
            {
                if (Int64.TryParse(textBox_PhoneNum.Text.Trim(), out PhoneNum) && textBox_PhoneNum.Text.Trim().Length >= 11)
                {
                    Model.ConfigVal = textBox_PhoneNum.Text.Trim(); 
                    list.Add(Model);
                }
                else { msg += "手机号输入有误！" + "\n"; }
            }


            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000AA08";
            Int64 SatelliteNum = 0;
            if (textBox_SatelliteNum.Text.Trim().Length > 0)
            {
                if (Int64.TryParse(textBox_SatelliteNum.Text.Trim(), out SatelliteNum))
                {
                    Model.ConfigVal = textBox_SatelliteNum.Text.Trim(); 
                    list.Add(Model);
                }
                else { msg += "卫星号输入有误！" + "\n"; }
            }

            #region 黑龙江增  最低控制水位--FF03
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "11000000FF03";
            decimal FF03 = 0;
            if (textBox_FF03.Text.Trim().Length > 0) 
            {
                if (decimal.TryParse(textBox_FF03.Text.Trim(), out FF03)) 
                {
                    if (Math.Floor(FF03).ToString().Length > 7)
                    {
                        msg += "最低控制水位输入有误！" + "\n";
                    }
                    else 
                    {
                        Model.ConfigVal = FF03.ToString("f3");
                        list.Add(Model);
                    }
                }
                else { msg += "最低控制水位输入有误！" + "\n"; }
            }
            #endregion
            return msg;
        }


        private void button_Set_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                List<Service.Model.YY_RTU_CONFIGDATA> list = new List<Service.Model.YY_RTU_CONFIGDATA>();
                string msg = Validate(out list);
                if (msg == "")
                {
                    bool b =Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ConfigID like '1100________'");
                    foreach (var item in list)
                    {
                        b=Service.PublicBD.db.AddRTU_ConfigData(item);
                    }

                    if (b)
                    { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else { DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ConfigID like '1100________'");
                if (b)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Clear();
                }
                else
                { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
              
            }
        }

        private void button_ReBootService_Click(object sender, EventArgs e)
        {
            ((MainForm)this.ParentForm).buttonItem_ReBootService_Click(null, null);
        }

        private void button_ReadRTU_Click(object sender, EventArgs e)
        {
            ((MainForm)this.ParentForm).buttonItem_ReadRTU_Click(null, null);
        }

     
    }
}
