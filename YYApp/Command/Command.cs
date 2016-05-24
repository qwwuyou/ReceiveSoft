using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYApp
{
    /// <summary>
    /// 召测命令表
    /// </summary>
    class Command
    {
        private string _stcd;
        private string _servicetype;
        private string _commandid;
        private string _data;
        private DateTime _datetime;
        private int _state;


        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }

        /// <summary>
        /// 服务类型 TCP/UDP/GSM/COM
        /// </summary>		
        public string SERVICETYPE
        {
            get { return _servicetype; }
            set { _servicetype = value; }
        }

        /// <summary>
        /// 命令标识（功能码）
        /// </summary>
        public string CommandID
        {
            get { return _commandid; }
            set { _commandid = value; }
        }

        /// <summary>
        /// 召测命令数据
        /// </summary>
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// 命令状态变化时间
        /// </summary>
        public DateTime DATETIME
        {
            get { return _datetime; }
            set { _datetime = value; }
        }

        /// <summary>
        /// 命令状态
        /// </summary>
        public int STATE
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
