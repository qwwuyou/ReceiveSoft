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
    public partial class _41 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _41()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            CBL_Init();
        }

        private void CBL_Init() 
        {
            List<Item> ItemList = new List<Item>();
            ItemList.Add(new Item() {Key ="01",Value ="中心站地址" });
            ItemList.Add(new Item() { Key = "02", Value = "遥测站地址" });
            ItemList.Add(new Item() { Key = "03", Value = "密码" });
            ItemList.Add(new Item() { Key = "04", Value = "中心站1主信道类型及地址" });
            ItemList.Add(new Item() { Key = "05", Value = "中心站1备用信道类型及地址" });
            ItemList.Add(new Item() { Key = "06", Value = "中心站2主信道类型及地址" });
            ItemList.Add(new Item() { Key = "07", Value = "中心站2备用信道类型及地址" });
            ItemList.Add(new Item() { Key = "08", Value = "中心站3主信道类型及地址" });
            ItemList.Add(new Item() { Key = "09", Value = "中心站3备用信道类型及地址" });
            ItemList.Add(new Item() { Key = "0A", Value = "中心站4主信道类型及地址" });
            ItemList.Add(new Item() { Key = "0B", Value = "中心站4备用信道类型及地址" });
            ItemList.Add(new Item() { Key = "0C", Value = "工作方式" });
            ItemList.Add(new Item() { Key = "0D", Value = "遥测站采集要素设置" });
            ItemList.Add(new Item() { Key = "0E", Value = "中继站（集合转发站）服务地址范围" });
            ItemList.Add(new Item() { Key = "0F", Value = "遥测站通信设备识别号" });

            cbl_Item.DataSource = ItemList;
            cbl_Item.DisplayMember = "Value";
            cbl_Item.ValueMember = "Key";
        }
        private List<string> Validate()
        {
            List<string> flags = new List<string>();
            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                if (cbl_Item.GetItemChecked(i))
                {
                    cbl_Item.SetSelected(i, true);
                    flags.Add(cbl_Item.SelectedValue.ToString());
                }
            }
            return flags;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "41";
            string[] commands = new string[Stcds.Length];

             List<string> flags = Validate();


            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        Package package = Package.Create_0x41Package(Stcds[i], 1, UInt16.Parse(model.First().PassWord), flags);
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());

                    }
                }
            }

            return commands;
        }

        private void cbl_Item_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                int count = 0;
                for (int i = 0; i < cbl_Item.Items.Count; i++)
                {
                    if (cbl_Item.GetItemChecked(i))
                    {
                        count++;
                    }
                }

                if (count == 5)
                {
                    e.NewValue = e.CurrentValue;
                    DevComponents.DotNetBar.MessageBoxEx.Show("最多能选择5个选择项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    class Item 
    {
        private string _key;
        private string _value;
        public string Key
        {
            set { _key = value; }
            get { return _key; }
        }
        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
    }
}
