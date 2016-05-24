using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;

namespace TcpService
{
    public class TcpServer
    {
        #region [变量]
        Socket socket;
        string IP;
        int PORT;
        public string ServiceID;
        public List<TcpSocket> Ts;
        public TcpQueue TQ;
        TcpThread TT;

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

        /// <summary>
        /// 有一个客户端连接上来
        /// </summary>
        public event EventHandler<ConnectedEventArgs> OnConnected;

        /// <summary>
        /// 断开释放
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> OnDisconnected;
        #endregion


        public TcpServer(string Ip, int Port, string serviceID)
        {
            IP = Ip;
            PORT = Port;
            ServiceID = serviceID;

            Ts = new List<TcpSocket>();
            TQ = new TcpQueue();
            TT = new TcpThread(this);
        }
       
        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP),PORT));
            socket.Listen(5000);
            socket.BeginAccept(new AsyncCallback(OnConnectRequest), socket);

            IsOpen = true ;
        }

        public void Stop() 
        {
            socket.Close();
            socket.Dispose();

            IsOpen = false;
        }

        #region 私有方法
        //得到连接
        private void OnConnectRequest(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;//原始socket
            Socket sock = listener.EndAccept(ar);
            listener.BeginAccept(new AsyncCallback(OnConnectRequest), listener);

            if (this.OnConnected != null)
                this.OnConnected(this, new ConnectedEventArgs(sock));

            byte[] temp = new byte[0];
            sock.BeginReceive(temp, 0, 0, SocketFlags.None, new AsyncCallback(OnRecievedData), sock);
        }

        //接收数据
        private void OnRecievedData(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;

                if (!socket.Connected)
                {
                    if (this.OnDisconnected != null)
                        this.OnDisconnected(this, new DisconnectedEventArgs(socket));
                    return;
                }

                int RecievedSize = socket.Available;
                if (RecievedSize == 0)
                {
                    if (this.OnDisconnected != null)
                        this.OnDisconnected(this, new DisconnectedEventArgs(socket));
                    return;
                }

                byte[] RecievedData = new byte[RecievedSize];

                socket.Receive(RecievedData);


                ///////////////////////////////////////////////////////////////////////////
                if (this.OnReceivedData != null)
                    this.OnReceivedData(this, new ReceivedDataEventArgs(socket, RecievedData));


                RecievedData = new byte[0];

                socket.BeginReceive(RecievedData, 0, 0, SocketFlags.None, new AsyncCallback(OnRecievedData), socket);
            }
            catch
            { }
           
        }

        //发送数据(未使用)
        private void SendData(IAsyncResult ar)
        {
            SendDataEventArgs arg = (SendDataEventArgs)ar.AsyncState;
            Socket sock = arg.ClientSocket;
            int send = sock.EndSend(ar);

            if (this.OnSendData != null)
                this.OnSendData(this, new SendDataEventArgs(sock, arg.SendData));
        }
        #endregion

        
    }


    //连接事件
    public class ConnectedEventArgs : EventArgs
    {
        #region 
        public readonly Socket ClientSocket = null;
        public ConnectedEventArgs(Socket clientsocket)
        {
            ClientSocket = clientsocket;
        }
        #endregion
    }
    //接收事件
    public class ReceivedDataEventArgs : EventArgs
    {
        #region 
        public readonly Socket ClientSocket = null;
        public readonly byte[] RevData = null;
        public ReceivedDataEventArgs(Socket clientsocket, byte[] revdata)
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
    //断开事件
    public class DisconnectedEventArgs : EventArgs
    {
        #region
        public readonly Socket ClientSocket = null;
        public DisconnectedEventArgs(Socket clientsocket)
        {
            ClientSocket = clientsocket;
        }
        #endregion
    }
}
