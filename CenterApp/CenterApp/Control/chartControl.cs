using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1Chart;
using DevComponents.DotNetBar;
using System.Collections;

namespace CenterApp
{
    public partial class chartControl : UserControl
    {
        StyleManager sm;
        public chartControl(StyleManager stylemanager)
        {
            InitializeComponent();
            sm = stylemanager;
        }
        


        private void chartControl_Load(object sender, EventArgs e)
        {
            SetChart(null,null);
            SetDataGird();
        }


        #region Chart
        private void SetChart(DateTime? DT1,DateTime? DT2)
        {
            IList<Service.Model.CENTER_SERVER> servers=null;
            IList<Service.Model.CENTER_STARTSTATE> serverstate = null;
            string Where = "";
            DateTime dt_start = new DateTime();
            DateTime dt_end = new DateTime();

            if (DT1 != null && DT2!= null)
            {
                //获得server的列表，和每个server的启停情况
                Where = " where Dtime>= '" + DT1.Value + "' and Dtime<='" + DT2.Value + "'";
                if (Service.PublicBD.DB == "ORACLE")
                {
                    Where = "where Dtime>=to_date('" + DT1.Value + "','yyyy-MM-dd HH24:MI:SS') and Dtime<=to_date('" + DT2.Value + "','yyyy-MM-dd HH24:MI:SS')";
                }
                DataClass.GetCenterInfo(Where);
                servers = DataClass.servers;
                serverstate = DataClass.serverstate;
                //X轴的起止时间
                dt_start = DT1.Value;
                dt_end = DT2.Value;
            }
            else 
            {
                
                servers = DataClass.servers;
                serverstate = DataClass.serverstate;
                //X轴的起止时间
                dt_start = (from s in serverstate orderby s.DTime ascending select s).First().DTime;
                dt_end = DateTime.Now.AddHours(1);

                dateTimeInput1.Value = dt_start;
                dateTimeInput2.Value = dt_end;
            }
            // simplify the chart reference.
            C1Chart chart =c1Chart1;

            chart.ToolTip.Enabled = true;
            chart.ShowTooltip -= new ShowTooltipEventHandler(chart_ShowTooltip);//避免重复注册
            chart.ShowTooltip += new ShowTooltipEventHandler(chart_ShowTooltip);
          
            #region Chart Style
            chart.Style.Border.Color = sm.MetroColorParameters.BaseColor;
            chart.Style.BackColor = sm.MetroColorParameters.CanvasColor;
            chart.ChartArea.Style.BackColor = sm.MetroColorParameters.CanvasColor;
            chart.Style.BackColor2 = Color.Azure;
            chart.Style.GradientStyle = GradientStyleEnum.FromCenter;
            #endregion

            #region Chart Area style
            Area area = chart.ChartArea;
            area.Style.BackColor = Color.Transparent;
            area.Style.GradientStyle = GradientStyleEnum.None;
            area.Inverted = true;	// X axis is vertical
            #endregion

            #region   Plot Area style
            area.PlotArea.BackColor = Color.Azure;
            area.PlotArea.Boxed = true;
            #endregion

            #region Axis
            //   Set up the style and format of the Horizontal (Y) axis.
            Axis ax = area.AxisY;

            ax.AnnoFormat = FormatEnum.DateManual;
            ax.AnnoFormatString = "MMM-dd";
            ax.AnnotationRotation = -30;
            //ax.Min = new DateTime(2016, 8, 10).ToOADate();
            //ax.Max = new DateTime(2016, 8, 30).ToOADate();
            ax.Min = dt_start.ToOADate();
            ax.Max = dt_end.ToOADate();
            ax.Origin = ax.Min;
            ax.Font = new Font("ArialBlack", 8, FontStyle.Bold);
            //ax.UnitMajor = 14; //x轴间隔
            ax.TickMinor = TickMarksEnum.None;
            ax.GridMajor.Pattern = LinePatternEnum.Dash;
            ax.GridMajor.Color = Color.LightGray;
            ax.GridMajor.Visible = true;

            //   Set up the style and format of the Vertical (X) axis.
            ax = area.AxisX;
            ax.TickMinor = TickMarksEnum.None;
            ax.Reversed = true;		// top to bottom
            ax.UnitMajor = 1;
            ax.GridMinor.Pattern = LinePatternEnum.Dash;
            ax.GridMinor.Color = Color.LightGray;
            ax.GridMinor.Visible = true;
            ax.GridMajor.Pattern = LinePatternEnum.Solid;
            ax.GridMajor.Color = area.PlotArea.BackColor;
            ax.GridMajor.Visible = true;
            ax.Font = new Font("ArialBlack", 8, FontStyle.Bold);
            #endregion

            //   Specify the chart type as Gantt in the ChartGroup
            ChartGroup cg = chart.ChartGroups.Group0;
            cg.ChartType = Chart2DTypeEnum.Gantt;
            cg.Gantt.Width = 40;

            //   Clear the existing data and add new Series data.
            ChartDataSeriesCollection cdsc = cg.ChartData.SeriesList;
            cdsc.Clear();

            // create a new series for each "row" of the Gantt chart.

            foreach (var item in servers)
            {
                var STs = from st in serverstate where st.ProjectName + st.PublicIP == item.ProjectName + item.PublicIP select st;

                if (STs.Count() > 0)
                {
                    List<DateTime> dt1 = new List<DateTime>();
                    List<DateTime> dt2 = new List<DateTime>();
                    List<string> runtimelist = new List<string>();
                    foreach (var sts in STs)
                    {
                        dt1.Add(sts.DTime);
                        string[] temp = sts.RunTime.Split(new char[] { 'd', 'h', 'm', 's' });
                        runtimelist.Add(sts.RunTime.Replace("d", "天").Replace("h", "小时").Replace("m", "分钟").Replace("s", "秒"));
                        dt2.Add(sts.DTime.AddDays(int.Parse(temp[0])).AddHours(int.Parse(temp[1])).AddMinutes(int.Parse(temp[2])).AddSeconds(int.Parse(temp[3])));
                    }
                    string ServerName = STs.First().ProjectName+"-"+item.RTUCount;
                    string IP = item.PublicIP;
                    AddGanttSeriesData(cdsc, ServerName, IP, runtimelist.ToArray(), dt1.ToArray(), dt2.ToArray());

                }
            }
         
        }

