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
    public partial class _42 : UserControl, ICommandControl
    {
        List<Service.Model.YY_RTU_Basic> list;
        public _42(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            list = Service.PublicBD.db.GetRTUList("").ToList<Service.Model.YY_RTU_Basic>();

            CB_Stcd_Init(Stcds); 
            
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
            for (int i = 0; i < Stcds.Length; i++)
            {
                var rtu = from r in list where r.STCD == Stcds[i] select r;
                if (rtu.Count() > 0)
                {
                    stcds.Add(new Item() { Key = rtu.First().STCD, Value = rtu.First().NiceName });
                }
            }
            comboBox_stcd.DataSource = stcds;
            comboBox_stcd.DisplayMember = "Value";
            comboBox_stcd.ValueMember = "Key";

            comboBox_stcd.SelectedIndex = 0;
        }

        private void DGV_Init(string Stcd)
        {
            string where = " or ConfigID='110000000020' or ConfigID='110000000021' or ConfigID='110000000022' or ConfigID='110000000023' or ConfigID='110000000024' or ConfigID='110000000025' or ConfigID='110000000026' or ConfigID='11000000FF03' ";

            List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(" where STCD='" + Stcd + "' and ConfigID like '1111________'" + where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
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

            dt.Columns.Add(new DataColumn("ConfigVal"));
            for (int i = 0; i < dt.Rows.Count ; i++)
            {
                var val = from c in list where c.STCD == Stcd && c.ItemID == dt.Rows[i]["ItemID"].ToString() && c.ConfigID == dt.Rows[i]["ConfigID"].ToString() select c;
                if (val.Count() > 0) 
                {
                    dt.Rows[i]["ConfigVal"] = val.First().ConfigVal;
                }
            }
            //dataGridView_Config.AutoGenerateColumns = false;//不允许自动创建列
            dataGridView_Config.DataSource = dt;
        }
        #endregion

        public string  Validate(out Dictionary<string ,string >  DIC) 
        {
            bool B = false;
            string msg = "";
            Dictionary<string ,string > dic=new Dictionary<string,string>();
            foreach (DataGridViewRow row in dataGridView_Config.Rows)
            {
                
                bool b = Convert.ToBoolean(row.Cells[0].Value);
                if (b) 
                {
                    if (row.Cells[6].Value.ToString() != "")
                    {
                        if (row.Cells["Column6"].Value.ToString().Substring(8, 2) == "FF") //FF为扩展功能码
                        {
                            dic.Add("FF" + row.Cells["Column6"].Value.ToString().Substring(10, 2), row.Cells["Column4"].Value.ToString());
                        }
                        else
                        {
                            dic.Add(row.Cells["Column6"].Value.ToString().Substring(10, 2), row.Cells["Column4"].Value.ToString());
                        }
                    }
                    else 
                    {
                        msg = "选择的配置项值不能为空！";
                    }

                    B = true;
                }
              
            }
            DIC = dic;
            if (!B) 
            {
                msg = "至少选择一个设置项！";
            }
            return msg;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            CommandCode = "42";
            string[] commands = new string[Stcds.Length];

            Dictionary<string, string> Dic = new Dictionary<string, string>();

            string msg = Validate(out Dic);
            if (msg != "")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show(msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else
            {

                for (int i = 0; i < Stcds.Length; i++)
                {
                    if (list != null && list.Count > 0)
                    {
                        var model = from rtu in list where rtu.STCD == Stcds[i] select rtu;
                        if (model.Count() > 0)
                        {
                            Package package = Package.Create_0x42Package(Stcds[i], 1, UInt16.Parse(model.First().PassWord), Dic);
                            commands[i] = ByteHelper.ByteToHexStr(package.GetFrames()[1].ToBytes());

                        }
                    }
                }
            }
            return commands;
        }

        private void comboBox_stcd_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Stcd = (comboBox_stcd.SelectedItem as Item).Key;
            DGV_Init(Stcd);
        }
    }
}
