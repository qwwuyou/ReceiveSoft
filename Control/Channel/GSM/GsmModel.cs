using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GsmService
{
    //测站手机号列表
    public class GsmMobile
    {
        private string _stcd;
        private string _mobile;
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
        /// 手机号
        /// </summary>
        public string MOBILE
        {
            get { return _mobile; }
            set { _mobile = value; }
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
    public class GsmReceivedData
    {
        private byte[] _data;
        private string _mobile;
        private DateTime _senddatetime;

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MOBILE
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SENDDATETIME 
        {
            get { return _senddatetime; }
            set { _senddatetime = value; }
        }
    }

    //回复的数据(召测用)
    public class GsmSendData
    {
        private string _stcd;
        private byte[] _data; 
        private int _state;
        private string _commandcode;

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

        /// <summary>
        /// 召测命令状态  0新产生未激活发送  1第一次发送完成  2第二次发送完成
        /// </summary>
        public int STATE
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 命令码
        /// </summary>
        public string COMMANDCODE
        {
            get { return _commandcode; }
            set { _commandcode = value; }
        }
    }
}
