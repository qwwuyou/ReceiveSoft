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
    public partial class _00 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _00(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill; 
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            CBL_Init(Stcds);
        }

        void CBL_Init(string[] Stcds)
        {
            cbl_Item.DataSource = list;
            cbl_Item.DisplayMember = "NiceName";
            cbl_Item.ValueMember = "STCD";

            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                cbl_Item.SetItemChecked(i, false);
                foreach (var item in Stcds)
                {
                    if ((cbl_Item.Items[i] as Service.Model.YY_RTU_Basic).STCD == item)
                    { cbl_Item.SetItemChecked(i, true); }
                }
            }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "00";
            DevComponents.DotNetBar.MessageBoxEx.Show("召测按钮无作用！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void BTselectAll_Click(object sender, EventArgs e)
        {
            if (BTselectAll.Text == "全 选")
            {
                for (int i = 0; i < cbl_Item.Items.Count; i++)
                {
                    cbl_Item.SetItemChecked(i, true);
                }
                BTselectAll.Text = "取消全选";
            }
            else
            {
                for (int i = 0; i < cbl_Item.Items.Count; i++)
                {
                    cbl_Item.SetItemChecked(i, false);
                }
                BTselectAll.Text = "全 选";
            }
        }

        private void BTReadDB_Click(object sender, EventArgs e)
        {
            List<Service.Model.YY_RTU_CONFIGDATA> list1 = Service.PublicBD.db.GetRTU_CONFIGDATAList(" where ItemID ='0000000000' and ConfigID='110000000000'").ToList<Service.Model.YY_RTU_CONFIGDATA>();
            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                cbl_Item.SetItemChecked(i, false);
                if(list1.Count()>0)
                foreach (var item in list1)
                {
                    if ((cbl_Item.Items[i] as Service.Model.YY_RTU_Basic).STCD == item.STCD)
                    { cbl_Item.SetItemChecked(i, true); }
                }
            }
        }

        private bool Insert00()    // ConfigID 110000000000     ItemID 0000000000
        {
            string where = " where ItemID='0000000000' and ConfigID='110000000000'";
            bool b=Service.PublicBD.db.DelRTU_ConfigData(where);
            if (b) 
            {
                for (int i = 0; i < cbl_Item.Items.Count; i++)
                {
                    if (cbl_Item.GetItemChecked(i))
                    {
                        cbl_Item.SetSelected(i, true);
                        string STCD = cbl_Item.SelectedValue.ToString();
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        model.STCD = STCD;
                        model.ItemID = "0000000000";
                        model.ConfigID = "110000000000";

                        b = Service.PublicBD.db.AddRTU_ConfigData(model);
                    }
                }
            }

            return b;
        }

        private void BTWriteAndReset_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                if (Insert00())
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据写入数据库成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TcpControl.SendUItoServiceCommand("--rtu|");
                }
            }
            else 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重置信息！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
