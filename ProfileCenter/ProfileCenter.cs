using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpService;
using Service.Model;
using System.Collections.Concurrent;

namespace Service
{
    public class ProfileCenter : DataProcess
    {

        public void SendCommand(UdpService.UdpServer US)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(TcpServer TS)
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

        public void PacketArrived(TcpServer TS)
        {
            string serviceID = TS.ServiceID;
            ConcurrentQueue<TcpReceivedData> qtrd = TS.TQ.Qtrd;
            List<TcpSocket> ts = TS.Ts;
            ConcurrentQueue<TcpSendData> qtsd = TS.TQ.Qtsd;
            while (qtrd.Count > 0)
            {
                TcpReceivedData tcpReceivedData = null;
                qtrd.TryDequeue(out tcpReceivedData);
                if (tcpReceivedData != null)
                {
                    string a = "";
                    string s = "";
                    string STCD = GetSTCD(tcpReceivedData.Data, out a, out s);
                    if (a == "H")
                    {
                        InsertNewSTCD(STCD, ServiceEnum.NFOINDEX.TCP, TS);
                        bool flag = false;
                        TcpBussiness.UpdSocket(TS, tcpReceivedData.SOCKET, STCD, out flag);
                    }
                    else
                    {
                        if (a == "C")
                        {
                            IEnumerable<TcpSocket> source =
                                from t in ts
                                where t.STCD == STCD && t.TCPSOCKET != null
                                select t;
                            List<TcpSocket> list = source.ToList<TcpSocket>();
                            if (list.Count<TcpSocket>() > 0)
                            {
                                foreach (TcpSocket current in list)
                                {
                                    current.TCPSOCKET.Send(Encoding.ASCII.GetBytes(s));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void InsertNewSTCD(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.TCP)
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
        }
        private string GetSTCD(byte[] data, out string state, out string ask)
        {
            string result = "";
            state = "";
            ask = "";
            string text = Encoding.ASCII.GetString(data).Replace("\r", "").Replace("\n", "");

            //Console.WriteLine(text);
            string[] array = text.Split(new char[] { '|' });
            if (array.Length > 1 && array[0] == "H")
            {
                result = array[1];
                state = "H";
            }
            else
            {
                if (array.Length > 2 && array[0] == "C")
                {
                    result = array[1];
                    state = "C";
                    ask = array[2];
                }
            }
            return result;
        }

        public void PacketArrived(GsmService.GsmServer GS)
        {
            //throw new NotImplementedException();
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            //throw new NotImplementedException();
        }
    }
}
