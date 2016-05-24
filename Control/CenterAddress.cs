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
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create("https://raw.githubusercontent.com/qwwuyou/iprepo/master/ip.txt");
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                byte[] byteData =new byte [1024];

                using (System.IO.Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
                string strText = "";
                // Get response   
                using (System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse)
                {
                    // Get the response stream   
                    System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                    // Console application output   
                    strText = strText + reader.ReadToEnd().Replace("\n", "");
                }

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
