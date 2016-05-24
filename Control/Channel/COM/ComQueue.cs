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
        private ConcurrentQueue<ComReceivedData> qcrd;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        private ConcurrentQueue<ComSendData> qcsd;

        public ConcurrentQueue<ComReceivedData> Qcrd
        {
            get { return qcrd; }
            set { qcrd = value; }
        }

        public ConcurrentQueue<ComSendData> Qcsd
        {
            get { return qcsd; }
            set { qcsd = value; }
        }

        public ComQueue()
        {
            qcrd = new ConcurrentQueue<ComReceivedData>();
            qcsd = new ConcurrentQueue<ComSendData>();
        }
    }
}
