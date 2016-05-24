using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace DataResaveTool
{
    class XMLOperation
    {
        #region 读写XML
        //读写转存库配置
        public void ReadDBXML(out string type, out string server, out string catalog, out string username, out string password)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
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
            xmlDoc.Save(path);
        }
        public void WriteDBXML(string type, string server, string catalog, string username, string password)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)root.SelectSingleNode("Type");
            nls.InnerText = type;
            nls = (XmlElement)root.SelectSingleNode("Source");
            nls.InnerText = server;
            nls = (XmlElement)root.SelectSingleNode("DataBase");
            nls.InnerText = catalog;
            nls = (XmlElement)root.SelectSingleNode("UserName");
            nls.InnerText = username;
            nls = (XmlElement)root.SelectSingleNode("PassWord");
            nls.InnerText = password;
            xmlDoc.Save(path);
        }

        //读写字段配置
        public string ReadFieldToFieldXML()
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("FieldToField");
            return root.InnerXml;
        }
        public void WriteFieldToFieldXML(string xml)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("FieldToField");
            root.InnerXml = xml;
            xmlDoc.Save(path);
        }


        public string ReadRainStage(out bool RainCheckedState, out bool StageCheckedState)
        {
            string RS = "";
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Rain");
            RainCheckedState = bool.Parse(root.InnerText);
            RS = root.InnerText + "|";
            root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Stage");
            StageCheckedState = bool.Parse(root.InnerText);
            return RS += root.InnerText;
        }
        public void WriteRainStage(string RainCheckedState)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Rain");
            root.InnerText = RainCheckedState;// checkBox_Rain.Checked.ToString();
            root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Stage");
            root.InnerText = RainCheckedState;// checkBox_Rain.Checked.ToString();
            xmlDoc.Save(path);
        }

        //读写执行时间
        public string ReadDateTimeXML()
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DateTime");
            return root.InnerText;
        }
        public void WriteDateTimeXML(DateTime datetime)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DateTime");
            root.InnerText = datetime.ToString("yyyy-MM-dd HH:mm:ss");
            xmlDoc.Save(path);
        }

        //读写执行时间间隔
        public string ReadMinuteXML()
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Minute");
            return root.InnerText;
        }
        public void WriteMinuteXML(int Minute)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Minute");
            root.InnerText = Minute.ToString();
            xmlDoc.Save(path);
        }


        #region list 与 xml 互转
        /// <summary>
        /// 将简单的xml字符串转换成为LIST
        /// </summary>
        /// <typeparam name="T">类型，仅仅支持int/long/datetime/string/double/decimal/object</typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> xmlToList<T>(string xml)
        {
            Type tp = typeof(T);
            List<T> list = new List<T>();
            if (xml == null || string.IsNullOrEmpty(xml))
            {
                return list;
            }
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(xml);
                if (tp == typeof(string) | tp == typeof(int) | tp == typeof(long) | tp == typeof(DateTime) | tp == typeof(double) | tp == typeof(decimal))
                {
                    System.Xml.XmlNodeList nl = doc.SelectNodes("/root/item");
                    if (nl.Count == 0)
                    {
                        return list;
                    }
                    else
                    {
                        foreach (System.Xml.XmlNode node in nl)
                        {
                            if (tp == typeof(string)) { list.Add((T)(object)Convert.ToString(node.InnerText)); }
                            else if (tp == typeof(int)) { list.Add((T)(object)Convert.ToInt32(node.InnerText)); }
                            else if (tp == typeof(long)) { list.Add((T)(object)Convert.ToInt64(node.InnerText)); }
                            else if (tp == typeof(DateTime)) { list.Add((T)(object)Convert.ToDateTime(node.InnerText)); }
                            else if (tp == typeof(double)) { list.Add((T)(object)Convert.ToDouble(node.InnerText)); }
                            else if (tp == typeof(decimal)) { list.Add((T)(object)Convert.ToDecimal(node.InnerText)); }
                            else { list.Add((T)(object)node.InnerText); }
                        }
                        return list;
                    }
                }
                else
                {
                    //如果是自定义类型就需要反序列化了
                    System.Xml.XmlNodeList nl = doc.SelectNodes("/root/items/" + typeof(T).Name);
                    if (nl.Count == 0)
                    {
                        return list;
                    }
                    else
                    {
                        foreach (System.Xml.XmlNode node in nl)
                        {
                            list.Add(XMLToObject<T>(node.OuterXml));
                        }
                        return list;
                    }
                }

            }
            catch (XmlException ex)
            {
                throw new ArgumentException("不是有效的XML字符串", "xml");
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("指定的数据类型不匹配", "T");
            }
            catch (Exception exx)
            {
                throw exx;
            }
        }

        /// <summary>
        /// 将List转化为xml字符串
        /// </summary>
        /// <typeparam name="T">类型，仅仅支持int/long/datetime/string/double/decimal/object</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string listToXml<T>(List<T> list)
        {
            Type tp = typeof(T);
            string xml = "<root>";
            if (tp == typeof(string) | tp == typeof(int) | tp == typeof(long) | tp == typeof(DateTime) | tp == typeof(double) | tp == typeof(decimal))
            {
                foreach (T obj in list)
                {
                    xml = xml + "<item>" + obj.ToString() + "</item>";
                }
            }
            else
            {
                xml = xml + "<items>";
                foreach (T obj in list)
                {
                    xml = xml + ObjectToXML<T>(obj);
                }
                xml = xml + "</items>";
            }

            xml = xml + "</root>";
            return xml;
        }

        // 序列化xml/反序列化
        /// <summary>
        /// 对象序列化为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ObjectToXML<T>(T obj, System.Text.Encoding encoding)
        {
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            Encoding utf8EncodingWithNoByteOrderMark = new UTF8Encoding(false);
            using (MemoryStream mem = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Encoding = utf8EncodingWithNoByteOrderMark
                };
                using (XmlWriter XmlWriter = XmlWriter.Create(mem, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    ser.Serialize(XmlWriter, obj, ns);
                    return encoding.GetString(mem.ToArray());
                }
            }
        }/// <summary>
        /// 对象序列化为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ObjectToXML<T>(T obj)
        {
            return ObjectToXML<T>(obj, Encoding.UTF8);
        }
        /// <summary>
        /// xml反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T XMLToObject<T>(string source, Encoding encoding)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream Stream = new MemoryStream(encoding.GetBytes(source)))
            {
                return (T)mySerializer.Deserialize(Stream);
            }
        }
        public static T XMLToObject<T>(string source)
        {
            return XMLToObject<T>(source, Encoding.UTF8);
        }
        #endregion
        #endregion
    }
}
