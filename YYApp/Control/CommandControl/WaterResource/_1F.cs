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
    public partial class _1F : UserControl, ICommandControl
    {
        public _1F(string[] stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            dataGridView_Init(stcds);
        }

        private void dataGridView_Init(string[] stcds)
        {
            string where = "where YY_RTU_CONFIGDATA.stcd in (";
            foreach (var item in stcds)
            {
                where += "'" + item + "',";
            }
            if (where != "")
            {
                where = where.Substring(0, where.Length - 1);
            }
            where = where + ") and YY_RTU_CONFIGDATA.ConfigID in ('01') and YY_RTU_CONFIGDATA.ItemID in ('0003000001')";
            dataGridView1.DataSource = PublicBD.db.GetRTU_CONFIGDATA(where);
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
            //DGV.Paint += new PaintEventHandler(DGV_Paint);
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
            CommandCode = "";
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string[] commands = null;

            if (rb6.Checked)
            {
                int gnm = 0x64;
                CommandCode = "64";
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

                string[] sjys = Validate(Stcds);
                if (sjys == null)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("配置项的数据值输入有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                if (dataGridView1.Rows.Count != Stcds.Length )
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("请确认所选站点已配置上限配置项！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                int gnm = 0x1F;
                CommandCode = "1F";
                commands = new string[Stcds.Length];
                for (int i = 0; i < Stcds.Length; i++)
                {
                    var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                    byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjys[i], int.Parse(RTU.First().PWD));

                    commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
                }
            }

            return commands;
        }

        private string[] Validate(string[] Stcds)
        {
            string[] sjys = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                decimal sx = 0;
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                   
                    if (dataGridView1.Rows[j].Cells["Column5"].Value.ToString() == Stcds[i] && dataGridView1.Rows[j].Cells["Column7"].Value.ToString() == "01")
                    {
                        if (!decimal.TryParse(dataGridView1.Rows[j].Cells["Column4"].Value.ToString(), out sx))
                        { return null; }
                        else
                        {
                            string str = sx.ToString();
                            string regText = @"^[-+]?\d{1,6}(\.\d{1,3})?$";
                            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regText);
                            if (!reg.IsMatch(str))
                            { return null; }

                        }
                    } 
                }
                sjys[i] =  sx.ToString() ;

            }

            return sjys;
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

        private void _1F_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;

            dataGridViewStyle(dataGridView1);
        }

    }
}
