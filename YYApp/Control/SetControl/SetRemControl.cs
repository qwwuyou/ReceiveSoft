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
    public partial class SetRemControl : UserControl
    {
        public SetRemControl()
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

        private IList<Service.Model.YY_RTU_ITEM> ITEMList = null;
        private void comboBox_Item_Init()
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(" where ItemCode!='-1' and ItemCode!='0000000000'");
            if (ItemList != null && ItemList.Count > 0)
            {
                ITEMList = ItemList;
                comboBox_Item.DataSource = ItemList;
                comboBox_Item.DisplayMember = "ItemName";
                comboBox_Item.ValueMember = "ItemID";
                comboBox_Item.SelectedIndex = 0;
            }
        }
        #endregion

        private void Search()
        {

            string Where = " where stcd='" + comboBox_STCD.SelectedValue + "'  and ItemID='" + comboBox_Item.SelectedValue + "' and TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'";
            if (PublicBD.DB == "ORACLE")
            {
                Where = " where stcd='" + comboBox_STCD.SelectedValue + "'  and ItemID='" + comboBox_Item.SelectedValue + "' and TM>= to_date('" + DateTime.Parse(dateTimePicker_B.Text) + "','yyyy-MM-dd HH24:MI:SS') AND TM<= to_date('" + DateTime.Parse(dateTimePicker_E.Text) + "','yyyy-MM-dd HH24:MI:SS') ";
            }
            DataTable dt = PublicBD.db.GetRemDataForWhere (Where);
            dataGridView1.DataSource = dt;
            if (dt != null) 
            for (int i = 0; i < 14; i++)
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
            comboBox_Item_Init();
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

            if (dgv.Columns[e.ColumnIndex].HeaderText == "值")
            {
                try
                {
                    e.Value = Math.Round(decimal.Parse(e.Value.ToString()), int.Parse(dataGridView1.Rows[e.RowIndex].Cells["Column13"].Value.ToString())).ToString();
                    if (!Validate(e.Value.ToString()))
                    { dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]; }
                }
                catch { dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]; }
            }

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

        private bool Validate(string str)
        {
            int ItemInteger = 0;
            int ItemDecimal = 0;
            bool PlusOrMinus = true;
            string regText = "";
            if (ITEMList != null)
            {
                var item = from i in ITEMList where i.ItemID == comboBox_Item.SelectedValue.ToString() select i;
                if (item.Count() > 0)
                {
                    ItemInteger = item.First().ItemInteger;
                    ItemDecimal = item.First().ItemDecimal;
                    PlusOrMinus = item.First().PlusOrMinus;

                    if (PlusOrMinus)
                    {
                        if (ItemDecimal == 0)
                        {
                            regText = @"^[-+]?\d{1," + ItemInteger + @"}$";
                        }
                        else
                        {
                            regText = @"^[-+]?\d{1," + ItemInteger + @"}(\.\d{1," + ItemDecimal + "})?$";
                        }
                    }
                    else
                    {
                        if (ItemDecimal == 0)
                        {
                            regText = @"^\d{1," + ItemInteger + @"}$";
                        }
                        else
                        {
                            regText = @"^\d{1," + ItemInteger + @"}(\.\d{1," + ItemDecimal + "})?$";
                        }
                    }
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regText);
                    if (!reg.IsMatch(str))
                    { return false; }
                    else
                    { return true; }
                }

            }
            return false;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (Validate(textBox_Val.Text.Trim()))
            {
                Service.Model.YY_DATA_REM model = new Service.Model.YY_DATA_REM();
                model.STCD = comboBox_STCD.SelectedValue.ToString();
                model.TM = DateTime.Parse(dateTimePicker_Add.Text);
                model.DATAVALUE = decimal.Parse(textBox_Val.Text.Trim());
                model.DOWNDATE = DateTime.Now;
                model.NFOINDEX = 5;
                model.ItemID = comboBox_Item.SelectedValue.ToString();
                bool b = PublicBD.db.AddRemData(model);
                if (b)
                {
                    dateTimePicker_Add.ResetText();
                    textBox_Val.Text = "";
                    Search();
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据添加成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("数据添加失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {

                Service.Model.YY_DATA_REM model = new Service.Model.YY_DATA_REM();
                model.STCD = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                model.TM = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["Column5"].Value;
                model.DATAVALUE = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value.ToString());
                model.DOWNDATE = DateTime.Now;
                model.NFOINDEX = 5;
                model.ItemID = dataGridView1.Rows[e.RowIndex].Cells["Column8"].Value.ToString();

                string Where = "where stcd='" + model.STCD + "' and ItemID='" + model.ItemID + "' and   TM='" + model.TM.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                if (PublicBD.DB == "ORACLE")
                {
                    Where = "where stcd='" + model.STCD + "' and ItemID='" + model.ItemID + "' and  TM=to_date('" + model.TM.ToString() + "','yyyy-MM-dd HH24:MI:SS')";
                }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    bool b = PublicBD.db.DelRemData(Where);
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
                    bool b = PublicBD.db.UdpRemData (model, Where);
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
            if (DevComponents.DotNetBar.MessageBoxEx.Show("是否删除测站[" + comboBox_STCD.Text  + "]的全部固态数据？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PublicBD.db.DelRemData(" where stcd='" + comboBox_STCD.Text + "'");
                Search();
            }
        }
    }
}

