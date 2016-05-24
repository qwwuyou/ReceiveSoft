using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Service.DB
{
    class RTF
    {
        /// <summary>
        /// 根据实时数据得到rtf文本
        /// </summary>
        /// <param name="dt">实时数据表</param>
        /// <returns></returns>
        public string GetRealTimeDataRtfString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"\rtf1\fbidis\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\froman\fprq2\fcharset134 方正姚体;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green191\blue255;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue255;}
\viewkind4\uc1\trowd\trgaph108\trleft-108\trbrdrt\brdrs\brdrw10\brdrcf16 \trbrdrl\brdrs\brdrw10\brdrcf16 \trbrdrb\brdrs\brdrw10\brdrcf16 \trbrdrr\brdrs\brdrw10\brdrcf16 \clbrdrt\brdrw15\brdrs\clbrdrl\brdrw15\brdrs\clbrdrb\brdrw15\brdrs\clbrdrr\brdrw15\brdrs 
\cellx1500
\cellx3600
\cellx5300
\cellx6500
\cellx8500
\cellx10500
\cellx11500\pard\intbl\ltrpar\kerning2\f0\fs20

站号\cell 站名\cell 监测项\cell 值\cell 接收时间\cell 采集时间\cell 信道\cell\row\intbl ");


            string ItemName = null;
            string DataValue = null;
            string TM = null;
            string DOWNDATE = null;
            string NFOINDEX = null;
            int ItemDecimal = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NFOINDEX = dt.Rows[i]["NFOINDEX"].ToString();
                
                if (NFOINDEX != "")
                {
                    ItemDecimal = int.Parse(dt.Rows[i]["ItemDecimal"].ToString());
                    if (ItemDecimal < 0) { ItemDecimal = 0; }
                    ItemName = dt.Rows[i]["itemname"].ToString();
                    DataValue = Math.Round(decimal.Parse(dt.Rows[i]["datavalue"].ToString()), ItemDecimal).ToString();
                    TM = DateTime.Parse(dt.Rows[i]["tm"].ToString()).ToString("dd日HH时mm分ss秒");
                    DOWNDATE=DateTime.Parse(dt.Rows[i]["DOWNDATE"].ToString()).ToString("dd日HH时mm分ss秒");
                }
                else 
                {
                    ItemName = "--";
                    DataValue = "--"; 
                    TM = "--";
                    DOWNDATE = "--";
                }


                sb.Append(@"\cf8 " + dt.Rows[i]["stcd"].ToString() + @"\cell ");
                sb.Append(@"\cf16 " + dt.Rows[i]["NiceName"].ToString() + @"\cell ");
                sb.Append(ItemName + @"\cell ");
                sb.Append( DataValue+ @"\cell ");
                sb.Append(TM + @"\cell ");
                sb.Append(DOWNDATE + @"\cell ");
                
                if (NFOINDEX == "1")
                    sb.Append("TCP" + @"\cell ");
                else if (NFOINDEX == "2")
                    sb.Append("UDP" + @"\cell ");
                else if (NFOINDEX == "3")
                    sb.Append("GSM" + @"\cell ");
                else if (NFOINDEX == "4")
                    sb.Append("卫星" + @"\cell ");
                else
                    sb.Append("--" + @"\cell ");
                sb.Append(@"\row\intbl ");
                

            }
            return "{" + sb.ToString() + "}";
        }

        /// <summary>
        /// 根据终端状态数据得到rtf文本
        /// </summary>
        /// <param name="dt">终端状态数据表</param>
        /// <returns></returns>
        public string GetRTUStateRtfString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"\rtf1\fbidis\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\froman\fprq2\fcharset134 方正姚体;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green191\blue255;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue255;}
\viewkind4\uc1\trowd\trgaph108\trleft-108\trbrdrt\brdrs\brdrw10\brdrcf16 \trbrdrl\brdrs\brdrw10\brdrcf16 \trbrdrb\brdrs\brdrw10\brdrcf16 \trbrdrr\brdrs\brdrw10\brdrcf16 \clbrdrt\brdrw15\brdrs\clbrdrl\brdrw15\brdrs\clbrdrb\brdrw15\brdrs\clbrdrr\brdrw15\brdrs 

\cellx1500
\cellx3600 
\cellx5600 
\cellx7600 
\cellx8600 ");
            int width = 8600;
            for (int i = 0; i < dt.Columns.Count -5; i++)
            {
                width = width + 1300;
                sb.Append(@"\cellx" + width+" ");
            }

            sb.Append(@"\pard\intbl\ltrpar\kerning2\f0\fs20");
            sb.Append(@"站号\cell 站名\cell 接收时间\cell 采集时间\cell 信道\cell");

            for (int i = 0; i < dt.Columns.Count - 5; i++)
            {
                sb.Append(dt.Columns[5+i].ColumnName+@"\cell ");
            }

            sb.Append(@"\row\intbl ");

            string TM = null;
            string DOWNDATE = null;
            string NFOINDEX = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NFOINDEX = dt.Rows[i]["信道"].ToString();

                if (NFOINDEX != "")
                {

                    TM = DateTime.Parse(dt.Rows[i]["接收时间"].ToString()).ToString("dd日HH时mm分ss秒");
                    DOWNDATE = DateTime.Parse(dt.Rows[i]["采集时间"].ToString()).ToString("dd日HH时mm分ss秒");
                }
                else
                {
                    TM = "--";
                    DOWNDATE = "--";
                }


                sb.Append(@"\cf8 " + dt.Rows[i]["站号"].ToString() + @"\cell ");
                sb.Append(@"\cf16 " + dt.Rows[i]["站名"].ToString() + @"\cell ");
                sb.Append(TM + @"\cell ");
                sb.Append(DOWNDATE + @"\cell ");

                if (NFOINDEX == "1")
                    sb.Append("TCP" + @"\cell ");
                else if (NFOINDEX == "2")
                    sb.Append("UDP" + @"\cell ");
                else if (NFOINDEX == "3")
                    sb.Append("GSM" + @"\cell ");
                else if (NFOINDEX == "4")
                    sb.Append("卫星" + @"\cell ");
                else
                    sb.Append("--" + @"\cell ");


                for (int j = 0; j < dt.Columns.Count - 5; j++)
                {
                    string state = dt.Rows[i][dt.Columns[5 + j].ColumnName].ToString();
                    if (state == "0")
                    {
                        state = "";
                        sb.Append(state + @"\cell ");
                    }
                    else if (state == "1")
                    {
                        state = "●";
                        sb.Append(@"\cf6"+state + @"\cell ");
                    }
                    else
                    {
                        state = "--";
                        sb.Append(state + @"\cell "); 
                    }
                    
                }


                sb.Append(@"\row\intbl ");


            } string sss= "{" + sb.ToString() + "}";
            return sss;
        }
    }
}
