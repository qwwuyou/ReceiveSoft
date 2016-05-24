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
        Timer timer_SendData;

        public UdpThread(UdpServer Udp)
        {
            udp = Udp;
            //udp服务没有在线概念，所以记录
            timer_SocketManager = new Timer(new TimerCallback(SocketManager), null, 5000, 5000);

            timer_SendData = new Timer(new TimerCallback(SendData), null, 100, 100);
        }

        //管理socket的一系列操作
        void SocketManager(object sender)
        {
            try
            {
                UdpBussiness.DelClosSocket(udp, 60);
            }
            catch (Exception ex) 
            { }
        }


        //回复数据的方法
        void SendData(object sender)
        {
            try
            {
                //UdpBussiness.SendData(udp);
                UdpBussiness.SendCommand(udp);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
