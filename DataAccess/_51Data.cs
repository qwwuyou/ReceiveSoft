using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Data;
using System.Reflection;

namespace Service
{
    public class _51Data
    {
        public string DataBase_State = "";

        #region //打开conn
        public SqlConnection conn;//数据库连接对象
        public string connectionString = "";
        string server, catalog, username, password;

        public _51Data() 
        {
            ReadXml();
            conn = new SqlConnection();
            connectionString = "server=SQLOLEDB.1;Persist Security Info=True;User ID=" + username + ";pwd=" + password + ";Initial Catalog=" + catalog + ";Data Source=" + server + ";MultipleActiveResultSets=true";
            conn.ConnectionString = connectionString; 
        }

        public _51Data(string Path)
        {
            ReadXml(Path);
            conn = new SqlConnection();
            connectionString = "server=SQLOLEDB.1;Persist Security Info=True;User ID=" + username + ";pwd=" + password + ";Initial Catalog=" + catalog + ";Data Source=" + server + ";MultipleActiveResultSets=true";
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
        public bool Insert(string TableName, string[] Fields, object[] Values)
        {
            bool Bool = true;
            StringBuilder sb_Str = new StringBuilder();
            StringBuilder sb_Field = new StringBuilder();
            StringBuilder sb_Sql = new StringBuilder();
            try
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    sb_Field.Append(Fields[i]);
                    sb_Field.Append(",");

                    sb_Str.Append("@");
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


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {
                       

                        for (int i = 0; i < Fields.Length; i++)
                        {
                            cmd.Parameters.Add("@" + Fields[i], Values[i]);
                        }

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
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

                    sb_Str.Append("@");
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


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {


                        for (int i = 0; i < Fields.Length; i++)
                        {
                            cmd.Parameters.Add("@" + Fields[i], Values[i]);
                        }

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
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
            catch (System.Data.SqlClient.SqlException ee)
            {
                SystemError.SystemLog(ee.Message);
                Bool = 0;
                if (ee.Number ==2627) { Bool = 2; }
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = 0;
            }
            return Bool;
        }

        public bool Insert<T>(string TableName, T model) where T : new()
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
                    {
                        da.Fill(dt);
                        da.Dispose();
                    }
                }
            }
            catch (Exception ex) 
            {
                SystemError.SystemLog("TTTTTTTTTTTTTTTTTT"+ex.Message);
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
                            if (dt.Rows[i]["column_name"].ToString() == item.Name)
                            { b = true; break; }
                        }
                        if (!b)
                        {
                            sb_Field.Append(item.Name);
                            sb_Field.Append(",");

                            sb_Str.Append("@");
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

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {

                        foreach (var item in entityPropertites)
                        {
                            if (item.GetValue(model, null) == null)
                                cmd.Parameters.Add("@" + item.Name, DBNull.Value);
                            else
                                cmd.Parameters.Add("@" + item.Name, item.GetValue(model, null));

                        }

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
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

        //返回第一行第一列（一般用于自增列）需要转换成int型
        public object Insert_ReturnVal(string TableName, string[] Fields, object[] Values)
        {
            object obj = null;
            StringBuilder sb_Str = new StringBuilder();
            StringBuilder sb_Field = new StringBuilder();
            StringBuilder sb_Sql = new StringBuilder();
            try
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    sb_Field.Append(Fields[i]);
                    sb_Field.Append(",");

                    sb_Str.Append("@");
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


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {

                        for (int i = 0; i < Fields.Length; i++)
                        {
                            cmd.Parameters.Add("@" + Fields[i], Values[i]);
                        }

                        try
                        {
                        connection.Open();
                        obj = cmd.ExecuteScalar();
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
            }
            return obj;
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
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sb_Sql.ToString(), connection))
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sb_Sql.ToString(), connection))
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

        /// <summary>
        /// 得到自增列的信息
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public DataTable GetIncrementColumn(string TableName)
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
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

            return dt;
        }

