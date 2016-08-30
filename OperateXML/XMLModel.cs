using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperateXML
{
    public class XMLObject
    {
        #region xml对象
        /// <summary>
        /// 界面样式
        /// </summary>
        public string Style = "";

        /// <summary>
        /// 项目名
        /// </summary>
        public List<ProjectInfo> projects = new List<ProjectInfo>();

        /// <summary>
        /// 服务列表
        /// </summary>
        public List<serviceModel> LsM = new List<serviceModel>();

        /// <summary>
        /// 访问界面通讯对象
        /// </summary>
        public TcpModel UiTcpModel = new TcpModel();

        /// <summary>
        /// 透传用实体
        /// </summary>
        public TcpModel TCTcpModel = new TcpModel();

        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public  bool IsToMail = false;

        /// <summary>
        /// 写入日志信息的编码格式
        /// </summary>
        public  string HEXOrASC = "";

        #region 数据库对象
        public string DBtype="";
        public string DBport = "";
        public string DBserver = "";
        public string DBcatalog = "";
        public string DBusername = "";
        public string DBpassword = "";
        #endregion

        #region 反射对象
        //协议组件
        public  string dllfile = "";
        public  string dllclass = "";
        #endregion

        #region 登录
        public  string UserName = "";
        public  string PassWord = "";
        #endregion

        #region 转存对象
        public string ResaveName = "";
        public string ResaveFile = "";
        public string ResaveClass = "";
        #endregion
        #endregion
    }
    /// <summary>
    /// 服务列表
    /// </summary>
    public class serviceModel
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

    /// <summary>
    /// 与界面交互/透传的tcp信息配置
    /// </summary>
    public class TcpModel
    {
        private string _ip;
        private int _port;
        /// <summary>
        /// IP
        /// </summary>		
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int PORT
        {
            get { return _port; }
            set { _port = value; }
        }
    }

    /// <summary>
    /// 项目&路径
    /// </summary>
    public class ProjectInfo
    {
        private string _project;
        private string _path;
        /// <summary>
        /// Project
        /// </summary>		
        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }

        /// <summary>
        /// Path
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }

}
