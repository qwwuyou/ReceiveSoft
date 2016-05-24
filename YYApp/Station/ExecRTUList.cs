using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YYApp
{
    class ExecRTUList
    {
        /// <summary>
        /// RTU在线列表
        /// </summary>
        public static List<RTU> Lrdm = null;

        public static void SetLrdm(IList<Service.Model.YY_RTU_Basic> rtus) 
        {
            ExecRTUList.Lrdm = new List<RTU>();
            RTU rdm = new RTU();

            if (rtus != null)
                foreach (var item in rtus)
                {
                    rdm = new RTU();
                    rdm.STCD = item.STCD;
                    rdm.PWD = item.PassWord;
                    rdm.NAME = item.NiceName;
                    rdm.SERVICETYPE = "gsm";
                    ExecRTUList.Lrdm.Add(rdm);
                }
        }

        /// <summary>
        /// 数据处理后更新测站在线状态
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public static string Updrdm(string data)
        {
            //上线例子--stcd|0012345679:udp 0012345678:tcp
            //下线例子--stcd|0012345679:udp： 0012345678:tcp：
            string Rstr = "";
            if (Lrdm != null)
            {
                string[] datas = data.Split(new string[] { "\n" }, StringSplitOptions.None);
                for (int k = 0; k < datas.Count(); k++)
                {
                    data = datas[k];
                    #region
                    if (data.Length >= 6)
                    {
                        //说明所有测站不在线
                        if (data == "--stcd|") 
                        {
                            ExecRTUList.Updrdm();
                            return "";
                        }

                        //有在线测站
                        string tem = data.Substring(0, 6);
                        if (tem == "--stcd")
                        {
                            string[] strs = data.Replace("--stcd|", "").Split(new char[] { ' ' });

                            foreach (var item in strs)
                            {
                                string[] temp = item.Split(new char[] { ':' });
                                if (temp.Length == 2)//上线
                                {
                                    var rtu = from r in Lrdm where r.STCD == temp[0] select r;
                                    if (rtu.Count() > 0)
                                    {
                                        rtu.First().SERVICETYPE = temp[1];
                                    }
                                }
                                else if(temp.Length == 3)//下线
                                {
                                    var rtu = from r in Lrdm where r.STCD == temp[0] select r;
                                    if (rtu.Count() > 0)
                                    {
                                        rtu.First().SERVICETYPE = null;
                                    }
                                }
                            }
                        }
                        else  //不是在线测站的数据报从新整理返回
                        {
                            Rstr += data + "\n";
                        }



                    }
                    else
                    {
                        if(data!="")
                        Rstr += data + "\n";
                    }
                
                    #endregion
                }
            }
            else
            {
                Rstr += data ;
            }

            return Rstr;
        }

        /// <summary>
        /// 全部数据在线测站变为下线状态(可用于与服务断开连接)
        /// </summary>
        public static void Updrdm()
        {
            if (Lrdm!=null)
            foreach (var item in Lrdm)
            {
                item.SERVICETYPE  = null;
            }
        }

    }
}
