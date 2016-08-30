using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Concurrent;
using Service;

namespace TcpService
{
    public class TcpBussiness
    {

        #region [控制在线列表的方法]
        /// <summary>
        /// 更新socket对象
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">socket对象</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="B">上线标识</param>
        public static void UpdSocket(TcpServer TS, Socket socket, string STCD, out bool B)
        {
            List<TcpSocket> Ts = TS.Ts;
            B = false;
            var temp = from t in Ts where t.STCD == STCD select t;
            if (temp.Count() > 0)
            {
                if (temp.First().CONNECTTIME != null)
                {
                    B = true;
                }
                else
                {
                    temp.First().CONNECTTIME = DateTime.Now;
                }
                temp.First().TCPSOCKET = socket;
                temp.First().DATATIME = DateTime.Now;
            }
           
        }

        /// <summary>
        /// 删除列表中掉线连接
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">socket对象</param>
        public static void DelSocket(TcpServer TS, Socket socket)
        {
            List<TcpSocket> Ts = TS.Ts;
            var temp = from t in Ts where t.TCPSOCKET == socket select t;
            if (temp.Count() > 0)
            {
                temp.First().CONNECTTIME = null;
                temp.First().DATATIME = null;
                temp.First().TCPSOCKET = null;
            }
        }

        /// <summary>
        /// 从列表删除并关闭socket
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="Second">秒</param>
        public static void DelClosSocket(TcpServer TS, int Second)
        {
            List<TcpSocket> Ts = TS.Ts;
            var temp = from t in Ts where t.CONNECTTIME != null select t;
            ArrayList al = new ArrayList();
            foreach (TcpSocket item in temp)
            {
                DateTime dt1 = item.DATATIME.Value.AddSeconds(Second);
                DateTime dt2 = DateTime.Now;
                if (DateTime.Compare(dt1, dt2) < 0)
                {
                    al.Add(item);
                }
            }

            foreach (TcpSocket item in al)
            {
                try
                {
                    //if (item.STCD != null || item.STCD  != "")
                    //{
                    TcpDisconnected(TS, item.TCPSOCKET);
                    //}
                    //item.TCPSOCKET.Close();
                    //item.TCPSOCKET.Dispose();
                    item.TCPSOCKET = null;
                    item.CONNECTTIME = null;
                    item.DATATIME = null;
                }
                catch(Exception ex)
                {
                    ServiceControl.log.Error(DateTime.Now + ex.ToString());
                }
            }
            al = null;
        }
        #endregion

        #region 收到或回复数据放入队列
        /// <summary>
        /// 将收到数据放入数据队列
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="bt">数据</param>
        public static void WriteTrdQ(TcpServer TS, Socket socket, byte[] bt)
        {
            ConcurrentQueue<TcpReceivedData> Qtrd = TS.TQ.Qtrd;

            TcpReceivedData trd = new TcpReceivedData();
            trd.SOCKET = socket;
            trd.Data = bt;
            if (bt.Length > 0)
            {
                //lock (Qtrd)
                //{
                Qtrd.Enqueue(trd);
                //}
            }
        }


        /// <summary>
        /// 将回复数据放入队列(每个测站同一命令只允许有1条，默认为最后1条)
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        /// <param name="CommandcCode">命令码</param>
        public static void WriteTsdQ(TcpServer TS, string STCD, byte[] bt, string CommandCode)
        {
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;
            var qtsd = from t in Qtsd where t.STCD == STCD && t.COMMANDCODE == CommandCode select t;
            lock (Qtsd)
                if (qtsd.Count() > 0)
                {
                    qtsd.First().Data = bt;
                    qtsd.First().STATE = 0;
                }
                else
                {
                    TcpSendData tsd = new TcpSendData();
                    tsd.Data = bt;
                    tsd.STCD = STCD;
                    tsd.COMMANDCODE = CommandCode;
                    tsd.STATE = 0;
                    Qtsd.Enqueue(tsd);
                }
        }

