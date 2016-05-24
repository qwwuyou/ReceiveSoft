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
    public partial class _43 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _43(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();
            

            CB_Stcd_Init(Stcds);
            //CBL_Init();

            if (Stcds.Length > 1)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("只能设置单个测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = false;
            }
        }

        #region 初始化
         private void CB_Stcd_Init(string[] Stcds) 
        {
            List<Item> stcds = new List<Item>();
            for (int i = 0; i < Stcds.Length ; i++)
            {
                var rtu =from r in list where r.STCD ==Stcds[i] select r;
                if(rtu .Count ()>0)
                {
                    stcds.Add(new Item() { Key=rtu.First().STCD ,Value =rtu.First().NiceName });
                }
            }
            comboBox_stcd.DataSource = stcds;
            comboBox_stcd.DisplayMember = "Value";
            comboBox_stcd.ValueMember = "Key";

            comboBox_stcd.SelectedIndex = 0;
        }

         void CBL_Init()
         {
             List<Service.Model.YY_RTU_CONFIGITEM> list = Service.PublicBD.db.GetRTU_ConfigItemList(" where ConfigID like '1111________'").ToList<Service.Model.YY_RTU_CONFIGITEM>();
             cbl_Item.DataSource = list;
             cbl_Item.DisplayMember = "ConfigItem";
             cbl_Item.ValueMember = "ConfigID";
         }
         void CBL_Init(string Stcd) 
         {
             DataTable dt = Service.PublicBD.db.GetRTU_ConfigItem(Stcd);

             #region RTU自身非监测项运行参数
             DataRow dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "定时报时间间隔", Stcd, "0000000000", "110000000020" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "加报时间间隔", Stcd, "0000000000", "110000000021" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "降水量日起始时间", Stcd, "0000000000", "110000000022" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "采样间隔", Stcd, "0000000000", "110000000023" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "水位数据存储间隔", Stcd, "0000000000", "110000000024" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "雨量计分辨力", Stcd, "0000000000", "110000000025" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "水位计分辨力", Stcd, "0000000000", "110000000026" };
             dt.Rows.Add(dr);
             dr = dt.NewRow();
             dr.ItemArray = new object[] { "RTU", "最低控制水位(黑)", Stcd, "0000000000", "11000000FF03" };
             dt.Rows.Add(dr);
             #endregion
             cbl_Item.DataSource = dt;
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 dt.Rows[i]["ConfigItem"]=dt.Rows[i]["ConfigItem"]+"--"+dt.Rows[i]["ItemName"];
             }
             cbl_Item.DisplayMember = "ConfigItem";
             cbl_Item.ValueMember = "ConfigID";
         }
        #endregion

         private List<string> Validate()
         {
             List<string> flags = new List<string>();
             for (int i = 0; i < cbl_Item.Items.Count; i++)
             {
                 if (cbl_Item.GetItemChecked(i)) 
                 {
                     cbl_Item.SetSelected(i, true);
                     if (cbl_Item.SelectedValue.ToString().Length == 12)
                     {
                         flags.Add(cbl_Item.SelectedValue.ToString().Substring(10,2));
                     }
                 }
             }
             return flags;
         }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
         {
            CommandCode = "43";
            List<string> flags = Validate();
            if (flags.Count == 0)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("至少选择1个监测项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            
            string STCD = comboBox_stcd.SelectedValue.ToString();
            string[] commands = new string[1];
            var model = from rtu in list where rtu.STCD == STCD select rtu;
            if (model.Count() > 0)
            {
                Package package = Package.Create_0x43Package(STCD, 1, UInt16.Parse(model.First().PassWord), flags);
                commands[0] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());

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

        private void comboBox_stcd_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Stcd = (comboBox_stcd.SelectedItem as Item).Key ;
            CBL_Init(Stcd);
        }
    }
}
