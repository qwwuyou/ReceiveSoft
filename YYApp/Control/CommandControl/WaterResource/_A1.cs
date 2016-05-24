using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;

namespace YYApp.CommandControl
{
    public partial class _A1 : UserControl, ICommandControl
    {
        string[] Stcds = null;
        public _A1(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Stcds = Stcds;
        }

        private void _A1_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;

            string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "报警或状态", "水压", "备用1", "备用2" };
            string Where = " where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID='1200000000A1'";
            IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList(Where);


            dataGridViewStyle(dataGridView1);

            int i = 0;
            DataGridViewRow dvr;

            if (CONFIGDATAList.Count > 0) 
            {
                string[] ItemGroup=CONFIGDATAList.First().ConfigVal.Split(new char[] { ',' });
                if (ItemGroup.Length ==16)
                    for (int j = 0; j < ItemGroup.Length; j++)
                    {
                        string[] temp = ItemGroup[j].Split(new char[] { ':' });
                        i = this.dataGridView1.Rows.Add();
                        dvr = this.dataGridView1.Rows[i];
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
            else if (CONFIGDATAList.Count == 0)
            {

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D0
                dvr.Cells[1].Value = "雨量";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D1
                dvr.Cells[1].Value = "水位";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D2
                dvr.Cells[1].Value = "流量(水量)";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D3
                dvr.Cells[1].Value = "流速";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D4
                dvr.Cells[1].Value = "闸位";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D5
                dvr.Cells[1].Value = "功率";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D6
                dvr.Cells[1].Value = "气压";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D7
                dvr.Cells[1].Value = "风速(风向)";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D8
                dvr.Cells[1].Value = "水温";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D9
                dvr.Cells[1].Value = "水质";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D10
                dvr.Cells[1].Value = "土壤含水率";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D11
                dvr.Cells[1].Value = "蒸发量";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D12
                dvr.Cells[1].Value = "报警或状态";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D13
                dvr.Cells[1].Value = "水压";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D14
                dvr.Cells[1].Value = "备用1";
                dvr.Cells[2].Value = "";

                i = this.dataGridView1.Rows.Add();
                dvr = this.dataGridView1.Rows[i];
                dvr.Cells[0].Value = false;
                //D15
                dvr.Cells[1].Value = "备用2";
                dvr.Cells[2].Value = "";
            }
        }

        #region DataGridView样式
        private void dataGridViewStyle(DataGridView DGV)
        {
            DGV.BackgroundColor = this.ParentForm.BackColor;

            DGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DGV.CellBorderStyle = DataGridViewCellBorderStyle.Single;
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
            DGV.CellMouseEnter += new DataGridViewCellEventHandler(DGV_CellMouseEnter);
            DGV.CellMouseLeave += new DataGridViewCellEventHandler(DGV_CellMouseLeave);
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

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;
            if (rb6.Checked)
            {
                int gnm = 0x53;
                CommandCode = "53";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, "", int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            else
            {
                int gnm = 0xA1;
                CommandCode = "A1";
                string sjy = Validate();
                if (sjy == null)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("时间间隔输入有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }
            return commands;
        }

        /// <summary>
        /// byte[] 转 Hex字符串
        /// </summary>
        /// <param name="Data">byte数组</param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] Data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Data.Length; i++)
            {
                sb.Append(Data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private string Validate()
        {
            string str0 = string.Empty;
            string strTemp = string.Empty;

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow dvr = dataGridView1.Rows[i];
                strTemp += Convert.ToBoolean(dvr.Cells[0].Value) ? "1" : "0";
                if (dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[2].Value != null)
                {
                    int val = 0;
                    if (dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[2].Value.ToString()==""||int.TryParse(dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[2].Value.ToString(), out val)) 
                    {
                        if (val < 0 || val > 9999)
                        {return null; }
                        bool Temp_b=true;
                        if (bool.TryParse(dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[0].Value.ToString(), out Temp_b))
                        {
                            if (Temp_b&&val < 1) 
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }

                    bool Temp_B = true;
                    if (bool.TryParse(dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[0].Value.ToString(), out Temp_B))
                    {
                        if (Temp_B)
                        {
                            str0 += string.IsNullOrEmpty(dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[2].Value.ToString()) ? ",0" : "," + dataGridView1.Rows[dataGridView1.Rows.Count - (i + 1)].Cells[2].Value.ToString();
                        }
                        else { str0 += ",0"; }
                    }
                }
                else
                {
                    str0 += ",0";
                }

            }
            strTemp = "00" + strTemp;
            byte[] b = new byte[strTemp.Length / 8];
            for (int i = 0; i < b.Length; i++)
                b[i] = Convert.ToByte(strTemp.Substring(i * 8, 8), 2);

            return ByteArrayToHexStr(b) + str0;
        }

        private void rb6_CheckedChanged(object sender, EventArgs e)
        {
            if (rb6.Checked)
            {
                dataGridView1.Enabled = false;
            }
            else
            {
                dataGridView1.Enabled = true;
            }
        }

       
    }
}
