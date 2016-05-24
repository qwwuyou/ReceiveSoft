using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HJT212_2005
{
    public class CN_DataList
    {
        private string _cn;
        private DateTime _tm;
        private string _st;
        private List<DataModel> _dm;


        /// <summary>
        /// CN
        /// </summary>		
        public string CN
        {
            get { return _cn; }
            set { _cn = value; }
        }

        
        /// <summary>
        /// 监测时间
        /// </summary>
        public DateTime TM 
        {
            get { return _tm; }
            set { _tm = value; }
        }

        /// <summary>
        /// ST 污染源类型
        /// </summary>		
        public string ST
        {
            get { return _st; }
            set { _st = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<DataModel> DM 
        {
            get { return _dm; }
            set { _dm = value; }
        }

    }

    public class DataModel 
    {
        private string _itemcode;
        private string _key;
        private decimal? _datavalue;

        /// <summary>
        /// 监测元素编码
        /// </summary>
        public string ItemCode
        {
            set { _itemcode = value; }
            get { return _itemcode; }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KEY
        {
            set { _key = value; }
            get { return _key; }
        }

        /// <summary>
        /// 数据值
        /// </summary>
        public decimal? DATAVALUE
        {
            set { _datavalue = value; }
            get { return _datavalue; }
        }
    }
}
