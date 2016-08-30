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
    public partial class GetCommandState : UserControl
    {
        public GetCommandState()
        {
            InitializeComponent();
            dateTimePicker_E.Text = DateTime.Now.AddHours(1).ToString();
        }

        private void comboBox_STCD_Init()
        {
            string Where = "where NiceName like '%" + comboBox_STCD.Text + "%'";
            IList<Service.Model.YY_RTU_Basic> RtuList = PublicBD.db.GetRTUList(Where);
            Service.Model.YY_RTU_Basic rtu = new Service.Model.YY_RTU_Basic();
            rtu.NiceName = "全部";
            rtu.STCD = "-1";
            RtuList.Insert(0, rtu);

            comboBox_STCD.DataSource = RtuList;
            comboBox_STCD.DisplayMember = "NiceName";
            comboBox_STCD.ValueMember = "STCD";

        }

        private void GetCommandState_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;  //强制控件列位置不许变

            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor;
            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            dataGridViewStyle(dataGridView1);

            comboBox_State.SelectedIndex = 0;
            comboBox_STCD_Init();

            Search();

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

        private void Search()
        {
            string Where="where ";
            if (comboBox_STCD.SelectedValue != null && comboBox_STCD.SelectedValue.ToString() != "-1") 
            {
                Where += " YY_DATA_COMMAND.STCD='" + comboBox_STCD.SelectedValue.ToString() + "' and ";
            }
            if (comboBox_State.SelectedItem.ToString() == "超时") 
            {
                Where += " YY_DATA_COMMAND.State=-1 and ";
            }
            else if(comboBox_State.SelectedItem.ToString() == "完成")
            {
                Where += " YY_DATA_COMMAND.State=-2 and ";
            }

            Where += " YY_DATA_COMMAND.TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and YY_DATA_COMMAND.TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'  order by YY_DATA_COMMAND.TM desc";
            if (PublicBD.DB == "ORACLE")
            {
                Where += " YY_DATA_COMMAND.TM>=to_date('" + DateTime.Parse(dateTimePicker_B.Text) + "','yyyy-MM-dd HH24:MI:SS') and YY_DATA_COMMAND.TM<=to_date('" + DateTime.Parse(dateTimePicker_E.Text) + "','yyyy-MM-dd HH24:MI:SS')  order by YY_DATA_COMMAND.TM desc";
            }
            DataTable dt = PublicBD.db.GetCommandState(Where);
            dataGridView1.DataSource = dt;

            //for (int i = 0; i < 10; i++)
            //{
            //    dataGridView1.Columns["Column" + (i + 1)].DisplayIndex = i;
            //}
           
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("Column1"))
                {
                    if (e.Value.ToString() == "-1")
                        e.Value = PubObject.imgList.Images[2];
                    else if (e.Value.ToString() == "-2")
                        e.Value = PubObject.imgList.Images[3];
                }
                if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("Column4"))
                {
                    if (e.Value.ToString() == "1")
                    { e.Value = "TCP"; }
                    else if (e.Value.ToString() == "2")
                    { e.Value = "UDP"; }
                    else if (e.Value.ToString() == "3")
                    { e.Value = "短信"; }
                    else if (e.Value.ToString() == "4")
                    { e.Value = "卫星"; }
                }
            }
        }
    }
}
