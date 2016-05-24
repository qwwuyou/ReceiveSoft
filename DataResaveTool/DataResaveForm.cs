using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Service;
using System.Threading;

namespace DataResaveTool
{
    public partial class DataResaveForm : Form
    {
        public DataResaveForm()
        {
            InitializeComponent();
        }

        XMLOperation xml = new XMLOperation();
        private void DataResaveForm_Load(object sender, EventArgs e)
        {
            comboBox_DBType.SelectedIndex = 0;
            comboBox_Item.SelectedIndex = 0;
            comboBox_Decimal.SelectedIndex = 0;
            comboBox_Minute.SelectedItem = xml.ReadMinuteXML();

            bool RainCheckedState;
            bool StageCheckedState;
            xml.ReadRainStage(out RainCheckedState, out StageCheckedState);
            checkBox_Rain.Checked = RainCheckedState;
            checkBox_Stage.Checked = StageCheckedState;
           


            dateTimePicker_Init();

            textBox_Val.Enabled = false;

            DB_Init();
            dataGridView_Init();
            comboBox_items_Init();
        }

        //转存数据库配置控件初始化
        private void DB_Init()
        {
            string type, server, catalog, username, password;
            try
            {
                xml.ReadDBXML(out type, out server, out catalog, out username, out password);
                textBox_Source.Text = server;
                textBox_DataBase.Text = catalog;
                textBox_UserName.Text = username;
                textBox_PassWord.Text = password;
                if (type.ToLower() == "mssql")
                {
                    comboBox_DBType.SelectedIndex = 0;
                }
                else { comboBox_DBType.SelectedIndex = 1; }
                //else if (type.ToLower() == "mysql")
                //{ comboBox_DBType.SelectedIndex = 1; }
            }
            catch { }
        }

        //时间控件初始化
        private void dateTimePicker_Init()
        {
            string datetime = xml.ReadDateTimeXML();
            DateTime Dt = new DateTime();
            if (DateTime.TryParse(datetime, out Dt))
            {
                dateTimePicker_B.Value = Dt;
            }
        }


        //监测元素List初始化
        private void comboBox_items_Init()
        {
            PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
            PublicBD.DB = "MSSQL";
            PublicBD.ReInit();
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

        //Datagrid初始化
        List<DataResaveClass> dGVlist = new List<DataResaveClass>();
        private void dataGridView_Init()
        {
            dGVlist = XMLOperation.xmlToList<DataResaveClass>(xml.ReadFieldToFieldXML());
            if (dGVlist != null && dGVlist.Count > 0)
            {
                dataGridView_List.DataSource = dGVlist;
                SetdataGridViewHeader();
            }
            else { dataGridView_List.DataSource = null; }
        }
        private void SetdataGridViewHeader()
        {
            if (dataGridView_List.DataSource != null)
            {
                dataGridView_List.Columns[0].HeaderText = "编号";
                dataGridView_List.Columns[1].HeaderText = "表名";
                dataGridView_List.Columns[2].HeaderText = "原字段";
                dataGridView_List.Columns[3].HeaderText = "值";
                dataGridView_List.Columns[4].HeaderText = "新字段";
                dataGridView_List.Columns[5].HeaderText = "条件";
            }
        }


        //测试成功，表List初始化
        private bool listBox_Table_Init(string DBType)
        {
            listBox_Table.Items.Clear();
            PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";

            if (DBType == "MSSQL")
            {
                PublicBD.DB = "MSSQL";
                PublicBD.ReInit();
                _51Data dt = PublicBD.db.dt as _51Data;

                DataTable datatable = dt.Select("sys.indexes a , sys.objects b , sys.dm_db_partition_stats c", new string[] { "b.name as tablename ,c.row_count as datacount" },
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
            else
            {
                PublicBD.DB = "ORACLE";
                PublicBD.ReInit();
                OracleData dt = PublicBD.db.dt as OracleData;
                //select * from user_tables
                DataTable datatable = dt.Select("all_tables", new string[] { "*" }, "where  owner='" + textBox_UserName.Text.Trim().ToUpper() + "'");
                //DataTable datatable = dt.Select("sys.indexes a , sys.objects b , sys.dm_db_partition_stats c", new string[] { "b.name as tablename ,c.row_count as datacount" },
                //                                    "where a.[object_id] = b.[object_id] AND b.[object_id] = c.[object_id] AND a.index_id = c.index_id AND a.index_id < 2 AND b.is_ms_shipped = 0");

                if (datatable != null && datatable.Rows.Count > 0)
                {
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        listBox_Table.Items.Add(datatable.Rows[i]["table_name"].ToString());
                    }
                    return true;
                }
                else { return false; }
            }
        }

        //根据表名的到该表字段名
        private void listBox_Field_Init(string TableName, string DBType)
        {
            PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";
            if (DBType == "MSSQL")
            {
                PublicBD.DB = "MSSQL";
                PublicBD.ReInit();
                _51Data dt = PublicBD.db.dt as _51Data;
                DataTable datatable = dt.Select("SysColumns", new string[] { "Name" }, "Where id=Object_Id('" + TableName + "')");
                if (datatable != null && datatable.Rows.Count > 0)
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        listBox_Field.Items.Add(datatable.Rows[i][0]);
                    }
            }
            else
            {
                PublicBD.DB = "ORACLE";
                PublicBD.ReInit();
                OracleData dt = PublicBD.db.dt as OracleData;
                //SELECT * FROM user_tab_columns t WHERE t.TABLE_NAME='表名'
                DataTable datatable = dt.Select("user_tab_columns t", new string[] { "*" }, "Where t.TABLE_NAME='" + TableName + "'");

                if (datatable != null && datatable.Rows.Count > 0)
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        listBox_Field.Items.Add(datatable.Rows[i][1]);
                    }
            }
        }

