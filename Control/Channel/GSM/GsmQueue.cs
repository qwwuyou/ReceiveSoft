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
        private ConcurrentQueue<GsmReceivedData> qgrd;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        private ConcurrentQueue<GsmSendData> qgsd;

        public ConcurrentQueue<GsmReceivedData> Qgrd
        {
            get { return qgrd; }
            set { qgrd = value; }
        }

        public ConcurrentQueue<GsmSendData> Qgsd
        {
            get { return qgsd; }
            set { qgsd = value; }
        }

        public GsmQueue()
        {
            qgrd = new ConcurrentQueue<GsmReceivedData>();
            qgsd = new ConcurrentQueue<GsmSendData>();
        }
    }
}
