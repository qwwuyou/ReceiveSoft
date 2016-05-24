using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;

namespace ConfigRationTool
{
    public partial class ConfigRationForm : Form
    {
        public ConfigRationForm()
        {
            InitializeComponent();
        }

        private void ConfigRationForm_Load(object sender, EventArgs e)
        {
            comboBox_db.SelectedIndex = 0;
        }

        private void Conn_button_Click(object sender, EventArgs e)
        {
            PublicBD.DB = comboBox_db.SelectedItem.ToString();
            if (PublicBD.ConnectState)
            {
                Conn_label.Text = "成功！";
                Conn_label.ForeColor = Color.Green;
                checkedListBox_config_Init();
                comboBox_Item_Init();
            }
            else
            {
                Conn_label.Text = "失败！";
                Conn_label.ForeColor = Color.Red;
            }
        }

        private void comboBox_Item_Init() 
        {
            IList<Service.Model.YY_RTU_ITEM> list=  PublicBD.db.GetItemList("");

            foreach (var item in list)
            {
                ComboxItem cbi = new ComboxItem();
                cbi.ID = item.ItemID;
                cbi.Name = item.ItemName;
                comboBox_Item.Items.Add(cbi);
            }
            comboBox_Item.DisplayMember = "Name";
            comboBox_Item.ValueMember = "id";

            comboBox_Item.SelectedIndex = 0;
        }

        private void checkedListBox_config_Init() 
        {
            IList<Service.Model.YY_RTU_CONFIGITEM> list = PublicBD.db.GetRTU_ConfigItemList("");
            foreach (var item in list)
            { 
                ComboxItem cbi = new ComboxItem();
                cbi.ID = item.ConfigID;
                cbi.Name = item.ConfigItem;
                checkedListBox_config.Items.Add(cbi);
            }
            checkedListBox_config.DisplayMember = "Name";
            checkedListBox_config.ValueMember = "id";
        }

        private void comboBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();

            string ItemID =( comboBox_Item.SelectedItem as ComboxItem ).ID  ;
            IList<Service.Model.YY_RTU_ITEMCONFIG> list = PublicBD.db.GetRTU_ItemConfig(" where ItemID='" + ItemID + "'");

              

            foreach (var item in list)
            {
                for (int i = 0; i < checkedListBox_config.Items.Count; i++)
                {
                    if ((checkedListBox_config.Items[i] as ComboxItem).ID == item.ConfigID)
                    {
                        checkedListBox_config.SelectedItem=checkedListBox_config.Items[i];
                        checkedListBox_config.SetItemChecked(i, true);
                        break;
                    }
                }
            }
            
        }

        //去掉所有选择
        void ClearAll() 
        {
            
            for (int i = 0; i < checkedListBox_config.Items.Count; i++)
            {
                checkedListBox_config.SetItemChecked(i, false);
            }
        }

        private void Set_button_Click(object sender, EventArgs e)
        {
            string ItemID = (comboBox_Item.SelectedItem as ComboxItem).ID;
            bool b = PublicBD.db.DelRTU_ItemConfig(" where ItemID='" + ItemID + "'");
            if(b)
            {
                for (int j = 0; j < checkedListBox_config.Items.Count; j++)
                {
                    if (checkedListBox_config.GetItemChecked(j))
                    {
                        checkedListBox_config.SetSelected(j, true);
                        string ConfigID = (checkedListBox_config.SelectedItem as ComboxItem).ID;
                        Service.Model.YY_RTU_ITEMCONFIG model = new Service.Model.YY_RTU_ITEMCONFIG();
                        model.ItemID = ItemID;
                        model.ConfigID = ConfigID;
                        PublicBD.db.AddRTU_ItemConfig(model);
                    }
                }

                MessageBox.Show("配置成功！");
            }
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" 确认删除监测项的相关配置项关系？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ItemID = (comboBox_Item.SelectedItem as ComboxItem).ID;
                bool b=PublicBD.db.DelRTU_ItemConfig(" where ItemID='" + ItemID + "'");
                if(b)
                ClearAll();
            }
        }

        private void button_DelAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" 确认删除所有监测项关系？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool b = PublicBD.db.DelRTU_ItemConfig("");
                if(b)
                ClearAll();
            }
        }
    }

    public class ComboxItem
    {
        private string _id = "";
        private string _Name = "";

        public string ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
    }
}
