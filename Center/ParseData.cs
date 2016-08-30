using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;

namespace Service
{
    class ParseData
    {

        /// <summary>
        /// 解析符合条件的报文
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public CENTER_SERVER GetServer(byte[] data)
        {
            CENTER_SERVER centerserver = null;
            string datastr = Encoding.GetEncoding("gb2312").GetString(data).Replace("\r", "").Replace("\n", "");
            Console.WriteLine(datastr);
            //datastr = "0d0h0m20s|0|bj|218|127.0.0.1|start";
            string[] Info=datastr.Split(new char[] { '|' });
            if (Info.Length==6) 
            {
                //dateDiff + "|" + RtuCount + "|" + project + "|" + RegistrationInfo + "|" + PublicIP + "|" +RunState;
                centerserver = new CENTER_SERVER();
                centerserver.ProjectName = Info[2];
                centerserver.PublicIP =Info[4];
                int RegisterTime=-1;
                int.TryParse (Info[3],out RegisterTime);
                centerserver.RegisterTime = RegisterTime;
                centerserver.RTUCount = int.Parse(Info[1]);
                centerserver.DTime = DateTime.Now;
                centerserver.RunTime = Info[0];
                centerserver.RunState = Info[5];
                centerserver.SrartTime = centerserver.DTime;
            }

            return centerserver;
        }

        /// <summary>
        /// 比较列表中是否存在该服务器信息，并执行更新（存在）或插入（不存在）操作
        /// </summary>
        /// <param name="list">服务器信息列表</param>
        /// <param name="server">新服务器信息</param>
        public void ExistsServer(List<CENTER_SERVER> list, CENTER_SERVER server) 
        {
            var ser=from s in list where s.ProjectName==server.ProjectName  &&　s.PublicIP== server.PublicIP select s;
            lock (list)
            {

                
                if (ser.Count() > 0)
                {
                    if (ser.First().RTUCount != server.RTUCount)
                    {
                        AddRTUChange(server);
                    }


                    ser.First().RegisterTime = server.RegisterTime;
                    ser.First().RTUCount = server.RTUCount;
                    ser.First().DTime = server.DTime;
                    ser.First().RunTime = server.RunTime;
                    ser.First().RunState = server.RunState;
                    if (server.RunState == "start")
                    { ser.First().SrartTime = server.DTime; }
                    else
                    {
                        server.SrartTime = ser.First().SrartTime;
                    }

                    //更新
                    bool b = PublicBD.db.UpdCENTER_SERVER(server, " where ProjectName='" + server.ProjectName + "' and PublicIP='" + server.PublicIP + "'");
                                
                }
                else
                {
                    list.Add(server);
                    //写入
                    bool b = PublicBD.db.AddCENTER_SERVER(server);

                    AddRTUChange(server);
                }
            }    
        }

        /// <summary>
        /// 更新和写入服务启动信息
        /// </summary>
        /// <param name="server"></param>
        public void ExistsServerStartState(List<CENTER_STARTSTATE> list, CENTER_SERVER server) 
        {
            if (server.RunState == "start")
            {

                CENTER_STARTSTATE startstate = new CENTER_STARTSTATE();
                startstate.DTime = server.DTime;
                startstate.ProjectName = server.ProjectName;
                startstate.PublicIP = server.PublicIP;
                startstate.RunTime = server.RunTime;

                list.Add(startstate);

                //写入
                bool b = PublicBD.db.AddCENTER_STARTSTATE(startstate);

            }
            else
            {
                var ser = from s in list where s.ProjectName == server.ProjectName && s.PublicIP == server.PublicIP && s.DTime == server.SrartTime  select s;
                lock (list)
                {
                    if (ser.Count() > 0)
                    {
                        ser.First().RunTime = server.RunTime;
                        //更新
                        bool b = PublicBD.db.UdpCENTER_STARTSTATE(ser.First(), " where ProjectName='" + server.ProjectName + "' and PublicIP='" + server.PublicIP + "' and  CONVERT(varchar(100), DTime, 120) like '%" + server.SrartTime.ToString("yyyy-MM-dd HH:mm:ss") + "%'");
                    }
                   
                }
            }
            
            
        }

        /// <summary>
        /// 写入RTU变化状态
        /// </summary>
        /// <param name="list">服务器信息列表</param>
        /// <param name="server">新服务器信息</param>
        private void AddRTUChange( CENTER_SERVER server) 
        {
            CENTER_RTUCHANGE rtuchange = new CENTER_RTUCHANGE();
            rtuchange.DTime = server.DTime;
            rtuchange.ProjectName = server.ProjectName;
            rtuchange.PublicIP = server.PublicIP;
            rtuchange.RTUCount = server.RTUCount;
            bool b = PublicBD.db.AddCENTER_RTUCHANGE(rtuchange);
        }
     }
}
