using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;
using UdpService;
using TcpService;
using GsmService;

namespace JK
{
    //北京水文总站--基康RTU设备的协议解析类
    class JK: DataProcess
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
                string SensorType = pd.GetSubCode(data);
                switch (SensorType)
                {
                    case "39": //ADCP
                        PackageProcess.Process_39(data, NFOINDEX, Server);
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



        public void SendCommand(UdpService.UdpServer US)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(TcpService.TcpServer TS)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(GsmService.GsmServer GS)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(ComService.ComServer CS)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(UdpService.UdpServer US)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(TcpService.TcpServer TS)
        {
            
        }

        public void PacketArrived(GsmService.GsmServer GS)
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
