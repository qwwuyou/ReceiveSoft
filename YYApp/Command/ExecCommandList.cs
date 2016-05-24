using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYApp
{
    class ExecCommandList
    {
        /// <summary>
        /// 服务端返回的召测命令列表
        /// </summary>
        public static List<Command> LC = null;

        /// <summary>
        /// 命令集合--来自数据库
        /// </summary>
        public static IList<Service.Model.YY_RTU_COMMAND>  Commands = null;

        /// <summary>
        /// 数据处理后更新命令列表
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public static string UpdCommand(string data)
        {
            //例子--cmd|tcp 0012345678 02 680F6830001234567802F180502954102700A416 0 1999-09-09 12:00:00
            string Rstr = "";
            lock(LC)
            if (LC != null)
            {
                string[] datas = data.Split(new string[] { "\n" }, StringSplitOptions.None);
                for (int k = 0; k < datas.Count(); k++)
                {
                    data = datas[k];
                    #region
                    if (data.Length >= 5)
                    {
                        //命令
                        string tem = data.Substring(0, 5);
                        if (tem == "--cmd")
                        {
                            string[] strs = data.Replace("--cmd|", "").Split(new char[] { ' ' });
                            if (strs.Length == 7)
                            {
                                Command c = new Command();
                                c.SERVICETYPE = strs[0];
                                c.STCD = strs[1];
                                c.CommandID = strs[2];
                                c.Data = strs[3];
                                c.STATE = int.Parse(strs[4]);
                                c.DATETIME = DateTime.Parse(strs[5] + " " + strs[6]);
                                var command = from com in LC where com.STCD == c.STCD && com.CommandID == c.CommandID && com.SERVICETYPE == c.SERVICETYPE select com;
                                if (command.Count() > 0)
                                {
                                    foreach (var item in command)
                                    {
                                        item.STATE = c.STATE;
                                        item.Data = c.Data;
                                        item.DATETIME = c.DATETIME;
                                    }
                                }
                                else {LC.Add(c); }
                            }
                           
                        }
                        else  //不是命令数据报从新整理返回
                        {
                            Rstr += data + "\n";
                        }



                    }
                    else
                    {
                        if (data != "")
                            Rstr += data + "\n";
                    }

                    #endregion
                }
            }

            return Rstr;
        }
    }
}
