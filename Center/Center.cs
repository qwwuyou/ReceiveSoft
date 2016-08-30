using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using TcpService;
using Service.Model;


namespace Service
{
    public class Center:DataProcess
    {
        ParseData pd = new ParseData();
        List<CENTER_SERVER> servers;
        List<CENTER_STARTSTATE> StartState;
        public Center()
        {
            servers = Service.PublicBD.db.GetServers().ToList();
            StartState = Service.PublicBD.db.GetStartStateServers().ToList();
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
          
            //dateDiff + "|" + RtuCount + "|" + project + "|" + RegistrationInfo + "|" + PublicIP +"|" +RunState;
            
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
                    CENTER_SERVER centerserver = pd.GetServer(trd.Data);
                    if (centerserver != null)
                    {
                        InsertNewSTCD(centerserver.ProjectName+centerserver.PublicIP, Service.ServiceEnum.NFOINDEX.TCP, TS);
                    
                        //判断servers中是否存在，如存在更新信息
                        pd.ExistsServer(servers, centerserver);
                        //判断centerserver.RunState是否为start，如是写入库表CENTER_STARTSTATE
                        pd.ExistsServerStartState(StartState, centerserver);
                        //判断centerserver.RTUCount是否变化，如是写入库表CENTER_RTUCHANGE

                    }

                    
                    //if (state == "H")
                    //{
                    //    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);
                    //    bool B = false;
                    //    //更新socket列表的stcd、socket
                    //    TcpBussiness.UpdSocket(TS, trd.SOCKET, STCD, out B);
                    //}
                    //else if (state == "C")
                    //{
                    //    var tcps = from t in Ts where t.STCD == STCD && t.TCPSOCKET != null select t;
                    //    List<TcpService.TcpSocket> Tcps = tcps.ToList<TcpService.TcpSocket>();
                    //    if (Tcps.Count() > 0)
                    //    {
                    //        foreach (var item in Tcps)
                    //        {
                    //            item.TCPSOCKET.Send(Encoding.ASCII.GetBytes(ask));
                    //        }
                    //    }
                    //}
                }
            }
            
            //throw new NotImplementedException();
        }


        public void InsertNewSTCD(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.TCP)
            {
                
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                List<TcpService.TcpSocket> Ts = TS.Ts;
                var tcps = from t in Ts where t.STCD == STCD   select t;
                if (tcps.Count() == 0)
                {
                    TcpSocket ts = new TcpSocket() { STCD = STCD };
                    Ts.Add(ts);
                }
                
            }
        }

       
        public void PacketArrived(GsmService.GsmServer GS)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            throw new NotImplementedException();
        }
    }

}
