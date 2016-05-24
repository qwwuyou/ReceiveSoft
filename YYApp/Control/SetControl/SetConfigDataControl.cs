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
    public partial class SetConfigDataControl : UserControl
    {
        public SetConfigDataControl()
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
        }

        private IList<Service.Model.YY_RTU_ITEM> ITEMList = null;
        private void comboBox_Item_Init()
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(" where ItemCode!='-1'");
            if (ItemList != null && ItemList.Count > 0)
            {
                //Service.Model.YY_RTU_ITEM item = new Service.Model.YY_RTU_ITEM();
                //item.ItemID = "0000000000";
                //item.ItemName = "RTU";
                //ItemList.Insert(0,item);

                ITEMList = ItemList;
                comboBox_Item.DataSource = ItemList;
                comboBox_Item.DisplayMember = "ItemName";
                comboBox_Item.ValueMember = "ItemID";
                comboBox_Item.SelectedIndex = 0;
            }
        }

        private void comboBox_ConfigItem_Init()
        {
            IList<Service.Model.YY_RTU_CONFIGITEM> ConfigItemList = PublicBD.db.GetRTU_ConfigItemList("");
            if (ConfigItemList !=null &&ConfigItemList.Count > 0)
            {
                CIL = ConfigItemList;
                comboBox_ConfigItem.DataSource = ConfigItemList;
                comboBox_ConfigItem.DisplayMember = "ConfigItem";
                comboBox_ConfigItem.ValueMember = "ConfigID";
                comboBox_ConfigItem.SelectedIndex = 0;
            }
        }
        #endregion 

        private bool Validate(string str)
        {
            int ItemInteger = 0;
            int ItemDecimal = 0;
            bool PlusOrMinus = true;
            string regText = "";
            if (ITEMList != null)
            {
                var item = from i in ITEMList where i.ItemID == comboBox_Item.SelectedValue.ToString() select i;
                if (item.Count() > 0)
                {
                    ItemInteger = item.First().ItemInteger;
                    ItemDecimal = item.First().ItemDecimal;
                    PlusOrMinus = item.First().PlusOrMinus;

                    if (PlusOrMinus)
                    {
                        if (ItemDecimal == 0)
                        {
                            regText = @"^[-+]?\d{1," + ItemInteger + @"}$";
                        }
                        else
                        {
                            regText = @"^[-+]?\d{1," + ItemInteger + @"}(\.\d{1," + ItemDecimal + "})?$";
                        }
                    }
                    else
                    {
                        if (ItemDecimal == 0)
                        {
                            regText = @"^\d{1," + ItemInteger + @"}$";
                        }
                        else
                        {
                            regText = @"^\d{1," + ItemInteger + @"}(\.\d{1," + ItemDecimal + "})?$";
                        }
                    }
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regText);
                    if (!reg.IsMatch(str))
                    { return false; }
                    else
                    { return true; }
                }

            }
            return false;
        }

        private void Search()
        {
            string Where = " where YY_RTU_CONFIGDATA.STCD='" + comboBox_STCD.SelectedValue + "'";
            DataTable dt = PublicBD.db.GetRTU_CONFIGDATA(Where);

            //for (int i = 0; i < dt.Rows.Count ; i++)
            //{
            //    if (dt.Rows[i]["ItemID"].ToString() == "0000000000") 
            //    {
            //        dt.Rows[i]["ItemName"] = "RTU";
            //    }
            //}

            dataGridView1.DataSource = dt;
            for (int i = 0; i < 7; i++)
            {
                dataGridView1.Columns["Column" + (i + 1)].DisplayIndex = i;
            }
        }

        //对显示的数据进行处理（水资源专用）
        private DataTable SetDt(DataTable dt) 
        {
            if (dt != null && dt.Rows.Count > 0) 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ItemID"].ToString().Length ==10)
                    if (dt.Rows[i]["ItemID"].ToString().Substring(0, 8) == "00000000") 
                    {
                        dt.Rows[i]["ItemName"] = "水泵" + dt.Rows[i]["ItemID"].ToString().Substring(8, 2);
                    }
                    else if (dt.Rows[i]["ItemID"].ToString().Substring(0, 8) == "00001111")
                    {
                        dt.Rows[i]["ItemName"] = "阀门/闸门" + dt.Rows[i]["ItemID"].ToString().Substring(8, 2);
                    }
                    else if (dt.Rows[i]["ItemID"].ToString().Substring(0, 8) == "00002222")
                    {
                        dt.Rows[i]["ItemName"] = "水量" + dt.Rows[i]["ItemID"].ToString().Substring(8, 2);
                    }
                }
            }
            return dt;
        }

        private void SetConfigDataControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;

            dataGridViewStyle(dataGridView1);

            comboBox_STCD_Init();

            comboBox_ConfigItem_Init();

           comboBox_Item_Init();
        }


        #region DataGridView样式
        private void dataGridViewStyle(DataGridView DGV) 
        {
            DGV.BackgroundColor = this.ParentForm.BackColor;

            DGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DGV.RowHeadersBorderStyle =DataGridViewHeaderBorderStyle.Single;
            DGV.CellBorderStyle =DataGridViewCellBorderStyle.Single;
            DGV.EnableHeadersVisualStyles = false;
            DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            DGV.ColumnHeadersHeight = 25;

            DGV.GridColor = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Color c = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            int Alpha = 30;
            int R = 255 + (c.R - 255) * Alpha / 255;
            int G = 255 + (c.G - 255) * Alpha / 255;
            int B = 255 + (c.B - 255) * Alpha / 255;
            DGV.RowsDefaultCellStyle.BackColor = Color.White;
            DGV.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(R, G, B);
            DGV.CellMouseEnter +=new DataGridViewCellEventHandler(DGV_CellMouseEnter);
            DGV.CellMouseLeave +=new DataGridViewCellEventHandler(DGV_CellMouseLeave);
            DGV.Paint += new PaintEventHandler(DGV_Paint);
        }

        void DGV_Paint(object sender, PaintEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            Pen p = new Pen(((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor);
            e.Graphics.DrawRectangle(p, new Rectangle(0, 0, DGV.Width - 1, DGV.Height - 1));
        }
        void DGV_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            if (e.RowIndex >= 0)
            {
                DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = colorTmp;
            }
        }

        Color colorTmp = Color.White;
        void DGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            if (e.RowIndex >= 0)
            {
                colorTmp = DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor;
                DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            }
        }
        #endregion
       

        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            //Validate(textBox_Val.Text.Trim()) &&
            if ( comboBox_STCD.SelectedValue != null)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                model.STCD = comboBox_STCD.SelectedValue.ToString();
                model.ItemID = comboBox_Item.SelectedValue.ToString();
                model.ConfigID = comboBox_ConfigItem.SelectedValue.ToString();
                model.ConfigVal = textBox_Val.Text.Trim();

                PublicBD.db.DelRTU_ConfigData(" where STCD='" + model.STCD + "' and ItemID='" + model.ItemID + "' and ConfigID='" + model.ConfigID + "'");

                bool b=PublicBD.db.AddRTU_ConfigData(model);
                if (b)
                {
                    textBox_Val.Text = "";
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据添加成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Search();
                }
                else
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据添加失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex!=-1) 
            {
                string Where = "where stcd='" + dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString() + "' and ItemID='" + dataGridView1.Rows[e.RowIndex].Cells["Column7"].Value.ToString() + "' and  ConfigID='" + dataGridView1.Rows[e.RowIndex].Cells["Column8"].Value.ToString() + "'";
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    bool b = PublicBD.db.DelRTU_ConfigData(Where);
                    if (b)
                    {
                        Search();
                        DevComponents.DotNetBar.MessageBoxEx.Show("数据删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("数据删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
        }


        IList<Service.Model.YY_RTU_CONFIGITEM> CIL = null;
        private void comboBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Program.wrx.ReadDllXML().ToLower() == "hydrologicprotocol.dll" || Program.wrx.ReadDllXML().ToLower() == "protocol.dll")
            {
                string ItemID = "";
                comboBox_ConfigItem.DataSource = null;
                if (comboBox_Item.SelectedValue is Service.Model.YY_RTU_ITEM)
                {
                    ItemID = (comboBox_Item.SelectedValue as Service.Model.YY_RTU_ITEM).ItemID;
                }
                else
                {
                    ItemID = comboBox_Item.SelectedValue.ToString();
                }
                IList<Service.Model.YY_RTU_ITEMCONFIG> list = Service.PublicBD.db.GetRTU_ItemConfig(" where ItemID ='" + ItemID + "'");
                if (list.Count() > 0 && CIL != null && CIL.Count() > 0)
                {
                    var ConfigItem = from ci in CIL from l in list where l.ConfigID == ci.ConfigID select ci;
                    comboBox_ConfigItem.DataSource = ConfigItem.ToList<Service.Model.YY_RTU_CONFIGITEM>();
                    comboBox_ConfigItem.DisplayMember = "ConfigItem";
                    comboBox_ConfigItem.ValueMember = "ConfigID";
                    comboBox_ConfigItem.SelectedIndex = 0;

                    button_Set.Enabled = true;
                }
                else
                {
                    button_Set.Enabled = false;
                }
            }
        }

        
    }
}
