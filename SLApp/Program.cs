using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using ToUI;
using log4net;

namespace SLApp
{
    class Program
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");
        static void Main(string[] args)
        {
            #region Silverlight用socket访问服务，是否有需要安全策略文件请求(服务器的端口必须启动)
            try
            {
                ToUI.TcpServer Silverlight_UI = new ToUI.TcpServer("0.0.0.0", 943, "Silverlight");
                Silverlight_UI.Start();
                Silverlight_UI.OnReceivedData += new EventHandler<ToUI.ReceivedDataEventArgs>(Silverlight_UI_OnReceivedData);
                log.Warn("Silverlight访问服务安全策略服务启动成功！");
            }
            catch (Exception ex)
            {
                log.Warn("Silverlight访问服务安全策略服务启动失败！", ex);
                throw ex;
            }
            #endregion


            Console.ReadLine();
        }



        #region Silverlight用socket访问服务的策略文件
        static void Silverlight_UI_OnReceivedData(object sender, ToUI.ReceivedDataEventArgs e)
        {
            string clientPolicyString = "<policy-file-request/>";
            string requeststring = System.Text.Encoding.UTF8.GetString(e.RevData, 0, e.RevData.Length);

            if (requeststring == clientPolicyString)
            {
                //策略步骤二：如果客户端请求是<policy-file-request/>,则将安全策略文件作为bytes发送给客户端
                string path = System.Windows.Forms.Application.StartupPath + @"\clientaccesspolicy.xml";

                if (System.IO.File.Exists(path))
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    int length = (int)fs.Length;
                    byte[] accessbytes = new byte[length];
                    fs.Read(accessbytes, 0, length);
                    fs.Close();
                    e.ClientSocket.Send(accessbytes, accessbytes.Length, System.Net.Sockets.SocketFlags.None);
                    e.ClientSocket.Close();
                }
                else
                {
                    log.Warn("Silverlight访问服务未找到安全策略文件！");
                }
            }
        }
        #endregion
    }
}
