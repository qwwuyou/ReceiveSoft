using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;

namespace Service.DB
{
    class DBBussiness
    {
        _51Data dt = new _51Data();
        public DBBussiness() 
        { dt.Open(); }

        /// <summary>
        /// 得到RTU列表
        /// </summary>
        /// <returns></returns>
        public IList<YY_RTU_Basic> GetRTUList() 
        {
            return  dt.Select<YY_RTU_Basic>("YY_RTU_Basic",new string[]{"*"},"");
        }

        /// <summary>
        /// 得到RTU手机卡号列表
        /// </summary>
        /// <returns></returns>
        public IList<YY_RTU_WORK > GetRTUMobileList() 
        {
            return dt.Select<YY_RTU_WORK>("YY_RTU_WORK",new string[]{"*"},"");
        }


        /// <summary>
        /// 得到RTU召测列表
        /// </summary>
        /// <returns></returns>
        public IList<YY_RTU_COMMAND> GetRTUCommandList() 
        {
            return dt.Select<YY_RTU_COMMAND>("YY_RTU_COMMAND", new string[] { "*" },  "where state=1");
        }

        /// <summary>
        /// 添加实时数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="ItemID">监测项号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Value">数据值</param>
        /// <returns></returns>
        public bool AddRealTimeData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value) 
        {
            YY_DATA_AUTO model=new YY_DATA_AUTO();
            model.STCD=STCD;
            model.ItemID =ItemID;
            model.TM =TM;
            model.DOWNDATE =RTM;
            model.NFOINDEX =NFOINDEX;
            model.DATAVALUE =Value;
            //model.DATATYPE = null;
            return dt.Insert<YY_DATA_AUTO>("YY_DATA_AUTO", model);
        }

        /// <summary>
        /// 添加终端状态和报警状态
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Alarms">报警数据</param>
        /// <returns></returns>
        public bool AddRTUState(string STCD, DateTime TM, DateTime RTM, int NFOINDEX, string AlarmsStr) 
        {
            YY_DATA_STATE model = new YY_DATA_STATE();
            model.STCD = STCD;
            model.TM = TM;
            model.DOWNDATE = RTM;
            model.NFOINDEX = NFOINDEX;
            model.STATEDATA = AlarmsStr;
            return dt.Insert<YY_DATA_STATE>("YY_DATA_STATE", model);
        }

        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeData()
        {
            string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID  order by YY_DATA_AUTO.TM desc";
            string[] fields = new string[] { "top 100 YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.DATAVALUE", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemDecimal" };
            return dt.Select(name, fields, "");
        }
        // <summary>
        /// 根据测站站号得到前100条数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeData(string STCD)
        {
            string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID";
            string[] fields = new string[] { "top 100 YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.DATAVALUE", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemDecimal" };
            string where = "where YY_RTU_Basic.STCD='" + STCD + "'  order by YY_DATA_AUTO.TM desc";
            return dt.Select(name, fields, where);
        }

        /// <summary>
        /// 得到各站的最新一条数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeNewData()
        {
            string name = "YY_DATA_AUTO AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN  YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] { "YY_RTU_Basic.STCD", "t.ItemID", "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.DATAVALUE", "t.DATATYPE", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemName", "YY_RTU_ITEM.ItemDecimal" };
            string where = "WHERE (NOT EXISTS (SELECT     1 AS Expr1 FROM    YY_DATA_AUTO  WHERE      (STCD = t.STCD) AND (TM > t.TM)))";
            return dt.Select(name, fields, where);
        }


        #region 设备状态查询相关方法
        /// <summary>
        /// 根据测站站号得到前100条状态数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
        public System.Data.DataTable GetRTUState(string STCD) 
        {
            string name = "YY_DATA_STATE RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_STATE.STCD = YY_RTU_Basic.STCD ";
            string[] fields = new string[] { "TOP 100 YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_DATA_STATE.TM", "YY_DATA_STATE.DOWNDATE", "YY_DATA_STATE.NFOINDEX", "YY_DATA_STATE.STATEDATA" };
            string where = "where YY_RTU_Basic.STCD='" + STCD + "' ORDER BY YY_DATA_STATE.TM DESC";
            return dt.Select(name, fields, where);
        }

        /// <summary>
        /// 得到各测站最新一条状态数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRTUNewState() 
        {
            string name = "YY_DATA_STATE AS t right OUTER JOIN YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] { "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.STATEDATA", "YY_RTU_Basic.NiceName", "YY_RTU_Basic.STCD" };
            string where = "WHERE     (NOT EXISTS (SELECT     1 AS Expr1  FROM  YY_DATA_STATE WHERE      (STCD = t.STCD) AND (TM > t.TM)))";
            return dt.Select(name, fields, where);
        }

        /// <summary>
        /// 创建符合格式的状态列表
        /// </summary>
        /// <param name="datatable">终端状态数据表</param>
        /// <returns></returns>
        public System.Data.DataTable CreateRTUStateDataTable(System.Data.DataTable datatable) 
        {
            IList<YY_STATE> list = dt.Select<YY_STATE>("YY_STATE", new string[] { "*" }, "order by STATEID asc");

            System.Data.DataTable DT = new System.Data.DataTable();
            DT.Columns.Add("站号");
            DT.Columns.Add("站名");
            DT.Columns.Add("接收时间");
            DT.Columns.Add("采集时间");
            DT.Columns.Add("信道");
            for (int i = 0; i < list.Count ; i++)
            {
                DT.Columns.Add(list[i].RTUSTATE);
            }

            for (int i = 0; i < datatable.Rows.Count ; i++)
            {
                System.Data.DataRow dr =DT.NewRow();
                dr["站号"] = datatable.Rows[i]["STCD"].ToString();
                dr["站名"] = datatable.Rows[i]["NiceName"].ToString();
                dr["接收时间"] = datatable.Rows[i]["TM"].ToString();
                dr["采集时间"] = datatable.Rows[i]["DOWNDATE"].ToString();
                dr["信道"] = datatable.Rows[i]["NFOINDEX"].ToString();

                string statedata = datatable.Rows[i]["STATEDATA"].ToString();
                for (int j = 0; j < statedata.Length ; j++)
                {
                    dr[5 + j] = statedata[j].ToString();
                }
                DT.Rows.Add(dr);
            }

            return DT;
            
        }
        #endregion
    }

    static class PublicBD 
    {
        public  static DBBussiness db =null;

        static  PublicBD() 
        {
            db = new DBBussiness();
        }

    }
}
