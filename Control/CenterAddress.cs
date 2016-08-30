using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Service
{
    public class CenterAddress
    {
        public void GetCenterAddress(out string IP, out string Port)
        {
            IP = "";
            Port = "";

            try
            {
                WebRequest webRequest = WebRequest.Create("https://raw.githubusercontent.com/qwwuyou/iprepo/master/ip.txt");
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;
                System.IO.Stream responseStream = httpRequest.GetResponse().GetResponseStream();


                string strText = string.Empty;
                using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("gb2312")))
                {
                    strText = responseReader.ReadToEnd();
                }
                responseStream.Close();
                responseStream.Flush();

                string[] temp = strText.Split(new char[] { ':' });
                if (temp.Length == 2)
                {
                    IPAddress ip;
                    if (IPAddress.TryParse(temp[0], out ip))
                    {
                        IP = ip.ToString();
                    }
                    int port;
                    if (int.TryParse(temp[1], out port))
                    {
                        Port = port.ToString();
                    }
                }
            }
            catch { }
        }
    }
}
