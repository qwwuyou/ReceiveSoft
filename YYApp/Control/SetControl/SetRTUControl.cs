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
    public partial class SetRTUControl : UserControl
    {
        public SetRTUControl()
        {
            InitializeComponent();
        }

        private void SetRTUControl_Load(object sender, EventArgs e)
        {

            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            checkedListBox_Item_Init();
            checkedListBox_RTU_Init();

            comboBox_ItemDecimal.SelectedIndex = 0;
            comboBox_ItemInteger.SelectedIndex = 0;
        }

        #region 初始化checkedListBox
        private void checkedListBox_Item_Init() 
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList("where ItemCode!='-1' and ItemCode!='0000000000'");
            checkedListBox_Item.DataSource = ItemList;
            checkedListBox_Item.DisplayMember = "ItemName";
            checkedListBox_Item.ValueMember = "ItemID";

            for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
            {
                checkedListBox_Item.SetItemChecked(i, false);
            }
        }

        private void checkedListBox_RTU_Init() 
        {
            IList<Service.Model.YY_RTU_Basic> RTUList = PublicBD.db.GetRTUList("");
            checkedListBox_RTU.DataSource = RTUList;
            checkedListBox_RTU.DisplayMember = "NiceName";
            checkedListBox_RTU.ValueMember = "STCD";

            for (int i = 0; i < checkedListBox_RTU.Items.Count; i++)
            {
                checkedListBox_RTU.SetItemChecked(i, false);
            }
        }
        #endregion

        #region 验证
        private bool ValidateCheckedListBox( CheckedListBox  clb) 
        {
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (clb.GetItemChecked(i))
                {
                    return true; 
                }
            }
            return false;
        }

        private bool ValidateRTU() 
        {
            if (textBox_STCD.Text.Trim() == "" || textBox_RTUName.Text.Trim() == "" || textBox_RTUPWD.Text.Trim() == "")
                return false;
            return true;
        }

        private bool ValidateItem() 
        {
            if (textBox_ItemID.Text.Trim() == "" || textBox_ItemName.Text.Trim() == "" || textBox_ItemCode.Text.Trim() == "")
                return false;
            return true;
        }
        #endregion

        /// <summary>
        /// 得到多选测站字符串
        /// </summary>
        /// <returns></returns>
        private string GetSTCDstr() 
        {
            string checkedValue = string.Empty;
            for (int i = 0; i < checkedListBox_RTU.Items.Count; i++)
            {
                if (checkedListBox_RTU.GetItemChecked(i))
                {
                    checkedListBox_RTU.SetSelected(i, true);
                    checkedValue += (String.IsNullOrEmpty(checkedValue) ? "" : ",") + checkedListBox_RTU.SelectedValue.ToString();  
                }
            }

            return "('"+checkedValue.Replace(",","','")+"')";
        }

        /// <summary>
        /// 得到多选监测项字符串
        /// </summary>
        /// <returns></returns>
        private string GetItemstr()
        {
            string checkedValue = string.Empty;
            for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
            {
                if (checkedListBox_Item.GetItemChecked(i))
                {
                    checkedListBox_Item.SetSelected(i, true);
                    checkedValue += (String.IsNullOrEmpty(checkedValue) ? "" : ",") + checkedListBox_Item.SelectedValue.ToString();
                }
            }

            return "('" + checkedValue.Replace(",", "','") + "')";
        }


        /// <summary>
        /// 添加测站信息的业务逻辑
        /// </summary>
        private void AddRTU() 
        {
            if (!ValidateRTU())//验证
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请填写完整测站信息！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Service.Model.YY_RTU_Basic model = new Service.Model.YY_RTU_Basic();
                model.STCD = textBox_STCD.Text.Trim();
                model.PassWord = textBox_RTUPWD.Text.Trim();
                model.NiceName = textBox_RTUName.Text.Trim();
                string Where = " where STCD='" + textBox_STCD.Text.Trim() + "'";
                IList<Service.Model.YY_RTU_Basic> RTUList = PublicBD.db.GetRTUList(Where); //查重
                if (RTUList != null)
                {
                    if (RTUList.Count() > 0)
                    {
                        PublicBD.db.UpdRTU(model, Where); //更新
                    }
                    else
                    {
                        PublicBD.db.AddRTU(model);     //添加 
                    }
                }
                checkedListBox_RTU_Init();  //更新控件 
            }
        }

        /// <summary>
        /// 添加监测项信息的业务逻辑
        /// </summary>
        private void AddItem() 
        {
            if (!ValidateItem())//验证
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请填写完整监测项信息！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Service.Model.YY_RTU_ITEM model = new Service.Model.YY_RTU_ITEM();
                model.ItemID = textBox_ItemID.Text.Trim();
                model.ItemName = textBox_ItemName.Text.Trim();
                model.ItemCode = textBox_ItemCode.Text.Trim();
                model.Units = textBox_ItemUnites.Text.Trim();
                if (comboBox_ItemDecimal.SelectedItem.ToString() == "其他")
                { model.ItemDecimal = -1; }
                else
                {
                    model.ItemDecimal = int.Parse(comboBox_ItemDecimal.SelectedItem.ToString());
                }
                if (comboBox_ItemInteger.SelectedItem.ToString() == "其他") 
                {
                    model.ItemInteger = -1;
                }
                else
                {
                    model.ItemInteger = int.Parse(comboBox_ItemInteger.SelectedItem.ToString());
                }
                model.PlusOrMinus = checkBox_ItemPlusOrMinus.Checked;
                string Where = " where ItemID='" + textBox_ItemID.Text.Trim() + "'";
                IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(Where); //查重
                if (ItemList.Count() > 0)
                {
                    PublicBD.db.UdpItem(model, Where); //更新
                }
                else
                {
                    PublicBD.db.AddItem(model);     //添加 
                }
                checkedListBox_Item_Init();  //更新控件 
            }
        }

        /// <summary>
        /// 测站与监测项关系入库
        /// </summary>
        private void AddRTU_Item() 
        {
            Service.Model.YY_RTU_BI model = null;
            for (int i = 0; i < checkedListBox_RTU.Items.Count; i++)
            {
                if (checkedListBox_RTU.GetItemChecked(i))
                {
                    checkedListBox_RTU.SetSelected(i, true);
                    string STCD=checkedListBox_RTU.SelectedValue.ToString();
                    for (int j = 0; j < checkedListBox_Item.Items.Count; j++)
                    {
                        if (checkedListBox_Item.GetItemChecked(j))
                        {
                            checkedListBox_Item.SetSelected(j, true);
                            string item = checkedListBox_Item.SelectedValue.ToString();
                            model = new Service.Model.YY_RTU_BI();
                            model.STCD = STCD;
                            model.ItemID = item;
                            PublicBD.db.AddRTU_Item(model);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置测站与监测项关系的业务逻辑
        /// </summary>
        private void SetRTU_Item() 
        {
            if (!ValidateCheckedListBox(checkedListBox_RTU)) 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择要配置的测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!ValidateCheckedListBox(checkedListBox_Item))
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请为测站选择监测项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else 
            {
                //先删除原有内容
                PublicBD.db.DelRTU_Item("STCD in "+GetSTCDstr());
                //再添加最新关系信息
                AddRTU_Item();

                DevComponents.DotNetBar.MessageBoxEx.Show("测站与监测项配置成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 删除测站信息（先删除测站与监测项关系，在删除测站信息）
        /// </summary>
        private void DelRTU() 
        {
            string where = " Where STCD in " + GetSTCDstr();
            PublicBD.db.DelRTU_ConfigData(where);
            PublicBD.db.DelRTU_Work(where);
            PublicBD.db.DelRTU_WRES(where);
            PublicBD.db.DelRTU_Item(where);
            PublicBD.db.DelRTU(where);

            checkedListBox_RTU_Init();
        }

        /// <summary>
        /// 删除监测项信息（先删除测站与监测项关系，在删除监测项信息）
        /// </summary>
        private void DelItem() 
        {
            PublicBD.db.DelRTU_Item(" Where ItemID in "+ GetItemstr());
            PublicBD.db.DelItem(" Where ItemID in " + GetItemstr());
            checkedListBox_Item_Init();
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            SetRTU_Item();
        }

        private void button_RTUAdd_Click(object sender, EventArgs e)
        {
            AddRTU();
        }

        private void button_ItemAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        
        private void button_RTUCheckAll_Click(object sender, EventArgs e)
        {
            
                if (button_RTUCheckAll.Text == "全 选")
                {
                    for (int i = 0; i < checkedListBox_RTU.Items.Count; i++)
                    {
                        checkedListBox_RTU.SetItemChecked(i, true);
                    }
                    button_RTUCheckAll.Text = "取消全选";
                }
                else 
                {
                    for (int i = 0; i < checkedListBox_RTU.Items.Count; i++)
                    {
                        checkedListBox_RTU.SetItemChecked(i, false);
                    }
                    button_RTUCheckAll.Text = "全 选";
                }
            
        }

        private void button_ItemCheckAll_Click(object sender, EventArgs e)
        {
            if (button_ItemCheckAll.Text == "全 选")
            {
                for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
                {
                    checkedListBox_Item.SetItemChecked(i, true);
                }
                button_ItemCheckAll.Text = "取消全选";
            }
            else
            {
                for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
                {
                    checkedListBox_Item.SetItemChecked(i, false);
                }
                button_ItemCheckAll.Text = "全 选";
            }
        }

        private void button_RTUDel_Click(object sender, EventArgs e)
        {
            if (DevComponents.DotNetBar.MessageBoxEx.Show(" 确认删除测站及相关配置监测项？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DelRTU();
                for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
                {
                    checkedListBox_Item.SetItemChecked(i, false);
                }
            }
        }

        private void button_ItemDel_Click(object sender, EventArgs e)
        {
            if (DevComponents.DotNetBar.MessageBoxEx.Show(" 确认删除监测项？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DelItem();
            }
        }

        private void checkedListBox_RTU_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Where = " where STCD='" + checkedListBox_RTU.SelectedValue.ToString() + "'";
            IList<Service.Model.YY_RTU_Basic> RTUList = PublicBD.db.GetRTUList(Where);
            if (RTUList!=null &&　RTUList.Count() > 0) 
            {
                textBox_RTUName.Text  = RTUList.First().NiceName;
                textBox_RTUPWD.Text = RTUList.First().PassWord;
                textBox_STCD.Text = RTUList.First().STCD;
            }

        }

        private void checkedListBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Where = " where ItemID='" + checkedListBox_Item.SelectedValue.ToString() + "'";
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(Where);
            if (ItemList.Count() > 0)
            { 
                textBox_ItemID.Text = ItemList.First().ItemID;
                textBox_ItemName.Text = ItemList.First().ItemName;
                textBox_ItemCode.Text = ItemList.First().ItemCode;
                textBox_ItemUnites.Text = ItemList.First().Units;
                //comboBox_ItemDecimal.SelectedIndex = ItemList.First().ItemDecimal;
                //comboBox_ItemInteger.SelectedIndex = ItemList.First().ItemInteger-1;

                if (ItemList.First().ItemDecimal == -1)
                { comboBox_ItemDecimal.SelectedIndex = comboBox_ItemDecimal.Items.Count - 1; }
                else
                { comboBox_ItemDecimal.SelectedIndex = ItemList.First().ItemDecimal; }

                if (ItemList.First().ItemInteger == -1)
                {
                    comboBox_ItemInteger.SelectedIndex = comboBox_ItemInteger.Items.Count - 1;
                }
                else
                {
                    comboBox_ItemInteger.SelectedIndex = ItemList.First().ItemInteger - 1;
                }

                checkBox_ItemPlusOrMinus.Checked = ItemList.First().PlusOrMinus;
            }
        }

        private void checkedListBox_RTU_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                IList<Service.Model.YY_RTU_BI> RTU_BIList = PublicBD.db.GetRTU_BIList(" where STCD ='" + checkedListBox_RTU.SelectedValue.ToString() + "'");
                if (RTU_BIList!=null)
                for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
                {
                    checkedListBox_Item.SetItemChecked(i, false );
                    if (checkedListBox_Item.Items[i] is Service.Model.YY_RTU_ITEM)
                    {
                        foreach (var item in RTU_BIList)
                        {
                            if ((checkedListBox_Item.Items[i] as Service.Model.YY_RTU_ITEM).ItemID == item.ItemID)
                            { checkedListBox_Item.SetItemChecked(i, true); }
                        }
                    }
                }



            }
            else 
            {
                for (int i = 0; i < checkedListBox_Item.Items.Count; i++)
                { checkedListBox_Item.SetItemChecked(i, false); }

                button_RTUCheckAll.Text = "全 选";
            }
        }

        private void button_ReBootService_Click(object sender, EventArgs e)
        {
            ((MainForm)this.ParentForm).buttonItem_ReBootService_Click(null,null);
        }

        private void checkedListBox_Item_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Unchecked)
            { button_ItemCheckAll.Text = "全 选"; }
        }

       

        
    }
}
