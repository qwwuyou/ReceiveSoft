using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYApp
{
    class ExecServiceList
    {
        /// <summary>
        /// 数据处理后更新各信道在线状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UpdLsm(string data)
        {

            string Rstr = "";
            if (Lsm != null)
            {
                string[] datas = data.Split(new string[] { "\n" }, StringSplitOptions.None);
                for (int k = 0; k < datas.Count(); k++)
                {
                    data = datas[k];
                    #region
                    if (data.Length > 5)
                    {
                        string tem = data.Substring(0, 5);
                        if (tem == "--tcp")  //例子 --tcp|1:1 12,0,0,4:2,0,0,4
                        {
                            string[] strs = data.Replace("--tcp|", "").Split(new char[] { ' ' });
                            if (strs.Length >= 2)
                            {
                                string[] strs1 = strs[0].Split(new char[] { ':' });
                                var tcp = from t in Lsm where t.SERVICETYPE.ToLower() == "tcp" select t;


                                int i = 0;
                                foreach (var item in tcp)
                                {
                                    if (i <= strs1.Length - 1)
                                        if (strs1[i] == "0")
                                            item.STATE = false;
                                        else
                                            item.STATE = true;

                                    i++;
                                }

                                string[] strs2 = strs[1].Split(new char[] { ':' });
                                tcp = from t in Lsm where t.SERVICETYPE.ToLower() == "tcp" select t;
                                i = 0;

                                foreach (var item in tcp)
                                {
                                    if (i <= strs2.Length - 1)
                                        item.LISTCOUNT = strs2[i];
                                    i++;
                                }
                            }
                        }
                        else if (tem == "--udp")
                        {
                            string[] strs = data.Replace("--udp|", "").Split(new char[] { ' ' });
                            if (strs.Length >= 2)
                            {
                                string[] strs1 = strs[0].Split(new char[] { ':' });
                                var udp = from t in Lsm where t.SERVICETYPE.ToLower() == "udp" select t;
                                int i = 0;
                                foreach (var item in udp)
                                {
                                    if (i <= strs1.Length - 1)
                                        if (strs1[i] == "0")
                                            item.STATE = false;
                                        else
                                            item.STATE = true;

                                    i++;
                                }


                                string[] strs2 = strs[1].Split(new char[] { ':' });
                                udp = from t in Lsm where t.SERVICETYPE.ToLower() == "udp" select t;
                                i = 0;
                                foreach (var item in udp)
                                {
                                    if (i <= strs2.Length - 1)
                                        item.LISTCOUNT = strs2[i];
                                    i++;
                                }
                            }
                        }
                        else if (tem == "--com")
                        {
                            string[] strs = data.Replace("--com|", "").Split(new char[] { ' ' });
                            if (strs.Length >= 2)
                            {
                                //var com = from t in Lsm where t.SERVICETYPE.ToLower() == "com" select t;
                                //int i = 0;
                                //foreach (var item in com)
                                //{
                                //    if (i <= strs.Length - 1)
                                //        if (strs[i] == "0")
                                //            item.STATE = false;
                                //        else
                                //            item.STATE = true;

                                //    i++;
                                //}
                                string[] strs1 = strs[0].Split(new char[] { ':' });
                                var com = from t in Lsm where t.SERVICETYPE.ToLower() == "com" select t;
                                int i = 0;
                                foreach (var item in com)
                                {
                                    if (i <= strs1.Length - 1)
                                        if (strs1[i] == "0")
                                            item.STATE = false;
                                        else
                                            item.STATE = true;

                                    i++;
                                }

                                string[] strs2 = strs[1].Split(new char[] { ':' });
                                com = from t in Lsm where t.SERVICETYPE.ToLower() == "com" select t;
                                i = 0;
                                foreach (var item in com)
                                {
                                    if (i <= strs2.Length - 1)
                                        item.LISTCOUNT = strs2[i];
                                    i++;
                                }
                            }
                        }
                        else if (tem == "--gsm")
                        {
                            string[] strs = data.Replace("--gsm|", "").Split(new char[] { ':' });
                            var gsm = from t in Lsm where t.SERVICETYPE.ToLower() == "gsm" select t;
                            int i = 0;
                            foreach (var item in gsm)
                            {
                                if (i <= strs.Length - 1)
                                if (strs[i] == "0")
                                    item.STATE = false;
                                else
                                    item.STATE = true;

                                i++;
                            }
                        }
                        else
                        {
                            Rstr += data + "\n";
                        }
                    }
                    #endregion
                }
            }

            return Rstr;
        }

        /// <summary>
        /// 与服务器断开连接各信道在线状态关闭
        /// </summary>
        public static void UpdLsm()
        {
            foreach (var item in Lsm)
            {
                item.STATE = false;
            }
        }

        public static List<service> Lsm = null;

        public static bool ServiceDBConnectionState=false;

        /// <summary>
        /// 数据处理后更新服务连接数据库状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UpdDBConnectionState(string data)
        {
            string Rstr = "";
             string[] datas = data.Split(new string[] { "\n" }, StringSplitOptions.None);
             for (int k = 0; k < datas.Count(); k++)
             {
                 data = datas[k];
                 #region
                 if (data.Length > 4)
                 {
                     string tem = data.Substring(0, 4);
                     if (tem == "--db")  //例子 --db|1
                     {
                         if (data.Replace("--db|", "") == "1")
                         {
                             ServiceDBConnectionState = true;
                         }
                         else if (data.Replace("--db|", "") == "0")
                         {
                             ServiceDBConnectionState = false; ;
                         }
                     }
                     else
                     { Rstr += data + "\n"; }
                 }
                 #endregion
             }
             return Rstr;
        }
    }
}
