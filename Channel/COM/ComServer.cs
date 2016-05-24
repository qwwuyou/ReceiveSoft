using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace ComService
{
    public class ComServer
    {
        #region [变量]
        public SerialPort sp;
        string PortName;
        int BaudRate;
        public string ServiceID;
        public List<ComSatellite> Cs;
        public ComQueue CQ;
        ComThread CT;
        #endregion

        #region [事件]
        /// <summary>
        /// 接收数据
        /// </summary>
        public event EventHandler<ReceivedDataEventArgs> OnReceivedData;

        /// <summary>
        /// 发送数据(未使用)
        /// </summary>
        //public event EventHandler<SendDataEventArgs> OnSendData;

        /// <summary>
        /// 有一个客户端连接上来
        /// </summary>
        //public event EventHandler<ConnectedEventArgs> OnConnected;

        /// <summary>
        /// 断开释放
        /// </summary>
        //public event EventHandler<DisconnectedEventArgs> OnDisconnected;
        #endregion

        public ComServer(string Portname, int Baudrate,string serviceID)
        {
            PortName = Portname;
            BaudRate = Baudrate;
            ServiceID = serviceID;

            Cs = ComBussiness.GetComSatelliteList();
            CQ = new ComQueue();
            CT = new ComThread(this);
        }
       
        public void Start()
        {
            sp = new SerialPort();
            sp.PortName = PortName;
            sp.BaudRate = BaudRate;

            try
            {
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                sp.Open();
            }
            catch (Exception ex)
            { }
        }

        public void Stop()
        {
            sp.Close();
            sp.Dispose();
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp.IsOpen)
            {
                try
                {
                    string str = sp.ReadLine();
                    if (str.Length > 0)
                    {
                        byte[] data = Encoding.ASCII.GetBytes(str);
                        if (this.OnReceivedData != null)
                            this.OnReceivedData(this, new ReceivedDataEventArgs(sp, data));
                    }
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
        public readonly SerialPort serialPort = null;
        public readonly byte[]  Data = null;
        public ReceivedDataEventArgs(SerialPort serialport, byte[] data)
        {
            serialPort = serialport;
            Data = data;
        }
        #endregion
    }
}
