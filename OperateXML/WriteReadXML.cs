using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OperateXML
{
    public class WriteReadXML
    {
        public XMLObject XMLObj = new XMLObject();

        //日志记录对象
        public static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        //如果没有配置文件自动生成配置文件
        public WriteReadXML()
        {
            string Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
            if (!System.IO.File.Exists(Path))
            {
                //首先创建 XmlDocument xml文档 
                XmlDocument xml = new XmlDocument();
                //创建根节点 config 
                XmlElement system = xml.CreateElement("system");
                //把根节点加到xml文档中            
                xml.AppendChild(system);

                #region 工程项目配置信息
                //创建一个节点 Project(用于做子节点)         
                XmlElement Elm = xml.CreateElement("Project");
                //将Elm添加为Project的子节点 
                system.AppendChild(Elm);
                XmlElement elm = xml.CreateElement("Pro");
                Elm.AppendChild(elm);
                XmlAttribute name = xml.CreateAttribute("name");
                name.InnerText = "未知";
                elm.Attributes.Append(name);
                XmlAttribute path = xml.CreateAttribute("path");
                path.InnerText = Path;
                elm.Attributes.Append(path);
                #endregion

                #region 数据库配置信息
                //创建一个节点 DataBaseConnect(用于做子节点) 
                Elm = xml.CreateElement("DataBaseConnect");
                //将Elm添加为DataBaseConnect的子节点 
                system.AppendChild(Elm);
                elm = xml.CreateElement("Type");
                Elm.AppendChild(elm);
                elm.InnerText = "MSSQL";
                XmlAttribute xa=xml.CreateAttribute("Port");
                xa.InnerText = "1433";
                elm.Attributes.Append(xa);
                elm = xml.CreateElement("Source");
                Elm.AppendChild(elm);
                elm.InnerText = ".";
                elm = xml.CreateElement("DataBase");
                Elm.AppendChild(elm);
                elm.InnerText = "DB";
                elm = xml.CreateElement("UserName");
                Elm.AppendChild(elm);
                elm.InnerText = "sa";
                elm = xml.CreateElement("PassWord");
                Elm.AppendChild(elm);
                elm.InnerText = "123";
                #endregion

                #region 配置组件信息
                Elm = xml.CreateElement("Dll");
                //将Elm添加为DataBaseConnect的子节点 
                system.AppendChild(Elm);
                elm = xml.CreateElement("DataProtocol");
                Elm.AppendChild(elm);
                XmlAttribute _file = xml.CreateAttribute("file");
                _file.InnerText = "GSProtocol.dll";
                elm.Attributes.Append(_file);
                XmlAttribute _class = xml.CreateAttribute("class");
                _class.InnerText = "Service.GS";
                elm.Attributes.Append(_class);
                #endregion

                #region 配置信道服务信息
                Elm = xml.CreateElement("Service");
                //将Elm添加为Service的子节点 
                system.AppendChild(Elm);
                elm = xml.CreateElement("tcp");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                XmlAttribute serviceid = xml.CreateAttribute("serviceid");
                serviceid.Value = "1";
                XmlAttribute ip_portname = xml.CreateAttribute("ip_portname");
                ip_portname.Value = "0.0.0.0";
                XmlAttribute port_baudrate = xml.CreateAttribute("port_baudrate");
                port_baudrate.Value = "111";
                elm.Attributes.Append(serviceid);
                elm.Attributes.Append(ip_portname);
                elm.Attributes.Append(port_baudrate);

                elm = xml.CreateElement("udp");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                serviceid = xml.CreateAttribute("serviceid");
                serviceid.Value = "1";
                ip_portname = xml.CreateAttribute("ip_portname");
                ip_portname.Value = "0.0.0.0";
                port_baudrate = xml.CreateAttribute("port_baudrate");
                port_baudrate.Value = "222";
                elm.Attributes.Append(serviceid);
                elm.Attributes.Append(ip_portname);
                elm.Attributes.Append(port_baudrate);

                elm = xml.CreateElement("gsm");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                serviceid = xml.CreateAttribute("serviceid");
                serviceid.Value = "1";
                ip_portname = xml.CreateAttribute("ip_portname");
                ip_portname.Value = "com1";
                port_baudrate = xml.CreateAttribute("port_baudrate");
                port_baudrate.Value = "9600";
                XmlAttribute num = xml.CreateAttribute("num");
                num.Value = "13800000000";
                elm.Attributes.Append(serviceid);
                elm.Attributes.Append(ip_portname);
                elm.Attributes.Append(port_baudrate);
                elm.Attributes.Append(num);

                elm = xml.CreateElement("com");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                serviceid = xml.CreateAttribute("serviceid");
                serviceid.Value = "1";
                ip_portname = xml.CreateAttribute("ip_portname");
                ip_portname.Value = "com2";
                port_baudrate = xml.CreateAttribute("port_baudrate");
                port_baudrate.Value = "9600";
                num = xml.CreateAttribute("num");
                num.Value = "123456";
                elm.Attributes.Append(serviceid);
                elm.Attributes.Append(ip_portname);
                elm.Attributes.Append(port_baudrate);
                elm.Attributes.Append(num);
                #endregion

                #region 配置界面通讯信息
                Elm = xml.CreateElement("UI");
                //将Elm添加为Service的子节点 
                system.AppendChild(Elm);
                elm = xml.CreateElement("tcp");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                XmlAttribute ip = xml.CreateAttribute("ip");
                //ip属性的内容为""          
                ip.InnerText = "0.0.0.0";
                //创建完标签的属性timeout 后需要将其添加到ini标签的属性里  
                elm.Attributes.Append(ip);
                XmlAttribute port = xml.CreateAttribute("port");
                //ip属性的内容为""          
                port.InnerText = "8888";
                //创建完标签的属性timeout 后需要将其添加到ini标签的属性里  
                elm.Attributes.Append(port);
                #endregion

                #region 配置透传信息
                Elm = xml.CreateElement("TC");
                //将Elm添加为Service的子节点 
                system.AppendChild(Elm);
                elm = xml.CreateElement("tcp");
                //将Elm添加为Service的子节点 
                Elm.AppendChild(elm);
                ip = xml.CreateAttribute("ip");
                //ip属性的内容为""          
                ip.InnerText = "0.0.0.0";
                //创建完标签的属性timeout 后需要将其添加到ini标签的属性里  
                elm.Attributes.Append(ip);
                port = xml.CreateAttribute("port");
                //ip属性的内容为""          
                port.InnerText = "333";
                //创建完标签的属性timeout 后需要将其添加到ini标签的属性里  
                elm.Attributes.Append(port);
                #endregion

                #region 登录信息配置
                Elm = xml.CreateElement("Login");
                system.AppendChild(Elm);
                elm = xml.CreateElement("UserName");
                elm.InnerText = "admin";
                Elm.AppendChild(elm);
                elm = xml.CreateElement("PassWord");
                elm.InnerText = "123";
                Elm.AppendChild(elm);
                #endregion

                #region 是否发送Mail信息配置
                Elm = xml.CreateElement("Mail");
                XmlAttribute IsToMail = xml.CreateAttribute("IsToMail");
                IsToMail.Value = "0";
                Elm.Attributes.Append(IsToMail);
                system.AppendChild(Elm);
                #endregion

                #region 写入信息日志编码格式信息配置<WriteInfo Encoding ="HEX"/>或<WriteInfo Encoding ="ASC"/>
                Elm = xml.CreateElement("WriteInfo");
                XmlAttribute Encoding = xml.CreateAttribute("Encoding");
                Encoding.Value = "HEX";
                Elm.Attributes.Append(Encoding);
                system.AppendChild(Elm);
                #endregion

                #region 写入转发配置信息
                Elm = xml.CreateElement("Resave");
                XmlAttribute Name = xml.CreateAttribute("Name");
                Name.Value = "";
                Elm.Attributes.Append(Name);
                
                XmlAttribute file = xml.CreateAttribute("file");
                file.InnerText = "";
                Elm.Attributes.Append(file);
                XmlAttribute _class_ = xml.CreateAttribute("class");
                _class_.InnerText = "";
                Elm.Attributes.Append(_class_);
                system.AppendChild(Elm);
                #endregion
                //最后将整个xml文件保存            
                xml.Save(Path);
            }
        }


        public void ReadXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNodeList nls;
            XmlNode node;
            #region  projects 
            XMLObj.projects.Clear();
            try
            {
                nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project").SelectNodes("Pro");
                OperateXML.ProjectInfo pi;
                foreach (XmlNode item in nls)
                {
                    pi = new ProjectInfo();
                    pi.Project = item.Attributes["name"].Value;
                    pi.Path = item.Attributes["path"].Value;
                    XMLObj.projects.Add(pi);
                }
                log.Warn(DateTime.Now + "读取服务配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取服务配置文件失败！", e);
            }
            #endregion

            #region DB
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XMLObj.DBtype = node.SelectSingleNode("Type").InnerText;
            XMLObj.DBport = node.SelectSingleNode("Type").Attributes["Port"].Value;
            XMLObj.DBserver = node.SelectSingleNode("Source").InnerText; 
            XMLObj.DBcatalog = node.SelectSingleNode("DataBase").InnerText; 
            XMLObj.DBusername = node.SelectSingleNode("UserName").InnerText; 
            XMLObj.DBpassword = node.SelectSingleNode("PassWord").InnerText; 
            #endregion

            #region serviceModel
            XMLObj.LsM.Clear();
            serviceModel sm;
            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("tcp");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "TCP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                XMLObj.LsM.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("udp");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "UDP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                XMLObj.LsM.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("com");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "COM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.NUM = item.Attributes["num"].Value;
                XMLObj.LsM.Add(sm);
            }

            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("gsm");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "GSM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.NUM = item.Attributes["num"].Value;
                XMLObj.LsM.Add(sm);
            }
            #endregion

            #region UITcpModel
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI").FirstChild;
                XMLObj.UiTcpModel.IP = node.Attributes["ip"].Value;
                XMLObj.UiTcpModel.PORT = int.Parse(node.Attributes["port"].Value);
                log.Warn(DateTime.Now + "读取服务与界面交互配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取服务与界面交互配置服务配置文件失败！", e);
            }
            #endregion

            #region 透传TCTcpModel
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("TC").FirstChild;
                XMLObj.TCTcpModel.IP = node.Attributes["ip"].Value;
                XMLObj.TCTcpModel.PORT = int.Parse(node.Attributes["port"].Value);
                log.Warn(DateTime.Now + "读取透传配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取透传配置文件失败！", e);
            }
            #endregion

            #region ReadIsToMail
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Mail");
                if (node.Attributes["IsToMail"].Value == "1")
                {
                    XMLObj.IsToMail = true;
                }
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取是服务停止后是否发送异常信息到管理员Mail失败！", e); }
            #endregion

            #region ReadHexOrAsc
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo");
                XMLObj.HEXOrASC = node.Attributes["Encoding"].Value;

            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取写入日志信息的编码格式的配置文件失败！", e); }
            #endregion

            #region dll
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").FirstChild;
                XMLObj.dllfile = node.Attributes["file"].Value;
                XMLObj.dllclass = node.Attributes["class"].Value;
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取协议反射类或判断服务是否为中心服务失败,默认为非中心服务！", e); }

            #endregion

            #region login
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Login");
            XMLObj.UserName = node.SelectSingleNode("UserName").InnerText;
            XMLObj.PassWord = node.SelectSingleNode("PassWord").InnerText;
            #endregion

            #region 转存Resave
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Resave");
            XMLObj.ResaveName = node.Attributes["Name"].Value;
            XMLObj.ResaveFile = node.Attributes["file"].Value;
            XMLObj.ResaveClass = node.Attributes["class"].Value;
            #endregion

            #region Style
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Style");
                XMLObj.Style = node.InnerText;
            }
            catch { }
            #endregion
        }
        public void ReadXML(string path)
        {
            string Path = path;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList nls;
            XmlNode node;
            #region  projects
            XMLObj.projects.Clear();
            try
            {
                nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project").SelectNodes("Pro");
                OperateXML.ProjectInfo pi;
                foreach (XmlNode item in nls)
                {
                    pi = new ProjectInfo();
                    pi.Project = item.Attributes["name"].Value;
                    pi.Path = item.Attributes["path"].Value;
                    XMLObj.projects.Add(pi);
                }
                log.Warn(DateTime.Now + "读取服务配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取服务配置文件失败！", e);
            }
            #endregion

            #region DB
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XMLObj.DBtype = node.SelectSingleNode("Type").InnerText;
            XMLObj.DBport = node.SelectSingleNode("Type").Attributes["Port"].Value;
            XMLObj.DBserver = node.SelectSingleNode("Source").InnerText;
            XMLObj.DBcatalog = node.SelectSingleNode("DataBase").InnerText;
            XMLObj.DBusername = node.SelectSingleNode("UserName").InnerText;
            XMLObj.DBpassword = node.SelectSingleNode("PassWord").InnerText;
            #endregion

            #region serviceModel
            serviceModel sm;
            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("tcp");
            XMLObj.LsM.Clear();
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "TCP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                XMLObj.LsM.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("udp");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "UDP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                XMLObj.LsM.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("com");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "COM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.NUM = item.Attributes["num"].Value;
                XMLObj.LsM.Add(sm);
            }

            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("gsm");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "GSM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.NUM = item.Attributes["num"].Value;
                XMLObj.LsM.Add(sm);
            }
            #endregion

            #region UITcpModel
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI").FirstChild;
                XMLObj.UiTcpModel.IP = node.Attributes["ip"].Value;
                XMLObj.UiTcpModel.PORT = int.Parse(node.Attributes["port"].Value);
                log.Warn(DateTime.Now + "读取服务与界面交互配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取服务与界面交互配置服务配置文件失败！", e);
            }
            #endregion

            #region 透传TCTcpModel
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("TC").FirstChild;
                XMLObj.TCTcpModel.IP = node.Attributes["ip"].Value;
                XMLObj.TCTcpModel.PORT = int.Parse(node.Attributes["port"].Value);
                log.Warn(DateTime.Now + "读取透传配置文件成功！");
            }
            catch (Exception e)
            {
                log.Warn(DateTime.Now + "读取透传配置文件失败！", e);
            }
            #endregion

            #region ReadIsToMail
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Mail");
                if (node.Attributes["IsToMail"].Value == "1")
                {
                    XMLObj.IsToMail = true;
                }
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取是服务停止后是否发送异常信息到管理员Mail失败！", e); }
            #endregion

            #region ReadHexOrAsc
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo");
                XMLObj.HEXOrASC = node.Attributes["Encoding"].Value;

            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取写入日志信息的编码格式的配置文件失败！", e); }
            #endregion

            #region dll
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").FirstChild;
                XMLObj.dllfile = node.Attributes["file"].Value;
                XMLObj.dllclass = node.Attributes["class"].Value;
            }
            catch (Exception e)
            { log.Warn(DateTime.Now + "读取协议反射类或判断服务是否为中心服务失败,默认为非中心服务！", e); }

            #endregion

            #region login
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Login");
            XMLObj.UserName = node.SelectSingleNode("UserName").InnerText;
            XMLObj.PassWord = node.SelectSingleNode("PassWord").InnerText;
            #endregion

            #region 转存Resave
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Resave");
            XMLObj.ResaveName = node.Attributes["Name"].Value;
            XMLObj.ResaveFile = node.Attributes["file"].Value;
            XMLObj.ResaveClass = node.Attributes["class"].Value;
            #endregion


            #region Style
            try
            {
                node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Style");
                XMLObj.Style = node.InnerText;
            }
            catch { }
            #endregion

        }

        public  void WriteXML()
        {
            string Path=System.Windows.Forms.Application.StartupPath + "/System.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path);

            XmlNode node;
            XmlAttribute xa;

            #region Projects
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project ") ;
            node.InnerText = "";
            foreach (var item in XMLObj.projects )
            {
                XmlElement xe= xmlDoc.CreateElement("Pro");
                xa = xmlDoc.CreateAttribute("name");
                xa.Value = item.Project;
                xe.Attributes.Append(xa);
                xa = xmlDoc.CreateAttribute("path");
                xa.Value = item.Path;
                xe.Attributes.Append(xa);
                node.AppendChild(xe);
            }
            xmlDoc.Save(Path);
            #endregion

            #region UI IP，PORT
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI");
            node.InnerText = "";
            XmlNode xn = null;
            xn = xmlDoc.CreateElement("tcp");
            xa = xmlDoc.CreateAttribute("ip");
            xa.Value =XMLObj.UiTcpModel.IP;
            xn.Attributes.Append(xa);
            xa = xmlDoc.CreateAttribute("port");
            xa.Value = XMLObj.UiTcpModel.PORT.ToString();
            xn.Attributes.Append(xa);
            node.AppendChild(xn);
            xmlDoc.Save(Path);
            #endregion

            #region 透传IP，PORT
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("TC").FirstChild;
            node.Attributes["ip"].Value = XMLObj.TCTcpModel.IP;
            node.Attributes["port"].Value = XMLObj.TCTcpModel.PORT.ToString();
            xmlDoc.Save(Path);
            #endregion

            #region serviceModel
            XmlNode nl = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service");
            nl.InnerText = "";
            foreach (var item in XMLObj.LsM)
            {
                
                if (item.SERVICETYPE == "TCP")
                {
                    node = xmlDoc.CreateElement("tcp");
                    xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    node.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "UDP")
                {
                    node = xmlDoc.CreateElement("udp");
                    xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    node.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "COM")
                {
                    node = xmlDoc.CreateElement("com");
                    xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("num");
                    xa.Value = item.NUM;
                    node.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "GSM")
                {
                    node = xmlDoc.CreateElement("gsm");
                    xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    node.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("num");
                    xa.Value = item.NUM;
                    node.Attributes.Append(xa);
                }
                nl.AppendChild(node);
            }
            xmlDoc.Save(Path);
            #endregion

            #region DB
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)node.SelectSingleNode("Type");
            nls.InnerText = XMLObj. DBtype;
            nls = (XmlElement)node.SelectSingleNode("Source");
            nls.InnerText=XMLObj. DBserver ;
            nls = (XmlElement)node.SelectSingleNode("DataBase");
            nls.InnerText=XMLObj. DBcatalog;
            nls = (XmlElement)node.SelectSingleNode("UserName");
            nls.InnerText=XMLObj. DBusername;
            nls = (XmlElement)node.SelectSingleNode("PassWord");
            nls.InnerText=XMLObj. DBpassword;
            xmlDoc.Save(Path);
            #endregion

            #region dll
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").FirstChild;
            node.Attributes["file"].Value = XMLObj.dllfile;
            node.Attributes["class"].Value = XMLObj.dllclass;
            xmlDoc.Save(Path );
            #endregion

            #region Info编码
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo ");
            node.Attributes["Encoding"].Value = XMLObj.HEXOrASC;
            xmlDoc.Save(Path);
            #endregion

            #region login
            node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Login");
            node.SelectSingleNode("UserName").InnerText =XMLObj. UserName;
            node.SelectSingleNode("PassWord").InnerText=XMLObj.PassWord  ;
            xmlDoc.Save(Path);
            #endregion

        }

        /// <summary>
        /// 根据关键字得到配置文件
        /// </summary>
        /// <param name="keyword">关键字 目前Service可用</param>
        /// <returns></returns>
        public  string GetXMLStr(string keyword)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode(keyword);
            return root.InnerXml;
        }


        /// <summary>
        /// 根据关键字配置到文件
        /// </summary>
        /// <param name="keyword">关键字 目前Service可用</param>
        /// <returns></returns>
        public  void SetXMLStr(string keyword, string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode(keyword);
            root.InnerXml = xml;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

    }
}
