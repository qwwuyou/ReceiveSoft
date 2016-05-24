using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using YanYu.WRIMR.Protocol;
using YanYu.WRIMR.Protocol.InPacket;
using TcpService;
using UdpService;
using GsmService;
using ComService;
using System.Collections;

namespace Service
{
    public class WaterResource:DataProcess
    {
        /// <summary>
        /// 包路由器方法(从RTU收)
        /// </summary>
        /// <param name="pack"></param>
        public void PacketArrived(InPackage pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {

            try
            {
                switch (pack.AFN)
                {
                    case AFC._02H:
                        PackageProcess.Process_02H(new Deal0x02(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._C0H:
                        PackageProcess.Process_C0H(new Deal0xC0(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._10H:
                        PackageProcess.Process_10H(new Deal0x10(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._11H:
                        PackageProcess.Process_11H(new Deal0x11(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._12H:
                        PackageProcess.Process_12H(new Deal0x12(pack), NFOINDEX, Server);
                        break;
                    //case YanYu.WRIMR.Protocol.AFC._15H:
                    //    PackageProcess.Process_15H(new Deal0x15(pack), NFOINDEX, Server);
                    //    break;
                    //case YanYu.WRIMR.Protocol.AFC._16H:
                    //    PackageProcess.Process_16H(new Deal0x16(pack), NFOINDEX, Server);
                    //    break;
                    case YanYu.WRIMR.Protocol.AFC._17H:
                        PackageProcess.Process_17H(new Deal0x17(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._18H:
                        //PackageProcess.Process_18H(new Deal0x18(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._19H:
                        PackageProcess.Process_19H(new Deal0x19(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1AH:
                        PackageProcess.Process_1AH(new Deal0x1A(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1BH:
                        PackageProcess.Process_1BH(new Deal0x1B(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1CH:
                        PackageProcess.Process_1CH(new Deal0x1C(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1DH:
                        PackageProcess.Process_1DH(new Deal0x1D(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1EH:
                        PackageProcess.Process_1EH(new Deal0x1E(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._1FH:
                        PackageProcess.Process_1FH(new Deal0x1F(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._20H:
                        PackageProcess.Process_20H(new Deal0x20(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._21H:
                        PackageProcess.Process_21H(new Deal0x21(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._30H:
                        PackageProcess.Process_30H(new Deal0x30(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._31H:
                        PackageProcess.Process_31H(new Deal0x31(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._32H:
                        PackageProcess.Process_32H(new Deal0x32(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._33H:
                        PackageProcess.Process_33H(new Deal0x33(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._34H:
                        PackageProcess.Process_34H(new Deal0x34(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._50H:
                        PackageProcess.Process_50H(new Deal0x50(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._51H:
                        PackageProcess.Process_51H(new Deal0x51(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._52H:
                        PackageProcess.Process_52H(new Deal0x52(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._53H:
                        PackageProcess.Process_53H(new Deal0x53(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._54H:
                        PackageProcess.Process_54H(new Deal0x54(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._55H:
                        PackageProcess.Process_55H(new Deal0x55(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._56H:
                        PackageProcess.Process_56H(new Deal0x56(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._57H:
                        PackageProcess.Process_57H(new Deal0x57(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._58H:
                        PackageProcess.Process_58H(new Deal0x58(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._59H:
                        PackageProcess.Process_59H(new Deal0x59(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._5AH:
                        PackageProcess.Process_5AH(new Deal0x5A(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._5DH:   //遥测终端事件记录
                        PackageProcess.Process_5DH(new Deal0x5D(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._5EH:
                        PackageProcess.Process_5EH(new Deal0x5E(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._5FH:
                        PackageProcess.Process_5FH(new Deal0x5F(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._60H:
                        PackageProcess.Process_60H(new Deal0x60(pack), NFOINDEX, Server);
                        break;
                    //case YanYu.WRIMR.Protocol.AFC._61H:           //图像
                    //    PackageProcess.Process_61H(new Deal0x61(pack));
                    //    break;
                    case YanYu.WRIMR.Protocol.AFC._62H:
                        PackageProcess.Process_62H(new Deal0x62(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._63H:
                        PackageProcess.Process_63H(new Deal0x63(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._64H:
                        PackageProcess.Process_64H(new Deal0x64(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._81H:    //随机自报报警数据
                        PackageProcess.Process_81H(new Deal0x81(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._82H:    //人工置数
                        PackageProcess.Process_82H(new Deal0x82(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._8EH:    //心跳
                        PackageProcess.Process_8EH(new Deal0x8E(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._90H:
                        PackageProcess.Process_90H(new Deal0x90(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._91H:
                        PackageProcess.Process_91H(new Deal0x91(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._92H:
                        PackageProcess.Process_92H(new Deal0x92(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._93H:
                        PackageProcess.Process_93H(new Deal0x93(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._94H:
                        PackageProcess.Process_94H(new Deal0x94(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._95H:
                        PackageProcess.Process_95H(new Deal0x95(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._96H:
                        PackageProcess.Process_96H(new Deal0x96(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._A0H:
                        PackageProcess.Process_A0H(new Deal0xA0(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._A1H:
                        PackageProcess.Process_A1H(new Deal0xA1(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._B0H:                  //查询实时值
                        PackageProcess.Process_B0H(new Deal0xB0(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._B1H:                  //查询固态值
                        PackageProcess.Process_B1H(new Deal0xB1(pack), NFOINDEX, Server);
                        break;
                    case YanYu.WRIMR.Protocol.AFC._B2H:                //查询遥测终端内存自报数据
                        PackageProcess.Process_B2H(new Deal0xB2(pack), NFOINDEX, Server);
                        break;
                    default:
                        break;

                }

                //接收到命令的回复
                CommandReply(pack, NFOINDEX, Server);
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
            }
        }

        /// <summary>
        /// 接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Server">服务</param>
        public void InsertNewSTCD(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            var rtu = from r in Service.ServiceBussiness.RtuList where r.STCD == STCD select r;
            if (rtu.Count() == 0)
            {
                Service.Model.YY_RTU_Basic model = new Service.Model.YY_RTU_Basic();
                model.STCD = STCD;
                model.PassWord = "8050";
                model.NiceName = STCD;
                bool b = PublicBD.db.AddRTU(model);     //添加 
                if(b)
                Service.ServiceBussiness.RtuList.Add(new Service.Model.YY_RTU_Basic() { STCD= STCD, NiceName = STCD,  PassWord= "8050" });
                
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
            //else if (NFOINDEX == Service.ServiceEnum.NFOINDEX.GSM)
            //{
            //    GsmService.GsmServer GS = Server as GsmService.GsmServer;
            //    List<GsmService.GsmMobile> Gs = GS.Gs;
            //    var gsms = from g in Gs where g.STCD == STCD select g;
            //    if (gsms.Count() == 0)
            //    {
                   
            //    }
            //}
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
        /// 接收到命令的回复,短信卫星无回复
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        private void CommandReply(InPackage pack, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
            #region TCP
            if ((int)NFOINDEX == 1)
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                //接收的数据属于下发召测命令的回复                                        //16进制 ---------------------------16进制
                var command = from c in TS.TQ.Qtsd where c.STCD == pack.STCD && (int)pack.AFN == Convert.ToInt32(c.COMMANDCODE, 16) select c;
                List<TcpSendData> Command = command.ToList<TcpSendData>();
                if (Command.Count() > 0)
                {
                    string STCD = pack.STCD;
                    string COMMANDCODE = Command.First().COMMANDCODE;
                    byte[] Data = Command.First().Data;
                    string data = EnCoder.ByteArrayToHexStr(Data);
                    foreach (var item in ServiceControl.tcp)
                    {
                        TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                    }

                    DateTime datetime = DateTime.Now;
                    //清空的目的是调用PackageProcess时重新读取WORKList
                    if (COMMANDCODE == "12" || COMMANDCODE == "52")
                    {
                        ServiceBussiness.GetRTUList();
                    }
                    #region 状态更新
                    //写入命令列表  状态-2完成
                    ServiceBussiness.WriteListCommand(STCD, NFOINDEX, COMMANDCODE, data, datetime, -2);

                    //单条命令状态发送到界面程序
                    ServiceBussiness.CommandWriteQUIM(STCD, NFOINDEX.ToString(), COMMANDCODE, data, datetime, -2);

                    //收到回复后入库命令记录
                    List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, NFOINDEX, COMMANDCODE, -2);
                    foreach (var item in lc)
                    {
                        PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)NFOINDEX, -2);
                    }
                    #endregion
                }

            }
            #endregion
            #region UDP
             else if ((int)NFOINDEX == 2)
             {
                 UdpService.UdpServer US = Server as UdpService.UdpServer;

                 
                 //接收的数据属于下发召测命令的回复                                       //16进制 ---------------------------16进制                            
                 var command = from c in US.UQ.Qusd where c.STCD == pack.STCD && (int)pack.AFN == Convert.ToInt32(c.COMMANDCODE, 16) select c;
                 List<UdpSendData> Command = command.ToList<UdpSendData>();
                 if (Command.Count() > 0)
                 {
                     string STCD = pack.STCD;
                     string COMMANDCODE = Command.First().COMMANDCODE;
                     byte[] Data = Command.First().Data;
                     string data = EnCoder.ByteArrayToHexStr(Data);
         
                     foreach (var item in ServiceControl.udp)
                     {
                         UdpService.UdpBussiness.RemoveUsdQ(item, STCD, COMMANDCODE);
                     }
                     DateTime datetime = DateTime.Now;

                     //清空的目的是调用PackageProcess时重新读取WORKList
                     if (COMMANDCODE == "12" || COMMANDCODE == "52") 
                     {
                          ServiceBussiness.GetRTUList();
                     }

                     #region 状态更新
                     //写入命令列表  状态-2完成
                     ServiceBussiness.WriteListCommand(STCD, NFOINDEX, COMMANDCODE, data, datetime, -2);

                     //单条命令状态发送到界面程序
                     ServiceBussiness.CommandWriteQUIM(STCD, NFOINDEX.ToString(), COMMANDCODE, data, datetime, -2);


                     //收到回复后入库命令记录
                     List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, NFOINDEX, COMMANDCODE, -2);
                     foreach (var item in lc)
                     {
                         PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)NFOINDEX, -2);
                     }
                     #endregion
                 }

             }
             #endregion
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + "接收召测命令操作异常！", ex);
            }
        }


        #region 发送命令
        //-----------发送召测命令时使用  US.UQ.Qusd 或  TS.TQ.Qtsd
        //状态为0时为新产生---判断
        //触发线程执行业务逻辑方法
        //--------------------------------------------------------
        //1、判断是否有可发送命令
        //2、发送命令
        //3、更新连接时间（避免连接被强制断开）
        //4、通知界面
        //5、写入命令列表
        //6、单条命令状态发送到界面程序
        //睡眠30秒重新执行1~6
        //发送3次等待30后命令超时
        //如果超时 从US.UQ.Qusd 或  TS.TQ.Qtsd 删除命令
        //超时状态更新至命令列表
        //超时命令状态发送到界面程序
        //超时数据入召测命令记录


        //udp发送命令
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
            int count = 0;
            while (count < 3)
            {
                DateTime datetime = DateTime.Now;
                //判断命令列表里面有没有命令
                var command = from com in US.UQ.Qusd where com.STCD == usd.STCD && COMMANDCODE == com.COMMANDCODE  select com;
                List<UdpSendData> Command = command.ToList<UdpSendData>();
                //判断有没有在线设备
                var udp = from u in US.Us where u.STCD == usd.STCD && u.IpEndPoint !=null select u;
                List<UdpSocket> UDP = udp.ToList<UdpSocket>();

                if (Command.Count() > 0 && UDP.Count() > 0) //是否有
                {
                    string STCD = Command.First().STCD;
                    byte[] Data = Command.First().Data;
                    string data = EnCoder.ByteArrayToHexStr(Data);


                    Command.First().STATE = count + 1;
                    UDP.First().DATATIME = datetime;//更新为当前时间
                    US.UDPClient.Send(Command.First().Data, Command.First().Data.Length, UDP.First().IpEndPoint);


                    //通知界面
                    ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.UDP.ToString(), US.ServiceID, STCD, "发送召测命令(" + (count + 1) + "次)", Command.First().Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                    //写入本地命令列表
                    ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.UDP, COMMANDCODE, data, datetime, count + 1);

                    //单条命令状态发送到界面程序
                    ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.UDP.ToString(), COMMANDCODE, data, datetime, count + 1);


                    System.Threading.Thread.Sleep(30 * 1000);//可设置
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

        //tcp发送命令
        public void SendCommand(TcpService.TcpServer TS)
        {
            var temp = from t in TS.Ts where t.TCPSOCKET != null select t;
            foreach (var tcp in temp)
            {
                //查询命令集中  站号、功能码 符合条件的记录
                var command = from c in TS.TQ.Qtsd where c.STCD == tcp.STCD select c;
                List<TcpSendData> Command = command.ToList<TcpSendData>();
                if (Command.Count() > 0)
                {
                    if (Command.First().STATE == 0) //0为从未发给rtu
                    {
                        System.Threading.Thread thread = null;
                        try
                        {
                            System.Threading.Thread.Sleep(800);
                            thread = new System.Threading.Thread(() => StartCommand(thread, TS, Command.First()));
                            thread.Start();
                        }
                        catch
                        {
                            Service.ServiceControl.log.Error(DateTime.Now + "TCP信道召测线程启动异常！");
                        }
                    }
                }
            }
            //召测逻辑启动，但未回复，tcp主动下线，一分钟后，命令强制超时
            lock (ServiceControl.LC) 
            {
                foreach (var Lc in ServiceControl.LC)
                {
                    string STCD = Lc.STCD;
                    string COMMANDCODE = Lc.CommandID;
                    string data = Lc.Data;
                    DateTime datetime = DateTime.Now;
                    if (Lc.STATE > 0 && Lc.SERVICETYPE == "TCP" && Lc.DATETIME.AddSeconds(1 * 60) < datetime) 
                    {
                        foreach (var item in ServiceControl.tcp)
                        {
                            TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                        }

                        //写入命令列表  状态-1超时
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, data, datetime, -1);

                        //单条命令状态发送到界面程序
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.TCP.ToString(), COMMANDCODE, data, datetime, -1);

                        //System.Threading.Thread.Sleep(30 * 1000);

                        //datetime = DateTime.Now;
                        //命令本地列表中删除超时记录
                        List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, -1);
                        foreach (var item in lc)
                        {
                            //超时后入库
                            PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)Service.ServiceEnum.NFOINDEX.TCP, -1);
                        }
                    }
                }
            }

        }

        //tcp发送命令逻辑执行
        void StartCommand(System.Threading.Thread thread, TcpService.TcpServer TS, TcpService.TcpSendData tsd)
        {
            try
            {
                #region
                int count = 0;
                while (count < 3)
                {
                    DateTime datetime = DateTime.Now;
                    //判断命令列表里面有没有命令
                    var command = from com in TS.TQ.Qtsd where com.STCD == tsd.STCD && tsd.COMMANDCODE == com.COMMANDCODE select com;
                    List<TcpSendData> Command = command.ToList<TcpSendData>();
                    var tcp = from t in TS.Ts where t.STCD == tsd.STCD && t.TCPSOCKET !=null select t;
                    List<TcpSocket> TCP = tcp.ToList<TcpSocket>();
                    if (Command.Count() > 0 && TCP.Count() > 0)
                    {
                        string STCD = Command.First().STCD;
                        string COMMANDCODE = Command.First().COMMANDCODE;
                        byte[] Data = Command.First().Data;
                        string data = EnCoder.ByteArrayToHexStr(Data);
                        //判断有没有在线设备

                        TCP.First().TCPSOCKET.Send(tsd.Data);

                        Command.First().STATE = count + 1;
                        TCP.First().DATATIME = datetime;//更新为当前时间
                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.TCP.ToString(), TS.ServiceID, STCD, "发送召测命令(" + (count + 1) + "次)", Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        //写入本地命令列表
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, data, datetime, count + 1);
                        //单条命令状态发送到界面程序
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.TCP.ToString(), COMMANDCODE, data, datetime, count + 1);



                        System.Threading.Thread.Sleep(30 * 1000);//可设置
                        if (count >= 2)//三次后超时
                        {
                            datetime = DateTime.Now;
                            //再判断一次是否列表中还存在
                            command = from com in TS.TQ.Qtsd where com.STCD == tsd.STCD && tsd.COMMANDCODE == com.COMMANDCODE select com;
                            Command = command.ToList<TcpSendData>();
                            if (Command.Count() > 0)
                            {
                                foreach (var item in ServiceControl.tcp)
                                {
                                    TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                                }

                                //写入命令列表  状态-1超时
                                ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, data, datetime, -1);

                                //单条命令状态发送到界面程序
                                ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.TCP.ToString(), COMMANDCODE, data, datetime, -1);

                                //System.Threading.Thread.Sleep(30 * 1000);

                                //datetime = DateTime.Now;
                                //命令本地列表中删除超时记录
                                List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, -1);
                                foreach (var item in lc)
                                {
                                    //超时后入库
                                    PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)Service.ServiceEnum.NFOINDEX.TCP, -1);
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
                Service.ServiceControl.log.Error(DateTime.Now + "tcp发送命令逻辑执行异常！", ex);
            }
        }

        //gsm发送命令
        public void SendCommand(GsmService.GsmServer GS)
        {
            lock (GS.Gs)
            foreach (var gsm in GS.Gs)
            {
                //查询命令集中  站号、功能码 符合条件的记录
                var command = from c in GS.GQ.Qgsd where c.STCD == gsm.STCD select c;
                List<GsmSendData> Command = command.ToList<GsmSendData>();
                if (Command.Count() > 0)
                {
                    if (Command.First().STATE == 0) //0为从未发给rtu
                    {
                        try
                        {
                            StartCommand(GS, Command.First());

                            System.Threading.Thread.Sleep(1000);
                        }
                        catch
                        { Service.ServiceControl.log.Error(DateTime.Now + "GSM信道召测方法异常！"); }
                    }
                }
            }
        }

        //gsm发送命令逻辑执行
        void StartCommand( GsmService.GsmServer GS, GsmService.GsmSendData gsd)
        {
            var gsm = from g in GS.Gs where g.STCD == gsd.STCD select g;
            List<GsmMobile> GSM = gsm.ToList<GsmMobile>();
            var command = from cmd in GS.GQ.Qgsd where cmd.STCD == gsd.STCD && gsd.COMMANDCODE == cmd.COMMANDCODE select cmd;
            List<GsmSendData> Command = command.ToList<GsmSendData>();
            string STCD = gsd.STCD;
            string COMMANDCODE = gsd.COMMANDCODE;
            DateTime datetime = DateTime.Now;
            if (GSM.Count() > 0 && GSM.First().MOBILE != null && GSM.First().MOBILE != "")
            {
                if (Command.Count() > 0)
                {
                    byte[] Data = Command.First().Data;
                    try
                    {
                        GS.SendData(GSM.First().MOBILE, Data);

                        GSM.First().DATATIME = datetime;//更新为当前时间

                        string data = EnCoder.ByteArrayToHexStr(Data);
                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.GSM.ToString(), GS.ServiceID, STCD, "发送召测命令", Command.First().Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        //写入本地命令列表  状态-2完成
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.GSM, COMMANDCODE, data, datetime, -2);

                        //单条命令状态发送完成--更新UI命令列表
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.GSM.ToString(), COMMANDCODE, data, datetime, 2);
                        
                        
                        System.Threading.Thread.Sleep(2*1000);
                        datetime = DateTime.Now;
                        //单条命令状态发送到界面程序(成功)--更新UI命令列表
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.GSM.ToString(), COMMANDCODE, data, datetime, -2);
                        
                        //移除命令
                        foreach (var item in ServiceControl.gsm)
                        {
                            GsmService.GsmBussiness.RemoveGsdQ(item, STCD, COMMANDCODE);
                        }

                        
                        //发送成功30秒后 删除并入库
                        System.Threading.Thread.Sleep(30 * 1000);

                        datetime = DateTime.Now;
                        List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.GSM, COMMANDCODE, -2);
                        foreach (var item in lc)
                        {
                            PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, EnCoder.ByteArrayToHexStr(Data), (int)Service.ServiceEnum.NFOINDEX.GSM, -2);
                        }
                    }
                    catch
                    {
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.GSM.ToString(), GS.ServiceID, STCD, "发送召测命令失败", Command.First().Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    

                    
                }
            }
            else 
            {
                //通知界面
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, gsd.STCD, "测站["+gsd.STCD +"]没有设置手机号", new byte[]{}, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                //写入命令列表  状态-1失败
                ServiceBussiness.WriteListCommand(STCD,ServiceEnum.NFOINDEX.GSM , COMMANDCODE, "", datetime, -1);

                System.Threading.Thread.Sleep(2000);
                datetime = DateTime.Now;
                //单条命令状态发送到界面程序(失败)--更新UI命令列表
                ServiceBussiness.CommandWriteQUIM(STCD, "GSM", COMMANDCODE, "", datetime, -1);

                //移除命令
                foreach (var item in ServiceControl.gsm)
                {
                    GsmService.GsmBussiness.RemoveGsdQ(item, STCD, COMMANDCODE);
                }


                //超时30秒后 删除并入库
                System.Threading.Thread.Sleep(30 * 1000);
                //超时后入库
                PublicBD.db.AddDataCommand(STCD, COMMANDCODE, datetime, null, EnCoder.ByteArrayToHexStr(gsd.Data), (int)Service.ServiceEnum.NFOINDEX.GSM, -1);

                //命令列表中删除超时记录
                List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, ServiceEnum.NFOINDEX.GSM, COMMANDCODE, -1);
                }
        }

        //com发送命令
        public void SendCommand(ComService.ComServer CS)
        {
            lock (CS.Cs)
                foreach (var com in CS.Cs)
                {
                    //查询命令集中  站号、功能码 符合条件的记录
                    var command = from c in CS.CQ.Qcsd where c.STCD == com.STCD select c;
                    List<ComSendData> Command = command.ToList<ComSendData>();
                    if (Command.Count() > 0)
                    {
                        if (Command.First().STATE == 0) //0为从未发给rtu
                        {
                            try
                            {
                                StartCommand(CS, Command.First());

                                System.Threading.Thread.Sleep(1000);
                            }
                            catch
                            { Service.ServiceControl.log.Error(DateTime.Now + "COM信道召测方法异常！"); }
                        }
                    }
                }
        }

        //com发送命令逻辑执行
        void StartCommand(ComService.ComServer CS, ComService.ComSendData csd) 
        {
            var com = from c in CS.Cs where c.STCD == csd.STCD select c;
            List<ComSatellite> COM = com.ToList<ComSatellite>();
            var command = from cmd in CS.CQ.Qcsd where cmd.STCD == csd.STCD && csd.COMMANDCODE == cmd.COMMANDCODE select cmd;
            List<ComSendData> Command = command.ToList<ComSendData>();
            string STCD = csd.STCD;
            string COMMANDCODE = csd.COMMANDCODE;
            DateTime datetime = DateTime.Now;
            if (COM.Count() > 0 && COM.First().SATELLITE != null && COM.First().SATELLITE != "")
            {
                if (Command.Count() > 0)
                {
                    byte[] Data = Command.First().Data;
                    try
                    {
                        CS.SendData(COM.First().SATELLITE, Data);

                        COM.First().DATATIME = datetime;//更新为当前时间

                        string data = EnCoder.ByteArrayToHexStr(Data);
                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, STCD, "发送召测命令", Command.First().Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                        //写入本地命令列表  状态-2完成
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.COM, COMMANDCODE, data, datetime, -2);

                        //单条命令状态发送完成--更新UI命令列表
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.COM.ToString(), COMMANDCODE, data, datetime, 2);


                        System.Threading.Thread.Sleep(2 * 1000);
                        datetime = DateTime.Now;
                        //单条命令状态发送到界面程序(成功)--更新UI命令列表
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.COM.ToString(), COMMANDCODE, data, datetime, -2);

                        //移除命令
                        foreach (var item in ServiceControl.com)
                        {
                            ComService.ComBussiness.RemoveCsdQ(item, STCD, COMMANDCODE);
                        }


                        //发送成功30秒后 删除并入库
                        System.Threading.Thread.Sleep(30 * 1000);

                        datetime = DateTime.Now;
                        List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.COM, COMMANDCODE, -2);
                        foreach (var item in lc)
                        {
                            PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, EnCoder.ByteArrayToHexStr(Data), (int)Service.ServiceEnum.NFOINDEX.COM, -2);
                        }
                    }
                    catch
                    {
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, STCD, "发送召测命令失败", Command.First().Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }



                }
            }
            else
            {
                //通知界面
                ServiceBussiness.WriteQUIM("COM", CS.ServiceID, csd.STCD, "测站[" + csd.STCD + "]没有设置卫星号", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                //写入命令列表  状态-1失败
                ServiceBussiness.WriteListCommand(STCD, ServiceEnum.NFOINDEX.COM, COMMANDCODE, "", datetime, -1);

                System.Threading.Thread.Sleep(2000);
                datetime = DateTime.Now;
                //单条命令状态发送到界面程序(失败)--更新UI命令列表
                ServiceBussiness.CommandWriteQUIM(STCD, "COM", COMMANDCODE, "", datetime, -1);

                //移除命令
                foreach (var item in ServiceControl.com)
                {
                    ComService.ComBussiness.RemoveCsdQ(item, STCD, COMMANDCODE);
                }


                //超时30秒后 删除并入库
                System.Threading.Thread.Sleep(30 * 1000);
                //超时后入库
                PublicBD.db.AddDataCommand(STCD, COMMANDCODE, datetime, null, EnCoder.ByteArrayToHexStr(csd.Data), (int)Service.ServiceEnum.NFOINDEX.COM, -1);

                //命令列表中删除超时记录
                List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, ServiceEnum.NFOINDEX.COM, COMMANDCODE, -1);
            }
        }
        #endregion


        #region 解包处理（已实现除卫星信道外其他三信道）
        /// <summary>
        /// udp信道处理数据报
        /// </summary>
        /// <param name="US">udp服务</param>
        public void PacketArrived(UdpService.UdpServer US)
        {
            string ServiceId = US.ServiceID;
            ConcurrentQueue<UdpReceivedData> Qurd = US.UQ.Qurd;
            List<UdpSocket> Us = US.Us;
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;

            YanYu.WRIMR.Protocol.InPackage bp = null;

            while (Qurd.Count > 0)
            {
                UdpReceivedData urd = null;
                Qurd.TryDequeue(out urd);
                if (urd != null)
                {

                    //注册
                    Service.ServiceBussiness.Registered30(urd.Data);

                    //拆包
                    List<byte[]> lb = SplitPack(urd.Data);

                    if (lb != null && lb.Count > 0)
                    {
                        foreach (var item in lb)
                        {
                            #region 独立包处理
                            try
                            {
                                //详细解包
                                bp = new YanYu.WRIMR.Protocol.InPackage(item);
                                //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                InsertNewSTCD(bp.STCD, Service.ServiceEnum.NFOINDEX.UDP, US);
                            }
                            catch
                            {
                                bp = null;
                                //通知界面
                                ServiceBussiness.WriteQUIM("UDP", ServiceId, "", "接收异常数据", item, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }


                            if (bp != null)
                            {
                                // 根据数据得到STCD;
                                string STCD = bp.STCD;

                                //更新socket列表的stcd、socket
                                bool B = false;
                                UdpBussiness.UpdSocket(US, urd.IpEndPoint, STCD, out B);


                                if (!B)
                                {
                                    //上线
                                    UdpBussiness.UdpConnected(US, STCD);
                                }
                                ServiceBussiness.WriteQUIM("UDP", ServiceId, STCD, "接收数据", item, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                try
                                {
                                    //根据包路由器执行处理程序
                                    PacketArrived(bp, Service.ServiceEnum.NFOINDEX.UDP, US);
                                }
                                catch (Exception ex)
                                {
                                    Service.ServiceControl.log.Error(DateTime.Now + "UDP信道包处理异常", ex);
                                }
                            }
                            #endregion

                            #region 写入透明传输列表 udp_OnReceivedData事件中已经写入，因此注销以下代码
                            //ServiceBussiness.WriteQDM(item.Data);
                            #endregion
                        }
                       
                    }
                    else { ServiceBussiness.WriteQUIM("UDP", ServiceId, "[未知]", "分包失败，接收数据：", urd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text); }

      
                }
            }
        }

        /// <summary>
        /// tcp信道处理数据报
        /// </summary>
        /// <param name="TS">tcp服务</param>
        public void PacketArrived(TcpService.TcpServer TS)
        {
            string ServiceId = TS.ServiceID;
            ConcurrentQueue<TcpReceivedData> Qtrd = TS.TQ.Qtrd;
            List<TcpSocket> Ts = TS.Ts;
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;

            YanYu.WRIMR.Protocol.InPackage bp = null;

            while (Qtrd.Count > 0)
            {
                TcpReceivedData trd = null;
                Qtrd.TryDequeue(out trd);
                if (trd != null)
                {
                    //注册
                    Service.ServiceBussiness.Registered30(trd.Data);

                    //拆包
                    List<byte[]> lb = SplitPack(trd.Data);

                    if (lb != null && lb.Count > 0)
                    {
                        foreach (var item in lb)
                        {

                            #region 独立包处理
                            try
                            {
                                //详细解包
                                bp = new YanYu.WRIMR.Protocol.InPackage(item);
                                //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                InsertNewSTCD(bp.STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);
                            }
                            catch
                            {
                                bp = null;
                                //通知界面
                                ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", item, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }


                            if (bp != null)
                            {

                                // 根据数据得到STCD;
                                string STCD = bp.STCD;


                                bool B = false;
                                //更新socket列表的stcd、socket
                                TcpBussiness.UpdSocket(TS, trd.SOCKET, STCD, out B);

                                if (!B)
                                {
                                    //上线
                                    TcpBussiness.TcpConnected(TS, STCD);

                                }

                                //通知界面
                                ServiceBussiness.WriteQUIM("TCP", ServiceId, STCD, "接收数据", item, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                                try
                                {
                                    //根据包路由器执行处理程序
                                    PacketArrived(bp, Service.ServiceEnum.NFOINDEX.TCP, TS);
                                }
                                catch (Exception ex)
                                {
                                    Service.ServiceControl.log.Error(DateTime.Now + "TCP信道包处理异常", ex);
                                }
                            }
                            #endregion

                            #region 写入透明传输列表 tcp_OnReceivedData事件中已经写入，因此注销以下代码
                            //ServiceBussiness.WriteQDM(trd.Data);
                            #endregion
                        }
                    }
                    else { ServiceBussiness.WriteQUIM("TCP", ServiceId, "[未知]", "分包失败，接收数据：", trd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text); }

                }
            }
        }

        //连包拆包方法(已经用于tcp&udp信道)
        private List<byte[]> SplitPack(byte[] bt)
        {
            List<byte[]> lb = new List<byte[]>();
            int len = 0;
            int start = 0;
            try
            {
                while (bt != null && bt.Length > len + 5 && bt[len] == 0x68)
                {
                    int pl = 0;
                    start = len;//== 0 ? 0 : len - 1;
                    pl = bt[len + 1] + 5;
                    len = len + bt[len + 1] + 5;

                    if (bt[len - 1] != 0x16)
                    {
                        pl = pl + 1;
                        len = len + 1;
                    }

                    byte[] bp = new byte[pl];
                    if (pl != bt.Length)
                        Array.Copy(bt, start, bp, 0, pl);
                    else
                        bp = bt;

                    //调用解包传入参数bp
                    //例如:InPackage pack = packParser.ParserInPackage(bp);
                    lb.Add(bp);
                }
            }
            catch 
            {
                if (lb.Count > 0)
                { return lb; }
                else
                {
                    return null;
                }
            }
            return lb;
        }

        /// <summary>
        /// 短信信道处理数据报
        /// </summary>
        /// <param name="GS">短信服务</param>
        public void PacketArrived(GsmService.GsmServer GS)
        {  
            string ServiceId = GS.ServiceID;
            ConcurrentQueue<GsmReceivedData> Qgrd = GS.GQ.Qgrd;
            List<GsmMobile> Gs = GS.Gs;
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;

            YanYu.WRIMR.Protocol.InPackage bp = null;
            while (Qgrd.Count > 0)
            {
                GsmReceivedData item = null;
                Qgrd.TryDequeue(out item);
                if (item != null)
                {
                    try
                    {
                        //详细解包
                        bp = new YanYu.WRIMR.Protocol.InPackage(item.Data);
                        //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                        InsertNewSTCD(bp.STCD, Service.ServiceEnum.NFOINDEX.GSM , GS);
                    }
                    catch
                    {
                        bp = null;
                        //通知界面
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "接收异常数据", item.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "尝试中文数据转码[" + GS.TrySmsContent + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }

                    if (bp != null)
                    {
                        string STCD = bp.STCD;// 根据item.Data得到STCD;

                        InsertGsmMobile(GS, item.MOBILE, STCD);

                        //更新连接列表的stcd、mobile
                        GsmBussiness.UpdMobile(GS, item.MOBILE, STCD);

                        ServiceBussiness.WriteQUIM("GSM", ServiceId, STCD, "接收数据", item.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        
                        try{
                        //根据包路由器执行处理程序
                        PacketArrived(bp,Service.ServiceEnum.NFOINDEX.GSM , GS);//根据item.Data得到回复数据
                        }
                        catch (Exception ex)
                        {
                            Service.ServiceControl.log.Error(DateTime.Now + "GSM信道包处理异常", ex);
                        }
                    }


                    #region 写入透明传输列表 gsm_OnReceivedData事件中已经写入，因此注销以下代码
                    //ServiceBussiness.WriteQDM(item.Data);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 卫星信道处理数据报
        /// </summary>
        /// <param name="CS">卫星服务</param>
        public void PacketArrived(ComService.ComServer CS)
        {
            string ServiceId = CS.ServiceID;
            ConcurrentQueue<ComReceivedData> Qcrd = CS.CQ.Qcrd;
            List<ComSatellite> Cs = CS.Cs;
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;

            YanYu.WRIMR.Protocol.InPackage bp = null;
            while (Qcrd.Count > 0)
            {
                ComReceivedData item = null;
                Qcrd.TryDequeue(out item);

                if (item != null)
                {
                    

                    //验证数据是否符合卫星格式，并提取数据区内容及out卫星号
                    string Satellite = null;
                    byte[] data = DataValidate(item.Data, out Satellite);

                    if (data != null)
                    {

                        try
                        {
                            bp = new YanYu.WRIMR.Protocol.InPackage(data);
                            //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                            InsertNewSTCD(bp.STCD, Service.ServiceEnum.NFOINDEX.COM, CS);

                        }
                        catch
                        {
                            bp = null;
                            //通知界面
                            ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, "", "接收异常数据", item.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }

                        if (bp != null)
                        {
                            InsertComSatellite(CS, Satellite, bp.STCD);

                            ComBussiness.UpdSatellite(CS, Satellite, bp.STCD);

                            ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, bp.STCD, "接收数据", data, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                            try
                            {
                                //根据包路由器执行处理程序
                                PacketArrived(bp, Service.ServiceEnum.NFOINDEX.COM, CS);//根据item.Data得到回复数据
                            }
                            catch (Exception ex)
                            {
                                Service.ServiceControl.log.Error(DateTime.Now + "COM信道包处理异常", ex);
                            }
                        }
                    }
                    else
                    {
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, "", "接收未能解析卫星数据", data, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);
                    }
                    
                    #region 写入透明传输列表 com_OnReceivedData事件中已经写入，因此注销一下代码
                    //ServiceBussiness.WriteQDM(item.Data);
                    #endregion
                }
               
            }
        }

        private byte[] DataValidate(byte[] data, out string Satellite)
        {
            string str = System.Text.Encoding.ASCII.GetString(data);
            try
            {
                string[] Temp_Strs = str.Split(new string[] { "," }, StringSplitOptions.None);
                if (int.Parse(Temp_Strs[7]) == Temp_Strs[8].Length)
                {
                    Satellite = Temp_Strs[2];
                    return System.Text.Encoding.ASCII.GetBytes(Temp_Strs[8]);
                }
                else
                {
                    Satellite = null;
                    return null;
                }
            }
            catch
            {
                Satellite = null;
                return null;
            }
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
                Model.ConfigID = "12000000AA07";
                Model.ConfigVal = Mobile;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '12000000AA07'");
                Service.PublicBD.db.AddRTU_ConfigData(Model);
            }

        }

        /// <summary>
        ///  更新或写入测站卫星号码（仅用于卫星信道）
        /// </summary>
        /// <param name="CS"></param>
        /// <param name="Satellite">卫星号</param>
        /// <param name="STCD">站号</param>
        public void InsertComSatellite(ComServer CS, string Satellite, string STCD)
        {
            var temp = from c in CS.Cs where c.SATELLITE == Satellite && c.STCD == STCD select c;
            if (temp.Count() == 0)
            {
                Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = STCD;
                Model.ItemID = "0000000000";
                Model.ConfigID = "12000000AA08";
                Model.ConfigVal = Satellite;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '12000000AA08'");
                Service.PublicBD.db.AddRTU_ConfigData(Model);
            }
        }
        #endregion
    }
}
