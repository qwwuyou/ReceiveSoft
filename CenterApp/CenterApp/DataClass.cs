using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenterApp
{
    public static class DataClass
    {
        public static IList<Service.Model.CENTER_SERVER> servers = null;
        public static IList<Service.Model.CENTER_STARTSTATE> serverstate = null;
        public static IList<Service.Model.CENTER_RTUCHANGE> rtuchange = null;

        /// <summary>
        /// 获得服务信息
        /// </summary>
        /// <param name="Where"></param>
        public static void  GetCenterInfo()
        {
            servers = Service.PublicBD.db.GetServers();
            serverstate = Service.PublicBD.db.GetStartState("");
            rtuchange = Service.PublicBD.db.GetCenterRTUChange("");
        }

        /// <summary>
        /// 根据条件获得服务信息
        /// </summary>
        /// <param name="Where"></param>
        public static void GetCenterInfo(string Where)
        {
            servers = Service.PublicBD.db.GetServers();
            serverstate = Service.PublicBD.db.GetStartState(Where);
            rtuchange = Service.PublicBD.db.GetCenterRTUChange(Where);
        }

        /// <summary>
        /// 删除该服务所有信息
        /// </summary>
        public static void DeleteServerInfo(string server, string ip)
        {
            string Where = "where ProjectName='" + server + "' and PublicIP='" + ip + "'";
            bool b = Service.PublicBD.db.DelCENTER_RTUCHANGE(Where);
            b = Service.PublicBD.db.DelCENTER_STARTSTATE(Where);
            b = Service.PublicBD.db.DelCENTER_SERVER(Where);
        }

    }
}
