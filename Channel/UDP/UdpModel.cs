using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace UdpService
{

    //udp连接列表
    public class UdpSocket
    {
        private string _stcd;
        private IPEndPoint _ipendpoint;
        private DateTime _connecttime;
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
        /// udp的通讯标识
        /// </summary>
        public IPEndPoint IpEndPoint
        {
            get { return _ipendpoint; }
            set { _ipendpoint = value; }
        }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime CONNECTTIME
        {
            get { return _connecttime; }
            set { _connecttime = value; }
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
    public class UdpReceivedData
    {
        private byte[] _data;
        private IPEndPoint _ipendpoint;

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// udp的通讯标识
        /// </summary>
        public IPEndPoint IpEndPoint
        {
            get { return _ipendpoint; }
            set { _ipendpoint = value; }
        }
    }

    //回复的数据（召测用）
    public class UdpSendData
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
