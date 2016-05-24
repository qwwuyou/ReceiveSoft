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
        /// </summary>
        Timer timer_SocketManager;
        

        /// <summary>
        /// 回复数据的线程
        /// </summary>
        Timer timer_SendData;


        public TcpThread(TcpServer Tcp)
        {
            tcp = Tcp;
            timer_SocketManager = new Timer(new TimerCallback(SocketManager),null, 5000, 5000);

            timer_SendData = new Timer(new TimerCallback(SendData), null, 100, 100);
        }

        //管理socket的一系列操作
        void SocketManager(object sender)
        {
            TcpBussiness.DelClosSocket(tcp, 60);
        }


        //回复数据的方法
        void SendData(object sender)
        {
            try
            {
                TcpBussiness.SendCommand(tcp);
                //TcpBussiness.SendData(tcp);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        
    }
}
