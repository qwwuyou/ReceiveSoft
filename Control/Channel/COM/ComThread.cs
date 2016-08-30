using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Service;

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
            
            //定时向串口发送查询卫星状态指令，确保卫星正常。
            Thread timer_AccessCom = new Thread(new ThreadStart(AccessCom));
            timer_AccessCom.Start();

            //检查召测列表中是否有命令数据
            Thread timer_SendData = new Thread(new ThreadStart(SendData));
            timer_SendData.Start();

        }

        /// <summary>
        /// 定时发送查询卫星状态指令
        /// </summary>
        void AccessCom()
        {
            Thread.Sleep(10 * 1000);//10秒后开始执行该方法
            while (com.sp.IsOpen) //串口打开
            {

                //原版本卫星协议
                //ComBussiness.GetComState();
                //新版本卫星协议4.0
                ComBussiness.GetComStateFor4();


                Thread.Sleep( 3*1000); //等待3秒，等待卫星返回状态报
                CheckComState();
                Thread.Sleep(3*60 * 1000);
            }
        }

        //职守方法每5分钟检查1次与串口交互时间，如5分钟内无交互，重启串口
        public static void CheckComState()
        {
            foreach (ComServer com in Service.ServiceControl.com)
            {
                //原版本卫星协议
                //TimeSpan ts = DateTime.Now.Subtract(com.CState.DATATIME);
                //新版本卫星协议4.0
                TimeSpan ts = DateTime.Now.Subtract(com.CStateFor4.DATATIME);

                Console.WriteLine(DateTime.Now + "--------" + com.CStateFor4.DATATIME);
                if (ts.Minutes > 10)
                {
                    try
                    {
                        com.Stop();
                        Thread.Sleep(3 * 1000);
                        com.Start();
                        Console.WriteLine(DateTime.Now + " com职守 " + "Restart！");
                    }
                    catch (Exception ex)
                    { Console.WriteLine(DateTime.Now + ex.ToString()); }
                }
            }
        }

        //召测的方法
        void SendData()
        {
            Thread.Sleep(10 * 1000);
            while (true)
            {
                if (com.sp.IsOpen)//串口打开
                {
                    try
                    {
                        ComBussiness.SendCommand(com);
                    }
                    catch (Exception ex)
                    {
                        Service.ServiceControl.log.Error(DateTime.Now + ex.ToString());
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
