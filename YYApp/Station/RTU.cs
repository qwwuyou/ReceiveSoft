using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYApp
{
    /// <summary>
    /// RTU基础信息列表
    /// </summary>
    class RTU
    {
        private string _stcd;
        private string _name;
        private string _pwd;
        private string _servicetype;

        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }

        /// <summary>
        /// 测站编码
        /// </summary>		
        public string NAME
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string PWD
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        /// <summary>
        /// 在线状态
        /// </summary>
        public string SERVICETYPE
        {
            get { return _servicetype; }
            set { _servicetype = value; }
        }

    }
}
