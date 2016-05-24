using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Service;

namespace ToUI
{
    class TcpClient
    {

        #region [变量]
        public Socket socket;
        string IP;
        int PORT;
        string ServiceID;
        int k = 0;        //控制连接服务器时间的计数器
        bool B = false;   //socket在线标识=IsOpen
        #endregion

        #region [事件]
        /// <summary>
        /// 接收数据
        /// </summary>
        public event EventHandler<ReceivedDataEventArgs> OnReceivedData;
        #endregion

        /// <summary>
        /// 连接状态属性
        /// </summary>
        public bool Connected
        {
            get { return socket.Connected; }
        }

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
            catch (Exception ex)
            { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
        }

        //停止  //因为启动使用的socket.BeginConnect方法，所以不用socket.Close()，而用socket.Disconnect(true);
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
                B = true;
            }
            catch (Exception ex)
            { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
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
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
                //远程强制关闭连接时到此 --socket.Receive(RecievedData);
                //Stop();
                socket.Disconnect(true);
            }

           
        }


        //发送数据到界面
        public void TellUI()
        {
            #region 判断在线状态，是否重新连接
            k++;
            if (k >= 100)    //10秒1个心跳
            {
                k = 0;
                try
                {
                    socket.Send(Encoding.UTF8.GetBytes("+"));
                    B = true;
                }
                catch
                {
                    try
                    {
                        B = false;
                        socket.Disconnect(true);
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                }


                if (!B)//发送数据失败重新异步连接
                {
                    Start();
                }
            }
            #endregion


            //发送列表中的数据
            if (B)
            {
                while (ServiceQueue.QUIM.Count > 0)
                {
                    UIModel item = null;
                    ServiceQueue.QUIM.TryDequeue(out item);
                    //item = ServiceQueue.QUIM.Dequeue();
                    if (item != null)
                    {
                        try
                        {
                            socket.Send(Encoding.UTF8.GetBytes(item.EXPLAIN));
                            System.Threading.Thread.Sleep(1);
                        }
                        catch
                        {
                            B = false;
                            socket.Disconnect(true);
                        }
                    }

                }
                   
            }
        }

        //各信道接收的数据存放在此列表，并发送
        public void SendData() 
        {
            #region 判断在线状态，是否重新连接
            k++;
            if (k >= 100)    //10秒1个心跳
            {
                k = 0;
                try
                {
                    socket.Send(Encoding.UTF8.GetBytes("+"));
                    B = true;
                }
                catch
                {
                    try
                    {
                        B = false;
                        socket.Disconnect(true);
                    }
                    catch (Exception ex)
                    { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
                }


                if (!B)//发送数据失败重新异步连接
                {
                    Start();
                }
            }
            #endregion


            //发送列表中的数据
            if (B)
            {
                while (ServiceQueue.QDM.Count > 0)
                {
                    DataModel item = null;
                    ServiceQueue.QDM.TryDequeue(out item);
                    if (item != null)
                    {
                        try
                        {
                            socket.Send(item.Data);
                            System.Threading.Thread.Sleep(10);
                        }
                        catch
                        { 
                            B = false;
                            socket.Disconnect(true);
                        }
                    }

                }
                    
            }
        }


        public void SendCenterData(byte[] data) 
        {
            try
            {
                socket.Send(data);
                System.Threading.Thread.Sleep(10);
            }
            catch
            {
                socket.Disconnect(true);
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
