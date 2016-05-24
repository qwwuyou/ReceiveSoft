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
    public partial class _38 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _38()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            CB_Init();
            CBL_Init();
        }

        #region  //初始化
        void CB_Init()
        {
            for (int i = 1; i < 31; i++)
            {
                cb_day.Items.Add(i);
            }
            for (int i = 1; i < 25; i++)
            {
                cb_Hour.Items.Add(i);
            }

            for (int i = 1; i < 61; i++)
            {
                cb_Minute.Items.Add(i);
            }
            cb_day.SelectedIndex = 0;
            cb_Hour.SelectedIndex = 0;
            cb_Minute.SelectedIndex = 0;
            rb_Hour.Checked = true;
        }
        void CBL_Init()
        {
            List<Service.Model.YY_RTU_ITEM> list = Service.PublicBD.db.GetItemList(" where ItemCode!='0000000000'").ToList<Service.Model.YY_RTU_ITEM>();
            cbl_Item.DataSource = list;
            cbl_Item.DisplayMember = "ItemName";
            cbl_Item.ValueMember = "ItemID";
        }
        private void rb_Day_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Day.Checked)
            {
                cb_day.Enabled = true;
                cb_Hour.Enabled = false;
                cb_Minute.Enabled = false;
            }
        }

        private void rb_Hour_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Hour.Checked)
            {
                cb_Hour.Enabled = true;
                cb_day.Enabled = false;
                cb_Minute.Enabled = false;
            }
        }

        private void rb_Minute_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Minute.Checked)
            {
                cb_Minute.Enabled = true;
                cb_day.Enabled = false;
                cb_Hour.Enabled = false;
            }
        }

        private void cbl_Item_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                if (e.Index != i)
                    cbl_Item.SetItemChecked(i, false);
            }
        }
        #endregion

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "38";
            string sjy = Validate();
            if (sjy == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("输入开始或结束时间有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else if (sjy == "0")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择1个监测项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            uint Minute = 0;
            if (cb_day.Enabled)
            {
                Minute = uint.Parse(cb_day.SelectedItem.ToString()) * 24 * 60;
            }
            else if (cb_Hour.Enabled)
            {
                Minute = uint.Parse(cb_Hour.SelectedItem.ToString()) * 60;
            }
            else
            {
                Minute = uint.Parse(cb_Minute.SelectedItem.ToString());
            }

            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        UInt16 flag = ByteHelper.HexStrToByte(ItemID).Length > 1 ? BitConverter.ToUInt16(ByteHelper.HexStrToByte(ItemID), 0) : ByteHelper.HexStrToByte(ItemID)[0];
                        Package package = Package.Create_0x38Package(Stcds[i], 1, UInt16.Parse(model.First().PassWord), dateTimePicker1.Value, dateTimePicker2.Value, Minute, flag);
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());

                    }
                }
            }

            return commands;
        }

        string ItemID = "";
        private string Validate()
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
                return null;

            int count = 0;
            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                if (cbl_Item.GetItemChecked(i))
                {
                    count++;
                    ItemID = (cbl_Item.Items[i] as Service.Model.YY_RTU_ITEM).ItemID;
                }
            }
            if (count == 0)
            {
                ItemID = "";
                return "0";
            }

            return "";
        }
    }
}
