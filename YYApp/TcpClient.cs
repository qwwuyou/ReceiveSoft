using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace YYApp
{
    public class TcpClient
    {

        #region [变量]
        public Socket socket;
        string IP;
        int PORT;
        string ServiceID;
        #endregion

        #region [事件]
        /// <summary>
        /// 接收数据
        /// </summary>
        public event EventHandler<ReceivedDataEventArgs> OnReceivedData;
        #endregion
        
        public TcpClient(string Ip, int Port, string serviceID)
        {
            IP = Ip;
            PORT = Port;
            ServiceID = serviceID;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //启动
        public void Start()
        {
            try
            {
                socket.BeginConnect(IPAddress.Parse(IP), PORT, new AsyncCallback(OnConnect), socket);
            }
            catch
            {}
        }

        //停止
        public void Stop()
        {
            try
            {
                socket.Close();
            }
            catch { }
        }

        //异步连接
        private void OnConnect(IAsyncResult ar) 
        {
            Socket s = (Socket)ar.AsyncState;
            try
            {
                s.EndConnect(ar);


                s.BeginReceive(new byte[0], 0, 0, SocketFlags.None, new AsyncCallback(OnRecievedData), s);

                s.Send(Encoding.UTF8.GetBytes("+\n"));
                socket = s;
            }
            catch
            {}
        }

        //异步循环接收
        private void OnRecievedData(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;

                int RecievedSize = socket.Available;

                byte[] RecievedData = new byte[RecievedSize];

                socket.Receive(RecievedData);

                if (RecievedSize == 0)
                {
                    s.Disconnect(true);
                    return;
                }

                if (this.OnReceivedData != null)
                    this.OnReceivedData(this, new ReceivedDataEventArgs(socket, RecievedData));


                s.BeginReceive(new byte[0], 0, 0, SocketFlags.None, new AsyncCallback(OnRecievedData), s);
            }
            catch (Exception)
            {
                Stop();
                //远程强制关闭连接时到此 --socket.Receive(RecievedData);
            }

           
        }

       
        //接收事件
        public class ReceivedDataEventArgs : EventArgs
        {
            #region
            public readonly Socket Socket = null;
            public readonly byte[] RevData = null;
            public ReceivedDataEventArgs(Socket socket, byte[] revdata)
            {
                Socket = socket;
                RevData = revdata;
            }
            #endregion
        }
    }
}
