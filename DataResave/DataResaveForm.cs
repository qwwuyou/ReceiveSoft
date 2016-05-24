using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
namespace DataResave
{
    public partial class DataResaveForm : Form
    {
        public DataResaveForm()
        {
            InitializeComponent();
        }

        private void DataResaveForm_Load(object sender, EventArgs e)
        {
            comboBox_DBType.SelectedIndex = 0;
            comboBox_Minute.SelectedItem = ReadMinuteXML();
            comboBox_Item.SelectedIndex = 0;

            string datetime = ReadDateTimeXML();
            DateTime Dt=new DateTime();
            if(DateTime.TryParse(datetime, out Dt)){
                dateTimePicker_B.Value = Dt;
            }

            textBox_Val.Enabled  = false ;
            DB_Init();
            dGVlist = xmlToList<DataResaveClass>(ReadFieldToFieldXML());
            if (dGVlist != null && dGVlist.Count > 0)
            {
                dataGridView_List.DataSource = dGVlist;
            }
            else { dataGridView_List.DataSource = null; }
            comboBox_items_Init();
        }

        private void DB_Init()
        {
            string type, server, catalog, username, password;
            try
            {
                ReadDBXML(out type, out server, out catalog, out username, out password);
                textBox_Source.Text = server;
                textBox_DataBase.Text = catalog;
                textBox_UserName.Text = username;
                textBox_PassWord.Text = password;
                if (type.ToLower() == "mssql")
                {
                    comboBox_DBType.SelectedIndex = 0;
                }
                //else if (type.ToLower() == "mysql")
                //{ comboBox_DBType.SelectedIndex = 1; }
            }
            catch { }
        }


