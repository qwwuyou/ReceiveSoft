using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKHY_S3
{
    static class PackageProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        static ParseData pd = new ParseData();

        /// <summary>
        /// 上传监测数据报
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_FD(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            DataModel DM = pd.UnPack(data);

            #region 数据报信息
            string PacketType="";
            switch (DM.PacketType)
            {
                case "01":
                    PacketType = "定时报";
                    break;
                case "02":
                    PacketType = "加报";
                    break;
                default:
                    PacketType = "手动召测";
                    break;
            }

            //TCP
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;

                string Explain = "接收到数据报(" + PacketType + "),报头（"+DM.TiltleCode +"），RTU地址(" + DM.Code + ") 测量时间(" + DM.Datetime + ")  计算算法(" + DM.Algorithm + ")  测流历时(" + DM.Take + "秒)  左岸系数(" + DM.LeftBank + ")  右岸系数(" + DM.RightBank + ")   起算水位(" + DM.StartStage + ")   断面面积(" + DM.Section + "平方米)  参与计算的探头数量/探头数量("+DM.ComputeSensorCount +"/"+DM.SensorCount +")";
                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, DM.Code, Explain , new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
            #endregion

            DateTime DOWNDATE = DateTime.Now;
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
                    model.DATAVALUE =DM.Item_data[i].Data; //值
                    model.CorrectionVALUE = model.DATAVALUE;
                    model.NFOINDEX = (int)NFOINDEX;
                   


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
