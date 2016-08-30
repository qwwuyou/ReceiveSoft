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
using YanYu.Protocol.HIMR.LinkLayer;
using YanYu.Protocol.HIMR.InPacket;
using YanYu.Protocol.HIMR;
using HydrologicProtocol;

namespace Service
{
    public class Hydrologic : DataProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        /// <summary>
        /// 包路由器方法(从RTU收)
        /// </summary>
        /// <param name="pack"></param>
        public void PacketArrived(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                ushort _0x = frame.FramHead.FunCode;
                switch (_0x)
                {
                    case 0x2F:
                        PackageProcess.Process_0x2F(frame, NFOINDEX, Server);
                        break;
                    case 0x30:
                        PackageProcess.Process_0x30(frame, NFOINDEX, Server);
                        break;
                    case 0x31:
                        PackageProcess.Process_0x31(frame, NFOINDEX, Server);
                        break;
                    case 0x32:
                        PackageProcess.Process_0x32(frame, NFOINDEX, Server);
                        break;
                    case 0x33:
                        PackageProcess.Process_0x33(frame, NFOINDEX, Server);
                        break;
                    case 0x34:
                        PackageProcess.Process_0x34(frame, NFOINDEX, Server);
                        break;
                    case 0xEF: //黑龙江增过程报EF
                        PackageProcess.Process_0xEF(frame, NFOINDEX, Server);
                        break;
                    case 0x35:
                        PackageProcess.Process_0x35(frame, NFOINDEX, Server);
                        break;
                    case 0x36:
                        PackageProcess.Process_0x36(frame, NFOINDEX, Server);
                        break;
                    case 0x37:
                        PackageProcess.Process_0x37(frame, NFOINDEX, Server);
                        break;
                    case 0x38:
                        PackageProcess.Process_0x38(frame, NFOINDEX, Server);
                        break;
                    case 0x39:
                        PackageProcess.Process_0x39(frame, NFOINDEX, Server);
                        break;
                    case 0x3A:
                        PackageProcess.Process_0x3A(frame, NFOINDEX, Server);
                        break;
                    case 0x40:
                        PackageProcess.Process_0x40(frame, NFOINDEX, Server);
                        break;
                    case 0x41:
                        PackageProcess.Process_0x41(frame, NFOINDEX, Server);
                        break;
                    case 0x42:
                        PackageProcess.Process_0x42(frame, NFOINDEX, Server);
                        break;
                    case 0x43:
                        PackageProcess.Process_0x43(frame, NFOINDEX, Server);
                        break;
                    case 0x44:
                        PackageProcess.Process_0x44(frame, NFOINDEX, Server);
                        break;
                    case 0x45:
                        PackageProcess.Process_0x45(frame, NFOINDEX, Server);
                        break;
                    case 0x46:
                        PackageProcess.Process_0x46(frame, NFOINDEX, Server);
                        break;
                    case 0x47:
                        PackageProcess.Process_0x47(frame, NFOINDEX, Server);
                        break;
                    case 0x48:
                        PackageProcess.Process_0x48(frame, NFOINDEX, Server);
                        break;
                    case 0x49:
                        PackageProcess.Process_0x49(frame, NFOINDEX, Server);
                        break;
                    case 0x4A:
                        PackageProcess.Process_0x4A(frame, NFOINDEX, Server);
                        break;
                    case 0x4B:
                        PackageProcess.Process_0x4B(frame, NFOINDEX, Server);
                        break;
                    case 0x4C:
                        PackageProcess.Process_0x4C(frame, NFOINDEX, Server);
                        break;
                    case 0x4D:
                        PackageProcess.Process_0x4D(frame, NFOINDEX, Server);
                        break;
                    case 0x4E:
                        PackageProcess.Process_0x4E(frame, NFOINDEX, Server);
                        break;
                    case 0x4F:
                        PackageProcess.Process_0x4F(frame, NFOINDEX, Server);
                        break;
                    case 0x50:
                        PackageProcess.Process_0x50(frame, NFOINDEX, Server);
                        break;
                    case 0x51:
                        PackageProcess.Process_0x51(frame, NFOINDEX, Server);
                        break;
                    case 0x60:
                        PackageProcess.Process_0x60(frame, NFOINDEX, Server);
                        break;
                    default:
                        break;
                }
                //接收到命令的回复
                CommandReply(frame, NFOINDEX, Server);

            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
            }
        }


        /// <summary>
        /// 接收到命令的回复,短信卫星无回复
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        private void CommandReply(Frame frame, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {

                InPackage inPackage = new InPackage();
                inPackage.AddFrame(frame);
                string stcd = inPackage.StationNumber.ToString();


                #region TCP
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    //接收的数据属于下发召测命令的回复                                        //16进制 ---------------------------16进制
                    var command = from c in TS.TQ.Qtsd where c.STCD == stcd && (int)frame.FramHead.FunCode == Convert.ToInt32(c.COMMANDCODE, 16) select c;
                    List<TcpSendData> Command = command.ToList<TcpSendData>();
                    if (Command.Count() > 0)
                    {
                        string STCD = stcd;
                        string COMMANDCODE = Command.First().COMMANDCODE;
                        byte[] Data = Command.First().Data;
                        string data = EnCoder.ByteArrayToHexStr(Data);
                        foreach (var item in ServiceControl.tcp)
                        {
                            TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                        }

                        DateTime datetime = DateTime.Now;
                        //清空的目的是调用PackageProcess时重新读取WORKList
                        if (COMMANDCODE == "49" )//|| COMMANDCODE == "52"
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
                    var command = from c in US.UQ.Qusd where c.STCD == stcd && (int)frame.FramHead.FunCode == Convert.ToInt32(c.COMMANDCODE, 16) select c;
                    List<UdpSendData> Command = command.ToList<UdpSendData>();
                    if (Command.Count() > 0)
                    {
                        
                        string STCD = stcd;
                        string COMMANDCODE = Command.First().COMMANDCODE;
                        byte[] Data = Command.First().Data;
                        string data = EnCoder.ByteArrayToHexStr(Data);
                        
                        foreach (var item in ServiceControl.udp)
                        {
                            UdpService.UdpBussiness.RemoveUsdQ(item, STCD, COMMANDCODE);
                        }
                        DateTime datetime = DateTime.Now;

                        //清空的目的是调用PackageProcess时重新读取WORKList
                        if (COMMANDCODE == "49" )//|| COMMANDCODE == "52"
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
        public void SendCommand(UdpServer US)
        {
            //lock (US.Us)
            foreach (var udp in US.Us)//在线
            {
                //查询命令集中  站号、功能码 符合条件的记录
                var command = from c in US.UQ.Qusd where c.STCD == udp.STCD select c;
                List<UdpSendData> Command = command.ToList<UdpSendData>();
                if (Command.Count() > 0 && udp.IpEndPoint != null && udp.CanSend == true)   //&& udp.IpEndPoint != null && udp.CanSend == true 后加的,用来激活逻辑线程
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
                    var command = from com in US.UQ.Qusd where com.STCD == usd.STCD && COMMANDCODE == com.COMMANDCODE select com;
                    List<UdpSendData> Command = command.ToList<UdpSendData>();
                    //判断有没有在线设备
                    var udp = from u in US.Us where u.STCD == usd.STCD && u.IpEndPoint != null   select u;
                    List<UdpSocket> UDP = udp.ToList<UdpSocket>();

                    if (Command.Count() > 0 && UDP.Count() > 0) //是否有
                    {
                        if (Command.First().STATE == 0 && count > 0 )   //如果重新召测该命令，STATE == 0，但count>0 需要退出线程，重新激活召测逻辑线程
                        {
                            return;
                        }
                        if (Command.First().STATE > count) 
                        {
                            return;
                        }

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
                            UDP.First().CanSend = false;//恢复不可发送召测命令状态

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


        public void SendCommand(TcpServer TS)
        {
            var temp = from t in TS.Ts where t.TCPSOCKET != null && t.CanSend==true  select t;
            foreach (var tcp in temp)
            {
                //查询命令集中  站号、功能码 符合条件的记录
                var command = from c in TS.TQ.Qtsd where c.STCD == tcp.STCD select c;
                List<TcpSendData> Command = command.ToList<TcpSendData>();
                if (Command.Count() > 0 && tcp.TCPSOCKET != null && tcp.CanSend == true)  //&& tcp.TCPSOCKET != null && tcp.CanSend ==true 后加的,用来激活逻辑线程
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
            //lock (ServiceControl.LC)
            //{
            //    foreach (var Lc in ServiceControl.LC)
            //    {
            //        string STCD = Lc.STCD;
            //        string COMMANDCODE = Lc.CommandID;
            //        string data = Lc.Data;
            //        DateTime datetime = DateTime.Now;
            //        if (Lc.STATE > 0 && Lc.SERVICETYPE == "TCP" && Lc.DATETIME.AddSeconds(1 * 60) < datetime)
            //        {

            //            temp.First().CanSend = false;//恢复不可发送召测命令状态

            //            foreach (var item in ServiceControl.tcp)
            //            {
            //                TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
            //            }

            //            //写入命令列表  状态-1超时
            //            ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, data, datetime, -1);

            //            //单条命令状态发送到界面程序
            //            ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.TCP.ToString(), COMMANDCODE, data, datetime, -1);

            //            //System.Threading.Thread.Sleep(30 * 1000);

            //            //datetime = DateTime.Now;
            //            //命令本地列表中删除超时记录
            //            List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, -1);
            //            foreach (var item in lc)
            //            {
            //                //超时后入库
            //                PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, data, (int)Service.ServiceEnum.NFOINDEX.TCP, -1);
            //            }
            //        }
            //    }
            //}
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
                    var tcp = from t in TS.Ts where t.STCD == tsd.STCD && t.TCPSOCKET != null  select t;
                    List<TcpSocket> TCP = tcp.ToList<TcpSocket>();
                    if (Command.Count() > 0 && TCP.Count() > 0)
                    {
                        if (Command.First().STATE == 0 && count > 0)   //如果重新召测该命令，STATE == 0，但count>0 需要退出线程，重新激活召测逻辑线程
                        {
                            return;
                        }
                        if (Command.First().STATE > count)
                        {
                            return;
                        }

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

                            TCP.First().CanSend = false;//恢复不可发送召测命令状态

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


        public void SendCommand(GsmServer GS)
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
        void StartCommand(GsmService.GsmServer GS, GsmService.GsmSendData gsd)
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


                        System.Threading.Thread.Sleep(2 * 1000);
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
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, gsd.STCD, "测站[" + gsd.STCD + "]没有设置手机号", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                //写入命令列表  状态-1失败
                ServiceBussiness.WriteListCommand(STCD, ServiceEnum.NFOINDEX.GSM, COMMANDCODE, "", datetime, -1);

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


        public void SendCommand(ComService.ComServer CS)
        {
            lock (CS.Cs)
                foreach (var com in CS.Cs)
                {
                    //查询命令集中  站号、功能码 符合条件的记录
                    var command = from c in CS.CQ .Qcsd  where c.STCD == com.STCD select c;
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
                        foreach (var item in ServiceControl.com )
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

        public void PacketArrived(UdpServer US)
        {
            string ServiceId = US.ServiceID;
            ConcurrentQueue<UdpReceivedData> Qurd = US.UQ.Qurd;
            List<UdpSocket> Us = US.Us;
            ConcurrentQueue<UdpSendData> Qusd = US.UQ.Qusd;


            while (Qurd.Count > 0)
            {
                UdpReceivedData urd = null;
                Qurd.TryDequeue(out urd);
                if (urd != null)
                {
                    Frame[] frames = null;
                    
                    try
                    {
                        //注册&透传
                        Service.ServiceBussiness.RemoteCommand(urd.Data);
                        //Service.ServiceBussiness.Registered30(urd.Data);


                        frames = Frame.CreateFrame(urd.Data);
                        if (frames != null && frames.Length > 0)
                        {
                            string STCD = frames[0].FramHead.StationAddress.ToString();
                            //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                            InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.UDP, US);

                            bool B = false;
                            //更新socket列表的stcd、socket
                            UdpBussiness.UpdSocket(US, urd.IpEndPoint, STCD, out B);
                            if (!B)
                            {
                                //上线
                                UdpBussiness.UdpConnected(US, STCD);
                            }

                            //通知界面
                            ServiceBussiness.WriteQUIM("UDP", ServiceId, STCD, "接收数据", urd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);


                            foreach (Frame frame in frames)
                            {
                                PacketArrived(frame, ServiceEnum.NFOINDEX.UDP, US);
                            }
                        }
                        else
                        {
                            ServiceBussiness.WriteQUIM("UDP", ServiceId, "", "接收异常数据", urd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        frames = null;
                        //通知界面
                        ServiceBussiness.WriteQUIM("UDP", ServiceId, "", "接收异常数据", urd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }

                }
            }
            //throw new NotImplementedException();
        }

        public void PacketArrived(TcpServer TS)
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
                    //注册&透传
                    //Service.ServiceBussiness.Registered30(trd.Data);
                    Service.ServiceBussiness.RemoteCommand(trd.Data);

                    Frame[] frames = null;

                    try
                    {
                        frames = Frame.CreateFrame(trd.Data);
                        if (frames != null && frames.Length > 0)
                        {
                            string STCD = frames[0].FramHead.StationAddress.ToString();
                            //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
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
                            ServiceBussiness.WriteQUIM("TCP", ServiceId, STCD, "接收数据", trd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);


                            foreach (Frame frame in frames)
                            {
                                PacketArrived(frame, ServiceEnum.NFOINDEX.TCP, TS);
                            }
                        }
                        else
                        {
                            ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        frames = null;
                        //通知界面
                        ServiceBussiness.WriteQUIM("TCP", ServiceId, "", "接收异常数据", trd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }




                }
            }
            //throw new NotImplementedException();
        }

        public void PacketArrived(GsmServer GS)
        {
            string ServiceId = GS.ServiceID;
            ConcurrentQueue<GsmReceivedData> Qgrd = GS.GQ.Qgrd;
            List<GsmMobile> Gs = GS.Gs;
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;


            while (Qgrd.Count > 0)
            {
                GsmReceivedData grd = null;
                Qgrd.TryDequeue(out grd);
                if (grd != null)
                {
                    Frame[] frames = null;

                    try{

                    frames = Frame.CreateFrame(grd.Data);
                    if (frames != null && frames.Length > 0)
                    {
                        string STCD = frames[0].FramHead.StationAddress.ToString();
                        //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                        InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.GSM, GS);

                        //列表中如果不存在手机号，先写入数据库
                        InsertGsmMobile(GS, grd.MOBILE, STCD);

                        //更新mobile列表的stcd、mobile
                        GsmBussiness.UpdMobile(GS, grd.MOBILE, STCD);

                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.GSM.ToString(), ServiceId, STCD, "接收数据" , grd.Data , ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                        //包路由
                        foreach (Frame frame in frames)
                        {
                            PacketArrived(frame, ServiceEnum.NFOINDEX.GSM, GS);
                        }
                    }
                    else
                    {
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "接收异常数据", grd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "尝试中文数据转码[" + GS.TrySmsContent + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    }
                    catch (Exception ex)
                    {
                        frames = null;
                        //通知界面
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "接收异常数据", grd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "尝试中文数据转码[" + GS.TrySmsContent+"]", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }
                }
            }
        }

        public void PacketArrived(ComServer CS)
        {
            string ServiceId = CS.ServiceID;
            ConcurrentQueue<ComReceivedData> Qcrd = CS.CQ .Qcrd ;
            List<ComSatellite> Gs = CS.Cs ;
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;

            while (Qcrd.Count > 0)
            {
                ComReceivedData crd = null;
                Qcrd.TryDequeue(out crd);
                if (crd != null)
                {
                    Console.WriteLine("调用处理数据");
                    Frame[] frames = null;
                    try
                    {
                        string Satellite = crd.SATELLITE;
                        if (crd != null)
                        {
                            frames = Frame.CreateFrame(crd.Data);
                            if (frames != null && frames.Length > 0)
                            {
                                string STCD = frames[0].FramHead.StationAddress.ToString();
                                InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.COM, CS);
                                InsertComSatellite(CS, Satellite, STCD);
                                ComBussiness.UpdSatellite(CS, Satellite, STCD);

                                ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, STCD, "接收数据", crd.Data, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                                //包路由
                                foreach (Frame frame in frames)
                                {
                                    PacketArrived(frame, ServiceEnum.NFOINDEX.COM, CS);
                                }
                            }
                            else
                            {
                                ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, "", "接收异常数据", crd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                            }
                        }
                        else 
                        {
                            ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, "", "接收未能解析卫星数据", crd.Data, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        frames = null;
                        //通知界面
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, "", "接收异常数据", crd.Data, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                        log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                    }



                }
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
                model.PassWord = "1234";  //默认密码
                model.NiceName = STCD;
                bool b = PublicBD.db.AddRTU(model);     //添加 
                /////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////

                if (b)
                {
                    Service.ServiceBussiness.RtuList.Add(new Service.Model.YY_RTU_Basic() { STCD = STCD, NiceName = STCD, PassWord = "1234" });
                    //水文协议新添加，为处理多包
                    //PackageProcess.CurePackageList.Add(STCD, new InPackage());
                }

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
                GsmService.GsmServer GS = Server as GsmService.GsmServer;
                List<GsmService.GsmMobile> Gs = GS.Gs;
                var gsms = from g in Gs where g.STCD == STCD select g;
                if (gsms.Count() == 0)
                {
                    GsmMobile gs = new GsmMobile() { STCD = STCD };
                    Gs.Add(gs);
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
        public void InsertGsmMobile(GsmServer GS,string Mobile, string STCD)
        {
            var temp = from g in GS.Gs where g.MOBILE == Mobile && g.STCD ==STCD  select g;
            if (temp.Count() == 0) 
            {
                Service.Model.YY_RTU_CONFIGDATA Model = new Service.Model.YY_RTU_CONFIGDATA();
                Model.STCD = STCD;
                Model.ItemID = "0000000000";
                Model.ConfigID = "11000000AA07";
                Model.ConfigVal = Mobile;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '11000000AA07'");
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
                Model.ConfigID = "11000000AA08";
                Model.ConfigVal = Satellite;

                Service.PublicBD.db.DelRTU_ConfigData("where STCD='" + STCD + "' and ConfigID = '11000000AA08'");
                Service.PublicBD.db.AddRTU_ConfigData(Model);
            }
        }
    }
}