        #region 读写XML
        //读写转存库配置
        public void ReadDBXML(out string type, out string server, out string catalog, out string username, out string password)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

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
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

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
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("FieldToField");
            return root.InnerText;
        }
        public void WriteFieldToFieldXML(string xml)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("FieldToField");
            root.InnerText = xml;
            xmlDoc.Save(path);
        }

        //读写执行时间
        public string ReadDateTimeXML()
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DateTime");
            return root.InnerText;
        }
        public void WriteDateTimeXML(DateTime datetime)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DateTime");
            root.InnerText = datetime.ToString("yyyy-MM-dd HH:mm:ss");
            xmlDoc.Save(path);
        }

        //读写执行时间间隔
        public string ReadMinuteXML()
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Minute");
            return root.InnerText;
        }
        public void WriteMinuteXML(int Minute)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("Minute");
            root.InnerText = Minute.ToString();
            xmlDoc.Save(path);
        }
        #endregion

        //测试成功，表List初始化
        private bool listBox_Table_Init() 
        {
            listBox_Table.Items.Clear();
            PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";
            PublicBD.DB = "MSSQL";
            PublicBD.ReInit();
            _51Data dt = PublicBD.db.dt as _51Data;
            DataTable  datatable= dt.Select("sys.indexes a , sys.objects b , sys.dm_db_partition_stats c",new string []{"b.name as tablename ,c.row_count as datacount"},
                                                "where a.[object_id] = b.[object_id] AND b.[object_id] = c.[object_id] AND a.index_id = c.index_id AND a.index_id < 2 AND b.is_ms_shipped = 0");

            if (datatable != null && datatable.Rows.Count > 0)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    listBox_Table.Items.Add(datatable.Rows[i][0].ToString());
                }
                return true;
            }
            else { return false; }
        }

        //根据表名的到该表字段名
        private void listBox_Field_Init(string TableName) 
        {
             PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";
            PublicBD.DB = "MSSQL";
            PublicBD.ReInit();
            _51Data dt = PublicBD.db.dt as _51Data;
            DataTable  datatable= dt.Select("SysColumns",new string []{"Name"},"Where id=Object_Id('"+TableName+"')");


            if(datatable !=null&& datatable.Rows.Count>0)
            for (int i = 0; i < datatable.Rows.Count ; i++)
            {
                listBox_Field.Items.Add(datatable.Rows[i][0]);
            }
        }

        /// <summary>
        /// 监测元素List初始化
        /// </summary>
        private void comboBox_items_Init() 
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(" where ItemCode!='-1' and ItemCode!='0000000000'");
            if (ItemList != null && ItemList.Count > 0)
            {
                comboBox_items.DataSource = ItemList;
                comboBox_items.DisplayMember = "ItemName";
                comboBox_items.ValueMember = "ItemID";
                comboBox_items.SelectedIndex = 0;
            }
            comboBox_items.Enabled = false;
        }
        private void comboBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Item.SelectedItem.ToString() == "元    素") 
            {
                comboBox_items.Enabled = true;
                textBox_Val.Enabled = false;
            }
            else if (comboBox_Item.SelectedItem.ToString() == "其    他")
            {
                textBox_Val.Enabled = true;
                comboBox_items.Enabled = false;
            }
            else 
            { 
                comboBox_items.Enabled = false;
                textBox_Val.Enabled = false;
            }
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            string Msg = DBValidate();
            if (Msg != "")
            {
                MessageBox.Show(Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WriteDBXML("MSSQL", textBox_Source.Text.Trim(), textBox_DataBase.Text.Trim(), textBox_UserName.Text.Trim(), textBox_PassWord.Text.Trim());
                bool b=listBox_Table_Init();

                if (b)
                {
                    string Type = string.Empty;
                    if (comboBox_DBType.SelectedIndex == 0)
                    { Type = "MSSQL"; }
                   
                    MessageBox.Show("数据库信息配置成功!");
                }
                else { MessageBox.Show("配置信息有误，测试连接失败!"); }
            }
            
        }

        private void listBox_Table_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_Field.Items.Clear();
            listBox_Field_Init(listBox_Table.SelectedItem.ToString());
        }


        string tablename = "";
        List<DataResaveClass> dGVlist = new List<DataResaveClass>();
        private void button_SetField_Click(object sender, EventArgs e)
        {
            if (listBox_Table.SelectedIndex == -1)
            {
                MessageBox.Show("请检查是否已经连接数据库!");
                return;
            }

            if (tablename == "")
            {
                tablename = listBox_Table.SelectedItem.ToString();
            }
            else 
            {
                if (tablename != listBox_Table.SelectedItem.ToString())
                {
                    MessageBox.Show("请将表" + tablename + "配置完成!");
                    return;
                }
            }

            if (listBox_Field.SelectedIndex == -1)
            {
                MessageBox.Show("请选择转存字段!");
            }
            else
            {
                string setlist ="";
                string fidld = listBox_Field.SelectedItem.ToString();
                if (comboBox_Item.SelectedItem.ToString() == "元    素")
                {
                    setlist = comboBox_Item.SelectedItem.ToString() + "(" + comboBox_items .SelectedValue+ ")<--->" + fidld + "\r\n";
                }
                else if (comboBox_Item.SelectedItem.ToString() == "其    他")
                {
                    setlist = comboBox_Item.SelectedItem.ToString() + "(" + textBox_Val.Text .Trim()+ ")<--->" + fidld + "\r\n";
                }
                else
                {
                    setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "\r\n";
                }
                
                if (textBox_Text.Text.Contains(fidld))
                { 
                    MessageBox.Show("该字段已经配置!");
                }
                else
                { 
                    textBox_Text.Text += setlist; 
                }
            }
        }

        private void button_Right_Click(object sender, EventArgs e)
        {
            if (textBox_Text.Text.Trim() == "")
            {
                MessageBox.Show("没有配置任何转存字段!");
            }
            else
            {
                List<DataResaveClass> list = dGVlist;
                
                 if (list == null)
                 {
                     dGVlist = Format();
                     dataGridView_List.DataSource = null;
                     if (dGVlist != null && dGVlist.Count > 0)
                     {
                         dataGridView_List.DataSource = dGVlist;
                     }
                     else { dataGridView_List.DataSource = null; }
                     tablename = "";
                     textBox_Text.Text = "";
                     return; 
                 }


                var query =from l in list where l.TableName ==tablename select l;
                if (query.Count() > 0)
                {
                   if(MessageBox.Show("该表配置信息已存在，是否更新？", "提示", MessageBoxButtons.YesNo)==DialogResult.Yes)
                   {
                       var q = from l in list where l.TableName != tablename select l;
                       List<DataResaveClass> ss = q.ToList<DataResaveClass>() ;
                       dGVlist = ss;
                       dataGridView_List.DataSource = null;
                       if (dGVlist != null && dGVlist.Count > 0)
                       {
                           dataGridView_List.DataSource = dGVlist;
                       }
                       else { dataGridView_List.DataSource = null; }
                       dGVlist = Format();
                       dataGridView_List.DataSource = null;
                       if (dGVlist != null && dGVlist.Count > 0)
                       {
                           dataGridView_List.DataSource = dGVlist;
                       }
                       else { dataGridView_List.DataSource = null; }
                       tablename = "";
                       textBox_Text.Text = "";
                   }
                    
                }
                else 
                {
                    dGVlist = Format();
                    dataGridView_List.DataSource = null;
                    if (dGVlist != null && dGVlist.Count > 0)
                    {
                        dataGridView_List.DataSource = dGVlist;
                    }
                    else { dataGridView_List.DataSource = null; }
                    
                    tablename = "";
                    textBox_Text.Text = "";
                }

                
            }
        }

        private List<DataResaveClass> Format() 
        {
            List<DataResaveClass> list; 
            if(dataGridView_List.DataSource==null)
            {
                list=new List<DataResaveClass>();
            }else
            {
                list = dGVlist;
            }

            string str=textBox_Text.Text.Trim();
            string[] strs=str.Split(new string[] { "\r\n" },StringSplitOptions.None );
            if (strs.Length>0)
                for (int i = 0; i < strs.Length; i++)
            {
                DataResaveClass drc = new DataResaveClass();
                if (strs[i] != "") 
                {
                    string[] str_s = strs[i].Split(new string[] { "<--->" }, StringSplitOptions.None);
                    if (str_s[0].Contains("元    素") || str_s[0].Contains("其    他"))
                    {
                        int m = str_s[0].IndexOf('(');
                        int n = str_s[0].IndexOf(')');
                        drc.OldField = str_s[0].Substring(0,m);
                        drc.Value=str_s[0].Substring(m+1, n - m-1);

                        if (str_s.Length == 3) 
                        {
                            drc.Where = "条件";
                        }
                    }
                    else
                    {
                        drc.OldField = str_s[0];
                        drc.Value = "NULL";
                    }

                    drc.TableName = tablename;
                    drc.NewField = str_s[1];
                    list.Add(drc);
                }
            }
            
            return list;
        }

        private string ReFormat() 
        {
            
            string str = "";
            List<DataResaveClass> list = dGVlist;

            var query = from l in list where l.TableName == tablename select l;
            foreach (var item in query)
            {
                if (item.Value == "NULL")
                { str += item.OldField + "<--->" + item.NewField + "\r\n"; }
                else
                {
                    if (item.Where == "条件") 
                    {
                        str += item.OldField + "(" + item.Value + ")<--->" + item.NewField + "<--->条件\r\n";
                    }
                    else
                    {
                        str += item.OldField + "(" + item.Value + ")<--->" + item.NewField + "\r\n";
                    }
                }
            }

            var q = from l in list where l.TableName != tablename select l;
            dGVlist = q.ToList<DataResaveClass>();

            if (dGVlist != null && dGVlist.Count > 0)
            {
                dataGridView_List.DataSource = dGVlist;
            }
            else { dataGridView_List.DataSource = null; }
            return str;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            tablename = "";
            textBox_Text.Text  = "";
        }


        private string DBValidate()
        {
            string Msg = "";
            if (textBox_Source.Text.Trim() == "")
            {
                Msg = "请填写服务器名称！" + "\n";
            }
            if (textBox_DataBase.Text.Trim() == "")
            {
                Msg += "请填写数据库名称！" + "\n";
            }
            if (textBox_UserName.Text.Trim() == "")
            {
                Msg += "请填写用户名！" + "\n";
            }

            return Msg;
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
        return list;                    }                }                else                {                    //如果是自定义类型就需要反序列化了                    System.Xml.XmlNodeList nl = doc.SelectNodes("/root/items/" + typeof(T).Name);                    if (nl.Count == 0)                    {                        return list;                    }                    else                    {                        foreach (System.Xml.XmlNode node in nl)                        {                            list.Add(XMLToObject<T>(node.OuterXml));                        }                        return list;                    }                }            }            catch (XmlException ex)            {                throw new ArgumentException("不是有效的XML字符串", "xml");            }            catch (InvalidCastException e)            {                throw new ArgumentException("指定的数据类型不匹配", "T");            }            catch (Exception exx)            {                throw exx;            }        }

        /// <summary>        /// 将List转化为xml字符串        /// </summary>        /// <typeparam name="T">类型，仅仅支持int/long/datetime/string/double/decimal/object</typeparam>        /// <param name="list"></param>        /// <returns></returns>        /// <remarks></remarks>        public static string listToXml<T>(List<T> list)        {            Type tp = typeof(T);            string xml = "<root>";            if (tp == typeof(string) | tp == typeof(int) | tp == typeof(long) | tp == typeof(DateTime) | tp == typeof(double) | tp == typeof(decimal))            {                foreach (T obj in list)                {                    xml = xml + "<item>" + obj.ToString() + "</item>";                }            }            else            {                xml = xml + "<items>";                foreach (T obj in list)                {                    xml = xml + ObjectToXML<T>(obj);                }                xml = xml + "</items>";            }            xml = xml + "</root>";            return xml;        }

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
        }/// <summary>        /// 对象序列化为xml        /// </summary>        /// <typeparam name="T"></typeparam>        /// <param name="obj"></param>        /// <returns></returns>        /// <remarks></remarks>        public static string ObjectToXML<T>(T obj)        {            return ObjectToXML<T>(obj, Encoding.UTF8);        }        /// <summary>        /// xml反序列化为对象        /// </summary>        /// <typeparam name="T"></typeparam>        /// <param name="source"></param>        /// <param name="encoding"></param>        /// <returns></returns>        /// <remarks></remarks>        public static T XMLToObject<T>(string source, Encoding encoding)        {            XmlSerializer mySerializer = new XmlSerializer(typeof(T));            using (MemoryStream Stream = new MemoryStream(encoding.GetBytes(source)))            {                return (T)mySerializer.Deserialize(Stream);            }        }        public static T XMLToObject<T>(string source)        {            return XMLToObject<T>(source, Encoding.UTF8);        }
        #endregion

        private void button_OK_Click(object sender, EventArgs e)
        {
            string xml = listToXml<DataResaveClass>(dataGridView_List.DataSource as List<DataResaveClass>);

            WriteFieldToFieldXML(xml);
            WriteDateTimeXML(dateTimePicker_B.Value);
            WriteMinuteXML(int.Parse(comboBox_Minute.SelectedItem.ToString()));
        }
        private void button_ClearXML_Click(object sender, EventArgs e)
        {
            WriteFieldToFieldXML("");
            dataGridView_List.DataSource = xmlToList<DataResaveClass>(ReadFieldToFieldXML());
        }
       
        
        Thread thread = null;
        private void button_Start_Click(object sender, EventArgs e)
        {
            if (button_Start.Text == "启动")
            {
                groupBox3.Visible = false;
                groupBox2.Visible = false;

                button_Start.Text = "关闭";
                ThreadState = true;
                if (thread == null)
                {
                    thread = new Thread(ThreadSatrt);
                    thread.Start();
                }
            }
            else
            {
                groupBox3.Visible = true;
                groupBox2.Visible = true;

                thread.Abort();
                thread = null;
                ThreadState = false;
                button_Start.Text = "启动";
            }
        }
        
        bool ThreadState = false;
        public void ThreadSatrt() 
        {
            int M =int.Parse( ReadMinuteXML());
            DBtoDB();
            WriteDateTimeXML(DateTime.Now );
            while (ThreadState) 
            {

                DBtoDB();
                WriteDateTimeXML(DateTime.Now);
                Thread.Sleep(M * 60*1000);

                zxsj = DateTime.Now.ToString();
            }
        }

        string zxsj = "";
        int Success = 0;
        int Failure = 0;
        int Key = 0;
        private void DBtoDB()
        {
            #region 读取转存字段配置信息
            List<DataResaveClass> list=xmlToList<DataResaveClass>(ReadFieldToFieldXML());
            List<string> tables = new List<string>();
            foreach (var item in list)
            {
                tables.Add(item.TableName);
            }
            tables = tables.Distinct().ToList();
            #endregion
           

            List<string> fields ;
            List<object>  values ;
            List<object> values_1;
            string whereAnd = "";
            foreach (var table in tables)
            {
                var l = from L in list where table == L.TableName select L;
                fields=new List<string>();
                values =new List<object>();
                values_1 = new List<object>();
                foreach (var item in l)
                {
                    fields.Add(item.NewField);

                    if (item.OldField == "元    素" || item.OldField == "其    他")
                    {
                        values.Add (item.Value);
                        values_1.Add(item.Value);
                        bool b = true;
                        if (item.OldField == "元    素") 
                        {
                            if (b)
                            {
                                whereAnd += " and ItemID='" + item.Value + "'";
                                b = false;
                            }
                            else 
                            {
                                whereAnd += " or ItemID='" + item.Value + "'";
                            }
                        }
                    }
                    else if (item.OldField == "站    号")
                    {
                        values.Add ("[0]");
                        values_1.Add("[0]");
                    }
                    else if (item.OldField == "监测时间")
                    { 
                        values.Add ("[1]");
                        values_1.Add("[1]");
                    }
                    else if (item.OldField == "接收时间") 
                    {
                        values.Add("[2]");
                        values_1.Add("[2]");
                    }
                    else if (item.OldField == "值")
                    {
                        values.Add("[3]");
                        values_1.Add("[3]");
                    }

                }
                #region 读取原始数据库中的数据
                string datetime =ReadDateTimeXML();
                datetime = DateTime.Parse(datetime).AddSeconds(-60).ToString("yyyy-MM-dd HH:mm:ss");
                PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
                PublicBD.DB = "MSSQL";
                PublicBD.ReInit();
                _51Data dt = PublicBD.db.dt as _51Data;


                DataTable datatable = dt.Select("YY_DATA_AUTO", new string[] { "*" }, "where DOWNDATE>='" + datetime + "'" + whereAnd);
                #endregion

                if (datatable != null && datatable.Rows.Count > 0)
                {

                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        for (int j = 0; j < values.Count; j++)
                        {
                            if (values_1[j].ToString() == "[0]")
                            {
                                values[j] = datatable.Rows[i]["STCD"].ToString();
                            }
                            else if (values_1[j].ToString() == "[1]")
                            { values[j] = datatable.Rows[i]["TM"].ToString(); }
                            else if (values_1[j].ToString() == "[2]")
                            { values[j] = datatable.Rows[i]["DOWNDATE"].ToString(); }
                            else if (values_1[j].ToString() == "[3]")
                            { values[j] = datatable.Rows[i]["DATAVALUE"].ToString(); }
                        }

                        //写入转存库，并记录成功失败数量
                        PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave/System.xml";
                        PublicBD.DB = "MSSQL";
                        PublicBD.ReInit();
                        _51Data newdt = PublicBD.db.dt as _51Data;

                        int B = newdt.Insert_1(table, fields.ToArray(), values.ToArray());
                        if (B==1)
                        { Success++; }
                        else if (B == 2) 
                        {
                            Key++;
                        }
                        else
                        {
                            Failure++;
                        }
                    }


                }
              

            }


           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label12.Text = Success + " 成功， " + Failure + " 失败，主键冲突"+Key +"。";
            this.Text ="此次执行时间:"+zxsj;

        }

        private void DataResaveForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button_Left_Click(object sender, EventArgs e)
        {
            if (dataGridView_List.SelectedCells.Count == 0) { return; }   
            if (dataGridView_List.SelectedCells[0].ColumnIndex != 0)
            {
                MessageBox.Show("请在列表第一列中选择表名！");

            }
            else { 
                tablename = dataGridView_List.SelectedCells[0].Value.ToString();
                textBox_Text.Text = ReFormat();
            }
        }

    }


   



    public partial class DataResaveClass
    {
        public DataResaveClass()
        { }
        #region Model
        private string _tablename;
        private string _oldfield;
        private string _value;
        private string _newfield;
        private string _where;
        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OldField
        {
            set { _oldfield = value; }
            get { return _oldfield; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewField
        {
            set { _newfield = value; }
            get { return _newfield; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Where
        {
            set { _where = value; }
            get { return _where; }
        }
        #endregion Model

    }
}
