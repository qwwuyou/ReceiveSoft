using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HJT212_2005
{
    class ParseData
    {
        public bool GetValidataLength(string datastr) //报文长度合法
        {
            try
            {
                datastr = datastr.Replace("\r", "").Replace("\n", "");
                if (int.Parse(datastr.Substring(0, 4)) == datastr.Length - 8)
                { return true; }
                else { return false; }
            }
            catch { return false; }
        }

        public string GetMN(string datastr) //得到MN
        {
            return datastr.Substring(datastr.IndexOf("MN=") + 3, 14);
        }

        public string GetCN(string datastr) //区分报文类型
        {
            return datastr.Substring(datastr.IndexOf("CN=") + 3, 4);
        }

        public string GetST(string datastr) //污染源类型
        {
            return datastr.Substring(datastr.IndexOf("ST=") + 3, 2);
        }

        public DateTime GetDataTime(string datastr) //得到时间
        {
            DateTime dt=DateTime.Now ;
            DateTime.TryParse(datastr.Substring(datastr.IndexOf("DataTime=") + 9, 4) + "-" + datastr.Substring(datastr.IndexOf("DataTime=") + 13, 2) + "-" + datastr.Substring(datastr.IndexOf("DataTime=") + 15, 2) + " " + datastr.Substring(datastr.IndexOf("DataTime=") + 17, 2) + ":" + datastr.Substring(datastr.IndexOf("DataTime=") + 19, 2) + ":" + datastr.Substring(datastr.IndexOf("DataTime=") + 21, 2), out dt);
            return dt;
        }

        public CN_DataList GetRtdValues(string datastr) //得到监测元素和所对应的实时值--用于功能码2011
        {
            CN_DataList model = new CN_DataList();
            List<DataModel> DM = new List<DataModel>();
            model.CN = GetCN(datastr);
            model.TM =GetDataTime (datastr);
            model .ST =GetST(datastr);
            datastr = datastr.Replace(" ", "");
            if (datastr.LastIndexOf("&") == (datastr.Length - 5))
            { datastr =datastr.Split(new string[]{"&&"},StringSplitOptions.None )[1]; }

            string[] strs1 = datastr.Split(new string[] { "DataTime=" }, StringSplitOptions.None);
            if (strs1.Length == 2 && strs1[1].Length > 7) //7由"**-Rtd="来
            {
                string[] strs2 = strs1[1].Split(new char[] { ';', ',' });
                DataModel dm;
                foreach (var item in strs2)
                {
                    //实时数据
                    if (item.ToLower().Contains("-rtd"))
                    {
                        string[] strs3 = item.Split(new char[] { '-', '=' });
                        if (strs3.Length == 3)
                        {
                            decimal val;
                            if (decimal.TryParse(strs3[2], out val))
                            {
                                dm = new DataModel();
                                dm.ItemCode = strs3[0];
                                dm.KEY = "Rtd";
                                dm.DATAVALUE = val;
                                DM.Add(dm);
                            }
                            else
                            {
                                #region //坐标数据
                                if (strs3[2].IndexOf('N') == 0 || strs3[2].IndexOf('E') == 0)
                                {
                                    if (decimal.TryParse(strs3[2].Substring(1, strs3[2].Length - 1), out val))
                                    {
                                        dm = new DataModel();
                                        dm.ItemCode = strs3[0];
                                        dm.KEY = "Rtd";
                                        dm.DATAVALUE = val;
                                        DM.Add(dm);
                                    }

                                }
                                #endregion
                            }

                        }
                    }
                    //状态数据 
                    else if (item.ToLower().Contains("-flag"))
                    {
                        string[] strs3 = item.Split(new char[] { '-', '=' });
                        if (strs3.Length == 3)
                        {
                            decimal val;
                            if (decimal.TryParse(strs3[2], out val))
                            {
                                dm = new DataModel();
                                dm.ItemCode = strs3[0];
                                dm.KEY = "Flag";
                                dm.DATAVALUE = val;
                                DM.Add(dm);
                            }
                            else
                            {
                                dm = new DataModel();
                                dm.ItemCode = strs3[0];
                                dm.KEY = "Flag";
                                dm.DATAVALUE = Convert.ToInt32(Enum.Parse(typeof(Flag), strs3[2]));
                                DM.Add(dm);
                            }
                        }
                    }
                }
                model.DM = DM;
            }
            return model;
        }

        public CN_DataList GetValues(string datastr)    //得到监测元素和所对应的时段值--用于功能码2031日、2051分钟、2061小时
        {
            //ST=32;CN=2051;QN=20040516010101001;PW=123456;MN=88888880000001;PNO=1;PNUM=1;CP=&&DataTime=20040516021000;B01-Cou=200;101-Cou=2.5,101-Min=1.1,101-Avg=1.1,101-Max=1.1;102-Cou=2.5,102-Min=2.1,102-Avg=2.1,102-Max=2.1&& 
            CN_DataList model = new CN_DataList();
            List<DataModel> DM = new List<DataModel>();
            model.CN = GetCN(datastr);
            model.TM = GetDataTime(datastr);
            model.ST = GetST(datastr);
            datastr = datastr.Replace(" ", "");
            if (datastr.LastIndexOf("&") == (datastr.Length - 5))
            { datastr = datastr.Split(new string[] { "&&" }, StringSplitOptions.None)[1]; }

            string[] strs1 = datastr.Split(new string[] { "DataTime=" }, StringSplitOptions.None);
            //20040516021000;B01-Cou=200;101-Cou=2.5,101-Min=1.1,101-Avg=1.1,101-Max=1.1;102-Cou=2.5,102-Min=2.1,102-Avg=2.1,102-Max=2.1
            if (strs1.Length == 2 && strs1[1].Length > 7) //7由"**-Avg="来
            {
                string[] strs2=strs1[1].Split(new char[] { ';', ',' });
                DataModel dm ;
                foreach (var item in strs2)
                {
                    if (item.Contains("-"))
                    {
                        string[] strs3 = item.Split(new char[] { '-', '=' });
                        if (strs3.Length == 3) 
                        {
                            decimal val;
                            if (decimal.TryParse(strs3[2], out val))
                            {
                                dm = new DataModel();
                                dm.ItemCode = strs3[0];
                                dm.KEY = strs3[1];
                                dm.DATAVALUE = val;
                                DM.Add(dm);
                            }
                            
                        }
                        
                        
                    }
                }
            }
            model.DM = DM;
            return model;
        }

        #region 原方法，已过期
        //public ArrayList GetItem_Value(string datastr)//得到监测元素和所对应的实时值
        //{
        //    System.Collections.ArrayList al = new System.Collections.ArrayList();
        //    string[] str;
        //    string[] strs = datastr.Split(new string[] { "&&" }, StringSplitOptions.None)[1].Split(';');
        //    for (int i = 0; i < strs.Length; i++)
        //    {
        //        if (strs[i].Contains("Rtd"))
        //        {
        //            str = new string[2];
        //            str[0] = strs[i].Split(',')[0].Split('=')[0];
        //            str[1] = strs[i].Split(',')[0].Split('=')[1];
        //            al.Add(str);
        //        }
        //    }
        //    return al;
        //}

        //public string GetMaxItem(string datastr) //得到Max监测元素
        //{
        //    string str = datastr.Substring(datastr.IndexOf("Avg="), datastr.IndexOf("Max") + 3 - datastr.IndexOf("Avg="));
        //    return str.Split(new char[] { ',' })[1];
        //}

        //public string GetMinItem(string datastr) //得到Min监测元素
        //{
        //    string str = datastr.Substring(datastr.IndexOf("DataTime="), datastr.IndexOf("Min") + 3 - datastr.IndexOf("DataTime="));
        //    return str.Split(new char[] { ';' })[1];
        //}

        //public string GetAvgItem(string datastr) //得到Avg监测元素
        //{
        //    string str = datastr.Substring(datastr.IndexOf("Min="), datastr.IndexOf("Avg") + 3 - datastr.IndexOf("Min="));
        //    return str.Split(new char[] { ',' })[1];
        //}

        //public string GetData_MaxVal(string datastr)
        //{
        //    string str = datastr.Substring(datastr.IndexOf("Max=") + 4, datastr.Length - (datastr.IndexOf("Max=") + 4));
        //    return str.Substring(0, str.IndexOf("&&"));
        //}
        //public string GetData_MinVal(string datastr)
        //{
        //    string str = datastr.Substring(datastr.IndexOf("Min=") + 4, datastr.IndexOf("-Avg") - (datastr.IndexOf("Min=") + 4));
        //    return str.Substring(0, str.IndexOf(","));
        //}
        //public string GetData_AvgVal(string datastr)
        //{
        //    string str = datastr.Substring(datastr.IndexOf("Avg=") + 4, datastr.IndexOf("-Max") - (datastr.IndexOf("Avg=") + 4));
        //    return str.Substring(0, str.IndexOf(","));
        //}
        #endregion

        /// <summary>
        /// 对于污染源（ P：电源故障、F：排放源停运、 C：校验、M：维护、 T：超测上限、 D：故障、 S：设定值、 N：正常）
        /// 对于空气检测站（ 0：校准数据、 1：气象参数、 2：异常数据、 3 正常数据 
        /// </summary>
        public enum Flag
        {
            P = 11,
            C = 12,
            M = 13,
            T = 14,
　          D=15,
            S= 16,
            N=17
        
        }

        /// <summary>
        /// 时段数据的关键字 Cou-累计值 Max-最大值 Min-最小值 Avg平均值
        /// </summary>
        public enum Key 
        {
            Cou=1,
            Max=2,
            Min=3,
            Avg=4
        }
        }
}
