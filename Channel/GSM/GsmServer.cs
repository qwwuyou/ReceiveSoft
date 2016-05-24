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
        public GsmModem gm;
        string PortName;
        int BaudRate;
        public string ServiceID;
        public List<GsmMobile> Gs;
        public GsmQueue GQ;
        GsmThread GT;
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
            gm = new GsmModem();
            gm.ComPort = PortName;       
            gm.BaudRate = BaudRate;
            
            try
            {
                gm.SmsRecieved += new EventHandler(gm_SmsRecieved);
                gm.Open();
            }
            catch (Exception ex) 
            { }
        }

        public void Stop()
        {
            gm.Close();
        }

        void gm_SmsRecieved(object sender, EventArgs e)
        {
            if (gm.IsOpen)
            {
                try
                {
                    int sMsgIndex = 0;
                    DecodedMessage dm = gm.ReadNewMsg(out sMsgIndex);
                    if (this.OnReceivedData != null)
                        this.OnReceivedData(this, new ReceivedDataEventArgs(gm, dm));
                }
                catch (Exception ex)
                { } 
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
