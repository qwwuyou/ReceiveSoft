using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JK
{
    public class ParseData
    {
        /// <summary>
        /// 得到中心地址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetCenterAdr(string data) 
        {
            string CenterAdr = "";
            if (data.Length > 6)
            {
                CenterAdr = Convert.ToInt32(data.Substring(2, 4), 16).ToString();
            }
            return CenterAdr;
        }

        /// <summary>
        /// 得到站地址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetSTAdr(string data)
        {
            string CenterAdr = "";
            if (data.Length > 10)
            {
                CenterAdr = Convert.ToInt32(data.Substring(6, 4), 16).ToString();
            }
            return CenterAdr;
        }

        /// <summary>
        /// 得到数据长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetDataLength(string data)
        {
            int DataLength = 0;
            if (data.Length > 18)
            {
                DataLength = Convert.ToInt32(data.Substring(14, 4), 16);
            }
            return DataLength;
        }

        /// <summary>
        /// 得到子命令域是否有效
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetSubIsValid(string data)
        {
            bool SubIsValid = false;
            if (data.Length > 20)
            {
                int tmp = Convert.ToInt32(data.Substring(18, 2), 16);
                string Tmp= Convert.ToString(tmp, 2);
                if (Tmp[0] == '0')
                { SubIsValid = true; }
            }
            return SubIsValid;
        }


        /// <summary>
        /// 得到子命令码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetSubCode(string data)
        {
            string SubCode = "";
            if (data.Length > 24)
            {
                SubCode = data.Substring(20, 2);
            }
            return SubCode;
        }


        /// <summary>
        /// 得到子数据报长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetSubDataLength(string data)
        {
            int SubDataLength = 0;
            if (data.Length > 26)
            {
                SubDataLength = Convert.ToInt32(data.Substring(24, 2), 16);
            }
            return SubDataLength;
        }

        /// <summary>
        /// 得到传感器地址
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetSensorAdr(string data)
        {
            string SensorAdr = "";
            if (data.Length > 34)
            {
                SensorAdr = Convert.ToInt32(data.Substring(30, 4), 16).ToString();
            }
            return SensorAdr;
        }

        /// <summary>
        /// 得到采集时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DateTime GetDateTime(string data)
        {
            DateTime dateTime = DateTime.Now ;
            if (data.Length > 40)
            {
                long tmp = long.Parse(data.Substring(32, 8), System.Globalization.NumberStyles.AllowHexSpecifier);
                dateTime = DateTime.Parse("2000-1-1").AddSeconds(tmp);
            }
            return dateTime;
        }

        /// <summary>
        /// 得到参数数量
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetPramCount(string data)
        {
            int PramCount = 0;
            if (data.Length > 42)
            {
                int tmp = Convert.ToInt32(data.Substring(40, 2), 16);
                string Tmp = Convert.ToString(tmp, 2);
                PramCount=Convert.ToInt32(Tmp.Substring(0, 4), 2);
            }
            return PramCount;
        }



        /// <summary>
        /// 解包方法
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public DataModel UnPack(string data)
        {
            DataModel DM = new DataModel();
            DM.CenterAdr = GetCenterAdr(data);
            DM.STAdr = GetSTAdr(data);
            DM.DataLength = GetDataLength(data);
            DM.SubIsValid = GetSubIsValid(data);
            DM .SubCode=GetSubCode(data);
            DM.SubDataLength=GetSubDataLength(data);
            DM.SensorAdr =GetSensorAdr(data);
            DM.Datetime = GetDateTime(data);
            DM.PramCount = GetPramCount(data );

            //Item_Data

            return DM;
        }
    }

    #region 数据报实体类
    public class DataModel
    {
        
        private string _centeradr;

        /// <summary>
        /// 中心地址
        /// </summary>
        public string CenterAdr
        {
            get { return _centeradr; }
            set { _centeradr = value; }
        }

        private string _stadr;
        /// <summary>
        /// 站（模块）号/地址
        /// </summary>
        public string STAdr
        {
            set { _stadr = value; }
            get { return _stadr; }
        }

        private int _datalength;
        /// <summary>
        /// 数据区长度
        /// </summary>
        public int DataLength
        {
            set { _datalength = value; }
            get { return _datalength; }
        }

        private bool _subisvalid;
        /// <summary>
        /// 子命令域是否有效
        /// </summary>
        public bool SubIsValid
        {
            get { return _subisvalid; }
            set { _subisvalid = value; }
        }

        private string _subcode;
        /// <summary>
        /// 子命令码
        /// </summary>
        public string SubCode
        {
            get { return _subcode; }
            set { _subcode = value; }
        }

        private int _subdatalength;
        /// <summary>
        /// 子数据区长度
        /// </summary>
        public int SubDataLength
        {
            set { _subdatalength = value; }
            get { return _subdatalength; }
        }


        private string _sensoradr;
        /// <summary>
        /// 传感器地址
        /// </summary>
        public string SensorAdr
        {
            get { return _sensoradr; }
            set { _sensoradr = value; }
        }


        private DateTime _datetime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime Datetime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }

        private int _pramcount;
        /// <summary>
        /// 参数数量
        /// </summary>
        public int PramCount
        {
            get { return _pramcount; }
            set { _pramcount = value; }
        }


        private List<Item_Data> _item_data;

        /// <summary>
        /// 数据区 监测项目--数据
        /// </summary>
        public List<Item_Data> Item_data
        {
            set { _item_data = value; }
            get { return _item_data; }
        }

    }

    public class Item_Data
    {
        private string _item;
        private decimal _data;

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
        public decimal Data
        {
            set { _data = value; }
            get { return _data; }
        }
    }
    #endregion
}
