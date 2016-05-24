using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Service;

namespace UdpService
{
    public class UdpBussiness
    {
        #region [控制在线列表的方法]
        /// <summary>
        /// 添加Udp对象
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        public static void AddSocket(UdpServer US, IPEndPoint IpEndPoint)
        {
            List<UdpSocket> Us = US.Us;

            lock (Us)
            {
                var temp = from u in Us where u.IpEndPoint.Address.ToString()  == IpEndPoint.Address.ToString() && u.IpEndPoint.Port ==IpEndPoint.Port  select u;
                int count = temp.Count<UdpSocket>();
                if (count == 0)
                {
                    //添加
                    UdpSocket us = new UdpSocket();
                    us.CONNECTTIME = DateTime.Now;
                    us.IpEndPoint = IpEndPoint;
                    Us.Add(us);
                }
            }
        }

        /// <summary>
        /// 更新Udp对象
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="B">上线标识</param>
        public static void UpdSocket(UdpServer US, IPEndPoint IpEndPoint, string STCD, out bool B)
        {
            List<UdpSocket> Us = US.Us;

            B = false;
            lock (Us)
            {
                var temp = from u in Us where u.STCD == STCD select u;
                int count = temp.Count<UdpSocket>();
                if (count == 0)
                {
                    temp = from u in Us where u.IpEndPoint.Address.ToString() == IpEndPoint.Address.ToString() && u.IpEndPoint.Port ==IpEndPoint.Port select u;
                    count = temp.Count<UdpSocket>();
                    if (count == 0)
                    { AddSocket(US, IpEndPoint); }
                    else
                    {
                        if (STCD != null)
                        {
                            temp.First().STCD = STCD;
                            B = true;
                        }
                        temp.First().DATATIME = DateTime.Now;
                        temp.First().IpEndPoint = IpEndPoint;
                    }
                }
                else
                {
                    //更新
                    temp.First().DATATIME = DateTime.Now;
                    temp.First().IpEndPoint = IpEndPoint;
                }
            }
        }

        /// <summary>
        /// 删除列表中超时连接
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        public static void DelSocket(UdpServer US, IPEndPoint IpEndPoint)
        {
            List<UdpSocket> Us = US.Us;

            lock (Us)
            {
                var temp = from u in Us where u.IpEndPoint == IpEndPoint select u;

                foreach (var item in temp)
                {
                    Us.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 从列表删除Udp对象
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="Minute">分钟</param>
        public static void DelClosSocket(UdpServer US, int Minute)
        {
            string ServiceID=US.ServiceID;
            List<UdpSocket> Us = US.Us;

            lock (Us)
            {
                ArrayList al = new ArrayList();
                foreach (UdpSocket item in Us)
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

                foreach (UdpSocket item in al)
                {

                    UdpBussiness.UdpDisconnected(US, item.IpEndPoint);

                    Us.Remove(item);
                }
                al = null;
            }


        }
        #endregion


        #region 收到或回复数据放入队列

        /// <summary>
        /// 将收到数据放入数据队列
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        /// <param name="bt">数据</param>
        public static void WriteUrdQ(UdpServer US, IPEndPoint IpEndPoint, byte[] bt)
        {
            ConcurrentQueue<UdpReceivedData> Qurd = US.UQ.Qurd;
            UdpReceivedData urd = new UdpReceivedData();
            urd.IpEndPoint = IpEndPoint;
            urd.Data = bt;
            if (bt.Length > 0)
                lock (Qurd)
                {
                    Qurd.Enqueue(urd);
                }
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        /// <param name="CommandCode">命令码</param>
        public static void WriteUsdQ(UdpServer US, string STCD, byte[] bt,string CommandCode)
        {
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;

            UdpSendData usd = new UdpSendData();
            usd.Data = bt;
            usd.STCD = STCD;
            usd.COMMANDCODE = CommandCode;
            usd.STATE = 0;
            lock (Qusd)
            {
                Qusd.Enqueue(usd);
            }
        }

        /// <summary>
        /// 移除召测命令集中的命令
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="STCD">站号</param>
        /// <param name="CommandCode">命令码</param>
        public static void RemoveUsdQ(UdpServer US, string STCD, string CommandCode)
        {
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;
            lock (Qusd)
            {
                UdpSendData usd = null;
                if (Qusd.TryDequeue(out usd))
                {
                    if (usd.STCD != STCD || usd.COMMANDCODE != CommandCode)
                    {
                        Qusd.Enqueue(usd);
                    }
                }
            }
        }
        #endregion

        #region udp上下线通知界面
        /// <summary>
        /// 掉线通知
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        public static void UdpDisconnected(UdpServer US, IPEndPoint IpEndPoint)
        {
            string ServiceId = US.ServiceID;
            List<UdpSocket> Us = US.Us;

            var temp = from u in Us where u.IpEndPoint == IpEndPoint select u;
            if (temp.Count() > 0)
            {
                if (temp.First().STCD != null && temp.First().STCD != "")
                {
                    ServiceBussiness.WriteQUIM("UDP", ServiceId, temp.First().STCD, "下线！", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
                }
                else
                {
                    ServiceBussiness.WriteQUIM("UDP", ServiceId, IpEndPoint.Address.ToString() + ":" + IpEndPoint.Port, "下线！", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
                }
            }
        }

        /// <summary>
        /// 上线通知
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        public static void UdpConnected(UdpServer US, IPEndPoint IpEndPoint)
        {
            string ServiceId = US.ServiceID;

            ServiceBussiness.WriteQUIM("UDP", ServiceId, IpEndPoint.Address.ToString() + ":" + IpEndPoint.Port, "上线!", new byte[] { }, ServiceBussiness.EnCoderType.HEX, ServiceBussiness.DataType.Text);
        }
        #endregion

        /// <summary>
        /// 从回复队列中回复数据(未使用)
        /// </summary>
        /// <param name="US">udp服务</param>
        public static void SendData(UdpServer US)
        {
            string ServiceId = US.ServiceID;
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;
            List<UdpSocket> Us = US.Us;

            UdpClient udpclient;
            lock (Qusd)
            {
                int k = Qusd.Count;
                while (Qusd.Count > 0)
                {
                    UdpSendData us = null;
                    Qusd.TryDequeue(out us);
                    if (us != null)
                    {
                        lock (Us)
                        {
                            var temp = from u in Us where us.STCD == u.STCD select u;
                            if (temp.Count() > 0)
                            {
                                udpclient = new UdpClient(US.PORT);  //()绑定指定端口
                                udpclient.Connect(temp.First().IpEndPoint);
                                udpclient.Send(us.Data, us.Data.Length);
                                udpclient.Close();
                                //ServiceBussiness.WriteQSM(ServiceId, temp.First().STCD, "回复数据", us.Data);
                            }
                            else
                            {
                                Qusd.Enqueue(us);
                            }

                        }
                        k--;
                        if (k <= 0)
                        {
                            return;
                        }
                    }

                }
            }

        }

        /// <summary>
        /// 发送命令方法含试发三次的业务逻辑
        /// </summary>
        /// <param name="US">udp服务</param>
        public static void SendCommand(UdpServer US)
        {
            Reflection.SendCommand(US);
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="US">udp服务</param>
        public static void ResolvePacket(UdpServer US)
        {   
            Reflection.PacketArrived(US);
        }


    }
}
