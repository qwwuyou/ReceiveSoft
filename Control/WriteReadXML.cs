using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Service
{
    class WriteReadXML
    {
        //从原始配置文件中读取项目名称列表
        public List<string> GetProjects()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Project").SelectNodes("Pro");
            List<string> projects = new List<string>();
            foreach (XmlNode item in nls)
            {
                projects.Add(item.Attributes["name"].Value);
            }
            return projects;
        }

        /// <summary>
        /// 读取服务列表
        /// </summary>
        /// <returns></returns>
        public List <serviceModel> ReadServiceXML() 
        {
            List<serviceModel> Lsm = new List<serviceModel>();
            serviceModel sm;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml"); 
            XmlNodeList nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("tcp");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "TCP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                Lsm.Add(sm);
            }


            nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Service").SelectNodes("udp");
            foreach (XmlNode item in nls)
            {
                sm = new serviceModel();
                sm.SERVICETYPE = "UDP";
                sm.SERVICEID = item.Attributes["serviceid"].Value;
                sm.IP_PORTNAME = item.Attributes["ip_portname"].Value;
                sm.PORT_BAUDRATE = int.Parse(item.Attributes["port_baudrate"].Value);
                Lsm.Add(sm);
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
                Lsm.Add(sm);
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
                Lsm.Add(sm);
            }
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml"); 
            return Lsm;
        }


        /// <summary>
        /// 读取服务与界面交互ip和端口号
        /// </summary>
        /// <returns></returns>
        public UITcpModel ReadUITcpXML()
        {
            UITcpModel utm=new UITcpModel();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("UI").FirstChild;
            utm.IP = node.Attributes["ip"].Value;
            utm.PORT = int.Parse(node.Attributes["port"].Value);
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml"); 

            return utm;
        }

        /// <summary>
        /// 读取数据处理反射用的dll
        /// </summary>
        /// <param name="File">文件名(带后缀)</param>
        /// <param name="Class">类名(带命名空间)</param>
        public void ReadDllXML(out string File ,out string Class) 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Dll").FirstChild;
            File = node.Attributes["file"].Value;
            Class = node.Attributes["class"].Value;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        /// <summary>
        /// 读取透传的ip和端口号
        /// </summary>
        /// <returns></returns>
        public UITcpModel ReadTCTcpXML()
        {
            UITcpModel utm = new UITcpModel();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("TC").FirstChild;
            utm.IP = node.Attributes["ip"].Value;
            utm.PORT = int.Parse(node.Attributes["port"].Value);
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");

            return utm;
        }

        /// <summary>
        /// 写入透传的ip和端口号
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        public void WriteTCXML(string IP ,string Port) 
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("TC").FirstChild;
            node.Attributes["ip"].Value = IP;
            node.Attributes["port"].Value = Port;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
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

        //根据协议设置编码方式
        public void WriteInfoEncodingXML(string Encoding)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo ");
            node.Attributes["Encoding"].Value = Encoding;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        /// <summary>
        /// 根据关键字配置到文件
        /// </summary>
        /// <param name="keyword">关键字 目前Service可用</param>
        /// <returns></returns>
        public void SetXMLStr(string keyword, string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode(keyword);
            root.InnerXml = xml;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }

        /// <summary>
        /// 是否将异常发送至管理员Mail
        /// </summary>
        /// <returns></returns>
        public bool ReadIsToMail()
        {
             bool b= false;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("Mail");
            if (nls.Attributes["IsToMail"].Value == "1") 
            {
                b = true;
            } 
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
            return b;
        }

        /// <summary>
        /// 读取写入日志信息的编码格式
        /// </summary>
        /// <returns></returns>
        public string ReadHexOrAsc() 
        { 
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode nls = xmlDoc.SelectSingleNode("system").SelectSingleNode("WriteInfo");
            return nls.Attributes["Encoding"].Value;
        }

        /// <summary>
        /// 对象序列化成 XML String
        /// List<string> al = new List<string>();
        /// al.Add("xsm");
        /// string xml = Serializer.XmlSerialize<List<string>>(strPath, Encoding.UTF8, al);
        /// al = Serializer.XmlDeserialize<List<string>>(strPath, Encoding.UTF8);
        /// </summary>
        public void XmlSerialize<T>(string path, Encoding encoding, T obj)
        {
            string xmlString = string.Empty;
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }

            System.IO.File.WriteAllText(path, xmlString, encoding);
            //return xmlString;
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// List<string> al = new List<string>();
        /// al.Add("xsm");
        /// string xml = Serializer.XmlSerialize<List<string>>(strPath, Encoding.UTF8, al);
        /// al = Serializer.XmlDeserialize<List<string>>(strPath, Encoding.UTF8);
        /// </summary>
        //public T XmlDeserialize<T>(string path, Encoding encoding)
        //{
        //    string xmlString = "";
        //    if (System.IO.File.Exists(path))
        //    {
        //        xmlString = System.IO.File.ReadAllText(path, encoding);
        //    }

        //    T t = default(T);
        //    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        //    using (System.IO.Stream xmlStream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
        //    {
        //        using (XmlReader xmlReader = XmlReader.Create(xmlStream))
        //        {
        //            Object obj = xmlSerializer.Deserialize(xmlReader);
        //            t = (T)obj;
        //        }
        //    }
        //    return t;
        //}
    }
}
