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
    public partial class _3A : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _3A()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            CBL_Init();
        }

        void CBL_Init()
        {
            List<Service.Model.YY_RTU_ITEM> list = Service.PublicBD.db.GetItemList(" where ItemID!='0000000000'").ToList<Service.Model.YY_RTU_ITEM>();
            cbl_Item.DataSource = list;
            cbl_Item.DisplayMember = "ItemName";
            cbl_Item.ValueMember = "ItemID";
        }

        private List<UInt16> Validate()
        {
            List<UInt16>  flags = new List<UInt16>();
            int count = 0;
            for (int i = 0; i < cbl_Item.Items.Count; i++)
            {
                if (cbl_Item.GetItemChecked(i))
                {
                    count++;
                    string ItemID=(cbl_Item.Items[i] as Service.Model.YY_RTU_ITEM).ItemID;
                    UInt16 flag = ByteHelper.HexStrToByte(ItemID).Length > 1 ? BitConverter.ToUInt16(ByteHelper.HexStrToByte(ItemID), 0) : ByteHelper.HexStrToByte(ItemID)[0];
                    flags.Add(flag);
                }
            }
            if (count == 0)
            {
                return null;
            }

            return flags;
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
                    DevComponents.DotNetBar.MessageBoxEx.Show("最多能选择5个监测项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "3A";
            List<UInt16> flags = Validate();
            if (flags == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("至少选择1个监测项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

           

            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                if (list != null && list.Count > 0)
                {
                    var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                    if (model.Count() > 0)
                    {
                        Package package = Package.Create_0x3APackage(Stcds[i], 1, UInt16.Parse(model.First().PassWord),flags);
                        commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());

                    }
                }
            }

            return commands;
        }


    }
}
