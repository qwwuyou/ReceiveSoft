using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;
using System.Xml;

namespace Service
{
    public class Reflection_Resave
    {
        static bool state = true;
        public Reflection_Resave()
        {
            try
            {
                string File = "", Class = "";
                ReadResaveDllXML(out File, out Class);
                A = System.Reflection.Assembly.LoadFrom(System.Windows.Forms.Application.StartupPath + "/" + File);
                T = A.GetType(Class);
                O = Activator.CreateInstance(T);
            }
            catch { state = false; }
        }

        /// <summary>
        /// 数据处理接口（根据不同协议）
        /// </summary>
        static System.Reflection.Assembly A = null;
        static System.Type T = null;
        static object O = null;

        public static void InitInfo()
        {
            if (state)
            {
                System.Reflection.MethodInfo mi = T.GetMethod("InitInfo");
                mi.Invoke(O, new object[] { });
            }
        }

        public static void Resave(YY_DATA_AUTO model)
        {
            if (state)
            {
                System.Reflection.MethodInfo mi = T.GetMethod("Resave", new Type[] { typeof(YY_DATA_AUTO) });
                mi.Invoke(O, new object[] { model });
            }
        }
        /// <summary>
        /// 读取数据转存处理反射用的dll
        /// </summary>
        /// <param name="File">文件名(带后缀)</param>
        /// <param name="Class">类名(带命名空间)</param>
        private static void ReadResaveDllXML(out string File, out string Class)
        {
            XmlDocument xmlDoc = new XmlDocument();
            
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("Resave");
            File = node.Attributes["file"].Value;
            Class = node.Attributes["class"].Value;
            xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
        }
    }
}
