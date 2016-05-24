using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace UdpService
{
    public class UdpQueue
    {
        /// <summary>
        /// 接收数据队列
        /// </summary>
        public ConcurrentQueue<UdpReceivedData> Qurd;
        
        /// <summary>
        /// 发送数据队列
        /// </summary>
        public ConcurrentQueue<UdpSendData> Qusd;

        public UdpQueue() 
        {
            Qurd = new ConcurrentQueue<UdpReceivedData>();
            Qusd = new ConcurrentQueue<UdpSendData>();
        }
    }
}
