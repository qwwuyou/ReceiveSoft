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
        //public static void AddSocket(UdpServer US, IPEndPoint IpEndPoint)
        //{
        //    List<UdpSocket> Us = US.Us;

        //    lock (Us)
        //    {
        //        var temp = from u in Us where u.IpEndPoint.Address.ToString()  == IpEndPoint.Address.ToString() && u.IpEndPoint.Port ==IpEndPoint.Port  select u;
        //        int count = temp.Count<UdpSocket>();
        //        if (count == 0)
        //        {
        //            //添加
        //            UdpSocket us = new UdpSocket();
        //            us.CONNECTTIME = DateTime.Now;
        //            us.IpEndPoint = IpEndPoint;
        //            Us.Add(us);
        //        }
        //    }
        //}

        /// <summary>
        /// 更新Udp对象
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="IpEndPoint">Udp标识</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="B">上线标识</param>
        //public static void UpdSocket(UdpServer US, IPEndPoint IpEndPoint, string STCD, out bool B)
        //{
        //    List<UdpSocket> Us = US.Us;

        //    B = false;
        //    lock (Us)
        //    {
        //        var temp = from u in Us where u.STCD == STCD select u;
        //        List<UdpSocket> TEMP = temp.ToList<UdpSocket>();
        //        int count = temp.Count<UdpSocket>();
        //        if (count == 0)
        //        {
        //            temp = from u in Us where u.IpEndPoint.Address.ToString() == IpEndPoint.Address.ToString() && u.IpEndPoint.Port ==IpEndPoint.Port select u;
        //            TEMP = temp.ToList<UdpSocket>();
        //            count = TEMP.Count<UdpSocket>();
        //            if (count == 0)
        //            { AddSocket(US, IpEndPoint); }
        //            else
        //            {
        //                if (STCD != null)
        //                {
        //                    TEMP.First().STCD = STCD;
        //                    B = true;
        //                }
        //                TEMP.First().DATATIME = DateTime.Now;
        //                TEMP.First().IpEndPoint = IpEndPoint;

                        
        //            }
        //        }
        //        else
        //        {
        //            //更新
        //            TEMP.First().DATATIME = DateTime.Now;
        //            TEMP.First().IpEndPoint = IpEndPoint;

        //            temp = from u in Us where u.STCD == null && u.IpEndPoint.Address.ToString() == IpEndPoint.Address.ToString() && u.IpEndPoint.Port == IpEndPoint.Port select u;
        //            TEMP = temp.ToList<UdpSocket>();
        //            count = TEMP.Count<UdpSocket>();
        //            if (count > 0)
        //                Us.Remove(TEMP.First());
        //        }
        //    }
        //}

        public static void UpdSocket(UdpServer US, IPEndPoint IpEndPoint, string STCD, out bool B) 
        {
            List<UdpSocket> Us = US.Us;
            B = false;
            var temp = from u in Us where u.STCD == STCD select u;
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
                temp.First().IpEndPoint = IpEndPoint;
                temp.First().DATATIME = DateTime.Now;
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
            var temp = from u in Us where u.IpEndPoint == IpEndPoint select u;
            if (temp.Count() > 0)
            {
                temp.First().IpEndPoint = null;
                temp.First().CONNECTTIME = null;
                temp.First().DATATIME = null;
            }
        }

        /// <summary>
        /// 从列表删除Udp对象
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="Second">秒</param>
        public static void DelClosSocket(UdpServer US, int Second)
        {
            string ServiceID=US.ServiceID;
            List<UdpSocket> Us = US.Us;
            var temp = from u in Us where u.CONNECTTIME != null select u;
            ArrayList al = new ArrayList();
            foreach (var item in temp)
            {
                DateTime dt1 = item.DATATIME.Value.AddSeconds(Second);
                DateTime dt2 = DateTime.Now;
                if (DateTime.Compare(dt1, dt2) < 0)
                {
                    al.Add(item);
                }
            }
            foreach (UdpSocket item in al)
            {
                UdpBussiness.UdpDisconnected(US, item.IpEndPoint);
                item.IpEndPoint = null;
                item.CONNECTTIME = null;
                item.DATATIME = null;
            }
            al = null;
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
            {
                //lock (Qurd)
                //{
                Qurd.Enqueue(urd);
                //}
            }
        }


        /// <summary>
        /// 将回复数据放入队列(每个测站同一命令只允许有1条，默认为最后一条)
        /// </summary>
        /// <param name="US">udp服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        /// <param name="CommandCode">命令码</param>
        public static void WriteUsdQ(UdpServer US, string STCD, byte[] bt,string CommandCode)
        {
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;

            var qusd = from u in Qusd where u.STCD == STCD && u.COMMANDCODE == CommandCode select u;
            List<UdpSendData> QUSD = qusd.ToList<UdpSendData>();
            lock (Qusd)
                if (QUSD.Count() > 0)
            {
                QUSD.First().Data = bt;
                QUSD.First().STATE = 0;
            }
            else
            {
                UdpSendData usd = new UdpSendData();
                usd.Data = bt;
                usd.STCD = STCD;
                usd.COMMANDCODE = CommandCode;
                usd.STATE = 0;
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
            lock (US.UQ.Qusd)
            {
                for (int i = 0; i < US.UQ.Qusd.Count; i++)
                {
                    UdpSendData usd = null;
                    if (US.UQ.Qusd.TryDequeue(out usd))
                    {
                        if (usd.STCD != STCD || usd.COMMANDCODE != CommandCode)
                        {
                            US.UQ.Qusd.Enqueue(usd);
                        }
                        
                    }
                }
               
            }
        }

        /// <summary>
        /// 清空召测命令集中的命令
        /// </summary>
        /// <param name="US">udp服务</param>
        public static void ClearUsdQ(UdpServer US)
        {
            lock (US.UQ.Qusd)
            {
                US.UQ.Qusd = new ConcurrentQueue<UdpSendData>();
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
            List<UdpSocket> TEMP = temp.ToList<UdpSocket>();
            if (TEMP.Count() > 0)
            {
                if (TEMP.First().STCD != null && TEMP.First().STCD != "")
                {
                    ServiceBussiness.WriteQUIM("UDP", ServiceId, TEMP.First().STCD, "下线！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    ServiceBussiness.WriteQUIM("", "", "", "stcd|" + TEMP.First().STCD + ":udp:", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
                }
                else
                {
                    ServiceBussiness.WriteQUIM("UDP", ServiceId, IpEndPoint.Address.ToString() + ":" + IpEndPoint.Port, "断开连接！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
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

            ServiceBussiness.WriteQUIM("UDP", ServiceId, IpEndPoint.Address.ToString() + ":" + IpEndPoint.Port, "上线!", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            
        }

        /// <summary>
        /// 上线通知(数据解析时得到站号时调用)
        /// </summary>
        /// <param name="US"></param>
        /// <param name="STCD"></param>
        public static void UdpConnected(UdpServer US, string STCD)
        {
            string ServiceId = US.ServiceID;
            ServiceBussiness.WriteQUIM("UDP", ServiceId, STCD, "上线！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            ServiceBussiness.WriteQUIM("", "", "","stcd|"+STCD +":udp" , new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
                        
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
                        var temp = from u in Us where us.STCD == u.STCD && u.IpEndPoint !=null select u;
                        List<UdpSocket> TEMP = temp.ToList<UdpSocket>();
                        if (TEMP.Count() > 0)
                        {
                            udpclient = new UdpClient(US.PORT);  //()绑定指定端口
                            udpclient.Connect(TEMP.First().IpEndPoint);
                            udpclient.Send(us.Data, us.Data.Length);
                            udpclient.Close();
                            //ServiceBussiness.WriteQSM(ServiceId, temp.First().STCD, "回复数据", us.Data);
                        }
                        else
                        {
                            Qusd.Enqueue(us);
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
            Reflection_Protoco.SendCommand(US);
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="US">udp服务</param>
        public static void ResolvePacket(UdpServer US)
        {
            Reflection_Protoco.PacketArrived(US);
        }

        /// <summary>
        /// 信道启动时将命令读取到命令列表
        /// </summary>
        public static void ToQusd()
        {
            //命令本地列表中删除超时记录
            var cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.UDP.ToString() && c.STATE == -1 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            List<Command> CMDS = cmds.ToList<Command>();
            foreach (var items in CMDS)
            {
                var lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.UDP, items.CommandID, -1);
                List<Command> LC = lc.ToList<Command>();
                foreach (var item in LC)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.UDP, -1);
                }
               
            }

            cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.UDP.ToString() && c.STATE == -2 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            CMDS = cmds.ToList<Command>();
            foreach (var items in CMDS)
            {
                List<Command> lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.UDP, items.CommandID, -2);
                List<Command> LC = lc.ToList<Command>();
                foreach (var item in LC)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.UDP, -2);
                }
                
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "UDP" select cmd;
            CMDS = cmds.ToList<Command>();
            foreach (var u in ServiceControl.udp)
            {
                foreach (var item in CMDS)
                {
                    if(u!=null)
                    UdpService.UdpBussiness.WriteUsdQ(u, item.STCD, EnCoder.HexStrToByteArray(item.Data), item.CommandID);
                }
            }
        }

        
    }
}
