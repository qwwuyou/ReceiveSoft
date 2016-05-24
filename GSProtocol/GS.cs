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

namespace Service
{
    //*************************************************************************//
    // Rtu短信接收召测处理机制
    //1、每30秒读取一次卡中信息（没有用监听事件）
    //2、读取到信息后顺序执行并回复至中心站
    //3、直到读取并处理完最后一条信息，再重复执行30秒一次的循环
    //注：没有传感器时提取实时数据比较慢（重试3次）、提取固态快于提取实时数据
    //*************************************************************************//
    public class GS:DataProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");
        

        /// <summary>
        /// 包路由器方法(从RTU收)
        /// </summary>
        /// <param name="pack"></param>
        public void PacketArrived(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server) 
        {
            try 
            {
                switch (solution.C)
                {
                    case "0001": //101
                        PackageProcess.Process_01(solution, NFOINDEX, Server);
                        break;
                    case "0002":
                        PackageProcess.Process_02(solution, NFOINDEX, Server);
                        break;
                    case "0003":
                        PackageProcess.Process_03(solution, NFOINDEX, Server);
                        break;
                    case "0005":
                        PackageProcess.Process_05(solution, NFOINDEX, Server);
                        break;
                    case "0006":
                        PackageProcess.Process_06(solution, NFOINDEX, Server);
                        break;
                    case "0007":
                        PackageProcess.Process_07(solution, NFOINDEX, Server);
                        break;
                    default:
                        break;
                }

                //接收到命令的回复
                CommandReply(solution, NFOINDEX, Server);
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
            }
        }

