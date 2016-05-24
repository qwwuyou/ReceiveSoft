using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdpService;
using GsmService;
using ComService;
using TcpService;
using System.Threading;
using ToUI;
using log4net;
using System.IO;




namespace Service
{


    public class ServiceControl
    { 

        #region 与界面交互的对象
        ToUI.TcpServer TcpServer_UI;
        static Timer timer_SD;
        static TcpClient tcpclient_SD;
        UITcpModel UiTcpModel;
        #endregion

        #region 与前端设备交互的对象
        /// <summary>
        /// 服务列表
        /// </summary>
        public static List<serviceModel> LsM;
        public static TcpService.TcpServer[] tcp;
        public static UdpServer[] udp;
        public static GsmServer[] gsm;
        public static ComServer[] com;
        #endregion

        #region 其他对象
        /// <summary>
        /// 透传和发送数据到界面的队列
        /// </summary>
        ServiceQueue sq = new ServiceQueue();

        //日志记录对象
        public static log4net.ILog log = log4net.LogManager.GetLogger("Logger");
        
        /// <summary>
        /// 命令列表--保存命令执行状态
        /// </summary>
        public static  List<Command> LC;

        /// <summary>
        /// 透传用实体
        /// </summary>
        static UITcpModel TCTcpModel = null;

        /// <summary>
        /// 系统启动时间
        /// </summary>
        public static DateTime StartTime = DateTime.Now;

        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public static bool IsToMail = false;

        /// <summary>
        /// 写入日志信息的编码格式
        /// </summary>
        private static string HexOrAsc = "";
        public static string  HEXOrASC
        {
            get { return HexOrAsc; }
            set { HexOrAsc = value; }
        }

        /// <summary>
        /// 2016/3/27增加 访问中心（邮箱控制）的方法
        /// </summary>
        static TcpClient tcpclient_Center;
        static string CenterIP;
        static string CenterPort;
        static string project;
        static string RegistrationInfo;
        #endregion

        /// <summary>
        /// 信道重启，重新读取xml
        /// </summary>
        public static void ReReadXML()
        {
            //重新初始化读取数据库配置
            PublicBD.RePublicBD();

             #region 读取xml配置文件
            WriteReadXML wrx = new WriteReadXML();

            try
            {
                LsM = wrx.ReadServiceXML();
                log.Warn(DateTime.Now +"信道重启，读取服务配置文件成功！");
            }
            catch(Exception e)
            {
                log.Warn(DateTime.Now + "信道重启，读取服务配置文件失败！", e);
            }

            try 
            {
                IsToMail = wrx.ReadIsToMail();
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "信道重启，读取是服务停止后是否发送异常信息到管理员Mail失败！", e); }

            try 
            {
                HEXOrASC = wrx.ReadHexOrAsc();
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "信道重启，读取写入日志信息的编码格式的配置文件失败！", e); }
            #endregion

