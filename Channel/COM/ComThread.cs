using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ComService
{
    class ComThread
    {
        ComServer com;

        /// <summary>
        /// 回复数据的线程
        /// </summary>
        Timer timer_SendData;

        public ComThread(ComServer Com)
        {
            com = Com;

            timer_SendData = new Timer(new TimerCallback(SendData), null, 100, 100);
        }

        //回复数据的方法
        void SendData(object sender)
        { 
            //测试
            //cs.sp.WriteLine("bbbbbbbbbbbbbbbbbbbbb");

            ComBussiness.SendData(com);
        }
    }
}
