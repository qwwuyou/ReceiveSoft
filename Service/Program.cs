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

            //byte[] bytes = EnCoder.HexStrToByteArray("41 ed 47 ae".Replace(" ", ""));
            //Array.Reverse(bytes);
            //float f= BitConverter.ToSingle(bytes, 0);

            long k = long.Parse("1f 3b ba 2b".Replace(" ", ""), System.Globalization.NumberStyles.AllowHexSpecifier);
            DateTime dttt = DateTime.Parse("2000-1-1").AddSeconds(k);
            //string ip = Service.ServiceBussiness.GetPublicIP();
            //string data = "40 04 00 36 01 FD 10 06 0E 0D 0A 01 8C 00 00 27 42 FF FF FF  FF 01 01 3C 46 46 01 01 00 00 23 5A 00 00 27 10 64 00 01 86 A0 03 01 00 64 3C 01 00 64 3C 01 00 64 3C 03 01 02 03 11".Replace (" ","");
            //MKHY_S3.ParseData pd = new MKHY_S3.ParseData();
            //MKHY_S3.DataModel dm=  pd.UnPack(data)
            //string  DataState = data.Substring(44, 2);
            //string DataState1 = data.Substring(42, 2);
            //double  DataState2 = 0;
            //DataState2 = Convert.ToInt32(data.Substring(56, 8), 16);
            //string sss = "PARM 0023 00001 12345678901234567890 20160527120000 0202 P1=127.0.0.1:5555,Q5=60";
            //sss = "STAT 0000 00000 12345678901234567890 20160527120000 0101";
            //sss = sss.Replace(" ", "");
            //ADJC_001.ParseData pd = new ADJC_001.ParseData();
            //ADJC_001.DataModel dm = pd.UnPack(sss);

            //string ss = "40 04 00 36 01 FD 10 06 0E 0D 0A 01 8C 00 00 27 42 FF FF FF  FF 01 01 3C 46 46 01 01 00 00 23 5A 00 00 27 10 64 00 01 86 A0 03 01 00 64 3C 01 00 64 3C 01 00 64 3C 03 01 02 03 11".Replace(" ", "");


            // System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//实例化一个计时器
            // sw.Start();
            // //耗时巨大的代码


            //sw.Stop();
            //TimeSpan ts2 = sw.Elapsed;
            //Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
            
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
