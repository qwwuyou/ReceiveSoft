using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Service;

namespace GsmService
{
    public class GsmBussiness
    {
        #region [控制在线列表的方法]
        /// <summary>
        /// 添加GsmMobile对象
        /// </summary>
        /// <param name="GS">GSM服务</param>
        /// <param name="Mobile">测站手机号</param>
        /// <param name="STCD">测站编号</param>
        private static void AddMobile(GsmServer GS , string Mobile, string STCD)
        {
            List<GsmMobile> Gs = GS.Gs;

            lock (Gs)
            {
                var temp = from g in Gs where g.MOBILE == Mobile select g;
                int count = temp.Count<GsmMobile>();
                if (count == 0)
                {
                    //添加
                    GsmMobile gs = new GsmMobile();
                    gs.DATATIME = DateTime.Now;
                    gs.MOBILE = Mobile;
                    Gs.Add(gs);
                }
            }
        }

        /// <summary>
        /// 更新GsmMobile对象
        /// </summary>
        /// <param name="GS">GSM服务</param>
        /// <param name="Mobile">测站手机号</param>
        /// <param name="STCD">测站编号</param>
        public static void UpdMobile(GsmServer GS, string Mobile, string STCD)
        {
            List<GsmMobile> Gs = GS.Gs;

            lock (Gs)
            {
                var temp = from g in Gs where g.STCD == STCD select g;
                int count = temp.Count<GsmMobile>();
                if (count == 0)
                {
                    temp = from g in Gs where g.MOBILE == Mobile select g;
                    count = temp.Count<GsmMobile>();
                    if (count == 0)
                    { AddMobile(GS, Mobile, STCD); }
                    else
                    {
                        temp.First().STCD = STCD;
                        temp.First().DATATIME = DateTime.Now;
                        temp.First().MOBILE = Mobile;
                    }
                }
                else
                {
                    //更新
                    temp.First().DATATIME = DateTime.Now;
                    temp.First().MOBILE = Mobile;
                }
            }
        }
        #endregion


        /// <summary>
        /// 从数据库读取出手机号列表
        /// </summary>
        /// <returns></returns>
        public static List<GsmMobile> GetGsmMobileList() 
        {
           return ServiceBussiness.GetGsmMobileList();
        }
        
        #region 收到或回复数据放入队列
        /// <summary>
        /// 将收到数据放入数据队列
        /// </summary>
        /// <param name="GS">GSM服务</param>
        /// <param name="senddatetime">发送时间</param>
        /// <param name="bt">数据</param>
        public static void WriteGrdQ(GsmServer GS, string mobile, DateTime senddatetime, byte[] bt)
        {
            ConcurrentQueue<GsmReceivedData> Qgrd = GS.GQ.Qgrd;

            GsmReceivedData grd = new GsmReceivedData();
            grd.MOBILE = mobile;
            grd.SENDDATETIME = senddatetime;
            grd.Data = bt;
            if (bt.Length > 0)
                lock (Qgrd)
                {
                    Qgrd.Enqueue(grd);
                }
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="GS">GSM服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        public static void WriteGsdQ(GsmServer GS, string STCD, byte[] bt)
        {
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;

            GsmSendData gsd = new GsmSendData();
            gsd.Data = bt;
            gsd.STCD = STCD;
            lock (Qgsd)
            {
                Qgsd.Enqueue(gsd);
            }
        }
        #endregion


        /// <summary>
        /// 从回复队列中回复数据
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void SendData(GsmServer GS)
        {

            //string ServiceId=GS.ServiceID;
            //ConcurrentQueue<GsmSendData> Qgsd=GS.GQ.Qgsd;
            //List<GsmMobile> Gs=GS.Gs;
            //GSMMODEM.GsmModem gm = GS.gm;

            //lock (Qgsd)
            //{
            //    int k = Qgsd.Count;
            //    while (Qgsd.Count > 0)
            //    {
            //        GsmSendData gs = null;
            //        Qgsd.TryDequeue(out gs);
            //        if (gs != null)
            //        {
            //            lock (Gs)
            //            {
            //                var temp = from g in Gs where gs.STCD == g.STCD select g;
            //                if (temp.Count() > 0)
            //                {
            //                    gm.SendMsg(temp.First().MOBILE,Encoding.Default.GetString(gs.Data));

            //                    ServiceBussiness.WriteQUIM("GSM", ServiceId, temp.First().STCD, "回复数据", gs.Data, ServiceBussiness.EnCoderType.HEX,ServiceBussiness.DataType.Text);
            //                }
            //                else
            //                {
            //                    Qgsd.Enqueue(gs);
            //                }

            //            }
            //            k--;
            //            if (k <= 0)
            //            {
            //                return;
            //            }
            //        }

            //    }
            //}

        }


        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void ResolvePacket(GsmServer GS)
        {
            Reflection.PacketArrived(GS);
        }

    }
}
