using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UdpService
{
    class UdpThread
    {
        UdpServer udp;


        /// <summary>
        /// socket管理线程
        /// </summary>
        Timer timer_SocketManager;


        /// <summary>
        /// 回复数据的线程
        /// </summary>
        //Timer timer_SendData;

        public UdpThread(UdpServer Udp)
        {
            udp = Udp;
            //udp服务没有在线概念，所以记录
            timer_SocketManager = new Timer(new TimerCallback(SocketManager), null, 15000, 15000);

            Thread timer_SendData = new Thread(new ThreadStart(SendData));
            timer_SendData.Start();
        }

        //管理socket的一系列操作
        void SocketManager(object sender)
        {
            
                try
                {
                    UdpBussiness.DelClosSocket(udp, 60);
                }
                catch (Exception ex)
                { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            
        }


        //回复数据的方法
        void SendData()
        {
            while (true)
            {
                try
                {
                    UdpBussiness.SendCommand(udp);
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
