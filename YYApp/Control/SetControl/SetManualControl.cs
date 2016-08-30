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
    public partial class SetManualControl : UserControl
    {
        public SetManualControl()
        {
            InitializeComponent();
        }

        #region 初始化
        //private IList<Service.Model.YY_RTU_Basic> RTUList = null;
        private void comboBox_STCD_Init()
        {
            string Where = "where NiceName like '%" + comboBox_STCD.Text + "%'";
            IList<Service.Model.YY_RTU_Basic> RtuList = PublicBD.db.GetRTUList(Where);
            comboBox_STCD.DataSource = RtuList;
            comboBox_STCD.DisplayMember = "NiceName";
            comboBox_STCD.ValueMember = "STCD";
        }

        #endregion

        private void Search()
        {
            string Where = " where YY_DATA_MANUAL.stcd='" + comboBox_STCD.SelectedValue + "'  and YY_DATA_MANUAL.TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and YY_DATA_MANUAL.TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'  order by YY_DATA_MANUAL.TM desc";
            if (PublicBD.DB == "ORACLE")
            {
                Where = " where YY_DATA_MANUAL.stcd='" + comboBox_STCD.SelectedValue + "'  and YY_DATA_MANUAL.TM>=to_date('" + DateTime.Parse(dateTimePicker_B.Text) + "','yyyy-MM-dd HH24:MI:SS') and YY_DATA_MANUAL.TM<=to_date('" + DateTime.Parse(dateTimePicker_E.Text) + "','yyyy-MM-dd HH24:MI:SS')  order by YY_DATA_MANUAL.TM desc";
            }
            DataTable dt = PublicBD.db.GetManualDataForWhere(Where);
            dataGridView1.DataSource = dt;
            if(dt!=null)
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Columns["Column" + (i + 1)].DisplayIndex = i;
            }
        }

        private void SetRemControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            dataGridViewStyle(dataGridView1);

            comboBox_STCD_Init();
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


        private void comboBox_STCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键   
            {
                comboBox_STCD_Init();
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //if (e.RowIndex < dgv.Rows.Count - 1)//这个判断,如果DataGridView没有设置为添加.就可以不要了          
            //{

           

            if (dgv.Columns[e.ColumnIndex].HeaderText == "采集时间" || dgv.Columns[e.ColumnIndex].HeaderText == "接收时间")
            {
                try
                {
                    e.Value = DateTime.Parse(e.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch { dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]; }
            }
            //} 
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }


      
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {

                Service.Model.YY_DATA_MANUAL model = new Service.Model.YY_DATA_MANUAL();
                model.STCD = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                model.TM = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["Column5"].Value;
                model.DATAVALUE = dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
                model.DOWNDATE = DateTime.Now;
                model.NFOINDEX = 5;
              

                string Where = "where stcd='" + model.STCD + "'  and   TM='" + model.TM.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    bool b = PublicBD.db.DelManualData(Where);
                    if (b)
                    {
                        Search();
                        DevComponents.DotNetBar.MessageBoxEx.Show("数据删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("数据删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }

                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "更 新")
                {
                    bool b = PublicBD.db.UdpManualData(model, Where);
                    if (b)
                    {
                        Search();
                        DevComponents.DotNetBar.MessageBoxEx.Show("数据更新成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("数据更新失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
        }

        private void buttonX_DelAll_Click(object sender, EventArgs e)
        {
            if (DevComponents.DotNetBar.MessageBoxEx.Show("是否删除测站[" + comboBox_STCD.Text  + "]的全部人工置数数据？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PublicBD.db.DelManualData(" where stcd='" + comboBox_STCD.SelectedValue + "'");
                Search();
            }
        }
    }
}

