using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;

namespace Service
{
    static class PackageProcess 
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        

        //得到工作模式用来回复时 下发给rtu
        private static string GetElmData(string STCD) 
        {
            string ElmData = "00";
            if (ServiceBussiness.CONFIGDATAList != null) 
            {
                var list = from mode in ServiceBussiness.CONFIGDATAList where mode.STCD == STCD && mode.ItemID == "0000000000" && mode.ConfigID == "120000000012" select mode;
                if (list.Count() > 0)
                {
                    int mode=0;
                    if (int.TryParse(list.First().ConfigVal, out mode))
                    {
                        ElmData = mode.ToString("00");
                    }
                }
            }
            return ElmData;
        }

        //打包类
        static  YanYu.WRIMR.Protocol.Pack p = new YanYu.WRIMR.Protocol.Pack();

        public static void Process_LinkCheck_Package()
        { }


        //入实时库&报警库
        private static string AddRealTimeData_RTUState(YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType, string STCD,IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality, string AlarmsStr, DateTime TM, DateTime RTM, IList<double> data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                #region
                string Explain = "接收到自报数据，";
                bool B = false;//报警数据入库标识
                switch (DataType)
                {

                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Alarm_OR_Status_Self_Reported: //报警或状态参数 0013000001
                        //如果有报警信息入库
                        if (AlarmsStr != null & AlarmsStr != "")
                        {
                            B = PublicBD.db.AddRTUState(STCD, TM, RTM, (int)NFOINDEX, AlarmsStr);
                            Explain = "接收到报警数据，时间[" + TM + "],报警值[" + AlarmsStr + "]";
                        }
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Barometric_Pressure_Self_Reported://气压数据 0007000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00070000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[气压-" + "00070000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                }
                                else 
                                {
                                    Explain += "                                                                 " +
                                        "数据特征[气压-" + "00070000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Confirm://确认
                        #region SLD & 流量计算
                        if (data.Count() == 11)
                        {
                            //段位 -解包得到（段位、水位）
                            List<decimal> Section = new List<decimal>();
                            for (int i = 0; i < 9; i++)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                Section.Add(Value);
                                //段位入库
                                PublicBD.db.AddRealTimeData(STCD, "00160000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[段位流速" + (i + 1) + "-" + "00160000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                        "数据特征[段位流速" + (i + 1) + "-" + "00160000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                            }


                            decimal WaterLevel = Convert.ToDecimal(data[9].ToString());

                            //数据库得到
                            List<int> SectionIndex;
                            decimal Elevation = 0;
                            List<Stage> StageList;
                            decimal? kA = null;
                            decimal LL = 0;
                            GetSLD(STCD, out SectionIndex, out Elevation, out StageList);
                            if (SectionIndex != null && StageList != null && SectionIndex.Count > 0 && StageList.Count > 0 && WaterLevel > 0.15m) //水位必须大于15厘米监测数据才有效
                            {
                                decimal LS = 0;
                                foreach (int Index in SectionIndex)
                                {
                                    LS += Section[Index - 1];
                                }
                                LS = LS / SectionIndex.Count();


                                try
                                {
                                    kA = GetKaValue(Elevation + WaterLevel, StageList);
                                }
                                catch
                                {
                                    kA = null;
                                }
                                decimal ka = 0;
                                if (decimal.TryParse(kA.ToString(), out ka))
                                {
                                    LL = LS * ka;

                                    //流量入库
                                    PublicBD.db.AddRealTimeData(STCD, "0003000001", TM, RTM, (int)NFOINDEX, LL);
                                    if (Explain == "接收到自报数据，")
                                    {
                                        Explain += "数据特征[流量-" + "0003000001" + "],时间[" + TM + "],值[" + LL.ToString("f3") + "]\n";
                                    }
                                    else
                                    {
                                        Explain += "                                                                 " +
                                            "数据特征[流量-" + "0003000001" + "],时间[" + TM + "],值[" + LL.ToString("f3") + "]\n";
                                    }
                                }

                            }



                            //水位入库
                            if (Convert.ToDecimal(data[9].ToString()) == 0)
                            {
                                WaterLevel = 0;
                            }
                            else
                            {
                                WaterLevel = Convert.ToDecimal(data[9].ToString()) + Elevation;
                            }
                            PublicBD.db.AddRealTimeData(STCD, "0002000001", TM, RTM, (int)NFOINDEX, WaterLevel);
                            if (Explain == "接收到自报数据，")
                            {
                                Explain += "数据特征[水位-" + "0002000001" + "],时间[" + TM + "],值[" + WaterLevel.ToString("f3") + "]\n";
                            }
                            else
                            {
                                Explain += "                                                                 " +
                                    "数据特征[水位-" + "0002000001" + "],时间[" + TM + "],值[" + WaterLevel.ToString("f3") + "]\n";
                            }

                            //电压入库
                            decimal Voltage = Convert.ToDecimal(data[10].ToString());
                            PublicBD.db.AddRealTimeData(STCD, "0017000001", TM, RTM, (int)NFOINDEX, Voltage);
                            if (Explain == "接收到自报数据，")
                            {
                                Explain += "数据特征[电压-" + "0017000001" + "],时间[" + TM + "],值[" + Voltage.ToString("f2") + "]\n";
                            }
                            else
                            {
                                Explain += "                                                                 " +
                                    "数据特征[电压-" + "0017000001" + "],时间[" + TM + "],值[" + Voltage.ToString("f2") + "]\n";
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Evaporation_Self_Reported://蒸发 0012000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00120000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[蒸发量-" + "00120000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                              "数据特征[蒸发量-" + "00120000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Flow_Self_Reported: //流量水量 0003000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00030000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[流量水量-" + "00030000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[流量水量-" + "00030000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                            
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Gate_Position_Self_Reported://闸位 0005000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00050000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[闸位-" + "00050000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[闸位-" + "00050000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Power_Self_Reported: //功率 0006000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00060000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[功率-" + "00060000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[功率-" + "00060000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Rainfall_Self_Reported://雨量  0001000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00010000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[雨量-" + "00010000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[雨量-" + "00010000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Soil_Moisture_Content_Self_Reported://土壤含水率 0011000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00110000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[土壤含水率-" + "00110000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[土壤含水率-" + "00110000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Statistics_Rrainfall_Reported://统计雨量 0014000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00140000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[统计雨量-" + "00140000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[统计雨量-" + "00140000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Velocity_of_Flow_Self_Reported://流速 0004000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00040000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[流速-" + "00040000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[流速-" + "00040000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Pressure_Self_Reported://水压 0015000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00150000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[水压-" + "00150000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                        "数据特征[水压-" + "00150000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Quality_Self_Reported://水质参数 0010000001
                        #region 入库
                        IList<YanYu.WRIMR.Protocol.WaterQuality> wq = WaterQuality;
                        foreach (var item in wq)
                        {
                            switch (item.Type)
                            {
                                case YanYu.WRIMR.Protocol.WaterQualityType.WaterTemperature: //水温(水质) 0010000001
                                    if (item.Value != null)
                                    {
                                        decimal Value = Convert.ToDecimal(item.Value.ToString());
                                        bool b = PublicBD.db.AddRealTimeData(STCD, "0010000001", TM, RTM, (int)NFOINDEX, Value);
                                        if (Explain == "接收到自报数据，")
                                        {
                                            Explain += "数据特征[水质的水温-" + "0010000001" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                        else
                                        {
                                            Explain += "                                                                 " +
                                               "数据特征[水质的水温-" + "0010000001" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                    }
                                    break;
                                case YanYu.WRIMR.Protocol.WaterQualityType.PH: //0010000101
                                    if (item.Value != null)
                                    {
                                        decimal Value = Convert.ToDecimal(item.Value.ToString());
                                        bool b = PublicBD.db.AddRealTimeData(STCD, "0010000101", TM, RTM, (int)NFOINDEX, Value);
                                        if (Explain == "接收到自报数据，")
                                        {
                                            Explain += "数据特征[PH-" + "0010000101" + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                        }
                                        else
                                        {
                                            Explain += "                                                                 " +
                                               "数据特征[PH-" + "0010000101" + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                        }
                                    }
                                    break;
                                case YanYu.WRIMR.Protocol.WaterQualityType.PermanganateIndex: //高锰酸盐指数 0010000301
                                    if (item.Value != null)
                                    {
                                        decimal Value = Convert.ToDecimal(item.Value.ToString());
                                        bool b = PublicBD.db.AddRealTimeData(STCD, "0010000301", TM, RTM, (int)NFOINDEX, Value);
                                        if (Explain == "接收到自报数据，")
                                        {
                                            Explain += "数据特征[高锰酸盐指数-" + "0010000301" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                        else
                                        {
                                            Explain += "                                                                 " +
                                                "数据特征[高锰酸盐指数-" + "0010000301" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                    }
                                    break;
                                case YanYu.WRIMR.Protocol.WaterQualityType.DissolvedOxygen: //溶解氧 0010000201
                                    if (item.Value != null)
                                    {
                                        decimal Value = Convert.ToDecimal(item.Value.ToString());
                                        bool b = PublicBD.db.AddRealTimeData(STCD, "0010000201", TM, RTM, (int)NFOINDEX, Value);
                                        if (Explain == "接收到自报数据，")
                                        {
                                            Explain += "数据特征[溶解氧-" + "0010000201" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                        else
                                        {
                                            Explain += "                                                                 " +
                                               "数据特征[溶解氧-" + "0010000201" + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                        }
                                    }
                                    break;
                                case YanYu.WRIMR.Protocol.WaterQualityType.Conductivity: //电导率 0010000401
                                    if (item.Value != null)
                                    {
                                        decimal Value = Convert.ToDecimal(item.Value.ToString());
                                        bool b = PublicBD.db.AddRealTimeData(STCD, "0010000401", TM, RTM, (int)NFOINDEX, Value);
                                        if (Explain == "接收到自报数据，")
                                        {
                                            Explain += "数据特征[电导率-" + "0010000401" + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                        }
                                        else
                                        {
                                            Explain += "                                                                 " +
                                               "数据特征[电导率-" + "0010000401" + "],时间[" + TM + "],值[" + Value.ToString("f0") + "]\n";
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                        #endregion
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Temperature_Self_Reported://水温 0009000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00090000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[水温-" + "00090000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[水温-" + "00090000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f1") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Waterlevel_Self_Reported:  //水位参数  0002000001  0002000002   0002000003
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00020000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[水位-" + "00020000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                       "数据特征[水位-" + "00020000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f3") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Wind_Velocity_Self_Reported://风速参数 0008000001
                        #region  入库
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (data[i] != null)
                            {
                                decimal Value = Convert.ToDecimal(data[i].ToString());
                                bool b = PublicBD.db.AddRealTimeData(STCD, "00080000" + (i + 1).ToString("00"), TM, RTM, (int)NFOINDEX, Value);
                                if (Explain == "接收到自报数据，")
                                {
                                    Explain += "数据特征[风速-" + "00080000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                                else
                                {
                                    Explain += "                                                                 " +
                                      "数据特征[风速-" + "00080000" + (i + 1).ToString("00") + "],时间[" + TM + "],值[" + Value.ToString("f2") + "]\n";
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }

                //如果有报警信息入库
                if (AlarmsStr != null && AlarmsStr != "" && !B)
                {
                    bool b = PublicBD.db.AddRTUState(STCD, TM, RTM, (int)NFOINDEX, AlarmsStr);
                    if (Explain == "接收到自报数据，")
                        Explain = "接收到报警数据，时间[" + TM + "],报警值[" + AlarmsStr + "]";
                    else
                        Explain += "                                                                 " +
                                   "接收到报警数据，时间[" + TM + "],报警值[" + AlarmsStr + "]";
                }

                return Explain;
                #endregion
            }
            catch (Exception ex)
            { 
                log.Error(DateTime.Now + "C0入库操作异常" + ex.ToString());
                return "C0操作异常";
            }
        }

        //实时数据&报警数据通知UI
        private static void RealTimeData_RTUStateToUI(YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType, string STCD, IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality,string AlarmsStr, DateTime RTM, IList<double> data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region
            StringBuilder Explain =new StringBuilder() ;
            bool B = false;//报警数据标识
            switch (DataType)
            {

                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Alarm_OR_Status_Self_Reported: //报警或状态参数 0013000001
                    //如果有报警信息
                    if (AlarmsStr != null & AlarmsStr != "")
                    {
                        Explain.Append("["+RTM.ToString("yyyy-MM-dd HH:mm:ss")+"]报警信息:" + AlarmsStr+"\n");
                    }
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Barometric_Pressure_Self_Reported://气压数据 0007000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]气压数据:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Confirm://确认
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Evaporation_Self_Reported://蒸发参数 0012000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]蒸发参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Flow_Self_Reported: //流量水量 0003000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]流量水量:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Gate_Position_Self_Reported://闸位 0005000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]闸位:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Power_Self_Reported: //功率 0006000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]功率:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Rainfall_Self_Reported://雨量  0001000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]雨量:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Soil_Moisture_Content_Self_Reported://土壤含水率 0011000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]土壤含水率:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Statistics_Rrainfall_Reported://统计雨量 0014000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]统计雨量:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Velocity_of_Flow_Self_Reported://流速参数 0004000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]流速参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Pressure_Self_Reported://水压参数 0015000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]水压参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Quality_Self_Reported://水质参数 0010000001
                    #region 
                    IList<YanYu.WRIMR.Protocol.WaterQuality> wq = WaterQuality;
                    foreach (var item in wq)
                    {
                        switch (item.Type)
                        {
                            case YanYu.WRIMR.Protocol.WaterQualityType.WaterTemperature: //水温(水质) 0010000001
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]水温(水质):" + Value + "\n");
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.PH: //0010000101
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]PH:" + Value + "\n");
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.PermanganateIndex: //高锰酸盐指数 0010000301
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]高锰酸盐指数:" + Value + "\n");
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.DissolvedOxygen: //溶解氧 0010000201
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]溶解氧:" + Value + "\n");
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.Conductivity: //电导率 0010000401
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]电导率:" + Value + "\n");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                    #endregion
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Temperature_Self_Reported://水温参数 0009000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]水温参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Waterlevel_Self_Reported:  //水位参数  0002000001  0002000002   0002000003
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]水位参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Wind_Velocity_Self_Reported://风速参数 0008000001
                    #region  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != null)
                        {
                            decimal Value = Convert.ToDecimal(data[i].ToString());
                            Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]风速参数:" + Value + "\n");
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }

            //如果有报警信息
            if (AlarmsStr != null && AlarmsStr != "" && !B)
            {
                Explain.Append("[" + RTM.ToString("yyyy-MM-dd HH:mm:ss") + "]报警信息:" + AlarmsStr + "\n");
            }
            #endregion

            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain.ToString(), new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain.ToString(), new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
        }

        //入固态数据库
        private static void AddRemData(YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType, string STCD, IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality,  DateTime[] TMs, DateTime RTM, IList<double> data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            for (int j = 0; j < TMs.Length; j++)
            {
                DateTime TM = TMs[j];
                #region

            switch (DataType)
            {
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Barometric_Pressure_Self_Reported://气压数据 0007000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0007000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Confirm://确认
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Evaporation_Self_Reported://蒸发参数 0012000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0012000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Flow_Self_Reported: //流量水量 0003000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0003000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Gate_Position_Self_Reported://闸位 0005000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0005000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Power_Self_Reported: //功率 0006000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0006000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Rainfall_Self_Reported://雨量  0001000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0001000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Soil_Moisture_Content_Self_Reported://土壤含水率 0011000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0011000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Statistics_Rrainfall_Reported://统计雨量 0014000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0014000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Velocity_of_Flow_Self_Reported://流速参数 0004000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0004000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Pressure_Self_Reported://水压参数 0015000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0015000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Quality_Self_Reported://水质参数 0010000001
                    #region 入库
                    IList<YanYu.WRIMR.Protocol.WaterQuality> wq = WaterQuality;
                    foreach (var item in wq)
                    {
                        switch (item.Type)
                        {
                            case YanYu.WRIMR.Protocol.WaterQualityType.WaterTemperature: //水温(水质) 0010000001
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    bool b = PublicBD.db.AddRemData(STCD, "0010000001", TM, RTM, (int)NFOINDEX, Value);
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.PH: //0010000101
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    bool b = PublicBD.db.AddRemData(STCD, "0010000101", TM, RTM, (int)NFOINDEX, Value);
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.PermanganateIndex: //高锰酸盐指数 0010000301
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    bool b = PublicBD.db.AddRemData(STCD, "0010000301", TM, RTM, (int)NFOINDEX, Value);
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.DissolvedOxygen: //溶解氧 0010000201
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    bool b = PublicBD.db.AddRemData(STCD, "0010000201", TM, RTM, (int)NFOINDEX, Value);
                                }
                                break;
                            case YanYu.WRIMR.Protocol.WaterQualityType.Conductivity: //电导率 0010000401
                                if (item.Value != null)
                                {
                                    decimal Value = Convert.ToDecimal(item.Value.ToString());
                                    bool b = PublicBD.db.AddRemData(STCD, "0010000401", TM, RTM, (int)NFOINDEX, Value);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                    #endregion
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Temperature_Self_Reported://水温参数 0009000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0009000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Waterlevel_Self_Reported:  //水位参数  0002000001  0002000002   0002000003
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0002000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                case YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Wind_Velocity_Self_Reported://风速参数 0008000001
                    #region  入库
                    {
                        decimal Value = Convert.ToDecimal(data[j].ToString());
                        bool b = PublicBD.db.AddRemData(STCD, "0008000001", TM, RTM, (int)NFOINDEX, Value);
                    }
                    #endregion
                    break;
                default:
                    break;
            }

         
            #endregion
            }
        }

        //C0实时自报
        internal static void Process_C0H(YanYu.WRIMR.Protocol.InPacket.Deal0xC0 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                Dictionary<int, bool> alarms = pack.packData.Alarms;
                IList<double> data = pack.packData.Data;
                string AlarmsStr = pack.packData.AlarmsStr;
                YanYu.WRIMR.Protocol.RtuStatus RSs = pack.packData.RtuStatus;
                YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType = pack.packData.DataType;
                IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality = pack.packData.WaterQuality;
                string STCD = pack.STCD;
                DateTime TM = pack.packData.TP.SendTime;
                DateTime RTM = DateTime.Now;

                #region 生成回复数据
                byte[] sendData = null;
                var rtu = from r in ServiceBussiness.RtuList where r.STCD == pack.STCD select r;
                string ElmData = GetElmData(STCD);
                if (rtu.Count() > 0)
                {
                    int pwd = int.Parse(rtu.First().PassWord);
                    sendData = p.pack(pack.STCD, 0, 0, (int)pack.AFN, ElmData, pwd);
                }
                #endregion

                #region tcp回复
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    List<TcpService.TcpSocket> Ts = TS.Ts;

                    var tcps = from t in Ts where t.STCD == pack.STCD && t.TCPSOCKET != null select t;
                    List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                    //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                    if (sendData == null)
                    {
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        return;
                    }

                    if (Tcps.Count() > 0)
                    {
                        Tcps.First().TCPSOCKET.Send(sendData);
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                   
                }
                #endregion

                #region udp回复
                if ((int)NFOINDEX == 2)
                {
                    UdpService.UdpServer US = Server as UdpService.UdpServer;
                     List<UdpService.UdpSocket> Us = US.Us ;
                     var udps = from u in Us where u.STCD == pack.STCD && u.IpEndPoint != null select u;

                     //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                     if (sendData == null)
                     {
                         ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                         return;
                     }

                     if (udps.Count() > 0)
                     {
                         //System.Net.Sockets.UdpClient udpclient = new System.Net.Sockets.UdpClient(US.PORT);  //()绑定指定端口
                         //udpclient.Connect(udps.First().IpEndPoint);
                         //udpclient.Send(sendData, sendData.Length);
                         //udpclient.Close();

                         US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                         ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                     }
                    
                }
                #endregion

            
                //入实时库&报警库
                string Explain=  AddRealTimeData_RTUState(DataType, STCD, WaterQuality, AlarmsStr, TM, RTM, data, NFOINDEX, Server);

                #region tcp通知界面
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    //通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                #endregion

                #region udp通知界面
                if ((int)NFOINDEX == 2)
                {
                    UdpService.UdpServer US = Server as UdpService.UdpServer;
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                #endregion

                #region gsm通知界面
                if ((int)NFOINDEX == 3)
                {
                    GsmService.GsmServer GS = Server as GsmService.GsmServer;
                    Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                #endregion 

                #region com通知界面
                if ((int)NFOINDEX == 4)
                {
                    ComService.ComServer CS = Server as ComService.ComServer;
                    Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                #endregion
                ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX , Service.ServiceEnum.DataType.Text); 
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "C0自报操作异常" + ex.ToString());
            }
        }
        
        internal static void Process_02H(YanYu.WRIMR.Protocol.InPacket.Deal0x02 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            
            string Explain="";
            var rtu = from r in ServiceBussiness.RtuList where r.STCD == pack.STCD select r;
            if (rtu.Count() > 0)
            {
                int pwd=int.Parse(rtu.First().PassWord);
                if (pack.packData.LinkCheckType == YanYu.WRIMR.Protocol.LinkCheckType.Login)
                {
                    //sendData = p.pack(pack.STCD, 0, 1, (int)pack.AFN, "F0", pwd);
                    Explain = "链路状态[登录]";
                }
                else if (pack.packData.LinkCheckType == YanYu.WRIMR.Protocol.LinkCheckType.Exit)
                {
                    //sendData = p.pack(pack.STCD, 0, 1, int.Parse(pack.AFN.ToString()), "F1", pwd);
                    Explain = "链路状态[退出]";
                }
                else if (pack.packData.LinkCheckType == YanYu.WRIMR.Protocol.LinkCheckType.Online)
                {
                    //sendData = p.pack(pack.STCD, 0, 1, int.Parse(pack.AFN.ToString()), "F2", pwd);
                    Explain = "链路状态[保持在线]";
                }
                else 
                {
                    Explain = "链路状态[未知]";
                }
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion  
 
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            //GlobalApp.Instance.MsgToGUI("链路状态:" + pack.packData.LinkCheckType.ToString());
        }
        //需要商议 ---gsm
        internal static void Process_10H(YanYu.WRIMR.Protocol.InPacket.Deal0x10 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(设置)遥测终端、中继站地址修改为[" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(设置)遥测终端、中继站地址修改为[" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(设置)遥测终端、中继站地址修改为[" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(设置)遥测终端、中继站地址修改为[" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        
        internal static void Process_11H(YanYu.WRIMR.Protocol.InPacket.Deal0x11 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "时钟校准成功[" + pack.packData.Date .ToString("yyyy-MM-dd HH:mm:ss")+ "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "时钟校准成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "时钟校准成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
            
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "时钟校准成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        
        internal static void Process_12H(YanYu.WRIMR.Protocol.InPacket.Deal0x12 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                string Where = " where STCD ='"+ pack.STCD +"' and ItemID = '0000000000' and ConfigID ='120000000012'";
            var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000012" select d;
            if (list.Count() > 0) 
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                model.ConfigVal = ((int)pack.packData.WorkMode).ToString();
                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);
            } 
            if (list.Count() == 0) 
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000000012";
                model.ConfigVal = ((int)pack.packData.WorkMode).ToString();
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }


            string Explain = "";
            if ((int)pack.packData.WorkMode == 0)
            {
                Explain = "(配置)测站工作模式信息入库[兼容工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 1)
            {
                Explain = "(配置)测站工作模式信息入库[自报工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 2)
            {
                Explain = "(配置)测站工作模式信息入库[查询/应答工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 3)
            {
                Explain = "(配置)测站工作模式信息入库[调试/维修工作模式]";
            }
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "12操作异常" + ex.ToString());
            }
        }

        //internal static void Process_15H(YanYu.WRIMR.Protocol.InPacket.Deal0x15 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        //{
        //    #region tcp回复
        //    if ((int)NFOINDEX == 1)
        //    {
        //        TcpService.TcpServer TS = Server as TcpService.TcpServer;

        //        //回复通知界面
        //        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "???", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion

        //    #region udp回复
        //    if ((int)NFOINDEX == 2)
        //    {
        //        UdpService.UdpServer US = Server as UdpService.UdpServer;
        //        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "???", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion
        //}


        //internal static void Process_16H(YanYu.WRIMR.Protocol.InPacket.Deal0x16 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        //{
        //    #region tcp回复
        //    if ((int)NFOINDEX == 1)
        //    {
        //        TcpService.TcpServer TS = Server as TcpService.TcpServer;

        //        //回复通知界面
        //        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "???", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion

        //    #region udp回复
        //    if ((int)NFOINDEX == 2)
        //    {
        //        UdpService.UdpServer US = Server as UdpService.UdpServer;
        //        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "???", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion
        //}

        internal static void Process_17H(YanYu.WRIMR.Protocol.InPacket.Deal0x17 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID="";
            IList<YanYu.WRIMR.Protocol.WaterLevel> WaterLevelList=pack.packData.WaterLevel;

            string Explain = "(配置)水位配置项信息入库";
            foreach (var item in WaterLevelList)
            {
                ItemID = "00020000" + item.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "00";
                model.ConfigVal = item.BaseValue.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                model.ConfigID = "02";
                model.ConfigVal = item.LowerLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                model.ConfigID = "01";
                model.ConfigVal = item.UpperLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += " 基值[" + item.BaseValue.ToString() + "]  上限[" + item.UpperLimit.ToString() + "]  下限[" + item.LowerLimit.ToString() + "]";
            }


            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
               
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "17操作异常" + ex.ToString());
        }
        }

        //internal static void Process_18H(YanYu.WRIMR.Protocol.InPacket.Deal0x18 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        //{
        //    string ItemID = "";
        //    IList<YanYu.WRIMR.Protocol.WaterPressure> WaterPressureList = pack.packData.WaterPressure;
        //    foreach (var item in WaterPressureList)
        //    {
        //        ItemID = "00150000" + item.Index.ToString("00");
        //        string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "'";
        //        PublicBD.db.DelRTU_ConfigData(Where);

        //        Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
        //        model.STCD = pack.STCD;
        //        model.ItemID = ItemID;
        //        model.ConfigID = "02";
        //        model.ConfigVal = item.LowerLimit.ToString();
        //        PublicBD.db.AddRTU_ConfigData(model);
        //        model.ConfigID = "01";
        //        model.ConfigVal = item.UpperLimit.ToString();
        //        PublicBD.db.AddRTU_ConfigData(model);
        //    }

        //    #region tcp回复
        //    if ((int)NFOINDEX == 1)
        //    {
        //        TcpService.TcpServer TS = Server as TcpService.TcpServer;

        //        //回复通知界面
        //        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "水压上下限配置项信息入库(配置)", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion

        //    #region udp回复
        //    if ((int)NFOINDEX == 2)
        //    {
        //        UdpService.UdpServer US = Server as UdpService.UdpServer;
        //        //回复通知界面
        //        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "水压上下限配置项信息入库(配置)", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        //    }
        //    #endregion
        //}

        internal static void Process_19H(YanYu.WRIMR.Protocol.InPacket.Deal0x19 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQualityList = pack.packData.WaterQuality;
            string Explain = "(配置)水质上限配置项信息入库";
            foreach (var item in WaterQualityList)
            {
                ItemID = "0010" + ((int)item.Type).ToString("0000")+"01";
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='01'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "01";
                model.ConfigVal = item.Value.ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + item.Value.ToString() + "]" ;
            
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD,Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "19操作异常" + ex.ToString());
        }
        }

        internal static void Process_20H(YanYu.WRIMR.Protocol.InPacket.Deal0x20 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000020'";
                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速（风向）", "水温", "水质", "土壤含水率", "蒸发量", "水压", "备用1", "备用2", "备用3" };
                var list = from _20 in ServiceBussiness.CONFIGDATAList where _20.STCD == pack.STCD && _20.ItemID == "0000000000" && _20.ConfigID == "120000000020" select _20;

                int Index = (int)pack.packData.DataType;
                string DataVal = "";
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    string[] ItemGroup = list.First().ConfigVal.Split(new char[',']);
                    ItemGroup[Index] = "1:" + decimal.Parse(pack.packData.Threshold.ToString()) + ":" + pack.packData.Interval;

                    for (int i = 0; i < ItemGroup.Length ; i++)
                    {
                        DataVal += ItemGroup + ",";
                    }
                    if (DataVal.Length > 0)
                    {
                        DataVal = DataVal.TrimEnd(new char[] { ',' });
                    }
                    list.First().ConfigVal = DataVal;
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000000020";
                    string[] ItemGroup = new string[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        ItemGroup[i] = "0:0:0";
                    }
                    ItemGroup[Index] = "1:" + decimal.Parse(pack.packData.Threshold.ToString()) + ":" + pack.packData.Interval;
                    for (int i = 0; i < ItemGroup.Length; i++)
                    {
                        DataVal += ItemGroup[i] + ",";
                    }
                    if (DataVal.Length > 0)
                    {
                        DataVal = DataVal.TrimEnd(new char[] { ',' });
                    }
                    model.ConfigVal = DataVal; 
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            string Explain = "";
            Explain = "(设置)遥测终端检测参数[" + items [Index]+ "]启报阈值[" + pack.packData.Threshold.ToString() + "]及固态存储时间段间隔信息入库[" + pack.packData.Interval + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD,Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "20操作异常" + ex.ToString());
        }
        }

        internal static void Process_21H(YanYu.WRIMR.Protocol.InPacket.Deal0x21 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string Explain = "接收手机号码（或IC卡号）[" + pack.packData.StrNo + "]";
            string STCD = pack.STCD;
            DateTime TM = pack.packData.TP.SendTime;

            #region 生成回复数据
            byte[] sendData = null;
            var rtu = from r in ServiceBussiness.RtuList where r.STCD == pack.STCD select r;
            string ElmData = "500";// GetElmData(STCD);//得到剩余水量
            if (rtu.Count() > 0)
            {
                int pwd = int.Parse(rtu.First().PassWord);
                sendData = p.pack(STCD, 0, 0, (int)pack.AFN, ElmData, pwd);
            }
            #endregion

            Explain += ",回复剩余水量[500]";
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == pack.STCD && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == STCD && u.IpEndPoint != null select u;

                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    //System.Net.Sockets.UdpClient udpclient = new System.Net.Sockets.UdpClient(US.PORT);  //()绑定指定端口
                    //udpclient.Connect(udps.First().IpEndPoint);
                    //udpclient.Send(sendData, sendData.Length);
                    //udpclient.Close();

                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                var gsm = from g in GS.Gs where g.STCD == STCD select g;
                List<GsmService.GsmMobile> GSM = gsm.ToList<GsmService.GsmMobile>();
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }
                if (GSM.Count() > 0 && GSM.First().MOBILE != null && GSM.First().MOBILE != "")
                {
                    GS.SendData(GSM.First().MOBILE, sendData);
                    ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                else
                { //通知界面
                    ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "测站[" + STCD + "]没有设置手机号", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
              }
            #endregion 
            //0000222201 

            #region tcp通知界面
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                //通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp通知界面
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm通知界面
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        
        internal static void Process_1AH(YanYu.WRIMR.Protocol.InPacket.Deal0x1A pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQualityList = pack.packData.WaterQuality;
            string Explain = "(配置)水质下限配置项信息入库";
            foreach (var item in WaterQualityList)
            {
                ItemID = "0010" + ((int)item.Type).ToString("0000") + "01";
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='02'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "02";
                model.ConfigVal = item.Value .ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + item.Value.ToString() + "]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                 }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1A操作异常" + ex.ToString());
        }
        }

        internal static void Process_1BH(YanYu.WRIMR.Protocol.InPacket.Deal0x1B pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            double[] WaterBaseValue = pack.packData.WaterBaseValue;

            string Explain = "(配置)水量的表底（初始）值信息入库";
            for (int i = 0; i < WaterBaseValue.Length ; i++)
            {
                ItemID = "00002222"+(i+1).ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='21'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "21";
                model.ConfigVal = WaterBaseValue[i].ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + WaterBaseValue[i].ToString() + "]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                 }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1B操作异常" + ex.ToString());
        }
        }

        internal static void Process_1CH(YanYu.WRIMR.Protocol.InPacket.Deal0x1C pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{

                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='12000000001C'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "12000000001C" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.RelayLength.ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "12000000001C";
                    model.ConfigVal = pack.packData.RelayLength.ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
           

            string Explain = "(配置)遥测终端转发中继引导码长信息入库";
            Explain += "[" + pack.packData.RelayLength + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1C操作异常" + ex.ToString());
        }
        }

        internal static void Process_1DH(YanYu.WRIMR.Protocol.InPacket.Deal0x1D pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='12000000001D'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "12000000001D" select d;
                
            string Explain = "(配置)转发终端地址信息入库";
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                string stcds = "";
                foreach (var item in pack.packData.STCD)
                {
                    stcds += item + ",";
                }
                if (stcds.Length > 0) 
                {
                    stcds = stcds.Substring(0, stcds.Length - 1);
                }
                model.ConfigVal = stcds;
                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + stcds + "]";
            }
            if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "12000000001D";
                string stcds = "";
                foreach (var item in pack.packData.STCD)
                {
                    stcds += item + ",";
                }
                if (stcds.Length > 0)
                {
                    stcds = stcds.Substring(0, stcds.Length - 1);
                }
                model.ConfigVal = stcds;
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + stcds + "]";
            }
            
           

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1D操作异常" + ex.ToString());
        }
        }

        internal static void Process_1EH(YanYu.WRIMR.Protocol.InPacket.Deal0x1E pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Explain = "(配置)中继站工作机自动切换、自报状态信息入库";
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED0'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED0" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = ((int)pack.packData.SwitchStatus ).ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

            }
                if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000001ED0";
                model.ConfigVal = ((int)pack.packData.SwitchStatus).ToString();
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED2'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED2" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = ( (int)pack.packData.AllowRelaying).ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED2";
                    model.ConfigVal = ( (int)pack.packData.AllowRelaying).ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED4'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED4" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.PowerReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED4";
                    model.ConfigVal = pack.packData.PowerReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED5'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED5" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.SwitchReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED5";
                    model.ConfigVal = pack.packData.SwitchReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED6'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED6" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.FaultReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED6";
                    model.ConfigVal = pack.packData.FaultReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
           
            Explain += " 工作机中继转发[" + pack.packData.AllowRelaying + "]\n"+
                "工作机(值班/备份)自动切换[" + ((int)pack.packData.SwitchStatus == 0 ? false : true)+ "]\n"+
                "出现电源报警主动上报["+pack.packData.PowerReport+"]\n"+
                "出现工作机切换主动上报[" + pack.packData.SwitchReport + "]\n" +
                "出现故障主动上报["+pack.packData.FaultReport+"]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
            
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1E操作异常" + ex.ToString());
        }
        }

        internal static void Process_1FH(YanYu.WRIMR.Protocol.InPacket.Deal0x1F pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
             string ItemID="00030000__";
             string Where = " where stcd='" + pack.STCD + "' and ItemID like '" + ItemID + "' and  ConfigID='01'";

             string Explain = "(配置)流量上限配置项信息入库";

                PublicBD.db.DelRTU_ConfigData(Where);
                double[] UpperLimit = pack.packData.UpperLimit;
                for (int i = 0; i < UpperLimit.Length ; i++)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "00030000"+(i+1).ToString("00");
                    model.ConfigID = "01";
                    model.ConfigVal = UpperLimit[i].ToString();
                    PublicBD.db.AddRTU_ConfigData(model);

                    Explain += "[" + UpperLimit[i].ToString() + "]";
                }
                
           
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
               
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "1F操作异常" + ex.ToString());
        }
            
        }

        internal static void Process_30H(YanYu.WRIMR.Protocol.InPacket.Deal0x30 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try
            {
                string Explain = "(配置)测站IC卡功能有效信息入库";
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000030'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000030" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.ICStatus?"1":"0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000000030";
                    model.ConfigVal = pack.packData.ICStatus ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }


            Explain += "[" + pack.packData.ICStatus + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "30操作异常" + ex.ToString());
        }
        }