        /// <summary>
        /// 移除召测命令集中的命令
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="STCD">站号</param>
        /// <param name="CommandCode">命令号</param>
        public static void RemoveTsdQ(TcpServer TS, string STCD, string CommandCode)
        {
            lock (TS.TQ.Qtsd)
            {
                for (int i = 0; i < TS.TQ.Qtsd.Count; i++)
                {
                    TcpSendData tsd = null;
                    if (TS.TQ.Qtsd.TryDequeue(out tsd))
                    {
                        if (tsd.STCD != STCD || tsd.COMMANDCODE != CommandCode)
                        {
                            TS.TQ.Qtsd.Enqueue(tsd);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空召测命令集中的命令
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void ClearTsdQ(TcpServer TS)
        {
            lock (TS.TQ.Qtsd)
            {
                TS.TQ.Qtsd = new ConcurrentQueue<TcpSendData>();
            }
        }
        #endregion

        #region tcp上下线通知界面
        /// <summary>
        /// 掉线通知
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">掉线的socket</param>
        public static void TcpDisconnected(TcpServer TS, Socket socket)
        {
            string ServiceId = TS.ServiceID;
            List<TcpSocket> Ts = TS.Ts;
            var temp = from t in Ts where t.TCPSOCKET == socket select t;
            List<TcpSocket> Temp = temp.ToList<TcpSocket>();
            if (Temp.Count() > 0)
            {
                if (Temp.First().CONNECTTIME != null)
                {
                    ServiceBussiness.WriteQUIM("TCP", ServiceId, Temp.First().STCD, "下线！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    ServiceBussiness.WriteQUIM("", "", "", "stcd|" + Temp.First().STCD + ":tcp:", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
                }
                else
                {
                    if (Temp.First().TCPSOCKET.Connected)
                        ServiceBussiness.WriteQUIM("TCP", ServiceId, (Temp.First().TCPSOCKET.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString() + ":" + (Temp.First().TCPSOCKET.RemoteEndPoint as System.Net.IPEndPoint).Port, "断开连接！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
            }
        }

        /// <summary>
        /// 上线通知
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">上线的socket</param>
        public static void TcpConnected(TcpServer TS, Socket socket)
        {
            string ServiceId = TS.ServiceID;
            ServiceBussiness.WriteQUIM("TCP", ServiceId, (socket.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString() + ":" + (socket.RemoteEndPoint as System.Net.IPEndPoint).Port, "建立连接!", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
        }
        /// <summary>
        /// 上线通知(数据解析时得到站号时调用)
        /// </summary>
        /// <param name="US">tcp服务</param>
        /// <param name="STCD">站号</param>
        public static void TcpConnected(TcpServer TS, string STCD)
        {
            string ServiceId = TS.ServiceID;
            ServiceBussiness.WriteQUIM("TCP", ServiceId, STCD, "上线！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            ServiceBussiness.WriteQUIM("", "", "", "stcd|" + STCD + ":tcp", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
        }
        #endregion
        /// <summary>
        /// 从回复队列中回复数据(未使用)
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void SendData(TcpServer TS)
        {
            string ServiceId = TS.ServiceID;
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;
            List<TcpSocket> Ts = TS.Ts;

            int k = Qtsd.Count;
            lock (Qtsd)
                while (Qtsd.Count > 0)
                {
                    TcpSendData ts = null;
                    Qtsd.TryDequeue(out ts);
                    if (ts != null)
                    {
                        var temp = from t in Ts where ts.STCD == t.STCD && t.TCPSOCKET!=null select t;
                        if (temp.Count() > 0)
                        {
                            temp.First().TCPSOCKET.Send(ts.Data);
                        }
                        else
                        {
                            Qtsd.Enqueue(ts);
                        }

                        k--;
                        if (k <= 0)
                        {
                            return;
                        }
                    }
                }


        }

        /// <summary>
        /// 发送命令方法含试发三次的业务逻辑
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void SendCommand(TcpServer TS)
        {
            Reflection_Protoco.SendCommand(TS);
            //Service.WaterResource waterresource = new Service.WaterResource();
            //Service.DataProcess dataprocess = waterresource;
            //dataprocess.SendCommand(TS);
        }


        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void ResolvePacket(TcpServer TS)
        {
            Reflection_Protoco.PacketArrived(TS);

            //引用protocol后使用以下代码跟踪测试解包功能
            //Service.WaterResource waterresource = new Service.WaterResource();
            //Service.DataProcess dataprocess = waterresource;
            //dataprocess.PacketArrived(TS);
        }

        /// <summary>
        /// 信道启动时将命令读取到命令列表
        /// </summary>
        public static void ToQtsd()
        {
            //命令本地列表中删除超时记录
            var cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.TCP.ToString() && c.STATE == -1 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            foreach (var items in cmds)
            {
                List<Command> lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.TCP, items.CommandID, -1);
                foreach (var item in lc)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.TCP, -1);
                }
            }

            cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.TCP.ToString() && c.STATE == -2 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            foreach (var items in cmds)
            {
                List<Command> lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.TCP, items.CommandID, -2);
                foreach (var item in lc)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.TCP, -2);
                }
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "TCP" select cmd;
            foreach (var t in ServiceControl.tcp)
            {
                foreach (var item in cmds)
                {
                    if (t != null)
                    {
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.wrx.XMLObj.HEXOrASC == "HEX") 
                        {
                            EncoderData = EnCoder.HexStrToByteArray(item.Data);
                        }
                        if (ServiceControl.wrx.XMLObj.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(item.Data);
                        }
                        #endregion

                        TcpService.TcpBussiness.WriteTsdQ(t, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
        }
    }
}
