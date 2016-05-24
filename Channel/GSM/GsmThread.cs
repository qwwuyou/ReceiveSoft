using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GsmService
{
    class GsmThread
    {
        GsmServer gsm;
            
        /// <summary>
        /// 回复数据的线程
        /// </summary>
        Timer timer_SendData;

        public GsmThread(GsmServer Gsm) 
        {
            gsm = Gsm;

            timer_SendData = new Timer(new TimerCallback(SendData), null, 100, 100);
        }

        //回复数据的方法
        void SendData(object sender)
        {
            try
            {
                GsmBussiness.SendData(gsm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
