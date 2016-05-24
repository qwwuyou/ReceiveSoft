using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO.Ports;
using Service;

namespace ComService
{
    public class ComBussiness
    {
        #region [控制在线列表的方法]
        /// <summary>
        /// 添加ComSatellite对象
        /// </summary>
        /// <param name="CS">COM服务</param>
        /// <param name="Mobile">卫星标识</param>
        /// <param name="STCD">测站编号</param>
        private static void AddMobile(ComServer CS, string satellite, string STCD)
        {
            List<ComSatellite> Cs = CS.Cs;

            lock (Cs)
            {
                var temp = from c in Cs where c.SATELLITE == satellite select c;
                int count = temp.Count<ComSatellite>();
                if (count == 0)
                {
                    //添加
                    ComSatellite cs = new ComSatellite();
                    cs.DATATIME = DateTime.Now;
                    cs.SATELLITE = satellite;
                    Cs.Add(cs);
                }
            }
        }

        /// <summary>
        /// 更新ComSatellite对象
        /// </summary>
        /// <param name="CS">COM服务</param>
        /// <param name="Mobile">卫星标识</param>
        /// <param name="STCD">测站编号</param>
        public static void UpdMobile(ComServer CS, string satellite, string STCD)
        {
            List<ComSatellite> Cs = CS.Cs;

            lock (Cs)
            {
                var temp = from c in Cs where c.STCD == STCD select c;
                int count = temp.Count<ComSatellite>();
                if (count == 0)
                {
                    temp = from c in Cs where c.SATELLITE == satellite select c;
                    count = temp.Count<ComSatellite>();
                    if (count == 0)
                    { AddMobile(CS, satellite, STCD); }
                    else
                    {
                        temp.First().STCD = STCD;
                        temp.First().DATATIME = DateTime.Now;
                        temp.First().SATELLITE= satellite;
                    }
                }
                else
                {
                    //更新
                    temp.First().DATATIME = DateTime.Now;
                    temp.First().SATELLITE = satellite;

                }

            }
        }
        #endregion

        /// <summary>
        /// 从数据库读取出卫星号列表
        /// </summary>
        /// <returns></returns>
        public static List<ComSatellite> GetComSatelliteList()
        {
            return ServiceBussiness.GetComSatelliteList();
        }

        #region 收到或回复数据放入队列
        /// <summary>
        /// 将收到数据放入数据队列
        /// </summary>
        /// <param name="CS">COM服务</param>
        /// <param name="senddatetime">发送时间</param>
        /// <param name="bt">数据</param>
        public static void WriteCrdQ(ComServer CS, byte[] bt)
        {
            ConcurrentQueue<ComReceivedData> Qcrd = CS.CQ.Qcrd;
            ComReceivedData crd = new ComReceivedData();
            crd.Data = bt;
            if (bt.Length > 0)
                lock (Qcrd)
                {
                    Qcrd.Enqueue(crd);
                }
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="CS">COM服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        public static void WriteCsdQ(ComServer CS, string STCD, byte[] bt)
        {
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;

            ComSendData csd = new ComSendData();
            csd.Data = bt;
            csd.STCD = STCD;
            lock (Qcsd)
            {
                Qcsd.Enqueue(csd);
            }
        }
        #endregion

        /// <summary>
        /// 从回复队列中回复数据
        /// </summary>
        /// <param name="CS">COM服务</param>
        public static void SendData(ComServer CS)
        {
            string ServiceId = CS.ServiceID;
             ConcurrentQueue<ComSendData> Qcsd=CS.CQ.Qcsd;
             List<ComSatellite> Cs = CS.Cs;
             SerialPort sp = CS.sp;
             lock (Qcsd)
            {
                int k = Qcsd.Count;
                while (Qcsd.Count > 0)
                {
                    ComSendData cs = null;
                    Qcsd.TryDequeue(out cs);
                    if (cs != null)
                    {
                        lock (Cs)
                        {
                            var temp = from c in Cs where cs.STCD == c.STCD select c;
                            if (temp.Count() > 0)
                            {
                                sp.WriteLine(Encoding.ASCII.GetString(cs.Data));

                                ServiceBussiness.WriteQUIM("COM", ServiceId, temp.First().STCD, "回复数据", cs.Data, ServiceBussiness.EnCoderType.HEX,ServiceBussiness.DataType.Text);
                            }
                            else
                            {
                                Qcsd.Enqueue(cs);
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
        /// 解析数据包
        /// </summary>
        /// <param name="CS">COM服务</param>
        public static void ResolvePacket(ComServer CS)
        {
            Reflection.PacketArrived(CS);
        }

    }
}
