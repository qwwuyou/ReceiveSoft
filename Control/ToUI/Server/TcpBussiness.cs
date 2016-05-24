using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Concurrent;
using Service;

namespace ToUI
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
        public static void UpdSocket(TcpServer TS, Socket socket)
        {
            List<TcpSocket> Ts = TS.Ts;
            lock (Ts)
            {
                var temp = from t in Ts where t.TCPSOCKET == socket select t;
                int count = temp.Count<TcpSocket>();
                if (count == 0)
                { AddSocket(TS, socket); }
                else
                {
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
                //lock (Qtrd)
                //{
                    Qtrd.Enqueue(trd);
                //}
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="TS">tcp服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        public static void WriteTsdQ(TcpServer TS, string STCD, byte[] bt)
        {
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;

            TcpSendData tsd = new TcpSendData();
            tsd.Data = bt;
            tsd.STCD = STCD;
            lock (Qtsd)
            {
                Qtsd.Enqueue(tsd);
            }
        }
        #endregion


        /// <summary>
        /// 从回复队列中回复数据
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public static void SendData(TcpServer TS)
        {
            string ServiceId=TS.ServiceID;
            ConcurrentQueue<TcpSendData> Qtsd=TS.TQ.Qtsd ;
            List<TcpSocket> Ts = TS.Ts; 
            UIModel uim = new UIModel();
            lock (Ts)
            //lock (ServiceQueue.QUIM)
                while (ServiceQueue.QUIM.Count > 0)
                {
                    if (ServiceQueue.QUIM.Count >100)
                    {
                        for (int i = 0; i < ServiceQueue.QUIM.Count; i++)
                        {
                            ServiceQueue.QUIM.TryDequeue(out uim);
                        }
                        
                        
                        foreach (var item in Ts)
                        {
                            try
                            {
                                item.TCPSOCKET.Send(Encoding.UTF8.GetBytes("++"+DateTime.Now.ToString("MM-dd HH:mm:ss")+"缓冲区数据超100条，服务自动清空减压..."));
                                System.Threading.Thread.Sleep(1);
                            }
                            catch (Exception ex)
                            {
                                ServiceControl.log.Warn(DateTime.Now + ex.ToString()); 
                                //经常会出现socket客户端断开，此线程正在执行send方法
                            }
                        }
                        return;
                    }

                     uim = null;
                    ServiceQueue.QUIM.TryDequeue(out uim);
                    //uim = ServiceQueue.QUIM.Dequeue();
                    if (uim != null)
                    {
                        foreach (var item in Ts)
                        {
                            try
                            {
                                if (uim.DataType == 1)
                                {
                                    item.TCPSOCKET.Send(Encoding.UTF8.GetBytes(uim.EXPLAIN));
                                }
                                else if (uim.DataType == 2)
                                {
                                    item.TCPSOCKET.Send(Encoding.UTF8.GetBytes(uim.EXPLAIN));
                                }
                                System.Threading.Thread.Sleep(1);

                            }
                            catch (Exception ex)
                            {
                                ServiceControl.log.Warn(DateTime.Now + ex.ToString()); 
                                //经常会出现socket客户端断开，此线程正在执行send方法
                            }

                        }
                    }

                }

           
          
        }

        
    }
}
