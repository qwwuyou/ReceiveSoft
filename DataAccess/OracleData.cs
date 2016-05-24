using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Xml;
using System.Data;
using System.IO;
using System.Reflection; 


namespace Service
{
    public class OracleData
    {

        public string DataBase_State = "";
        #region //打开conn
        public OracleConnection conn;//数据库连接对象
        public string connectionString = "";
        string server, catalog, username, password,port;

        public OracleData()
        {
            ReadXml();
            conn = new OracleConnection();
            connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + server + ")(PORT=" + port + "))" + "(CONNECT_DATA=(SID=" + catalog + ")));User Id=" + username + ";Password=" + password + ";";
                               //"server=" + server + ";User Id=" + username + ";Persist Security Info=True;password=" + password + ";database=" + catalog;
            conn.ConnectionString = connectionString;


        }

        public OracleData(string Path)
        {
            ReadXml(Path);
            conn = new OracleConnection();
            connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + server + ")(PORT=" + port + "))" + "(CONNECT_DATA=(SID=" + catalog + ")));User Id=" + username + ";Password=" + password + ";";
            conn.ConnectionString = connectionString;
        }

        private void ReadXml()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/System.xml");
                XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
                XmlElement nls;
                nls = (XmlElement)root.SelectSingleNode("Source");
                server = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("Port");
                port = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("DataBase");
                catalog = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("UserName");
                username = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("PassWord");
                password = nls.InnerText;
                xmlDoc.Save(System.Windows.Forms.Application.StartupPath + "/System.xml");
            }
            catch { }
        }

        private void ReadXml(string Path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path);
                XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
                XmlElement nls;
                nls = (XmlElement)root.SelectSingleNode("Source");
                server = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("Port");
                port = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("DataBase");
                catalog = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("UserName");
                username = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("PassWord");
                password = nls.InnerText;
                xmlDoc.Save(Path);
            }
            catch  { }
        }
        public bool Open()
        {
            
            DataBase_State = "数据库打开成功！[" + DateTime.Now.ToString() + "]\r\n";
            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                    DataBase_State = "数据库打开成功！[" + DateTime.Now.ToString() + "]\r\n";
                    return true;
                }
                catch (Exception ex)
                {
                    SystemError.SystemLog(ex.Message);
                    DataBase_State = "数据库打开失败！[" + DateTime.Now.ToString() + "]\r\n";
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region//关闭conn
        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    SystemError.SystemLog(ex.Message);
                }
            }
        }
        #endregion

        #region //插入数据
        public bool Insert<T>(string TableName, T model) where T : new()
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append(" show columns from " + TableName);
            sb.Append(" where Extra='auto_increment'");

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleDataAdapter da = new OracleDataAdapter(sb.ToString(), connection))
                    {
                        da.Fill(dt);
                        da.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                SystemError.SystemLog("TTTTTTTTTTTTTTTTTT" + ex.Message);
            }

            bool Bool = true;
            StringBuilder sb_Str = new StringBuilder();
            StringBuilder sb_Field = new StringBuilder();
            StringBuilder sb_Sql = new StringBuilder();
            System.Reflection.PropertyInfo[] entityPropertites = typeof(T).GetProperties();
            try
            {
                bool b = false;
                foreach (var item in entityPropertites)
                {
                    b = false;
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["Field"].ToString() == item.Name)
                            { b = true; break; }
                        }
                        if (!b)
                        {
                            sb_Field.Append(item.Name);
                            sb_Field.Append(",");

                            sb_Str.Append("?");
                            sb_Str.Append(item.Name);
                            sb_Str.Append(",");
                        }

                    }
                }


                sb_Field.Remove(sb_Field.Length - 1, 1);
                sb_Str.Remove(sb_Str.Length - 1, 1);

                sb_Sql.Append("insert into ");
                sb_Sql.Append(TableName);
                sb_Sql.Append("(");
                sb_Sql.Append(sb_Field);
                sb_Sql.Append(") values(");
                sb_Sql.Append(sb_Str);
                sb_Sql.Append(")");

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand(sb_Sql.ToString(), connection))
                    {

                        foreach (var item in entityPropertites)
                        {
                            if (item.GetValue(model, null) == null)
                                cmd.Parameters.Add("?" + item.Name, DBNull.Value);
                            else
                                cmd.Parameters.Add("?" + item.Name, item.GetValue(model, null));

                        }

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            connection.Close();
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            connection.Close();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;
        }


        public int Insert_1(string TableName, string[] Fields, object[] Values)
        {
            int Bool = 1;
            StringBuilder sb_Str = new StringBuilder();
            StringBuilder sb_Field = new StringBuilder();
            StringBuilder sb_Sql = new StringBuilder();
            try
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    sb_Field.Append(Fields[i]);
                    sb_Field.Append(",");

                    sb_Str.Append(":");
                    sb_Str.Append(Fields[i]);
                    sb_Str.Append(",");
                }
                sb_Field.Remove(sb_Field.Length - 1, 1);
                sb_Str.Remove(sb_Str.Length - 1, 1);

                sb_Sql.Append("insert into ");
                sb_Sql.Append(TableName);
                sb_Sql.Append("(");
                sb_Sql.Append(sb_Field);
                sb_Sql.Append(") values(");
                sb_Sql.Append(sb_Str);
                sb_Sql.Append(")");


                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand(sb_Sql.ToString(), connection))
                    {


                        for (int i = 0; i < Fields.Length; i++)
                        {
                            cmd.Parameters.Add(":" + Fields[i], Values[i]);
                        }

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Oracle.DataAccess.Client.OracleException e)
                        {
                            SystemError.SystemLog(e.Message + "执行插入操作");
                            connection.Close();
                            throw e;
                        }
                    }
                }
            }
            catch (Oracle.DataAccess.Client.OracleException ee)
            {
                SystemError.SystemLog(ee.Message +"主键");
                Bool = 0;
                if (ee.ErrorCode == -2147467259) { Bool = 2; }
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message + "插入");
                Bool = 0;
            }
            return Bool;
        }
        #endregion

        #region //更新数据
        public bool Update(string TableName, string[] Fields, object[] newValues, string Where)
        {
            bool Bool = true;
            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append("Update ");
            sb_Sql.Append(TableName);
            sb_Sql.Append(" set ");
            for (int i = 0; i < Fields.Length; i++)
            {
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append("=:");
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append(",");
            }
            sb_Sql.Remove(sb_Sql.Length - 1, 1);
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);


            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sb_Sql.ToString(), connection))
                {
                    SystemError.SystemLog(sb_Sql.ToString());
                    try
                    {

                        StringBuilder sb_Param = new StringBuilder();
                        for (int i = 0; i < Fields.Length; i++)
                        {
                            cmd.Parameters.Add(":" + Fields[i], newValues[i]);
                        }
                        //Open();
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        //cmd.Parameters.Clear();
                        //Close();
                    }
                    catch (Exception ex)
                    {
                        SystemError.SystemLog(ex.Message + "更新");
                        connection.Close();
                        Bool = false;
                    }
                }
            }

            return Bool;

        }
        #endregion

        #region //查询数据
        public DataTable Select(string TableName, string[] Fields, string Where)
        {
            DataTable dt = new DataTable();
            StringBuilder sb_Sql = new StringBuilder();
            StringBuilder sb_field = new StringBuilder();
            for (int i = 0; i < Fields.Length; i++)
            {
                sb_field.Append(Fields[i]);
                sb_field.Append(",");
            }
            sb_field.Remove(sb_field.Length - 1, 1);
            sb_Sql.Append("select ");
            sb_Sql.Append(sb_field.ToString());
            sb_Sql.Append(" from ");
            sb_Sql.Append(TableName);
            sb_Sql.Append("  ");
            sb_Sql.Append(Where);

            try
            {
                OracleDataAdapter da = new OracleDataAdapter(sb_Sql.ToString(), conn);
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                dt = null;
            }

            return dt;
        }
        #endregion

        /// <summary>
        /// 实体转换辅助类
        /// 调用方法 IList<T> Name = ModelConvertHelper<T>.ConvertToModel(DataTable);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ModelConvertHelper<T> where T : new()
        {
            public static IList<T> ConvertToModel(DataTable dt)
            {
                // 定义集合
                IList<T> ts = new List<T>();

                // 获得此模型的类型
                Type type = typeof(T);

                string tempName = "";

                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();

                    // 获得此模型的公共属性
                    PropertyInfo[] propertys = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;

                        // 检查DataTable是否包含此列
                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter
                            if (!pi.CanWrite) continue;

                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                //mysql差异
                                if (pi.PropertyType.Name == "Boolean")
                                {
                                    int k = 0;
                                    if (int.TryParse(value.ToString(), out k))
                                    {
                                        if (k == 1)
                                            pi.SetValue(t, true, null);
                                        else if (k == 0)
                                            pi.SetValue(t, false, null);
                                    }
                                }
                                else
                                {
                                    pi.SetValue(t, value, null);
                                }
                            }
                        }
                    }

                    ts.Add(t);
                }

                return ts;

            }

            public static IList<T> Select(string TableName, string[] Fields, string Where)
            {
                DataTable dt = new DataTable();
                StringBuilder sb_Sql = new StringBuilder();
                StringBuilder sb_field = new StringBuilder();
                for (int i = 0; i < Fields.Length; i++)
                {
                    sb_field.Append(Fields[i]);
                    sb_field.Append(",");
                }
                sb_field.Remove(sb_field.Length - 1, 1);
                sb_Sql.Append("select ");
                sb_Sql.Append(sb_field.ToString());
                sb_Sql.Append(" from ");
                sb_Sql.Append(TableName);
                sb_Sql.Append(" ");
                sb_Sql.Append(Where);

                try
                {
                    OracleDataAdapter da = new OracleDataAdapter(sb_Sql.ToString(), new OracleConnection());//MySqlData.conn
                    da.Fill(dt);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    SystemError.SystemLog(ex.Message);
                    dt = null;
                }
                return ModelConvertHelper<T>.ConvertToModel(dt);
            }
        }

        #region//错误日志处理类
        public class SystemError
        {
            public static void SystemLog(string message)
            {
                string directory_name = System.Windows.Forms.Application.StartupPath + "/Log";
                string file_name = System.Windows.Forms.Application.StartupPath + "/Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                try
                {
                    if (!Directory.Exists(directory_name))
                    {
                        Directory.CreateDirectory(directory_name);
                    }
                    if (!File.Exists(file_name))
                    {
                        FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        fs.Close();
                    }
                    StreamWriter sr = File.AppendText(file_name);
                    sr.WriteLine("\n");
                    sr.WriteLine("[" + DateTime.Now.ToString() + "]\n" + message);
                    sr.Close();
                }
                catch { }
            }
        }
        #endregion
    }
}