            Reflection_Protoco R_Protoco = new Reflection_Protoco();
        }

        /// <summary>
        /// 重新读取透传xml
        /// </summary>
        public static void ReReadTCXML() 
        {
            WriteReadXML wrx = new WriteReadXML();
            try
            {
                TCTcpModel = wrx.ReadTCTcpXML();
                log.Warn(DateTime.Now + "读取透传配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取透传配置文件失败！", e);
            }
        }

        public static void ReTCstart() 
        {
            #region 透传的客户端启动
            try
            {
                tcpclient_SD = new TcpClient(TCTcpModel.IP, TCTcpModel.PORT, "");
                tcpclient_SD.Start();
                tcpclient_SD.OnReceivedData += new EventHandler<TcpClient.ReceivedDataEventArgs>(tcpclient_SD_OnReceivedData);
                timer_SD = new Timer(new TimerCallback(SendData), null, 100, 100);
                log.Warn("透传服务(" + TCTcpModel.IP + ":" + TCTcpModel.PORT + ")启动成功！");
            }
            catch (Exception e)
            {
                if (TCTcpModel == null)
                { log.Warn("透传服务未设置,启动失败！", e); }
                else
                {
                    log.Warn("透传服务(" + TCTcpModel.IP + ":" + TCTcpModel.PORT + ")启动失败！", e);
                }
            }
            #endregion
        }

        public static void AccessCenter()
        {
            
            #region 项目列表
            List<string> projects = (new WriteReadXML()).GetProjects();
            foreach (var item in projects)
            {
                project += item + ",";
            }
            project = project.TrimEnd(new char[] { ',' });
            #endregion

            Thread ConnectCenter = new Thread(new ThreadStart(ThreadConCenter));
            ConnectCenter.IsBackground = true;
            ConnectCenter.Start();
        }
        private static void ThreadConCenter()
        {
            while (true)
            {
                if (tcpclient_Center==null||!tcpclient_Center.Connected)
                {
                    TcpClient_Init();
                }
                else
                {
                    #region 运行时长计算
                    TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                    TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                    #endregion
                    #region Rtu数量(连接数据库时就被初始化了)
                    int RtuCount = ServiceBussiness.RtuList.Count();
                    #endregion
                    #region 注册信息
                    RegistrationInfo = ServiceBussiness.Registration();
                    #endregion
                    
                    dateDiff = "服务运行数据:" + "运行时长[" + dateDiff + "],RTU数量[" + RtuCount.ToString() + "],项目[" + project + "],注册信息[" + RegistrationInfo + "]";
                    //dateDiff = "Operation data:" + "RunTimeLength[" + dateDiff + "],RTUCount[" + RtuCount.ToString() + "]";
                   
                    tcpclient_Center.SendCenterData(Encoding.GetEncoding("gb2312").GetBytes(dateDiff));
                     
                 }
                Thread.Sleep(5*60*1000);
            }
        }
        public static void TcpClient_Init()
        {
            new CenterAddress().GetCenterAddress(out CenterIP, out CenterPort);

            if (CenterIP != "" && CenterPort != "")
            {

                tcpclient_Center = new TcpClient(CenterIP, int.Parse(CenterPort), "Center");
                tcpclient_Center.Start();
                tcpclient_Center.OnReceivedData += new EventHandler<TcpClient.ReceivedDataEventArgs>(tcpclient_Center_OnReceivedData);
            }
        }

        static void tcpclient_Center_OnReceivedData(object sender, TcpClient.ReceivedDataEventArgs e)
        {
            //throw new NotImplementedException();
        }



        public ServiceControl()
        {
            //创建注册文件
            ServiceBussiness.VerificationRegistration();

            #region 读取xml配置文件
            WriteReadXML wrx = new WriteReadXML();

            try
            {
                LsM = wrx.ReadServiceXML();
                log.Warn(DateTime.Now +"读取服务配置文件成功！");
            }
            catch(Exception e)
            {
                log.Warn(DateTime.Now + "读取服务配置文件失败！", e);
            }


            try
            {
                UiTcpModel = wrx.ReadUITcpXML();
                log.Warn(DateTime.Now + "读取服务与界面交互配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取服务与界面交互配置服务配置文件失败！", e);
            }

            try
            {
                TCTcpModel = wrx.ReadTCTcpXML();
                log.Warn(DateTime.Now + "读取透传配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取透传配置文件失败！", e);
            }

            try 
            {
                IsToMail = wrx.ReadIsToMail();
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取是服务停止后是否发送异常信息到管理员Mail失败！", e); }

            try 
            {
                HEXOrASC = wrx.ReadHexOrAsc();
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取写入日志信息的编码格式的配置文件失败！", e); }
            #endregion
            
            #region 用反射实现
            Reflection_Protoco R_Protoco = new Reflection_Protoco();
            #endregion

            //初始化命令列表
            LC = new List<Command>();
        }


        /// <summary>
        /// 启动各组服务---已经日志
        /// </summary>
        public void start()
        {
           // System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
           // sw.Start();
           // //耗时巨大的代码
            
         
           //sw.Stop();
           //TimeSpan ts2 = sw.Elapsed;
           //Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
            #region 尝试连接3次数据库
            int ConCount = 1;
            while (true)
            {
                if (PublicBD.ConnectState)
                {
                    log.Warn("数据库连接成功！");
                    ServiceBussiness.GetRTUList();
                    break;
                }
                else
                {
                    log.Warn("数据库连接第"+ConCount+"次失败！");
                    Thread.Sleep(2*60*1000);
                }
                 

                if (ConCount == 3)
                {
                    throw new Exception();
                }

                ConCount++;
            }
            #endregion
          
            #region 与前端设备交互的服务启动
            ChannelStart();
            #endregion

            #region 与界面交互的socket服务端启动
            try
            {
                TcpServer_UI = new ToUI.TcpServer(UiTcpModel.IP, UiTcpModel.PORT, "UI");
                TcpServer_UI.Start();
                TcpServer_UI.OnConnected += new EventHandler<ToUI.ConnectedEventArgs>(TcpServer_UI_OnConnected);
                TcpServer_UI.OnReceivedData += new EventHandler<ToUI.ReceivedDataEventArgs>(TcpServer_UI_OnReceivedData);
                TcpServer_UI.OnDisconnected += new EventHandler<ToUI.DisconnectedEventArgs>(TcpServer_UI_OnDisconnected);
                log.Warn(DateTime.Now + "界面交互服务(" + UiTcpModel.IP + ":" + UiTcpModel.PORT + ")启动成功！");
            }
            catch (Exception ex)
            {
                log.Warn("tcp服务(" + UiTcpModel.IP + ":" + UiTcpModel.PORT + ")启动失败！", ex);
                throw ex; 
            }
            #endregion

            #region 透传的客户端启动
            try
            {
                tcpclient_SD = new TcpClient(TCTcpModel.IP, TCTcpModel.PORT, "");
                tcpclient_SD.Start();
                tcpclient_SD.OnReceivedData += new EventHandler<TcpClient.ReceivedDataEventArgs>(tcpclient_SD_OnReceivedData);
                timer_SD = new Timer(new TimerCallback(SendData), null,100, 100);
                log.Warn("透传服务(" + TCTcpModel.IP + ":" + TCTcpModel.PORT + ")启动成功！");
            }
            catch (Exception e)
            {
                if (TCTcpModel == null)
                { log.Warn( "透传服务未设置,启动失败！", e); }
                else
                {
                    log.Warn( "透传服务(" + TCTcpModel.IP + ":" + TCTcpModel.PORT + ")启动失败！", e);
                }
            }
            #endregion

            #region 访问中心
            AccessCenter();
            #endregion

    

            log.Warn("启动工作完成！");
            log.Warn("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        }
        //各信道启动
        public void ChannelStart()
        {
            var temp = from t in LsM where t.SERVICETYPE == "TCP" select t;
            List<serviceModel> TEMP = temp.ToList<serviceModel>();
            tcp = new TcpService.TcpServer[TEMP.Count()];
            int k = 0;


            #region 读取命令列表(接收UI召测和GSM发送召测有定时写操作)，因此在gsm服务启动前读取
            ServiceControl.LC = ServiceBussiness.GetCommandTempToLC();
            #endregion

            #region 启动服务
            foreach (var item in TEMP)
            {
                try
                {
                    tcp[k] = new TcpService.TcpServer(item.IP_PORTNAME, item.PORT_BAUDRATE, item.SERVICEID);
                    tcp[k].Start();
                    tcp[k].OnConnected += new EventHandler<TcpService.ConnectedEventArgs>(tcp_OnConnected);
                    tcp[k].OnReceivedData += new EventHandler<TcpService.ReceivedDataEventArgs>(tcp_OnReceivedData);
                    tcp[k].OnDisconnected += new EventHandler<TcpService.DisconnectedEventArgs>(tcp_OnDisconnected);
                    log.Warn("tcp服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动成功！");
                }
                catch (Exception e)
                {
                    log.Warn("tcp服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动失败！", e);
                    //throw e;  //-------避免配置有误，服务死掉
                }
                k++;
            }

            temp = from t in LsM where t.SERVICETYPE == "UDP" select t;
            TEMP = temp.ToList<serviceModel>();
            udp = new UdpServer[TEMP.Count()];
            k = 0;
            foreach (var item in TEMP)
            {
                try
                {
                    udp[k] = new UdpServer(item.IP_PORTNAME, item.PORT_BAUDRATE, item.SERVICEID);
                    udp[k].Start();
                    udp[k].OnReceivedData += new EventHandler<UdpService.ReceivedDataEventArgs>(udp_OnReceivedData);
                    log.Warn(DateTime.Now + "udp服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动成功！");
                }
                catch (Exception e)
                {
                    log.Warn(DateTime.Now + "udp服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动失败！", e);
                    //throw e;  //-------避免配置有误，服务死掉
                }
                k++;
            }
            temp = from t in LsM where t.SERVICETYPE == "GSM" select t;
            TEMP = temp.ToList<serviceModel>();
            gsm = new GsmServer[TEMP.Count()];
            k = 0;
            foreach (var item in TEMP)
            {
                try
                {
                    gsm[k] = new GsmServer(item.IP_PORTNAME, item.PORT_BAUDRATE, item.SERVICEID);
                    gsm[k].Start();
                    Console.WriteLine(DateTime.Now + "启动各组服务命令，" + "Restart！");
                    gsm[k].OnReceivedData += new EventHandler<GsmService.ReceivedDataEventArgs>(gsm_OnReceivedData);
                    log.Warn(DateTime.Now + "gsm服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动成功！");
                }
                catch (Exception e)
                {
                    log.Warn(DateTime.Now + "gsm服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动失败！", e);
                    //throw e;  //-------避免配置有误，服务死掉
                }
                k++;
            }

            temp = from t in LsM where t.SERVICETYPE == "COM" select t;
            TEMP = temp.ToList<serviceModel>();
            com = new ComServer[TEMP.Count()];
            k = 0;
            foreach (var item in TEMP)
            {
                try
                {
                    com[k] = new ComServer(item.IP_PORTNAME, item.PORT_BAUDRATE,item.NUM , item.SERVICEID);
                    com[k].Start();
                    com[k].OnReceivedData += new EventHandler<ComService.ReceivedDataEventArgs>(com_OnReceivedData);
                    log.Warn(DateTime.Now + "com服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动成功！");
                }
                catch (Exception e)
                {
                    log.Warn(DateTime.Now + "com服务(" + item.IP_PORTNAME + ":" + item.PORT_BAUDRATE + ")启动失败！", e);
                    //throw e;  //-------避免配置有误，服务死掉
                }
                k++;
            }

            #endregion
        }

        //将命令分配到各信道发送列表
        void ToQxsd()
        {
            var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "TCP" select cmd;
            List<Command> CMDS = cmds.ToList<Command>();
            foreach (var t in ServiceControl.tcp)
            {
                foreach (var item in CMDS)
                {
                    #region 编码
                    byte[] EncoderData = null;
                    if (ServiceControl.HEXOrASC == "HEX")
                    {
                        EncoderData = EnCoder.HexStrToByteArray(item.Data);
                    }
                    if (ServiceControl.HEXOrASC == "ASC")
                    {
                        EncoderData = Encoding.ASCII.GetBytes(item.Data);
                    }
                    #endregion
                    TcpService.TcpBussiness.WriteTsdQ(t, item.STCD, EncoderData, item.CommandID);
                }
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "UDP" select cmd;
            CMDS = cmds.ToList<Command>();
            foreach (var u in ServiceControl.udp)
            {
                foreach (var item in CMDS)
                {
                    #region 编码
                    byte[] EncoderData = null;
                    if (ServiceControl.HEXOrASC == "HEX")
                    {
                        EncoderData = EnCoder.HexStrToByteArray(item.Data);
                    }
                    if (ServiceControl.HEXOrASC == "ASC")
                    {
                        EncoderData = Encoding.ASCII.GetBytes(item.Data);
                    }
                    #endregion
                    UdpService.UdpBussiness.WriteUsdQ(u, item.STCD, EncoderData, item.CommandID);
                }
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "GSM" select cmd;
            CMDS = cmds.ToList<Command>();
            foreach (var g in ServiceControl.gsm)
            {
                foreach (var item in CMDS)
                { 
                    #region 编码
                    byte[] EncoderData = null;
                    if (ServiceControl.HEXOrASC == "HEX")
                    {
                        EncoderData = EnCoder.HexStrToByteArray(item.Data);
                    }
                    if (ServiceControl.HEXOrASC == "ASC")
                    {
                        EncoderData = Encoding.ASCII.GetBytes(item.Data);
                    }
                    #endregion
                    GsmService.GsmBussiness.WriteGsdQ(g, item.STCD, EncoderData, item.CommandID);
                }
            }

            cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "COM" select cmd;
            CMDS = cmds.ToList<Command>();
            foreach (var c in ServiceControl.com)
            {
                foreach (var item in CMDS)
                {
                    #region 编码
                    byte[] EncoderData = null;
                    if (ServiceControl.HEXOrASC == "HEX")
                    {
                        EncoderData = EnCoder.HexStrToByteArray(item.Data);
                    }
                    if (ServiceControl.HEXOrASC == "ASC")
                    {
                        EncoderData = Encoding.ASCII.GetBytes(item.Data);
                    }
                    #endregion
                    ComService.ComBussiness.WriteCsdQ(c, item.STCD, EncoderData, item.CommandID);
                }
            }
        }
        public void ToQxsd(ServiceEnum.NFOINDEX NFOINDEX)
        {
            if (ServiceEnum.NFOINDEX.TCP == NFOINDEX)
            {
                var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "TCP" select cmd;
                List<Command> CMDS = cmds.ToList<Command>();
                foreach (var t in ServiceControl.tcp)
                {
                    foreach (var item in CMDS)
                    {
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(item.Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(item.Data);
                        }
                        #endregion
                        TcpService.TcpBussiness.WriteTsdQ(t, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
            else if (ServiceEnum.NFOINDEX.UDP == NFOINDEX)
            {
                var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "UDP" select cmd;
                List<Command> CMDS = cmds.ToList<Command>();
                foreach (var u in ServiceControl.udp)
                {
                    foreach (var item in CMDS)
                    {
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(item.Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(item.Data);
                        }
                        #endregion
                        UdpService.UdpBussiness.WriteUsdQ(u, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
            else if (ServiceEnum.NFOINDEX.GSM == NFOINDEX)
            {
                var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "GSM" select cmd;
                List<Command> CMDS = cmds.ToList<Command>();
                foreach (var g in ServiceControl.gsm)
                {
                    foreach (var item in CMDS)
                    {
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(item.Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(item.Data);
                        }
                        #endregion
                        GsmService.GsmBussiness.WriteGsdQ(g, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
            else if (ServiceEnum.NFOINDEX.COM == NFOINDEX)
            {
                var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "COM" select cmd;
                List<Command> CMDS = cmds.ToList<Command>();
                foreach (var c in ServiceControl.com)
                {
                    foreach (var item in CMDS)
                    {
                        #region 编码
                        byte[] EncoderData = null;
                        if (ServiceControl.HEXOrASC == "HEX")
                        {
                            EncoderData = EnCoder.HexStrToByteArray(item.Data);
                        }
                        if (ServiceControl.HEXOrASC == "ASC")
                        {
                            EncoderData = Encoding.ASCII.GetBytes(item.Data);
                        }
                        #endregion
                        ComService.ComBussiness.WriteCsdQ(c, item.STCD, EncoderData, item.CommandID);
                    }
                }
            }
        }

        //透传接收
        static void tcpclient_SD_OnReceivedData(object sender, TcpClient.ReceivedDataEventArgs e)
        {
            string temp = Encoding.ASCII.GetString(e.RevData);

            string[] data = temp.Split(new char[] { '#' });
        }


        #region 通知界面的tcp服务事件处理
        void TcpServer_UI_OnDisconnected(object sender, ToUI.DisconnectedEventArgs e)
        {
            try
            {
                ToUI.TcpServer tcp = sender as ToUI.TcpServer;

                //从连接列表删除
                ToUI.TcpBussiness.DelSocket(tcp, e.ClientSocket);
            }
            catch(Exception ex)
            {  log.Warn(ex.ToString()); }
        }


        //界面传过来的召测数据报
        void TcpServer_UI_OnReceivedData(object sender, ToUI.ReceivedDataEventArgs e)
        {
            try
            {
                ToUI.TcpServer tcp = sender as ToUI.TcpServer;
                ToUI.TcpBussiness.UpdSocket(tcp, e.ClientSocket);
                ServiceBussiness.WriteQxsd(e.RevData, this);
            }
            catch (Exception ex)
            { log.Warn( ex.ToString()); }

          
        }



        void TcpServer_UI_OnConnected(object sender, ToUI.ConnectedEventArgs e)
        {
            try
            {
                ToUI.TcpServer tcp = sender as ToUI.TcpServer;
                //添加入连接列表
                ToUI.TcpBussiness.AddSocket(tcp, e.ClientSocket);


                //第一次连接后，把所有通讯列表发送到客户端界面
                ServiceBussiness.SendCommandListState();
            }
            catch (Exception ex)
            { log.Warn( ex.ToString()); }
        }
        #endregion
       


        /// <summary>
        /// 停止各组服务---已经日志
        /// </summary>
        public void stop() 
        {
            foreach (var item in tcp)
            {
                var temp = from t in LsM where t.SERVICETYPE == "TCP" && t.SERVICEID == item.ServiceID select t;
                List<serviceModel> TEMP = temp.ToList<serviceModel>();
                try
                {
                    item.Stop();
                    if (TEMP.Count() > 0)
                    {
                        log.Warn( "tcp服务(" + temp.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止成功！");
                    }
                }
                catch
                { log.Warn( "tcp服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止失败！"); }
            }

            foreach (var item in udp)
            {
                var temp = from t in LsM where t.SERVICETYPE == "UDP" && t.SERVICEID == item.ServiceID select t;
                List<serviceModel> TEMP = temp.ToList<serviceModel>();
                try
                {
                    item.Stop();
                    if (TEMP.Count() > 0)
                    {
                        log.Warn( "udp服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止成功！");
                    }
                }
                catch
                { log.Warn("udp服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止失败！"); }
            }

            foreach (var item in gsm)
            {
                var temp = from t in LsM where t.SERVICETYPE == "GSM" && t.SERVICEID == item.ServiceID select t;
                List<serviceModel> TEMP = temp.ToList<serviceModel>();
                try
                {
                    item.Stop();
                    if (TEMP.Count() > 0)
                    {
                        log.Warn( "gsm服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止成功！");
                    }
                }
                catch
                { log.Warn("gsm服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止失败！"); }
            }

            foreach (var item in com)
            {
                var temp = from t in LsM where t.SERVICETYPE == "COM" && t.SERVICEID == item.ServiceID select t;
                List<serviceModel> TEMP = temp.ToList<serviceModel>();
                try
                {
                    item.Stop();
                    if (TEMP.Count() > 0)
                    {
                        log.Warn("com服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止成功！");
                    }
                }
                catch
                { log.Warn( "com服务(" + TEMP.First().IP_PORTNAME + ":" + TEMP.First().PORT_BAUDRATE + ")停止失败！"); }
            }

            log.Warn("停止工作完成！"); 
            log.Warn("*****************************************************************************");
        }

        #region GSSATELLITEM
        void com_OnReceivedData(object sender, ComService.ReceivedDataEventArgs e)
        {
            ComServer com = sender as ComServer;

            lock (com) 
            {
                List<ComReceivedData> Lcrd = ComBussiness.SubpackageFor4(com, e.Data);
                foreach (var item in Lcrd)
                {
                    //写入接收数据队列
                    ComBussiness.WriteCrdQ(com, item.SATELLITE, item.Data);

                    //写入透传列表
                    ServiceBussiness.WriteQDM(e.Data);
                    //写入日志
                    LogInfoToTxt(ServiceEnum.NFOINDEX.COM, "", item.Data);

                    //解析数据包(更新卫星号列表)
                    ComBussiness.ResolvePacket(com);
                }
            }

            
        }
        #endregion


        #region GSM
        void gsm_OnReceivedData(object sender, GsmService.ReceivedDataEventArgs e)
        {

            GsmServer gsm = sender as GsmServer;
            lock (gsm)
            {
                string ss = "Index：" + e.Decodedmessage.SmsIndex + "\r\n 短信中心：" + e.Decodedmessage.ServiceCenterAddress + "\r\n" + "手机号码：" + e.Decodedmessage.PhoneNumber + "\r\n" +
                                "短信内容：" + e.Decodedmessage.SmsContent + "\r\n" + "发送时间：" + e.Decodedmessage.SendTime;
               
                gsm.TrySmsContent = e.Decodedmessage.TrySmsContent;//中文短信解码方式

                //写入接收数据队列
                byte[] bt = null;
                if (ServiceControl.HEXOrASC == "HEX")
                {
                    bt = EnCoder.HexStrToByteArray(e.Decodedmessage.SmsContent);
                }
                else
                {
                    bt = Encoding.UTF8.GetBytes(e.Decodedmessage.SmsContent);
                }
                GsmBussiness.WriteGrdQ(gsm, e.Decodedmessage.PhoneNumber, e.Decodedmessage.SendTime, bt);

                //写入透传列表
                ServiceBussiness.WriteQDM(bt);
                //写入日志
                LogInfoToTxt(ServiceEnum.NFOINDEX.GSM, e.Decodedmessage.PhoneNumber, bt);

                //解析数据包(更新手机号列表)
                GsmBussiness.ResolvePacket(gsm);
            }
        }
        #endregion
   

        #region TCP  //上线和下线都注释了，接收里面有upd
        void tcp_OnDisconnected(object sender, TcpService.DisconnectedEventArgs e)
        {
            try
            {
                TcpService.TcpServer tcp = sender as TcpService.TcpServer;
                //下线通知
                TcpService.TcpBussiness.TcpDisconnected(tcp, e.ClientSocket);

                //从连接列表删除
                TcpService.TcpBussiness.DelSocket(tcp, e.ClientSocket);
            }
            catch (Exception ex) 
            {
                log.Error(DateTime.Now + "下线操作异常" + ex.ToString());
            }
        }

        void tcp_OnReceivedData(object sender, TcpService.ReceivedDataEventArgs e)
        {
            try{
            TcpService.TcpServer tcp = sender as TcpService.TcpServer;
            //写入接收数据队列
            TcpService.TcpBussiness.WriteTrdQ(tcp, e.ClientSocket, e.RevData);

            //Console.WriteLine(Encoding.GetEncoding("gb2312").GetString(e.RevData));

            //写入透明传输的数据报列表
            ServiceBussiness.WriteQDM(e.RevData);
            //写入日志
            System.Net.IPEndPoint ipep=(System.Net.IPEndPoint)e.ClientSocket.RemoteEndPoint;
            LogInfoToTxt(ServiceEnum.NFOINDEX.TCP, ipep.Address + ":" + ipep.Port, e.RevData);

            //写入待发送至界面的数据处理列表(透明传输代替)
            //ServiceBussiness.WriteQRDM(tcp.ServiceID, e.RevData);

            //解析数据包
            TcpService.TcpBussiness.ResolvePacket(tcp);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "接收处理操作异常" + ex.ToString());
            }
        }

        void tcp_OnConnected(object sender, TcpService.ConnectedEventArgs e)
        {
            try{
            TcpService.TcpServer tcp = sender as TcpService.TcpServer;
            //添加入连接列表
            //TcpService.TcpBussiness.AddSocket(tcp, e.ClientSocket);

            //上线通知
            TcpService.TcpBussiness. TcpConnected(tcp, e.ClientSocket);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "上线操作异常" + ex.ToString());
            }
        }
        #endregion


        #region UDP
        void udp_OnReceivedData(object sender, UdpService.ReceivedDataEventArgs e)
        {
            try
            {
                UdpServer udp = sender as UdpServer;
                //添加入连接列表
                //UdpBussiness.AddSocket(udp, e.ClientSocket);

                //写入接收数据队列
                UdpBussiness.WriteUrdQ(udp, e.ClientSocket, e.RevData);

                //写入透明传输的数据报列表
                ServiceBussiness.WriteQDM(e.RevData);
                //写入日志
                LogInfoToTxt(ServiceEnum.NFOINDEX.UDP, e.ClientSocket.Address + ":" + e.ClientSocket.Port,e.RevData);

                //写入待发送至界面的数据处理列表(透明传输代替)
                //ServiceBussiness.WriteQRDM(udp.ServiceID, e.RevData);

                //解析数据包
                UdpBussiness.ResolvePacket(udp);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now + "接收处理操作异常" + ex.ToString());
            }
        }
        #endregion


        //所有接收数据写入Info日志
        public static void LogInfoToTxt(Service.ServiceEnum.NFOINDEX nfoindex, string NFOINDEXinfo, byte[] bt) 
        {
            if (HEXOrASC == "HEX")
                log.Info(nfoindex + "|" + NFOINDEXinfo + "|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + EnCoder.ByteArrayToHexStr(bt));
            else
                log.Info(nfoindex + "|" + NFOINDEXinfo + "|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + Encoding.ASCII.GetString(bt));
        }

        public static void LogInfoToTxt(Service.ServiceEnum.NFOINDEX nfoindex, string NFOINDEXinfo, byte[] bt, string HEXOrASC)
        {
            if (HEXOrASC == "HEX")
                log.Info(nfoindex + "|" + NFOINDEXinfo + "|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + EnCoder.ByteArrayToHexStr(bt));
            else
                log.Info(nfoindex + "|" + NFOINDEXinfo + "|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + Encoding.ASCII.GetString(bt));
        }
        
        #region TcpClient通讯执行
        static void SendData(object sender)
        {
            tcpclient_SD.SendData();
        }
        #endregion




    }
}
