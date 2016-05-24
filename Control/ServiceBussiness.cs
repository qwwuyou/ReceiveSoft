using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;
using UdpService;
using ComService;
using TcpService;
using GsmService;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Service
{
    public class ServiceBussiness
    {

        /// <summary>
        /// 写入通知界面列表
        /// </summary>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="ServiceId">服务id</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="Explain">说明</param>
        /// <param name="Data">数据</param>
        /// <param name="DataType">数据类型 1、明文  2、状态报(自产生)</param>
        public static void WriteQUIM(string ServiceType, string ServiceId, string STCD, string Explain, byte[] Data, ServiceEnum.EnCoderType Encoder, Service.ServiceEnum.DataType DataType)
        {
            UIModel UIm = new UIModel();
            UIm.SERVICEID = ServiceId;
            UIm.STCD = STCD;
            UIm.DataType = (int)DataType;

            string dataStr = ""; 
            string datetime = DateTime.Now.ToString("MM-dd HH:mm:ss");

            if ((int)DataType == 1 )
            {
                string name = "[未知]";
                if (ServiceBussiness.RtuList != null && ServiceBussiness.RtuList.Count > 0)
                {
                    var Rtu = from rtu in ServiceBussiness.RtuList where rtu.STCD == STCD select rtu;
                    List<YY_RTU_Basic> RTU = Rtu.ToList<YY_RTU_Basic>();
                    if (RTU.Count() > 0)
                    {
                        name = "[" + RTU.First().NiceName + "]";
                    }
                }
                if (Data.Length > 0)
                {
                    if (STCD != null && STCD != "")
                    { STCD = "(" + STCD + ") "; }

                    //发送到界面的数据按照？格式编码
                    if ((int)Encoder == 1)
                    { dataStr = EnCoder.ByteArrayToHexStr(Data); }
                    else if ((int)Encoder == 2)
                    { dataStr = Encoding.ASCII.GetString(Data); }


                    UIm.EXPLAIN = "++" + datetime + " " + ServiceType + " 服务:" + ServiceId + " " + name + STCD + Explain + ":" + dataStr + "\n";
                }
                else
                {
                    if (STCD != null && STCD != "")
                    { STCD = "(" + STCD + ") "; }
                    UIm.EXPLAIN = "++" + datetime + " " + ServiceType + " 服务:" + ServiceId + " " + name + STCD + Explain + "\n"; 
                }
                UIm.Data = Data;

            }
            else
            {
                UIm.EXPLAIN = "--" + Explain + "\n";
            }

            //lock (ServiceQueue.QUIM)
            {
                ServiceQueue.QUIM.Enqueue(UIm);
                //if (ServiceQueue.QUIM.Count > 500)//控制数据集合最多不超过500条
                //{
                //    UIm = new UIModel();
                //    //ServiceQueue.QUIM.TryDequeue(out UIm);
                //    UIm = ServiceQueue.QUIM.Dequeue();
                //}
            }

        }

        //public static void WriteQUIM()
        //{
        //    UIModel UIm = new UIModel();
        //    UIm.EXPLAIN = rtf;
        //    UIm.DataType = 1;
        //    ServiceQueue.QUIM.Enqueue(UIm);
        //}

        #region 从UI接收命令后
        /// <summary>
        /// 将从界面接收的召测报解析分发至各服务的发送列表
        /// </summary>
        /// <param name="RevData">从界面接收的召测报</param>
        /// <param name="sc"></param>
        public static void WriteQxsd(byte[] RevData, ServiceControl sc) //++tcp|数据报|stcd|命令码
        {
            string temp = Encoding.UTF8.GetString(RevData);
            string[] data = temp.Split(new string[] { "\n" }, StringSplitOptions.None);

            string STCD = null;
            string ServiceType = null;
            string CommandID = null;
            string Data = null;
            int State = 0;
            DateTime datetime = DateTime.Now;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != "" && data[i].Length > 6)
                {
                    //Console.WriteLine(data[i]);

                    #region 接收的命令为召测命令相关
                    if (data[i].Substring(0, 6) == "++tcp|")
                    {
                        data[i] = data[i].Replace("++tcp|", "");
                        string[] temps = data[i].Split(new char[] { '|' });

                        STCD = temps[1];
                        ServiceType = ServiceEnum.NFOINDEX.TCP.ToString();
                        CommandID = temps[2];
                        Data = temps[0];
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(Data);
                        }
                        #endregion

                        foreach (var tcp in ServiceControl.tcp)
                        {
                            TcpBussiness.WriteTsdQ(tcp, STCD, EncoderData, CommandID);
                            //写入命令列表
                            WriteListCommand(STCD, ServiceEnum.NFOINDEX.TCP, CommandID, Data, datetime, State);



                            //单条命令状态发送到界面程序
                            CommandWriteQUIM(STCD, ServiceType, CommandID, Data, datetime, State);
                        }
                    }
                    else if (data[i].Substring(0, 6) == "++udp|")
                    {
                        data[i] = data[i].Replace("++udp|", "");
                        string[] temps = data[i].Split(new char[] { '|' });

                        STCD = temps[1];
                        ServiceType = ServiceEnum.NFOINDEX.UDP.ToString(); ;
                        CommandID = temps[2];
                        Data = temps[0];
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(Data);
                        }
                        #endregion
                        foreach (var udp in ServiceControl.udp)
                        {
                            UdpBussiness.WriteUsdQ(udp, STCD, EncoderData, CommandID);
                            //写入命令列表
                            WriteListCommand(STCD, ServiceEnum.NFOINDEX.UDP, CommandID, Data, datetime, State);

                            //单条命令状态发送到界面程序
                            CommandWriteQUIM(STCD, ServiceType, CommandID, Data, datetime, State);
                        }
                    }
                    else if (data[i].Substring(0, 6) == "++gsm|")
                    {
                        data[i] = data[i].Replace("++gsm|", "");
                        string[] temps = data[i].Split(new char[] { '|' });

                        STCD = temps[1];
                        ServiceType = ServiceEnum.NFOINDEX.GSM.ToString();
                        CommandID = temps[2];
                        Data = temps[0];
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(Data);
                        }
                        #endregion
                        foreach (var gsm in ServiceControl.gsm)
                        {
                            GsmBussiness.WriteGsdQ(gsm, STCD, EncoderData, CommandID);

                            //写入命令列表
                            WriteListCommand(STCD, ServiceEnum.NFOINDEX.GSM, CommandID, Data, datetime, State);

                            //单条命令状态发送到界面程序
                            CommandWriteQUIM(STCD, ServiceType, CommandID, Data, datetime, State);

                        }
                    }
                    else if (data[i].Substring(0, 6) == "++com|")
                    {
                        data[i] = data[i].Replace("++com|", "");
                        string[] temps = data[i].Split(new char[] { '|' });

                        STCD = temps[1];
                        ServiceType = ServiceEnum.NFOINDEX.COM.ToString();
                        CommandID = temps[2];
                        Data = temps[0];

                        foreach (var com in ServiceControl.com)
                        {
                            ComBussiness.WriteCsdQ(com, STCD, EnCoder.HexStrToByteArray(Data), CommandID);
                            //写入命令列表
                            WriteListCommand(STCD, ServiceEnum.NFOINDEX.COM, CommandID, Data, datetime, State);
                            //单条命令状态发送到界面程序
                            CommandWriteQUIM(STCD, ServiceType, CommandID, Data, datetime, State);
                        }
                    }
                    else if (data[i].Substring(0, 6) == "--cmd|")
                    {
                        #region 所有命令删除
                        if (data[i].Length == 11)
                        {
                            if (data[i].ToLower() == "--cmd|clear")
                            {
                                //清空所有信道的所有召测命令
                                ClearListCommand();
                            }
                        }
                        #endregion

                        #region 单命令删除
                        //删除服务器端列表中的召测命令     --cmd|tcp|0012345679|02
                        data[i] = data[i].Replace("--cmd|", "");
                        string[] temps = data[i].Split(new char[] { '|' });
                        ServiceEnum.NFOINDEX nfoindex = ServiceEnum.NFOINDEX.TCP;
                        if (temps[0].ToLower() == "tcp")
                        { nfoindex = ServiceEnum.NFOINDEX.TCP; }
                        else if (temps[0].ToLower() == "udp")
                        { nfoindex = ServiceEnum.NFOINDEX.UDP; }
                        else if (temps[0].ToLower() == "gsm")
                        { nfoindex = ServiceEnum.NFOINDEX.GSM; }
                        else if (temps[0].ToLower() == "com")
                        { nfoindex = ServiceEnum.NFOINDEX.COM; }
                        RemoveListCommand(temps[1], nfoindex, temps[2]);
                        #endregion
                    }
                    else if (data[i].Substring(0, 6) == "--pro|")
                    {
                        #region 设置信道并重启服务
                        data[i] = data[i].Replace("--pro|", "");
                        SetProtocol(data[i]);
                        System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ReBootService));
                        thread.Start(sc);
                        #endregion
                    }
                    //服务端的xml同步到界面端
                    else if (data[i].Substring(0, 7) == "--file|")
                    {
                        data[i] = data[i].Replace("--file|", "");
                        if (data[i].Length > 0)
                        {
                            SendXMLFile(data[i]);
                        }
                    }
                    //客户端xml信息配置到服务端
                    else if (data[i].Substring(0, 7) == "--File|")
                    {
                        data[i] = data[i].Replace("--File|", "");
                        if (data[i].Length > 0)
                        {
                            WriteXMLFile(data[i]);
                            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ReBootService));
                            thread.Start(sc);
                        }
                    }
                    #endregion

                    #region 提取卫星状态、信息时间
                    if (data[i].Length == 9 && data[i] == "--com|sta")
                    {
                        //原版本卫星协议
                        //ComBussiness.GetComState();
                        //新版本卫星协议4.0
                        ComBussiness.GetComStateFor4();
                    }
                    if (data[i].Length == 9 && data[i] == "--com|tim")
                    {
                        //原版本卫星协议
                        //ComBussiness.GetComTime();
                        //新版本卫星协议4.0
                        ComBussiness.GetComTimeFor4();
                    }
                    #endregion
                    
                    #region 接收重启某信道服务的命令
                    if (data[i].Substring(0, 6) == "--tcp|")
                    {
                        data[i] = data[i].Replace("--tcp|", "");
                        string[] temps = data[i].Split(new char[] { ':' });
                        for (int j = 0; j < temps.Length; j++)
                        {
                            if (temps[j] == "0")
                            {
                                ServiceBussiness sb = new ServiceBussiness();
                                int kk = j;

                                System.Threading.Thread thread = new System.Threading.Thread(() => sb.ReBootService(Service.ServiceEnum.NFOINDEX.TCP, kk));
                                thread.Start();
                            }
                        }
                    }
                    else if (data[i].Substring(0, 6) == "--udp|")
                    {
                        data[i] = data[i].Replace("--udp|", "");
                        string[] temps = data[i].Split(new char[] { ':' });
                        for (int j = 0; j < temps.Length; j++)
                        {
                            if (temps[j] == "0")
                            {
                                ServiceBussiness sb = new ServiceBussiness();
                                int kk = j;

                                System.Threading.Thread thread = new System.Threading.Thread(() => sb.ReBootService(Service.ServiceEnum.NFOINDEX.UDP, kk));
                                thread.Start();
                            }
                        }
                    }
                    else if (data[i].Substring(0, 6) == "--gsm|")
                    {
                        data[i] = data[i].Replace("--gsm|", "");
                        string[] temps = data[i].Split(new char[] { ':' });
                        for (int j = 0; j < temps.Length; j++)
                        {
                            if (temps[j] == "0")
                            {
                                ServiceBussiness sb = new ServiceBussiness();
                                int kk = j;

                                System.Threading.Thread thread = new System.Threading.Thread(() => sb.ReBootService(Service.ServiceEnum.NFOINDEX.GSM, kk));
                                thread.Start();
                            }
                        }
                    }
                    else if (data[i].Substring(0, 6) == "--com|")
                    {
                        data[i] = data[i].Replace("--com|", "");
                        string[] temps = data[i].Split(new char[] { ':' });
                        for (int j = 0; j < temps.Length; j++)
                        {
                            if (temps[j] == "0")
                            {
                                ServiceBussiness sb = new ServiceBussiness();
                                int kk = j;

                                System.Threading.Thread thread = new System.Threading.Thread(() => sb.ReBootService(Service.ServiceEnum.NFOINDEX.COM, kk));
                                thread.Start();
                            }
                        }
                    }
                    #endregion

                    #region BS询问某测站是否在线
                    if (data[i].Substring(0, 7) == "--stcd|")
                    {
                        data[i] = data[i].Replace("--stcd|", "");
                        string[] temps = data[i].Split(new char[] { ' ' });
                        ServiceEnum.NFOINDEX NFOINDEX = ServiceEnum.NFOINDEX.TCP;
                        if (temps[1].ToLower() == "tcp")
                        {
                            NFOINDEX = ServiceEnum.NFOINDEX.TCP;
                        }
                        else if (temps[1].ToLower() == "udp")
                        {
                            NFOINDEX = ServiceEnum.NFOINDEX.UDP;
                        }
                        string stcd = temps[0];
                        if (stcd.Length == 10)
                        {
                            ServiceBussiness sb = new ServiceBussiness();
                            System.Threading.Thread thread = new System.Threading.Thread(() => sb.GetRTUOnLine(stcd, NFOINDEX));
                            thread.Start();
                        }
                    }
                    #endregion
                }
                else if (data[i] != "" && data[i].Length == 6)
                {
                    //收到该命令发送所有命令列表
                    if (data[i].Substring(0, 6) == "--cmd|")
                        ServiceBussiness.SendCommandListState();
                    //重启服务命令
                    if (data[i].Substring(0, 6) == "--ser|")
                    {
                        System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ReBootService));
                        thread.Start(sc);
                    }
                    //发送邮件命令
                    if (data[i].Substring(0, 6) == "--mal|")
                    {
                        try
                        {
                            SendMail();
                        }
                        catch (Exception e)
                        {
                            TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                            TimeSpan ts = ts1.Subtract(ts2).Duration();
                            string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

                            ServiceControl.log.Error(DateTime.Now + "系统运行时长：" + dateDiff + " 接收到发送邮件命令，邮件发送失败！" + e.ToString());
                        }
                    }
                    //重新读取rtu信息
                    if (data[i].Substring(0, 6) == "--rtu|")
                    {
                        ServiceBussiness.GetRTUList();
                        WriteQUIM("", "Service", "", "服务已重新从数据库中读取RTU信息！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                    }
                    //远程注册
                    if (data[i].Substring(0, 6) == "--reg|")
                    {
                        //string path = @"***.exe";
                        //System.Diagnostics.Process.Start(path);
                        DeletEFileVerificationRegistration();
                    }
                }
            }
        }

        /// <summary>
        /// BS询问测站某信道是否在线
        /// </summary>
        /// <param name="STCD">测站编码</param>
        /// <param name="NFOINDEX">信道类型</param>
        private void GetRTUOnLine(string STCD, Service.ServiceEnum.NFOINDEX NFOINDEX) 
        {
            string OnlineState = "stcd|" ;
            bool ISOnLine = false;
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.UDP)
            {
                foreach (var item in ServiceControl.udp)
                {
                    var rtu = from r in item.Us where r.STCD == STCD && r.IpEndPoint != null select r;
                    if (rtu.Count() > 0)
                    {
                        ISOnLine = true;
                    }
                }

                if (ISOnLine)
                {
                    OnlineState += STCD + ":udp";
                }
                else 
                {
                    OnlineState += STCD + ":udp:";
                }
            }
            if (NFOINDEX == Service.ServiceEnum.NFOINDEX.TCP)
            {
                foreach (var item in ServiceControl.tcp)
                {
                    var rtu = from r in item.Ts where r.STCD == STCD && r.TCPSOCKET != null select r;
                    if (rtu.Count() > 0)
                    {
                        ISOnLine = true;
                    }
                }
                if (ISOnLine)
                {
                    OnlineState += STCD + ":tcp";
                }
                else
                {
                    OnlineState += STCD + ":tcp:";
                }
            }

           
            
            ServiceBussiness.WriteQUIM("", "", "", OnlineState, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
        
        }


        /// <summary>
        /// 重启服务
        /// </summary>
        private static void ReBootService(object sc)
        {
            ServiceControl.ReReadXML();

            ServiceBussiness.GetRTUList();  //重读RTU列表
            ServiceControl.LC = ServiceBussiness.GetCommandTempToLC();  //重读命令列表
            foreach (var tcp in Service.ServiceControl.tcp)
            {
                tcp.Stop();
            }
            foreach (var udp in Service.ServiceControl.udp)
            {
                udp.Stop();
            }
            foreach (var gsm in Service.ServiceControl.gsm)
            {
                gsm.Stop();
            }
            foreach (var com in Service.ServiceControl.com)
            {
                com.Stop();
            }
            SendEveryServcieState();
            System.Threading.Thread.Sleep(3 * 1000);
            //try
            //{
            //    foreach (var tcp in Service.ServiceControl.tcp)
            //    {
            //        tcp.Start();
            //    }
            //}
            //catch (Exception ex)
            //{ Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            //try
            //{
            //    foreach (var udp in Service.ServiceControl.udp)
            //    {
            //        udp.Start();
            //    }
            //}
            //catch (Exception ex)
            //{ Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            //try
            //{
            //    foreach (var gsm in Service.ServiceControl.gsm)
            //    {
            //        gsm.Start(); //该方法中重读了Mobile号
            //        Console.WriteLine(DateTime.Now + "远程命令重启服务，" + "Restart！");
            //    }
            //}
            //catch (Exception ex)
            //{ Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            //try
            //{
            //    foreach (var com in Service.ServiceControl.com)
            //    {
            //        com.Start();
            //    }
            //}
            //catch (Exception ex)
            //{ Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            (sc as ServiceControl).ChannelStart();

            //ResaveToHLJDB.ReResaveToHLJDB();
            Reflection_Resave.InitInfo();
            ServiceControl.AccessCenter();

            SendEveryServcieState();
        }
        /// <summary>
        /// 重启单个信道服务
        /// </summary>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="ServiceIndex">索引</param>
        private void ReBootService(Service.ServiceEnum.NFOINDEX NFOINDEX, int ServiceIndex)
        {
            ServiceBussiness.GetRTUList();  //重读RTU列表
            if (Service.ServiceEnum.NFOINDEX.TCP == NFOINDEX)
            {
                if (ServiceIndex <= Service.ServiceControl.tcp.Count()-1)
                {
                    Service.ServiceControl.tcp[ServiceIndex].Stop();
                    SendEveryServcieState();
                    System.Threading.Thread.Sleep(3 * 1000);
                    try
                    {
                        Service.ServiceControl.tcp[ServiceIndex].Start();
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                    SendEveryServcieState();
                }
            }
            else if (Service.ServiceEnum.NFOINDEX.UDP == NFOINDEX)
            {
                if (ServiceIndex <= Service.ServiceControl.udp.Count() - 1)
                {
                    Service.ServiceControl.udp[ServiceIndex].Stop();
                    SendEveryServcieState();
                    System.Threading.Thread.Sleep(3 * 1000);
                    try
                    {
                        Service.ServiceControl.udp[ServiceIndex].Start();
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                    SendEveryServcieState();
                }
            }
            else if (Service.ServiceEnum.NFOINDEX.COM == NFOINDEX)
            {
                if (ServiceIndex <= Service.ServiceControl.com.Count() - 1)
                {
                    Service.ServiceControl.com[ServiceIndex].Stop();
                    SendEveryServcieState();
                    System.Threading.Thread.Sleep(3 * 1000);
                    try
                    {
                        Service.ServiceControl.com[ServiceIndex].Start();
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                    SendEveryServcieState();
                }
            }
            else if (Service.ServiceEnum.NFOINDEX.GSM == NFOINDEX)
            {
                if (ServiceIndex <= Service.ServiceControl.gsm.Count() - 1)
                {
                    Service.ServiceControl.gsm[ServiceIndex].Stop();
                    SendEveryServcieState();
                    System.Threading.Thread.Sleep(3 * 1000);
                    try
                    {
                        Service.ServiceControl.gsm[ServiceIndex].Start();
                        Console.WriteLine(DateTime.Now + "远程命令重启单个信道服务，" + "Restart！");
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                    SendEveryServcieState();
                }
            }
        }

        /// <summary>
        /// 命令信息写入/更新命令列表(同时操作临时命令数据库表YY_COMMAND_TEMP)
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="CommandID">命令</param>
        /// <param name="Data">命令数据</param>
        /// <param name="datetime">时间</param>
        /// <param name="State">状态</param>
        public static void WriteListCommand(string STCD, ServiceEnum.NFOINDEX NFOINDEX, string CommandID, string Data, DateTime datetime, int State)
        {
            Command c = new Command();
            c.STCD = STCD;
            c.SERVICETYPE = NFOINDEX.ToString();
            c.CommandID = CommandID;
            c.Data = Data;
            c.DATETIME = datetime;
            c.STATE = State;

            lock (ServiceControl.LC)
            {
                var command = from cmd in ServiceControl.LC where cmd.STCD == STCD && cmd.CommandID == CommandID && cmd.SERVICETYPE == NFOINDEX.ToString() select cmd;
                List<Command> Command = command.ToList<Command>();
                if (Command.Count() > 0)
                {
                    foreach (var item in Command)
                    {
                        item.STATE = c.STATE;
                        item.Data = c.Data;
                        item.DATETIME = c.DATETIME;
                    }
                }
                else
                {
                    ServiceControl.LC.Add(c);
                }
            }

            PublicBD.db.DelCommandTemp(STCD, (int)NFOINDEX, CommandID);
            PublicBD.db.AddCommandTemp(STCD, (int)NFOINDEX, CommandID, Data, datetime, State);
        }

        /// <summary>
        /// 从命令列表移除命令(同时操作临时命令数据库表YY_COMMAND_TEMP)
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="CommandID">命令</param>
        public static void RemoveListCommand(string STCD, ServiceEnum.NFOINDEX NFOINDEX, string CommandID)
        {
            lock (ServiceControl.LC)
            {
                var command = from cmd in ServiceControl.LC where cmd.STCD == STCD && cmd.CommandID == CommandID && cmd.SERVICETYPE == NFOINDEX.ToString() select cmd;
                List<Command> Command = command.ToList<Command>();
                if (Command.Count() > 0)
                {
                    List<Command> cmds = new List<Command>(Command);
                    foreach (var item in cmds)
                    {
                        ServiceControl.LC.Remove(item);
                    }
                }

                RemoveXsd(STCD, NFOINDEX.ToString(), CommandID);

                //同时操作临时命令数据库表YY_COMMAND_TEMP
                PublicBD.db.DelCommandTemp(STCD, (int)NFOINDEX, CommandID);
            }
        }

        /// <summary>
        /// 清空所有信道的所有召测命令
        /// </summary>
        public static void ClearListCommand() 
        {
            lock (ServiceControl.LC)
            {
                //新建命令列表=清空列表
                ServiceControl.LC = new List<Command>();
                //清空所有信道的召测命令列表
                ClearXsd();
                //清空数据库里的临时命令列表
                PublicBD.db.DelCommandTemp("");
                
            }
        }

        /// <summary>
        /// 根据站号、服务类型，命令码，命令状态从命令列表中删除
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="CommandID">命令码</param>
        /// <param name="State">命令状态</param>
        /// <returns></returns>
        public static List<Command> RemoveListCommand(string STCD, ServiceEnum.NFOINDEX NFOINDEX, string CommandID, int State)
        {
            List<Command> lc = new List<Command>();
            
            lock (ServiceControl.LC)
            {
                var command = from com in ServiceControl.LC where com.STCD == STCD && com.CommandID == CommandID && com.SERVICETYPE == NFOINDEX.ToString() && com.STATE == State select com;
                
                lc.AddRange(command);
                foreach (var item in command)
                {
                    ServiceControl.LC.Remove(item);
                    break;
                }


                RemoveXsd(STCD, NFOINDEX.ToString(), CommandID);

                //同时操作临时命令数据库表YY_COMMAND_TEMP
                PublicBD.db.DelCommandTemp(STCD, (int)NFOINDEX, CommandID);
            }

            
            return lc;
        }

        /// <summary>
        /// 根据站号、服务类型，命令码从信道命令列表中删除
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="CommandID">命令码</param>
        public static void RemoveXsd(string STCD, string ServiceType, string CommandID) 
        {
            if (ServiceType == ServiceEnum.NFOINDEX.TCP.ToString())
            {
                foreach (var item in ServiceControl.tcp)
                {
                    TcpBussiness.RemoveTsdQ(item, STCD, CommandID);
                }
            }
            else if (ServiceType == ServiceEnum.NFOINDEX.UDP.ToString())
            {
                foreach (var item in ServiceControl.udp)
                {
                    UdpBussiness.RemoveUsdQ(item, STCD, CommandID);
                }
            }
            else if (ServiceType == ServiceEnum.NFOINDEX.GSM.ToString())
            {
                foreach (var item in ServiceControl.gsm)
                {
                    GsmBussiness.RemoveGsdQ(item, STCD, CommandID);
                }
            }
            else if (ServiceType == ServiceEnum.NFOINDEX.COM.ToString())
            {
                foreach (var item in ServiceControl.com)
                {
                    ComBussiness.RemoveCsdQ(item, STCD, CommandID);
                }
            }
        }

        /// <summary>
        /// 清空所有信道的召测命令列表
        /// </summary>
        public static void ClearXsd() 
        {
            foreach (var item in ServiceControl.tcp)
            {
                TcpBussiness.ClearTsdQ(item);
            }

            foreach (var item in ServiceControl.udp)
            {
                UdpBussiness.ClearUsdQ(item);
            }

            foreach (var item in ServiceControl.gsm)
            {
                GsmBussiness.ClearGsdQ(item);
            }

            foreach (var item in ServiceControl.com)
            {
                ComBussiness.ClearCsdQ(item);
            }
        }

        /// <summary>
        /// 设置协议
        /// </summary>
        public static void SetProtocol(string protocol) 
        {
            if (protocol.ToLower() == "shuiwen")
            {
                new Service.WriteReadXML().WriteProtocolXML("HydrologicProtocol.dll", "Service.Hydrologic");
                new Service.WriteReadXML().WriteInfoEncodingXML("HEX");
            }
            else if (protocol.ToLower() == "shuiziyuan")
            {
                new Service.WriteReadXML().WriteProtocolXML("Protocol.dll", "Service.WaterResource");
                new Service.WriteReadXML().WriteInfoEncodingXML("HEX");
            }
            else if (protocol.ToLower() == "yanyu")
            {
                new Service.WriteReadXML().WriteProtocolXML("GSProtocol.dll", "Service.GS");
                new Service.WriteReadXML().WriteInfoEncodingXML("ASC");
            }
            else if (protocol.ToLower() == "zhengda212")
            {
                new Service.WriteReadXML().WriteProtocolXML("HJT212-2005.dll", "Service.HTJ212");
                new Service.WriteReadXML().WriteInfoEncodingXML("ASC");
            }
        }

        /// <summary>
        /// 发送邮件到指定邮箱
        /// </summary>
        public static void SendMail()
        {
            if (ServiceControl.IsToMail)//配置文件配置了允许发送邮件
            {

                //注册信息
                string RegistrationInfo=Registration();

                //ipx信息
                List<string> IPs = GetHostIPAddress();
                string ips = "本机IP：";
                foreach (var item in IPs)
                {
                    ips += item + "<br>";
                }

                List<string> projects=(new WriteReadXML()).GetProjects();

                TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

                string To = "YYsoft2013@163.com";
                string From = To;
                string Body = "管理员：<br>   hello！<br> 系统运行时长为：" + dateDiff + "<br> 异常信息请查看附件内容!" + "<br>公网IP：" + GetPublicIP() + "<br>" + ips + "<bs>" + RegistrationInfo;


                foreach (var item in projects)
                {
                    Body += "<p align='right'>" + item + "</p>";
                }
                string Title = "Error";
                string Password = "hao1234567";

                string Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
                string Path0 = System.Windows.Forms.Application.StartupPath + "/log/Warn/Warn.txt";
                string Path1 = System.Windows.Forms.Application.StartupPath + "/log/Error/Error.txt";
                string Path2 = System.Windows.Forms.Application.StartupPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                string Path3 = "";

                using (_51Mail.SendMail sm = new _51Mail.SendMail(To, From, Body, Title, Password))
                {
                    if (File.Exists(Path))
                    {
                        Path3 = Path;
                    }
                    if (File.Exists(Path0))
                    {
                        File.Delete(Path0.Replace("Warn.txt", "ToMailWarn.txt"));
                        File.Copy(Path0, Path0.Replace("Warn.txt", "ToMailWarn.txt"), true);
                        Path3 +=","+ Path0.Replace("Warn.txt", "ToMailWarn.txt");
                    }

                    if (File.Exists(Path1))
                    {
                        File.Delete(Path1.Replace("Error.txt", "ToMailError.txt"));
                        File.Copy(Path1, Path1.Replace("Error.txt", "ToMailError.txt"), true);
                        Path3 += "," + Path1.Replace("Error.txt", "ToMailError.txt");
                    }
                    if (File.Exists(Path2))
                    {
                        Path3 += "," + Path2;
                    }
                    sm.Attachments(Path3);
                    sm.Send();
                    sm.Dispose();
                    
                    WriteQUIM("", "Service", "", "服务成功将异常信息发送到管理员Mail！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
            }
            else
            {
                WriteQUIM("", "Service", "", "服务未配置允许将异常信息发送到管理员Mail！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
            }
        }



        //获得公网IP地址
        public static string GetPublicIP()
        {
            try
            {
                Uri uri = new Uri("http://city.ip138.com/ip2city.asp");
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                req.Method = "get";
                using (Stream s = req.GetResponse().GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(s))
                    {
                        char[] ch = { '[', ']' };
                        string str = reader.ReadToEnd();
                        System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(str, @"\[(?<IP>[0-9\.]*)\]");
                        return m.Value.Trim(ch);

                    }
                }
            }
            catch
            { return "未获取到公网IP"; }
        }

        //获得本机IP列表
        private static List<string> GetHostIPAddress()
        {

            List<string> lstIPAddress = new List<string>();

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ipa in IpEntry.AddressList)
            {

                if (ipa.AddressFamily == AddressFamily.InterNetwork)

                    lstIPAddress.Add(ipa.ToString());

            }

            return lstIPAddress;

        }


        public static void RemoteCommand(byte[] b) 
        {
            Registered30(b);
            SetTCInfo(b);
        }


        #region 注册信息
        public static string Registration()
        {
            string Info = "注册信息：无";
            string text = EnDe.DESDecrypt(EnDe.ReadAk(), "1q2w3e4r", "11111111");
            string EnCPU = EnDe.GetCPU();

            if (EnCPU != text)
            {
                TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks); ;
                try
                {
                    ts2 = new TimeSpan(DateTime.Parse(text).Ticks);
                }
                catch
                {
                    Info = "注册文件错误!";
                    //MessageBox.Show("注册文件错误，请购买正版！");
                    //System.Environment.Exit(0);
                }

                TimeSpan tSpan = ts1 - ts2;
                int Day = (int)tSpan.TotalDays;
                if (Day > 300)
                {
                    Info = "系统试用结束!";
                    //MessageBox.Show("系统试用结束，请购买正版！");
                    //System.Environment.Exit(0);
                }
                else if (Day < 0) //可能用户向后调整了时间
                {
                    EnDe.WriteAk(EnDe.DESEncrypt("asdf", "1q2w3e4r", "11111111"));
                    Info = "注册文件错误,可能用户向后调整了时间!";
                    //MessageBox.Show("注册文件错误，请购买正版！");
                    //System.Environment.Exit(0);
                }
                else
                {
                    Info = "系统还可试用" + (300 - Day) + "天！";
                    //MessageBox.Show("系统还可试用" + (300 - Day) + "天！");
                }
            }

            return Info;
        }

        //注册三十天
        public static void Registered30(byte[] b)
        {
            if (System.Text.Encoding.ASCII.GetString(b) == "--reg|")
            {
                DeletEFileVerificationRegistration();
            }
        }

        private static void DeletEFileVerificationRegistration() 
        {
            string file_name = System.Windows.Forms.Application.StartupPath + "/HYXT.ak";
            try
            {
                if (File.Exists(file_name))
                {
                    File.Delete(file_name);
                }
            }
            catch { }
            VerificationRegistration();

            WriteQUIM("", "Service", "", "Registered success！", new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

        }

        public static void VerificationRegistration()
        {
            if (EnDe.ReadAk() == null)
            {
                EnDe.WriteAk(EnDe.DESEncrypt(DateTime.Now.ToString(), "1q2w3e4r", "11111111"));
            }
        }
        #endregion
        #endregion

        #region 透明传输--转发用
        /// <summary>
        /// 写入透明传输列表
        /// </summary>
        /// <param name="Data">数据报</param>
        public static void WriteQDM(byte[] Data)
        {
            try
            {
                DataModel Dm = new DataModel();
                Dm.Data = Data;

                ServiceQueue.QDM.Enqueue(Dm);
                if (ServiceQueue.QDM.Count > 500)//控制数据集合最多不超过500条
                {
                    Dm = new DataModel();
                    ServiceQueue.QDM.TryDequeue(out Dm);
                }
            }
            catch (Exception ex)
            { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
        }

        
        public static void SetTCInfo(byte[] b)
        {
            string str= System.Text.Encoding.ASCII.GetString(b);
            if (str.Substring(0, 6) == "--TCI|") 
            {
                string[] temp= str.Split(new char[] { '|' });
                WriteReadXML wrx = new WriteReadXML();
                wrx.WriteTCXML(temp[1], temp[2].Replace("\r\n",""));
                ServiceControl.ReReadTCXML();
                ServiceControl.ReTCstart();
            }
        } 
        #endregion
        
        #region 状态命令--发送至界面
        /// <summary>
        /// 发送各服务在线状态
        /// </summary>
        public static void SendEveryServcieState()
        {
            string TcpStr = "tcp|";
            string TListCount = " ";
            for (int i = 0; i < ServiceControl.tcp.Length; i++)
            {
                if (ServiceControl.tcp[i].IsOpen == true)
                {
                    //tcp|1:0:1 
                    //udp|1:1
                    //gsm|0
                    //com|1

                    TcpStr += "1:";
                }
                else { TcpStr += "0:"; }

                var temp = from t in ServiceControl.tcp[i].Ts where t.TCPSOCKET != null select t;
                TListCount += temp.Count() + "," + ServiceControl.tcp[i].TQ.Qtrd.Count + "," + ServiceControl.tcp[i].TQ.Qtsd.Count + "," + ServiceQueue.QUIM.Count + "," + ServiceQueue.QDM.Count + ":";
            }
            if (TcpStr != "tcp|")
            {
                TcpStr = TcpStr.Substring(0, TcpStr.Length - 1);

                TListCount = TListCount.Substring(0, TListCount.Length - 1);
                ServiceBussiness.WriteQUIM("", "", "", TcpStr + TListCount, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }

            string UdpStr = "udp|";
            string UListCount = " "; //列表数量 udp连接，接收数据，发送数据，发送到界面数据   （例子  15,0,0,20:12,0,0,2）
            for (int i = 0; i < ServiceControl.udp.Length; i++)
            {
                if (ServiceControl.udp[i].IsOpen == true)
                {
                    UdpStr += "1:";
                }
                else { UdpStr += "0:"; }

                var temp = from u in ServiceControl.udp[i].Us where u.IpEndPoint != null select u;
                UListCount += temp.Count() + "," + ServiceControl.udp[i].UQ.Qurd.Count + "," + ServiceControl.udp[i].UQ.Qusd.Count + "," + ServiceQueue.QUIM.Count + "," + ServiceQueue.QDM.Count + ":";
            }
            if (UdpStr != "udp|")
            {
                UdpStr = UdpStr.Substring(0, UdpStr.Length - 1);
                UListCount = UListCount.Substring(0, UListCount.Length - 1);
                ServiceBussiness.WriteQUIM("", "", "", UdpStr + UListCount, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }

            string ComStr = "com|";
            string CListCount = " ";
            for (int i = 0; i < ServiceControl.com.Length; i++)
            {
                if (ServiceControl.com[i].sp.IsOpen == true)
                {
                    ComStr += "1:";
                }
                else { ComStr += "0:"; }

                var temp = from c in ServiceControl.com [i].Cs  where c.SATELLITE != null select c;
                if (ServiceControl.com[i].CState != null)
                    //原版本卫星协议返回状态
                    //CListCount += ServiceControl.com[i].CState.Power1 + "," + ServiceControl.com[i].CState.Power2 + "," + ServiceControl.com[i].CState.Beam1 + "," + ServiceControl.com[i].CState.Beam2 + "," + ServiceControl.com[i].CState.Response + "," + ServiceControl.com[i].CState.Inhibition+  "," + ServiceControl.com[i].CState.PowerSupply +":";
                    //新版本卫星协议4.0返回状态
                    CListCount += ServiceControl.com[i].CStateFor4.ICState + "," + ServiceControl.com[i].CStateFor4.HardwareState + "," + ServiceControl.com[i].CStateFor4.Electricity + "," + ServiceControl.com[i].CStateFor4.Power + ",,,:";
             }

            if (ComStr != "com|")
            {
                ComStr = ComStr.Substring(0, ComStr.Length - 1);
                CListCount = CListCount.Substring(0, CListCount.Length - 1);
                ServiceBussiness.WriteQUIM("", "", "", ComStr + CListCount, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }

            string GsmStr = "gsm|";
            for (int i = 0; i < ServiceControl.gsm.Length; i++)
            {
                if (ServiceControl.gsm[i].gm.IsOpen == true)
                {
                    GsmStr += "1:";
                }
                else { GsmStr += "0:"; }
            }

            if (GsmStr != "gsm|")
            {
                GsmStr = GsmStr.Substring(0, GsmStr.Length - 1);
                ServiceBussiness.WriteQUIM("", "", "", GsmStr, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }
        }

        /// <summary>
        /// 发送各测站在线状态
        /// </summary>
        public static void SendEveryRTUOnlineState()
        {
            string OnlineStr = "stcd|";

            
            for (int i = 0; i < ServiceControl.tcp.Length; i++)
            {
                //lock (ServiceControl.tcp[i].Ts)
                var temp = from t in ServiceControl.tcp[i].Ts where t.TCPSOCKET != null select t;
                if (temp.Count()>0)
                    foreach (var item in temp)
                    {
                        OnlineStr += item.STCD + ":tcp ";
                    }
            }
            for (int i = 0; i < ServiceControl.udp.Length; i++)
            {
                //lock (ServiceControl.udp[i].Us)
                var temp = from u in ServiceControl.udp[i].Us where u.IpEndPoint != null select u;
                foreach (var item in temp)
                {
                    OnlineStr += item.STCD + ":udp ";
                }
            }

            if (OnlineStr != "stcd|")
            {
                OnlineStr = OnlineStr.Substring(0, OnlineStr.Length - 1);
                ServiceBussiness.WriteQUIM("", "", "", OnlineStr, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }
            else
            {
                ServiceBussiness.WriteQUIM("", "", "", OnlineStr, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
            }
        }

        /// <summary>
        /// 连接数据库状态
        /// </summary>
        public static void SendDBConnectionState()
        {
            string DBConnectionStr = null;
            if (PublicBD.ConnectState)
            { DBConnectionStr = "db|1"; }
            else
            {
                DBConnectionStr = "db|0";
                //if (PublicBD.db.dt.Open())
                //{
                //    DBConnectionStr = "db|1";
                //}
            }
            ServiceBussiness.WriteQUIM("", "", "", DBConnectionStr, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
        }

        /// <summary>
        /// 发送全部命令执行状态列表
        /// </summary>
        public static void SendCommandListState()
        {
            List<Command> LC = ServiceControl.LC;

            if (LC != null && LC.Count() > 0)
            {
                foreach (var item in LC)
                {
                    //cmd|tcp 0012345678 02 680F6830001234567802F180502954102700A416 0 1999-09-09 12:00:00
                    CommandWriteQUIM(item.STCD, item.SERVICETYPE, item.CommandID, item.Data, item.DATETIME, item.STATE);
                }

            }
        }

        /// <summary>
        /// 发送单条命令状态
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <param name="ServiceType">服务类型</param>
        /// <param name="CommandID">命令</param>
        /// <param name="Data">命令数据</param>
        /// <param name="datetime">时间</param>
        /// <param name="State">状态</param>
        public static void CommandWriteQUIM(string STCD, string ServiceType, string CommandID, string Data, DateTime datetime, int State)
        {
            string CommandStr = "cmd|" + ServiceType + " " + STCD + " " + CommandID + " " + Data + " " + State + " " + datetime.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(CommandStr);
            ServiceBussiness.WriteQUIM("", "", "", CommandStr, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
        }
        #endregion

        /// <summary>
        /// 将节点信息发到客户端
        /// </summary>
        /// <param name="node">节点信息</param>
        public static void SendXMLFile(string node)
        {
            WriteReadXML wrx = new WriteReadXML();
            string xml = wrx.GetXMLStr(node);
            xml = "file|" + node + "|" + xml;
            ServiceBussiness.WriteQUIM("", "", "", xml, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.State);
        }

        /// <summary>
        /// 将数据整理出节点信息写入配置文件
        /// </summary>
        /// <param name="data"></param>
        public static void WriteXMLFile(string data) 
        {
            string[] temps = data.Split(new char[] { '|' });
            if (temps.Length > 1)
            {
                WriteReadXML wrx = new WriteReadXML();
                wrx.SetXMLStr(temps[0], temps[1]);
            }
        }

        /// <summary>
        /// 从数据库中得到临时命令列表
        /// </summary>
        /// <returns></returns>
        public static List<Command> GetCommandTempToLC()
        {
            List<Command> LC = new List<Command>();
            Command cmd=null;
            IList<YY_COMMAND_TEMP> ILC= PublicBD.db.GetCommandTempList();
            if (ILC != null && ILC.Count > 0) 
            {
                foreach (var item in ILC)
                {
                    cmd = new Command();
                    if(item.NFOINDEX ==1)
                    {
                        cmd.SERVICETYPE = ServiceEnum.NFOINDEX.TCP.ToString();
                    }
                    else if (item.NFOINDEX == 2)
                    { cmd.SERVICETYPE = ServiceEnum.NFOINDEX.UDP.ToString(); }
                    else if (item.NFOINDEX == 3)
                    { cmd.SERVICETYPE = ServiceEnum.NFOINDEX.GSM.ToString(); }
                    else if (item.NFOINDEX == 4)
                    { cmd.SERVICETYPE = ServiceEnum.NFOINDEX.COM.ToString(); }
                    cmd.STCD = item.STCD;
                    cmd.Data = item.Data;
                    cmd.DATETIME = item.TM;
                    cmd.STATE = item.State;
                    cmd.CommandID = item.CommandID;
                    LC.Add(cmd);
                }
            }

            //将所有已经召测(1,2,3)并无最终状态（-1，-2）的命令重新发送
            foreach (var item in LC)
            {
                if (item.STATE == 1 || item.STATE == 2 || item.STATE == 3)
                {
                    item.STATE = 0;
                }
            }
            return LC;
        }

        #region RTU列表(操作数据库)
        public static List<YY_RTU_Basic> RtuList = null;
        public static List<YY_RTU_CONFIGDATA> CONFIGDATAList = null;
        public static List<YY_RTU_WRES> WRESList = null;
        public static List<YY_RTU_ITEMCONFIG> ITEMCONFIGList = null;
        public static List<YY_RTU_CONFIGITEM> CONFIGITEMList = null;
        public static List<YY_RTU_ITEM> ITEMList = null;
        /// <summary>
        /// 得到RTU列表
        /// </summary>
        public static void GetRTUList()
        {
            RtuList = PublicBD.db.GetRTUList("").ToList<YY_RTU_Basic>();
            
            CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("").ToList<YY_RTU_CONFIGDATA>();
            WRESList = PublicBD.db.GetRTU_WRESList("").ToList<YY_RTU_WRES>();
            ITEMCONFIGList = Service.PublicBD.db.GetRTU_ItemConfig("").ToList<YY_RTU_ITEMCONFIG>();
            CONFIGITEMList = Service.PublicBD.db.GetRTU_ConfigItemList("").ToList<YY_RTU_CONFIGITEM>();
            ITEMList = Service.PublicBD.db.GetItemList("").ToList<YY_RTU_ITEM>();

            if (ServiceControl.gsm != null)
            {
                //重新读取手机号
                List<GsmMobile> Gs = ServiceBussiness.GetGsmMobileList();
                foreach (var gsm in ServiceControl.gsm)
                {
                    gsm.Gs = Gs;
                }
            }
        }

        /// <summary>
        /// 从数据库读取出手机号列表
        /// </summary>
        /// <returns></returns>
        public static List<GsmMobile> GetGsmMobileList()
        {
            List<YY_RTU_CONFIGDATA> list = PublicBD.db.GetRTU_CONFIGDATAList(" where ConfigID like '__00____AA07'").ToList<YY_RTU_CONFIGDATA>();
            List<GsmMobile> gsmmobilelist = new List<GsmMobile>();
            GsmMobile gm = null;
            foreach (var item in RtuList)
            {
                gm = new GsmMobile();
                gm.STCD = item.STCD;
                var config = from c in list where c.STCD == item.STCD select c;
                if (config.Count() > 0)
                {
                    gm.MOBILE = config.First().ConfigVal;
                }
                gsmmobilelist.Add(gm);
            }

            return gsmmobilelist;
        }

        /// <summary>
        /// 从数据库读取出卫星号列表
        /// </summary>
        /// <returns></returns>
        public static List<ComSatellite> GetComSatelliteList()
        {

            IList<YY_RTU_CONFIGDATA> list = PublicBD.db.GetRTU_CONFIGDATAList(" where ConfigID like '__00____AA08'").ToList<YY_RTU_CONFIGDATA>();
            List<ComSatellite> comsatellitelist = new List<ComSatellite>();
            ComSatellite cs = null;
            foreach (var item in RtuList)
            {
                cs = new ComSatellite();
                cs.STCD = item.STCD;
                var config = from c in list where c.STCD == item.STCD select c;
                if (config.Count() > 0)
                {
                    cs.SATELLITE = config.First().ConfigVal;
                }
                comsatellitelist.Add(cs);
            }

            return comsatellitelist;
        }
        #endregion

    }
}
