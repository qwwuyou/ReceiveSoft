using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using GSMMODEM;

namespace GsmService
{
    public class GsmServer
    {
        #region [变量]
        public GsmModem gm=null;
        string PortName;
        int BaudRate;
        public string ServiceID;
        public List<GsmMobile> Gs;
        public GsmQueue GQ;
        GsmThread GT;
        public string TrySmsContent;  
        #endregion

        #region [事件]
        /// <summary>
        /// 接收数据
        /// </summary>
        public event EventHandler<ReceivedDataEventArgs> OnReceivedData;

        /// <summary>
        /// 发送数据(未使用)
        /// </summary>
        public event EventHandler<SendDataEventArgs> OnSendData;
        #endregion

        public GsmServer(string Portname, int Baudrate,string serviceID)
        {
            PortName = Portname;
            BaudRate = Baudrate;
            ServiceID = serviceID;

            Gs = GsmBussiness.GetGsmMobileList();
            GQ = new GsmQueue();
            GT = new GsmThread(this);
        }

        public void Start()
        {
            //第一次启动
            if (gm==null)
            {

                gm = new GsmModem();
                gm.ComPort = PortName;
                gm.BaudRate = BaudRate;
                gm.AutoDelMsg = true;

                gm.SmsRecieved += new EventHandler(gm_SmsRecieved);
                gm.Open(12 * 1000);
              
                GsmBussiness.ToQgsd();
            }
            //重启
            else if (gm != null && !gm.IsOpen)
            {
                try
                {
                    Gs = GsmBussiness.GetGsmMobileList();
                    gm = new GsmModem();
                    gm.ComPort = PortName;
                    gm.BaudRate = BaudRate;
                    gm.AutoDelMsg = true;

                    gm.SmsRecieved += new EventHandler(gm_SmsRecieved);
                    gm.Open(12 * 1000);

                    GsmBussiness.ToQgsd();
                }
                catch(Exception ex)
                {
                    Service.ServiceControl.log.Warn(DateTime.Now +"gsm服务(" + gm.ComPort + ":" + gm.BaudRate + ")重启失败！", ex); 
                }
            }
        }

        public void Stop()
        {
            try
            {
                if (gm.IsOpen)
                {
                    gm.SmsRecieved -= new EventHandler(gm_SmsRecieved);
                    gm.Close();
                }
            }
            catch (Exception ex)
            { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
        }
        static int ii = 0;
        void gm_SmsRecieved(object sender, EventArgs e)
        {
            lock(this)
            if (gm.IsOpen)
            {
                try
                {
                    int sMsgIndex = 0;
                    DecodedMessage dm = gm.ReadNewMsg(out sMsgIndex);
                    
                    ii++;
                    Console.WriteLine(ii+"   "+dm.SmsContent);///////////////////////////
                    Service.ServiceControl.log.Error(  "-----------" + ii + "-------------"+DateTime.Now);


                    if (this.OnReceivedData != null)
                        this.OnReceivedData(this, new ReceivedDataEventArgs(gm, dm));
                }
                catch (Exception ex)
                {
                    Service.ServiceControl.log.Error(DateTime.Now + "接收出现异常," + ex.ToString());

                    try
                    {
                        Stop();
                        System.Threading.Thread.Sleep(5 * 1000);
                        Start();
                        Console.WriteLine("接收出现异常，重启了！");
                    }
                    catch (Exception ee)
                    { Service.ServiceControl.log.Error(DateTime.Now +"重启异常,gsm信道目前状态:"+gm.IsOpen.ToString()+"   "+ ee.ToString()); }
                }
            }
        }

        //水资源项目专用
        public void SendData(string phone, byte[] msg)
        {
            if (gm.IsOpen)
            {
                gm.SendMsg(phone, msg);
            }
        }

        public void SendData(string phone, string msg)
        {
            if (gm.IsOpen)
            {
                gm.SendMsg(phone, msg);
            }
        }
    }

    //接收事件
    public class ReceivedDataEventArgs : EventArgs
    {
        #region
        public readonly GsmModem  Gsmmodel = null;
        public readonly DecodedMessage Decodedmessage = null;
        public ReceivedDataEventArgs(GsmModem gsmmodel, DecodedMessage decodedmessage)
        {
            Gsmmodel = gsmmodel;
            Decodedmessage = decodedmessage;
        }
        #endregion
    }

    //发送事件(未使用)
    public class SendDataEventArgs : EventArgs
    {
        #region
        public readonly GsmModem Gsmmodel = null;
        public readonly byte[] SendData = null;
        public SendDataEventArgs(GsmModem gsmmodel, byte[] senddata)
        {
            Gsmmodel = gsmmodel;
            SendData = senddata;
        }
        #endregion
    }
}
