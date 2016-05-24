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
        static YYPack.Pack pack = new YYPack.Pack();


        //自报
        internal static void Process_01(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string STCD = SetSTCD(solution,NFOINDEX ,Server);
            //打回复包 101=0065 ---C
            string Answer = pack.GetAnswer(solution.N, "0065", GetAuto(STCD), GetWaterLevelOrder(STCD), GetRainfallOrder(STCD), GetPowerMode(STCD), GetCenterAddress(STCD), "FFFF", "FFFF", "FFFF");
             
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
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
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            //是否解包成功
            if (solution.IsUnpack)
            {
                int k = 0;
                foreach (DLYY.Model.WaterLevelInfo waterlevel in solution.Items)
                {
                    int Milliseconds = (Convert.ToInt32(solution.N, 16) % 10) * 100 +(10* k++);
                    PublicBD.db.AddRealTimeData(STCD, waterlevel.ItemID, DateTime.Parse(waterlevel.TM).AddMilliseconds(Milliseconds), DateTime.Now, (int)NFOINDEX, decimal.Parse(waterlevel.DATAVALUE));
                    //k++;

                    string ItemName = GetItemName(waterlevel.ItemID);
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到自报数据,数据特征[" + ItemName + "-" + waterlevel.ItemID + "],时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region gsm通知界面
                    if ((int)NFOINDEX == 3)
                    {
                        GsmService.GsmServer GS = Server as GsmService.GsmServer;
                        ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    if (solution.Items.Count == k)
                    {
                        ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                }
            }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "01操作异常" + ex.ToString());
            }
        }

        //人工置数
        internal static void Process_02(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
            string STCD =  SetSTCD(solution,NFOINDEX ,Server);
            //打回复包 102=0066 ---C
            string Answer = pack.GetAnswer(solution.N, "0066", GetAuto(STCD), GetWaterLevelOrder(STCD), GetRainfallOrder(STCD), GetPowerMode(STCD), GetCenterAddress(STCD), "FFFF", "FFFF", "FFFF");

            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
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
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            if (solution.IsUnpack)
            {
                int k = 0;
                foreach (DLYY.Model.WaterLevelInfo waterlevel in solution.Items)
                {

                    int Milliseconds = (Convert.ToInt32(solution.N, 16) % 10) * 100 + (10 * k++);

                    Service.Model.YY_DATA_MANUAL model = new Model.YY_DATA_MANUAL();
                    model.STCD = STCD;
                    model.DATAVALUE = waterlevel.DATAVALUE;
                    model.TM = DateTime.Now.AddMilliseconds(Milliseconds);
                    model.DOWNDATE = DateTime.Now;
                    model.NFOINDEX = (int)NFOINDEX;
                    PublicBD.db.AddManualData(model);
                    //k++;

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到人工置数，值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到人工置数，值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region gsm通知界面
                    if ((int)NFOINDEX == 3)
                    {
                        GsmService.GsmServer GS = Server as GsmService.GsmServer;
                        ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到人工置数，值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到人工置数，值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                }
            }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "02操作异常" + ex.ToString());
            }
        }

        //召测当前数据
        internal static void Process_03(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string STCD = SetSTCD(solution, NFOINDEX, Server);
            #region  //3上报后 中心端不做响应
            //打回复包 103=0067 ---C
            //string Answer = pack.GetAnswer(solution.N, "0067", GetAuto(STCD), GetWaterLevelOrder(STCD), GetRainfallOrder(STCD), GetPowerMode(STCD), GetCenterAddress(STCD), "FFFF", "FFFF", "FFFF");

            //#region tcp回复
            //if ((int)NFOINDEX == 1)
            //{
            //    TcpService.TcpServer TS = Server as TcpService.TcpServer;
            //    List<TcpService.TcpSocket> Ts = TS.Ts;

            //    var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
            //    List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
            //    //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
            //    if (Answer == null)
            //    {
            //        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
            //        return;
            //    }

            //    if (Tcps.Count() > 0)
            //    {
            //        byte[] sendData = Encoding.ASCII.GetBytes(Answer);
            //        Tcps.First().TCPSOCKET.Send(sendData);
            //        //回复通知界面
            //        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
            //    }

            //}
            //#endregion
            //#region udp回复
            //if ((int)NFOINDEX == 2)
            //{
            //    UdpService.UdpServer US = Server as UdpService.UdpServer;
            //    List<UdpService.UdpSocket> Us = US.Us;
            //    var udps = from u in Us where u.STCD == STCD && u.IpEndPoint != null select u;

            //    //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
            //    if (Answer == null)
            //    {
            //        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
            //        return;
            //    }

            //    if (udps.Count() > 0)
            //    {
            //        byte[] sendData = Encoding.ASCII.GetBytes(Answer);
            //        US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
            //        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
            //    }

            //}
            //#endregion
            #endregion

            if (solution.IsUnpack)
            {
                int k = 0;
                foreach (DLYY.Model.WaterLevelInfo waterlevel in solution.Items)
                {
                    int Milliseconds = (Convert.ToInt32(solution.N, 16) % 10) * 100 + (10 * k++);
                    PublicBD.db.AddRealTimeData(STCD, waterlevel.ItemID, DateTime.Parse(waterlevel.TM).AddMilliseconds(Milliseconds), DateTime.Now, (int)NFOINDEX, decimal.Parse(waterlevel.DATAVALUE));
                    //k++;

                    string ItemName = GetItemName(waterlevel.ItemID);
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "召测到当前数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "召测到当前数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region gsm通知界面
                    if ((int)NFOINDEX == 3)
                    {
                        GsmService.GsmServer GS = Server as GsmService.GsmServer;
                        ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "召测到当前数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "召测到当前数据，数据特征[" + ItemName + "-" + waterlevel.ItemID + "]，时间[" + waterlevel.TM + "],值[" + waterlevel.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                }
            }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "03操作异常" + ex.ToString());
            }
        }

        //参数修改的命令确认rtu上报5（中心端不用处理）
        internal static void Process_05(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try
            {
                string STCD = SetSTCD(solution, NFOINDEX, Server);
                #region tcp通知界面
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    List<TcpService.TcpSocket> Ts = TS.Ts;
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "RTU接收到修改参数命令[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }
                #endregion
                #region udp通知界面
                if ((int)NFOINDEX == 2)
                {
                    UdpService.UdpServer US = Server as UdpService.UdpServer;
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "RTU接收到修改参数命令[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }
                #endregion
                #region gsm通知界面
                if ((int)NFOINDEX == 3)
                {
                    GsmService.GsmServer GS = Server as GsmService.GsmServer;
                    ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "RTU接收到修改参数命令[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }
                #endregion
                #region com通知界面
                if ((int)NFOINDEX == 4)
                {
                    ComService.ComServer CS = Server as ComService.ComServer;
                    Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "RTU接收到修改参数命令[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "05操作异常" + ex.ToString());
            }
        }

        internal static void Process_06(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string STCD = SetSTCD(solution, NFOINDEX, Server);
            //打回复包 106=006A ---C
            string Answer = pack.GetAnswer(solution.N, "006A", GetAuto(STCD), GetWaterLevelOrder(STCD), GetRainfallOrder(STCD), GetPowerMode(STCD), GetCenterAddress(STCD), "FFFF", "FFFF", "FFFF");

                       
            #region tcp回复
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;

                var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (Tcps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    Tcps.First().TCPSOCKET.Send(sendData);
                    //回复通知界面
                    ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
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
                if (Answer == null)
                {
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    return;
                }

                if (udps.Count() > 0)
                {
                    byte[] sendData = Encoding.ASCII.GetBytes(Answer);
                    US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                    ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                }

            }
            #endregion

            if (solution.IsUnpack)
            {
                int k = 0;
                foreach (DLYY.Model.WaterLevelInfo waterlevel in solution.Items)
                {
                    int Milliseconds = (Convert.ToInt32(solution.N, 16) % 10) * 100 + (10 * k++);
                    Service.Model.YY_DATA_REM model = new Model.YY_DATA_REM();
                    model.STCD = STCD;
                    model.DATAVALUE = decimal.Parse(waterlevel.DATAVALUE);
                    model.TM = DateTime.Parse(waterlevel.TM).AddMilliseconds(Milliseconds);
                    model.DOWNDATE = DateTime.Now;
                    model.NFOINDEX = (int)NFOINDEX;
                    model.ItemID = waterlevel.ItemID;
                    PublicBD.db.AddRemData(model);
                    //k++;

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "提取到固态数据(有下一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "提取到固态数据(有下一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region gsm通知界面
                    if ((int)NFOINDEX == 3)
                    {
                        GsmService.GsmServer GS = Server as GsmService.GsmServer;
                        ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "提取到固态数据(有下一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "提取到固态数据(有下一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                }
            }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "06操作异常" + ex.ToString());
            }
        }

        //提取固态时，无查询结果或发送查询结果至最后一条时rtu上报7（中心端不用处理）
        internal static void Process_07(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try{
                string STCD = SetSTCD(solution, NFOINDEX, Server);
            if (solution.IsUnpack)
            {
                int k = 0;
                if (solution.Items == null)
                {
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "召测时间段内没有数据[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "召测时间段内没有数据[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region gsm通知界面
                    if ((int)NFOINDEX == 3)
                    {
                        GsmService.GsmServer GS = Server as GsmService.GsmServer;
                        ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "召测时间段内没有数据[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "召测时间段内没有数据[确认]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                }
                else
                {
                    foreach (DLYY.Model.WaterLevelInfo waterlevel in solution.Items)
                    {
                        int Milliseconds = (Convert.ToInt32(solution.N, 16) % 10) * 100 + (10 * k++);
                        Service.Model.YY_DATA_REM model = new Model.YY_DATA_REM();
                        model.STCD = STCD;
                        model.DATAVALUE = decimal.Parse(waterlevel.DATAVALUE);
                        model.TM = DateTime.Parse(waterlevel.TM).AddMilliseconds(Milliseconds);
                        model.DOWNDATE = DateTime.Now;
                        model.NFOINDEX = (int)NFOINDEX;
                        model.ItemID = waterlevel.ItemID;
                        PublicBD.db.AddRemData(model);
                        //k++;


                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "提取到固态数据(最后一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "提取到固态数据(最后一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region gsm通知界面
                        if ((int)NFOINDEX == 3)
                        {
                            GsmService.GsmServer GS = Server as GsmService.GsmServer;
                            ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "提取到固态数据(最后一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "提取到固态数据(最后一帧)，数据特征[" + waterlevel.ItemID + "]，时间[" + model.TM + "],值[" + model.DATAVALUE + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
                
            }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "07操作异常" + ex.ToString());
            }
        
        }

        #region  从数据库里得到回复用的数据
        /// <summary>
        /// 得到设备自报间隔D7  (默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        private static string GetAuto(string STCD)
        {
            if (ServiceBussiness.CONFIGDATAList != null && ServiceBussiness.CONFIGDATAList.Count > 0)
            {
                var time = from t in ServiceBussiness.CONFIGDATAList where t.STCD == STCD && t.ConfigID == "1000000007" select t;
                if (time.Count() > 0)
                {
                    return Convert.ToInt32(time.First().ConfigVal,16).ToString().PadLeft(4, '0');
                }
            }

            return "0001";
        }

        /// <summary>
        /// 得到雨量量级D10（默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private static string GetRainfallOrder(string STCD)
        {
            if (ServiceBussiness.CONFIGDATAList != null && ServiceBussiness.CONFIGDATAList.Count > 0)
            {
                var time = from t in ServiceBussiness.CONFIGDATAList where t.STCD == STCD && t.ConfigID == "1000000010" select t;
                if (time.Count() > 0)
                {
                    decimal Val = 0;
                    if (decimal.TryParse(time.First().ConfigVal, out Val))
                    {
                        return Convert.ToInt32(Val.ToString(),16).ToString().PadLeft(4, '0');
                    }
                }
            }

            return "0001";
        }

        /// <summary>
        /// 得到水位量级D8（默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private static string GetWaterLevelOrder(string STCD)
        {
            if (ServiceBussiness.CONFIGDATAList != null && ServiceBussiness.CONFIGDATAList.Count > 0)
            {
                var time = from t in ServiceBussiness.CONFIGDATAList where t.STCD == STCD && t.ConfigID == "1000000010" select t;
                if (time.Count() > 0)
                {
                    decimal Val = 0;
                    if (decimal.TryParse(time.First().ConfigVal, out Val))
                    {
                        return Convert.ToInt32(Val.ToString(),16).ToString().PadLeft(4, '0');
                    }
                }
            }

            return "0001";
        }

        /// <summary>
        /// 得到电源供电模式D11（默认值0 =0000自动掉电  1=0001保持长供电）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private static string GetPowerMode(string STCD)
        {
            if (ServiceBussiness.CONFIGDATAList != null && ServiceBussiness.CONFIGDATAList.Count() > 0)
            {
                var work = from w in ServiceBussiness.CONFIGDATAList where w.STCD == STCD && w.ItemID=="1000000011"  select w;
                if (work.Count() > 0)
                {
                    return Convert.ToInt32(work.First().ConfigVal,16).ToString().PadLeft(4, '0');
                }
            }

            return "0000";
        }

        /// <summary>
        ///得到能够远程提取历史数据的中心站号D13(默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        ///// <returns></returns>
        private static string GetCenterAddress(string STCD)
        {
            //YY_RTU_WRES表ADR_ZX字段 
            if (ServiceBussiness.WRESList != null && ServiceBussiness.WRESList.Count > 0)
            {
                var wres = from w in ServiceBussiness.WRESList where w.STCD == STCD && w.ADR_ZX != null && w.CODE == 1 select w;
                if (wres.Count() > 0)
                {
                    return pack.From10ToX(wres.First().ADR_ZX.Value, 16).ToString().PadLeft(4, '0');
                }
            }

            return "0001";

        }
        #endregion

        #region 得到itemname
        private static string GetItemName(string itemid) 
        {
            string ItemName = "";
            var item = from i in ServiceBussiness.ITEMList where i.ItemID == itemid select i;
            if (item.Count() > 0)
            {
                ItemName = item.First().ItemName;
            }
            return ItemName;
        }
        #endregion

        //设置站号 tcp、udp 端口+00+编号  gsm、com 0000000+编号
        private static string SetSTCD(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string STCD = solution.STCD.PadLeft(10, '0');
            //if ((int)NFOINDEX == 1)
            //{
            //    TcpService.TcpServer TS = Server as TcpService.TcpServer;
            //    STCD = TS.PORT.ToString().PadLeft(5, '0') + solution.STCD.PadLeft(5, '0');
            //}
            //if ((int)NFOINDEX == 2)
            //{
            //    UdpService.UdpServer US = Server as UdpService.UdpServer;
            //    STCD = US.PORT.ToString().PadLeft(5, '0') + solution.STCD.PadLeft(5, '0');
            //}
            return STCD;
        }
    }
}