        /// <summary>
        /// 得到自增列列名
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public string GetIncrementColumnName(string TableName)
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
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

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Column_name"].ToString();
                }
            }
            return null;
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
                sb_Sql.Append("=@");
                sb_Sql.Append(Fields[i]);
                sb_Sql.Append(",");
            }
            sb_Sql.Remove(sb_Sql.Length - 1, 1);
            sb_Sql.Append(" ");
            sb_Sql.Append(Where);

            
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {
                        try
                        {

                            StringBuilder sb_Param = new StringBuilder();
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                cmd.Parameters.Add("@" + Fields[i], newValues[i]);
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
                            connection.Close();
                            Bool = false;
                        }
                    }
                }
            
            return Bool;

        }

        public bool Update<T>(string TableName, T model, string Where) where T : new()
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), connection))
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
                            if (dt.Rows[i]["column_name"].ToString() == item.Name)
                            { b = true; break; }
                        }
                        if (!b)
                        {
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append("=@");
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append(",");
                        }

                    }
                }


                sb_Sql.Remove(sb_Sql.Length - 1, 1);
                sb_Sql.Append(" ");
                sb_Sql.Append(Where);


                 using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), connection))
                    {
                        try
                        {

                            foreach (var item in entityPropertites)
                            {
                                if (item.GetValue(model, null) == null)
                                    cmd.Parameters.Add("@" + item.Name, DBNull.Value);
                                else
                                    cmd.Parameters.Add("@" + item.Name, item.GetValue(model, null));
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlDel, connection))
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
                SqlDataAdapter da = new SqlDataAdapter(ProcedureName, conn);
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
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < VariableName.Length; i++)
                    {
                        cmd.Parameters.Add("@" + VariableName[i], Values[i]);
                    }
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    da.Dispose();
                }
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
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                for (int i = 0; i < VariableName.Length; i++)
                {
                    cmd.Parameters.Add("@" + VariableName[i], Values[i]);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;
        }
        #endregion

        #region //DataTable转List
        public static IList<T> ConvertToModel<T>(DataTable dt) where T : new()
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
                            pi.SetValue(t, value, null);
                    }
                }

                ts.Add(t);
            }

            return ts;

        }
        #endregion

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

            public static void  SystemLog(string FieldPath,string FileName,string message)
            {
                string directory_name = FieldPath;
                string file_name = FieldPath + "/" + FileName + ".log";
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

            private static object sycObject = new object();
            public static void SystemLog(string FileName,string message)
            {
                string directory_name = System.Windows.Forms.Application.StartupPath + "/Log/Img";
                string file_name = System.Windows.Forms.Application.StartupPath + "/Log/Img/" + FileName + ".log";
                try
                {
                    lock (sycObject)
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
                        //sr.WriteLine("\n");
                        sr.WriteLine(message);
                        sr.Close();
                    }
                }
                catch { }
            }
        }
        #endregion


    }

    /// <summary>
    /// 数据层实例
    /// </summary>
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
                            pi.SetValue(t, value, null);
                    }
                }

                ts.Add(t);
            }

            return ts;

        }

        public static IList<T> Select(string TableName, string[] Fields, string Where, SqlConnection conn)
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
                using (SqlDataAdapter da = new SqlDataAdapter(sb_Sql.ToString(), conn))
                {
                    da.Fill(dt);
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }

            IList<T> List = null;
            if (dt != null)
            {
                List = ModelConvertHelper<T>.ConvertToModel(dt);
            }
            return List;
        }


        public static bool Insert(string TableName, T model, SqlConnection conn)
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");

            using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), conn))
            {
                da.Fill(dt);
                da.Dispose();
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
                            if (dt.Rows[i]["column_name"].ToString() == item.Name)
                            { b = true; break; }
                        }
                        if (!b)
                        {
                            sb_Field.Append(item.Name);
                            sb_Field.Append(",");

                            sb_Str.Append("@");
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

                SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), conn);

                foreach (var item in entityPropertites)
                {
                    cmd.Parameters.Add("@" + item.Name, item.GetValue(model, null));
                }

                //Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                //Close();
            }
            catch (Exception ex)
            {
                //SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;
        }


        public static bool Update(string TableName, T model, SqlConnection conn, string Where)
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.Append("select so.name Table_name,sc.name Column_name,");
            sb.Append("ident_current(so.name) curr_value,");
            sb.Append("ident_incr(so.name)incr_value,");   //增量
            sb.Append("ident_seed(so.name) seed_value ");  //种子
            sb.Append("from sysobjects so inner join syscolumns sc ");
            sb.Append("on so.id=sc.id and columnproperty(sc.id,sc.name,'IsIdentity')=1 where upper(so.name)=upper('" + TableName + "')");

            using (SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), conn))
            {
                da.Fill(dt);
                da.Dispose();
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
                            if (dt.Rows[i]["column_name"].ToString() == item.Name)
                            { b = true; break; }
                        }
                        if (!b)
                        {
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append("=@");
                            sb_Sql.Append(item.Name);
                            sb_Sql.Append(",");
                        }

                    }
                }


                sb_Sql.Remove(sb_Sql.Length - 1, 1);
                sb_Sql.Append(" ");
                sb_Sql.Append(Where);


                SqlCommand cmd = new SqlCommand(sb_Sql.ToString(), conn);

                foreach (var item in entityPropertites)
                {
                    cmd.Parameters.Add("@" + item.Name, item.GetValue(model, null));
                }

                //Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                //Close();
            }
            catch (Exception ex)
            {
                //SystemError.SystemLog(ex.Message);
                Bool = false;
            }
            return Bool;

        }

    }
}
