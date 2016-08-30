using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADJC_001
{
    static class PackageProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        static ParseData pd = new ParseData();

        /// <summary>
        /// 上传状态报
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_STAT(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            DataModel DM = pd.UnPack(data);
            DateTime DOWNDATE = DateTime.Now;
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == DM.Code && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();


                if (Tcps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(DM.Reply);
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, DM.Code, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion
            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == DM.Code && u.IpEndPoint != null select u;



                if (udps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(DM.Reply);
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, DM.Code, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            if (DM.Item_data.Count() > 0)
                for (int i = 0; i < DM.Item_data.Count(); i++)
                {
                    string StateName = GetStateName(DM.Item_data[i].Item);

                    #region //入状态表

                    Service.Model.YY_DATA_STATE model = new Service.Model.YY_DATA_STATE();
                   

                    model.STCD = DM.Code;
                    model.TM = DM.Datetime; //监测时间
                    model.STATEDATA = DM.Item_data[i].Item;//监测项  
                    model.DOWNDATE = DOWNDATE;
                    model.DATAVALUE = decimal.Parse(DM.Item_data[i].Data); //值
                    model.NFOINDEX = (int)NFOINDEX;


                    Service.PublicBD.db.AddRTUState(model);
                    //////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, DM.Code, "接收到状态报数据，数据特征[" + StateName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == DM.Code select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, DM.Code, "接收到状态报数据，数据特征[" + StateName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == DM.Code select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, DM.Code, "接收到状态报数据，数据特征[" + StateName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, DM.Code, "接收到状态报数据，数据特征[" + StateName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion

                    #endregion
                }
        }

        /// <summary>
        /// 上传参数报
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_PARM(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            
        }
        /// <summary>
        /// 上传监测数据报
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_DATA(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            DataModel DM = pd.UnPack(data);
            DateTime DOWNDATE = DateTime.Now;
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == DM.Code && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();


                if (Tcps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(DM.Reply);
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, DM.Code, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion
            #region udp回复
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == DM.Code && u.IpEndPoint != null select u;



                if (udps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(DM.Reply);
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, DM.Code, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            if (DM.Item_data.Count() > 0)
                for (int i = 0; i < DM.Item_data.Count(); i++)
                {
                    string ItemName = GetItemName(DM.Item_data[i].Item);

                    #region //入实时表
                    Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                    model.STCD = DM.Code;
                    model.TM = DM.Datetime; //监测时间
                    model.ItemID = DM.Item_data[i].Item;//监测项  
                    model.DOWNDATE = DOWNDATE;
                    model.DATAVALUE = decimal.Parse(DM.Item_data[i].Data); //值
                    model.CorrectionVALUE = model.DATAVALUE;
                    //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                    model.STTYPE = DM.PacketCode.ToString();
                    model.NFOINDEX = (int)NFOINDEX;
                    //数据类型暂存包号 2016.6.6
                    //model.DATATYPE = DM.PacketCode;


                    Service.PublicBD.db.AddRealTimeData(model);
                    //////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, DM.Code, "接收到数据报数据，数据特征[" + ItemName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == DM.Code select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, DM.Code, "接收到数据报数据，数据特征[" + ItemName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == DM.Code select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, DM.Code, "接收到数据报数据，数据特征[" + ItemName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, DM.Code, "接收到数据报数据，数据特征[" + ItemName + "-" + DM.Item_data[i].Item + "]，时间[" + DM.Datetime + "],值[" + DM.Item_data[i].Data + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion

                    #endregion
                }
        }

        #region 得到itemname&statename
        private static string GetItemName(string itemid)
        {
            string ItemName = "";
            var item = from i in Service.ServiceBussiness.ITEMList where  i.ItemID == itemid select i;
            if (item.Count() > 0)
            {
                ItemName = item.First().ItemName;
            }
            return ItemName;
        }

        private static string GetStateName(string stateid)
        {
            string StateName = "";
            var item = from i in Service.ServiceBussiness.STATEList where i.STATEID == stateid select i;
            if (item.Count() > 0)
            {
                StateName = item.First().RTUSTATE;
            }
            return StateName;
        }
        #endregion
    }
}
