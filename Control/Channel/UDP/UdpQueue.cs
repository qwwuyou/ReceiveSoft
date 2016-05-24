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
        private ConcurrentQueue<UdpReceivedData> qurd;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        private ConcurrentQueue<UdpSendData> qusd;


        public ConcurrentQueue<UdpReceivedData> Qurd
        {
            get { return qurd; }
            set { qurd = value; }
        }

        public ConcurrentQueue<UdpSendData> Qusd
        {
            get { return qusd; }
            set { qusd = value; }
        }

        public UdpQueue() 
        {
            qurd = new ConcurrentQueue<UdpReceivedData>();
            qusd = new ConcurrentQueue<UdpSendData>();
        }
    }
}
