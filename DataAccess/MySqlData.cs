using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections;
using MySql.Data.MySqlClient;

namespace Service
{
    public class MySqlData
    {

        public string DataBase_State = "";
        #region //打开conn
        public MySqlConnection conn;//数据库连接对象
        public string connectionString = "";
        string server, catalog, username, password;

        public MySqlData() 
        {
            ReadXml();
            conn = new MySqlConnection();
            connectionString = "server=" + server + ";User Id=" + username + ";Persist Security Info=True;password=" + password + ";database=" + catalog;
            conn.ConnectionString = connectionString;
        }

        public MySqlData(string Path)
        {
            ReadXml(Path);
            conn = new MySqlConnection();
            //connectionString = "server=192.168.40.54;User Id=root;Persist Security Info=True;password=;database=yy_db";
            connectionString = "server=" + server + ";User Id=" + username + ";Persist Security Info=True;password=" + password + ";database=" + catalog;
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
                nls = (XmlElement)root.SelectSingleNode("DataBase");
                catalog = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("UserName");
                username = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("PassWord");
                password = nls.InnerText;
                xmlDoc.Save(Path);
            }
            catch { }
        }
        public bool Open()
        {
            //conn = new SqlConnection();
            //conn.ConnectionString = connectionString;
            //conn.ConnectionString = @"server=SQLOLEDB.1;Password=123;Persist Security Info=True;User ID=sa;Initial Catalog=YY_DB;Data Source=.";
            //conn.Open();
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
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(sb.ToString(), connection))
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

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sb_Sql.ToString(), connection))
                    {

                        foreach (var item in entityPropertites)
                        {
                            if (item.GetValue(model, null) == null)
                                cmd.Parameters.Add("?"+item.Name , DBNull.Value);
                            else
                                cmd.Parameters.Add("?"+item.Name  , item.GetValue(model, null));

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
                MySqlDataAdapter da = new MySqlDataAdapter(sb_Sql.ToString(), conn);
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


        public IList<T> Select<T>(string TableName, string[] Fields, string Where) where T : new()
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
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(sb_Sql.ToString(), connection))
                    {
                        da.Fill(dt);
                        da.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                dt = null;
            }


            IList<T> List = null;
            if (dt != null)
            {
                List = ModelConvertHelper<T>.ConvertToModel(dt);
            }
            return List;
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
                sb_Sql.Append("=?");
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append(",");
            }
            sb_Sql.Remove(sb_Sql.Length - 1, 1);
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sb_Sql.ToString(), conn);
                StringBuilder sb_Param = new StringBuilder();
                for (int i = 0; i < Fields.Length; i++)
                {
                    cmd.Parameters.Add("?" + Fields[i], newValues[i]);
                }
                Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
                Close();
            }
            return Bool;

        }

        public bool Update(object obj, string KeyField)
        {
            string TableName;
            string Where = "";
            Type t = obj.GetType();
            TableName = t.Name;
            MemberInfo[] memberInfot = t.GetMembers();

            ArrayList Fields = new ArrayList();
            ArrayList Values = new ArrayList();

            PropertyInfo propertyInfo;
            object Tempobj;
            foreach (MemberInfo var in memberInfot)
            {
                if (var.MemberType == MemberTypes.Property)
                {
                    propertyInfo = t.GetProperty(var.Name);
                    Fields.Add(propertyInfo.Name);

                    Tempobj = propertyInfo.GetValue(obj, null);
                    if (Tempobj == null)
                    {
                        Tempobj = DBNull.Value;
                    }
                    Values.Add(Tempobj);

                    if (KeyField == propertyInfo.Name)
                    {
                        Where = " where " + KeyField + "='" + Tempobj + "'";
                    }
                }
            }

            #region //实体类中自增列去掉
            StringBuilder sb_Str_IsIdentity = new StringBuilder();
            sb_Str_IsIdentity.Append("Select Field=a.name ");
            sb_Str_IsIdentity.Append("FROM syscolumns a ");
            sb_Str_IsIdentity.Append("inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' ");
            sb_Str_IsIdentity.Append("where d.name='" + TableName + "' and COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 ");

            DataTable dt = new DataTable();
            new MySqlDataAdapter(sb_Str_IsIdentity.ToString(), conn).Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int Index = Fields.IndexOf(dt.Rows[0]["Field"].ToString());
                Fields.Remove(dt.Rows[0]["Field"].ToString());
                Values.Remove(Values[Index]);
            }
            #endregion

            bool Bool = true;
            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append("Update ");
            sb_Sql.Append(TableName);
            sb_Sql.Append(" set ");
            for (int i = 0; i < Fields.Count; i++)
            {
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append("=?");
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append(",");
            }
            sb_Sql.Remove(sb_Sql.Length - 1, 1);
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sb_Sql.ToString(), conn);
                StringBuilder sb_Param = new StringBuilder();
                for (int i = 0; i < Fields.Count; i++)
                {
                    cmd.Parameters.Add("?" + Fields[i], Values[i]);
                }
                Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;

        }

        public bool Update_W(object obj, string Where)
        {
            string TableName;

            Type t = obj.GetType();
            TableName = t.Name;
            MemberInfo[] memberInfot = t.GetMembers();

            ArrayList Fields = new ArrayList();
            ArrayList Values = new ArrayList();

            PropertyInfo propertyInfo;
            object Tempobj;
            foreach (MemberInfo var in memberInfot)
            {
                if (var.MemberType == MemberTypes.Property)
                {
                    propertyInfo = t.GetProperty(var.Name);
                    Fields.Add(propertyInfo.Name);

                    Tempobj = propertyInfo.GetValue(obj, null);
                    if (Tempobj == null)
                    {
                        Tempobj = DBNull.Value;
                    }
                    Values.Add(Tempobj);
                }
            }


            #region //实体类中自增列去掉
            StringBuilder sb_Str_IsIdentity = new StringBuilder();
            sb_Str_IsIdentity.Append("Select Field=a.name ");
            sb_Str_IsIdentity.Append("FROM syscolumns a ");
            sb_Str_IsIdentity.Append("inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' ");
            sb_Str_IsIdentity.Append("where d.name='" + TableName + "' and COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 ");

            DataTable dt = new DataTable();
            new MySqlDataAdapter(sb_Str_IsIdentity.ToString(), conn).Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int Index = Fields.IndexOf(dt.Rows[0]["Field"].ToString());
                Fields.Remove(dt.Rows[0]["Field"].ToString());
                Values.Remove(Values[Index]);
            }
            #endregion

            bool Bool = true;
            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append("Update ");
            sb_Sql.Append(TableName);
            sb_Sql.Append(" set ");
            for (int i = 0; i < Fields.Count; i++)
            {
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append("=?");
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append(",");
            }
            sb_Sql.Remove(sb_Sql.Length - 1, 1);
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sb_Sql.ToString(), conn);
                StringBuilder sb_Param = new StringBuilder();
                for (int i = 0; i < Fields.Count; i++)
                {
                    cmd.Parameters.Add("?" + Fields[i], Values[i]);
                }
                Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;

        }


        public bool Update<T>(string TableName, T model, string Where) where T : new()
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append(" show columns from " + TableName) ;
            sb.Append(" where Extra='auto_increment'");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(sb.ToString(), connection))
                {
                    da.Fill(dt);
                    da.Dispose();
                }
            }
            bool Bool = true;
            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append("Update ");
            sb_Sql.Append(TableName);
            sb_Sql.Append(" set ");
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
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append("=?");
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append(",");
                        }

                    }
                }


                sb_Sql.Remove(sb_Sql.Length - 1, 1);
                sb_Sql.Append(" ");
                sb_Sql.Append(Where);


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sb_Sql.ToString(), connection))
                    {
                        try
                        {

                            foreach (var item in entityPropertites)
                            {
                                if (item.GetValue(model, null) == null)
                                    cmd.Parameters.Add("?" + item.Name, DBNull.Value);
                                else
                                    cmd.Parameters.Add("?" + item.Name, item.GetValue(model, null));
                            }

                            //Open();
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            //cmd.Parameters.Clear();
                            //Close();
                        }
                        catch (Exception e)
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
        #endregion

        #region  //删除操作数据库的方法
        /// <summary>
        /// 删除操作数据库的方法
        /// </summary>
        /// <param name="sqlDel">删除操作的SQL语句</param>
        /// <returns>返回操作状态（true成功，false失败）</returns>
        public bool Delete(string sqlDel)
        {
            bool Bool = true;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlDel, connection))
                {
                    try
                    {
                        //Open();
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        //Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        SystemError.SystemLog(ex.Message);
                        Bool = false;
                    }
                }
            }

            return Bool;
        }

        public bool Delete(object obj, string KeyField)
        {
            string TableName;
            string Where = "";
            Type t = obj.GetType();
            TableName = t.Name;
            MemberInfo[] memberInfot = t.GetMembers();

            ArrayList Fields = new ArrayList();
            ArrayList Values = new ArrayList();

            PropertyInfo propertyInfo;
            object Tempobj;
            foreach (MemberInfo var in memberInfot)
            {
                if (var.MemberType == MemberTypes.Property)
                {
                    propertyInfo = t.GetProperty(var.Name);
                    Fields.Add(propertyInfo.Name);

                    Tempobj = propertyInfo.GetValue(obj, null);
                    if (Tempobj == null)
                    {
                        Tempobj = DBNull.Value;
                    }
                    Values.Add(Tempobj);

                    if (KeyField == propertyInfo.Name)
                    {
                        Where = " where " + KeyField + "='" + Tempobj + "'";
                    }
                }
            }


            bool Bool = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from " + TableName + " " + Where, conn);
                Open();
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
                Close();
            }
            return Bool;
        }

        public bool Delete_W(object obj, string Where)
        {
            string TableName;

            Type t = obj.GetType();
            TableName = t.Name;

            bool Bool = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from " + TableName + " " + Where, conn);
                Open();
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
                Close();
            }
            return Bool;
        }


        #endregion

        /// <summary>
        /// 查询操作数据库的方法（存储过程）
        /// </summary>
        /// <param name="ProcedureName">存储过程名</param>
        /// <returns>返回DataTable的集合</returns>
        public DataTable ExecProcedureSelect(string ProcedureName)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(ProcedureName, conn);
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

        /// <summary>
        /// 查询操作数据库的方法（存储过程）
        /// </summary>
        /// <param name="ProcedureName">存储过程名</param>
        /// <param name="VariableName">参数集合</param>
        /// <param name="Values">值集合</param>
        /// <returns>返回DataTable的集合</returns>
        public DataTable ExecProcedureSelect(string ProcedureName, string[] VariableName, object[] Values)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter();
                MySqlCommand cmd = new MySqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < VariableName.Length; i++)
                {
                    cmd.Parameters.Add("?" + VariableName[i], Values[i]);
                }
                da.SelectCommand = cmd;
                da.Fill(dt);//自动打开con
                da.Dispose();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                dt = null;
            }
            return dt;
        }

        #region //增、删、改操作数据库的方法（存储过程）
        /// <summary>
        /// 增、删、改操作数据库的方法（存储过程）
        /// </summary>
        /// <param name="ProcedureName">存储过程名</param>
        /// <param name="VariableName">参数集合</param>
        /// <param name="Values">值集合</param>
        /// <returns>返回操作状态（true成功，false失败）</returns>
        public bool ExecProcedure(string ProcedureName, string[] VariableName, object[] Values)
        {
            bool Bool = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand(ProcedureName, conn);
                for (int i = 0; i < VariableName.Length; i++)
                {
                    cmd.Parameters.Add("?" + VariableName[i], Values[i]);
                }

                cmd.CommandType = CommandType.StoredProcedure;

                Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Close();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;
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
                    MySqlDataAdapter da = new MySqlDataAdapter(sb_Sql.ToString(), new MySqlConnection ());//MySqlData.conn
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
