using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace ToUI
{
    class TcpThread
    {
        TcpServer tcp;
        
        
        /// <summary>
        /// socket管理线程&通讯守护
        /// </summary>
        Timer timer_SocketManager;
        
        /// <summary>
        /// 回复数据的线程
        /// </summary>
        Timer timer_SendData;

        /// <summary>
        /// 服务状态信息发到客户端
        /// </summary>
        Timer timer_ServiceState;

        public TcpThread(TcpServer Tcp)
        {
            tcp = Tcp;
            timer_SocketManager = new Timer(new TimerCallback(SocketManager),null, 5000, 5000);

            timer_SendData = new Timer(new TimerCallback(SendData), null, 100, 100);

            timer_ServiceState = new Timer(new TimerCallback(ServiceState),null, 5000, 10000);
        }

        //管理socket的一系列操作
        void SocketManager(object sender)
        {
            TcpBussiness.DelClosSocket(tcp, 60);

        }


        //回复数据的方法
        void SendData(object sender)
        {
            TcpBussiness.SendData(tcp);
        }

        //服务状态信息发到客户端（rtu在线，各服务状态，数据库连接状态）
        void ServiceState(object sender) 
        {
            if (tcp.Ts.Count() > 0)
            {
                try
                {
                    Service.ServiceBussiness.SendEveryRTUOnlineState();
                    Service.ServiceBussiness.SendEveryServcieState();
                    Service.ServiceBussiness.SendDBConnectionState();
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
