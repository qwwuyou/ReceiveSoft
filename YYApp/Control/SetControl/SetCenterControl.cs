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
    public partial class SetCenterControl : UserControl
    {
        public SetCenterControl()
        {
            InitializeComponent();
        }

        private void SetCenterControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            comboBox_STCD_Init();
            DropDownList_Init(comboBox_COM_M1,true);
            DropDownList_Init(comboBox_COM_M2, true);
            DropDownList_Init(comboBox_COM_M3, true);
            DropDownList_Init(comboBox_COM_M4, true);
            DropDownList_Init(comboBox_COM_B1,false);
            DropDownList_Init(comboBox_COM_B2, false);
            DropDownList_Init(comboBox_COM_B3, false);
            DropDownList_Init(comboBox_COM_B4, false);
        }

        #region 初始化
        private void comboBox_STCD_Init() 
        {
            string Where = "where NiceName like '%" + comboBox_STCD .Text + "%'";
            IList<Service.Model.YY_RTU_Basic> ItemList = PublicBD.db.GetRTUList(Where);

            if (ItemList != null && ItemList.Count >0)
            {
                comboBox_STCD.DataSource = ItemList;
                comboBox_STCD.DisplayMember = "NiceName";
                comboBox_STCD.ValueMember = "STCD";
                comboBox_STCD.SelectedIndex = 0;

                comboBox_STCD_SelectedIndexChanged(comboBox_STCD, new EventArgs());
                this.comboBox_STCD.SelectedIndexChanged += new System.EventHandler(this.comboBox_STCD_SelectedIndexChanged);
            }
        }

        private void DropDownList_Init(ComboBox cb,bool b)
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
            //if(b)
            cb.SelectedIndex = 0;
        }
        #endregion

        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }

        #region 验证&清空
        private Service.Model.YY_RTU_WRES  Validate1(out string Msg) 
        {
            Msg = "";
            Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();

            if (comboBox_STCD.SelectedValue == null)
            {
                Msg += "请选择测站!" + "\n";
            }
            else 
            {
                model.STCD = comboBox_STCD.SelectedValue.ToString();
            }

            model.COM_M =comboBox_COM_M1.SelectedIndex ; //------------
            model.CODE = 1;
            int adr=0;
            if (!int.TryParse(textBox_ADR_ZX1.Text.Trim(), out adr))//----
            {
                Msg += "中心站地址输入有误!" + "\n";
            }
            else 
            {
                if (adr < 1 || adr > 255)
                {
                    Msg += "中心站地址输入有误!" + "\n";
                }
                else 
                {
                    model.ADR_ZX = adr;
                }
            }

            if (comboBox_COM_M1.SelectedIndex == 2 ||comboBox_COM_M1.SelectedIndex == 7) //-----------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_M1.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "主信道地址输入有误!" + "\n";
                }else
                {
                    model.ADR_M =textBox_ADR_M1.Text.Trim(); //--------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_M1.Text.Trim(), out port))//---------------
                {
                    Msg += "主信道端口输入有误!" + "\n";
                }
                else 
                {
                    if (port < 0 || port > 65535)
                    { Msg += "主信道端口输入有误!" + "\n"; }
                    else 
                    {
                        model.PORT_M = port;
                    }
                }
            }
            if (comboBox_COM_B1.SelectedIndex != -1)             //------------
            {
                model.COM_B = comboBox_COM_B1.SelectedIndex ;  //-----------
            }
            if (comboBox_COM_B1.SelectedIndex == 2 ) //-------------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_B1.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "备用信道地址输入有误!" + "\n";
                }
                else 
                {
                    model.ADR_B = textBox_ADR_B1.Text.Trim();//---------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_B1.Text.Trim(), out port))//---------------
                {
                    Msg += "备用信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "备用信道端口输入有误!" + "\n"; }
                    else {
                        model.PORT_B = port;
                    }
                }
            }

            if (comboBox_COM_M1.SelectedIndex == 1 || comboBox_COM_B1.SelectedIndex == 1) //------------------
            {
                if (!IsPhone(textBox_PhoneNum1.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else 
                {
                    model.PhoneNum = textBox_PhoneNum1.Text.Trim(); //----------
                }
            }


            if (comboBox_COM_M1.SelectedIndex == 3 || comboBox_COM_M1.SelectedIndex == 4 || comboBox_COM_B1.SelectedIndex == 3 || comboBox_COM_B1.SelectedIndex == 4) //-----------
            {
                int port = 0;
                if (!int.TryParse(textBox_SatelliteNum1.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum1.Text.Trim();//------------------
                }
            }

            #region IPV4+短信/卫星  ---黑龙江
            if (comboBox_COM_M1.SelectedIndex == 7) 
            {
                if (textBox_PhoneNum1.Text.Trim() == "" && textBox_SatelliteNum1.Text.Trim() == "") 
                {
                    Msg += "手机号码或卫星号码至少输入一个!" + "\n";
                }

                if (textBox_PhoneNum1.Text.Trim()!=""&&!IsPhone(textBox_PhoneNum1.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum1.Text.Trim(); //----------
                }

                int port = 0;
                if (textBox_SatelliteNum1.Text.Trim()!="" && !int.TryParse(textBox_SatelliteNum1.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum1.Text.Trim();//------------------
                }
            }
            #endregion

            return model ;
        }

        private void Clear1() 
        {
            foreach (var item in groupBox1.Controls )
            {
                if (item is TextBox) 
                {
                    (item as TextBox).Text = ""; 
                }
            }

            DropDownList_Init(comboBox_COM_M1, true);
            DropDownList_Init(comboBox_COM_B1, false);
        }

        private Service.Model.YY_RTU_WRES Validate2(out string Msg)
        {
            Msg = "";
            Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();

            if (comboBox_STCD.SelectedValue == null)
            {
                Msg += "请选择测站!" + "\n";
            }
            else
            {
                model.STCD = comboBox_STCD.SelectedValue.ToString();
            }

            model.COM_M = comboBox_COM_M2.SelectedIndex ; //------------
            model.CODE = 2;                                  //------------
            int adr = 0;
            if (!int.TryParse(textBox_ADR_ZX2.Text.Trim(), out adr))//----
            {
                Msg += "中心站地址输入有误!" + "\n";
            }
            else
            {
                if (adr < 1 || adr > 255)
                {
                    Msg += "中心站地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_ZX = adr;
                }
            }

            if (comboBox_COM_M2.SelectedIndex == 2 || comboBox_COM_M2.SelectedIndex == 7) //-----------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_M2.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "主信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_M = textBox_ADR_M2.Text.Trim(); //--------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_M2.Text.Trim(), out port))//---------------
                {
                    Msg += "主信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "主信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_M = port;
                    }
                }
            }

            if (comboBox_COM_B2.SelectedIndex != -1) 
            {
                model.COM_B = comboBox_COM_B2.SelectedIndex ;
            }

            if (comboBox_COM_B2.SelectedIndex == 2) //-------------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_B2.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "备用信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_B = textBox_ADR_B2.Text.Trim();//---------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_B2.Text.Trim(), out port))//---------------
                {
                    Msg += "备用信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "备用信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_B = port;
                    }
                }
            }

            if ( comboBox_COM_M2.SelectedIndex == 1 ||  comboBox_COM_B2.SelectedIndex == 1) //------------------
            {
                if (!IsPhone(textBox_PhoneNum2.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum2.Text.Trim(); //----------
                }
            }


            if (comboBox_COM_M2.SelectedIndex == 3 || comboBox_COM_M2.SelectedIndex == 4 || comboBox_COM_B2.SelectedIndex == 3 || comboBox_COM_B2.SelectedIndex == 4) //-----------
            {
                int port = 0;
                if (!int.TryParse(textBox_SatelliteNum2.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum2.Text.Trim();//------------------
                }

            }


            #region IPV4+短信/卫星  ---黑龙江
            if (comboBox_COM_M2.SelectedIndex == 7)
            {
                if (textBox_PhoneNum2.Text.Trim() == "" && textBox_SatelliteNum2.Text.Trim() == "")
                {
                    Msg += "手机号码或卫星号码至少输入一个!" + "\n";
                }

                if (textBox_PhoneNum2.Text.Trim() != "" && !IsPhone(textBox_PhoneNum2.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum2.Text.Trim(); //----------
                }

                int port = 0;
                if (textBox_SatelliteNum2.Text.Trim() != "" && !int.TryParse(textBox_SatelliteNum2.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum2.Text.Trim();//------------------
                }
            }
            #endregion

            return model;
        }

        private void Clear2()
        {
            foreach (var item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
            }

            DropDownList_Init(comboBox_COM_M2, true);
            DropDownList_Init(comboBox_COM_B2, false);
        }

        private Service.Model.YY_RTU_WRES Validate3(out string Msg)
        {
            Msg = "";
            Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();

            if (comboBox_STCD.SelectedValue == null)
            {
                Msg += "请选择测站!" + "\n";
            }
            else
            {
                model.STCD = comboBox_STCD.SelectedValue.ToString();
            }

            model.COM_M = comboBox_COM_M3.SelectedIndex; //------------
            model.CODE = 3;                                  //------------
            int adr = 0;
            if (!int.TryParse(textBox_ADR_ZX3.Text.Trim(), out adr))//----
            {
                Msg += "中心站地址输入有误!" + "\n";
            }
            else
            {
                if (adr < 1 || adr > 255)
                {
                    Msg += "中心站地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_ZX = adr;
                }
            }

            if (comboBox_COM_M3.SelectedIndex == 2 || comboBox_COM_M3.SelectedIndex == 7) //-----------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_M3.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "主信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_M = textBox_ADR_M3.Text.Trim(); //--------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_M3.Text.Trim(), out port))//---------------
                {
                    Msg += "主信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "主信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_M = port;
                    }
                }
            }

            if (comboBox_COM_B3.SelectedIndex != -1)   //--------------
            {
                model.COM_B = comboBox_COM_B3.SelectedIndex ;  //------
            }

            if (comboBox_COM_B3.SelectedIndex == 2) //-------------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_B3.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "备用信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_B = textBox_ADR_B3.Text.Trim();//---------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_B3.Text.Trim(), out port))//---------------
                {
                    Msg += "备用信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "备用信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_B = port;
                    }
                }
            }

            if (comboBox_COM_M3.SelectedIndex ==  1 || comboBox_COM_B3.SelectedIndex ==  1) //------------------
            {
                if (!IsPhone(textBox_PhoneNum3.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum3.Text.Trim(); //----------
                }
            }


            if (comboBox_COM_M3.SelectedIndex == 3 || comboBox_COM_M3.SelectedIndex == 4 || comboBox_COM_B3.SelectedIndex == 3 || comboBox_COM_B3.SelectedIndex == 4) //-----------
            {
                int port = 0;
                if (!int.TryParse(textBox_SatelliteNum3.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum3.Text.Trim();//------------------
                }

            }


            #region IPV4+短信/卫星  ---黑龙江
            if (comboBox_COM_M3.SelectedIndex == 7)
            {
                if (textBox_PhoneNum3.Text.Trim() == "" && textBox_SatelliteNum3.Text.Trim() == "")
                {
                    Msg += "手机号码或卫星号码至少输入一个!" + "\n";
                }

                if (textBox_PhoneNum3.Text.Trim() != "" && !IsPhone(textBox_PhoneNum3.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum3.Text.Trim(); //----------
                }

                int port = 0;
                if (textBox_SatelliteNum3.Text.Trim() != "" && !int.TryParse(textBox_SatelliteNum3.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum3.Text.Trim();//------------------
                }
            }
            #endregion

            return model;
        }

        private void Clear3()
        {
            foreach (var item in groupBox3.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
            }

            DropDownList_Init(comboBox_COM_M3, true);
            DropDownList_Init(comboBox_COM_B3, false);
        }

        private Service.Model.YY_RTU_WRES Validate4(out string Msg)
        {
            Msg = "";
            Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();

            if (comboBox_STCD.SelectedValue == null)
            {
                Msg += "请选择测站!" + "\n";
            }
            else
            {
                model.STCD = comboBox_STCD.SelectedValue.ToString();
            }

            model.COM_M = comboBox_COM_M4.SelectedIndex ; //------------
            model.CODE = 4;                                  //------------
            int adr = 0;
            if (!int.TryParse(textBox_ADR_ZX4.Text.Trim(), out adr))//----
            {
                Msg += "中心站地址输入有误!" + "\n";
            }
            else
            {
                if (adr < 1 || adr > 255)
                {
                    Msg += "中心站地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_ZX = adr;
                }
            }

            if (comboBox_COM_M4.SelectedIndex == 2 || comboBox_COM_M3.SelectedIndex == 7) //-----------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_M4.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "主信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_M = textBox_ADR_M4.Text.Trim(); //--------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_M4.Text.Trim(), out port))//---------------
                {
                    Msg += "主信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "主信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_M = port;
                    }
                }
            }

            if (comboBox_COM_B4.SelectedIndex != -1)   //--------------
            {
                model.COM_B = comboBox_COM_B4.SelectedIndex ;  //------
            }

            if (comboBox_COM_B4.SelectedIndex == 2) //-------------
            {
                System.Net.IPAddress address;
                if (!System.Net.IPAddress.TryParse(textBox_ADR_B4.Text.Trim(), out address))//-----------
                {
                    //失败
                    Msg += "备用信道地址输入有误!" + "\n";
                }
                else
                {
                    model.ADR_B = textBox_ADR_B4.Text.Trim();//---------------
                }


                int port = 0;
                if (!int.TryParse(textBox_PORT_B4.Text.Trim(), out port))//---------------
                {
                    Msg += "备用信道端口输入有误!" + "\n";
                }
                else
                {
                    if (port < 0 || port > 65535)
                    { Msg += "备用信道端口输入有误!" + "\n"; }
                    else
                    {
                        model.PORT_B = port;
                    }
                }
            }

            if (comboBox_COM_M4.SelectedIndex == 1 || comboBox_COM_B4.SelectedIndex == 1) //------------------
            {
                if (!IsPhone(textBox_PhoneNum4.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum4.Text.Trim(); //----------
                }
            }


            if (comboBox_COM_M4.SelectedIndex == 3 || comboBox_COM_M4.SelectedIndex == 4 || comboBox_COM_B4.SelectedIndex == 3 || comboBox_COM_B4.SelectedIndex == 4) //-----------
            {
                int port = 0;
                if (!int.TryParse(textBox_SatelliteNum4.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum4.Text.Trim();//------------------
                }

            }


            #region IPV4+短信/卫星  ---黑龙江
            if (comboBox_COM_M4.SelectedIndex == 7)
            {
                if (textBox_PhoneNum4.Text.Trim() == "" && textBox_SatelliteNum4.Text.Trim() == "")
                {
                    Msg += "手机号码或卫星号码至少输入一个!" + "\n";
                }

                if (textBox_PhoneNum4.Text.Trim() != "" && !IsPhone(textBox_PhoneNum4.Text.Trim())) //---------------
                {
                    Msg += "手机号输入有误!" + "\n";
                }
                else
                {
                    model.PhoneNum = textBox_PhoneNum4.Text.Trim(); //----------
                }

                int port = 0;
                if (textBox_SatelliteNum4.Text.Trim() != "" && !int.TryParse(textBox_SatelliteNum4.Text.Trim(), out port))//---------------
                {
                    Msg += "卫星号码输入有误!" + "\n";
                }
                else
                {
                    model.SatelliteNum = textBox_SatelliteNum4.Text.Trim();//------------------
                }
            }
            #endregion

            return model;
        }

        private void Clear4()
        {
            foreach (var item in groupBox4.Controls)
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

        private void button_Add1_Click(object sender, EventArgs e)
        {
            string Msg="";
            Service.Model.YY_RTU_WRES model=Validate1(out Msg);
            if (Msg == "")
            {
                bool b1 = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                bool b2 = PublicBD.db.AddRTU_WRES(model);
                if (b1 && b2)
                    DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                else
                    DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站\n" + Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Del1_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                if (b)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear1();
                }
                else { DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站信息删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void button_Add2_Click(object sender, EventArgs e)
        {
            string Msg = "";
            Service.Model.YY_RTU_WRES model = Validate2(out Msg);
            if (Msg == "")
            {
                bool b1 = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                bool b2 = PublicBD.db.AddRTU_WRES(model);
                if (b1 && b2)
                    DevComponents.DotNetBar.MessageBoxEx.Show("第二中心站信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DevComponents.DotNetBar.MessageBoxEx.Show("第二中心站信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站\n" + Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Del2_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                if (b)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("第二中心站信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear2();
                }
                else { DevComponents.DotNetBar.MessageBoxEx.Show("第二中心站信息删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void button_Add3_Click(object sender, EventArgs e)
        {
            string Msg = "";
            Service.Model.YY_RTU_WRES model = Validate3(out Msg);
            if (Msg == "")
            {
                bool b1 = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                bool b2 = PublicBD.db.AddRTU_WRES(model);
                if (b1 && b2)
                    DevComponents.DotNetBar.MessageBoxEx.Show("第三中心站信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DevComponents.DotNetBar.MessageBoxEx.Show("第三中心站信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站\n" + Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Del3_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                if (b)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("第三中心站信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear3();
                }
                else { DevComponents.DotNetBar.MessageBoxEx.Show("第三中心站信息删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void button_Add4_Click(object sender, EventArgs e)
        {
            string Msg = "";
            Service.Model.YY_RTU_WRES model = Validate4(out Msg);
            if (Msg == "")
            {
                bool b1 = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                bool b2 = PublicBD.db.AddRTU_WRES(model);
                if (b1 && b2)
                    DevComponents.DotNetBar.MessageBoxEx.Show("第四中心站信息配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DevComponents.DotNetBar.MessageBoxEx.Show("第四中心站信息配置失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("第一中心站\n" + Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Del4_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                bool b = PublicBD.db.DelRTU_WRES("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and CODE=1");
                if (b)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("第四中心站信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear4();
                }
                else { DevComponents.DotNetBar.MessageBoxEx.Show("第四中心站信息删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void comboBox_STCD_SelectedIndexChanged(object sender, EventArgs e)
        {
             IList<Service.Model.YY_RTU_WRES> RTU_WRESList= PublicBD.db.GetRTU_WRESList("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "'");
             Clear1();
             Clear2();
             Clear3();
             Clear4();
             foreach (var item in RTU_WRESList)
             {
                 if (item.CODE == 1) 
                 {
                     textBox_ADR_ZX1.Text = item.ADR_ZX.ToString();
                     if (item.COM_M.HasValue==true )
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
                     { comboBox_COM_B1.SelectedIndex = -1; }
                     textBox_ADR_B1.Text = item.ADR_B;
                     if (item.PORT_B.HasValue == true)
                     {
                         textBox_PORT_B1.Text = item.PORT_B.Value.ToString();
                     }
                     textBox_PhoneNum1.Text = item.PhoneNum;
                     textBox_SatelliteNum1.Text = item.SatelliteNum;
                     
                 }
                 else if (item.CODE == 2)
                 {
                     textBox_ADR_ZX2.Text = item.ADR_ZX.ToString();
                     if (item.COM_M.HasValue == true)
                     {
                         comboBox_COM_M2.SelectedIndex = item.COM_M.Value ;
                     }
                     textBox_ADR_M1.Text = item.ADR_M;
                     if (item.PORT_M.HasValue == true)
                     {
                         textBox_PORT_M2.Text = item.PORT_M.Value.ToString();
                     }
                     if (item.COM_B.HasValue == true)
                     {
                         comboBox_COM_B2.SelectedIndex = item.COM_B.Value ;
                     }
                     else
                     { comboBox_COM_B2.SelectedIndex = -1; }
                     textBox_ADR_B2.Text = item.ADR_B;

                     if (item.PORT_B.HasValue == true)
                     {
                         textBox_PORT_B2.Text = item.PORT_B.Value.ToString();
                     }
                     textBox_PhoneNum2.Text = item.PhoneNum;
                     textBox_SatelliteNum2.Text = item.SatelliteNum;
                 }
                 else if (item.CODE == 3)
                 {
                     textBox_ADR_ZX3.Text = item.ADR_ZX.ToString();
                     if (item.COM_M.HasValue == true)
                     {
                         comboBox_COM_M3.SelectedIndex = item.COM_M.Value ;
                     }
                     textBox_ADR_M1.Text = item.ADR_M;
                     if (item.PORT_M.HasValue == true)
                     {
                         textBox_PORT_M3.Text = item.PORT_M.Value.ToString();
                     }
                     if (item.COM_B.HasValue == true)
                     {
                         comboBox_COM_B3.SelectedIndex = item.COM_B.Value ;
                     }
                     else
                     { comboBox_COM_B3.SelectedIndex = -1; }
                     textBox_ADR_B3.Text = item.ADR_B;

                     if (item.PORT_B.HasValue == true)
                     {
                         textBox_PORT_B3.Text = item.PORT_B.Value.ToString();
                     }
                     textBox_PhoneNum3.Text = item.PhoneNum;
                     textBox_SatelliteNum3.Text = item.SatelliteNum;
                 }
                 else if (item.CODE == 4)
                 {
                     textBox_ADR_ZX4.Text = item.ADR_ZX.ToString();
                     if (item.COM_M.HasValue == true)
                     {
                         comboBox_COM_M3.SelectedIndex = item.COM_M.Value ;
                     }
                     textBox_ADR_M1.Text = item.ADR_M;
                     if (item.PORT_M.HasValue == true)
                     {
                         textBox_PORT_M4.Text = item.PORT_M.Value.ToString();
                     }
                     if (item.COM_B.HasValue == true)
                     {
                         comboBox_COM_B4.SelectedIndex = item.COM_B.Value ;
                     }
                     else
                     { comboBox_COM_B4.SelectedIndex = -1; }
                     textBox_ADR_B4.Text = item.ADR_B;
                     if (item.PORT_B.HasValue == true)
                     {
                         textBox_PORT_B4.Text = item.PORT_B.Value.ToString();
                     }
                     textBox_PhoneNum4.Text = item.PhoneNum;
                     textBox_SatelliteNum4.Text = item.SatelliteNum;
                 }
             }
        }
    }
}
