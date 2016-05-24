using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

namespace Service
{

    public static class PublicBD
    {
        public static DBBussiness db = null;
        public static string Path = "";
        public static string DB = "";

        static PublicBD()
        {
            DB = ReadDB().ToUpper();
            if (DB == "MSSQL")
                db = new SqlDBBussiness();
            else if (DB == "ORACLE")
                db = new OracleDBBussiness();
            else
                db = new MySqlDBBussiness();

            Reflection_Resave R_Resave = new Reflection_Resave();
        }

        public static void  RePublicBD()
        {
            DB = ReadDB().ToUpper();
            if (DB == "MSSQL")
                db = new SqlDBBussiness();
            else if (DB == "ORACLE") 
                db = new OracleDBBussiness();
            else
                db = new MySqlDBBussiness();
        }

        //重新实例化DBBussiness对象，为了改变读取不同的配置文件
        public static void ReInit()
        {
            DB = ReadDB(Path).ToUpper();
            if (DB == "MSSQL")
                db = new SqlDBBussiness(Path); 
            else if (DB == "ORACLE") 
                db = new OracleDBBussiness(Path); 
            else
                db = new MySqlDBBussiness(Path);
        }


        public static bool ConnectState
        {
            get
            {
                if (DB == "MSSQL")
                {
                    (db.dt as _51Data ).Open();
                    if ((db.dt as _51Data).conn.State == ConnectionState.Open)
                    {
                        (db.dt as _51Data).Close();
                        return true;
                    }
                    else
                        return false;
                }
                else if (DB == "ORACLE")
                {

                    (db.dt as OracleData).Open();
                    if ((db.dt as OracleData).conn.State == ConnectionState.Open)
                    {
                        (db.dt as OracleData).Close();
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    (db.dt as MySqlData).Open();
                    if ((db.dt as MySqlData).conn.State == ConnectionState.Open)
                    {
                        (db.dt as MySqlData).Close();
                        return true;
                    }
                    else
                        return false;
                }
            }
        }


        /// <summary>
        ///读取数据库类型 MSSQL或MYSQL
        /// </summary>
        /// <returns></returns>
        public static string ReadDB()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)root.SelectSingleNode("Type");
            return nls.InnerText;
        }
        public static string ReadDB(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
            XmlElement nls;
            nls = (XmlElement)root.SelectSingleNode("Type");
            return nls.InnerText;
        }
    }
}
