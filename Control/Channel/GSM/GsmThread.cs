using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GsmService
{
    class GsmThread
    {
        GsmServer gsm;
            
        /// <summary>
        /// 回复数据的线程
        /// </summary>
        Timer timer_CheckSerialPort;

        public GsmThread(GsmServer Gsm) 
        {
            gsm = Gsm;


            Thread timer_ReadGSMCard = new Thread(new ThreadStart(ReadGSMCard));
            timer_ReadGSMCard.Start();

            //检查召测列表中是否有命令数据
            Thread timer_SendData = new Thread(new ThreadStart(SendData));
            timer_SendData.Start();

            //职守线程每5分钟检查1次与串口交互时间，如5分钟内无交互，重启串口。
            timer_CheckSerialPort = new Timer(new TimerCallback(CheckSerialPort), null, 5 * 60 * 1000, 5 * 60 * 1000);
        }

        //召测方法
        void SendData()
        {
            Thread.Sleep(30 * 1000);
            while (true)
            {
                if (gsm.gm.IsOpen)//串口打开
                {
                    try
                    {
                        //Console.WriteLine("AAAAAAAAA"+ DateTime.Now );
                        //GsmBussiness.SendData(gsm);
                        GsmBussiness.SendCommand(gsm);
                        //Console.WriteLine("BBBBBBBBB" + DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        Service.ServiceControl.log.Error(DateTime.Now+ ex.ToString());
                    }
                }
                Thread.Sleep(1000);
            }
        }

        //读取GSM卡里的指定信息
        void ReadGSMCard()
        {
            Thread.Sleep(60*1000);//30秒后开始执行该方法
            while(true) //串口打开
            {
                lock(gsm)
                if (gsm.gm.IsOpen)
                {  
                    //与串口交互后，如有异常重启串口
                    Restart(gsm);

                    if (gsm.GQ.Qgsd.Count == 0)  //无召测命令
                    {
                        lock (gsm.GQ.Qgsd)  //锁住召测列表避免发送信息与读取信息同时进行，导致异常
                        {
                            string sResult = "";

                            try
                            {
                                //从卡里读出未读（0）信息
                                List<GSMMODEM.DecodedMessage> dms = gsm.gm.GetReceiveMsg(0, out sResult);
                                foreach (var item in dms)
                                {
                                    GSMMODEM.DecodedMessage dm = item;
                                    gsm.gm.DeleteMsgByIndex(dm.SmsIndex);  //根据索引删除
                                    byte[] bt = null;
                                    if (Service.ServiceControl.wrx.XMLObj.HEXOrASC == "HEX")
                                    {
                                        bt = Service.EnCoder.HexStrToByteArray(dm.SmsContent);
                                    }
                                    else
                                    {
                                        //bt = Encoding.ASCII.GetBytes(dm.SmsContent);
                                        bt = Encoding.UTF8.GetBytes(dm.SmsContent);
                                    }
                                    GsmBussiness.WriteGrdQ(gsm, dm.PhoneNumber, dm.SendTime, bt);

                                    //写入透传列表
                                    Service.ServiceBussiness.WriteQDM(bt);
                                    //写入日志
                                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.GSM, dm.PhoneNumber, bt);

                                    //解析数据包(更新手机号列表)
                                    GsmBussiness.ResolvePacket(gsm);

                                    Thread.Sleep(200);
                                }
                            }
                            catch (Exception ex)
                            {
                                Service.ServiceControl.log.Error(DateTime.Now+"读出卡中未读信息出现异常，" + ex.ToString());
                            }

                            try
                            {
                                Thread.Sleep(200);
                                //从卡里读出已读（1）信息
                                List<GSMMODEM.DecodedMessage> dms = gsm.gm.GetReceiveMsg(1, out sResult);
                                foreach (var item in dms)
                                {
                                    GSMMODEM.DecodedMessage dm = item;
                                    gsm.gm.DeleteMsgByIndex(dm.SmsIndex);//根据索引删除
                                    byte[] bt = null;
                                    if (Service.ServiceControl.wrx.XMLObj.HEXOrASC == "HEX")
                                    {
                                        bt = Service.EnCoder.HexStrToByteArray(dm.SmsContent);
                                    }
                                    else
                                    {
                                        //bt = Encoding.ASCII.GetBytes(dm.SmsContent);
                                        bt = Encoding.UTF8.GetBytes(dm.SmsContent);
                                    }
                                    GsmBussiness.WriteGrdQ(gsm, dm.PhoneNumber, dm.SendTime, bt);

                                    //写入透传列表
                                    Service.ServiceBussiness.WriteQDM(bt);
                                    //写入日志
                                    Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.GSM, dm.PhoneNumber, bt);
                                    
                                    //解析数据包(更新手机号列表)
                                    GsmBussiness.ResolvePacket(gsm);

                                    Thread.Sleep(200);
                                }
                            }
                            catch (Exception ex)
                            {
                                Service.ServiceControl.log.Error(DateTime.Now + "读出卡中已读信息出现异常，" + ex.ToString());
                            }

                            Thread.Sleep(800);
                        }
                    }

                }
                else
                {
                    //重启串口
                    gsm.Stop();
                    Thread.Sleep(3 * 1000);
                    gsm.Start(); 
                    Console.WriteLine(DateTime .Now +" "+"||||Restart！");
                    Service.ServiceControl.log.Warn(DateTime.Now + "串口状态为关闭，Restart！");
                }
                Thread.Sleep(30*1000);
            }
        }

        DateTime dtime = DateTime.Now;
        //与串口交互后，如有异常重启串口
        void Restart(GsmServer gsm) 
        {
            string result = gsm.gm.SendAT("AT+CGMI");
            if (result == null||!result.Contains("OK"))
            {
                try
                {
                    gsm.Stop();
                    Thread.Sleep(3 * 1000);
                    gsm.Start();
                    Console.WriteLine(DateTime.Now + "定时与串口交互异常，" + "Restart！");
                }
                catch (Exception ex)
                { Console.WriteLine(DateTime.Now + ex.ToString()); }
            }
            else
            { 
                Console.WriteLine(DateTime.Now + " " + "living！");
            }

            dtime = DateTime.Now;
        }

        //职守方法每5分钟检查1次与串口交互时间，如5分钟内无交互，重启串口
        void CheckSerialPort(object sender)
        {
            TimeSpan ts = DateTime.Now.Subtract(dtime);
            if (ts.Minutes > 5)
            {
                try
                {
                    gsm.Stop();
                    Thread.Sleep(3 * 1000);
                    gsm.Start();
                    Console.WriteLine(DateTime.Now + " gsm职守 " + "Restart！");
                }
                catch (Exception ex)
                { Console.WriteLine(DateTime.Now + ex.ToString()); }
            }
        }
    }
}
