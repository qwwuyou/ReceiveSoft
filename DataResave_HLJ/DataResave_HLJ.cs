using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;
using Service.Model;
using System.Data;

namespace DataResave_HLJ
{
    public  class DataResave_HLJ : ResaveProcess
    {
        string FieldPath = System.Windows.Forms.Application.StartupPath + "/Log/DataResaveHLJ";
        public DataResave_HLJ()
        {
            string FileName = "Warn" + DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                msdb = new SqlDBBussiness(System.Windows.Forms.Application.StartupPath + "/Resave_hlj.xml");
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "读取HLJ转发数据库信息失败"); }
            RainList = new List<YY_DATA_AUTO>();
            RiverList = new List<YY_DATA_AUTO>();
            RsvrList = new List<YY_DATA_AUTO>();

            try
            {
                RainList_Init();
                RiverList_Init();
                RsvrList_Init();
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "初始化水位（河道、水库）、雨量列时失败"); }
        }
        public static List<YY_DATA_AUTO> RainList;
        public static List<YY_DATA_AUTO> RiverList;
        public static List<YY_DATA_AUTO> RsvrList;

        public void InitInfo()
        {
            ReResaveToHLJDB();
        }

        public void Resave(YY_DATA_AUTO model)
        {
            
            decimal d=0;
            if (decimal.TryParse(model.CorrectionVALUE.ToString(), out d))
            {
                if(d<9999)
                    UpdRain_River_RsvrList(model);
            }
        }


        public SqlDBBussiness msdb;
       

        public void ReResaveToHLJDB() 
        {

            string FileName = "Warn" + DateTime.Now.ToString("yyyy-MM-dd") ;
            try
            {
                msdb = new SqlDBBussiness(System.Windows.Forms.Application.StartupPath + "/Resave_hlj.xml");
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "重新读取HLJ转发数据库信息失败"); }
            RainList = new List<YY_DATA_AUTO>();
            RiverList = new List<YY_DATA_AUTO>();
            RsvrList = new List<YY_DATA_AUTO>();

            try
            {
                RainList_Init();
                RiverList_Init();
                RsvrList_Init();
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "初始化水位（河道、水库）、雨量列时失败"); }
        }

        
        #region 初始化雨量、河道水位、水库水位表
         void RainList_Init()
        {

            DataTable datatable = PublicBD.db.GetRealTimeNewData(new string[] { "26" });
            if (datatable != null && datatable.Rows.Count > 0)
            {
                YY_DATA_AUTO model;
                foreach (DataRow row in datatable.Rows)
                {
                    model = new YY_DATA_AUTO();
                    model.STCD = row["STCD"].ToString();
                    model.ItemID = row["ItemID"].ToString();
                    model.DOWNDATE = DateTime.Parse(row["DOWNDATE"].ToString());
                    model.TM = DateTime.Parse(row["TM"].ToString());
                    model.CorrectionVALUE = decimal.Parse(row["CorrectionVALUE"].ToString());
                    model.STTYPE = row["STTYPE"].ToString();
                    RainList.Add(model);
                }
            }
        }

         void RiverList_Init()
        {
            DataTable datatable = PublicBD.db.GetRealTimeNewData(new string[] { "39" }, "48");
            if (datatable != null && datatable.Rows.Count > 0)
            {
                YY_DATA_AUTO model;
                foreach (DataRow row in datatable.Rows)
                {
                    model = new YY_DATA_AUTO();
                    model.STCD = row["STCD"].ToString();
                    model.ItemID = row["ItemID"].ToString();
                    model.DOWNDATE = DateTime.Parse(row["DOWNDATE"].ToString());
                    model.TM = DateTime.Parse(row["TM"].ToString());
                    model.CorrectionVALUE = decimal.Parse(row["CorrectionVALUE"].ToString());
                    model.STTYPE = row["STTYPE"].ToString();
                    RiverList.Add(model);
                }
            }
        }

         void RsvrList_Init()
        {

            DataTable datatable = PublicBD.db.GetRealTimeNewData(new string[] { "39" },"4B");//"3B"
            if (datatable != null && datatable.Rows.Count > 0)
            {
                YY_DATA_AUTO model;
                foreach (DataRow row in datatable.Rows)
                {
                    model = new YY_DATA_AUTO();
                    model.STCD = row["STCD"].ToString();
                    model.ItemID = row["ItemID"].ToString();
                    model.DOWNDATE = DateTime.Parse(row["DOWNDATE"].ToString());
                    model.TM = DateTime.Parse(row["TM"].ToString());
                    model.CorrectionVALUE = decimal.Parse(row["CorrectionVALUE"].ToString());
                    model.STTYPE = row["STTYPE"].ToString();
                    RsvrList.Add(model);
                }
            }
        }
        #endregion

        #region 更新雨量、河道水位、水库水位表
        public  void UpdRain_River_RsvrList(YY_DATA_AUTO model) 
        {
            if (model.ItemID == "26")
            {
                YY_DATA_AUTO oldModel = null;
                var rainlist = from a in RainList where a.STCD == model.STCD select a;
                lock (RainList)
                {
                    
                    if (rainlist.Count() > 0)
                    {
                        oldModel = rainlist.First();
                        ResaveRain(oldModel, model);

                        rainlist.First().CorrectionVALUE = model.CorrectionVALUE;
                        rainlist.First().DOWNDATE = model.DOWNDATE;
                        rainlist.First().TM = model.TM;
                        
                    }
                    else
                    {
                        ResaveRain(oldModel, model);
                        RainList.Add(model);
                    }
                    
                }

            }
            else if (model.ItemID == "39"&& model.STTYPE=="48")  //河道
            {
                YY_DATA_AUTO oldModel=null;
                var riverlist = from a in RiverList where a.STCD == model.STCD select a;
                lock (RiverList)
                {
                    if (riverlist.Count() > 0)
                    {
                        oldModel = riverlist.First(); 
                        ResaveRiver(oldModel, model);

                        riverlist.First().CorrectionVALUE = model.CorrectionVALUE;
                        riverlist.First().DOWNDATE = model.DOWNDATE;
                        riverlist.First().TM = model.TM;
                        
                    }
                    else
                    {
                        ResaveRiver(oldModel, model);
                        RiverList.Add(model);
                    }

                }
            }
            else if (model.ItemID == "39" && model.STTYPE == "4B") //水库model.ItemID == "3B"
            {
                YY_DATA_AUTO oldModel = null;
                var rsvrlist = from a in RsvrList where a.STCD == model.STCD select a;
                lock (RsvrList)
                {
                    if (rsvrlist.Count() > 0)
                    {
                        oldModel = rsvrlist.First();
                        ResaveRsvr(oldModel, model);

                        rsvrlist.First().CorrectionVALUE = model.CorrectionVALUE;
                        rsvrlist.First().DOWNDATE = model.DOWNDATE;
                        rsvrlist.First().TM = model.TM;
                        
                    }
                    else
                    {
                        ResaveRsvr(oldModel, model);
                        RsvrList.Add(model);
                    }
                    
                }
            }
            else if (model.ItemID == "38") //电压
            {
                ResaveVoltage(model);
            }
        }
        #endregion

        #region 转存方法

        private  void ResaveRain(YY_DATA_AUTO oldModel, YY_DATA_AUTO newModel)
        {
            #region 转存站号只能存8位
            string _stcd = "";
            if (newModel.STCD.Length == 10)
            {
                _stcd = newModel.STCD.Substring(2, 8);
            }
            #endregion

            decimal pdr = 0;
            bool b = true;
            string FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd") ;
            string message = "";
            if (oldModel != null)
            {
                pdr = newModel.CorrectionVALUE.Value - oldModel.CorrectionVALUE.Value;
                TimeSpan span = (TimeSpan)(newModel.TM - oldModel.TM);
                if (pdr > 0 && pdr <= 4)
                {
                    if ((int)(span.TotalSeconds) > 0) //雨量大于0 ，时间差不为负值（先发后至数据不入库）
                    {
                        int k = Convert.ToInt16(pdr / 0.5m);

                        int Seconds = 60 / k;   //分配每个雨量值写入间隔
                        //可以用if( ((int)(span.TotalSeconds))>86400)  --一天  判断超过某时长 就不平均分配时间写入雨量
                        for (int i = 0; i < k; i++)
                        {
                            DateTime datetime = newModel.TM.AddSeconds(Seconds * (-1 * i));
                            b=msdb.dt.Insert("ST_YCRAIN_R", new string[] { "STCD", "TM", "SYSTM", "V" }, new object[] { _stcd, datetime, newModel.DOWNDATE, 0.5 });
                            if (!b)
                            {
                                message = "站号：" + _stcd + "   监测项：雨量   监测时间：" + datetime + "   监测值：0.5 -----转存失败";
                                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                            }
                        }
                    }
                }
                else if (pdr > 4)
                {
                    if ((int)(span.TotalSeconds) > 0) //雨量大于0 ，时间差不为负值（先发后至数据不入库）
                    {
                        int k = Convert.ToInt16(pdr / 0.5m);

                        int Seconds = ((int)(span.TotalSeconds)) / k; //分配每个雨量值写入间隔
                        //可以用if( ((int)(span.TotalSeconds))>86400)  --一天  判断超过某时长 就不平均分配时间写入雨量
                        for (int i = 0; i < k; i++)
                        {
                            DateTime datetime = newModel.TM.AddSeconds(Seconds * (-1 * i));
                            b=msdb.dt.Insert("ST_YCRAIN_R", new string[] { "STCD", "TM", "SYSTM", "V" }, new object[] { _stcd, datetime, newModel.DOWNDATE, 0.5 });
                            if (!b)
                            {
                                message = "站号：" + _stcd + "   监测项：雨量   监测时间：" + datetime + "   监测值：0.5 -----转存失败";
                                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                            }
                        }
                    }
                }

            }
            else
            {
                if (newModel.CorrectionVALUE > 0)
                {
                    //第一次回传雨量
                    b = msdb.dt.Insert("ST_YCRAIN_R", new string[] { "STCD", "TM", "SYSTM", "V" }, new object[] { _stcd, newModel.TM, newModel.DOWNDATE, 0.5 });
                    if (!b)
                    {
                        message = "站号：" + _stcd + "   监测项：雨量   监测时间：" + newModel.TM + "   监测值：0.5 -----转存失败";
                        Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                    }
                }
            }

        }
        private  void ResaveRiver(YY_DATA_AUTO oldModel,YY_DATA_AUTO newModel) 
        {
            #region 转存站号只能存8位
            string _stcd = "";
            if (newModel.STCD.Length == 10)
            {
                _stcd = newModel.STCD.Substring(2, 8);
            }
            #endregion
            bool b = true;
            string FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd") ;
            string message = "";
            int wptn = 6;  //落4，涨5，平6
            if(oldModel !=null)
            if (oldModel.CorrectionVALUE > newModel.CorrectionVALUE)
            { wptn = 4; }
            else if (oldModel.CorrectionVALUE < newModel.CorrectionVALUE)
            { wptn = 5; }
            
            b=msdb.dt.Insert("ST_YCRIVER_R", new string[] { "STCD", "TM", "Z", "WPTN" }, new object[] { _stcd, newModel.TM, newModel.CorrectionVALUE, wptn });
            if (!b)
            {
                message = "站号：" + _stcd + "   监测项：河道水位   监测时间：" + newModel.TM + "   监测值：" + newModel.CorrectionVALUE + " -----转存失败";
                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
            }
        }
        private  void ResaveRsvr(YY_DATA_AUTO oldModel, YY_DATA_AUTO newModel)
        {
            #region 转存站号只能存8位
            string _stcd = "";
            if (newModel.STCD.Length == 10)
            {
                _stcd = newModel.STCD.Substring(2, 8);
            }
            #endregion
            bool b = true;
            string FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd") ;
            string message = "";
            int rwptn = 6;  //落4，涨5，平6
            if (oldModel != null)
                if (oldModel.CorrectionVALUE > newModel.CorrectionVALUE)
                { rwptn = 4; }
                else if (oldModel.CorrectionVALUE < newModel.CorrectionVALUE)
                { rwptn = 5; }

            b=msdb.dt.Insert("ST_YCRSVR_R", new string[] { "STCD","TM","RZ","RWPTN" }, new object[] { _stcd, newModel.TM, newModel.CorrectionVALUE, rwptn });
            if (!b)
            {
                message = "站号：" + _stcd + "   监测项：水库水位   监测时间：" + newModel.TM + "   监测值：" + newModel.CorrectionVALUE + " -----转存失败";
                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
            }
        }
        private  void ResaveVoltage(YY_DATA_AUTO model)
        {
            #region 转存站号只能存8位
            string _stcd = "";
            if (model.STCD.Length == 10)
            {
                _stcd = model.STCD.Substring(2, 8);
            }
            #endregion
            bool b = true;
            string FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd") ;
            string message = "";
            //只每天存4次
            if (model.TM.ToString("yyyy-MM-dd HH:mm:ss") == model.TM.ToString("yyyy-MM-dd 02:00:00") || model.TM.ToString("yyyy-MM-dd HH:mm:ss") == model.TM.ToString("yyyy-MM-dd 08:00:00") || model.TM.ToString("yyyy-MM-dd HH:mm:ss") == model.TM.ToString("yyyy-MM-dd 14:00:00") || model.TM.ToString("yyyy-MM-dd HH:mm:ss") == model.TM.ToString("yyyy-MM-dd 20:00:00"))
            {
                b=msdb.dt.Insert("ST_RUNINFO_R", new string[] { "STCD", "TM", "SYSTM", "VOLTAGE" }, new object[] { _stcd, model.TM, DateTime.Now, model.CorrectionVALUE });
                if (!b)
                {
                    message = "站号：" + _stcd + "   监测项：电压   监测时间：" + model.TM + "   监测值：" + model.CorrectionVALUE + " -----转存失败";
                    Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                }
            }
        }
        #endregion

    }
}
