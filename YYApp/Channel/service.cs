using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYApp
{
    /// <summary>
    /// 服务列表
    /// </summary>
    public class service
    {
        private string _servicetype;
        private string _serviceid;
        private string _ip_portname;
        private int _port_baudrate;
        private string _num;
        private bool _state;
        private string _listcount;
        /// <summary>
        /// 服务类型
        /// </summary>		
        public string SERVICETYPE
        {
            get { return _servicetype; }
            set { _servicetype = value; }
        }

        /// <summary>
        /// 服务标示
        /// </summary>		
        public string SERVICEID
        {
            get { return _serviceid; }
            set { _serviceid = value; }
        }

        /// <summary>
        /// IP或串口号
        /// </summary>		
        public string IP_PORTNAME
        {
            get { return _ip_portname; }
            set { _ip_portname = value; }
        }

        /// <summary>
        /// 端口或波特率
        /// </summary>
        public int PORT_BAUDRATE
        {
            get { return _port_baudrate; }
            set { _port_baudrate = value; }
        }

        public string NUM
        {
            get { return _num; }
            set { _num = value; }
        }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool STATE
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 列表内记录数量
        /// </summary>
        public string LISTCOUNT
        {
            get { return _listcount; }
            set { _listcount = value; }
        }
    }
}
