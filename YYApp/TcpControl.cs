using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YYApp;

namespace YYApp
{
    public static class TcpControl
    {
        static TcpClient tcpclient;

        /// <summary>
        /// 连接状态属性
        /// </summary>
        public static bool Connected
        {
            get {return tcpclient.socket.Connected;  }
        }


        public static void TcpClient_Init()
        {
            if (Program.wrx.XMLObj.UiTcpModel.IP != "" && Program.wrx.XMLObj.UiTcpModel.PORT.ToString() != "")
            {
                tcpclient = new TcpClient(Program.wrx.XMLObj.UiTcpModel.IP, Program.wrx.XMLObj.UiTcpModel.PORT, "");
                tcpclient.Start();
                tcpclient.OnReceivedData += new EventHandler<TcpClient.ReceivedDataEventArgs>(tcpclient_OnReceivedData);
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



        static int k = 0;        //控制连接服务器时间的计数器
        //各信道接收的数据存放在此列表，并发送
        public static void SendData()
        {
            #region 判断在线状态，是否重新连接
            k++;
            if (k >= 100)    //10秒1个心跳
            {
                k = 0;
                try
                {
                    if (Connected)
                        tcpclient.socket.Send(Encoding.UTF8.GetBytes("+"));
                }
                catch
                {
                    try
                    {
                        tcpclient.socket.Disconnect(true);
                    }
                    catch { }
                }


                if (!Connected)//发送数据失败重新异步连接
                {
                    tcpclient.Start();
                }
            }
            #endregion

        }


    }
}
