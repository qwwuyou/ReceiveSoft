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
        private ConcurrentQueue<TcpReceivedData> qtrd;
        
        /// <summary>
        /// 发送数据队列
        /// </summary>
        private ConcurrentQueue<TcpSendData> qtsd;

        public ConcurrentQueue<TcpReceivedData> Qtrd
        {
            get { return qtrd; }
            set { qtrd = value; }
        }

        public ConcurrentQueue<TcpSendData> Qtsd
        {
            get { return qtsd; }
            set { qtsd = value; }
        }

        public TcpQueue() 
        {
            qtrd = new ConcurrentQueue<TcpReceivedData>();
            qtsd = new ConcurrentQueue<TcpSendData>();
        }
    }
}
