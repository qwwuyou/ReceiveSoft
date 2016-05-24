using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace YYApp
{
    public class WriteReadXML
    {
        //原始配置文件路径
         string Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
         string ReturnPath = "";
        //界面重指向配置文件路径
        public void SetPath(string path)
        {
            ReturnPath = path;
        }

        //得到重指向后的配置文件路径
        public string GetPath()
        {
            if (ReturnPath != "")
            {
                //如果指定路径存在
                if (System.IO.File.Exists(ReturnPath))
                {
                    return ReturnPath;
                }
                else { return Path; }
            }
            else
            { return Path; }
        }

        //从原始配置文件中读取各项目配置文件路径信息
        public string GetXmlPath(string name) 
        {
            string path = Path;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path);
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project").SelectNodes("Pro");
            if (nls!=null)
            foreach (XmlNode item in nls)
            {
                if (item.Attributes["name"].Value == name) 
                {
                    path = item.Attributes["path"].Value;
                }
            }
            return path;
        }

        //从原始配置文件中读取项目名称列表
        public List<string> GetProjects() 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path);
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project").SelectNodes("Pro");
            List<string> projects = new List<string>();
            foreach (XmlNode item in nls)
            {
                projects.Add(item.Attributes["name"].Value );
            }
            return projects ;
        }


        /// <summary>
        /// 根据关键字得到配置文件
        /// </summary>
        /// <param name="keyword">关键字 目前Service可用</param>
        /// <returns></returns>
        public string GetXMLStr(string keyword)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode(keyword);

            return root.InnerXml;
        }

        //如果没有配置文件自动生成配置文件
        public  WriteReadXML()
        {
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
                elm  = xml.CreateElement("tcp");
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
                Elm.Attributes.Append(Encoding);
                system.AppendChild(Elm);
                XmlAttribute file = xml.CreateAttribute("file");
                file.InnerText = "";
                elm.Attributes.Append(file);
                XmlAttribute _class_ = xml.CreateAttribute("class");
                _class_.InnerText = "";
                elm.Attributes.Append(_class_);
                #endregion
                //最后将整个xml文件保存            
                xml.Save(Path); 
            }
        }

        #region 根据指向后路径读取信息的方法
        public List<service> ReadXML()
        {
            List<service> Lsm = new List<service>();
            service sm;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("tcp");
            foreach (XmlNode item in nls)
            {
                sm = new service();
                sm.SERVICETYPE = "TCP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.STATE = false;
                Lsm.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("udp");
            foreach (XmlNode item in nls)
            {
                sm = new service();
                sm.SERVICETYPE = "UDP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.STATE = false;
                Lsm.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("com");
            foreach (XmlNode item in nls)
            {
                sm = new service();
                sm.SERVICETYPE = "COM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.NUM = item.Attributes["num"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.STATE = false;
                Lsm.Add(sm);
            }

            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("gsm");
            foreach (XmlNode item in nls)
            {
                sm = new service();
                sm.SERVICETYPE = "GSM";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.NUM = item.Attributes["num"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                sm.STATE = false;
                Lsm.Add(sm);
            }
            xmlDoc.Save(GetPath());


            return Lsm;
        }

        public void WriteXML(List<service> Lsm)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode nl = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service");
            nl.InnerXml = "";
            if (Lsm.Count() == 0) 
            {
                xmlDoc.Save(GetPath());
                return;
            }

            foreach (service item in Lsm)
            {
                XmlNode xn = null;
                if (item.SERVICETYPE == "TCP")
                {
                    xn = xmlDoc.CreateElement("tcp");
                    XmlAttribute xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    xn.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "UDP")
                {
                    xn = xmlDoc.CreateElement("udp");
                    XmlAttribute xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    xn.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "COM")
                {
                    xn = xmlDoc.CreateElement("com");
                    XmlAttribute xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("num");
                    xa.Value = item.NUM;
                    xn.Attributes.Append(xa);
                }
                if (item.SERVICETYPE == "GSM")
                {
                    xn = xmlDoc.CreateElement("gsm");
                    XmlAttribute xa = xmlDoc.CreateAttribute("serviceid");
                    xa.Value = item.SERVICEID;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("ip_portname");
                    xa.Value = item.IP_PORTNAME;
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("port_baudrate");
                    xa.Value = item.PORT_BAUDRATE.ToString();
                    xn.Attributes.Append(xa);
                    xa = xmlDoc.CreateAttribute("num");
                    xa.Value = item.NUM;
                    xn.Attributes.Append(xa);
                }
                nl.AppendChild(xn);
                xmlDoc.Save(GetPath());
            }
        }

        public void ReadDBXML(out string type,out string server, out string catalog, out string username, out string password)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)root.SelectSingleNode("Type");
            type = nls.InnerText;
            nls = (XmlElement)root.SelectSingleNode("Source");
            server = nls.InnerText;
            nls = (XmlElement)root.SelectSingleNode("DataBase");
            catalog = nls.InnerText;
            nls = (XmlElement)root.SelectSingleNode("UserName");
            username = nls.InnerText;
            nls = (XmlElement)root.SelectSingleNode("PassWord");
            password = nls.InnerText;
            xmlDoc.Save(GetPath());
        }

        public void WriteDBXML(string type,string server, string catalog, string username, string password) 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)root.SelectSingleNode("Type");
            nls.InnerText = type;
            nls = (XmlElement)root.SelectSingleNode("Source");
            nls.InnerText=server ;
            nls = (XmlElement)root.SelectSingleNode("DataBase");
            nls.InnerText=catalog;
            nls = (XmlElement)root.SelectSingleNode("UserName");
            nls.InnerText=username;
            nls = (XmlElement)root.SelectSingleNode("PassWord");
            nls.InnerText=password;
            xmlDoc.Save(GetPath());
        }

        /// <summary>
        /// 设置反射协议信息
        /// </summary>
        /// <param name="File">dll文件</param>
        /// <param name="Class">操作类</param>
        public void WriteProtocolXML(string File, string Class)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").FirstChild;
            node.Attributes["file"].Value = File;
            node.Attributes["class"].Value = Class;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        public void WriteInfoEncodingXML(string Encoding)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo ");
            node.Attributes["Encoding"].Value = Encoding;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        public string ReadProjectXML() 
        {
            string Project = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project");
            Project = nls.InnerText;
            xmlDoc.Save(GetPath());
            return Project;
        }

        public void WriteProjectXML(string ProjectName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project ").SelectSingleNode("Pro");
            node.Attributes["name"].Value = ProjectName;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        public void ReadUIXML(out string IP, out string Port)
        {
            IP = "";
            Port = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI").SelectNodes("tcp");
            if (nls.Count > 0)
            {
                XmlNode item = nls[0];
                IP = item.Attributes["ip"].Value;
                Port = item.Attributes["port"].Value;
            }
            xmlDoc.Save(GetPath());
        }

        public string ReadDllXML() 
        {
            string file = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").SelectNodes("DataProtocol");
            if (nls.Count > 0)
            {
                XmlNode item = nls[0];
                file = item.Attributes["file"].Value;
            }
            xmlDoc.Save(GetPath());
            return file;
        }

        public string ReadResaveName()
        {
            string Name = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Resave");
            Name=node.Attributes["Name"].Value;
            xmlDoc.Save(GetPath());
            return Name;
        }

        public void WriteUIXML(string IP, string Port) 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI");
            root.InnerXml = "";
            XmlNode xn = null;
            xn = xmlDoc.CreateElement("tcp");
            XmlAttribute xa = xmlDoc.CreateAttribute("ip");
            xa.Value = IP;
            xn.Attributes.Append(xa);
            xa = xmlDoc.CreateAttribute("port");
            xa.Value = Port;
            xn.Attributes.Append(xa);
            root.AppendChild(xn);
            xmlDoc.Save(GetPath());
        }

        public void ReadLoginXML(out string UserName,out string PassWord) 
        {
            UserName = "";
            PassWord = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Login");
            XmlElement nls;
             nls = (XmlElement)root.SelectSingleNode("UserName");
            UserName = nls.InnerText;
            nls = (XmlElement)root.SelectSingleNode("PassWord");
            PassWord = nls.InnerText;
            xmlDoc.Save(GetPath());
        }

        public void WriteLoginXML(string UserName, string PassWord) 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Login");
            XmlElement nls;
             nls = (XmlElement)root.SelectSingleNode("UserName");
            nls.InnerText = UserName;
            nls = (XmlElement)root.SelectSingleNode("PassWord");
            nls.InnerText=PassWord  ;
            xmlDoc.Save(GetPath());
        }

        /// <summary>
        /// 根据关键字配置到文件
        /// </summary>
        /// <param name="keyword">关键字 目前Service可用</param>
        /// <returns></returns>
        public void SetXMLStr(string keyword,string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPath());
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode(keyword);
            root.InnerXml = xml;
            xmlDoc.Save(GetPath());
        }
        #endregion
    }

    static class SynXml 
    {
        public static string synxml(string data) 
        {

             string Rstr = "";
             string[] datas = data.Split(new string[] { "\n" }, StringSplitOptions.None);
             for (int k = 0; k < datas.Count(); k++)
             {
                 data = datas[k];
                 #region
                 if (data.Length > 7)
                 {
                     string tem = data.Substring(0, 7);
                     if (tem == "--file|") 
                     {
                         data = data.Replace("--file|","");
                         string[] temps = data.Split(new char[] { '|'});
                         if (temps.Length > 1)
                         {

                             Program.wrx.SetXMLStr(temps[0], temps[1]);
                             
                         }
                     }
                     else
                     { Rstr += data + "\n"; }
                 }
                 #endregion
             }
             return Rstr;


            
        }
    }


}
