using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HJT212_2005
{
    static class PackageProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        static ParseData pd = new ParseData();
        /// <summary>
        /// 上传现场机时间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_1011(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传污染物报警门限值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_1021(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传上位机地址
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_1031(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传数据上报时间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_1041(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传实时数据间隔
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_1061(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }


        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_9011(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 操作执行结果 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_9012(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 通知应答 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_9013(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 数据应答
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_9014(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        ///上传污染物实时数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2011(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                if (pd.GetValidataLength(data))
                {
                    string MN = pd.GetMN(data);
                    DateTime DOWNDATE = DateTime.Now;

                    CN_DataList CD= pd.GetRtdValues(data);

                    //获得回复----------------------------------
                    if (true)
                    {

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == MN  && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = new byte[]{};  //回复报的内容-------------------------------------
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == MN && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                   if(CD.DM.Count()>0)
                       for (int i = 0; i < CD.DM.Count(); i++)
                    {
                        string ItemName = GetItemName(CD.DM[i].ItemCode );

                        #region //入实时表
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                        model.STCD = MN;
                        model.TM = CD.TM; //监测时间
                        model.ItemID = CD.DM[i].ItemCode;//监测项  
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = CD.DM[i].DATAVALUE; //值
                        model.CorrectionVALUE = CD.DM[i].DATAVALUE;
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = CD.ST;
                        model.NFOINDEX = (int)NFOINDEX;
                        model.DATATYPE = int.Parse(CD.CN);


                        Service.PublicBD.db.AddRealTimeData(model);
                        //////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "接收到自报数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == MN select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "接收到自报数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == MN select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }

                        }
                        #endregion
                        #region gsm通知界面
                        if ((int)NFOINDEX == 3)
                        {
                            GsmService.GsmServer GS = Server as GsmService.GsmServer;
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, MN, "接收到自报数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, MN, "接收到自报数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                       
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "2011操作异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 上传设备运行状态数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2021(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传污染物日历史数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2031(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                if (pd.GetValidataLength(data))
                {
                    string MN = pd.GetMN(data);
                    DateTime DOWNDATE = DateTime.Now;

                    CN_DataList CD = pd.GetValues(data);

                    //获得回复----------------------------------
                    if (true)
                    {

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == MN && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == MN && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    if (CD.DM.Count() > 0)
                        for (int i = 0; i < CD.DM.Count(); i++)
                        {
                            string ItemName = null;

                            #region //入实时表
                            Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                            model.STCD = MN;
                            model.TM = CD.TM; //监测时间
                            model.ItemID = CD.DM[i].ItemCode;//监测项  
                            model.DOWNDATE = DOWNDATE;
                            model.DATAVALUE = CD.DM[i].DATAVALUE; //值
                            model.CorrectionVALUE = CD.DM[i].DATAVALUE;
                            //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                            model.STTYPE = CD.ST;
                            model.NFOINDEX = (int)NFOINDEX;
                            model.DATATYPE =int.Parse(CD.CN);
                            #region //2031*100 + 1  =203101 =日历史数据累计值 (参考HJT212_2005.ParseData.Key)
                            model.DATATYPE = model.DATATYPE * 100 + Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY));
                            model.TM = model.TM.AddMilliseconds(10 * Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY)));//避免主键冲突
                            #endregion

                            #region 环保212协议特殊添加
                            string s4 = model.DATATYPE.ToString().Substring(0, 4);
                            string s2 = model.DATATYPE.ToString().Substring(4, 2);

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

                            #endregion

                            ItemName += GetItemName(CD.DM[i].ItemCode);

                            Service.PublicBD.db.AddRealTimeData(model);
                            //////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "接收到日历史数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                var list = from rtu in TS.Ts where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "接收到日历史数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                var list = from rtu in US.Us where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }

                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, MN, "接收到日历史数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, MN, "接收到日历史数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion

                            #endregion
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "2031操作异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 上传设备运行时间日历史数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2041(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传污染物分钟数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2051(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                if (pd.GetValidataLength(data))
                {
                    string MN = pd.GetMN(data);
                    DateTime DOWNDATE = DateTime.Now;

                    CN_DataList CD = pd.GetValues(data);

                    //获得回复----------------------------------
                    if (true)
                    {

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == MN && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == MN && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    if (CD.DM.Count() > 0)
                        for (int i = 0; i < CD.DM.Count(); i++)
                        {
                            string ItemName = null;

                            #region //入实时表
                            Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                            model.STCD = MN;
                            model.TM = CD.TM; //监测时间
                            model.ItemID = CD.DM[i].ItemCode;//监测项  
                            model.DOWNDATE = DOWNDATE;
                            model.DATAVALUE = CD.DM[i].DATAVALUE; //值
                            model.CorrectionVALUE = CD.DM[i].DATAVALUE;
                            //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                            model.STTYPE = CD.ST;
                            model.NFOINDEX = (int)NFOINDEX;
                            model.DATATYPE = int.Parse(CD.CN);
                            #region //2051*100 + 1  =205101 =分钟数据累计值 (参考HJT212_2005.ParseData.Key)
                            model.DATATYPE = model.DATATYPE * 100 + Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY));
                            model.TM = model.TM.AddMilliseconds(10 * Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY)));//避免主键冲突
                            #endregion

                            #region 环保212协议特殊添加
                            string s4 = model.DATATYPE.ToString().Substring(0, 4);
                            string s2 = model.DATATYPE.ToString().Substring(4, 2);

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

                            #endregion

                            ItemName += GetItemName(CD.DM[i].ItemCode);

                            Service.PublicBD.db.AddRealTimeData(model);
                            //////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "接收到分钟数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                var list = from rtu in TS.Ts where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "接收到分钟数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                var list = from rtu in US.Us where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }

                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, MN, "接收到分钟数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, MN, "接收到分钟数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion

                            #endregion
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "2051操作异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 上传污染物小时数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2061(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                if (pd.GetValidataLength(data))
                {
                    string MN = pd.GetMN(data);
                    DateTime DOWNDATE = DateTime.Now;

                    CN_DataList CD = pd.GetValues(data);

                    //获得回复----------------------------------
                    if (true)
                    {

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == MN && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == MN && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if ("" == null)//生成回复报失败----------------------
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = new byte[] { };  //回复报的内容-------------------------------------
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    if (CD.DM.Count() > 0)
                        for (int i = 0; i < CD.DM.Count(); i++)
                        {
                            string ItemName = null;

                            #region //入实时表
                            Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                            model.STCD = MN;
                            model.TM = CD.TM; //监测时间
                            model.ItemID = CD.DM[i].ItemCode;//监测项  
                            model.DOWNDATE = DOWNDATE;
                            model.DATAVALUE = CD.DM[i].DATAVALUE; //值
                            model.CorrectionVALUE = CD.DM[i].DATAVALUE;
                            //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                            model.STTYPE = CD.ST;
                            model.NFOINDEX = (int)NFOINDEX;
                            model.DATATYPE = int.Parse(CD.CN);
                            #region //2061*100 + 1  =206101 =小时数据累计值 (参考HJT212_2005.ParseData.Key)
                            model.DATATYPE = model.DATATYPE * 100 + Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY));
                            model.TM = model.TM.AddMilliseconds(10 * Convert.ToInt32(Enum.Parse(typeof(HJT212_2005.ParseData.Key), CD.DM[i].KEY)));//避免主键冲突
                            #endregion

                            #region 环保212协议特殊添加
                            string s4 = model.DATATYPE.ToString().Substring(0, 4);
                            string s2 = model.DATATYPE.ToString().Substring(4, 2);

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

                            #endregion

                            ItemName += GetItemName(CD.DM[i].ItemCode);
                            Service.PublicBD.db.AddRealTimeData(model);
                            //////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////
                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, MN, "接收到小时数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                var list = from rtu in TS.Ts where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, MN, "接收到小时数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                var list = from rtu in US.Us where rtu.STCD == MN select rtu;
                                if (list.Count() > 0)
                                {
                                    list.First().CanSend = false;
                                }

                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, MN, "接收到小时数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, MN, "接收到小时数据，数据特征[" + ItemName + "-" + CD.DM[i].ItemCode + "]，时间[" + CD.TM + "],值[" + CD.DM[i].DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion

                            #endregion
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "2061操作异常" + ex.ToString());
            }
        }
        
        /// <summary>
        /// 上传污染物报警记录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2071(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }

        /// <summary>
        /// 上传报警事件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_2072(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

        }



        #region 得到itemname
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