        //接收到命令的回复
        private void CommandReply(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            try
            {
                string STCD = SetSTCD(solution, NFOINDEX, Server);
                string C = solution.C;

                #region TCP
                if ((int)NFOINDEX == 1)
                {
                    TcpService.TcpServer TS = Server as TcpService.TcpServer;
                    if (C == "0005" || C == "0006" || C == "0007")
                    {
                        string COMMANDCODE="0069";
                        //接收的数据属于下发召测命令的回复                                       
                        var command = from c in TS.TQ.Qtsd where c.STCD == STCD && COMMANDCODE == c.COMMANDCODE select c;
                        List<TcpSendData> Command = command.ToList<TcpSendData>();
                        if (Command.Count() > 0)
                        {
                            byte[] Data = Command.First().Data;
                            string data =Encoding.ASCII.GetString(Data);
                            foreach (var item in ServiceControl.tcp)
                            {
                                TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                            }

                            DateTime datetime = DateTime.Now;

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
                    else if(C =="0003")
                    {
                        string COMMANDCODE="0067";
                        //接收的数据属于下发召测命令的回复                                       
                        var command = from c in TS.TQ.Qtsd where c.STCD == STCD && COMMANDCODE == c.COMMANDCODE select c;
                        List<TcpSendData> Command = command.ToList<TcpSendData>();
                        if (Command.Count() > 0)
                        {
                            byte[] Data = Command.First().Data;
                            string data = Encoding.ASCII.GetString(Data);
                            foreach (var item in ServiceControl.tcp)
                            {
                                TcpService.TcpBussiness.RemoveTsdQ(item, STCD, COMMANDCODE);
                            }

                            DateTime datetime = DateTime.Now;

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

                }
                #endregion
                #region UDP
                else if ((int)NFOINDEX == 2)
                {
                    UdpService.UdpServer US = Server as UdpService.UdpServer;
                    if (C == "0005" || C == "0006" || C == "0007")
                    {
                        string COMMANDCODE = "0069";
                        //接收的数据属于下发召测命令的回复                                                               
                        var command = from c in US.UQ.Qusd where c.STCD == STCD && COMMANDCODE == c.COMMANDCODE select c;
                        List<UdpSendData> Command = command.ToList<UdpSendData>();
                        if (Command.Count() > 0)
                        {
                            byte[] Data = Command.First().Data;
                            string data = Encoding.ASCII.GetString(Data);

                            foreach (var item in ServiceControl.udp)
                            {
                                UdpService.UdpBussiness.RemoveUsdQ(item, STCD, COMMANDCODE);
                            }
                            DateTime datetime = DateTime.Now;


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
                    else if (C == "0003")
                    {
                        string COMMANDCODE = "0067";
                        //接收的数据属于下发召测命令的回复                                       
                        var command = from c in US.UQ.Qusd where c.STCD == STCD && COMMANDCODE == c.COMMANDCODE select c;
                        List<UdpSendData> Command = command.ToList<UdpSendData>();
                        if (Command.Count() > 0)
                        {
                            byte[] Data = Command.First().Data;
                            string data = Encoding.ASCII.GetString(Data);

                            foreach (var item in ServiceControl.udp)
                            {
                                UdpService.UdpBussiness.RemoveUsdQ(item, STCD, COMMANDCODE);
                            }
                            DateTime datetime = DateTime.Now;


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
                }
                #endregion
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + "接收召测命令操作异常！", ex);
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
                        
                        string data =　Encoding.ASCII .GetString(Data);  

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
          

        }

        //tcp发送命令逻辑执行
        void StartCommand(System.Threading.Thread thread, TcpService.TcpServer TS, TcpService.TcpSendData tsd)
        {
            try
            {
                #region
                int count = 2;
                while (count < 3)
                {
                    DateTime datetime = DateTime.Now;
                    //判断命令列表里面有没有命令
                    var command = from com in TS.TQ.Qtsd where com.STCD == tsd.STCD && tsd.COMMANDCODE == com.COMMANDCODE select com;
                    List<TcpSendData> Command = command.ToList<TcpSendData>();
                    var tcp = from t in TS.Ts where t.STCD == tsd.STCD && t.TCPSOCKET != null select t;
                    List<TcpSocket> TCP = tcp.ToList<TcpSocket>();
                    if (Command.Count() > 0 && TCP.Count() > 0)
                    {
                        string STCD = Command.First().STCD;
                        string COMMANDCODE = Command.First().COMMANDCODE;
                        byte[] Data = Command.First().Data; //Data ASC

                       
                        string data =　Encoding.ASCII .GetString(Data);

                        //判断有没有在线设备
                        TCP.First().TCPSOCKET.Send(tsd.Data);

                        Command.First().STATE = count + 1;
                        TCP.First().DATATIME = datetime;//更新为当前时间
                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.TCP.ToString(), TS.ServiceID, STCD, "发送召测命令", Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        //写入本地命令列表
                        ServiceBussiness.WriteListCommand(STCD, Service.ServiceEnum.NFOINDEX.TCP, COMMANDCODE, data, datetime, count + 1);
                        //单条命令状态发送到界面程序
                        ServiceBussiness.CommandWriteQUIM(STCD, Service.ServiceEnum.NFOINDEX.TCP.ToString(), COMMANDCODE, data, datetime, count + 1);



                        System.Threading.Thread.Sleep(60 * 1000);//可设置
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


        public void SendCommand(GsmService.GsmServer GS)
        {
            lock (GS)//避免与接收冲突
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
                    byte[] Data = Command.First().Data;  //Data ASC
                    try
                    {
                        string data = Encoding.ASCII.GetString(Data);

                        lock(GS)
                        //**重要：SendData(mobile,byte[]) byte[]必须为Hex（原来为水资源修改的短信发送方法）**//
                        GS.SendData(GSM.First().MOBILE, EnCoder.HexStrToByteArray(data));


                        GSM.First().DATATIME = datetime;//更新为当前时间

                        //通知界面
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.GSM.ToString(), GS.ServiceID, STCD, "发送召测命令", Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

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
                        //System.Threading.Thread.Sleep(30 * 1000);

                        datetime = DateTime.Now;
                        List<Command> lc = ServiceBussiness.RemoveListCommand(STCD, Service.ServiceEnum.NFOINDEX.GSM, COMMANDCODE, -2);
                        foreach (var item in lc)
                        {
                            PublicBD.db.AddDataCommand(item.STCD, item.CommandID, item.DATETIME, datetime, EnCoder.ByteArrayToHexStr(Data), (int)Service.ServiceEnum.NFOINDEX.GSM, -2);
                        }
                    }
                    catch
                    {
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.GSM.ToString(), GS.ServiceID, STCD, "发送召测命令失败", Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }



                }
            }
            else
            {
                //通知界面
                ServiceBussiness.WriteQUIM("GSM", GS.ServiceID, gsd.STCD, "测站[" + gsd.STCD + "]没有设置手机号", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

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
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, STCD, "发送召测命令", Command.First().Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

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
                        ServiceBussiness.WriteQUIM(Service.ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, STCD, "发送召测命令失败", Command.First().Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }



                }
            }
            else
            {
                //通知界面
                ServiceBussiness.WriteQUIM("COM", CS.ServiceID, csd.STCD, "测站[" + csd.STCD + "]没有设置卫星号", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);

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

        #region 包处理
        public void PacketArrived(UdpService.UdpServer US)
        {
            //throw new NotImplementedException();
            string Port = "00000";// US.PORT.ToString().PadLeft(5, '0');

            string ServiceId = US.ServiceID;
            ConcurrentQueue<UdpReceivedData> Qurd = US.UQ.Qurd;
            List<UdpSocket> Us = US.Us;
            ConcurrentQueue<UdpSendData> Qtsd = US.UQ.Qusd;
            DLYY.Protocol.IProtocol<DLYY.Model.WaterLevelInfo> waterLevel;
            try
            {
                waterLevel = DLYY.Protocol.Factory<DLYY.Model.WaterLevelInfo>.Instance("DLYY.Protocol", string.Format("{0}.{1}", "DLYY.Protocol", "WaterLevel"));
            }
            catch (Exception ex)
            { log.Error(DateTime.Now + "包加载异常" + ex.ToString()); return; }

            while (Qurd.Count > 0)
            {
                UdpReceivedData item = null;
                Qurd.TryDequeue(out item);
                if (item != null)
                {
                   
                    //注册&透传
                    Service.ServiceBussiness.RemoteCommand(item.Data);
                    //Service.ServiceBussiness.Registered30(item.Data);

                    string strPacket = Encoding.ASCII.GetString(item.Data);// sb.ToString();

                    List<string> Pack = SplitPack(strPacket);
                    if (Pack != null)
                    {
                        foreach (var pack in Pack)
                        {
                            #region
                            string STCD = "";
                            try
                            {
                                bool B = false;

                                #region 心跳 & 新将测站信息入库
                                if (pack.Substring(0, 1) == "$")
                                {
                                    //F2F200EE00050002000B323133343536
                                    STCD = Port + pack.Substring(1, 3).PadLeft(5, '0');
                                    //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.UDP, US);

                                    UdpBussiness.UpdSocket(US, item.IpEndPoint, STCD, out B);
                                    if (!B)
                                    {
                                        //上线
                                        UdpBussiness.UdpConnected(US, STCD);

                                    }


                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, STCD, "接收到心跳" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                                    //回复心跳
                                    byte[] data = Encoding.ASCII.GetBytes("T030");
                                    US.UDPClient.Send(data, data.Length, item.IpEndPoint);
                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, STCD, "回复心跳", data, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                                    //ServiceBussiness.WriteQDM(item.Data); //写入透明传输列表
                                    //return;
                                }
                                #endregion
                                #region 数据
                                else
                                {
                                    //解包
                                    DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution = waterLevel.Analysis(pack);
                                    STCD = Port + solution.STCD.PadLeft(5, '0');
                                    if (STCD == Port+"00000")
                                    {
                                        //(修改参数命令确认，不带stcd)根据socket得到stcd
                                        GetRtuSTCD(pack, item, US, solution);
                                        STCD = solution.STCD;
                                    }
                                    //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.UDP, US);
                                    //更新socket列表的stcd、socket
                                    B = false;
                                    UdpBussiness.UpdSocket(US, item.IpEndPoint, STCD, out B);
                                    if (!B)
                                    {
                                        //上线
                                        UdpBussiness.UdpConnected(US, STCD);

                                    }
                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, STCD, "接收数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                                    //包路由
                                    PacketArrived(solution, ServiceEnum.NFOINDEX.UDP, US);


                                    #region 推数异常
                                    try
                                    {
                                        //ToWeb(JsonHelper.JsonSerializer<List<DLYY.Model.WaterLevelInfo>>(solution.Items));
                                    }
                                    catch { ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, STCD, "推数异常", new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text); }
                                    #endregion
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, STCD, "接收异常数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                                log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                            }




                            #region 写入透明传输列表
                            ServiceBussiness.WriteQDM(item.Data);
                            #endregion
                            #endregion
                        }
                    }
                    else
                    {
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.UDP.ToString(), ServiceId, "[未知]", "分包失败，接收数据：", item.Data, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                    }
                }
            }
        }

        private static void ToWeb(string Json)
        {
            System.Net.WebClient mywebclient = new System.Net.WebClient();
            byte[] cheklist = mywebclient.DownloadData("http://192.168.40.41:8077/handler/ToCometHandler.ashx?content=" + Json);
            string strcheck = Encoding.ASCII.GetString(cheklist);

        }
        public void PacketArrived(TcpService.TcpServer TS)
        {
            //throw new NotImplementedException();
            string Port = "00000";// TS.PORT.ToString().PadLeft(5,'0');
            string ServiceId = TS.ServiceID;
            ConcurrentQueue<TcpReceivedData> Qtrd = TS.TQ.Qtrd;
            List<TcpSocket> Ts = TS.Ts;
            ConcurrentQueue<TcpSendData> Qtsd = TS.TQ.Qtsd;
            DLYY.Protocol.IProtocol<DLYY.Model.WaterLevelInfo> waterLevel;
            try
            {
                waterLevel = DLYY.Protocol.Factory<DLYY.Model.WaterLevelInfo>.Instance("DLYY.Protocol", string.Format("{0}.{1}", "DLYY.Protocol", "WaterLevel"));
            }
            catch (Exception ex)
            { log.Error(DateTime.Now + "包加载异常" + ex.ToString()); return; }

            while (Qtrd.Count > 0)
            {
                TcpReceivedData item = null;
                Qtrd.TryDequeue(out item);
                if (item != null)
                {
                    //注册&透传
                    Service.ServiceBussiness.RemoteCommand(item.Data);
                    //Service.ServiceBussiness.Registered30(item.Data);
                    

                    string strPacket = Encoding.ASCII.GetString(item.Data);// sb.ToString();
                    List<string> Pack = SplitPack(strPacket);
                    if (Pack != null)
                    {
                        foreach (var pack in Pack)
                        {
                            #region
                            string STCD = "";
                            try
                            {
                                bool B = false;

                                #region 心跳 & 新将测站信息入库
                                if (pack.Substring(0, 1) == "$")
                                {
                                    //F2F200EE00050002000B323133343536
                                    STCD =Port+ pack.Substring(1, 3).PadLeft(5, '0');
                                    //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);

                                    TcpBussiness.UpdSocket(TS, item.SOCKET, STCD, out B);
                                    if (!B)
                                    {
                                        //上线
                                        TcpBussiness.TcpConnected(TS, STCD);
                                    }
                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, STCD, "接收到心跳" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                                    //回复心跳
                                    item.SOCKET.Send(Encoding.ASCII.GetBytes("T030"));
                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, STCD, "回复心跳", Encoding.ASCII.GetBytes("T030"), ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                                    //ServiceBussiness.WriteQDM(item.Data); //写入透明传输列表
                                    //return;
                                }
                                #endregion
                                #region 数据
                                else
                                {
                                    //解包
                                    DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution = waterLevel.Analysis(pack);
                                    STCD =Port+ solution.STCD.PadLeft(5, '0'); 
                                    if (STCD ==Port+ "00000")
                                    {
                                        //(修改参数命令确认，不带stcd)根据socket得到stcd
                                        GetRtuSTCD(pack, item, TS, solution);
                                        STCD = solution.STCD;
                                    }
                                    //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                                    InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.TCP, TS);

                                    //更新socket列表的stcd、socket
                                    B = false;
                                    TcpBussiness.UpdSocket(TS, item.SOCKET, STCD, out B);
                                    if (!B)
                                    {
                                        //上线
                                        TcpBussiness.TcpConnected(TS, STCD);
                                    }
                                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, STCD, "接收数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                                    //包路由
                                    PacketArrived(solution, ServiceEnum.NFOINDEX.TCP, TS);
                                    
                                    #region 推数异常
                                    try
                                    {
                                        //ToWeb(JsonHelper.JsonSerializer<List<DLYY.Model.WaterLevelInfo>>(solution.Items));
                                    }
                                    catch { ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, STCD, "推数异常", new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text); }
                                    #endregion
                                }
                                #endregion

                            }
                            catch (Exception ex)
                            {
                                ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, STCD, "接收异常数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                                log.Error(DateTime.Now + "包处理操作异常" + ex.ToString());
                            }



                            #region 写入透明传输列表
                            ServiceBussiness.WriteQDM(item.Data);
                            #endregion
                            #endregion
                        }
                    }
                    else
                    {
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.TCP.ToString(), ServiceId, "[未知]", "分包失败，接收数据：", item.Data , ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);
                    }
                  
                }
                    
            }
        }

        public void PacketArrived(GsmService.GsmServer GS)
        {
            string ServiceId = GS.ServiceID;
            ConcurrentQueue<GsmReceivedData> Qgrd = GS.GQ.Qgrd;
            List<GsmMobile> Gs = GS.Gs;
            ConcurrentQueue<GsmSendData> Qgsd = GS.GQ.Qgsd;

            DLYY.Protocol.IProtocol<DLYY.Model.WaterLevelInfo> waterLevel;
            try
            {
                waterLevel = DLYY.Protocol.Factory<DLYY.Model.WaterLevelInfo>.Instance("DLYY.Protocol", string.Format("{0}.{1}", "DLYY.Protocol", "WaterLevel"));
            }
            catch (Exception ex)
            { log.Error(DateTime.Now + "包加载异常" + ex.ToString()); return; }

            while (Qgrd.Count > 0)
            {
                GsmReceivedData item = null;
                Qgrd.TryDequeue(out item);
                if (item != null)
                {
                    string STCD = "";
                    string pack = "";
                    try
                    {
                        pack = Encoding.ASCII.GetString(item.Data);
                        //解包
                        DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution = waterLevel.Analysis(pack);
                        STCD = solution.STCD.PadLeft(10, '0');
                        if (STCD == "0000000000")
                        {
                            //(修改参数命令确认，不带stcd)根据socket得到stcd
                            GetRtuSTCD(pack, item,GS, solution);
                            STCD = solution.STCD;
                        }
                        
                        //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                        InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.GSM, GS);
                        //列表中如果不存在手机号，先写入数据库
                        InsertGsmMobile(GS, item.MOBILE, STCD);
                        //更新mobile列表的stcd、mobile
                        GsmBussiness.UpdMobile(GS, item.MOBILE, STCD);
                        
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.GSM.ToString(), ServiceId, STCD, "接收数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                        //包路由
                        PacketArrived(solution, ServiceEnum.NFOINDEX.GSM, GS);

                        #region 推数异常
                        try
                        {
                            //ToWeb(JsonHelper.JsonSerializer<List<DLYY.Model.WaterLevelInfo>>(solution.Items));
                        }
                        catch { ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.GSM.ToString(), ServiceId, STCD, "推数异常", new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text); }
                        #endregion
                    }
                    catch
                    {
                        //通知界面
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "接收异常数据", item.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                        ServiceBussiness.WriteQUIM("GSM", ServiceId, "", "尝试中文数据转码[" + GS.TrySmsContent + "]", new byte[] { }, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }

                    
                    #region 写入透明传输列表 gsm_OnReceivedData事件中已经写入，因此注销以下代码
                    //ServiceBussiness.WriteQDM(item.Data);
                    #endregion
                }
            }
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            string ServiceId = CS.ServiceID;
            ConcurrentQueue<ComReceivedData> Qcrd = CS.CQ.Qcrd;
            List<ComSatellite > Cs = CS.Cs;
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;

            DLYY.Protocol.IProtocol<DLYY.Model.WaterLevelInfo> waterLevel;
            try
            {
                waterLevel = DLYY.Protocol.Factory<DLYY.Model.WaterLevelInfo>.Instance("DLYY.Protocol", string.Format("{0}.{1}", "DLYY.Protocol", "WaterLevel"));
            }
            catch (Exception ex)
            { log.Error(DateTime.Now + "包加载异常" + ex.ToString()); return; }

            while (Qcrd.Count > 0)
            {
                ComReceivedData item = null;
                Qcrd.TryDequeue(out item);
                if (item != null)
                {
                    string STCD = "";
                    string pack = "";
                    try
                    {
                        pack = Encoding.ASCII.GetString(item.Data);
                        //解包
                        DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution = waterLevel.Analysis(pack);
                        STCD = solution.STCD.PadLeft(10, '0');
                        if (STCD == "0000000000")
                        {
                            //(修改参数命令确认，不带stcd)根据socket得到stcd
                            GetRtuSTCD(pack, item, CS, solution);
                            STCD = solution.STCD;
                        }

                        //接收数据时收到未设置测站信息，自动将测站信息入库并在列表中添加
                        InsertNewSTCD(STCD, Service.ServiceEnum.NFOINDEX.COM, CS);
                        //列表中如果不存在手机号，先写入数据库
                        InsertComMobile(CS, item.SATELLITE, STCD);
                        //更新mobile列表的stcd、mobile
                        ComBussiness.UpdSatellite(CS, item.SATELLITE, STCD);

                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, STCD, "接收数据" + pack, new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text);

                        //包路由
                        PacketArrived(solution, ServiceEnum.NFOINDEX.COM, CS);

                        #region 推数异常
                        try
                        {
                            //ToWeb(JsonHelper.JsonSerializer<List<DLYY.Model.WaterLevelInfo>>(solution.Items));
                        }
                        catch { ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), ServiceId, STCD, "推数异常", new byte[] { }, ServiceEnum.EnCoderType.ASCII, ServiceEnum.DataType.Text); }
                        #endregion
                    }
                    catch
                    {
                        //通知界面
                        ServiceBussiness.WriteQUIM("COM", ServiceId, "", "接收异常数据", item.Data, Service.ServiceEnum.EnCoderType.ASCII, Service.ServiceEnum.DataType.Text);
                    }


                    #region 写入透明传输列表 gsm_OnReceivedData事件中已经写入，因此注销以下代码
                    //ServiceBussiness.WriteQDM(item.Data);
                    #endregion
                }
            }
        }

        //根据socket得到stcd(修改参数命令确认，不带stcd)
        private void GetRtuSTCD(string command, TcpReceivedData item, TcpService.TcpServer TS, DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution) 
        {
            if (command.Length == 16 && (command.Substring(8, 8) == "00010005" || command.Substring(8, 8) == "00010007")) 
            {
                var rtu = from r in TS.Ts where r.TCPSOCKET == item.SOCKET select r;
                if (rtu.Count() > 0) 
                {
                    solution.STCD= rtu.First().STCD;
                }                
            }
            
        }
        private void GetRtuSTCD(string command, UdpReceivedData item, UdpService.UdpServer US, DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution)
        {
            if (command.Length == 16 && (command.Substring(8, 8) == "00010005"|| command.Substring(8, 8) == "00010007"))
            {
                var rtu = from r in US.Us where r.IpEndPoint == item.IpEndPoint select r;
                if (rtu.Count() > 0)
                {
                    solution.STCD = rtu.First().STCD;
                }
            }

        }
        private void GetRtuSTCD(string command,GsmReceivedData item, GsmService.GsmServer GS, DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution)
        {
            if (command.Length == 16 && (command.Substring(8, 8) == "00010005" || command.Substring(8, 8) == "00010007"))
            {
                var rtu = from r in GS.Gs where r.MOBILE == item.MOBILE  select r;
                if (rtu.Count() > 0)
                {
                    solution.STCD = rtu.First().STCD;
                }
            }

        }
        private void GetRtuSTCD(string command, ComReceivedData item, ComService.ComServer CS, DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution)
        {
            if (command.Length == 16 && (command.Substring(8, 8) == "00010005" || command.Substring(8, 8) == "00010007"))
            {
                var rtu = from r in CS.Cs where r.SATELLITE == item.SATELLITE select r;
                if (rtu.Count() > 0)
                {
                    solution.STCD = rtu.First().STCD;
                }
            }

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
                model.PassWord = "0";
                model.NiceName = STCD;
                bool b = PublicBD.db.AddRTU(model);     //添加 
                if (b)
                    Service.ServiceBussiness.RtuList.Add(new Service.Model.YY_RTU_Basic() { STCD = STCD, NiceName = STCD, PassWord = "0" });

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

        //燕禹协议分包方法
        private List<string> SplitPack(string Pack)
        {
            try
            {
                List<string> Packs = new List<string>();
                if (Pack.Contains('$'))
                {
                    string[] packs = Pack.Split(new char[] { '$' }, StringSplitOptions.None);
                    foreach (var item in packs)
                    {
                        if (item != "")
                        {
                            if (item.Length == 3)
                            {
                                Packs.Add("$" + item);
                            }
                            else if (item.Length > 3)
                            {
                                Packs.Add("$" + item.Substring(0,3));
                                string temp = item.Substring(3, item.Length - 3);
                                string[] packstemp = temp.Split(new string[] { "F2F2" }, StringSplitOptions.None);
                                foreach (var pt in packstemp)
                                {
                                    if (pt != "")
                                    {
                                        Packs.Add("F2F2" + pt);
                                    }
                                }
                            }
                        }
                    }
                    return Packs;
                }
                else 
                {
                    if (Pack.IndexOf("F2F2") != -1)
                    {
                        string[] packstemp = Pack.Split(new string[] { "F2F2" }, StringSplitOptions.None);
                        foreach (var pt in packstemp)
                        {
                            if (pt != "")
                            {
                                Packs.Add("F2F2" + pt);
                            }
                        }
                    }
                    else 
                    {
                        Packs.Add(Pack);
                    }
                    return Packs;
                }
                
            }
            catch { return null; }
        }

        //设置站号 tcp、udp 端口+00+编号  gsm、com 0000000+编号
        private string SetSTCD(DLYY.Protocol.Solution<DLYY.Model.WaterLevelInfo> solution, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            string STCD = solution.STCD.PadLeft(10, '0');
            if ((int)NFOINDEX == 1) 
            {
                TcpService.TcpServer TS = Server as TcpService.TcpServer;
                STCD = TS.PORT.ToString().PadLeft(5, '0') + solution.STCD.PadLeft(5, '0');
            }
            if ((int)NFOINDEX == 2)
            {
                UdpService.UdpServer US = Server as UdpService.UdpServer;
                STCD = US.PORT.ToString().PadLeft(5, '0') + solution.STCD.PadLeft(5, '0');
            }
            return STCD;
        }
    }
}

