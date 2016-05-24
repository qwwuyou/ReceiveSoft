using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace TcpService
{
    class TcpThread
    {
        TcpServer tcp;
        
        
        /// <summary>
        /// socket管理线程
        /// 管理socket在线情况
        /// 5秒执行一个轮询，判断大于60秒的连接，杀死
        /// </summary>
        Timer timer_SocketManager;
        

        /// <summary>
        /// 回复数据的线程
        /// 100毫秒轮询回复一次
        /// </summary>
        //Timer timer_SendData;


        public TcpThread(TcpServer Tcp)
        {
            tcp = Tcp;
            timer_SocketManager = new Timer(new TimerCallback(SocketManager),null, 15000, 15000);

            Thread timer_SendData = new Thread(new ThreadStart(SendData));
            timer_SendData.Start();
        }

        //管理socket的一系列操作
        void SocketManager(object sender)
        {
            try
            {
                TcpBussiness.DelClosSocket(tcp, 120);
            }
            catch (Exception ex)
            {
                Service.ServiceControl.log.Error(DateTime.Now + "管理socket出现异常！" + ex.ToString());
            }
        }


        //回复数据的方法
        void SendData()
        {
            while (true)
            {
                try
                {
                    TcpBussiness.SendCommand(tcp);
                    //TcpBussiness.SendData(tcp);
                }
                catch (Exception ex)
                {
                    Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
                }
                Thread.Sleep(500);
            }
        }
        
    }
}
