using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Service;
using TcpService;
using UdpService;
using GsmService;
using ComService;
using HJT212_2005;

namespace Service
{
    public class HTJ212 : DataProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        //数据处理类
        static ParseData pd = new ParseData();

        /// <summary>
        /// 包路由器方法(从数采仪收)
        /// </summary>
        /// <param name="pack"></param>
        public void PacketArrived(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                string CN = pd.GetCN(data);
                switch (CN)
                {
                    case "":
                        //PackageProcess.Process_9021(data, NFOINDEX, Server);
                        break;
                    case "2011":
                        PackageProcess.Process_2011(data, NFOINDEX, Server);
                        break;
                    case "2031":
                        PackageProcess.Process_2031(data, NFOINDEX, Server);
                        break;
                    case "2051":
                        PackageProcess.Process_2051(data, NFOINDEX, Server);
                        break;
                    case "2061":
                        PackageProcess.Process_2061(data, NFOINDEX, Server);
                        break;
                    default:
                        break;
                }
                //接收到命令的回复
                //CommandReply(frame, NFOINDEX, Server);

            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
            }
        }


        #region 发送数据
        public void SendCommand(UdpService.UdpServer US)
        {
            //lock (US.Us)
            foreach (var udp in US.Us)//在线
            {
                //查询命令集中  站号、功能码 符合条件的记录
                var command = from c in US.UQ.Qusd where c.STCD == udp.STCD select c;
                List<UdpSendData> Command = command.ToList<UdpSendData>();
                if (Command.Count() > 0)
                {
                    if (Command.First().STATE == 0) //0为从未发给rtu
                    {
                        System.Threading.Thread thread = null;
                        try
                        {
                            System.Threading.Thread.Sleep(800);
                            thread = new System.Threading.Thread(() => StartCommand(thread, US, Command.First(), Command.First().COMMANDCODE));
                            thread.Start();
                        }
                        catch
                        {
                            Service.ServiceControl.log.Error(DateTime.Now + "UDP信道召测线程启动异常！");
                        }
                    }
                }
            }
        }
        //udp发送命令逻辑执行
        void StartCommand(System.Threading.Thread thread, UdpService.UdpServer US, UdpService.UdpSendData usd, string COMMANDCODE)
        {
            try
            {
                #region
                int count = 2;
                while (count < 3)
                {
                    DateTime datetime = DateTime.Now;
                    //判断命令列表里面有没有命令
                    var command = from com in US.UQ.Qusd where com.STCD == usd.STCD && COMMANDCODE == com.COMMANDCODE select com;
                    List<UdpSendData> Command = command.ToList<UdpSendData>();
                    //判断有没有在线设备
                    var udp = from u in US.Us where u.STCD == usd.STCD && u.IpEndPoint != null select u;
                    List<UdpSocket> UDP = udp.ToList<UdpSocket>();

                    if (Command.Count() > 0 && UDP.Count() > 0) //是否有
                    {
                        string STCD = Command.First().STCD;
                        byte[] Data = Command.First().Data; //Data ASC

                        string data = Encoding.ASCII.GetString(Data);

                        Command.First().STATE = count + 1;
                        UDP.First().DATATIME = datetime;//更新为当前时间
                        US.UDPClient.Send(Command.First().Data, Command.First().Data.Length, UDP.First().IpEndPoint);


                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.UDP.ToString(), US.ServiceID, STCD, "发送召测命令", Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

                        //写入本地命令列表
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.UDP, COMMANDCODE, data, datetime, count + 1);

                        //单条命令状态发送到界面程序
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.UDP.ToString(), COMMANDCODE, data, datetime, count + 1);


                        System.Threading.Thread.Sleep(60 * 1000);//可设置
                        if (count >= 2)//三次后超时
                        {

                            datetime = DateTime.Now;
                            //再判断一次是否列表中还存在（有回复时，会从列表中将命令删除）
                            command = from com in US.UQ.Qusd where com.STCD == usd.STCD && usd.COMMANDCODE == com.COMMANDCODE select com;
                            Command = command.ToList<UdpSendData>();
                            if (Command.Count() > 0)
                            {
                                foreach (var item in ServiceControl.udp)
                                {
                                    UdpService.UdpBussiness.RemoveUsdQ(item, STCD, COMMANDCODE);
                                }

                                //写入命令列表  状态-1超时
                                ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.UDP, COMMANDCODE, data, datetime, -1);

                                //单条命令状态发送到界面程序
                                ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.UDP.ToString(), COMMANDCODE, data, datetime, -1);


                                //System.Threading.Thread.Sleep(30 * 1000);

                                //datetime = DateTime.Now;
                                //命令本地列表中删除超时记录
                                List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.UDP, COMMANDCODE, -1);
                                foreach (var item in lc)
                                {
                                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)Service.ServiceEnum.NFOINDEX.UDP, -1);
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    count++;

                }

                #endregion
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + "udp发送命令逻辑执行异常！", ex);
            }
        }


        public void SendCommand(TcpService.TcpServer TS)
        {
            //throw new NotImplementedException();
        }

        public void SendCommand(GsmService.GsmServer GS)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(ComService.ComServer CS)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 包处理
        public void PacketArrived(UdpService.UdpServer US)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(TcpService.TcpServer TS)
        {
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
                    //Frame[] frames = null;

                    try
                    {
                        //注册&透传
                        Service.ServiceBussiness.RemoteCommand(trd.Data);
                        //Service.ServiceBussiness.Registered30(urd.Data);

                        //ASCII To String 
                        string STR = Encoding.ASCII.GetString(trd.Data);
                        string[] STRs = STR.Split(new string[] { "##" }, StringSplitOptions.None);
                        if (STRs.Length > 0)
                        {
                            foreach (var item in STRs)
                            {
                                if (item != "")
                                {
                                    string STCD = pd.GetMN(item);
                                    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);
                                    bool B = false;
                                    //更新socket列表的stcd、socket
                                    TcpBussiness.UpdSocket(TS, trd.SOCKET, STCD, out B);
                                    if (!B)
                                    {
                                        //上线
                                        TcpBussiness.TcpConnected(TS, STCD);
                                    }
                                    //通知界面
                                    ServiceBussiness.WriteQUIM("TCP", ServiceId, STCD, "接收数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

                                    PacketArrived(item, ServiceEnum.NFOINDEX.TCP, TS);
                                }
                            }
                        }
                        else
                        {
                            ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        //通知界面
                        ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }

                }
            }
            //throw new NotImplementedException();
        }

        public void PacketArrived(GsmService.GsmServer GS)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Server">服务</param>
        public void InsertNewSTCD(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            if (STCD.Length == 0)
                return;
            var rtu = from r in Service.ServiceBussiness.RtuList where r.STCD == STCD select r;
            if (rtu.Count() == 0)
            {
                Service.Model.YY_RTU_Basic model = new Service.Model.YY_RTU_Basic();
                model.STCD = STCD;
                model.PassWord = "123456";
                model.NiceName = STCD;
                bool b = PublicBD.db.AddRTU(model);     //添加 
                if (b)
                    Service.ServiceBussiness.RtuList.Add(new Service.Model.YY_RTU_Basic() { STCD = STCD, NiceName = STCD, PassWord = "123456" });

            }
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.UDP)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                List<UdpService.UdpSocket> Us = US.Us;
                var udps = from u in Us where u.STCD == STCD select u;
                if (udps.Count() == 0)
                {
                    UdpSocket us = new UdpSocket() { STCD = STCD };
                    Us.Add(us);
                }
            }
            else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.TCP)
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
            else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.GSM)
            {
                if (STCD != null || STCD != "")
                {
                    GsmService.GsmServer GS = Server as GsmService.GsmServer;
                    List<GsmService.GsmMobile> Gs = GS.Gs;
                    var gsms = from g in Gs where g.STCD == STCD select g;
                    if (gsms.Count() == 0)
                    {
                        GsmMobile gs = new GsmMobile() { STCD = STCD };
                        Gs.Add(gs);
                    }
                }
            }
            //else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.COM)
            //{
            //    ComService.ComServer CS = Server as ComService.ComServer;
            //    List<ComService.ComSatellite> Cs = CS.Cs;
            //    var coms = from g in Cs where g.STCD == STCD select g;
            //    if (coms.Count() == 0)
            //    {

            //    }
            //}
        }

        /// <summary>
        /// 更新或写入测站手机号码（仅用于短信信道）
        /// </summary>
        /// <param name="GS"></param>
        /// <param name="Mobile">手机号</param>
        /// <param name="STCD">站号</param>
        public void InsertGsmMobile(GsmServer GS, string Mobile, string STCD)
        {
            var temp = from g in GS.Gs where g.MOBILE == Mobile && g.STCD == STCD select g;
            if (temp.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = STCD;
                Model.ItemID = "0000000000";
                Model.ConfigID = "10000000AA07";
                Model.ConfigVal = Mobile;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '10000000AA07'");
                Service.PublicBD.db.AddRTU_ConfigData(Model);
            }

        }

        /// <summary>
        /// 更新或写入测站卫星号码（仅用于卫星信道）
        /// </summary>
        /// <param name="CS"></param>
        /// <param name="Satellite">卫星号码</param>
        /// <param name="STCD">站号</param>
        public void InsertComMobile(ComServer CS, string Satellite, string STCD)
        {
            var temp = from c in CS.Cs where c.SATELLITE == Satellite && c.STCD == STCD select c;
            if (temp.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = STCD;
                Model.ItemID = "0000000000";
                Model.ConfigID = "11000000AA08";
                Model.ConfigVal = Satellite;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '11000000AA08'");
                Service.PublicBD.db.AddRTU_ConfigData(Model);
            }

        }
    }
}
