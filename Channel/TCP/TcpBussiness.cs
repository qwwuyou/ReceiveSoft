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
        /// 添加socket对象
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">socket对象</param>
        public static void AddSocket(TcpServer TS, Socket socket)
        {
            List<TcpSocket> Ts = TS.Ts;
            
            lock (Ts)
            {
                var temp = from t in Ts where t.TCPSOCKET == socket select t;
                int count = temp.Count<TcpSocket>();
                if (count == 0)
                {
                    //添加
                    TcpSocket ts = new TcpSocket();
                    ts.CONNECTTIME = DateTime.Now;
                    ts.TCPSOCKET = socket;
                    Ts.Add(ts);
                }
            }
        }

        /// <summary>
        /// 更新socket对象
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="socket">socket对象</param>
        /// <param name="STCD">测站编号</param>
        public static void UpdSocket(TcpServer TS, Socket socket, string STCD)
        {
            List<TcpSocket> Ts = TS.Ts;
            lock (Ts)
            {
                var temp = from t in Ts where t.STCD == STCD select t;
                int count = temp.Count<TcpSocket>();
                if (count == 0)
                {
                    temp = from t in Ts where t.TCPSOCKET == socket select t;
                    count = temp.Count<TcpSocket>();
                    if (count == 0)
                    { AddSocket(TS, socket); }
                    else
                    {
                        temp.First().STCD = STCD;
                        temp.First().DATATIME = DateTime.Now;
                        temp.First().TCPSOCKET = socket;
                    }
                }
                else
                {
                    //更新
                    temp.First().DATATIME = DateTime.Now;
                    temp.First().TCPSOCKET = socket;
                }
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

            lock (Ts)
            {
                var temp = from t in Ts where t.TCPSOCKET == socket select t;
                foreach (var item in temp)
                {
                    Ts.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 从列表删除并关闭socket
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="Minute">分钟</param>
        public static void DelClosSocket(TcpServer TS, int Minute)
        {
             List<TcpSocket> Ts=TS.Ts;

            ArrayList al = new ArrayList();
            lock (Ts)
            {
                foreach (TcpSocket item in Ts)
                {
                    if (item.DATATIME.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
                    {
                        DateTime dt1 = item.DATATIME.AddSeconds(Minute);
                        DateTime dt2 = DateTime.Now;
                        if (DateTime.Compare(dt1, dt2) < 0)
                        {
                            al.Add(item);
                        }
                    }
                    else
                    {
                        DateTime dt1 = item.CONNECTTIME.AddSeconds(Minute);
                        DateTime dt2 = DateTime.Now;
                        if (DateTime.Compare(dt1, dt2) < 0)
                        {
                            al.Add(item);
                        }
                    }
                }

                foreach (TcpSocket item in al)
                {
                    if (item.STCD == null || item.STCD  == "")
                    {
                         TcpDisconnected(TS, item.TCPSOCKET);
                    }
                    item.TCPSOCKET.Close();
                    item.TCPSOCKET.Dispose();

                    Ts.Remove(item);
                }
                al = null;
            }            
        }
        #endregion

        #region 收到或回复数据放入队列
        /// <summary>
        /// 将收到数据放入数据队列
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="bt">数据</param>
        public static void WriteTrdQ(TcpServer TS,Socket socket, byte[] bt)
        {
            ConcurrentQueue<TcpReceivedData> Qtrd = TS.TQ.Qtrd;

            TcpReceivedData trd = new TcpReceivedData();
            trd.SOCKET = socket;
            trd.Data = bt;
            if (bt.Length > 0)
                lock (Qtrd)
                {
                    Qtrd.Enqueue(trd);
                }
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        /// <param name="CommandcCode">命令码</param>
        public static void WriteTsdQ(TcpServer TS, string STCD, byte[] bt,string CommandCode)
        {
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;

            TcpSendData tsd = new TcpSendData();
            tsd.Data = bt;
            tsd.STCD = STCD;
            tsd.COMMANDCODE = CommandCode;
            tsd.STATE = 0;
            lock (Qtsd)
            {
                Qtsd.Enqueue(tsd);
            }
        }

        /// <summary>
        /// 移除召测命令集中的命令
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="STCD">站号</param>
        /// <param name="CommandCode">命令号</param>
        public static void RemoveTsdQ(TcpServer TS,string STCD, string CommandCode) 
        {
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;
            lock (Qtsd) 
            {
                TcpSendData tsd = null;
                if (Qtsd.TryDequeue(out tsd)) 
                {
                    if (tsd.STCD != STCD || tsd.COMMANDCODE != CommandCode)
                    {
                        Qtsd.Enqueue(tsd);
                    }
                }
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
            if (temp.Count() > 0)
            {
                if (temp.First().STCD != null && temp.First().STCD != "")
                {
                    ServiceBussiness.WriteQUIM("TCP", ServiceId, temp.First().STCD, "下线！", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
                }
                else
                {
                    if (temp.First().TCPSOCKET.Connected)
                        ServiceBussiness.WriteQUIM("TCP", ServiceId, (temp.First().TCPSOCKET.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString() + ":" + (temp.First().TCPSOCKET.RemoteEndPoint as System.Net.IPEndPoint).Port, "下线！", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
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
            ServiceBussiness.WriteQUIM("TCP", ServiceId, (socket.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString() + ":" + (socket.RemoteEndPoint as System.Net.IPEndPoint).Port, "上线!", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
        }
        #endregion
        /// <summary>
        /// 从回复队列中回复数据(未使用)
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void SendData(TcpServer TS)
        {
            string ServiceId=TS.ServiceID;
            ConcurrentQueue<TcpSendData> Qtsd=TS.TQ.Qtsd ;
            List<TcpSocket> Ts = TS.Ts;

            int k = Qtsd.Count;
            lock (Qtsd)
                while (Qtsd.Count > 0)
                {
                    TcpSendData ts = null;
                    Qtsd.TryDequeue(out ts);
                    if (ts != null)
                    {
                        var temp = from t in Ts where ts.STCD == t.STCD select t;
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
            Reflection.SendCommand(TS);
        }


        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void ResolvePacket(TcpServer TS)
        {
            Reflection.PacketArrived(TS);

            //引用protocol后使用以下代码跟踪测试解包功能
            //Service.WaterResource waterresource = new Service.WaterResource();
            //Service.DataProcess dataprocess = waterresource;
            //dataprocess.PacketArrived(TS);
        }

    }
}
