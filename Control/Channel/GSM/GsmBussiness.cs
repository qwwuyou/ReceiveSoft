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
                List<GsmMobile> TEMP = temp.ToList<GsmMobile>();
                int count = TEMP.Count<GsmMobile>();
                if (count == 0)
                {
                    temp = from g in Gs where g.MOBILE == Mobile select g;
                    TEMP = temp.ToList<GsmMobile>();
                    count = TEMP.Count<GsmMobile>();
                    if (count == 0)//Add
                    { AddMobile(GS, Mobile, STCD); }
                    else//Upd
                    {
                        TEMP.First().STCD = STCD;
                        TEMP.First().DATATIME = DateTime.Now;
                        TEMP.First().MOBILE = Mobile;
                    }
                }
                else
                {
                    //更新
                    TEMP.First().DATATIME = DateTime.Now;
                    TEMP.First().MOBILE = Mobile;
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
            if (mobile.Substring(0, 2) == "86" && mobile.Length >= 13)
            {
                grd.MOBILE = mobile.Substring(2, mobile.Length-2);
            }
            else 
            {
                grd.MOBILE = mobile;
            }
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
        /// <param name="CommandCode">命令码</param>
        public static void WriteGsdQ(GsmServer GS, string STCD, byte[] bt, string CommandCode)
        {
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;

            var qgsd = from g in Qgsd where g.STCD == STCD && g.COMMANDCODE == CommandCode select g;
            List<GsmSendData> QGSD = qgsd.ToList<GsmSendData>();
            lock (Qgsd)
                if (QGSD.Count() > 0)
                {
                    QGSD.First().Data = bt;
                }
                else
                {
                    GsmSendData gsd = new GsmSendData();
                    gsd.Data = bt;
                    gsd.STCD = STCD;
                    gsd.COMMANDCODE = CommandCode;
                    Qgsd.Enqueue(gsd);
                }

        }

        /// <summary>
        /// 移除召测命令集中的命令
        /// </summary>
        /// <param name="GS">gsm服务</param>
        /// <param name="STCD">站号</param>
        /// <param name="CommandCode">命令码</param>
        public static void RemoveGsdQ(GsmServer GS, string STCD, string CommandCode)
        {
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;
            lock (Qgsd)
            {
                for (int i = 0; i < GS.GQ.Qgsd.Count ; i++)
                {
                    GsmSendData gsd = null;
                    if (Qgsd.TryDequeue(out gsd))
                    {
                        if (gsd.STCD != STCD || gsd.COMMANDCODE != CommandCode)
                        {
                            Qgsd.Enqueue(gsd);
                        }
                    }
                }
               
            }
        }

        /// <summary>
        /// 清空召测命令集中的命令
        /// </summary>
        /// <param name="GS">gsm服务</param>
        public static void ClearGsdQ(GsmServer GS)
        {
            lock (GS.GQ.Qgsd)
            {
                GS.GQ.Qgsd = new ConcurrentQueue<GsmSendData>();
            }
        }
        #endregion


        /// <summary>
        /// 从回复队列中回复数据(未使用)
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void SendData(GsmServer GS)
        {

            string ServiceId = GS.ServiceID;
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;
            List<GsmMobile> Gs = GS.Gs;
            GSMMODEM.GsmModem gm = GS.gm;

            lock (Qgsd)
            {
                int k = Qgsd.Count;
                while (Qgsd.Count > 0)
                {
                    GsmSendData gs = null;
                    Qgsd.TryDequeue(out gs);
                    if (gs != null)
                    {
                        lock (Gs)
                        {
                            var temp = from g in Gs where gs.STCD == g.STCD select g;
                            List<GsmMobile> TEMP = temp.ToList<GsmMobile>();
                            if (TEMP.Count() > 0)
                            {
                                try
                                {
                                    gm.SendMsg(TEMP.First().MOBILE, gs.Data);
                                }
                                catch (Exception ex)
                                {
                                    ServiceControl.log.Error(DateTime.Now + ex.ToString());
                                }
                                //ServiceBussiness.WriteQUIM("GSM", ServiceId, temp.First().STCD, "回复数据", gs.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                            else
                            {
                                Qgsd.Enqueue(gs);
                            }

                        }
                        k--;
                        if (k <= 0)
                        {
                            return;
                        }
                    }

                    System.Threading.Thread.Sleep(800);
                }
            }

        }


        /// <summary>
        /// 发送命令方法，短信仅发一次，业务逻辑
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void SendCommand(GsmServer GS)
        {
            Reflection_Protoco.SendCommand(GS);
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void ResolvePacket(GsmServer GS)
        {
            Reflection_Protoco.PacketArrived(GS);
        }


        /// <summary>
        /// 信道启动时将命令读取到命令列表
        /// </summary>
        public static void ToQgsd()
        {
            //命令本地列表中删除超时记录
            var cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.GSM.ToString() && c.STATE == -1 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            
            foreach (var items in cmds)
            {
                List<Command> lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.GSM, items.CommandID, -1);
                foreach (var item in lc)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.GSM, -1);
                }
            }

            cmds = from c in ServiceControl.LC where c.SERVICETYPE == ServiceEnum.NFOINDEX.GSM.ToString() && c.STATE == -2 && c.DATETIME < DateTime.Now.AddSeconds(-30) select c;
            foreach (var items in cmds)
            {
                List<Command> lc = ServiceBussiness.RemoveListCommand(items.STCD, Service.ServiceEnum.NFOINDEX.GSM, items.CommandID, -2);
                foreach (var item in lc)
                {
                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, DateTime.Now, item.Data, (int)Service.ServiceEnum.NFOINDEX.GSM, -2);
                }
                
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "GSM" select cmd;
            foreach (var g in ServiceControl.gsm)
            {
                foreach (var item in cmds)
                {
                    if (g != null)
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
                        GsmService.GsmBussiness.WriteGsdQ(g, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
        }
    }
}
