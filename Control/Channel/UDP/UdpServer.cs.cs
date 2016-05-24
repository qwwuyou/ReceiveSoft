using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Service;

namespace UdpService
{
    public class UdpServer
    {
        #region [变量]
        public UdpClient UDPClient;
        string IP;
        public int PORT;
        public string ServiceID;
        /// <summary>
        /// udp在线列表
        /// </summary>
        public List<UdpSocket> Us;
        public UdpQueue UQ;
        UdpThread UT;

        public bool IsOpen = false;
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

        public UdpServer(string Ip, int Port, string serviceID)
        {
            IP = Ip;
            PORT = Port;
            ServiceID = serviceID;

            Us = new List<UdpSocket>();
            UQ = new UdpQueue();
            UT = new UdpThread(this);
        }

        private void Us_Init()
        {
            foreach (var item in Service.ServiceBussiness.RtuList)
            {
                UdpSocket us = new UdpSocket();
                us.STCD = item.STCD;
                Us.Add(us);
            }
        }

        public void Start()
        {
            Us_Init();
            if (!IsOpen)
            {
                //(new Service.ServiceControl()).ToQxsd(Service.ServiceEnum.NFOINDEX.UDP);
                UDPClient = new UdpClient(new IPEndPoint(IPAddress.Parse(IP), PORT));
                UDPClient.BeginReceive(new AsyncCallback(OnRecievedData), UDPClient);
                #region 改代码段设置解决了UDP通讯的10054异常
                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                UDPClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                #endregion
                IsOpen = true;

                UdpBussiness.ToQusd();
            }
        }

        public void Stop()
        {
            if (IsOpen)
            {
                UDPClient.Close();
                Us.Clear();

                IsOpen = false;
            }
        }

        //接收数据
        private void OnRecievedData(IAsyncResult ar)
        {

            UdpClient udpclient = (System.Net.Sockets.UdpClient)ar.AsyncState;
            IPEndPoint ipendpoint = null;

            try
            {
                byte[] receiveBytes = udpclient.EndReceive(ar, ref ipendpoint);
                udpclient.BeginReceive(new AsyncCallback(OnRecievedData), udpclient);


                if (this.OnReceivedData != null)
                    this.OnReceivedData(this, new ReceivedDataEventArgs(ipendpoint, receiveBytes));
            }
            catch (Exception ex)
            { 
                IsOpen = false;
                try
                {
                    UdpBussiness.DelSocket(this, ipendpoint);

                    UdpBussiness.UdpDisconnected(this, ipendpoint);

                    UDPClient.Client.Dispose();
                    Start();
                }
                catch (Exception e)
                { Service.ServiceControl.log.Error(DateTime.Now + e.ToString()); }
            }

          
        }

        //发送数据
        private void SendData(IAsyncResult ar)
        {
            try
            {
                SendDataEventArgs arg = (SendDataEventArgs)ar.AsyncState;
                Socket sock = arg.ClientSocket;
                int send = sock.EndSend(ar);

                if (this.OnSendData != null)
                    this.OnSendData(this, new SendDataEventArgs(sock, arg.SendData));
            }
            catch (Exception ex)
            { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
           
        }
    }

    //接收事件
    public class ReceivedDataEventArgs : EventArgs
    {
        #region
        public readonly IPEndPoint ClientSocket = null;
        public readonly byte[] RevData = null;
        public ReceivedDataEventArgs(IPEndPoint clientsocket, byte[] revdata)
        {
            ClientSocket = clientsocket;
            RevData = revdata;
        }
        #endregion
    }


    //发送事件(未使用)
    public class SendDataEventArgs : EventArgs
    {
        #region
        public readonly Socket ClientSocket = null;
        public readonly byte[] SendData = null;
        public SendDataEventArgs(Socket clientsocket, byte[] senddata)
        {
            ClientSocket = clientsocket;
            SendData = senddata;
        }
        #endregion
    }
}
