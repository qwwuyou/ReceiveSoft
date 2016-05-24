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
    //协议（2） 是否监测项（2）  内部编码（4，如果为0，未归类） 协议编码（4） 例：111100000025


    public partial class SetYyRTUWorkControl : UserControl
    {
        public SetYyRTUWorkControl()
        {
            InitializeComponent();
        }

        public List<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = null;
        #region 初始化
        private void comboBox_STCD_Init()
        {
            string Where = "where NiceName like '%" + comboBox_STCD.Text + "%'";
            IList<Service.Model.YY_RTU_Basic> ItemList = PublicBD.db.GetRTUList(Where);

            if (ItemList != null && ItemList.Count > 0)
            {
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
        }

        private void comboBox_MODE_Init() 
        {
            comboBox_MODE.Items.Clear();
            comboBox_MODE.Items.Add("自动掉电(00)");
            comboBox_MODE.Items.Add("长供电(01)");
            comboBox_MODE.SelectedIndex = 0;
        }

        private void ShowInit() 
        {
            Point p = new Point(300, 0);
            groupBox3.Location = p;
            comboBox_MODE_Init();
        }
        #endregion

        private void comboBox_STCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("").ToList<Service.Model.YY_RTU_CONFIGDATA>();
            Clear(); 
            GetRTUTime(comboBox_STCD.SelectedValue.ToString());
            GetRTUWork(comboBox_STCD.SelectedValue.ToString());
        }

        private void Clear() 
        {
            foreach (var item in panelEx2.Controls)
            {
                if (item is ComboBox) 
                {
                    (item as ComboBox).SelectedIndex = -1;
                }
                if (item is TextBox) 
                {
                    (item as TextBox).Text = "";
                }
                if (item is CheckBox) 
                {
                    (item as CheckBox).Checked = false;
                }

                if (item is GroupBox) 
                {
                    if ((item as GroupBox).Name == "groupBox1" || (item as GroupBox).Name == "groupBox2")
                    foreach (var item1 in (item as GroupBox).Controls )
                    {
                        (item1 as RadioButton).Checked = false;
                    }
                    
                }
            }
        }

        private void GetRTUTime(string stcd)
        {
            var time = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "100000000007" select t;
            //0雨量量级  1水位量级
            var time0 = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "100000000010" select t;
            var time1 = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "100000000008" select t;
            if (time.Count() > 0)
            {
                textBox_Interval.Text = time.First().ConfigVal;
            }
            else { textBox_Interval.Text = "1"; }
            if (time0.Count() > 0)
            {
                textBox_Rainfall.Text = Convert.ToInt32(time0.First().ConfigVal).ToString(); 
            }
            else { textBox_Rainfall.Text = "1"; }
            if (time1.Count() > 0)
            {
                textBox_WaterLevel.Text =Convert.ToInt32(time1.First().ConfigVal).ToString();
            }
            else { textBox_WaterLevel.Text = "1"; }
        }

        private void GetRTUWork(string stcd) 
        {
            comboBox_MODE.SelectedIndex = 0;
            textBox_PhoneNum.Text = "";
            textBox_SatelliteNum.Text = "";
            var time = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "100000000011" select t;

            int mode=0;
            if (time.Count() > 0 && int.TryParse (time.First().ConfigVal,out  mode)) 
            {
                comboBox_MODE.SelectedIndex =mode ;
            }

            var _AA07 = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "10000000AA07" select t;
            if (_AA07.Count() > 0)
            {
                textBox_PhoneNum.Text = _AA07.First().ConfigVal;
            } 
            var _AA08 = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "10000000AA08" select t;
            if (_AA08.Count() > 0)
            {
                textBox_SatelliteNum.Text = _AA08.First().ConfigVal;
            }

            var _13 = from t in CONFIGDATAList where t.STCD == stcd && t.ConfigID == "100000000013" select t;

            int Address = 0;
            if (_13.Count() > 0 && int.TryParse(_13.First().ConfigVal, out  Address))
            {
                textBox_Address .Text =Address.ToString();
            }

        }

        private void SetRTUWorkControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            ShowInit();

            comboBox_STCD_Init();
        }

        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }

        private string Validate(out List<Service.Model.YY_RTU_CONFIGDATA> list)
        {
            string msg = "";
            list = new List<Service.Model.YY_RTU_CONFIGDATA>();

            Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "100000000011";
            Model.ConfigVal = (comboBox_MODE.SelectedIndex).ToString();
            list.Add(Model);


            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "10000000AA07";
            Int64 PhoneNum = 0;
            if (textBox_PhoneNum.Text.Trim().Length > 0)
            {
                if (Int64.TryParse(textBox_PhoneNum.Text.Trim(), out PhoneNum) && textBox_PhoneNum.Text.Trim().Length >= 11)
                {
                    Model.ConfigVal = textBox_PhoneNum.Text.Trim() ;
                }
                else { msg += "手机号输入有误！" + "\n"; }
            }

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "10000000AA08";
            Int64 SatelliteNum = 0;
            if (textBox_SatelliteNum.Text.Trim().Length > 0)
            {
                if (Int64.TryParse(textBox_SatelliteNum.Text.Trim(), out SatelliteNum))
                {
                    Model.ConfigVal =textBox_SatelliteNum.Text.Trim();
                }
                else { msg += "卫星号输入有误！" + "\n"; }
            }
          

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "100000000007";
            if (textBox_Interval.Text.Trim() != "")
            {
                int Interval = 0;
                if (int.TryParse(textBox_Interval.Text.Trim(), out Interval))
                {
                    Model.ConfigVal = Interval.ToString();
                    list.Add(Model);
                }
                else
                {
                    msg = "自报间隔输入有误！" + "\n";
                }
            }
            else
            {
                Model.ConfigVal = "1";
                list.Add(Model);
            }

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "100000000010";
            if (textBox_Rainfall.Text.Trim() != "")
            {
                int Rainfall = 0;
                if (int.TryParse(textBox_Rainfall.Text.Trim(), out Rainfall))
                {
                    Model.ConfigVal= Rainfall.ToString();
                    list.Add(Model);
                }
                else
                {
                    msg += "雨量量级输入有误！" + "\n";
                }
            }
            else
            {
                Model.ConfigVal = "1";
                list.Add(Model);
            }

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "100000000008";
            if (textBox_WaterLevel.Text.Trim() != "")
            {
                int WaterLevel = 0;
                if (int.TryParse(textBox_WaterLevel.Text.Trim(), out WaterLevel))
                {
                    Model .ConfigVal= WaterLevel.ToString();
                    list.Add(Model);
                }
                else
                {
                    msg += "水位量级输入有误！" + "\n";
                }
            }
            else
            {
                Model.ConfigVal = "1";
                list.Add(Model);
            }

            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "100000000013";
            if (textBox_Address.Text.Trim() != "")
            {
                int Address = 0;
                if (int.TryParse(textBox_Address.Text.Trim(), out Address))
                {
                    Model.ConfigVal = Address.ToString();
                    list.Add(Model);
                }
                else
                {
                    msg += "中心地址输入有误！" + "\n";
                }
            }
            else
            {
                Model.ConfigVal = "1";
                list.Add(Model);
            }
            return msg;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                List<Service.Model.YY_RTU_CONFIGDATA> list = new List<Service.Model.YY_RTU_CONFIGDATA>();
                string msg= Validate(out list);
                if (msg== "" )
                {
                    bool b=Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ConfigID like '1000________'");
                    foreach (var item in list)
                    {
                        b = Service.PublicBD.db.AddRTU_ConfigData(item);
                    }
                    if (b)
                    { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else
                { DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ConfigID like '1000________'");
                if (b)
                {
                    comboBox_MODE.SelectedIndex = -1;
                    textBox_PhoneNum.Text = "";
                    textBox_SatelliteNum.Text = "";
                    textBox_Interval.Text = "";
                    textBox_Rainfall.Text = "";
                    textBox_WaterLevel.Text = "";
                    DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
