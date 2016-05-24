using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YanYu.Protocol.HIMR.LinkLayer;
using YanYu.Protocol.HIMR.InPacket;
using YanYu.Protocol.HIMR;
using System.IO;

namespace HydrologicProtocol
{
    static class PackageProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");
        public static Dictionary<string, InPackage> CurePackageList = new Dictionary<string, InPackage>();

        //用于存储多包报文  0x36（图片）和多包升级
        static PackageProcess()
        {
            ImageFramesCache.Notify += new EventHandler<NotifyEventArgs>(ImageFramesCache_Notify);
            foreach (var item in Service.ServiceBussiness.RtuList)
            {
                CurePackageList.Add(item.STCD, new InPackage());
            }
        }

        static void ImageFramesCache_Notify(object sender, NotifyEventArgs e)
        {
            string STCD = e.STCD.ToString();

            try
            {
                if (e.Status == RetFlag.ASK || e.Status == RetFlag.SuccessASK)
                {
                    Frame askFrame = ImageFramesCache.GetAskFrame(e.Key, e.Param, GetOnLine(STCD));
                    byte[] ask;
                    if (askFrame != null)
                    {
                        ask = askFrame.ToBytes();

                        #region tcp回复
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "ASK:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                                Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == STCD && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);


                                string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "ASK:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                                Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                            }

                        }
                        #endregion


                        #region tcp通知界面
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "图片报不完整，向RTU索要第"+askFrame.FramHead.CurrentPackageNo+"帧", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "图片报不完整，向RTU索要第"+askFrame.FramHead.CurrentPackageNo+"帧", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion

                    }
                }
                else if (e.Status == RetFlag.Success)
                {
                    byte[] ask = null;
                    //得到回复报
                    ask = ImageFramesCache.GetAskFrame(e.Key, e.Param, GetOnLine(STCD)).ToBytes();


                    #region tcp回复
                    if ((int)e.NFOIndex == 1)
                    {
                        TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                        List<TcpService.TcpSocket> Ts = TS.Ts;

                        var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                        List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                        //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                        if (ask == null)
                        {
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            return;
                        }

                        if (Tcps.Count() > 0)
                        {
                            byte[] sendData = ask;
                            Tcps.First().TCPSOCKET.Send(sendData);
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "SUC:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                            Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                        }

                    }
                    #endregion
                    #region udp回复
                    if ((int)e.NFOIndex == 2)
                    {
                        UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                        List<UdpService.UdpSocket> Us = US.Us;
                        var udps = from u in Us where u.STCD == STCD && u.IpEndPoint != null select u;

                        //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                        if (ask == null)
                        {
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            return;
                        }

                        if (udps.Count() > 0)
                        {
                            byte[] sendData = ask;
                            US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);


                            string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "SUC:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                            Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                        }

                    }
                    #endregion


                    //成功-生成图片
                    InPackage inPackage = ImageFramesCache.GetImagePackage(e.Key);
                    Deal0x36 package = new Deal0x36(inPackage);
                    byte[] img = package.packData.Image;

                    if (img != null && img.Length > 0)
                    {
                        Service.Model.YY_DATA_IMG model = new Service.Model.YY_DATA_IMG();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.DOWNDATE = DateTime.Now;
                        model.DATAVALUE = img; //值
                        model.NFOINDEX = (int)e.NFOIndex;

                        Service.PublicBD.db.AddImg(model);

                        string ItemName = "图片";
                        #region tcp通知界面
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成图片，数据特征[" + ItemName + "-IMG]，时间[" + package.packData.ObservationTime + "],值[0000]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成图片，数据特征[" + ItemName + "-IMG]，时间[" + package.packData.ObservationTime + "],值[0000]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                    }
                    else 
                    {
                        Service.Model.YY_DATA_IMG model = new Service.Model.YY_DATA_IMG();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.DOWNDATE = DateTime.Now;
                        model.DATAVALUE = new byte[] { };
                        model.NFOINDEX = (int)e.NFOIndex;
                        model.DATATYPE = -1;
                        Service.PublicBD.db.AddImg(model);

                        string ItemName = "图片";

                        #region tcp通知界面
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成图片，数据特征[" + ItemName + "-IMG]，时间[" + package.packData.ObservationTime + "],值[null]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成图片，数据特征[" + ItemName + "-IMG]，时间[" + package.packData.ObservationTime + "],值[null]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                    }


                }
                else if (e.Status == RetFlag.BeforeASK) 
                {
                    Frame askFrame = ImageFramesCache.GetAskFrame(e.Key, e.Param, GetOnLine(STCD));
                    byte[] ask;
                    if (askFrame != null)
                    {
                        ask = askFrame.ToBytes();

                        #region tcp回复
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "BEF:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                                Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                            }

                        }
                        #endregion
                        #region udp回复
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            List<UdpService.UdpSocket> Us = US.Us;
                            var udps = from u in Us where u.STCD == STCD && u.IpEndPoint != null select u;

                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "BEF:" + Service.EnCoder.ByteArrayToHexStr(sendData);
                                Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                            }

                        }
                        #endregion

                        #region tcp通知界面
                        if ((int)e.NFOIndex == 1)
                        {
                            TcpService.TcpServer TS = e.Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "确认开始接收图片", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)e.NFOIndex == 2)
                        {
                            UdpService.UdpServer US = e.Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "确认开始接收图片", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (list.Count() > 0)
                            {
                                list.First().CanSend = false;
                            }
                        }
                        #endregion

                    }
                }
            }
            catch(Exception ex)
            { 
                log.Error(DateTime.Now + "36操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x2F(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到链路维持报", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                        if (list.Count() > 0)
                        {
                            list.First().CanSend = true;
                        }
                    }
                    #endregion
                    #region udp通知界面
                    if ((int)NFOINDEX == 2)
                    {
                        UdpService.UdpServer US = Server as UdpService.UdpServer;
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到链路维持报", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                        if (list.Count() > 0)
                        {
                            list.First().CanSend = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "2F操作异常" + ex.ToString());
            }
        }

        //测试报，不入库
        internal static void Process_0x30(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;



                    Deal0x30 package = new Deal0x30(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package); // package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {

                        //Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                        //model.STCD = STCD;
                        //model.TM = package.packData.ObservationTime; //监测时间
                        //model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        //model.DOWNDATE = DOWNDATE;
                        //model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        //model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //model.NFOINDEX = (int)NFOINDEX;

                        //Service.PublicBD.db.AddRealTimeData(model);

                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到测试报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到测试报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到测试报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到测试报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "30操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x31(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x31 package = new Deal0x31(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        Service.Model.YY_DATA_REM model = new Service.Model.YY_DATA_REM();

                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.NFOINDEX = (int)NFOINDEX;

                        //31H	均匀时段水文信息报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }
                        Service.PublicBD.db.AddRemData(model);

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到均匀时段水文信息报数据(固)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到均匀时段水文信息报数据(固)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到均匀时段水文信息报数据(固)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到均匀时段水文信息报数据(固)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "31操作异常" + ex.ToString());
            }
        }

        //自报
        internal static void Process_0x32(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x32 package = new Deal0x32(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region //入固态表
                        if (0xF4 <= package.packData.Values[i].Style.SymbolHex && package.packData.Values[i].Style.SymbolHex <= 0xFC)
                        {
                            Service.Model.YY_DATA_REM Model = new Service.Model.YY_DATA_REM();
                            Model.STCD = STCD;
                            Model.TM = package.packData.ObservationTime; //监测时间
                            Model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                            Model.DOWNDATE = DOWNDATE;
                            Model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                            Model.NFOINDEX = (int)NFOINDEX;
                            Model.DATATYPE = package.packData.Values[i].Style.SymbolHex;
                            Service.PublicBD.db.AddRemData(Model);

                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            if (package.packData.Values.Count == i)
                            {
                                Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            continue;
                        }
                        #endregion

                        #region //入实时表
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项  
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }
                        Service.PublicBD.db.AddRealTimeData(model);
                        //////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "32操作异常" + ex.ToString());
            }
        }

        //加报
        internal static void Process_0x33(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x33 package = new Deal0x33(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }


                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region //入固态表
                        if (0xF4 <= package.packData.Values[i].Style.SymbolHex && package.packData.Values[i].Style.SymbolHex <= 0xFC)
                        {
                            Service.Model.YY_DATA_REM Model = new Service.Model.YY_DATA_REM();
                            Model.STCD = STCD;
                            Model.TM = package.packData.ObservationTime; //监测时间
                            Model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                            Model.DOWNDATE = DOWNDATE;
                            Model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                            Model.NFOINDEX = (int)NFOINDEX;
                            Model.DATATYPE = package.packData.Values[i].Style.SymbolHex;
                            Service.PublicBD.db.AddRemData(Model);

                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            if (package.packData.Values.Count == i)
                            {
                                Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            continue;
                        }
                        #endregion

                        #region//入实时表
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }

                        Service.PublicBD.db.AddRealTimeData(model);

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到加报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "33操作异常" + ex.ToString());
            }
        }

        //小时报
        internal static void Process_0x34(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x34 package = new Deal0x34(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region //入固态表
                        if (0xF4 <= package.packData.Values[i].Style.SymbolHex && package.packData.Values[i].Style.SymbolHex <= 0xFC)
                        {
                            Service.Model.YY_DATA_REM Model = new Service.Model.YY_DATA_REM();
                            Model.STCD = STCD;
                            Model.TM = package.packData.ObservationTime; //监测时间
                            Model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                            Model.DOWNDATE = DOWNDATE;
                            Model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                            Model.NFOINDEX = (int)NFOINDEX;
                            Model.DATATYPE = package.packData.Values[i].Style.SymbolHex;
                            Service.PublicBD.db.AddRemData(Model);

                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            continue;
                        }
                        #endregion

                        #region //入实时表
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }

                        Service.PublicBD.db.AddRealTimeData(model);

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到小时报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "34操作异常" + ex.ToString());
            }
        }

        //黑龙江增过程报EF
        internal static void Process_0xEF(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0xEF package = new Deal0xEF(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region //入固态表
                        if (0xF4 <= package.packData.Values[i].Style.SymbolHex && package.packData.Values[i].Style.SymbolHex <= 0xFC)
                        {
                            Service.Model.YY_DATA_REM Model = new Service.Model.YY_DATA_REM();
                            Model.STCD = STCD;
                            Model.TM = package.packData.ObservationTime; //监测时间
                            Model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                            Model.DOWNDATE = DOWNDATE;
                            Model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                            Model.NFOINDEX = (int)NFOINDEX;
                            Model.DATATYPE = package.FunCode; //EF的十进制 =239
                            Service.PublicBD.db.AddRemData(Model);

                            #region tcp通知界面
                            if ((int)NFOINDEX == 1)
                            {
                                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到过程报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region udp通知界面
                            if ((int)NFOINDEX == 2)
                            {
                                UdpService.UdpServer US = Server as UdpService.UdpServer;
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到过程报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region gsm通知界面
                            if ((int)NFOINDEX == 3)
                            {
                                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                                Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到过程报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                            #region com通知界面
                            if ((int)NFOINDEX == 4)
                            {
                                ComService.ComServer CS = Server as ComService.ComServer;
                                Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到过程报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            #endregion
                        }
                        #endregion

                        #region //入实时表
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();

                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项  
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }
                        Service.PublicBD.db.AddRealTimeData(model);
                        //////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到自报数据，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "34操作异常" + ex.ToString());
            }
        }
        //人工置数报
        internal static void Process_0x35(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x35 package = new Deal0x35(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);//package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {

                        Service.Model.YY_DATA_MANUAL model = new Service.Model.YY_DATA_MANUAL();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime;
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = package.packData.Values[i].ToString(); //值
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }



                        Service.PublicBD.db.AddManualData(model);

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到人工置数报数据，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到人工置数报数据，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到人工置数报数据，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到人工置数报数据，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "35操作异常" + ex.ToString());
            }
        }

        //图片
        internal static void Process_0x36(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                string STCD = frame.FramHead.StationAddress.ToString();
                #region tcp通知界面
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    //回复通知界面
                    Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到图片数据，第"+frame.FramHead.CurrentPackageNo +"/"+frame.FramHead.SumPackageCount +"包", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                    var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                    Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到图片数据，第" + frame.FramHead.CurrentPackageNo + "/" + frame.FramHead.SumPackageCount + "包", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                    if (list.Count() > 0)
                    {
                        list.First().CanSend = false;
                    }
                }
                #endregion

                RetStatus status = ImageFramesCache.AddFrame(frame, (int)NFOINDEX, Server);

                string dataStr = DateTime.Now.ToString("yyyyMMddHHmmss") + " " + "REV:" + Service.EnCoder.ByteArrayToHexStr(frame.ToBytes());
                Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "36操作异常" + ex.ToString());
            }
        }

        //实时数据(查询)
        internal static void Process_0x37(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x37 package = new Deal0x37(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }

                        Service.PublicBD.db.AddRealTimeData(model);


                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到实时数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到实时数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到实时数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到实时数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "37操作异常" + ex.ToString());
            }
        }

        //时段数据(查询)
        internal static void Process_0x38(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x31 package = new Deal0x31(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {

                        Service.Model.YY_DATA_REM model = new Service.Model.YY_DATA_REM();

                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.NFOINDEX = (int)NFOINDEX;

                        //31H	均匀时段水文信息报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }
                        Service.PublicBD.db.AddRemData(model);

                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到时段数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到时段数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到时段数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到时段数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "38操作异常" + ex.ToString());
            }
        }

        //人工置数报数据(查询)
        internal static void Process_0x39(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x35 package = new Deal0x35(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {

                        Service.Model.YY_DATA_MANUAL model = new Service.Model.YY_DATA_MANUAL();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime;
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = package.packData.Values[i].ToString(); //值
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }



                        Service.PublicBD.db.AddManualData(model);


                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到人工置数报数据(查询)，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到人工置数报数据(查询)，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到人工置数报数据(查询)，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到人工置数报数据(查询)，数据特征[" + package.packData.Values[i].Style.Name + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "39操作异常" + ex.ToString());
            }
        }

        //指定要素数据(查询)
        internal static void Process_0x3A(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x37 package = new Deal0x37(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }



                        Service.PublicBD.db.AddRealTimeData(model);

                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到指定要素数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到指定要素数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到指定要素数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到指定要素数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "3A操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x40(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x41 package = new Deal0x41(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    string Explain = "遥测站基本配置信息(设置)\n";
                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        ushort key = package.packData.Values[i].SymbolHex;
                        string explain = "";
                        switch (key)
                        {
                            case 0x01:
                                {
                                    _01H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x02:
                                {
                                    _02H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x03:
                                {
                                    _03H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x04:
                                {
                                    _04H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x05:
                                {
                                    _05H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x06:
                                {
                                    _06H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x07:
                                {
                                    _07H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x08:
                                {
                                    _08H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x09:
                                {
                                    _09H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0A:
                                {
                                    _0AH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0B:
                                {
                                    _0BH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0C:
                                {
                                    _0CH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0D:
                                {
                                    _0DH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0E:
                                {
                                    _0EH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0F:
                                {
                                    _0FH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "40操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x41(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x41 package = new Deal0x41(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    string Explain = "遥测站基本配置信息(查询)\n";
                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        ushort key = package.packData.Values[i].SymbolHex;
                        string explain = "";
                        switch (key)
                        {
                            case 0x01:
                                {
                                    _01H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x02:
                                {
                                    _02H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x03:
                                {
                                    _03H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x04:
                                {
                                    _04H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x05:
                                {
                                    _05H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x06:
                                {
                                    _06H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x07:
                                {
                                    _07H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x08:
                                {
                                    _08H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x09:
                                {
                                    _09H(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0A:
                                {
                                    _0AH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0B:
                                {
                                    _0BH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0C:
                                {
                                    _0CH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0D:
                                {
                                    _0DH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0E:
                                {
                                    _0EH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            case 0x0F:
                                {
                                    _0FH(package.packData.Values[i].ValueToString(), STCD, out explain);
                                    Explain += explain;
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "41操作异常" + ex.ToString());
            }
        }
        #region 40、41命令处理方法
        //RtuList
        //CONFIGDATAList
        //WRESList
        private static void _01H(string Value, string STCD, out string explain)
        {
            string[] values = Value.Split(new char[] { ',' });
            if (values.Length == 4)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 1 select l;
                if (list.Count() > 0)
                {
                    int adr = 0;
                    if (int.TryParse(values[0], out adr))
                    {
                        list.First().ADR_ZX = adr;
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    int adr = 0;
                    if (int.TryParse(values[0], out adr))
                    {
                        Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                        model.STCD = STCD;
                        model.CODE = 1;
                        model.ADR_ZX = adr;
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
                list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 2 select l;
                if (list.Count() > 0)
                {
                    int adr = 0;
                    if (int.TryParse(values[1], out adr))
                    {
                        list.First().ADR_ZX = adr;
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=2 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    int adr = 0;
                    if (int.TryParse(values[1], out adr))
                    {
                        Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                        model.STCD = STCD;
                        model.CODE = 2;
                        model.ADR_ZX = adr;
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
                list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 3 select l;
                if (list.Count() > 0)
                {
                    int adr = 0;
                    if (int.TryParse(values[2], out adr))
                    {
                        list.First().ADR_ZX = adr;
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=3 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    int adr = 0;
                    if (int.TryParse(values[2], out adr))
                    {
                        Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                        model.STCD = STCD;
                        model.CODE = 3;
                        model.ADR_ZX = adr;
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
                list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 4 select l;
                if (list.Count() > 0)
                {
                    int adr = 0;
                    if (int.TryParse(values[3], out adr))
                    {
                        list.First().ADR_ZX = adr;
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=4 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    int adr = 0;
                    if (int.TryParse(values[3], out adr))
                    {
                        Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                        model.STCD = STCD;
                        model.CODE = 4;
                        model.ADR_ZX = adr;
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
            explain = "                                                 中心站地址[" + Value + "]\n";
        }
        private static void _02H(string Value, string STCD, out string explain)
        {
            explain = "                                                 遥测站地址[" + Value + "]\n";
        }
        private static void _03H(string Value, string STCD, out string explain)
        {
            var list = from l in Service.ServiceBussiness.RtuList where l.STCD == STCD select l;
            if (list.Count() > 0)
            {
                list.First().PassWord = Value;
                Service.PublicBD.db.UpdRTU(list.First(), " where STCD='" + STCD + "'");
            }
            explain = "                                                 密码[" + Value + "]\n";
        }
        private static void _04H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });

            #region IPv4+短信/卫星
            if (values.Length == 4)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 1 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 7)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                list.First().PhoneNum = values[3];
                                explain = "                                                 中心1主信道[IPv4+短信]\n";

                                Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                            }

                        }
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 1;
                    int port = 0;
                    if (int.TryParse(values[2].Trim(), out port))
                    {
                        model.ADR_M = values[1];
                        model.PORT_M = port;
                        model.PhoneNum = values[3];
                        explain = "                                                 中心1主信道[IPv4+短信]\n";

                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }
                }
            }
            #endregion

            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 1 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_M = xd;
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心1主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_M = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心1主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                explain = "                                                 中心1主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_M = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心1主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心1主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心1主信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 1;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_M = xd;
                            explain = "                                                 中心1主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_M = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心1主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_M = xd;
                                model.ADR_M = values[1];
                                model.PORT_M = port;
                                explain = "                                                 中心1主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_M = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心1主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心1主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心1主信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _05H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });
            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 1 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_B = xd;
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心1备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_B = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心1备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_B = xd;
                                list.First().ADR_B = values[1];
                                list.First().PORT_B = port;
                                explain = "                                                 中心1备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_B = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心1备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心1备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心1备用信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 1;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_B = xd;
                            explain = "                                                 中心1备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_B = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心1备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_B = xd;
                                model.ADR_B = values[1];
                                model.PORT_B = port;
                                explain = "                                                 中心1备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_B = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心1备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心1备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心1备用信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _06H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });


            #region IPv4+短信/卫星
            if (values.Length == 4)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 2 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 7)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                list.First().PhoneNum = values[3];
                                explain = "                                                 中心2主信道[IPv4+短信]\n";

                                Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=2 and STCD='" + STCD + "'");
                            }

                        }
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 2;
                    int port = 0;
                    if (int.TryParse(values[2].Trim(), out port))
                    {
                        model.ADR_M = values[1];
                        model.PORT_M = port;
                        model.PhoneNum = values[3];
                        explain = "                                                 中心2主信道[IPv4+短信]\n";

                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }
                }
            }
            #endregion


            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 2 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_M = xd;
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心2主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_M = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心2主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                explain = "                                                 中心2主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_M = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心2主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心2主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心2主信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 2;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_M = xd;
                            explain = "                                                 中心2主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_M = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心2主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_M = xd;
                                model.ADR_M = values[1];
                                model.PORT_M = port;
                                explain = "                                                 中心2主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_M = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心2主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心2主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心2主信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _07H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });
            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 2 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_B = xd;
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心2备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_B = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心2备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_B = xd;
                                list.First().ADR_B = values[1];
                                list.First().PORT_B = port;
                                explain = "                                                 中心2备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_B = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心2备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心2备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心2备用信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 2;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_B = xd;
                            explain = "                                                 中心2备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_B = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心2备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_B = xd;
                                model.ADR_B = values[1];
                                model.PORT_B = port;
                                explain = "                                                 中心2备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_B = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心2备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心2备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心2备用信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _08H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });

            #region IPv4+短信/卫星
            if (values.Length == 4)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 3 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 7)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                list.First().PhoneNum = values[3];
                                explain = "                                                 中心3主信道[IPv4+短信]\n";

                                Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=3 and STCD='" + STCD + "'");
                            }

                        }
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 3;
                    int port = 0;
                    if (int.TryParse(values[2].Trim(), out port))
                    {
                        model.ADR_M = values[1];
                        model.PORT_M = port;
                        model.PhoneNum = values[3];
                        explain = "                                                 中心3主信道[IPv4+短信]\n";

                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }
                }
            }
            #endregion

            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 3 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_M = xd;
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心3主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_M = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心3主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                explain = "                                                 中心3主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_M = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心3主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心3主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心3主信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 3;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_M = xd;
                            explain = "                                                 中心3主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_M = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心3主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_M = xd;
                                model.ADR_M = values[1];
                                model.PORT_M = port;
                                explain = "                                                 中心3主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_M = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心3主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心3主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心3主信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _09H(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });
            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 3 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_B = xd;
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心3备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_B = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心3备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_B = xd;
                                list.First().ADR_B = values[1];
                                list.First().PORT_B = port;
                                explain = "                                                 中心3备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_B = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心3备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心3备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心3备用信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 3;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_B = xd;
                            explain = "                                                 中心3备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_B = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心3备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_B = xd;
                                model.ADR_B = values[1];
                                model.PORT_B = port;
                                explain = "                                                 中心3备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_B = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心3备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心3备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心3备用信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _0AH(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });
            #region IPv4+短信/卫星
            if (values.Length == 4)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 4 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 7)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                list.First().PhoneNum = values[3];
                                explain = "                                                 中心4主信道[IPv4+短信]\n";

                                Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=4 and STCD='" + STCD + "'");
                            }

                        }
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 4;
                    int port = 0;
                    if (int.TryParse(values[2].Trim(), out port))
                    {
                        model.ADR_M = values[1];
                        model.PORT_M = port;
                        model.PhoneNum = values[3];
                        explain = "                                                 中心4主信道[IPv4+短信]\n";

                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }
                }
            }
            #endregion

            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 4 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_M = xd;
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心4主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_M = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心4主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_M = xd;
                                list.First().ADR_M = values[1];
                                list.First().PORT_M = port;
                                explain = "                                                 中心4主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_M = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_M = null;
                            list.First().PORT_M = null;
                            explain = "                                                 中心4主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心4主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心4主信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 4;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_M = xd;
                            explain = "                                                 中心4主信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_M = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心4主信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_M = xd;
                                model.ADR_M = values[1];
                                model.PORT_M = port;
                                explain = "                                                 中心4主信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_M = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心4主信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_M = xd;
                            if (xd == 5)
                            { explain = "                                                 中心4主信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心4主信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _0BH(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] values = Value.Split(new char[] { ',' });
            if (values.Length == 3)
            {
                var list = from l in Service.ServiceBussiness.WRESList where l.STCD == STCD && l.CODE == 4 select l;
                if (list.Count() > 0)
                {
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            list.First().COM_B = xd;
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心4备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            list.First().COM_B = xd;
                            list.First().PhoneNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心4备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                list.First().COM_B = xd;
                                list.First().ADR_B = values[1];
                                list.First().PORT_B = port;
                                explain = "                                                 中心4备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            list.First().COM_B = xd;
                            list.First().SatelliteNum = values[1];
                            list.First().ADR_B = null;
                            list.First().PORT_B = null;
                            explain = "                                                 中心4备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心4备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心4备用信道[超短波]\n"; }
                        }
                        Service.PublicBD.db.UpdRTU_WRES(list.First(), " where CODE=1 and STCD='" + STCD + "'");
                    }
                }
                else
                {
                    Service.Model.YY_RTU_WRES model = new Service.Model.YY_RTU_WRES();
                    model.STCD = STCD;
                    model.CODE = 4;
                    int xd = 0;
                    if (int.TryParse(values[0], out xd))
                    {
                        if (xd == 0)
                        {
                            model.COM_B = xd;
                            explain = "                                                 中心4备用信道[禁用]\n";
                        }
                        else if (xd == 1)
                        {
                            model.COM_B = xd;
                            model.PhoneNum = values[1];
                            explain = "                                                 中心4备用信道[短信][" + values[1] + "]\n";
                        }
                        else if (xd == 2)
                        {
                            int port = 0;
                            if (int.TryParse(values[2].Trim(), out port))
                            {
                                model.COM_B = xd;
                                model.ADR_B = values[1];
                                model.PORT_B = port;
                                explain = "                                                 中心4备用信道[IPV4][" + values[1] + ":" + port + "]\n";
                            }
                        }
                        else if (xd == 3 || xd == 4)
                        {
                            model.COM_B = xd;
                            model.SatelliteNum = values[1];
                            explain = "                                                 中心4备用信道[卫星][" + values[1] + "]\n";
                        }
                        else
                        {
                            list.First().COM_B = xd;
                            if (xd == 5)
                            { explain = "                                                 中心4备用信道[PSTN]\n"; }
                            else if (xd == 6)
                            { explain = "                                                 中心4备用信道[超短波]\n"; }
                        }
                        Service.ServiceBussiness.WRESList.Add(model);
                        Service.PublicBD.db.AddRTU_WRES(model);
                    }

                }
            }
        }
        private static void _0CH(string Value, string STCD, out string explain)
        {
            explain = "";
            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "11000000000C" select l;
            if (list.Count() > 0)
            {
                list.First().ConfigVal = Value;
                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='11000000000C'");
                Service.PublicBD.db.AddRTU_ConfigData(list.First());
            }
            else
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                model.STCD = STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "11000000000C";
                model.ConfigVal = Value;
                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                Service.PublicBD.db.AddRTU_ConfigData(model);
            }
            if (Value == "01")
            { explain = "                                                 工作方式[自报工作状态]\n"; }
            else if (Value == "02")
            { explain = "                                                 工作方式[自报确认工作状态]\n"; }
            else if (Value == "03")
            { explain = "                                                 工作方式[查询/应答工作状态]\n"; }
            else if (Value == "04")
            { explain = "                                                 工作方式[调试或维修状态]\n"; }

        }
        private static void _0DH(string Value, string STCD, out string explain)
        {
            explain = "";
            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "11000000000D" select l;
            if (list.Count() > 0)
            {
                list.First().ConfigVal = Value;
                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='11000000000D'");
                Service.PublicBD.db.AddRTU_ConfigData(list.First());
            }
            else
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                model.STCD = STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "11000000000D";
                model.ConfigVal = Value;
                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                Service.PublicBD.db.AddRTU_ConfigData(model);
            }

            string[] item = new string[] { "降水量", "蒸发量", "风向", "风速", "气温", "湿度", "地温", "气压", 
                "水位8", "水位7", "水位6", "水位5", "水位4", "水位3", "水位2", "水位1",
                "地下水埋深", "图片", "波浪", "闸门开度", "水量", "流速", "流量", "水压",
                "水表8",	"水表7",	"水表6",	"水表5",	"水表4",	"水表3",	"水表2",	"水表1",
                "100CM墒情",	"80CM墒情",	"60CM墒情",	"50CM墒情",	"40CM墒情",	"30CM墒情",	"20CM墒情",	"10CM墒情" ,
                "pH值",	"溶解氧",	"电导率",	"浊度",	"氧化还原电位",	"高锰酸盐指数",	"氨氮",	"水温",
                "总有机碳",	"总氮",	"总磷",	"锌",	"硒",	"砷",	"总汞",	"镉",
                "D7",	"D6",	"D5",	"D4",	"D3","叶绿素a",	"铜",	"铅"};
            if (Value.Length == 64)
            {
                explain = "                                                 遥测站采集要素[";
                string temp = "";
                for (int i = 0; i < 64; i++)
                {
                    if (Value[i] == '1')
                    {
                        temp += item[i] + ",";
                    }
                }
                if (temp.Length > 0)
                {
                    temp = temp.TrimEnd(new char[] { ',' });
                }
                else { temp = "未设置"; }
                explain += temp + "]\n";
            }

        }
        private static void _0EH(string Value, string STCD, out string explain)
        {
            explain = "";
            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "11000000000E" select l;
            if (list.Count() > 0)
            {
                list.First().ConfigVal = Value;
                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='11000000000E'");
                Service.PublicBD.db.AddRTU_ConfigData(list.First());
            }
            else
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                model.STCD = STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "11000000000E";
                model.ConfigVal = Value;
                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                Service.PublicBD.db.AddRTU_ConfigData(model);
            }
            explain = "                                                 站服务地址范围[" + Value + "]\n";
        }
        private static void _0FH(string Value, string STCD, out string explain)
        {
            explain = "";
            string[] temps = Value.Split(new char[] { ',' });
            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "11000000000F" select l;
            if (list.Count() > 0)
            {
                list.First().ConfigVal = Value;
                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='11000000000F'");
                Service.PublicBD.db.AddRTU_ConfigData(list.First());
            }
            else
            {
                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                model.STCD = STCD;
                model.ItemID = "0000000000";
                model.ConfigID = "11000000000F";
                model.ConfigVal = Value;
                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                Service.PublicBD.db.AddRTU_ConfigData(model);
            }
            if (temps[1].Length > 0)
            {
                int temp = 0;
                if (int.TryParse(temps[0], out temp))
                {
                    if (temp == 0)
                    {
                        explain = "                                                 遥测站手机通信设备识别号[" + temps[1] + "]\n";
                    }
                    else
                    {
                        explain = "                                                 遥测站卫星通信设备识别号[" + temps[1] + "]\n";
                    }
                }
            }
            else
            { explain = "                                                 遥测站通信设备识别号[无]\n"; }

        }
        #endregion

        internal static void Process_0x42(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x42 package = new Deal0x42(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);//  package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    string Explain = "遥测站运行参数配置信息(设置)\n";
                    List<Service.Model.YY_RTU_ITEMCONFIG> ItemConfig = Service.ServiceBussiness.ITEMCONFIGList;
                    List<Service.Model.YY_RTU_CONFIGITEM> ConfigItem = Service.ServiceBussiness.CONFIGITEMList;
                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        ushort key = package.packData.Values[i].SymbolHex;
                        string Key = key.ToString("X");
                        string val = package.packData.Values[i].ValueToString();

                        if (key >= 0x20 && key <= 0x26)
                        {

                            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "1100000000" + Key select l;
                            if (list.Count() > 0)
                            {
                                list.First().ConfigVal = val;
                                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='1100000000" + Key + "'");
                                Service.PublicBD.db.AddRTU_ConfigData(list.First());
                            }
                            else
                            {
                                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                                model.STCD = STCD;
                                model.ItemID = "0000000000";
                                model.ConfigID = "1100000000" + Key;
                                model.ConfigVal = val;
                                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                                Service.PublicBD.db.AddRTU_ConfigData(model);
                            }
                            var ci = from c in ConfigItem where c.ConfigID == "1100000000" + Key select c;
                            if (ci.Count() > 0)
                            {
                                Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "]\n";
                            }
                        }
                        else if (key >= 0x27 && key <= 0xA8)
                        {
                            string ItemID = "";
                            var list1 = from l in ItemConfig where l.ConfigID == "1111000000" + Key select l;
                            if (list1.Count() > 0)
                            {
                                ItemID = list1.First().ItemID;

                                var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "1111000000" + Key && l.ItemID == ItemID select l;
                                if (list.Count() > 0)
                                {
                                    list.First().ConfigVal = val;
                                    Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='1111000000" + Key + "' and ItemID='" + ItemID + "'");
                                    Service.PublicBD.db.AddRTU_ConfigData(list.First());
                                }
                                else
                                {
                                    Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                                    model.STCD = STCD;
                                    model.ItemID = ItemID;
                                    model.ConfigID = "1111000000" + Key;
                                    model.ConfigVal = val;
                                    Service.ServiceBussiness.CONFIGDATAList.Add(model);
                                    Service.PublicBD.db.AddRTU_ConfigData(model);
                                }
                                var ci = from c in ConfigItem where c.ConfigID == "1111000000" + Key select c;
                                if (ci.Count() > 0)
                                {
                                    Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "]\n";
                                }
                            }
                            else
                            {
                                var ci = from c in ConfigItem where c.ConfigID == "1111000000" + Key select c;
                                if (ci.Count() > 0)
                                {
                                    Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "] 监测项与配置项未设置,不能入库！\n";
                                }
                            }
                        }
                    }

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "42操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x43(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x43 package = new Deal0x43(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    string Explain = "遥测站运行参数配置信息(查询)\n";
                    List<Service.Model.YY_RTU_ITEMCONFIG> ItemConfig = Service.ServiceBussiness.ITEMCONFIGList;
                    List<Service.Model.YY_RTU_CONFIGITEM> ConfigItem = Service.ServiceBussiness.CONFIGITEMList;
                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        ushort key = package.packData.Values[i].SymbolHex;
                        string val = package.packData.Values[i].ValueToString();
                        if (key >= 0x20 && key <= 0x26)
                        {
                            var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "1100000000" + key.ToString("X") select l;
                            if (list.Count() > 0)
                            {
                                list.First().ConfigVal = val;
                                Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='1100000000" + key.ToString("X") + "'");
                                Service.PublicBD.db.AddRTU_ConfigData(list.First());
                            }
                            else
                            {
                                Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                                model.STCD = STCD;
                                model.ItemID = "0000000000";
                                model.ConfigID = "1100000000" + key.ToString("X");
                                model.ConfigVal = val;
                                Service.ServiceBussiness.CONFIGDATAList.Add(model);
                                Service.PublicBD.db.AddRTU_ConfigData(model);
                            }
                            var ci = from c in ConfigItem where c.ConfigID == "1100000000" + key.ToString("X") select c;
                            if (ci.Count() > 0)
                            {
                                Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "]\n";
                            }
                        }
                        else if (key >= 0x27 && key <= 0xA8)
                        {
                            string ItemID = "";
                            var list1 = from l in ItemConfig where l.ConfigID == "1111000000" + key.ToString("X") select l;
                            if (list1.Count() > 0)
                            {
                                ItemID = list1.First().ItemID;

                                var list = from l in Service.ServiceBussiness.CONFIGDATAList where l.STCD == STCD && l.ConfigID == "1111000000" + key.ToString("X") && l.ItemID == ItemID select l;
                                if (list.Count() > 0)
                                {
                                    list.First().ConfigVal = val;
                                    Service.PublicBD.db.DelRTU_ConfigData(" where STCD='" + STCD + "' and ConfigID='1111000000" + key.ToString("X") + "' and ItemID='" + ItemID + "'");
                                    Service.PublicBD.db.AddRTU_ConfigData(list.First());
                                }
                                else
                                {
                                    Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                                    model.STCD = STCD;
                                    model.ItemID = ItemID;
                                    model.ConfigID = "1111000000" + key.ToString("X");
                                    model.ConfigVal = val;
                                    Service.ServiceBussiness.CONFIGDATAList.Add(model);
                                    Service.PublicBD.db.AddRTU_ConfigData(model);
                                }
                                var ci = from c in ConfigItem where c.ConfigID == "1111000000" + key.ToString("X") select c;
                                if (ci.Count() > 0)
                                {
                                    Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "]\n";
                                }
                            }
                            else
                            {
                                var ci = from c in ConfigItem where c.ConfigID == "1111000000" + key.ToString("X") select c;
                                if (ci.Count() > 0)
                                {
                                    Explain += "                                                 " + ci.First().ConfigItem + "[" + val + "] 监测项与配置项未设置,不能入库！\n";
                                }
                            }
                        }

                    }


                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "43操作异常" + ex.ToString());
            }
        }

        //水泵电机实时工作数据(查询)
        internal static void Process_0x44(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x44 package = new Deal0x44(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_DATA_AUTO model = new Service.Model.YY_DATA_AUTO();
                        model.STCD = STCD;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.ItemID = package.packData.Values[i].Style.CustomNum;//监测项
                        model.DOWNDATE = DOWNDATE;
                        model.DATAVALUE = decimal.Parse(package.packData.Values[i].ToString()); //值
                        model.CorrectionVALUE = decimal.Parse(package.packData.Values[i].ToString());
                        //存入数据库，2015.8.25添加，请检查数据库YY_DATA_AUTO表是否建立STTYPE字段
                        model.STTYPE = package.packData.StationClassificationCodes.ToString("X2");
                        model.NFOINDEX = (int)NFOINDEX;
                        //32H	遥测站定时报
                        //33H	遥测站加报报
                        //34H	遥测站小时报
                        int datatype = 0;
                        if (int.TryParse(package.FunCode.ToString(), out datatype))
                        {
                            model.DATATYPE = datatype;  //数据类型，仅水文协议使用
                        }



                        Service.PublicBD.db.AddRealTimeData(model);

                        string ItemName = GetItemName(package.packData.Values[i].Style.CustomNum);
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到水泵电机实时工作数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到水泵电机实时工作数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到水泵电机实时工作数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到水泵电机实时工作数据(查询)，数据特征[" + ItemName + "-" + package.packData.Values[i].Style.CustomNum + "]，时间[" + package.packData.ObservationTime + "],值[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        if (package.packData.Values.Count == i)
                        {
                            Service.ServiceBussiness.WriteQUIM(null, null, STCD, "packover", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "44操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x45(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;



                    Deal0x45 package = new Deal0x45(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "接收到软件版本信息[" + package.packData.Values[0].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "接收到软件版本信息[" + package.packData.Values[0].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "接收到软件版本信息[" + package.packData.Values[0].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "接收到软件版本信息[" + package.packData.Values[0].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "45操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x46(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x46 package = new Deal0x46(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA02'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA02";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.DelRTU_ConfigData(Where);
                        Service.PublicBD.db.AddRTU_ConfigData(model);

                        #region 明文
                        string Explain = "测站状态和报警(查询)";
                        if (ConfigVal.Substring(0, 1) == "0")
                        {
                            Explain += "\n                                                 交流电充电状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 交流电充电状态[停电]"; }

                        if (ConfigVal.Substring(1, 1) == "0")
                        {
                            Explain += "\n                                                 蓄电池电压状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 蓄电池电压状态[电压低]"; }

                        if (ConfigVal.Substring(2, 1) == "0")
                        {
                            Explain += "\n                                                 水位超限报警状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 水位超限报警状态[报警]"; }

                        if (ConfigVal.Substring(3, 1) == "0")
                        {
                            Explain += "\n                                                 流量超限报警状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 流量超限报警状态[报警]"; }

                        if (ConfigVal.Substring(4, 1) == "0")
                        {
                            Explain += "\n                                                 水质超限报警状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 水质超限报警状态[报警]"; }

                        if (ConfigVal.Substring(5, 1) == "0")
                        {
                            Explain += "\n                                                 流量仪表状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 流量仪表状态[故障]"; }

                        if (ConfigVal.Substring(6, 1) == "0")
                        {
                            Explain += "\n                                                 水位仪表状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 水位仪表状态[故障]"; }

                        if (ConfigVal.Substring(7, 1) == "0")
                        {
                            Explain += "\n                                                 终端箱门状态[开启]";
                        }
                        else
                        { Explain += "\n                                                 终端箱门状态[关闭]"; }

                        if (ConfigVal.Substring(8, 1) == "0")
                        {
                            Explain += "\n                                                 存储器状态[正常]";
                        }
                        else
                        { Explain += "\n                                                 存储器状态[异常]"; }

                        if (ConfigVal.Substring(9, 1) == "0")
                        {
                            Explain += "\n                                                 IC卡功能有效[关闭]";
                        }
                        else
                        { Explain += "\n                                                 IC卡功能有效[有效]"; }

                        if (ConfigVal.Substring(10, 1) == "0")
                        {
                            Explain += "\n                                                 水泵工作状态[工作]";
                        }
                        else
                        { Explain += "\n                                                 水泵工作状态[停机]"; }

                        if (ConfigVal.Substring(11, 1) == "0")
                        {
                            Explain += "\n                                                 剩余水量报警[未超限]";
                        }
                        else
                        { Explain += "\n                                                 剩余水量报警[超限]"; }
                        #endregion

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "46操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x47(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;



                    Deal0x47 package = new Deal0x47(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    //for (int i = 0; i < package.packData.Values.Count; i++)
                    //{
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "初始化固态存储数据[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "初始化固态存储数据[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "初始化固态存储数据[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "初始化固态存储数据[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "47操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x48(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;



                    Deal0x48 package = new Deal0x48(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    //for (int i = 0; i < package.packData.Values.Count; i++)
                    //{
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "恢复终端出厂设置[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "恢复终端出厂设置[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "恢复终端出厂设置[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "恢复终端出厂设置[成功]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "48操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x49(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x49 package = new Deal0x49(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        var rtu = from r in Service.ServiceBussiness.RtuList where r.STCD == STCD select r;
                        if (rtu.Count() > 0)
                        {
                            rtu.First().PassWord = package.packData.Values[i].ToString();
                            Service.PublicBD.db.UpdRTU(rtu.First(), " where STCD='" + STCD + "'");
                        }


                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "密码已修改,新密码[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var list = from r in TS.Ts where r.STCD == STCD select r;
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
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "密码已修改,新密码[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var list = from r in US.Us where r.STCD == STCD select r;
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
                            Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "密码已修改,新密码[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                        #region com通知界面
                        if ((int)NFOINDEX == 4)
                        {
                            ComService.ComServer CS = Server as ComService.ComServer;
                            Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "密码已修改,新密码[" + package.packData.Values[i].ToString() + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "49操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4A(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4A package = new Deal0x4A(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }
                    DateTime RTUruntime = package.packData.DeliveryTime;
                    //for (int i = 0; i < package.packData.Values.Count; i++)
                    //{
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "终端时间(设置)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "终端时间(设置)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "终端时间(设置)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "终端时间(设置)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4A操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4B(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4B package = new Deal0x4B(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA02'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA02";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.DelRTU_ConfigData(Where);
                        Service.PublicBD.db.AddRTU_ConfigData(model);

                        #region 明文
                        string Explain = "测站状态和报警(设置)，";
                        //if (ConfigVal.Substring(0, 1) == "0")
                        //{
                        //    Explain += "\n交流电充电状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n交流电充电状态[停电]"; }

                        //if (ConfigVal.Substring(1, 1) == "0")
                        //{
                        //    Explain += "\n蓄电池电压状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n蓄电池电压状态[电压低]"; }

                        //if (ConfigVal.Substring(2, 1) == "0")
                        //{
                        //    Explain += "\n水位超限报警状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n水位超限报警状态[报警]"; }

                        //if (ConfigVal.Substring(3, 1) == "0")
                        //{
                        //    Explain += "\n流量超限报警状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n流量超限报警状态[报警]"; }

                        //if (ConfigVal.Substring(4, 1) == "0")
                        //{
                        //    Explain += "\n水质超限报警状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n水质超限报警状态[报警]"; }

                        //if (ConfigVal.Substring(5, 1) == "0")
                        //{
                        //    Explain += "\n流量仪表状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n流量仪表状态[故障]"; }

                        //if (ConfigVal.Substring(6, 1) == "0")
                        //{
                        //    Explain += "\n水位仪表状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n水位仪表状态[故障]"; }

                        //if (ConfigVal.Substring(7, 1) == "0")
                        //{
                        //    Explain += "\n终端箱门状态[开启]";
                        //}
                        //else
                        //{ Explain += "\n终端箱门状态[关闭]"; }

                        //if (ConfigVal.Substring(8, 1) == "0")
                        //{
                        //    Explain += "\n存储器状态[正常]";
                        //}
                        //else
                        //{ Explain += "\n存储器状态[异常]"; }

                        if (ConfigVal.Substring(9, 1) == "0")
                        {
                            Explain += "IC卡功能有效[关闭]";
                        }
                        else
                        { Explain += "IC卡功能有效[有效]"; }

                        //if (ConfigVal.Substring(10, 1) == "0")
                        //{
                        //    Explain += "\n水泵工作状态[工作]";
                        //}
                        //else
                        //{ Explain += "\n水泵工作状态[停机]"; }

                        //if (ConfigVal.Substring(11, 1) == "0")
                        //{
                        //    Explain += "\n剩余水量报警[未超限]";
                        //}
                        //else
                        //{ Explain += "\n剩余水量报警[超限]"; }
                        #endregion

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4B操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4C(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4C package = new Deal0x4C(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA04'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                            Service.PublicBD.db.DelRTU_ConfigData(Where);
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA04";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.AddRTU_ConfigData(model);


                        string Explain = "水泵状态、命令信息(设置),[";
                        for (int j = 1; j < ConfigVal.Length + 1; j++)
                        {
                            if (ConfigVal.Substring(j - 1, 1) == "1")
                            {
                                Explain += j + ":开  ";
                            }
                            else
                            {
                                Explain += j + ":关  ";
                            }
                        }
                        Explain = Explain.Trim();
                        Explain += "]";

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4C操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4D(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4D package = new Deal0x4D(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);//package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA05'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                            Service.PublicBD.db.DelRTU_ConfigData(Where);
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA05";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.AddRTU_ConfigData(model);


                        string Explain = "阀门状态、命令信息(设置),[";
                        for (int j = 1; j < ConfigVal.Length + 1; j++)
                        {
                            if (ConfigVal.Substring(j - 1, 1) == "1")
                            {
                                Explain += j + ":开  ";
                            }
                            else
                            {
                                Explain += j + ":关  ";
                            }
                        }
                        Explain = Explain.Trim();
                        Explain += "]";

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4D操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4E(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4D package = new Deal0x4D(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA06'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                            Service.PublicBD.db.DelRTU_ConfigData(Where);
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA06";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.AddRTU_ConfigData(model);


                        string Explain = "闸门状态、命令信息(设置),[";
                        for (int j = 1; j < ConfigVal.Length + 1; j++)
                        {
                            if (ConfigVal.Substring(j - 1, 1) == "1")
                            {
                                Explain += j + ":开  ";
                            }
                            else
                            {
                                Explain += j + ":关  ";
                            }
                        }
                        Explain = Explain.Trim();
                        Explain += "]";

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4E操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x4F(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x4F package = new Deal0x4F(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();
                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        Service.Model.YY_RTU_CONFIGDATA model = new Service.Model.YY_RTU_CONFIGDATA();
                        string Where = " where STCD ='" + STCD + "' and ConfigID like '1100____AA03'";
                        string ConfigVal = package.packData.Values[i].ToString();
                        List<Service.Model.YY_RTU_CONFIGDATA> list = Service.PublicBD.db.GetRTU_CONFIGDATAList(Where).ToList<Service.Model.YY_RTU_CONFIGDATA>();
                        if (list != null && list.Count() > 0)
                        {
                            list.First().ConfigVal = ConfigVal;
                            model = list.First();
                            Service.PublicBD.db.DelRTU_ConfigData(Where);
                        }
                        else
                        {
                            model.STCD = STCD;
                            model.ItemID = "0000000000";
                            model.ConfigID = "11000000AA03";
                            model.ConfigVal = ConfigVal;
                        }
                        Service.PublicBD.db.AddRTU_ConfigData(model);


                        string Explain = "水量定值控制(设置),";
                        if (ConfigVal == "0")
                        {
                            Explain += "状态[退出]";
                        }
                        else
                        { Explain += "状态[投入]"; }

                        #region tcp通知界面
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            //回复通知界面
                            Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                            var l = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }
                        }
                        #endregion
                        #region udp通知界面
                        if ((int)NFOINDEX == 2)
                        {
                            UdpService.UdpServer US = Server as UdpService.UdpServer;
                            Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            var l = from rtu in US.Us where rtu.STCD == STCD select rtu;
                            if (l.Count() > 0)
                            {
                                l.First().CanSend = false;
                            }

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
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "4F操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x50(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x50 package = new Deal0x50(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }
                    List<Service.Model.YY_LOG> yy_log = Service.PublicBD.db.GetLog().ToList<Service.Model.YY_LOG>();  //得到log列表，用于显示明文

                    string Explain = "遥测终端的事件记录(查询)\n";
                    List<Service.Model.YY_DATA_LOG> listlog = new List<Service.Model.YY_DATA_LOG>();
                    for (int i = 0; i < package.packData.Values.Count; i++)
                    {
                        if (i >= 18)
                        { break; }

                        Service.Model.YY_DATA_LOG model = new Service.Model.YY_DATA_LOG();
                        model.STCD = STCD;
                        model.NFOINDEX = (int)NFOINDEX;
                        model.TM = package.packData.ObservationTime; //监测时间
                        model.DOWNDATE = DOWNDATE;
                        model.ERC = i + 1;
                        model.LOGID = i + 1;
                        model.COUNT = int.Parse(package.packData.Values[i].ToString());
                        listlog.Add(model);

                        if (yy_log.Count > 0)
                        {
                            var log = from l in yy_log where l.LOGID == i + 1 select l;
                            if (log.Count() > 0)
                            {
                                Explain += "                                                 [次数：" + model.COUNT + "    描述：" + log.First().RTULOG + "]\n";
                            }
                        }
                    }

                    #region 入库
                    Service.PublicBD.db.DelDataLog(" where STCD='" + STCD + "'");
                    foreach (var item in listlog)
                    {
                        Service.PublicBD.db.AddDataLog(item);
                    }
                    #endregion

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "50操作异常" + ex.ToString());
            }
        }

        internal static void Process_0x51(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    DateTime DOWNDATE = DateTime.Now;

                    Deal0x51 package = new Deal0x51(inPackage);

                    Dictionary<int, Frame> Ask = GetAsk(STCD, package);// package.CretatAsk_Package().GetFrames();
                    if (Ask != null && Ask.Count > 0)
                    {
                        byte[] ask = Ask[1].ToBytes();

                        #region tcp回复
                        if ((int)NFOINDEX == 1)
                        {
                            TcpService.TcpServer TS = Server as TcpService.TcpServer;
                            List<TcpService.TcpSocket> Ts = TS.Ts;

                            var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                            List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                            //没有该测站信息，不能向下执行，上“生成回复数据”没成功。
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (Tcps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                Tcps.First().TCPSOCKET.Send(sendData);
                                //回复通知界面
                                Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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
                            if (ask == null)
                            {
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "生成回复报出现异常！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                                return;
                            }

                            if (udps.Count() > 0)
                            {
                                byte[] sendData = ask;
                                US.UDPClient.Send(sendData, sendData.Length, udps.First().IpEndPoint);
                                Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "回复数据", sendData, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }

                        }
                        #endregion
                    }

                    DateTime RTUruntime = package.packData.DeliveryTime;

                    //for (int i = 0; i < package.packData.Values.Count; i++)
                    //{
                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "终端时间(查询)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "终端时间(查询)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, STCD, "终端时间(查询)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    #region com通知界面
                    if ((int)NFOINDEX == 4)
                    {
                        ComService.ComServer CS = Server as ComService.ComServer;
                        Service.ServiceBussiness.WriteQUIM("COM", CS.ServiceID, STCD, "终端时间(查询)为[" + RTUruntime + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    #endregion
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "51操作异常" + ex.ToString());
            }
        }

        //图片发送前的握手
        internal static void Process_0x60(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try
            {
                
                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);

                if (inPackage.ValidataPackage())
                {
                    string STCD = inPackage.StationNumber.ToString();
                    Deal0x60 package = new Deal0x60(inPackage);

                    #region tcp通知界面
                    if ((int)NFOINDEX == 1)
                    {
                        TcpService.TcpServer TS = Server as TcpService.TcpServer;
                        //回复通知界面
                        Service.ServiceBussiness.WriteQUIM("TCP", TS.ServiceID, STCD, "RTU准备发送图片报,等待确认", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        var list = from rtu in TS.Ts where rtu.STCD == STCD select rtu;
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
                        Service.ServiceBussiness.WriteQUIM("UDP", US.ServiceID, STCD, "RTU准备发送图片报,等待确认", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        var list = from rtu in US.Us where rtu.STCD == STCD select rtu;
                        if (list.Count() > 0)
                        {
                            list.First().CanSend = false;
                        }

                    }
                    #endregion
                    ImageFramesCache.AddFrame(frame, (int)NFOINDEX, Server);

                    
                    string dataStr =DateTime.Now.ToString("yyyyMMddHHmmss")+" "+"REV:" +Service.EnCoder.ByteArrayToHexStr(frame.ToBytes());
                    Service._51Data.SystemError.SystemLog(STCD + "-" + DateTime.Now.ToString("yyyyMMdd"), dataStr);
                }
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "60操作异常" + ex.ToString());
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

        /// <summary>
        /// 根据指定测站是否要求保持在线，打回复报
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="package">回复报类对象</param>
        /// <returns>回复报</returns>
        private static Dictionary<int, Frame> GetAsk(string STCD, InPackage package)
        {
            var stcd = from t in Service.ServiceBussiness.CONFIGDATAList where t.STCD == STCD && t.ConfigID == "110000000000" && t.ItemID == "0000000000" select t;
            if (stcd.Count() > 0)
            {
                return package.CretatAsk_Package(true).GetFrames();
            }
            else
            {
                return package.CretatAsk_Package(false).GetFrames();
            }
        }

        /// <summary>
        /// 根据测站得到是否在线
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <returns></returns>
        private static bool GetOnLine(string STCD)
        {
            var stcd = from t in Service.ServiceBussiness.CONFIGDATAList where t.STCD == STCD && t.ConfigID == "110000000000" && t.ItemID == "0000000000" select t;
            if (stcd.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
