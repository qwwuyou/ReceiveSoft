using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace CenterApp
{
    public static class TcpControl
    {
        static TcpClient tcpclient;

        public static string UserID = "";

        /// <summary>
        /// 连接状态属性
        /// </summary>
        public static bool Connected
        {
            get {return tcpclient.socket.Connected;  }
        }


        public static void TcpClient_Init(string IP, string Port)
        {
            if (IP != "" && Port != "")
            {
                tcpclient = new TcpClient(IP, int.Parse(Port), "");
                tcpclient.Start();
                tcpclient.OnReceivedData += new EventHandler<TcpClient.ReceivedDataEventArgs>(tcpclient_OnReceivedData);
            }

            Thread ReConnect = new Thread(new ThreadStart(ThreadReConnect));
            ReConnect.IsBackground = true;
            ReConnect.Start();

            Thread Heartbeat = new Thread(new ThreadStart(SendData));
            Heartbeat.IsBackground = true;
            Heartbeat.Start();
        }

      

        public static void ReStart()
        {
            tcpclient.ReInit(Program.wrx.XMLObj.LsM[0].IP_PORTNAME ,Program.wrx.XMLObj.LsM[0].PORT_BAUDRATE.ToString());
            
            tcpclient.Start();
        }
        private static void ThreadReConnect()
        {
            while (true)
            {
                if (!TcpControl.Connected)
                {
                    tcpclient.Start();
                    //TcpControl.TcpClient_Init();
                }
                Thread.Sleep(1000);
            }
        }


        static void tcpclient_OnReceivedData(object sender, TcpClient.ReceivedDataEventArgs e)
        {
            TcpClient tcp = sender as TcpClient;

            //写入接收数据队列
            lock (DataQueue)
            {
                DataQueue.Enqueue(Encoding.UTF8.GetString(e.RevData));
            }
        }

        /// <summary>
        /// 界面向服务发送命令字符串
        /// </summary>
        /// <param name="command">命令字符串</param>
        /// <returns></returns>
        public static bool SendUItoServiceCommand(string command)
        {
            try
            {
                tcpclient.socket.Send(Encoding.UTF8.GetBytes(command));
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 数据队列
        /// </summary>
        public static Queue<string> DataQueue = new Queue<string>();



        //各信道接收的数据存放在此列表，并发送
        public static void SendData()
        {
            #region 判断在线状态，是否重新连接
            while (true) 
            {
                try
                {
                    if (Connected)
                    {
                        if (UserID == "")
                        {
                            tcpclient.socket.Send(Encoding.UTF8.GetBytes("+"));
                        }
                        else
                        {
                            tcpclient.socket.Send(Encoding.ASCII.GetBytes("H|" + UserID));
                        }
                    }
                }
                catch
                {
                    try
                    {
                        tcpclient.socket.Disconnect(true);
                    }
                    catch { }
                }

                Thread.Sleep(10 * 1000);
            }
            #endregion

        }


    }
}