        private void comboBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Item.SelectedItem.ToString() == "站    号")
            {
                textBox_PadLeft.Enabled = true;
                checkBox_8.Enabled = true;
                checkBox_Where.Enabled = false;
                comboBox_items.Enabled = false;
                textBox_Val.Enabled = false;
                comboBox_Decimal.Enabled = false;
                comboBox_Format.Enabled = false;
            }
            else if (comboBox_Item.SelectedItem.ToString() == "元    素")
            {
                textBox_PadLeft.Enabled = false;
                checkBox_Where.Enabled = true;
                comboBox_items.Enabled = true;
                textBox_Val.Enabled = false;
                checkBox_8.Enabled = false;
                comboBox_Decimal.Enabled = false;
                comboBox_Format.Enabled = false;
            }
            else if (comboBox_Item.SelectedItem.ToString() == "值")
            {
                textBox_PadLeft.Enabled = false;
                comboBox_Decimal.Enabled = true;
                checkBox_8.Enabled = false;
                checkBox_Where.Enabled = false;
                comboBox_items.Enabled = false;
                textBox_Val.Enabled = false;
                comboBox_Format.Enabled = false;
            }
            else if (comboBox_Item.SelectedItem.ToString() == "其    他")
            {
                textBox_PadLeft.Enabled = false;
                textBox_Val.Enabled = true;
                checkBox_Where.Enabled = false;
                comboBox_items.Enabled = false;
                checkBox_8.Enabled = false;
                comboBox_Decimal.Enabled = false;
                comboBox_Format.Enabled = false;
            }
            else
            {
                textBox_PadLeft.Enabled = false;
                checkBox_Where.Enabled = false;
                comboBox_items.Enabled = false;
                textBox_Val.Enabled = false;
                checkBox_8.Enabled = false;
                comboBox_Decimal.Enabled = false;
                comboBox_Format.Enabled = true;
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
                string Type = string.Empty;
                if (comboBox_DBType.SelectedIndex == 0)
                {
                    Type = "MSSQL";
                }
                else
                {
                    Type = "ORACLE";
                }



                xml.WriteDBXML(Type, textBox_Source.Text.Trim(), textBox_DataBase.Text.Trim(), textBox_UserName.Text.Trim(), textBox_PassWord.Text.Trim());


                //得改
                bool b = listBox_Table_Init(Type);

                if (b)
                {
                    MessageBox.Show("数据库信息配置成功!");
                }
                else { MessageBox.Show("配置信息有误，测试连接失败!"); }
            }
        }

        //验证数据库信息
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

        private void listBox_Table_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Type = string.Empty;
            if (comboBox_DBType.SelectedIndex == 0)
            {
                Type = "MSSQL";
            }
            else
            {
                Type = "ORACLE";
            }
            listBox_Field.Items.Clear();
            listBox_Field_Init(listBox_Table.SelectedItem.ToString(), Type);
        }


        string tablename = "";
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
                string setlist = "";
                string fidld = listBox_Field.SelectedItem.ToString();
                if (comboBox_Item.SelectedItem.ToString() == "站    号")
                {
                    if (checkBox_8.Checked)
                    {
                        setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "<--->8\r\n";
                    }
                    else
                    {
                        setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "\r\n";
                    }
                }
                else if (comboBox_Item.SelectedItem.ToString() == "元    素")
                {
                    if (checkBox_Where.Checked)
                    {
                        setlist = comboBox_Item.SelectedItem.ToString() + "(" + comboBox_items.SelectedValue + ")<--->" + fidld + "<--->条件\r\n";
                    }
                    else
                    {
                        setlist = comboBox_Item.SelectedItem.ToString() + "(" + comboBox_items.SelectedValue + ")<--->" + fidld + "\r\n";
                    }
                }
                else if (comboBox_Item.SelectedItem.ToString() == "其    他")
                {
                    setlist = comboBox_Item.SelectedItem.ToString() + "(" + textBox_Val.Text.Trim() + ")<--->" + fidld + "\r\n";
                }
                else if (comboBox_Item.SelectedItem.ToString() == "值")
                {
                    if (comboBox_Decimal.SelectedIndex > 0)
                    {
                        setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "<--->" + comboBox_Decimal.SelectedItem.ToString() + "\r\n";
                    }
                    else { setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "\r\n"; }
                }
                else
                {
                    setlist = comboBox_Item.SelectedItem.ToString() + "<--->" + fidld + "\r\n";
                }

                if (textBox_Text.Text.Contains(fidld + "\r\n"))
                {
                    MessageBox.Show("该字段已经配置!");
                }
                else
                {
                    textBox_Text.Text += setlist;
                }
            }
        }

        //配置field信息格式化为List
        private List<DataResaveClass> Format()
        {
            List<DataResaveClass> list;
            if (dataGridView_List.DataSource == null)
            {
                list = new List<DataResaveClass>();
            }
            else
            {
                list = dGVlist;
            }

            string str = textBox_Text.Text.Trim();
            string[] strs = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string Code = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (strs.Length > 0)
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
                            drc.OldField = str_s[0].Substring(0, m);
                            drc.Value = str_s[0].Substring(m + 1, n - m - 1);

                            if (str_s.Length == 3)
                            {
                                drc.Where = "条件";
                            }
                        }
                        else if (str_s[0].Contains("站    号"))
                        {
                            drc.OldField = str_s[0];
                            drc.Value = "NULL";
                            if (str_s.Length == 3 && str_s[2] == "8")
                            {
                                drc.Where = "8";
                            }
                        }
                        else if (str_s[0].Contains("值"))
                        {
                            drc.OldField = str_s[0];
                            drc.Value = "NULL";
                            if (str_s.Length == 3)
                            {
                                drc.Where = str_s[2];
                            }
                        }
                        else
                        {
                            drc.OldField = str_s[0];
                            drc.Value = "NULL";
                        }

                        drc.TableName = tablename;
                        drc.NewField = str_s[1];
                        drc.Code = Code;
                        list.Add(drc);
                    }
                }

            return list;
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
                        SetdataGridViewHeader();
                    }
                    else { dataGridView_List.DataSource = null; }
                    tablename = "";
                    textBox_Text.Text = "";
                    return;
                }


                var query = from l in list where l.TableName == tablename select l;
                if (query.Count() > 0)
                {
                    DialogResult Result =MessageBox.Show("该表配置信息已存在，是否更新（Y更新，N追加）？", "提示", MessageBoxButtons.YesNoCancel);
                    if (Result == DialogResult.Yes)
                    {
                        var q = from l in list where l.TableName != tablename select l;
                        List<DataResaveClass> ss = q.ToList<DataResaveClass>();
                        dGVlist = ss;
                        dataGridView_List.DataSource = null;
                        if (dGVlist != null && dGVlist.Count > 0)
                        {
                            dataGridView_List.DataSource = dGVlist;
                            SetdataGridViewHeader();
                        }
                        else { dataGridView_List.DataSource = null; }
                        dGVlist = Format();
                        dataGridView_List.DataSource = null;
                        if (dGVlist != null && dGVlist.Count > 0)
                        {
                            dataGridView_List.DataSource = dGVlist;
                            SetdataGridViewHeader();
                        }
                        else { dataGridView_List.DataSource = null; }
                        tablename = "";
                        textBox_Text.Text = "";
                    }
                    else if (Result == DialogResult.No)
                    {
                        dGVlist = Format();
                        dataGridView_List.DataSource = null;
                        if (dGVlist != null && dGVlist.Count > 0)
                        {
                            dataGridView_List.DataSource = dGVlist;
                            SetdataGridViewHeader();
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
                        SetdataGridViewHeader();
                    }
                    else { dataGridView_List.DataSource = null; }

                    tablename = "";
                    textBox_Text.Text = "";
                }


            }
        }
        //配置field信息的List转化为string
        private string ReFormat()
        {

            string str = "";
            List<DataResaveClass> list = dGVlist;
            string Code = dataGridView_List.Rows[dataGridView_List.SelectedCells[0].RowIndex].Cells[dataGridView_List.SelectedCells[0].ColumnIndex - 1].Value.ToString();
            var query = from l in list where l.TableName == tablename && l.Code == Code select l;
            foreach (var item in query)
            {
                if (item.Value == "NULL")
                {
                    if (item.OldField == "值" || item.OldField == "站    号")
                    {
                        if (item.Where != null)
                        {
                            str += item.OldField + "<--->" + item.NewField + "<--->" + item.Where + "\r\n";
                        }
                        else
                        {
                            str += item.OldField + "<--->" + item.NewField + "\r\n";
                        }
                    }
                    else
                    {
                        str += item.OldField + "<--->" + item.NewField + "\r\n";
                    }
                }
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

            var q = from l in list where l.Code != Code select l;
            dGVlist = q.ToList<DataResaveClass>();

            if (dGVlist != null && dGVlist.Count > 0)
            {
                dataGridView_List.DataSource = dGVlist;
                SetdataGridViewHeader();
            }
            else { dataGridView_List.DataSource = null; }
            return str;
        }

        private void button_Left_Click(object sender, EventArgs e)
        {
            if (dataGridView_List.SelectedCells.Count == 0) { return; }
            if (dataGridView_List.SelectedCells[0].ColumnIndex != 1)
            {
                MessageBox.Show("请在列表第二列中选择表名！");
            }
            else
            {
                tablename = dataGridView_List.SelectedCells[0].Value.ToString();
                textBox_Text.Text = ReFormat();
            }
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            tablename = "";
            textBox_Text.Text = "";
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try{
                string xmlstr = XMLOperation.listToXml<DataResaveClass>(dataGridView_List.DataSource as List<DataResaveClass>);

                xml.WriteFieldToFieldXML(xmlstr);
                xml.WriteDateTimeXML(dateTimePicker_B.Value);
                xml.WriteMinuteXML(int.Parse(comboBox_Minute.SelectedItem.ToString()));
                xml.WriteRainStage(checkBox_Rain.Checked .ToString());
                MessageBox.Show("字段信息保存成功！");
            }
            catch { MessageBox.Show("字段信息保存失败！"); }
        }

        private void button_ClearXML_Click(object sender, EventArgs e)
        {
            xml.WriteFieldToFieldXML("");
            dataGridView_List.DataSource = XMLOperation.xmlToList<DataResaveClass>(xml.ReadFieldToFieldXML());
            SetdataGridViewHeader();
        }

        Thread thread = null;
        private void button_Start_Click(object sender, EventArgs e)
        {
            if (button_Start.Text == "启动")
            {
                groupBox3.Visible = false;
                groupBox2.Visible = false;

                //checkBox_Rain.Visible = false;
                //checkBox_Stage.Visible = false;

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

                //checkBox_Rain.Visible = true;
                //checkBox_Stage.Visible = true;

                thread.Abort();
                thread = null;
                ThreadState = false;
                button_Start.Text = "启动";
                dateTimePicker_Init();
            }
        }
        bool ThreadState = false;
        public void ThreadSatrt()
        {
            int M = int.Parse(xml.ReadMinuteXML());
            DBtoDB();
            xml.WriteDateTimeXML(DateTime.Now);
            Thread.Sleep(M * 60 * 1000);
            while (ThreadState)
            {
                S = 0;
                F = 0;
                K = 0;
                DBtoDB();
                xml.WriteDateTimeXML(DateTime.Now);
                Thread.Sleep(M * 60 * 1000);

                zxsj = DateTime.Now.ToString();
                COUNT++;
            }
        }

        string zxsj = "";
        int COUNT = 0;
        int Success = 0;
        int S = 0;
        int Failure = 0;
        int F = 0;
        int Key = 0;
        int K = 0;

        private void DBtoDB()
        {
            #region 读取转存字段配置信息
            List<DataResaveClass> list =XMLOperation. xmlToList<DataResaveClass>(xml.ReadFieldToFieldXML());
            List<string> tables = new List<string>();
            foreach (var item in list)
            {
                tables.Add(item.TableName + "*" + item.Code);
            }
            tables = tables.Distinct().ToList();
            #endregion



            List<string> fields;
            List<object> values;
            List<object> values_1;
            foreach (var table in tables)
            {
                string tb = table.Split(new char[] { '*' })[0];

                string whereAnd = "";
                var l = from L in list where table == L.TableName + "*" + L.Code select L;
                fields = new List<string>();
                values = new List<object>();
                values_1 = new List<object>();
                foreach (var item in l)
                {

                    if (item.Where != "条件")
                    {
                        fields.Add(item.NewField);
                    }

                    if (item.OldField == "元    素" || item.OldField == "其    他")
                    {
                        if (item.Where != "条件")
                        {
                            values.Add(item.Value);
                            values_1.Add(item.Value);
                        }
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
                        if (item.Where == "8")
                        {
                            values.Add("[0]-8");
                            values_1.Add("[0]-8");
                        }
                        else
                        {
                            values.Add("[0]");
                            values_1.Add("[0]");
                        }
                    }
                    else if (item.OldField == "监测时间")
                    {
                        values.Add("[1]");
                        values_1.Add("[1]");
                    }
                    else if (item.OldField == "接收时间")
                    {
                        values.Add("[2]");
                        values_1.Add("[2]");
                    }
                    else if (item.OldField == "值")
                    {
                        if (item.Where != null)
                        {
                            values.Add("[3]-" + item.Where);
                            values_1.Add("[3]-" + item.Where);
                        }
                        else
                        {
                            values.Add("[3]");
                            values_1.Add("[3]");
                        }

                    }

                }


                #region 读取原始数据库中的数据
                string datetime = xml.ReadDateTimeXML();
                datetime = DateTime.Parse(datetime).AddSeconds(-60).ToString("yyyy-MM-dd HH:mm:ss");
                PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/System.xml";
                PublicBD.DB = "MSSQL";
                PublicBD.ReInit();
                _51Data dt = PublicBD.db.dt as _51Data;

                //根据测站类型筛选数据
                string STTypeWhere = "";
                //if (table.ToUpper().Contains("PPTN"))
                //{ STTypeWhere = " and STTYPE='50'"; }
                //else if (table.ToUpper() .Contains("RIVER"))
                //{ STTypeWhere = " and STTYPE='48'"; }
                //else if (table.ToUpper().Contains("RSVR"))
                //{ STTypeWhere = " and STTYPE='4B'"; }

                if (tb.ToUpper().Contains("RIVER"))
                { STTypeWhere = " and STTYPE='48'"; }
                else if (tb.ToUpper().Contains("RSVR"))
                { STTypeWhere = " and STTYPE='4B'"; }


                DataTable datatable = dt.Select("YY_DATA_AUTO", new string[] { "*" }, "where DOWNDATE>='" + datetime + "'" + whereAnd + STTypeWhere);

                #endregion

                string Type = string.Empty;
                if (comboBox_DBType.SelectedIndex == 0)
                {
                    Type = "MSSQL";
                }
                else
                {
                    Type = "ORACLE";
                }

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
                            if (values_1[j].ToString() == "[0]-8")  //只导末尾8位
                            {
                                if (datatable.Rows[i]["STCD"].ToString().Length == 10)
                                {
                                    values[j] = datatable.Rows[i]["STCD"].ToString().Substring(2, 8);
                                }
                                else
                                {
                                    values[j] = datatable.Rows[i]["STCD"].ToString();
                                }
                            }
                            else if (values_1[j].ToString() == "[1]")
                            {
                                if (Type == "MSSQL")
                                {
                                    values[j] = datatable.Rows[i]["TM"].ToString();
                                }
                                else
                                {
                                    values[j] = DateTime.Parse(datatable.Rows[i]["TM"].ToString());
                                }

                            }
                            else if (values_1[j].ToString() == "[2]")
                            {
                                if (Type == "MSSQL")
                                {
                                    values[j] = datatable.Rows[i]["DOWNDATE"].ToString();
                                }
                                else
                                {
                                    values[j] = DateTime.Parse(datatable.Rows[i]["DOWNDATE"].ToString());
                                }

                            }
                            else if (values_1[j].ToString() == "[3]")
                            { values[j] = datatable.Rows[i]["DATAVALUE"].ToString(); }
                            else if (values_1[j].ToString().Contains("[3]-"))
                            {
                                string[] strs = values_1[j].ToString().Split(new char[] { '-' });
                                int Decimal = 0;
                                if (int.TryParse(strs[1], out Decimal))
                                {
                                    decimal val;
                                    if (decimal.TryParse(datatable.Rows[i]["DATAVALUE"].ToString(), out val))
                                    {
                                        values[j] = val.ToString("f" + Decimal);
                                    }
                                }
                            }
                        }


                        PublicBD.Path = System.Windows.Forms.Application.StartupPath + "/Resave.xml";
                        if (Type == "MSSQL")
                        {
                            //写入转存库，并记录成功失败数量
                            PublicBD.DB = "MSSQL";
                            PublicBD.ReInit();
                            _51Data newdt = PublicBD.db.dt as _51Data;

                            //雨量pptn逻辑
                            //同时设置了导入日雨量（DYP）和时段雨量（DRP）
                            //先导入其中一个
                            //导入另一个时，会出现因为时间主键冲突
                            //然后更新这个时间的数据。

                            int B = 0;
                            string INTV = "";
                            if (tb.ToUpper().Contains("PPTN"))
                            {
                                int IS5 = 0;
                                var I = from F in fields where F.ToUpper() == "INTV" select F;
                                if (I.Count() > 0)
                                {
                                    for (int t = 0; t < fields.Count; t++)
                                    {
                                        if (fields[t].ToUpper() == "INTV")
                                        {
                                            if (values[t].ToString() == "0.05")
                                            { INTV = values[t].ToString(); IS5++; }
                                        }
                                        if (fields[t].ToUpper() == "TM")
                                        {
                                            DateTime DT = DateTime.Parse(values[t].ToString());
                                            if (DT.Minute == 0 && DT.Second == 0)
                                            { IS5++; }
                                        }
                                    }
                                }

                                if (IS5 != 2)   //如果不是整点五分钟
                                {
                                    B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());
                                    if (B == 1) //成功
                                    { Success++; S++; }
                                    else if (B == 2)//主键冲突
                                    { Key++; K++; }
                                    else //失败
                                    { Failure++; F++; }
                                }
                                else
                                {
                                    if (INTV == "1") //整点小时数据
                                    {
                                        B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());
                                        if (B == 1) //成功
                                        { Success++; S++; }
                                        else if (B == 2)//主键冲突
                                        {
                                            Key++; K++;

                                            #region //删除INTV字段和值，然后更新
                                            string UpdWhere = "";
                                            int z = -1;
                                            var f = from F in fields where F.ToUpper() == "DYP" || F.ToUpper() == "DRP" select F;
                                            if (f.Count() > 0)
                                            {

                                                for (int j = 0; j < fields.Count; j++)
                                                {
                                                    if (fields[j].ToUpper() == "STCD")
                                                    {
                                                        UpdWhere = " where stcd='" + values[j] + "'";
                                                    }
                                                }

                                                for (int j = 0; j < fields.Count; j++)
                                                {
                                                    if (fields[j].ToUpper() == "TM")
                                                    {
                                                        UpdWhere += " and TM='" + values[j] + "'";
                                                    }
                                                    if (fields[j].ToUpper() == "INTV")
                                                    {
                                                        z = j;
                                                    }
                                                }
                                                if (z != -1)
                                                {
                                                    fields.RemoveAt(z);
                                                    values.RemoveAt(z);
                                                }
                                                bool b = newdt.Update(tb, fields.ToArray(), values.ToArray(), UpdWhere);
                                                if (!b)
                                                { Failure++; F++; }
                                                else
                                                {
                                                    Success++; S++;
                                                }
                                            }
                                            #endregion
                                        }
                                        else //失败
                                        { Failure++; F++; }
                                    }
                                    else if (INTV == "24")
                                    {
                                        B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());
                                        if (B == 1) //成功
                                        { Success++; S++; }
                                        else if (B == 2)//主键冲突
                                        {
                                            Key++; K++;
                                            #region //直接更新
                                            string UpdWhere = "";
                                            var f = from F in fields where F.ToUpper() == "DYP" || F.ToUpper() == "DRP" select F;
                                            if (f.Count() > 0)
                                            {

                                                for (int j = 0; j < fields.Count; j++)
                                                {
                                                    if (fields[j].ToUpper() == "STCD")
                                                    {
                                                        UpdWhere = " where stcd='" + values[j] + "'";
                                                    }
                                                }

                                                for (int j = 0; j < fields.Count; j++)
                                                {
                                                    if (fields[j].ToUpper() == "TM")
                                                    {
                                                        UpdWhere += " and TM='" + values[j] + "'";
                                                    }
                                                }
                                                bool b = newdt.Update(tb, fields.ToArray(), values.ToArray(), UpdWhere);
                                                if (!b)
                                                { Failure++; F++; }
                                                else
                                                {
                                                    Success++; S++;
                                                }
                                            }
                                            #endregion
                                        }
                                        else //失败
                                        { Failure++; F++; }
                                    }
                                }

                            }
                            else  //如果不是pptn表 
                            {
                                B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());
                                if (B == 1) //成功
                                { Success++; S++; }
                                else if (B == 2)//主键冲突
                                { Key++; K++; }
                                else //失败
                                { Failure++; F++; }
                            }






                            //int B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());

                            //if (B == 1) //成功
                            //{ Success++; S++; }
                            //else if (B == 2)
                            //{

                            //    string UpdWhere = "";
                            //    var f = from F in fields where F.ToUpper() == "DYP" || F.ToUpper() == "DRP" select F;
                            //    if (f.Count() > 0)
                            //    {

                            //        for (int j = 0; j < fields.Count; j++)
                            //        {
                            //            if (fields[j].ToUpper() == "STCD")
                            //            {
                            //                UpdWhere = " where stcd='" + values[j] + "'";
                            //            }
                            //        }

                            //        for (int j = 0; j < fields.Count; j++)
                            //        {
                            //            if (fields[j].ToUpper() == "TM")
                            //            {
                            //                UpdWhere += " and TM='" + values[j] + "'";
                            //            }
                            //        }


                            //        #region //插入报主键失败肯定是8：00数据，如果设置了INTV字段，值设置24，如没有设置，增加字段，值设置24。
                            //        int IS5=0;
                            //        var I = from F in fields where F.ToUpper() == "INTV" select F;
                            //        if (I.Count() > 0)
                            //        {
                            //            for (int t = 0; t < fields.Count; t++)
                            //            {
                            //                if (fields[t].ToUpper() == "INTV") 
                            //                {
                            //                    if (values[t].ToString() == "0.05")
                            //                    { IS5++; }
                            //                    values[t] = 24;
                            //                }
                            //                if (fields[t].ToUpper() == "TM") 
                            //                {
                            //                    DateTime DT = DateTime.Parse(values[t].ToString());
                            //                    if (DT.Minute == 0 && DT.Second == 0) 
                            //                    {
                            //                        IS5++;
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            fields.Add("intv");
                            //            values.Add(24);
                            //        }
                            //        #endregion


                            //        if (IS5 < 2) //IS5如果等于2说明数据是整点5分钟的数据，将不执行更新语句
                            //        {
                            //            bool b = newdt.Update(tb, fields.ToArray(), values.ToArray(), UpdWhere);
                            //            if (!b)
                            //            { Failure++; F++; }
                            //            else
                            //            {
                            //                Success++; S++;
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {

                            //        //主键冲突
                            //        Key++; K++;
                            //    }
                            //}
                            //else
                            //{
                            //    Failure++; F++;
                            //}
                        }
                        else
                        {
                            //写入转存库，并记录成功失败数量
                            PublicBD.DB = "ORACLE";
                            PublicBD.ReInit();
                            OracleData newdt = PublicBD.db.dt as OracleData;

                            int B = newdt.Insert_1(tb, fields.ToArray(), values.ToArray());

                            if (B == 1)
                            { Success++; S++; }
                            else if (B == 2)
                            {
                                string UpdWhere = "";
                                var f = from F in fields where F.ToUpper() == "DYP" || F.ToUpper() == "DRP" select F;
                                if (f.Count() > 0)
                                {

                                    for (int j = 0; j < fields.Count; j++)
                                    {
                                        if (fields[j].ToUpper() == "STCD")
                                        {
                                            UpdWhere = " where stcd='" + values[j] + "'";
                                        }
                                    }

                                    for (int j = 0; j < fields.Count; j++)
                                    {
                                        if (fields[j].ToUpper() == "TM")
                                        {
                                            UpdWhere += " and TM= to_date('" + values[j] + "','yyyy-mm-dd hh24:mi:ss')";
                                        }
                                    }



                                    #region //插入报主键失败肯定是8：00数据，如果设置了INTV字段，值设置24，如没有设置，增加字段，值设置24。
                                    int IS5 = 0;
                                    var I = from F in fields where F.ToUpper() == "INTV" select F;
                                    if (I.Count() > 0)
                                    {
                                        for (int t = 0; t < fields.Count; t++)
                                        {
                                            if (fields[t].ToUpper() == "INTV")
                                            {
                                                if (values[t].ToString() == "0.05")
                                                { IS5++; }
                                                values[t] = 24;
                                            }
                                            if (fields[t].ToUpper() == "TM")
                                            {
                                                DateTime DT = DateTime.Parse(values[t].ToString());
                                                if (DT.Minute == 0 && DT.Second == 0)
                                                {
                                                    IS5++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fields.Add("intv");
                                        values.Add(24);
                                    }
                                    #endregion

                                    if (IS5 < 2) //IS5如果等于2说明数据是整点5分钟的数据，将不执行更新语句
                                    {
                                        bool b = newdt.Update(tb, fields.ToArray(), values.ToArray(), UpdWhere);
                                        if (!b)
                                        { Failure++; F++; }
                                        else
                                        {
                                            Success++; S++;
                                        }
                                    }
                                }
                                else
                                {

                                    //主键冲突
                                    Key++; K++;
                                }
                            }
                            else
                            {
                                Failure++; F++;
                            }
                        }


                    }

                }


            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label11.Text = "统计  " + Success + " 成功， " + Failure + " 失败，主键冲突" + Key + "。";
            label12.Text = "本次  " + S + " 成功， " + F + " 失败，主键冲突" + K + "。";
            label14.Text = "执行次数：  " + COUNT;
            this.Text = "此次执行时间:" + zxsj;
        }

    }


    public partial class DataResaveClass
    {
        public DataResaveClass()
        { }
        #region Model
        private string _code;
        private string _tablename;
        private string _oldfield;
        private string _value;
        private string _newfield;
        private string _where;

        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
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
        }/// <summary>
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
