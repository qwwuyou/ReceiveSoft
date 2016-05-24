using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace ComService
{
    public class ComQueue
    {
        /// <summary>
        /// 接收数据队列
        /// </summary>
        public ConcurrentQueue<ComReceivedData> Qcrd;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        public ConcurrentQueue<ComSendData> Qcsd;

        public ComQueue()
        {
            Qcrd = new ConcurrentQueue<ComReceivedData>();
            Qcsd = new ConcurrentQueue<ComSendData>();
        }
    }
}
