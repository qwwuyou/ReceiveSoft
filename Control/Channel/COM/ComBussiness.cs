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
        private static void AddSatellite(ComServer CS, string satellite, string STCD)
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
        public static void UpdSatellite(ComServer CS, string satellite, string STCD)
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
                    { AddSatellite(CS, satellite, STCD); }
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
        public static void WriteCrdQ(ComServer CS,string Satellite, byte[] bt)
        {
            ConcurrentQueue<ComReceivedData> Qcrd = CS.CQ.Qcrd;
            ComReceivedData crd = new ComReceivedData();
            crd.SATELLITE = Satellite;
            crd.Data = bt;
            /////////////////////////测试输出
            Console.Write(EnCoder.ByteArrayToHexStr(bt));
            if (bt.Length > 0)
                lock (Qcrd)
                {
                    Qcrd.Enqueue(crd);
                }
        }

        /// <summary>
        /// 从COM口读取数据分包，并处理状态时间等数据报
        /// </summary>
        /// <param name="CS"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ComReceivedData> Subpackage(ComServer CS, byte[] data) 
        {
            List<ComReceivedData> Lcrd = new List<ComReceivedData>();
            string str = System.Text.Encoding.ASCII.GetString(data);
            string[] Temp_Strs = str.Split(new string[] {CS.sp.NewLine  }, StringSplitOptions.None);

            bool IsCOUT = false;
            foreach (var item in Temp_Strs)
            {
                if (item.Length > 0) 
                {
                    string[] temp = item.Split(new string[] { "," }, StringSplitOptions.None);
                    if (temp[0] == "$COUT" && temp.Length >= 10) 
                    {
                         int dataLen = 0;
                         if (int.TryParse(temp[7], out dataLen) && dataLen == temp[8].Length)
                         {
                             ComReceivedData crd = new ComReceivedData();
                             crd.SATELLITE = temp[4];
                             //从原始数据报里copy报文
                             byte[] vBuffer = new byte[dataLen];
                             Array.Copy(data, data.Length - dataLen - 4, vBuffer, 0, dataLen);
                             crd.Data =vBuffer;

                             Lcrd.Add(crd);
                         }
                         IsCOUT = true;
                        //反馈给界面信息
                         ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "接收卫星", "接收数据" + temp[0] + "," + temp[1] + "," + temp[2] + "," + temp[3] + "," + temp[4] + "," + temp[5] + "," + temp[6] + "," + temp[7], new byte[] { }, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);
                    }
                    //校验状态
                    else if (temp[0] == "$CASS" && temp.Length >= 3)
                    {
                        int check = 0;
                        string Explain = "[失败]";
                        if (int.TryParse(temp[1], out check))
                        {
                            if (check == 1)
                                Explain = "[成功]";
                        }

                        //反馈给界面信息
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID,"接收卫星", "接收数据"+item,new byte[]{}, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "", "接收校验状态" + Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    //授时信息
                    else if (temp[0] == "$TINF" && temp.Length >= 3)
                    {
                        //反馈给界面信息
                        string Explain = "接收授时信息" + "[" + temp[1] + "]";
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "接收卫星", "接收数据" + item, new byte[] { }, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "",  Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                    //卫星状态
                    else if (temp[0] == "$TSTA" && temp.Length >= 15)
                    {
                        string Satellite = temp[8];
                        string Explain =
                         "                                                   通道1信号功率：" + temp[1] + "\n" +
                         "                                                   通道2信号功率：" + temp[2] + "\n" +
                         "                                                   通道1卫星波束：" + temp[3] + "\n" +
                         "                                                   通道2卫星波束：" + temp[4] + "\n" +
                         "                                                   响应波束：" + temp[5] + "\n" +
                         "                                                   信号抑制：" + (temp[6] == "0" ? "有" : "无") + "\n" +
                         "                                                   供电状态：" + (temp[7] == "0" ? "异常" : "正常");
                        int Power1 = 0;
                        int Power2 = 0;
                        int Beam1 = 0;
                        int Beam2 = 0;
                        int Response = 0;
                        int Inhibition = 0;
                        int PowerSupply = 0;
                        if (int.TryParse(temp[1], out Power1))
                        { CS.CState.Power1 = Power1; }
                        if (int.TryParse(temp[2], out Power2))
                        { CS.CState.Power2 = Power2; }
                        if (int.TryParse(temp[3], out Beam1))
                        { CS.CState.Beam1 = Beam1; }
                        if (int.TryParse(temp[4], out Beam2))
                        { CS.CState.Beam2 = Beam2; }
                        if (int.TryParse(temp[5], out Response))
                        { CS.CState.Response = Response; }
                        if (int.TryParse(temp[6], out Inhibition))
                        { CS.CState.Inhibition = Inhibition; }
                        if (int.TryParse(temp[7], out PowerSupply))
                        { CS.CState.PowerSupply = PowerSupply; }
                        CS.CState.DATATIME = DateTime.Now;
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, Satellite, "接收卫星" + item, new byte[] { }, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, Satellite, "接收到状态信息\n" + Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                    }
                    else
                    {
                        ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "", "接收异常数据" + item, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                    }
                }
            }
            if (IsCOUT) //如果接收的数据报含有$COUT指令，回复$COSS
            {
                ComBussiness.Reply(CS);
            }
            return Lcrd;
        }

        public static List<ComReceivedData> SubpackageFor4(ComServer CS, byte[] data)
        {
            
            List<ComReceivedData> Lcrd = new List<ComReceivedData>();
            string str = EnCoder.ByteArrayToHexStr(data);
 
            if (str.Length >= 10) 
            {
                string temp = str.Substring(0,10);
                if (temp == "2454585858")  //$TXXX   通讯信息
                {
                    //报文内容长度
                    long l=Convert.ToInt64(str.Substring(32, 4), 16);
                    //用户地址
                    string RevSatellite = Convert.ToInt64(str.Substring(14, 6), 16).ToString();
                    //发信方地址
                    string SendSatellite = Convert.ToInt64(str.Substring(22, 6), 16).ToString();
                    //时间
                    string Time = Convert.ToInt64(str.Substring(28, 2), 16).ToString() + "时" + Convert.ToInt64(str.Substring(30, 2), 16).ToString() + "分";

                    ComReceivedData crd = new ComReceivedData();
                    crd.SATELLITE = SendSatellite;

                    //从原始数据报里copy报文
                    byte[] vBuffer1 = new byte[l/8];

                    Array.Copy(data, 18, vBuffer1, 0, l/8);
                    crd.Data = vBuffer1;
                    Lcrd.Add(crd);


                    //IsCOUT = true;
                    //反馈给界面信息
                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "接收卫星", "接收数据,用户地址:"+RevSatellite +",发信方地址:"+SendSatellite +",时间"+Time+",数据报长度:" +l, new byte[] { }, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);
                }
                else if (temp == "245A4A5858") //$ZJXX  自检信息
                {
                    string Explain = "";
                    //用户地址
                    string RevSatellite = Convert.ToInt64(str.Substring(14, 6), 16).ToString();
                    //IC卡状态
                    string ICState = str.Substring(20, 2);
                    if (ICState == "00")//正常
                    { Explain += "IC卡状态:正常\n"; }
                    else //异常
                    { Explain += "IC卡状态:异常\n"; }
                    CS.CStateFor4.ICState = ICState;

                    //硬件状态
                    string HardwareState = str.Substring(22, 2);
                    if (HardwareState == "00")//正常
                    { Explain += "                                                   硬件状态:正常\n"; }
                    else //异常
                    { Explain += "                                                   硬件状态:异常\n"; }
                    CS.CStateFor4.HardwareState = HardwareState;

                    //电量百分比
                    string Electricity = str.Substring(24, 2);
                    Electricity=Convert.ToInt64(Electricity, 16).ToString();
                    Explain += "                                                   电量:" + Electricity + "%\n";
                    CS.CStateFor4.Electricity = Electricity;

                    //入站状态
                    string Inbound= str.Substring(26, 2);
                    if (Inbound == "00")//正常
                    { }
                    else //异常
                    { }

                    string Power = str.Substring(28, 12);
                    CS.CStateFor4.Power = Power;
                    string[] bs = new string[] { "＜-158dBW", "-156～-157dBW", "-154～-155dBW", "-152～-153dBW", "＞-152dBW" };
                    for (int i = 0; i < 6; i++)
                    {
                        string s=Power.Substring(2 * i, 2);
                        if ( s== "00") 
                        {
                            Explain +="                                                   波束"+(i+1)+"#功率:" + bs[0]+"\n";
                        }
                        else if (s == "01")
                        { Explain += "                                                   波束" + (i + 1) + "#功率:" + bs[1] + "\n"; }
                        else if (s == "02")
                        { Explain += "                                                   波束" + (i + 1) + "#功率:" + bs[2] + "\n"; }
                        else if (s == "03")
                        { Explain += "                                                   波束" + (i + 1) + "#功率:" + bs[3] + "\n"; }
                        else if (s == "04")
                        { Explain += "                                                   波束" + (i + 1) + "#功率:" + bs[4] + "\n"; }
                    }

                    CS.CStateFor4.DATATIME = DateTime.Now;

                    //将以上解析信息反馈到界面时注销下行
                    //Explain = " 服务器与卫星接收设备通讯正常";
                   
                    //反馈给界面信息
                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, RevSatellite, "接收到状态信息\n" + Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);
                }
                else if (temp == "24544A5858") //$SJXX 时间信息
                {
                    //用户地址
                    string RevSatellite = Convert.ToInt64(str.Substring(14, 6), 16).ToString();
                    //日期时间
                    string Date = Convert.ToInt64(str.Substring(20, 4), 16).ToString() + "年" + Convert.ToInt64(str.Substring(24, 2), 16).ToString() + "月" + Convert.ToInt64(str.Substring(26, 2), 16).ToString() + "日";
                    string Time = Convert.ToInt64(str.Substring(28, 2), 16).ToString() + "时" + Convert.ToInt64(str.Substring(30, 2), 16).ToString() + "分" + Convert.ToInt64(str.Substring(32, 2), 16).ToString() + "秒";

                    //反馈给界面信息
                    string Explain = "用户地址" + RevSatellite + ",接收时间信息" + "[" + Date + " " + Time + "]";
                    //ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "接收卫星", "接收数据" + item, new byte[] { }, ServiceEnum.EnCoderType.HEX, ServiceEnum.DataType.Text);

                    ServiceBussiness.WriteQUIM(ServiceEnum.NFOINDEX.COM.ToString(), CS.ServiceID, "", Explain, new byte[] { }, Service.ServiceEnum.EnCoderType.HEX, Service.ServiceEnum.DataType.Text);

                }
            }


            
            return Lcrd;
        }


        /// <summary>
        /// 将回复数据放入队列
        /// </summary>
        /// <param name="CS">COM服务</param>
        /// <param name="STCD">测站编号</param>
        /// <param name="bt">数据</param>
        public static void WriteCsdQ(ComServer CS, string STCD, byte[] bt, string CommandCode)
        {
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;

            var qcsd = from c in Qcsd where c.STCD == STCD && c.COMMANDCODE == CommandCode select c;
            List<ComSendData> QCSD = qcsd.ToList<ComSendData>();
            lock (Qcsd)
                if (QCSD.Count() > 0)
                {
                    QCSD.First().Data = bt;
                }
                else
                {
                    ComSendData csd = new ComSendData();
                    csd.Data = bt;
                    csd.STCD = STCD;
                    csd.COMMANDCODE = CommandCode;
                    Qcsd.Enqueue(csd);
                }
        }

        /// <summary>
        /// 移除召测命令集中的命令
        /// </summary>
        /// <param name="GS">gsm服务</param>
        /// <param name="STCD">站号</param>
        /// <param name="CommandCode">命令码</param>
        public static void RemoveCsdQ(ComServer CS, string STCD, string CommandCode)
        {
            ConcurrentQueue<ComSendData> Qcsd = CS.CQ.Qcsd;
            lock (Qcsd)
            {
                for (int i = 0; i < CS.CQ.Qcsd.Count ; i++)
                {
                    ComSendData csd = null;
                    if (Qcsd.TryDequeue(out csd))
                    {
                        if (csd.STCD != STCD || csd.COMMANDCODE != CommandCode)
                        {
                            Qcsd.Enqueue(csd);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空召测命令集中的命令
        /// </summary>
        /// <param name="CS">com服务</param>
        public static void ClearCsdQ(ComServer CS)
        {
            lock (CS.CQ.Qcsd)
            {
                CS.CQ.Qcsd = new ConcurrentQueue<ComSendData>();
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

                                ServiceBussiness.WriteQUIM("COM", ServiceId, temp.First().STCD, "回复数据", cs.Data,Service.ServiceEnum.EnCoderType.HEX,Service.ServiceEnum.DataType.Text);
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
        /// 发送命令方法，短信仅发一次，业务逻辑
        /// </summary>
        /// <param name="GS">GSM服务</param>
        public static void SendCommand(ComServer CS)
        {
            Reflection_Protoco.SendCommand(CS);
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="CS">COM服务</param>
        public static void ResolvePacket(ComServer CS)
        {
            Reflection_Protoco.PacketArrived(CS);
        }

        /// <summary>
        /// 信道启动时将命令读取到命令列表
        /// </summary>
        public static void ToQcsd()
        {
            var cmds = from cmd in ServiceControl.LC where cmd.SERVICETYPE == "COM" select cmd;
            foreach (var c in ServiceControl.com)
            {
                foreach (var item in cmds)
                {
                    if(c!=null)
                    ComService.ComBussiness.WriteCsdQ(c, item.STCD, EnCoder.HexStrToByteArray(item.Data), item.CommandID);
                }
            }
        }

        /// <summary>
        /// 发送卫星状态问询命令
        /// </summary>
        public static void GetComState()
        {
            foreach (ComServer item in Service.ServiceControl.com)
            {
                if (item.sp.IsOpen)
                {
                    lock (item.sp)
                    {
                        byte[] vBuffer = ASCIIEncoding.ASCII.GetBytes("$QSTA,0,");
                        ComBussiness.XorSAT(ref vBuffer);
                        item.sp.Write(vBuffer, 0, vBuffer.Length);
                        //byte[] enter=ASCIIEncoding.ASCII.GetBytes(item.sp.NewLine); //加回车
                        //Array.Resize(ref vBuffer, vBuffer.Length + enter.Length );
                        //Array.Copy(enter, 0, vBuffer, vBuffer.Length - enter.Length, enter.Length);
                        
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);
                    }
                }
            }
        }

        public static void GetComStateFor4()
        {
            foreach (ComServer item in Service.ServiceControl.com)
            {
                if (item.sp.IsOpen)
                {
                    lock (item.sp)
                    {
                        long satellite=0;
                        string Satellite="000000";
                        if(long.TryParse(item.Satellite,out satellite))
                        {
                            Satellite = satellite.ToString("X").PadLeft(6, '0');
                        }

                        
                       


                        //XTZJ
                        byte[] a = EnCoder.HexStrToByteArray("2458545A4A000D" + Satellite + "0000");
                        byte b = xor(a);//异或校验
                        byte[] vBuffer = copybyte(a, b);//追加异或校验

                        //byte[] enter = ASCIIEncoding.ASCII.GetBytes(item.sp.NewLine); //加回车
                        //Array.Resize(ref vBuffer, vBuffer.Length + enter.Length);
                        //Array.Copy(enter, 0, vBuffer, vBuffer.Length - enter.Length, enter.Length);

                        //ComBussiness.XorSAT(ref vBuffer);
                        item.sp.Write(vBuffer, 0, vBuffer.Length);
                       

                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);

                    }
                }
            }
        }

        /// <summary>
        /// 发送卫星时间问询命令
        /// </summary>
        public static void GetComTime()
        {
            foreach (ComServer item in Service.ServiceControl.com)
            {
                if (item.sp.IsOpen)
                {
                    lock (item.sp)
                    {
                        //item.sp.WriteLine("$TINF,13:56:41.00,   " + "\n");---返回的结果

                        byte[] vBuffer = ASCIIEncoding.ASCII.GetBytes("$TAPP,");
                        ComBussiness.XorSAT(ref vBuffer);
                        item.sp.Write(vBuffer, 0, vBuffer.Length);


                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);
                    }
                }
            }
        }

        public static void GetComTimeFor4()
        {
            foreach (ComServer item in Service.ServiceControl.com)
            {
                if (item.sp.IsOpen)
                {
                    lock (item.sp)
                    {
                        long satellite = 0;
                        string Satellite = "000000";
                        if (long.TryParse(item.Satellite, out satellite))
                        {
                            Satellite = satellite.ToString("X").PadLeft(6, '0');
                        }

                        byte[] a = EnCoder.HexStrToByteArray("24534A5343000D" + Satellite + "0000");
                        byte b = xor(a);//异或校验
                        byte[] vBuffer = copybyte(a, b);//追加异或校验

                        //ComBussiness.XorSAT(ref vBuffer);
                        item.sp.Write(vBuffer, 0, vBuffer.Length);


                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);
                    }
                }
            }
        }


        //卫星验证异或
        private static byte xor(byte[] data)
        {
            byte cc = 0;
            foreach (byte bb in data)
            {
                cc ^= bb;
            }
            return cc;
        }
        //byte[]追加byte
        public static byte[] copybyte(byte[] a, byte b)
        {
            byte[] c = new byte[a.Length + 1];
            Array.Copy(a, c, a.Length);
            c[a.Length] = b;
            return c;
        }

        /// <summary>
        /// 发送召侧命令
        /// </summary>
        /// <param name="CS"></param>
        /// <param name="satellite"></param>
        /// <param name="msg"></param>
        public static void SendCommand(ComServer CS, string satellite, byte[] msg) 
        {
            if (CS.sp.IsOpen)
            {
                lock (CS.sp)
                {
                    byte[] vBuffer = ASCIIEncoding.ASCII.GetBytes("$TTCA,1," + satellite + ",1,0" + msg.Length + "," );
                    Array.Resize(ref vBuffer, vBuffer.Length + msg.Length + 1);
                    Array.Copy(msg, 0, vBuffer, vBuffer.Length-1, msg.Length);
                    vBuffer[vBuffer.Length - 1] = (byte)0x2C;

                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);
                }
            }
        }

        /// <summary>
        /// 收到上报数据给卫星回复
        /// </summary>
        /// <param name="CS"></param>
        public static void Reply(ComServer CS)
        {
            if (CS.sp.IsOpen)
            {
                lock (CS.sp)
                {
                    byte[] vBuffer = ASCIIEncoding.ASCII.GetBytes("$COSS,1,");
                    ComBussiness.XorSAT(ref vBuffer);
                    CS.sp.Write(vBuffer, 0, vBuffer.Length);

                    
                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer, "ASC");
                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", vBuffer);
                }
            }
        }

        #region 杜成龙提供验证卫星报相关方法
         /// <summary>
        /// 验证数据是否符合异或验证
        /// </summary>
        /// <param name="vBuffer">要验证的内容,最后一个字节是异或的结果</param>
        /// <returns>是否符合验证</returns>
        public static bool XorSATVerify(byte[] vBuffer)
        {
            if (vBuffer == null || vBuffer.Length < 1)
                throw new ArgumentNullException("要验证校验的数据不能为空");

            byte xorValue = vBuffer[0];
            //按顺序取异或
            for (int i = 1; i < vBuffer.Length - 1; i++)
                xorValue ^= vBuffer[i];
            return xorValue.Equals(vBuffer[vBuffer.Length - 1]);

        }

        /// <summary>
        /// 对数据顺序异或,并且
        /// </summary>
        /// <param name="vBuffer"></param>
        public static void XorSAT(ref byte[] vBuffer)
        {
            if (vBuffer == null || vBuffer.Length < 1)
                throw new ArgumentNullException("要校验的数据不能为空");
            byte xorValue = vBuffer[0];
            //按顺序取异或
            for (int i = 1; i < vBuffer.Length; i++)
                xorValue ^= vBuffer[i];
            //将校验结果添加到末尾
            Array.Resize(ref vBuffer, vBuffer.Length + 1+2);
            vBuffer[vBuffer.Length - 3] = xorValue;
            vBuffer[vBuffer.Length - 2] = 0x0D;
            vBuffer[vBuffer.Length - 1] = 0x0A;
            
        }


        #endregion
    }
}
