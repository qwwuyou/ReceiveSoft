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

namespace CenterApp
{
    public partial class registerControl : UserControl
    {
        StyleManager sm;
        public registerControl(StyleManager stylemanager)
        {
            InitializeComponent();
            sm = stylemanager;
        }

        private void registerControl_Load(object sender, EventArgs e)
        {
            SetChart();
            SetDataGird();
        }

        private void SetChart()
        {
            IList<Service.Model.CENTER_SERVER> servers = DataClass.servers;
           
 
            // simplify the chart reference.
            C1Chart chart = c1Chart1;
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
            //ax.AnnoFormat = FormatEnum.;
            ax.AnnotationRotation = 30;
            ax.Min = 0;
            ax.Max = 300;
            //ax.Origin = ax.Min;
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
            //ax.GridMinor.Visible = true;
            ax.GridMajor.Pattern = LinePatternEnum.Solid;
            ax.GridMajor.Color = area.PlotArea.BackColor;
            //ax.GridMajor.Visible = true;
            ax.Font = new Font("ArialBlack", 8, FontStyle.Bold);
            #endregion

            //   Specify the chart type as Gantt in the ChartGroup
            ChartGroup cg = chart.ChartGroups.Group0;
            cg.ChartType = Chart2DTypeEnum.Bar;
            

            //   Clear the existing data and add new Series data.
            ChartDataSeriesCollection cdsc = cg.ChartData.SeriesList;
            cdsc.Clear();

            // create a new series for each "row" of the Gantt chart.
            ChartDataSeries cds = cdsc.AddNewSeries();
            
            List<int> vals = new List<int>();
            List<string> names = new List<string>();


            foreach (var item in servers)
            {
                vals.Add(item.RegisterTime);
                names.Add(item.ProjectName);
            }
            cds.Y.CopyDataIn(vals.ToArray());
            cds.X.CopyDataIn(names.ToArray());
            cds.Tag = names;
        }

        private void SetDataGird() 
        {
            dataGridViewX1.AutoGenerateColumns = false;
            dataGridViewX1.DataSource =DataClass . servers;
        }

        void chart_ShowTooltip(object sender, ShowTooltipEventArgs e)
        {
            if (sender is ChartDataSeries)
            {
                ChartDataSeries cds = (ChartDataSeries)sender;
                e.TooltipText = (cds.Tag as List<string>)[e.PointIndex] + "\r\n注册时长:" + cds.Y[e.PointIndex];
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (dataGridViewX1.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    string server = dataGridViewX1.Rows[e.RowIndex].Cells["服务名"].Value.ToString();
                    string ip = dataGridViewX1.Rows[e.RowIndex].Cells["IP"].Value.ToString();

                    DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("是否要注册服务[" + server + ":" + ip + "]！", "[提示]", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        
                    }
                }
            }
        }

        

    }
}
