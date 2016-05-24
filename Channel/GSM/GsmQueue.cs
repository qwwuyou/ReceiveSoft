using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace GsmService
{
    public class GsmQueue
    {
        /// <summary>
        /// 接收数据队列
        /// </summary>
        public ConcurrentQueue<GsmReceivedData> Qgrd;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        public ConcurrentQueue<GsmSendData> Qgsd;

        public GsmQueue()
        {
            Qgrd = new ConcurrentQueue<GsmReceivedData>();
            Qgsd = new ConcurrentQueue<GsmSendData>();
        }
    }
}