        internal static void Process_31H(YanYu.WRIMR.Protocol.InPacket.Deal0x31 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try
            {
                string Explain = "(配置)取消测站IC卡功能有效信息入库";
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000030'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000030" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.ICStatus ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000000030";
                    model.ConfigVal = pack.packData.ICStatus ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            Explain += "[" + pack.packData.ICStatus + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "31操作异常" + ex.ToString());
        }
        }

        internal static void Process_32H(YanYu.WRIMR.Protocol.InPacket.Deal0x32 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000032'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000032" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.FixValueStatus ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000000032";
                    model.ConfigVal = pack.packData.FixValueStatus ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            string Explain = "(配置)测站定值投入信息入库";
            Explain += "[" + pack.packData.FixValueStatus + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "32操作异常" + ex.ToString());
        }
        }

        internal static void Process_33H(YanYu.WRIMR.Protocol.InPacket.Deal0x33 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string Explain = "(配置)测站定值控制退出信息入库";
            string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000032'";
            var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000032" select d;
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                model.ConfigVal = pack.packData.FixValueStatus ? "1" : "0";
                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);
            }
            if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000000032";
                model.ConfigVal = pack.packData.FixValueStatus ? "1" : "0";
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }
            Explain += "[" + pack.packData.FixValueStatus + "]";
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "33操作异常" + ex.ToString());
        }
        }

        internal static void Process_34H(YanYu.WRIMR.Protocol.InPacket.Deal0x34 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
           string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000034'";
            var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000034" select d;
            decimal? Fixval = null;
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                decimal fixval = 0;
                if (decimal.TryParse(pack.packData.FixValue.ToString(), out fixval))
                {
                    model.ConfigVal = fixval.ToString();
                }
                else 
                {
                   model.ConfigVal =  null;
                }
                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);

                if (model.ConfigVal == null)
                    Fixval = null;
                else
                    Fixval = fixval;
            }
            if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000000034";
                decimal fixval = 0;
                if (decimal.TryParse(pack.packData.FixValue.ToString(), out fixval))
                {
                    model.ConfigVal = fixval.ToString();
                }
                else
                {
                    model.ConfigVal = null;
                }
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);

                if (model.ConfigVal == null)
                    Fixval = null;
                else
                    Fixval = fixval;
            }

            string Explain = "(配置)测站定值量信息入库[" + Fixval + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "34操作异常" + ex.ToString());
        }
        }
        internal static void Process_50H(YanYu.WRIMR.Protocol.InPacket.Deal0x50 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer; 

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "遥测终端、中继站地址 [" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "遥测终端、中继站地址 [" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "遥测终端、中继站地址 [" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "遥测终端、中继站地址 [" + pack.packData.STCD + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_51H(YanYu.WRIMR.Protocol.InPacket.Deal0x51 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "时钟查询成功["+pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss")+"]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "时钟查询成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "时钟查询成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "时钟查询成功[" + pack.packData.Date.ToString("yyyy-MM-dd HH:mm:ss") + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_52H(YanYu.WRIMR.Protocol.InPacket.Deal0x52 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{

                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000012'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000012" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = ((int)pack.packData.WorkMode).ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000000012";
                    model.ConfigVal = ((int)pack.packData.WorkMode).ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
          

            string Explain = "";
            if ((int)pack.packData.WorkMode == 0) 
            {
                Explain = "(查询)测站工作模式信息入库[兼容工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 1)
            {
                Explain = "(查询)测站工作模式信息入库[自报工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 2)
            {
                Explain = "(查询)测站工作模式信息入库[查询/应答工作模式]";
            }
            else if ((int)pack.packData.WorkMode == 3)
            {
                Explain = "(查询)测站工作模式信息入库[调试/维修工作模式]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "52操作异常" + ex.ToString());
        }
        }
        internal static void Process_53H(YanYu.WRIMR.Protocol.InPacket.Deal0x53 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='1200000000A1'";
                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "报警或状态", "水压", "备用1", "备用2" };
                string Explain = "(查询)遥测终端的数据自报种类及时间间隔信息入库";
                IList<YanYu.WRIMR.Protocol.DataKind> dk = pack.packData.AutoTime;

                string DataVal = "";
                for (int i = 0; i < dk.Count; i++)
                {
                    if (dk[i].AutoReport)
                    {
                        DataVal += "1:" + dk[i].Interval + ",";
                    }
                    else
                    {
                        DataVal += "0:0,";
                    }
                    Explain += "\n[" + items[i] + " 是否自报:" + dk[i].AutoReport + " 自报间隔:" + dk[i].Interval + "]";
                }
                if (DataVal.Length > 0)
                {
                    DataVal = DataVal.TrimEnd(new char[] { ',' });
                }

                var list = from A1 in ServiceBussiness.CONFIGDATAList where A1.STCD == pack.STCD && A1.ItemID == "0000000000" && A1.ConfigID == "1200000000A1" select A1;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = DataVal;
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "1200000000A1";
                    model.ConfigVal = DataVal;
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }


            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
  
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "53操作异常" + ex.ToString());
        }
        }
        internal static void Process_54H(YanYu.WRIMR.Protocol.InPacket.Deal0x54 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='1200000000A0'";
                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "终端内存", "固态存储", "上报水压", "备用" };
                string Explain = "(设置)遥测站需查询的实时数据种类信息入库";
                Dictionary<int, bool> NeedQuerys = pack.packData.NeedQuerys;

                int k = 0;
                string DataVal = "";
                foreach (var item in NeedQuerys)
                {
                    if (item.Value)
                    {
                        DataVal += "1";
                        Explain += "[" + items[k] + "]";
                    }
                    else
                    {
                        DataVal += "0";
                    }
                    k++;
                }

                var list = from A0 in ServiceBussiness.CONFIGDATAList where A0.STCD == pack.STCD && A0.ItemID == "0000000000" && A0.ConfigID == "1200000000A0" select A0;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = DataVal;
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "1200000000A0";
                    model.ConfigVal = DataVal;
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "54操作异常" + ex.ToString());
        }
        }
        internal static void Process_55H(YanYu.WRIMR.Protocol.InPacket.Deal0x55 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string Explain="剩余水量["+pack.packData.RemainingWater.ToString()+"]";
            Explain += " 最近成功充值量[" + pack.packData.WaterRecentSuccessRecharge.ToString()+"]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_56H(YanYu.WRIMR.Protocol.InPacket.Deal0x56 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string Explain = "剩余水量[" + pack.packData.RemainingWater.ToString()+"]";
            Explain += " 水量报警值[" + pack.packData.AlarmValue.ToString() + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_57H(YanYu.WRIMR.Protocol.InPacket.Deal0x57 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterLevel> WaterLevelList = pack.packData.WaterLevel;
            string Explain = "(查询)水位配置项信息入库";
            foreach (var item in WaterLevelList)
            {
                ItemID = "00020000" + item.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "00";
                model.ConfigVal = item.BaseValue.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                model.ConfigID = "02";
                model.ConfigVal = item.LowerLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                model.ConfigID = "01";
                model.ConfigVal = item.UpperLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                Explain += " 基值[" + item.BaseValue.ToString() + "]  上限[" + item.UpperLimit.ToString() + "]  下限[" + item.LowerLimit.ToString() + "]";
            
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "57操作异常" + ex.ToString());
        }
            
        }
        internal static void Process_58H(YanYu.WRIMR.Protocol.InPacket.Deal0x58 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterPressure> WaterPressureList = pack.packData.WaterPressure;

            string Explain = "(查询)水压上下限配置项信息入库";
            foreach (var item in WaterPressureList)
            {
                ItemID = "00150000" + item.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "02";
                model.ConfigVal = item.LowerLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);
                model.ConfigID = "01";
                model.ConfigVal = item.UpperLimit.ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += " 上限[" + item.UpperLimit.ToString() + "]  下限[" + item.LowerLimit.ToString() + "]";
            
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                //回复通知界面
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "58操作异常" + ex.ToString());
        }
        }
        internal static void Process_59H(YanYu.WRIMR.Protocol.InPacket.Deal0x59 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQualityList = pack.packData.WaterQuality;
            string Explain = "(查询)水质上限配置项信息入库";
            foreach (var item in WaterQualityList)
            {
                ItemID = "0010" + ((int)item.Type).ToString("0000") + "01";
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='01'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "01";
                model.ConfigVal = item.Value.ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + item.Value.ToString() + "]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "59操作异常" + ex.ToString());
        }
        }
        internal static void Process_5AH(YanYu.WRIMR.Protocol.InPacket.Deal0x5A pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQualityList = pack.packData.WaterQuality;

            string Explain = "(查询)水质下限配置项信息入库";
            foreach (var item in WaterQualityList)
            {
                ItemID = "0010" + ((int)item.Type).ToString("0000") + "02";
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='02'";
                PublicBD.db.DelRTU_ConfigData(Where);

                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = ItemID;
                model.ConfigID = "01";
                model.ConfigVal = item.Value.ToString();
                PublicBD.db.AddRTU_ConfigData(model);

                Explain += "[" + item.Value.ToString() + "]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "5A操作异常" + ex.ToString());
        }
        }
        internal static void Process_5DH(YanYu.WRIMR.Protocol.InPacket.Deal0x5D pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
           string dateName = "[";
           for (int i = 0; i < pack.packData.EventCount.Length; i++)
            {
                if (i == 0)
                    dateName = "数据初始化记录";
                else if (i == 1)
                    dateName = "参数变更记录";
                else if (i == 2)
                    dateName = "状态量变位记录";
                else if (i == 3)
                    dateName = "仪表故障记录";
                else if (i == 4)
                    dateName = "密码错误记录";
                else if (i == 5)
                    dateName = "终端故障记录";
                else if (i == 6)
                    dateName = "交流失电记录";
                else if (i == 7)
                    dateName = "蓄电池电压低告警记录";
                else if (i == 8)
                    dateName = "终端箱非法打开记录";
                else if (i == 9)
                    dateName = "水泵故障记录";
                else if (i == 10)
                    dateName = "剩余水量越限告警记录";
                else if (i == 11)
                    dateName = "水位超限告警记录";
                else if (i == 12)
                    dateName = "水压超限告警记录";
                else if (i == 13)
                    dateName = "水质参数超限告警记录";
                else if (i == 14)
                    dateName = "数据出错记录";
                else if (i == 15)
                    dateName = "发报文记录";
                else if (i == 16)
                    dateName = "收报文记录";
                else if (i == 17)
                    dateName = "发报文出错记录";
                else
                    dateName = "备用" + (i + 1).ToString();

               dateName +=pack.packData.EventCount[i].ToString();
            }
            if(dateName.Length >1)
            {
                dateName=dateName.Substring(0,dateName.Length -1);
            }
            dateName+="]";
            
            string Explain = "遥测终端的事件记录" + dateName;   
          
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_5EH(YanYu.WRIMR.Protocol.InPacket.Deal0x5E pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string WorkMode = "";
            string PowerStatus = "";
            switch (pack.packData.RtuStatus.WorkMode)
            {
                case  YanYu.WRIMR.Protocol.RtuWorkMode.Compatible:
                  WorkMode = "兼容工作状态";
                  break;
                case YanYu.WRIMR.Protocol.RtuWorkMode.Auto:
                  WorkMode = "自报工作状态";
                  break;
                case YanYu.WRIMR.Protocol.RtuWorkMode.Passive:
                  WorkMode = "查询/应答工作状态";
                  break;
                case YanYu.WRIMR.Protocol.RtuWorkMode.Debug:
                  WorkMode = "调试/维修状态";
                  break;
            }
            switch (pack.packData.RtuStatus.PowerStatus)
            {
                case YanYu.WRIMR.Protocol.PowerStatus.AC220:
                    PowerStatus = "AC220V 供电";
                    break;
                case YanYu.WRIMR.Protocol.PowerStatus.Battery:
                    PowerStatus = "蓄电池供电";
                    break;
            }

            string Explain = "终端工作模式[" + WorkMode;
            Explain += " IC卡功能[" + (pack.packData.RtuStatus.ICStatus ? "有效" : "无效") + "]";
            Explain += " 定值控制[" + (pack.packData.RtuStatus.FixValueStatus ? "投入" : "退出") + "]";
            Explain += " 水泵工作状态[" + (pack.packData.RtuStatus.FixValueStatus ? "启动" : "停止") + "]";
            Explain += " 终端箱门状态[" + (pack.packData.RtuStatus.DoorClosed ? "关闭" : "开启") + "]";
            Explain += " 电源工作状态[" + PowerStatus+"]";
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_5FH(YanYu.WRIMR.Protocol.InPacket.Deal0x5F pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string explain = "[";
            for (int i = 0; i < pack.packData.Value.Count; i++)
            {
                if (i == 0)
                    explain += "A相电压:";
                else if (i == 1)
                    explain += "B相电压:";
                else if (i == 2)
                    explain += "C相电压:";
                else if (i == 3)
                    explain += "A相电流:";
                else if (i == 4)
                    explain += "B相电流:";
                else if (i == 5)
                    explain += "C相电流:";
                explain += pack.packData.Value[i].ToString()+",";
            }

            if (explain.Length > 1) 
            {
                explain = explain.Substring(0, explain.Length - 1);
            }
            explain = "]";


            string Explain = "水泵电机实时工作数据" + explain;   

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_60H(YanYu.WRIMR.Protocol.InPacket.Deal0x60 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='12000000001C'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "12000000001C" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.RelayLength.ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "12000000001C";
                    model.ConfigVal = pack.packData.RelayLength.ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
            string Explain = "(查询)遥测终端转发中继引导码长信息入库";
            Explain += "[" + pack.packData.RelayLength + "]";

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
   
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "60操作异常" + ex.ToString());
        }
        }
        internal static void Process_62H(YanYu.WRIMR.Protocol.InPacket.Deal0x62 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='12000000001D'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "12000000001D" select d;

                string Explain = "(配置)转发终端地址信息入库";
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    string stcds = "";
                    foreach (var item in pack.packData.STCD)
                    {
                        stcds += item + ",";
                    }
                    if (stcds.Length > 0)
                    {
                        stcds = stcds.Substring(0, stcds.Length - 1);
                    }
                    model.ConfigVal = stcds;
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                    Explain += "[" + stcds + "]";
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "12000000001D";
                    string stcds = "";
                    foreach (var item in pack.packData.STCD)
                    {
                        stcds += item + ",";
                    }
                    if (stcds.Length > 0)
                    {
                        stcds = stcds.Substring(0, stcds.Length - 1);
                    }
                    model.ConfigVal = stcds;
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);

                    Explain += "[" + stcds + "]";
                }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "62操作异常" + ex.ToString());
        }
        }
        internal static void Process_63H(YanYu.WRIMR.Protocol.InPacket.Deal0x63 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED0'";
                var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED0" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = ((int)pack.packData.SwitchStatus).ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED0";
                    model.ConfigVal = ((int)pack.packData.SwitchStatus).ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED2'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED2" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = ((int)pack.packData.AllowRelaying).ToString();
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED2";
                    model.ConfigVal = ((int)pack.packData.AllowRelaying).ToString();
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED4'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED4" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.PowerReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED4";
                    model.ConfigVal = pack.packData.PowerReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED5'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED5" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.SwitchReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED5";
                    model.ConfigVal = pack.packData.SwitchReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

                Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000001ED6'";
                list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000001ED6" select d;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = pack.packData.FaultReport ? "1" : "0";
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);

                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "120000001ED6";
                    model.ConfigVal = pack.packData.FaultReport ? "1" : "0";
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            string Explain = "(查询)中继站状态信息入库";
            Explain += " 工作机A错误[" +pack.packData.WM_A_Normal+"]";
            Explain += " 工作机B错误" + pack.packData.WM_B_Normal+"]";
            if (pack.packData.WM_A_Duty)
                Explain +=  " 工作机A值班";
            else
                Explain += " 工作机B值班";
            Explain += " 中继站转发允许" + pack.packData.Relaying + "]";
            Explain += " 电源报警" + pack.packData.PowerAlarm +"]";
            Explain += " 中继站故障" + pack.packData.RS_Fault +"]";


            string dt = " ";
            List<DateTime> DT = pack.packData.SwitchLog;
            foreach (var item in DT)
            {
                dt += "["+item.ToString("yyyy-mm-dd HH:MM:ss")+"] ";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain+dt, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD,  Explain + dt, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "63操作异常" + ex.ToString());
        }
        }
        internal static void Process_64H(YanYu.WRIMR.Protocol.InPacket.Deal0x64 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
             string ItemID="00030000__";
             string Where = " where stcd='" + pack.STCD + "' and ItemID like '" + ItemID + "' and  ConfigID='01'";
             PublicBD.db.DelRTU_ConfigData(Where);
             double[] UpperLimit = pack.packData.UpperLimit;

             string Explain = "(查询)流量上限配置项信息入库";
             for (int i = 0; i < UpperLimit.Length; i++)
             {
                 Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                 model.STCD = pack.STCD;
                 model.ItemID = "00030000" + (i + 1).ToString("00");
                 model.ConfigID = "01";
                 model.ConfigVal = UpperLimit[i].ToString();
                 PublicBD.db.AddRTU_ConfigData(model);

                 Explain += "[" + UpperLimit[i].ToString() + "]";
             }
           
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
               
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "64操作异常" + ex.ToString());
        }
        }
        //???
        internal static void Process_81H(YanYu.WRIMR.Protocol.InPacket.Deal0x81 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try{
            #region 生成回复数据
            byte[] sendData = null;
            var rtu = from r in ServiceBussiness.RtuList where r.STCD == pack.STCD select r;
            string ElmData = GetElmData(pack.STCD);
            if (rtu.Count() > 0)
            {
                int pwd = int.Parse(rtu.First().PassWord);
                sendData = p.pack(pack.STCD, 0, 0, (int)pack.AFN, ElmData, pwd);
            }
            #endregion
         
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;
                var tcps = from t in Ts where t.STCD == pack.STCD && t.TCPSOCKET!=null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "接收数据为随机自报报警数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == pack.STCD && u.IpEndPoint!=null select u;

                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                //回复通知界面
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "接收数据为随机自报报警数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "接收数据为随机自报报警数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "接收数据为随机自报报警数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "81操作异常" + ex.ToString());
            }
        }
        //???
        internal static void Process_82H(YanYu.WRIMR.Protocol.InPacket.Deal0x82 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            #region 生成回复数据(与C0格式相同)
            byte[] sendData = null;
            var rtu = from r in ServiceBussiness.RtuList where r.STCD == pack.STCD select r;
            string ElmData = GetElmData(pack.STCD);
            if (rtu.Count() > 0)
            {
                int pwd = int.Parse(rtu.First().PassWord);
                sendData = p.pack(pack.STCD, 0, 0, (int)pack.AFN, ElmData, pwd);
            }
            #endregion             
            #region 人工置数入库代码
            Service.Model.YY_DATA_MANUAL model = new Model.YY_DATA_MANUAL();
            model.STCD = pack.STCD;
            model.TM = pack.packData.TP.SendTime;
            model.DOWNDATE = DateTime.Now;
            model.DATAVALUE = pack.packData.Info;
            model.NFOINDEX = (int)NFOINDEX;
            PublicBD.db.AddManualData(model);
            #endregion
            
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;
                var tcps = from t in Ts where t.STCD == pack.STCD && t.TCPSOCKET !=null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }

                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "人工置数报文入库", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us ;
                var udps = from u in Us where u.STCD == pack.STCD && u.IpEndPoint != null select u;

                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (sendData == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "没有该站号设备，无法回复数据", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }

                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "人工置数报文入库", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "人工置数报文入库", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "人工置数报文入库", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "82操作异常" + ex.ToString());
        }
        }
        internal static void Process_8EH(YanYu.WRIMR.Protocol.InPacket.Deal0x8E pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "设备在线心跳", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "设备在线心跳", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "设备在线心跳", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "设备在线心跳", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_90H(YanYu.WRIMR.Protocol.InPacket.Deal0x90 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(设置)复位遥测终端参数和状态[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(设置)复位遥测终端参数和状态[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(设置)复位遥测终端参数和状态[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(设置)复位遥测终端参数和状态[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_91H(YanYu.WRIMR.Protocol.InPacket.Deal0x91 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(设置)清空遥测终端历史数据单元[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(设置)清空遥测终端历史数据单元[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(设置)清空遥测终端历史数据单元[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(设置)清空遥测终端历史数据单元[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process_92H(YanYu.WRIMR.Protocol.InPacket.Deal0x92 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            //false =水泵 =00000000+编号01~15
            //true  =阀门 =00001111+编号01~15
            if (pack.packData.Type)
            {
                ItemID = "00001111" + pack.packData.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='20'";
                PublicBD.db.DelRTU_ConfigData(Where);
            }
            else 
            {
                ItemID = "00000000" + pack.packData.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='20'";
                PublicBD.db.DelRTU_ConfigData(Where);
            }

            Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
            model.STCD = pack.STCD;
            model.ItemID = ItemID;
            model.ConfigID = "20";
            model.ConfigVal = "1";  //代表打开
            PublicBD.db.AddRTU_ConfigData(model);

            string Explain = "(配置)启动水泵或阀门/闸门配置项信息入库";
            if (pack.packData.Type)
            {
                Explain += "阀门/闸门[" + pack.packData.Index.ToString("00") + "]";
            }
            else
            {
                Explain += "水泵[" + pack.packData.Index.ToString("00") + "]";
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "92操作异常" + ex.ToString());
        }
        }
        internal static void Process_93H(YanYu.WRIMR.Protocol.InPacket.Deal0x93 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string ItemID = "";
            //false =水泵 =00000000+编号01~15
            //true  =阀门 =00001111+编号01~15
            if (pack.packData.Type)  //阀门
            {
                ItemID = "00001111" + pack.packData.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='20'";
                PublicBD.db.DelRTU_ConfigData(Where);
            }
            else
            {
                ItemID = "00000000" + pack.packData.Index.ToString("00");
                string Where = " where stcd='" + pack.STCD + "' and ItemID='" + ItemID + "' and ConfigID='20'";
                PublicBD.db.DelRTU_ConfigData(Where);
            }

            Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
            model.STCD = pack.STCD;
            model.ItemID = ItemID;
            model.ConfigID = "20";
            model.ConfigVal = "0";  //代表关闭
            PublicBD.db.AddRTU_ConfigData(model);

            string Explain = "(配置)关闭水泵或阀门/闸门配置项信息入库";
            if (pack.packData.Type)
            {
                Explain += "阀门/闸门[" + pack.packData.Index.ToString("00") + "]";
            }
            else
            {
                Explain += "水泵[" + pack.packData.Index.ToString("00") + "]";
            }
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
   
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "93操作异常" + ex.ToString());
        }
        }
        internal static void Process_94H(YanYu.WRIMR.Protocol.InPacket.Deal0x94 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string Machine = "";
            string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000094'";
            var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000094" select d;
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                if (pack.packData.DutyMachine == YanYu.WRIMR.Protocol.Machine.A)
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal  = "1";
                    }
                    else
                    {
                        model.ConfigVal = "0";
                    }
                }
                else 
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "0";
                    }
                    else
                    {
                        model.ConfigVal = "1";
                    }
                }

                if (model.ConfigVal == "1")
                {
                    Machine = "A";
                }
                else
                { Machine = "B"; }

                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);
            }
            if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000000094";
                if (pack.packData.DutyMachine == YanYu.WRIMR.Protocol.Machine.A)
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "1";
                    }
                    else
                    {
                        model.ConfigVal = "0";
                    }
                }
                else
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "0";
                    }
                    else
                    {
                        model.ConfigVal = "1";
                    }
                }

                if (model.ConfigVal == "1")
                {
                    Machine = "A";
                }
                else
                { Machine = "B"; }
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(配置)通信机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(配置)通信机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(配置)通信机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(配置)通信机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "94操作异常" + ex.ToString());
        }
        }
        internal static void Process_95H(YanYu.WRIMR.Protocol.InPacket.Deal0x95 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string Machine = "";

            string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='120000000095'";
            var list = from d in ServiceBussiness.CONFIGDATAList where d.STCD == pack.STCD && d.ItemID == "0000000000" && d.ConfigID == "120000000095" select d;
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                if (pack.packData.DutyMachine == YanYu.WRIMR.Protocol.Machine.A)
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "1";
                    }
                    else
                    {
                        model.ConfigVal = "0";
                    }
                }
                else
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "0";
                    }
                    else
                    {
                        model.ConfigVal = "1";
                    }
                }

                if (model.ConfigVal  == "1")
                {
                    Machine = "A";
                }
                else
                { Machine = "B"; }

                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);
            }
            if (list.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "120000000095";
                if (pack.packData.DutyMachine == YanYu.WRIMR.Protocol.Machine.A)
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "1";
                    }
                    else
                    {
                        model.ConfigVal = "0";
                    }
                }
                else
                {
                    if (pack.packData.Successful)
                    {
                        model.ConfigVal = "0";
                    }
                    else
                    {
                        model.ConfigVal = "1";
                    }
                }

                if (model.ConfigVal == "1")
                {
                    Machine = "A";
                }
                else
                { Machine = "B"; }
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(配置)工作机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(配置)工作机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(配置)工作机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(配置)工作机为值班机信息入库[" + Machine + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion


            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "95操作异常" + ex.ToString());
        }
        }
        internal static void Process_96H(YanYu.WRIMR.Protocol.InPacket.Deal0x96 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            Model.YY_RTU_Basic RTU = new Model.YY_RTU_Basic();

            lock(Service.ServiceBussiness.RtuList)
            {
                var rtu = from r in Service.ServiceBussiness.RtuList where r.STCD == pack.STCD select r;

                RTU.STCD=rtu.First().STCD ;
                RTU.PassWord =pack.packData.PassWord.ToString();
                RTU.NiceName =rtu.First().NiceName;
                PublicBD.db.UpdRTU(RTU, " where stcd='" + RTU.STCD + "'");//更新数据库

                rtu.First().PassWord= pack.packData.PassWord.ToString();//更新内存
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(设置)测站密码信息入库[" + pack.packData.PassWord.ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(设置)测站密码信息入库[" + pack.packData.PassWord.ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(设置)测站密码信息入库[" + pack.packData.PassWord.ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(设置)测站密码信息入库[" + pack.packData.PassWord.ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "96操作异常" + ex.ToString());
        }
        }
        internal static void Process_A0H(YanYu.WRIMR.Protocol.InPacket.Deal0xA0 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='1200000000A0'";
                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "终端内存", "固态存储", "上报水压", "备用" };
                string Explain = "(设置)遥测站需查询的实时数据种类信息入库";
            Dictionary<int, bool> NeedQuerys = pack.packData.NeedQuerys;

            int k = 0;
            string DataVal = "";
            foreach (var item in NeedQuerys)
            {
                if (item.Value)
                {
                    DataVal += "1";
                    Explain += "[" + items[k] + "]";
                }
                else
                {
                    DataVal += "0";
                }
                k++;
            }
            
            var list = from A0 in ServiceBussiness.CONFIGDATAList where A0.STCD == pack.STCD && A0.ItemID == "0000000000" && A0.ConfigID == "1200000000A0" select A0;
            if (list.Count() > 0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = list.First();
                model.ConfigVal = DataVal;
                PublicBD.db.DelRTU_ConfigData(Where);
                PublicBD.db.AddRTU_ConfigData(model);
            }
            if(list .Count()==0)
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                model.STCD = pack.STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "1200000000A0";
                model.ConfigVal = DataVal;
                ServiceBussiness.CONFIGDATAList.Add(model);
                PublicBD.db.AddRTU_ConfigData(model);
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
 
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "A0操作异常" + ex.ToString());
        }
        }
        internal static void Process_A1H(YanYu.WRIMR.Protocol.InPacket.Deal0xA1 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string Where = " where STCD ='" + pack.STCD + "' and ItemID = '0000000000' and ConfigID ='1200000000A1'";
                string[] items = new string[] { "雨量", "水位", "流量(水量)", "流速", "闸位", "功率", "气压", "风速", "水温", "水质", "土壤含水率", "蒸发量", "报警或状态", "水压", "备用1", "备用2" };
                string Explain = "(设置)遥测终端的数据自报种类及时间间隔信息入库";
                IList<YanYu.WRIMR.Protocol.DataKind> dk = pack.packData.AutoTime;

                string DataVal = "";
                for (int i = 0; i < dk.Count; i++)
                {
                    if (dk[i].AutoReport)
                    {
                        DataVal += "1:" + dk[i].Interval+",";
                    }
                    else
                    {
                        DataVal += "0:0,";
                    }
                    Explain += "\n[" + items[i] + " 是否自报:" + dk[i].AutoReport + " 自报间隔:" + dk[i].Interval + "]";
                }
                if (DataVal.Length > 0) 
                {
                    DataVal = DataVal.TrimEnd(new char[] { ',' });
                }

                var list = from A1 in ServiceBussiness.CONFIGDATAList where A1.STCD == pack.STCD && A1.ItemID == "0000000000" && A1.ConfigID == "1200000000A1" select A1;
                if (list.Count() > 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = list.First();
                    model.ConfigVal = DataVal;
                    PublicBD.db.DelRTU_ConfigData(Where);
                    PublicBD.db.AddRTU_ConfigData(model);
                }
                if (list.Count() == 0)
                {
                    Service.Model.YY_RTU_CONFIGDATA model = new Model.YY_RTU_CONFIGDATA();
                    model.STCD = pack.STCD;
                    model.ItemID = "0000000000";
                    model.ConfigID = "1200000000A1";
                    model.ConfigVal = DataVal;
                    ServiceBussiness.CONFIGDATAList.Add(model);
                    PublicBD.db.AddRTU_ConfigData(model);
                }

            
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "A1操作异常" + ex.ToString());
        }
        }
        internal static void Process_B0H(YanYu.WRIMR.Protocol.InPacket.Deal0xB0 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try{
            Dictionary<int, bool> alarms = pack.packData.Alarms;
            IList<double> data = pack.packData.Data;
            string AlarmsStr = pack.packData.AlarmsStr;
            YanYu.WRIMR.Protocol.RtuStatus RSs = pack.packData.RtuStatus;
            YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType = pack.packData.DataType;
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality = pack.packData.WaterQuality;
            string STCD = pack.STCD;
            DateTime RTM = DateTime.Now;
            int count = 0;
            if (DataType == YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Quality_Self_Reported)
            {
                count = WaterQuality.Count;
            }
            else
            {
                count = data.Count;
            }
            RealTimeData_RTUStateToUI(DataType, STCD, WaterQuality, AlarmsStr,  RTM, data, NFOINDEX, Server);

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(查询)终端实时数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(查询)终端实时数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(查询)终端实时数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 
                
            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(查询)终端实时数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "B0操作异常" + ex.ToString());
        }
        }
        internal static void Process_B1H(YanYu.WRIMR.Protocol.InPacket.Deal0xB1 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try{
            IList<double> data = pack.packData.Data;
            YanYu.WRIMR.Protocol.LinkLayer.LFC_IN DataType = pack.packData.DataType;
            IList<YanYu.WRIMR.Protocol.WaterQuality> WaterQuality = pack.packData.WaterQuality;
            string STCD = pack.STCD;
            DateTime BTM = pack.packData.BeginDate;
            DateTime ETM = pack.packData.EndData;
            DateTime RTM = DateTime.Now;

            TimeSpan ts1 = new TimeSpan(BTM.Ticks);
            TimeSpan ts2 = new TimeSpan(ETM.Ticks);
            TimeSpan tSpan = ts1 - ts2;
            int dltMinutes = (int)tSpan.TotalMinutes;
            int count = 0;
            if (DataType == YanYu.WRIMR.Protocol.LinkLayer.LFC_IN.Water_Quality_Self_Reported)
            {
                count = WaterQuality.Count;
            }
            else 
            {
                count = data.Count;
            }
            if (count > 0)
            {
                int Minutes = dltMinutes / count;
                DateTime[] dt = new DateTime[count];

                for (int i = 0; i < dt.Length; i++)
                {
                    dt[i] = BTM.AddMinutes(i * Minutes);
                }

                //入固态数据库
                AddRemData(DataType, STCD, WaterQuality, dt, RTM, data, NFOINDEX, Server);
            }

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "(查询)终端固态存储数据["+count +"条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "(查询)终端固态存储数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "(查询)终端固态存储数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "(查询)终端固态存储数据[" + count + "条记录]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
                }
        catch (Exception ex)
        {
            log.Error(DateTime.Now + "B1操作异常" + ex.ToString());
        }
        }
        internal static void Process_B2H(YanYu.WRIMR.Protocol.InPacket.Deal0xB2 pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                //回复通知界面
                ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, pack.STCD, "遥测终端内存已无自报数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                ServiceBussiness.WriteQUIM("UDP", US.ServiceID, pack.STCD, "遥测终端内存已无自报数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            #region gsm回复
            if ((int)NFOINDEX == 3)
            {
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, pack.STCD, "遥测终端内存已无自报数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion 

            #region com通知界面
            if ((int)NFOINDEX == 4)
            {
                ComService.ComServer CS = Server as ComService.ComServer;
                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, pack.STCD, "遥测终端内存已无自报数据[]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion
        }
        internal static void Process(YanYu.WRIMR.Protocol.InPackage pack)
        {
            throw new NotImplementedException();
        }

        #region SLD计算流量相关
        private static void GetSLD(string STCD, out List<int> SectionIndex, out decimal Elevation, out List<Stage> StageList) 
        {
            SectionIndex = null;
            Elevation = 0;
            StageList = null;

            #region SLD计算-初始化
            //有效段位
            var list = from d in Service.ServiceBussiness.CONFIGDATAList where d.ConfigID == "12000000SLD1" && d.STCD == STCD select d;
            if (list.Count() > 0)
            {
                var items = list.First().ConfigVal.Split(new string[] { "," }, StringSplitOptions.None);
                if (items.Count() == 9)
                {
                    int count = 1;
                    SectionIndex = new List<int>();
                    foreach (var item in items)
                    {
                        if (item == "1") 
                        {
                            SectionIndex.Add(int.Parse(item));
                        }
                        count++;
                    }
                }
            }



            //设备安装点高程
            list = from d in Service.ServiceBussiness.CONFIGDATAList where d.ConfigID == "12000000SLD2" && d.STCD == STCD select d;
            if (list.Count() > 0)
            {
                Elevation =decimal.Parse(list.First().ConfigVal);
            }

            //水位、流量关系
            list = from d in Service.ServiceBussiness.CONFIGDATAList where d.ConfigID == "12000000SLD3" && d.STCD == STCD select d;
            if (list.Count() > 0)
            {
                StageList = GetStageList(list.First().ConfigVal);
                
            }
            #endregion
        }

        /// <summary>
        /// 水位、流量关系类
        /// </summary>
        public class Stage
        {
            public Stage()
            { }
            private decimal _WaterLevel;
            private decimal _kA;
            /// <summary>
            /// 水位
            /// </summary>
            public decimal WaterLevel
            {
                set { _WaterLevel = value; }
                get { return _WaterLevel; }
            }
            /// <summary>
            /// 流量系数
            /// </summary>
            public decimal kA
            {
                set { _kA = value; }
                get { return _kA; }
            }
        }
        private static List<Stage> GetStageList(string Val)
        {
            List<Stage> list = new List<Stage>();
            string[] Items = Val.Split(new string[] { "," }, StringSplitOptions.None);
            if (Items.Count() > 0)
            {
                foreach (var item in Items)
                {
                    string[] items = item.Split(new string[] { ":" }, StringSplitOptions.None);
                    Stage stage = new Stage();
                    stage.WaterLevel = decimal.Parse(items[0]);
                    stage.kA = decimal.Parse(items[1]);
                    list.Add(stage);
                }
            }
            return list;
        }

        #region 杜成龙
        public class Lagrange
    {
        /// <summary>
        /// X各点坐标组成的数组
        /// </summary>
        public decimal[] x { get; set; }

        /// <summary>
        /// X各点对应的Y坐标值组成的数组
        /// </summary>
        public decimal[] y { get; set; }

        /// <summary>
        /// x数组或者y数组中元素的个数
        /// , 注意两个数组中的元素个数需要一样
        /// </summary>
        public int itemNum { get; set; }

        /// <summary>
        /// 初始化拉格朗日插值
        /// </summary>
        /// <param name="x">X各点坐标组成的数组</param>
        /// <param name="y">X各点对应的Y坐标值组成的数组</param>
        public Lagrange(decimal[] x, decimal[] y)
        {
            this.x = x; this.y = y;
            this.itemNum = x.Length;
        }

        /// <summary>
        /// 获得某个横坐标对应的Y坐标值
        /// </summary>
        /// <param name="xValue">x坐标值</param>
        /// <returns></returns>
        public decimal GetValue(decimal xValue)
        {
            //用于累乘数组始末下标
            int start, end;
            //返回值
            decimal value = 0;
            //如果初始的离散点为空, 返回0
            if (itemNum < 1) { return value; }
            //如果初始的离散点只有1个, 返回该点对应的Y值
            if (itemNum == 1) { value = y[0]; return value; }
            //如果初始的离散点只有2个, 进行线性插值并返回插值
            if (itemNum == 2)
            {
                value = (y[0] * (xValue - x[1]) - y[1] * (xValue - x[0])) / (x[0] - x[1]);
                return value;
            }
            //如果插值点小于第一个点X坐标, 取数组前3个点做插值
            if (xValue <= x[1]) { start = 0; end = 2; }
            //如果插值点大于等于最后一个点X坐标, 取数组最后3个点做插值
            else if (xValue >= x[itemNum - 2]) { start = itemNum - 3; end = itemNum - 1; }
            //除了上述的一些特殊情况, 通常情况如下
            else
            {
                start = 1; end = itemNum;
                int temp;
                //使用二分法决定选择哪三个点做插值
                while ((end - start) != 1)
                {
                    temp = (start + end) / 2;
                    if (xValue < x[temp - 1])
                        end = temp;
                    else
                        start = temp;
                }
                start--; end--;
                //看插值点跟哪个点比较靠近
                if (Math.Abs(xValue - x[start]) < Math.Abs(xValue - x[end]))
                    start--;
                else
                    end++;
            }
            //这时已经确定了取哪三个点做插值, 第一个点为x[start]
            decimal valueTemp;
            //注意是二次的插值公式
            for (int i = start; i <= end; i++)
            {
                valueTemp = 1;
                for (int j = start; j <= end; j++)
                    if (j != i)
                        valueTemp *= ((xValue - x[j]) / (x[i] - x[j]));
                value += valueTemp * y[i];
            }
            return value;
        }
    }
        private static decimal GetKaValue(decimal waterLevel, List<Stage> list)
        {
            decimal[] waterLevelArray = new decimal[list.Count];
            decimal[] karray = new decimal[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                waterLevelArray[i] = list[i].WaterLevel;
                karray[i] = list[i].kA;
            }

            Lagrange l = new Lagrange(waterLevelArray, karray);
            return l.GetValue(waterLevel);
        }
        #endregion
        #endregion

        #region 得到itemname //水资源协议未使用
        private static string GetItemName(string itemid)
        {
            string ItemName = "";
            var item = from i in Service.ServiceBussiness.ITEMList where i.ItemID == itemid select i;
            if (item.Count() > 0)
            {
                ItemName = item.First().ItemName;
            }
            return ItemName;
        }
        #endregion
    }
}
