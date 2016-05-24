using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComService
{
    public class ComSatellite
    {
        private string _stcd;
        private string _satellite;
        private DateTime _datatime;

        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }

        /// <summary>
        /// 卫星标识
        /// </summary>
        public string SATELLITE
        {
            get { return _satellite; }
            set { _satellite = value; }
        }

        /// <summary>
        /// 数据接收时间(最新)
        /// </summary>
        public DateTime DATATIME
        {
            get { return _datatime; }
            set { _datatime = value; }
        }
    }

    //收到的数据
    public class ComReceivedData
    {
        private byte[] _data;

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

    }

    //回复的数据
    public class ComSendData
    {
        private string _stcd;
        private byte[] _data;

        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }


        /// <summary>
        /// 要发送的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
