using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Service
{
    class ServiceQueue
    {
        /// <summary>
        /// 发送到界面的数据列表---server
        /// </summary>
        public static ConcurrentQueue<UIModel> QUIM; 


        /// <summary>
        /// 透明传输的数据报列表----Client
        /// </summary>
        public static ConcurrentQueue<DataModel> QDM;


        public ServiceQueue() 
        {
            QUIM = new ConcurrentQueue<UIModel>();
            QDM = new ConcurrentQueue<DataModel>();
        }
    }
}
