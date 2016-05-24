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
    public partial class SetRTUWorkControl : UserControl
    {
        public SetRTUWorkControl()
        {
            InitializeComponent();
        }


        #region 初始化
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

        //已弃用
        private void comboBox_MODE_Init() 
        {
            comboBox_MODE.Items.Clear();
            comboBox_MODE.Items.Add("自动掉电(00)");
            comboBox_MODE.Items.Add("长供电(01)");
            comboBox_MODE.SelectedIndex = 0;
        }
        #endregion

        //根据change事件先清空，再赋值
        private void comboBox_STCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            #region 水资源
            List<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ItemID='0000000000' and ConfigID like '1200________'").ToList<Service.Model.YY_RTU_CONFIGDATA>();
            //IList<Service.Model.YY_RTU_WORK> RTU_WORKList = PublicBD.db.GetRTU_WORKList("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "'");
            if (CONFIGDATAList.Count() > 0)
            {
                var list =from d in CONFIGDATAList where d.ConfigID =="120000000012"  select d;
                if (list.Count() > 0) 
                {
                    int mode = 0;
                    if (int.TryParse(list.First().ConfigVal,out mode)) 
                    {
                        comboBox_MODE.SelectedIndex = mode;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "12000000AA07" select d; //手机号
                if (list.Count() > 0)
                {
                    textBox_PhoneNum.Text = list.First().ConfigVal;
                }
                list = from d in CONFIGDATAList where d.ConfigID == "12000000AA08" select d;  //卫星号
                if (list.Count() > 0)
                {
                    textBox_SatelliteNum.Text = list.First().ConfigVal;
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED0" select d; //工作机（值班/备份）是否自动切换
                if (list.Count() > 0)
                {
                    int AutoSwitch = 0;
                    if (int.TryParse(list.First().ConfigVal, out AutoSwitch))
                    {
                        if (AutoSwitch==0)
                        checkBox_AutoSwitch.Checked = false;
                        else
                            checkBox_AutoSwitch.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED2" select d; //工作机中继是否允许转发
                if (list.Count() > 0)
                {
                    int Relaying = 0;
                    if (int.TryParse(list.First().ConfigVal, out Relaying))
                    {
                        if (Relaying == 0)
                            checkBox_Relaying.Checked = false;
                        else
                            checkBox_Relaying.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED4" select d; //电源报警是否主动上报
                if (list.Count() > 0)
                {
                    int PowerReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out PowerReport))
                    {
                        if (PowerReport == 0)
                            checkBox_PowerReport.Checked = false;
                        else
                            checkBox_PowerReport.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED5" select d; //工作机切换是否主动上报
                if (list.Count() > 0)
                {
                    int SwitchReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out SwitchReport))
                    {
                        if (SwitchReport == 0)
                            checkBox_SwitchReport.Checked = false;
                        else
                            checkBox_SwitchReport.Checked = true;
                    }
                }
                list = from d in CONFIGDATAList where d.ConfigID == "120000001ED6" select d; //出现故障是否主动上报
                if (list.Count() > 0)
                {
                    int FaultReport = 0;
                    if (int.TryParse(list.First().ConfigVal, out FaultReport))
                    {
                        if (FaultReport == 0)
                            checkBox_FaultReport.Checked = false;
                        else
                            checkBox_FaultReport.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000000032" select d; //定值控制
                if (list.Count() > 0)
                {
                    int FixValueStatus = 0;
                    if (int.TryParse(list.First().ConfigVal, out FixValueStatus))
                    {
                        if (FixValueStatus == 0)
                            checkBox_FixValueStatus.Checked = false;
                        else
                            checkBox_FixValueStatus.Checked = true;
                    }
                }
                list = from d in CONFIGDATAList where d.ConfigID == "120000000030" select d; // IC 卡功能是否有效
                if (list.Count() > 0)
                {
                    int ICStatus = 0;
                    if (int.TryParse(list.First().ConfigVal, out ICStatus))
                    {
                        if (ICStatus == 0)
                            checkBox_ICStatus.Checked = false;
                        else
                            checkBox_ICStatus.Checked = true;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000000034" select d; // 定值量
                if (list.Count() > 0)
                {
                    decimal  FixValue = 0;
                    if (decimal.TryParse(list.First().ConfigVal, out FixValue))
                    {
                        textBox_FixValue.Text = list.First().ConfigVal;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "12000000001C" select d; // 中继引导码长
                if (list.Count() > 0)
                {
                    int RelayLength = 0;
                    if (int.TryParse(list.First().ConfigVal, out RelayLength))
                    {
                        textBox_RelayLength.Text = list.First().ConfigVal;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000000095" select d; // 工作机切换
                if (list.Count() > 0)
                {
                    int GZJ = 0;
                    if (int.TryParse(list.First().ConfigVal, out GZJ))
                    {
                        if (GZJ == 0)
                        {
                            radGZ_A.Checked = false;
                            radGZ_B.Checked = true;
                            
                        }
                        else
                        {
                            radGZ_A.Checked = true;
                            radGZ_B.Checked = false;
                        }
                    }
                    else 
                    {
                        radGZ_A.Checked = false;
                        radGZ_B.Checked = false;
                    }
                }

                list = from d in CONFIGDATAList where d.ConfigID == "120000000094" select d; // 通信机切换
                if (list.Count() > 0)
                {


                    int TXJ = 0;
                    if (int.TryParse(list.First().ConfigVal, out TXJ))
                    {
                        if (TXJ == 0)
                        {
                            radTX_A.Checked = false;
                            radTX_B.Checked = true;
                        }
                        else
                        {
                            radTX_A.Checked = true;
                            radTX_B.Checked = false;
                        }
                    }
                    else
                    {
                        radTX_A.Checked = false;
                        radTX_B.Checked = false;
                    }
                }
                list = from d in CONFIGDATAList where d.ConfigID == "12000000001D" select d; // 中继站转发终端地址
                if (list.Count() > 0)
                {
                    textBox_RelayAddress.Text = list.First().ConfigVal;
                }

                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "终端内存", "固态存储", "上报水压", "备用" };
                list = from d in CONFIGDATAList where d.ConfigID == "1200000000A0" select d; // 实时数据种类
                if (list.Count() > 0 && list.First().ConfigVal.Length == 16)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (list.First().ConfigVal[i] == '1')
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


                items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速(风向)", "水温", "水质", "土壤含水率", "蒸发量", "水压", "备用1", "备用2", "备用3" };
                list = from d in CONFIGDATAList where d.ConfigID == "120000000020" select d; // 阈值及固态存储时间间隔
                int I = 0;
                DataGridViewRow dvr;
                if (list.Count() > 0)
                {
                    string[] ItemGroup = list.First().ConfigVal.Split(new char[] { ',' });
                    if (ItemGroup.Length == 16)
                        for (int j = 0; j < ItemGroup.Length; j++)
                        {
                            string[] temp = ItemGroup[j].Split(new char[] { ':' });
                            I = this.dataGridView2.Rows.Add();
                            dvr = this.dataGridView2.Rows[I];
                            if (temp[0] == "1")
                            {
                                dvr.Cells[0].Value = true;
                            }
                            else
                            { dvr.Cells[0].Value = false; }
                            dvr.Cells[1].Value = items[j];
                            dvr.Cells[2].Value = temp[1];
                            dvr.Cells[3].Value = temp[2];
                        }

                }


                items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "报警或状态", "水压", "备用1", "备用2" };
                list = from d in CONFIGDATAList where d.ConfigID == "1200000000A1" select d; // 自报种类及时间间隔
                I = 0;
                if (list.Count() > 0)
                {
                    string[] ItemGroup = list.First().ConfigVal.Split(new char[] { ',' });
                    if (ItemGroup.Length == 16)
                        for (int j = 0; j < ItemGroup.Length; j++)
                        {
                            string[] temp = ItemGroup[j].Split(new char[] { ':' });
                            I = this.dataGridView1.Rows.Add();
                            dvr = this.dataGridView1.Rows[I];
                            if (temp[0] == "1")
                            {
                                dvr.Cells[0].Value = true;
                            }
                            else
                            { dvr.Cells[0].Value = false; }
                            dvr.Cells[1].Value = items[j];
                            dvr.Cells[2].Value = temp[1];
                        }

                }


                #region SLD计算-初始化
                //有效段位
                list = from d in CONFIGDATAList where d.ConfigID == "12000000SLD1"  select d;
                if (list.Count() > 0)
                {
                    items = list.First().ConfigVal.Split(new string[] { "," }, StringSplitOptions.None);
                    if (items.Count() == 9)
                    {
                        foreach (var item in panelEx3.Controls)
                        {

                            if (item is CheckBox)
                            {
                                int index=int.Parse((item as CheckBox).Name.Substring(2,1));
                                {
                                    if (items[index - 1].ToString() == "0")
                                        (item as CheckBox).Checked = false;
                                    else
                                        (item as CheckBox).Checked = true;
                                }

                            }
                        }
                    }
                }

                //设备安装点高程
                 list = from d in CONFIGDATAList where d.ConfigID == "12000000SLD2"  select d;
                 if (list.Count() > 0)
                 {
                     textBox_Elevation.Text = list.First().ConfigVal;
                 }

                //水位、流量关系
                 list = from d in CONFIGDATAList where d.ConfigID == "12000000SLD3"  select d;
                 if (list.Count() > 0)
                 {
                     BindingList<Stage> StageList = GetStageList(list.First().ConfigVal);
                     dataGridView3.DataSource = StageList;
                 }
                #endregion
                

                //////////////////////////////
                #region 
                //comboBox_MODE.SelectedIndex = RTU_WORKList.First().MODE;
                //textBox_PhoneNum.Text = RTU_WORKList.First().PhoneNum;
                //textBox_SatelliteNum.Text = RTU_WORKList.First().SatelliteNum;

                //bool b = RTU_WORKList.First().AutoSwitch.HasValue;
                //if (b)
                //{
                //    checkBox_AutoSwitch.Checked = RTU_WORKList.First().AutoSwitch.Value;
                //}

                //b = RTU_WORKList.First().Relaying.HasValue;
                //if (b)
                //{
                //    checkBox_Relaying.Checked = RTU_WORKList.First().Relaying.Value;
                //}

                //b = RTU_WORKList.First().PowerReport.HasValue;
                //if (b)
                //{
                //    checkBox_PowerReport.Checked = RTU_WORKList.First().PowerReport.Value;
                //}

                //b = RTU_WORKList.First().SwitchReport.HasValue;
                //if (b)
                //{
                //    checkBox_SwitchReport.Checked = RTU_WORKList.First().SwitchReport.Value;
                //}

                //b = RTU_WORKList.First().FaultReport.HasValue;
                //if (b)
                //{
                //    checkBox_FaultReport.Checked = RTU_WORKList.First().FaultReport.Value;
                //}

                //b = RTU_WORKList.First().FixValueStatus.HasValue;
                //if (b)
                //{
                //    checkBox_FixValueStatus.Checked = RTU_WORKList.First().FixValueStatus.Value;
                //}

                //b = RTU_WORKList.First().ICStatus.HasValue;
                //if (b)
                //{
                //    checkBox_ICStatus.Checked = RTU_WORKList.First().ICStatus.Value;
                //}

                //textBox_FixValue.Text = RTU_WORKList.First().FixValue.ToString();
                //textBox_RelayAddress.Text = RTU_WORKList.First().RelayAddress;
                //textBox_RelayLength.Text = RTU_WORKList.First().RelayLength.ToString();

                //b = RTU_WORKList.First().WorkM.HasValue;
                //if (b)
                //{
                //    radGZ_A.Checked = RTU_WORKList.First().WorkM.Value;
                //    radGZ_B.Checked = !RTU_WORKList.First().WorkM.Value;
                //}
                //else 
                //{
                //    radGZ_A.Checked = false;
                //    radGZ_B.Checked = false;
                //}

                //b = RTU_WORKList.First().CommunicationM.HasValue;
                //if (b)
                //{
                //    radTX_A.Checked = RTU_WORKList.First().CommunicationM.Value;
                //    radTX_B.Checked = !RTU_WORKList.First().CommunicationM.Value;
                //}
                //else
                //{
                //    radTX_A.Checked = false;
                //    radGZ_B.Checked = false;
                //}
                #endregion
            }

            #endregion
        }

        private void Clear()
        {
            radGZ_A.Checked = false;
            radGZ_B.Checked = false;
            radTX_A.Checked = false;
            radTX_B.Checked = false;
            textBox_PhoneNum.Text = "";
            textBox_SatelliteNum.Text = "";
            comboBox_MODE.SelectedIndex = -1;
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
            }

            checkedListBox1.Items.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            #region SLD
            foreach (var item in panelEx3.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
                if (item is CheckBox)
                {
                    (item as CheckBox).Checked = false;
                }
            }
            dataGridView3.DataSource = new BindingList<Stage>();
            #endregion
        }

        private void SetRTUWorkControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            comboBox_STCD_Init();
        }

        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }

        private string Validate(out List<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList)
        {
            List<Service.Model.YY_RTU_CONFIGDATA> list = new List<Service.Model.YY_RTU_CONFIGDATA>();
            string msg = "";
            #region 工作模式
            Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000000012";
            if (comboBox_MODE.SelectedIndex == -1)
            {
                msg += "请选择工作模式！" + "\n";
            }
            else
            {
                Model.ConfigVal = comboBox_MODE.SelectedIndex.ToString();
            }
            list.Add(Model);
            #endregion

            #region 工作机（值班/备份）是否自动切换
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000001ED0";
            if (checkBox_AutoSwitch.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 工作机中继是否允许转发
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000001ED2";
            if (checkBox_Relaying.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 电源报警是否主动上报
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000001ED4";
            if (checkBox_PowerReport.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 工作机切换是否主动上报
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000001ED5";
            if (checkBox_SwitchReport.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 出现故障是否主动上报
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000001ED6";
            if (checkBox_FaultReport.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 定值控制
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000000032";
            if (checkBox_FixValueStatus.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region IC卡功能是否有效
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000000030";
            if (checkBox_ICStatus.Checked)
            {
                Model.ConfigVal = "1";
            }
            else
            {
                Model.ConfigVal = "0";
            }
            list.Add(Model);
            #endregion

            #region 手机号 
            if (textBox_PhoneNum.Text.Trim().Length > 0) 
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "12000000AA07";
            Int64 PhoneNum=0;
                if (Int64.TryParse(textBox_PhoneNum.Text.Trim(), out PhoneNum) && textBox_PhoneNum.Text.Trim().Length >= 11)
                {
                    Model.ConfigVal = textBox_PhoneNum.Text.Trim();
                }
                else { msg += "手机号输入有误！" + "\n"; }
                list.Add(Model);
            }
            
            #endregion

            #region 卫星号
            if (textBox_SatelliteNum.Text.Trim().Length > 0)
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000AA08";
                Int64 SatelliteNum = 0;
                if (Int64.TryParse(textBox_SatelliteNum.Text.Trim(), out SatelliteNum))
                {
                    Model.ConfigVal = textBox_SatelliteNum.Text.Trim();
                }
                else { msg += "卫星号输入有误！" + "\n"; }

                list.Add(Model);
            }
            #endregion

            #region 定值量
            if (textBox_FixValue.Text.Trim().Length > 0)
            {
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "120000000034";
                double FixValue = 0;
                if (double.TryParse(textBox_FixValue.Text.Trim(), out FixValue))
                {
                    if (FixValue >= -99.999 && FixValue <= 99.999)
                    {
                        Model.ConfigVal= FixValue.ToString();
                    }
                    else { msg += "定值量输入有误！" + "\n"; }
                }
                else { msg += "定值量输入有误！" + "\n"; }
                list.Add(Model);
            }
            #endregion

            #region 中继引导码长
             if (textBox_RelayLength.Text.Trim().Length > 0)
            { 
                 Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000001C";
                int RelayLength = 0;
                if (int.TryParse(textBox_RelayLength.Text.Trim(), out RelayLength))
                {
                    if (RelayLength >= 0 && RelayLength <= 255)
                    {
                        Model.ConfigVal = RelayLength.ToString();
                    }
                    else
                    { msg += "中继引导码长输入有误！" + "\n"; }
                }
                else
                { msg += "中继引导码长输入有误！" + "\n"; }
                list.Add(Model);
            }
            #endregion

            #region 中继站转发终端地址
            if (textBox_RelayAddress.Text.Trim().Length > 0)
            { 
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000001D";
                string[] stcds = textBox_RelayAddress.Text.Trim().Split(new char[] { ',' });
                foreach (var item in stcds)
                {
                    if (item.Length != 10)
                    {
                        msg += "中继站转发终端地址输入有误！" + "\n";
                        break;
                    }
                }

                Model.ConfigVal = textBox_RelayAddress.Text.Trim();

                list.Add(Model);
            }
            #endregion

            #region 工作机切换
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000000095";
            if (radGZ_A.Checked)
                Model.ConfigVal = "1";
            else if (radGZ_B.Checked)
                Model.ConfigVal = "0";
            if (radGZ_A.Checked != radGZ_B.Checked)
            list.Add(Model);
            #endregion

            #region 通信机切换
            Model = new Service.Model.YY_RTU_CONFIGDATA();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            Model.ItemID = "0000000000";
            Model.ConfigID = "120000000094";
            if (radTX_A.Checked)
                Model.ConfigVal = "1";
            else if (radTX_B.Checked)
                Model.ConfigVal = "0";
            if (radTX_A.Checked != radTX_B.Checked)
            list.Add(Model);
            #endregion

            #region SLD
            bool b1=true, b2=true , b3=true;
            string Cbs = (cb1.Checked ? "1" : "0")+","+(cb2.Checked ? "1" : "0")+","+(cb3.Checked ? "1" : "0")+","+(cb4.Checked ? "1" : "0")+","+(cb5.Checked ? "1" : "0")+","+(cb6.Checked ? "1" : "0")+","+(cb7.Checked ? "1" : "0")+","+(cb8.Checked ? "1" : "0") +","+(cb9.Checked ? "1" : "0");
            if (Cbs == "0,0,0,0,0,0,0,0,0")
            { b1 = false; }

            string Elevation = textBox_Elevation.Text.Trim();
            decimal elevation=0;
            if (Elevation == "")
            { b2 = false; }
            else 
            {
                if (!decimal.TryParse(Elevation, out elevation)) 
                {
                    msg += "SLD计算流量的[设备安装点高程]输入有误！" + "\n";
                }
            }

            string stage = "";
            BindingList<Stage> StageList=dataGridView3.DataSource as BindingList<Stage>;
            if (StageList == null || StageList.Count() == 0)
            { b3 = false; }
            else if (StageList.Count() == 1) 
            {
                b3 = false;
                msg += "SLD计算流量的[水位、流量关系数据]至少输入2组！" + "\n";
            }
            else
            {
                foreach (var item in StageList)
                {
                    stage += item.WaterLevel + ":" + item.kA + ",";
                }
            }
            stage = stage.TrimEnd(',');

            if (!b1 && !b2 && !b3)
            {  }
            else if (b1 && b2 && b3)
            {
                //写入 list
                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000SLD1";
                Model.ConfigVal = Cbs;
                list.Add(Model);

                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000SLD2";
                Model.ConfigVal = elevation.ToString();
                list.Add(Model);

                Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = comboBox_STCD.SelectedValue.ToString();
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000SLD3";
                Model.ConfigVal = stage;
                list.Add(Model);
            }
            else
            { msg += "SLD计算流量信息输入不完整或输入有误！" + "\n"; }

            #endregion

            CONFIGDATAList = list;
            return msg;
        }

        //已弃用
        private string Validate(out Service.Model.YY_RTU_WORK model)
        {
            Service.Model.YY_RTU_WORK Model = new Service.Model.YY_RTU_WORK();
            Model.STCD = comboBox_STCD.SelectedValue.ToString();
            string msg = "";
            if (comboBox_MODE.SelectedIndex == -1)
            {
                msg += "请选择工作模式！" + "\n";
            }
            else
            {
                Model.MODE = comboBox_MODE.SelectedIndex;
            }

            Model.AutoSwitch = checkBox_AutoSwitch.Checked;
            Model.Relaying = checkBox_Relaying.Checked;
            Model.PowerReport =checkBox_PowerReport .Checked;
            Model.SwitchReport = checkBox_SwitchReport.Checked;
            Model.FaultReport = checkBox_FaultReport.Checked;
            Model.FixValueStatus = checkBox_FixValueStatus.Checked;
            Model.ICStatus = checkBox_ICStatus.Checked;

            if (radGZ_A.Checked)
                Model.WorkM = true;
            else if (radGZ_B.Checked)
                Model.WorkM = false;

            if (radTX_A.Checked)
                Model.CommunicationM = true;
            else if (radTX_B.Checked)
                Model.CommunicationM = false;


            if (textBox_RelayAddress.Text.Trim().Length > 0)
            {
                string[] stcds = textBox_RelayAddress.Text.Trim().Split(new char[] { ',' });
                foreach (var item in stcds)
                {
                    if (item.Length != 10)
                    {
                        msg += "中继站转发终端地址输入有误！" + "\n";
                        break;
                    }
                }

                Model.RelayAddress = textBox_RelayAddress.Text.Trim();
            }

            if (textBox_RelayLength.Text.Trim().Length > 0)
            {
                int RelayLength = 0;
                if (int.TryParse(textBox_RelayLength.Text.Trim(), out RelayLength))
                {
                    if (RelayLength >= 0 && RelayLength <= 255)
                    {
                        Model.RelayLength = RelayLength;
                    }
                    else
                    { msg += "中继引导码长输入有误！" + "\n"; }
                }
                else
                { msg += "中继引导码长输入有误！" + "\n"; }
            }

            if (textBox_FixValue.Text.Trim().Length > 0)
            {
                double FixValue = 0;
                if (double.TryParse(textBox_FixValue.Text.Trim(), out FixValue))
                {
                    if (FixValue >= -99.999 && FixValue <= 99.999)
                    {
                        Model.FixValue = decimal.Parse(FixValue.ToString());
                    }
                    else { msg += "定值量输入有误！" + "\n"; }
                }
                else { msg += "定值量输入有误！" + "\n"; }
            }

            Int64 PhoneNum=0;
            if (textBox_PhoneNum.Text.Trim().Length > 0) 
            {
                if (Int64.TryParse(textBox_PhoneNum.Text.Trim(), out PhoneNum) && textBox_PhoneNum.Text.Trim().Length >= 11)
                {
                    Model.PhoneNum = textBox_PhoneNum.Text.Trim();
                }
                else { msg += "手机号输入有误！" + "\n"; }
            }

            Int64 SatelliteNum=0;
            if (textBox_SatelliteNum.Text.Trim().Length > 0) 
            {
                if (Int64.TryParse(textBox_SatelliteNum.Text.Trim(), out SatelliteNum))
                {
                    Model.SatelliteNum = textBox_SatelliteNum.Text.Trim();
                }
                else { msg += "卫星号输入有误！" + "\n"; }
            }

            model = Model;
            return msg;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            if (comboBox_STCD.SelectedValue != null)
            {
                List<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = new List<Service.Model.YY_RTU_CONFIGDATA>();
                string msg = Validate(out CONFIGDATAList);
                if (msg == "")
                {
                    bool b = Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ItemID='0000000000'  and ConfigID like '1200________'");
                    foreach (var item in CONFIGDATAList)
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
                //bool b = Service.PublicBD.db.DelRTU_Work("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "'");
                bool b = Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ItemID='0000000000' and ConfigID like '1200________'");
                if (b)
                { DevComponents.DotNetBar.MessageBoxEx.Show("测站工作状态信息删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
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

        #region SLD的事件
        private void button_StageDel_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = new BindingList<Stage>();
        }
        private void button_DelAll_Click(object sender, EventArgs e)
        {
            foreach (var item in panelEx3.Controls)
            {
                if (item is TextBox)
                {
                    (item as TextBox).Text = "";
                }
                if (item is CheckBox)
                {
                    (item as CheckBox).Checked = false;
                }
            }
            dataGridView3.DataSource = new BindingList<Stage>();
        }
        private string SLDValidate(out Stage stage)
        {
            stage = new Stage();
            string msg = "";
            decimal WaterLevel = 0;
            decimal kA = 0;
            if (!decimal.TryParse(textBox_WaterLevel.Text.Trim(), out WaterLevel))
            {
                msg += "SLD计算流量的[水位]输入有误！" + "\n";
            }
            else 
            {
                stage.WaterLevel = decimal.Parse(WaterLevel.ToString("0.00"));
            }
            if (!decimal.TryParse(textBox_kA.Text.Trim(), out kA))
            {
                msg += "SLD计算流量的[流量系数]输入有误！" + "\n";
            }
            else 
            {
                stage.kA = decimal.Parse(kA.ToString("0.00")); 
            }
            return msg;
        }
        private void button_StageAdd_Click(object sender, EventArgs e)
        {
            Stage stage = new Stage();
            string msg = SLDValidate(out stage);
            if (msg != "")
            { DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else
            {
                BindingList<Stage> StageList = null;
                StageList = dataGridView3.DataSource as BindingList<Stage>;

                if (StageList == null)
                {
                    StageList = new BindingList<Stage>();
                }
                
                //验证该水位是否存在
                var list = from d in StageList where stage.WaterLevel == d.WaterLevel select d;
                if (list.Count() > 0)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("该水位数据已经存在，请核实！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    StageList.Add(stage);
                    textBox_WaterLevel.Text = "";
                    textBox_kA.Text = "";
                }
                //排序，重新绑定
                List <Stage>  sl=StageList.OrderBy(i => i.WaterLevel).ToList();
                StageList=new BindingList<Stage>();
                foreach (var item in sl)
                {
                    Stage s=new Stage();
                    s.kA=item.kA ;
                    s.WaterLevel =item.WaterLevel;
                    StageList.Add(s);
                }
                dataGridView3.DataSource = StageList;
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (dataGridView3.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    dataGridView3.Rows.Remove(dataGridView3.Rows[e.RowIndex]);
                }
            }
        }
        #endregion
        
        /// <summary>
        /// 水位、流量关系类
        /// </summary>
        public  class Stage
        {
            public Stage()
            { }
            private decimal _WaterLevel;
            private decimal _kA;
            /// <summary>
            /// 水位
            /// </summary>
            public decimal WaterLevel
            {
                set { _WaterLevel = value; }
                get { return _WaterLevel; }
            }
            /// <summary>
            /// 流量系数
            /// </summary>
            public decimal kA
            {
                set { _kA = value; }
                get { return _kA; }
            }
        }
        private BindingList<Stage> GetStageList(string Val) 
        {
            BindingList<Stage> list = new BindingList<Stage>();
            string[] Items = Val.Split(new string[] { "," }, StringSplitOptions.None);
            if (Items.Count() > 0) 
            {
                foreach (var item in Items)
                {
                    string[] items= item.Split(new string[] { ":" }, StringSplitOptions.None);
                    Stage stage = new Stage();
                    stage.WaterLevel = decimal.Parse(items[0]);
                    stage.kA = decimal.Parse(items[1]);
                    list.Add(stage);
                }
            }

            return list;
        }

    }
}
