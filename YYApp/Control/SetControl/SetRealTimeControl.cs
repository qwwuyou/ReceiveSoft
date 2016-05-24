using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;
using System.Windows.Forms.DataVisualization.Charting;

namespace YYApp.SetControl
{
    public partial class SetRealTimeControl : UserControl
    {
        public SetRealTimeControl()
        {
            InitializeComponent();
        }

        string ItemId = null;
        string stcd =null;
        public SetRealTimeControl(string ItemID,string STCD)
        {
            InitializeComponent();
            ItemId = ItemID;
            stcd = STCD;
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

        private void Control_Init()
        {
            
            comboBox_STCD.SelectedValue = stcd;
            comboBox_Item.SelectedValue = ItemId;
            dateTimePicker_B.Value = DateTime.Now.AddDays(-3);
            Search();
        }
        #endregion

        private void SetRealTimeControl_Load(object sender, EventArgs e)
        {
            panelEx1.Style.BackColor1.Color = this.ParentForm.BackColor; 
            panelEx2.Style.BackColor1.Color = this.ParentForm.BackColor;

            panelEx1.Style.BorderColor.Color=((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            panelEx2.Style.BorderColor.Color = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            dataGridViewStyle(dataGridView1);


            comboBox_STCD_Init();
            comboBox_Item_Init();
            RadioButtong_Init();

            if (ItemId != null && stcd != null)
            {
                Control_Init();
                superTabControl1.SelectedTabIndex = 1;
            }
        }
        private void RadioButtong_Init() 
        {
            if (Program.wrx.ReadDllXML().ToLower() == "hjt212-2005.dll")
            {
                radioButton_2011.Enabled = true;
                radioButton_2031.Enabled = true;
                radioButton_2051.Enabled = true;
                radioButton_2061.Enabled = true;
                radioButton_2011.Checked = true;

                comboBox_Item.Width = 95;
            }
            else 
            {
                radioButton_2011.Enabled = false;
                radioButton_2031.Enabled = false;
                radioButton_2051.Enabled = false;
                radioButton_2061.Enabled = false;

                comboBox_Item.Width = 208;
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

                if (dgv.Columns[e.ColumnIndex].HeaderText == "采集时间" ||dgv.Columns[e.ColumnIndex].HeaderText == "接收时间")
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

        private void Search() 
        {
            string Where = " where stcd='" + comboBox_STCD.SelectedValue + "'  and ItemID='" + comboBox_Item.SelectedValue + "' and TM>='" + DateTime.Parse(dateTimePicker_B.Text) + "' and TM<='" + DateTime.Parse(dateTimePicker_E.Text) + "'";

            if (Program.wrx.ReadDllXML().ToLower() == "hjt212-2005.dll")
            {
                if (radioButton_2011.Checked) 
                {
                    Where += " and DATATYPE='2011'";
                }
                else if (radioButton_2031.Checked) 
                {
                    Where += " and DATATYPE like '2031__'";
                }
                else if (radioButton_2051.Checked)
                {
                    Where += " and DATATYPE like '2051__'";
                }
                else if (radioButton_2061.Checked)
                {
                    Where += " and DATATYPE like '2061__'";
                }
            }
            DataTable dt = PublicBD.db.GetRealTimeDataForWhere(Where);
            dataGridView1.DataSource = dt; 
            
            //重要，如去掉该句列顺序会变乱
            dataGridView1.AutoGenerateColumns = false; 
            if (dt != null)
                for (int i = 0; i < 14; i++)
                {
                    dataGridView1.Columns["Column" + (i + 1)].DisplayIndex = i;
                }
            
            Chart_LoadData(dt);
        }

        //private void Chart_LoadData(DataTable dt)
        //{
        //    double val = 0;
        //    if (dt != null)
        //    {
        //        microChart1.DataPoints.Clear();
        //        microChart1.Refresh();
        //        List<string> str = new List<string>();
        //        for (int i = 0; i < dt.Rows .Count ; i++)
        //        {
        //            val = double.Parse(dt.Rows[i]["datavalue"].ToString());
        //            microChart1.DataPoints.Add(val);

        //            str.Add("值：" + val + "\n时间：" + dt.Rows[i]["TM"].ToString() + "");
        //        }
        //        microChart1.Refresh();
        //        microChart1.DataPointTooltips = str;
        //    }
        //}

        private void Chart_LoadData(DataTable dt)
        {
            if (dt != null)
            {
                DataView dataView = dt.DefaultView;
                DataTable dataTableDistinct = dataView.ToTable(true, "datatype");
                if (dataTableDistinct.Rows.Count == 1)
                {
                    double val = 0;
                    string date = "";// DateTime.Now;
                    chart1.Series.Clear();

                    Series series = new Series("");
                    if (dt.Rows.Count > 0)
                    {
                        series.Name = dt.Rows[0]["ItemName"].ToString();
                    }
                    series.ChartType = SeriesChartType.Line;
                    series.MarkerStyle = MarkerStyle.Circle;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        val = double.Parse(dt.Rows[i]["datavalue"].ToString());
                        date = DateTime.Parse(dt.Rows[i]["TM"].ToString()).ToString("MM月dd日 HH时");
                        series.Points.AddXY(date, val);
                        series.Points[i].ToolTip = "值：#VAL" + "\n时间：" + dt.Rows[i]["TM"].ToString();
                    }

                    chart1.Series.Add(series);
                }
                else
                {
                    double val = 0;
                    string date = "";// DateTime.Now;
                    
                    chart1.Series.Clear();
                    Series series = null;
                    DataTable NewTable = null;
                    DataView NewDv = null;
                    Legend legend = null;
                    for (int k = 0; k < dataTableDistinct.Rows.Count; k++)
                    {
                        var newtable = from dv in dt.AsEnumerable() where dv["DATATYPE"].ToString() == dataTableDistinct.Rows[k]["DATATYPE"].ToString() select dv;
                        NewDv = newtable.AsDataView();
                        NewDv.Sort = "TM asc";
                        NewTable = NewDv.ToTable();

                        series = new Series();
                        if (NewTable.Rows.Count > 0)
                        {
                            #region 环保212协议特殊添加
                            string ItemName = null;
                            if (dataTableDistinct.Rows[k]["DATATYPE"].ToString().Length == 6)
                            {
                                string s4 = dataTableDistinct.Rows[k]["DATATYPE"].ToString().Substring(0, 4);
                                string s2 = dataTableDistinct.Rows[k]["DATATYPE"].ToString().Substring(4, 2);

                                switch (s4)
                                {
                                    case "2031":
                                        ItemName = "日";
                                        break;
                                    case "2051":
                                        ItemName = "分钟";
                                        break;
                                    case "2061":
                                        ItemName = "小时";
                                        break;

                                }

                                switch (s2)
                                {
                                    case "01":
                                        ItemName += "|累计|";
                                        break;
                                    case "02":
                                        ItemName += "|最大|";
                                        break;
                                    case "03":
                                        ItemName += "|最小|";
                                        break;
                                    case "04":
                                        ItemName += "|平均|";
                                        break;
                                }
                            }
                            #endregion
                            series.Name = ItemName+NewTable.Rows[0]["ItemName"].ToString();
                            legend = new Legend(series.Name);
                            
                            //legend.Position = new ElementPosition(0, 0, 20, 100);
                            legend.Docking = Docking.Bottom;
                            legend.TitleAlignment = StringAlignment.Center;
                            legend.Alignment = StringAlignment.Center;
                            chart1.Legends.Add(legend);
                        }
                        series.ChartType = SeriesChartType.Line;
                        series.MarkerStyle = MarkerStyle.Circle;

                        for (int i = 0; i < NewTable.Rows.Count; i++)
                        {
                            val = double.Parse(NewTable.Rows[i]["datavalue"].ToString());
                            date = DateTime.Parse(NewTable.Rows[i]["TM"].ToString()).ToString("MM月dd日 HH时");
                            series.Points.AddXY(date, val);
                            series.Points[i].ToolTip = "值：#VAL" + "\n时间：" + NewTable.Rows[i]["TM"].ToString();
                        }
                        
                        chart1.Series.Add(series);
                    }

                   
                   
                }
                
            }
         }


        private void button_Add_Click(object sender, EventArgs e)
        {
            if (Validate(textBox_Val.Text.Trim()) && comboBox_STCD.SelectedValue!=null) 
            {
                 Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                 model.STCD = comboBox_STCD.SelectedValue.ToString();
                 model.TM = DateTime.Parse(dateTimePicker_Add.Text);
                 model.CorrectionVALUE= decimal.Parse(textBox_Val.Text.Trim());
                 model.DOWNDATE = DateTime.Now;
                 model.NFOINDEX = 5;
                 model.ItemID = comboBox_Item.SelectedValue.ToString();
                 //model.DATATYPE = "";
                 bool b=PublicBD.db.AddRealTimeData(model);
                 if (b)
                 {
                     dateTimePicker_Add.ResetText();
                     textBox_Val.Text = "";
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
            if (e.RowIndex != -1 && e.ColumnIndex!=-1)
            {

                Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                model.STCD = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                model.TM = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["Column5"].Value;//此字段为记录原始采集时间（精确到毫秒的主键）

                model.CorrectionVALUE = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value.ToString());
                model.DOWNDATE = DateTime.Now;
                model.NFOINDEX = 5;
                model.ItemID = dataGridView1.Rows[e.RowIndex].Cells["Column9"].Value.ToString();
                //model.DATATYPE = "";
                string Where = "where stcd='" + model.STCD + "' and ItemID='" + model.ItemID + "' and  TM='" + model.TM.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删 除")
                {
                    bool b = PublicBD.db.DelRealTimeData(Where);
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
                    bool b=PublicBD.db.UdpRealTimeData(model, Where);
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
            if (DevComponents.DotNetBar.MessageBoxEx.Show("是否删除测站[" + comboBox_STCD.Text  + "]的全部实时数据？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PublicBD.db.DelRealTimeData(" where stcd='" + comboBox_STCD.Text + "'");
                Search();
            }
        }
        
    }
}
