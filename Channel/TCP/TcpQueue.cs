using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace TcpService
{
    public class TcpQueue
    {
        /// <summary>
        /// 接收数据队列
        /// </summary>
        public ConcurrentQueue<TcpReceivedData> Qtrd;
        
        /// <summary>
        /// 发送数据队列
        /// </summary>
        public ConcurrentQueue<TcpSendData> Qtsd;

        public TcpQueue() 
        {
            Qtrd = new ConcurrentQueue<TcpReceivedData>();
            Qtsd = new ConcurrentQueue<TcpSendData>();
        }
    }
}