        void chart_ShowTooltip(object sender, ShowTooltipEventArgs e)
        {
            if (sender is ChartDataSeries)
            {
                ChartDataSeries ds = (ChartDataSeries)sender;
                object obj = ds.Tag;
                e.TooltipText += "\r\n" + (obj as string[])[e.PointIndex];
            }
        }

        // Adds a new series to the Gantt chart accepting a task name,
        // an array of starting times and an array of ending times.
        private void AddGanttSeriesData(ChartDataSeriesCollection cdsc, string ServerName, string IP, string[] runtimelist, DateTime[] startTimes, DateTime[] endTimes)
        {
            ChartDataSeries cds = cdsc.AddNewSeries();
            cds.Label = ServerName;
            cds.Y.CopyDataIn(startTimes);
            cds.Y1.CopyDataIn(endTimes);
            cds.Tag = runtimelist;
            cds.TooltipText = ServerName + "\r\n" + IP;
        }

        #endregion


        private void SetDataGird()
        {
            dataGridViewX1.AutoGenerateColumns = false;
            dataGridViewX1.DataSource = DataClass.servers;
        }


        private void button_Seach_Click(object sender, EventArgs e)
        {
            SetChart(dateTimeInput1.Value, dateTimeInput2.Value);
            SetDataGird();
        }

        
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1) 
            {
                if (dataGridViewX1.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    string server=dataGridViewX1.Rows[e.RowIndex].Cells["服务名"].Value.ToString();
                    string ip = dataGridViewX1.Rows[e.RowIndex].Cells["IP"].Value.ToString();

                    DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("是否要删除服务[" + server + ":"+ip+"]的信息！", "[提示]", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes) 
                    {
                        DataClass.DeleteServerInfo(server,ip);
                        SetChart(dateTimeInput1.Value, dateTimeInput2.Value);
                    }
                }
            }
        }

        

    
    }
}
