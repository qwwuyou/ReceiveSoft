using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;

namespace ADJC_001
{
    public class ParseData
    {
        
        //public byte[] GetDateTime() 
        //{
        //    string strTime = DateTime.Now.ToString("yyMMddHHmmss");
        //    byte[] tmpBody = EnCoder.HexStrToByteArray(strTime);
        //    Array.Copy(EnCoder.HexStrToByteArray(strTime), 0, tmpBody, 2, 6);
        //    return tmpBody;
        //}


        /// <summary>
        /// 分割字符串为多包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<string> SubPackage(string data) 
        {
            List<string> list = data.Split(new string[] { "##", "&&" }, StringSplitOptions.None).ToList<string>();
            List<string> l = (from i in list where i != "" select i).ToList<string>();
            return l;
        }


        /// <summary>
        /// 数据报类型
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public string PacketType(string data) 
        {
            string PacketType = "";
            if (data.Length > 4) 
            {
                PacketType=data.Substring(0, 4);
            }
            return PacketType;
        }

        /// <summary>
        /// 数据区长度
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private int DataLength(string data)
        {
            string DataLength = "";
            int dl=0;
            if (data.Length > 8)
            {
                DataLength = data.Substring(4, 4);
            }
            if (int.TryParse(DataLength, out dl)) 
            {
                return dl;
            }
            return dl;
        }

        /// <summary>
        /// 得到时间  14个F取当前系统时间
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private DateTime GetDateTime(string data) 
        {
            string dateString = data.Substring(33,14);
            DateTime dt=DateTime.Now;
            if (dateString != "FFFFFFFFFFFFFF")
            { dt = DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture); }
            return dt;
        }

        /// <summary>
        /// 分包标识
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private SubPacket Subpacket(string data) 
        {
            SubPacket sp = new SubPacket();
            int total = 0;
            int part=0;
            if(int.TryParse(data.Substring(49, 2),out total ))
            {
                sp.Total=total;
            }
            if(int.TryParse(data.Substring(47, 2),out part ))
            {
                sp.Part=part;
            }
            return sp;
        }

        /// <summary>
        /// 参数&数据
        /// </summary>
        /// <param name="data">数据报</param>
        /// <param name="length">数据区长度</param>
        /// <returns></returns>
        private List<Item_Data> GetData(string data, int length) 
        {
            List<Item_Data> I_Ds = new List<Item_Data>();
            Item_Data I_D;
            string d = data.Substring(51, length);
            string[] ds = d.Split(new char[] { ',' });
            foreach (var item in ds)
            {
                if (item.Contains("=")) 
                {
                    string[] i = item.Split(new char[] { '=' });
                    if (i.Length ==2) 
                    {
                        I_D = new Item_Data();
                        I_D.Item =i[0];
                        I_D .Data =i[1];
                        I_Ds.Add(I_D);
                    }
                }
            }

            return I_Ds;
        }

        /// <summary>
        /// 用包长度验证包有效性 无包头包尾
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public bool PacketValidate(string data) 
        {
            int len=DataLength(data);
            if (data.Length == (len + 51))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 得到站号
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public string GetCode(string data) 
        {
            return data.Substring(13, 20);
        }

        /// <summary>
        /// 解包方法
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public DataModel UnPack(string data)
        {
            DataModel DM = new DataModel();
            DM.Code = data.Substring(13,20);
            DM.DataLength = DataLength(data);
            DM.Datetime = GetDateTime(data);
            DM.PacketType = data.Substring(0, 4);
            DM.PacketCode = int.Parse(data.Substring(8, 5));
            DM.SubPacket = Subpacket(data);
            DM.Item_data = GetData(data,DM.DataLength);

            DM.Reply = "##" + DM.PacketType + "0000" + DM.PacketCode.ToString().PadLeft(4, '0') + DM.Code + DateTime.Now.ToString("yyyyMMddHHmmss") + "0101&&";
            
            return DM;
        }
    }

    #region 数据报实体类
    public class DataModel
    {
        private string _code;
        private int _packetcode;
        private string _packettype;
        private SubPacket _subpacket;
        private int _datalength;
        private DateTime _datetime;
        private List<Item_Data> _item_data;
        private string _reply;

        /// <summary>
        /// 站（模块）号
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }

        /// <summary>
        /// 包序号
        /// </summary>
        public int PacketCode 
        {
            set { _packetcode = value; }
            get { return _packetcode; }
        }

        /// <summary>
        /// 报类型
        /// </summary>
        public string PacketType
        {
            set { _packettype = value; }
            get { return _packettype; }
        }

        /// <summary>
        /// 数据区长度
        /// </summary>
        public int DataLength
        {
            set { _datalength = value; }
            get { return _datalength; }
        }

        /// <summary>
        /// 分包信息
        /// </summary>
        public SubPacket SubPacket 
        {
            set { _subpacket = value; }
            get { return _subpacket; }
        }

        /// <summary>
        /// 上报时间，如14个F，则使用当前时间
        /// </summary>
        public DateTime Datetime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }

        /// <summary>
        /// 数据区 监测项目--数据
        /// </summary>
        public List<Item_Data> Item_data 
        {
            set { _item_data = value; }
            get { return _item_data; }
        }

        /// <summary>
        /// 回复报
        /// </summary>
        public string Reply 
        {
            set { _reply = value; }
            get { return _reply; }
        }
    }

    public class SubPacket 
    {
        private int _total;
        private int _part;

        /// <summary>
        /// 总包数
        /// </summary>
        public int Total 
        {
            set { _total = value; }
            get { return _total; }
        }

        /// <summary>
        /// 第几包
        /// </summary>
        public int Part
        {
            set { _part = value; }
            get { return _part; }
        }
    }

    public class Item_Data
    {
        private string _item;
        private string _data;

        /// <summary>
        /// 项目（元素）
        /// </summary>
        public string Item
        {
            set { _item = value; }
            get { return _item; }
        }

        /// <summary>
        /// 数据值
        /// </summary>
        public string Data
        {
            set { _data = value; }
            get { return _data; }
        }
    }
    #endregion
}
