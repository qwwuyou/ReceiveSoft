using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;
using System.IO;

namespace YYApp.SetControl
{
    public partial class SetImgControl : UserControl
    {
        public SetImgControl()
        {
            InitializeComponent();
            dateTimePicker_E.Text = DateTime.Now.AddHours(1).ToString("yyyy年MM月dd日 HH时");
        }

        #region 初始化
        //private IList<Service.Model.YY_RTU_Basic> RTUList = null;
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

        #endregion

        private void Search()
        {
            string Where="";
            if (comboBox_STCD.SelectedValue.ToString() != "-1")
            {
                Where = " where YY_DATA_IMG.stcd='" + comboBox_STCD.SelectedValue + "'  and YY_DATA_IMG.TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and YY_DATA_IMG.TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'  order by YY_DATA_IMG.TM desc";
                if (PublicBD.DB == "ORACLE")
                {
                    Where = " where YY_DATA_IMG.stcd='" + comboBox_STCD.SelectedValue + "'  and YY_DATA_IMG.TM>=to_date('" + DateTime.Parse(dateTimePicker_B.Text) + "','yyyy-MM-dd HH24:MI:SS') and YY_DATA_IMG.TM<=to_date('" + DateTime.Parse(dateTimePicker_E.Text) + "','yyyy-MM-dd HH24:MI:SS')  order by YY_DATA_IMG.TM desc";
                }
            }
            else 
            {
                Where = " where  YY_DATA_IMG.TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and YY_DATA_IMG.TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'  order by YY_DATA_IMG.TM desc";
                if (PublicBD.DB == "ORACLE")
                {
                    Where = " where  YY_DATA_IMG.TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and YY_DATA_IMG.TM<=to_date('" + DateTime.Parse(dateTimePicker_E.Text)  +"','yyyy-MM-dd HH24:MI:SS')  order by YY_DATA_IMG.TM desc";
                }
            }
            DataTable dt = PublicBD.db.GetImgForWhere(Where);
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

            panelEx1.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

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
                model.TM = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["Column6"].Value;
                model.DATAVALUE = dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
                model.DOWNDATE = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["Column7"].Value;
                model.NFOINDEX = 5;
              

                string Where = "where stcd='" + model.STCD + "'  and   TM='" + model.TM.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    bool b = PublicBD.db.DelImg(Where);
                    if (b)
                    {
                        Search();
                        DevComponents.DotNetBar.MessageBoxEx.Show("数据删除成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    { DevComponents.DotNetBar.MessageBoxEx.Show("数据删除失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }

                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "查 看")
                {
                    string STCD = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                    DateTime TM = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["Column6"].Value.ToString());
                    List<Service.Model.YY_DATA_IMG> list = PublicBD.db.GetImg(STCD, TM).ToList<Service.Model.YY_DATA_IMG>();
                    if (list.Count > 0)
                    {
                        byte[] bt = list.First().DATAVALUE;
                        if (bt != null && bt.Length > 0)
                        {
                            string Path = CreateImage(STCD, TM.ToString(), bt);
                            System.Diagnostics.Process.Start(Path);
                        }
                        else
                        { DevComponents.DotNetBar.MessageBoxEx.Show("图片内容为空！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                }
            }
        }

        private  string CreateImage(string STCD, string Time, byte[] img)
        {
            string Path="";
            string directory_name = System.Windows.Forms.Application.StartupPath + "/Image";
            string file_name = directory_name + "/" + STCD + "/" + STCD + "-" + DateTime.Parse(Time).ToString("yyyyMMddHHmmss") + ".jpg";
            if (!Directory.Exists(directory_name))
            {
                Directory.CreateDirectory(directory_name);
            }
            if (!Directory.Exists(directory_name + "/" + STCD))
            {
                Directory.CreateDirectory(directory_name + "/" + STCD);
            }
            if (!File.Exists(file_name))
            {
                FileInfo fi = new FileInfo(file_name);
                FileStream fs = fi.OpenWrite();
                fs.Write(img, 0, img.Length);
                fs.Close();
                fs.Dispose();
                Path = file_name;
            }
            else { Path = file_name; }
            return Path;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            string STCD = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            string name = STCD + "-" + DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["Column6"].Value.ToString()).ToString("yyyyMMddHHmmss")+".jpg";
            string filename = System.Windows.Forms.Application.StartupPath + "/Image/" + STCD + "/" + name;
            if (File.Exists(filename))
            {
                ((DataGridViewDisableButtonCell)dataGridView1["Column8", e.RowIndex]).Enabled = true;
            }
            else 
            {
                ((DataGridViewDisableButtonCell)dataGridView1["Column8", e.RowIndex]).Enabled = false ;
            }

            if (dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value.ToString() == "-1")
            {
                ((DataGridViewDisableButtonCell)dataGridView1["Column8", e.RowIndex]).UseColumnTextForButtonValue = false;
            }
            else 
            {
                ((DataGridViewDisableButtonCell)dataGridView1["Column8", e.RowIndex]).UseColumnTextForButtonValue = true;
            }
        }

        #region 禁用按钮操作
        public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
        {
            public DataGridViewDisableButtonColumn()
            {
                this.CellTemplate = new DataGridViewDisableButtonCell();
            }
        }

        public class DataGridViewDisableButtonCell : DataGridViewButtonCell
        {
            private bool enabledValue;
            public bool Enabled
            {
                get
                {
                    return enabledValue;
                }
                set
                {
                    enabledValue = value;
                }
            }

            public override object Clone()
            {
                DataGridViewDisableButtonCell cell =
                    (DataGridViewDisableButtonCell)base.Clone();
                cell.Enabled = this.Enabled;
                return cell;
            }

            public DataGridViewDisableButtonCell()
            {
                this.enabledValue = true;
            }

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates elementState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                if (!this.enabledValue)
                {
                    if ((paintParts & DataGridViewPaintParts.Background) ==
                        DataGridViewPaintParts.Background)
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.BackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }

                    if ((paintParts & DataGridViewPaintParts.Border) ==
                        DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                            advancedBorderStyle);
                    }
                    Rectangle buttonArea = cellBounds;
                    Rectangle buttonAdjustment =
                        this.BorderWidths(advancedBorderStyle);
                    buttonArea.X += buttonAdjustment.X;
                    buttonArea.Y += buttonAdjustment.Y;
                    buttonArea.Height -= buttonAdjustment.Height;
                    buttonArea.Width -= buttonAdjustment.Width;
                    ButtonRenderer.DrawButton(graphics, buttonArea,
                        System.Windows.Forms.VisualStyles.PushButtonState.Disabled);

                    if (this.FormattedValue is String)
                    {
                        TextRenderer.DrawText(graphics,
                            (string)this.FormattedValue,
                            this.DataGridView.Font,
                            buttonArea, SystemColors.GrayText);
                    }
                }
                else
                {
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                        elementState, value, formattedValue, errorText,
                        cellStyle, advancedBorderStyle, paintParts);
                }
            }
        }
        #endregion

        private void button_Open_Click(object sender, EventArgs e)
        {
            string Path=System.Windows.Forms.Application.StartupPath + @"\Image";
            if (Directory.Exists(Path))
                System.Diagnostics.Process.Start("explorer.exe", Path);
            else
                DevComponents.DotNetBar.MessageBoxEx.Show("图片文件夹不存在！");
        }

    }
}

