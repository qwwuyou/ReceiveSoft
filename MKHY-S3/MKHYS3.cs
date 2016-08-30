using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;
using System.Collections.Concurrent;
using UdpService;
using TcpService;
using GsmService;


namespace MKHY_S3
{
    //北京水文总站--美科华仪S3设备的协议解析类
    public class MKHYS3 : DataProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        //数据处理类
        static ParseData pd = new ParseData();


        /// <summary>
        /// 包路由器方法(从数采仪收)
        /// </summary>
        /// <param name="pack"></param>
        public void PacketArrived(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                string PacketType = pd.CommandCode(data);
                switch (PacketType)
                {
                    case "FD":
                        PackageProcess.Process_FD(data, NFOINDEX, Server);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
            }
        }



        public void SendCommand(UdpServer US)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(TcpServer TS)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(GsmServer GS)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(ComService.ComServer CS)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(UdpServer US)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(TcpServer TS)
        {
            string ServiceId = TS.ServiceID;
            ConcurrentQueue<TcpReceivedData> Qtrd = TS.TQ.Qtrd;
            List<TcpSocket> Ts = TS.Ts;
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;


            while (Qtrd.Count > 0)
            {
                TcpReceivedData trd = null;
                Qtrd.TryDequeue(out trd);
                if (trd != null)
                {
                    try
                    {
                        //注册&透传
                        Service.ServiceBussiness.RemoteCommand(trd.Data);
                        //Service.ServiceBussiness.Registered30(urd.Data);

                        //ASCII To String 
                        string data = EnCoder.ByteArrayToHexStr(trd.Data); //Encoding.ASCII.GetString(trd.Data);
                        if (pd.GetDataState(data) == "01") 
                        {
                            string STCD = pd.GetCode(data);
                            InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);
                            bool B = false;
                            //更新socket列表的stcd、socket
                            TcpBussiness.UpdSocket(TS, trd.SOCKET, STCD, out B);
                            if (!B)
                            {
                                //上线
                                TcpBussiness.TcpConnected(TS, STCD);
                            }
                            //通知界面
                            ServiceBussiness.WriteQUIM("TCP", ServiceId, STCD, "接收数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

                            PacketArrived(data, ServiceEnum.NFOINDEX.TCP, TS);
                        }
                        else
                        {
                            ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        //通知界面
                        ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }

                }
            }
        }

        public void PacketArrived(GsmServer GS)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Server">服务</param>
        public void InsertNewSTCD(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            if (STCD.Length == 0)
                return;
            var rtu = from r in Service.ServiceBussiness.RtuList where r.STCD == STCD select r;
            if (rtu.Count() == 0)
            {
                Service.Model.YY_RTU_Basic model = new Service.Model.YY_RTU_Basic();
                model.STCD = STCD;
                model.PassWord = "123456";
                model.NiceName = STCD;
                bool b = PublicBD.db.AddRTU(model);     //添加 
                if (b)
                    Service.ServiceBussiness.RtuList.Add(new Service.Model.YY_RTU_Basic() { STCD = STCD, NiceName = STCD, PassWord = "123456" });

            }
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.UDP)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == STCD select u;
                if (udps.Count() == 0)
                {
                    UdpSocket us = new UdpSocket() { STCD = STCD };
                    Us.Add(us);
                }
            }
            else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.TCP)
            {

                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;
                var tcps = from t in Ts where t.STCD == STCD select t;
                if (tcps.Count() == 0)
                {
                    TcpSocket ts = new TcpSocket() { STCD = STCD };
                    Ts.Add(ts);
                }
            }
            else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.GSM)
            {
                if (STCD != null || STCD != "")
                {
                    GsmService.GsmServer GS = Server as GsmService.GsmServer;
                    List<GsmService.GsmMobile> Gs = GS.Gs;
                    var gsms = from g in Gs where g.STCD == STCD select g;
                    if (gsms.Count() == 0)
                    {
                        GsmMobile gs = new GsmMobile() { STCD = STCD };
                        Gs.Add(gs);
                    }
                }
            }
            //else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.COM)
            //{
            //    ComService.ComServer CS = Server as ComService.ComServer;
            //    List<ComService.ComSatellite> Cs = CS.Cs;
            //    var coms = from g in Cs where g.STCD == STCD select g;
            //    if (coms.Count() == 0)
            //    {

            //    }
            //}
        }
    }
}
