using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Service
{
    class Program
    {
        static DateTime dt;
        static void Main(string[] args)
        {
            byte[] ddd = EnCoder.HexStrToByteArray("99");
            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();//实例化一个计时器
            //watch.Start();//开始计时  
            //watch.Stop();
            //Console.WriteLine(watch.ElapsedMilliseconds.ToString());

            //ReadMail rm = new ReadMail();
            //string ip = "";
            //string port = "";
            //rm.ReadMailInfo(out ip,out port );
            
            dt = DateTime.Now;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ServiceControl sc = new ServiceControl();
            sc.start();
            
            

            Console.ReadLine();

        }

        private static byte[] DataValidate(byte[] data, out string Satellite)
        {
            string str = System.Text.Encoding.ASCII.GetString(data);
            try
            {
                string[] Temp_Strs = str.Split(new string[] { "," }, StringSplitOptions.None);
                if (Temp_Strs[0]=="$COUT"&&int.Parse(Temp_Strs[7]) == Temp_Strs[8].Length)
                {
                    Satellite = Temp_Strs[4];
                    return System.Text.Encoding.ASCII.GetBytes(Temp_Strs[8]);
                }
                else
                {
                    Satellite = null;
                    return null;
                }
            }
            catch
            {
                Satellite = null;
                return null;
            }
        }

        //系统崩溃出现的异常此处捕捉到发送到指定邮箱
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ServiceBussiness.SendMail();
            }
            catch(Exception ex)
            {
                TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                
                ServiceControl.log.Error("系统崩溃！\n系统启动时间：" + ServiceControl.StartTime + "\n运行时长：" + dateDiff+"\n异常原因：" + ex.ToString());
            }
        }


    }
}
