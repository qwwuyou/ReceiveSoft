using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp.SetControl
{
    public partial class SetSystemControl : UserControl
    {
        public SetSystemControl()
        {
            InitializeComponent();
        }

        List<OperateXML.serviceModel> Lsm = null;
        private void SetSystemControl_Load(object sender, EventArgs e)
        {
            Form_Init();

            comboBox_NFOINDEX_SelectedIndexChanged(comboBox_NFOINDEX, new EventArgs());
            this.comboBox_NFOINDEX.SelectedIndexChanged += new System.EventHandler(this.comboBox_NFOINDEX_SelectedIndexChanged);
        }


        private void Form_Init() 
        {
            comboBox_DBType_Init();
            DB_Init();
            dataGridViewStyle(dataGridView1);

            Lsm = Program.wrx.XMLObj.LsM;
            DropDownList_Init(comboBox_NFOINDEX);
            comboBox_Protocol_Init();

            textBox_ip.Text = Program.wrx.XMLObj.UiTcpModel.IP;
            textBox_port.Text = Program.wrx.XMLObj.UiTcpModel.PORT.ToString();

            //if (Program.LoginState) 
            //{
            //    button_Set1.Enabled = true;
            //    button_Set2.Enabled = true;
            //    button_Set4.Enabled = true;
            //    button_Restar.Enabled = true;
            //}
        }

        private void DB_Init() 
        {
            try
            {
                textBox_Source.Text = Program.wrx.XMLObj.DBserver;
                textBox_DataBase.Text =  Program.wrx.XMLObj.DBcatalog;
                textBox_UserName.Text =  Program.wrx.XMLObj.DBusername;
                textBox_PassWord.Text =  Program.wrx.XMLObj.DBpassword;
                if ( Program.wrx.XMLObj.DBtype.ToLower() == "mssql")
                {
                    comboBox_DBType.SelectedIndex = 0;
                }
                else if ( Program.wrx.XMLObj.DBtype.ToLower() == "mysql")
                { comboBox_DBType.SelectedIndex = 1; }
                else
                { comboBox_DBType.SelectedIndex = 2; }
            }
            catch { }
        }

        private void DropDownList_Init(ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.Add("TCP--GPRS");
            cb.Items.Add("UDP--GPRS");
            cb.Items.Add("GSM--短信");
            cb.Items.Add("COM--卫星");
            cb.SelectedIndex= 0;

        }

        private void comboBox_DBType_Init()
        {
            comboBox_DBType.Items.Clear();
            comboBox_DBType.Items.Add("sql server");
            comboBox_DBType.Items.Add("mysql");
            comboBox_DBType.Items.Add("oracle");
            comboBox_DBType.SelectedIndex = 0;
        }

        private void comboBox_Protocol_Init() 
        {
            string protocol= Program.wrx.XMLObj.dllfile.ToLower() ;
            if (protocol == "gsprotocol.dll")
            {
                comboBox_Protocol.SelectedIndex = 0;
                label16.Text = "编码：ASC";
            }
            else if (protocol == "hydrologicprotocol.dll")
            {
                comboBox_Protocol.SelectedIndex = 1; 
                label16.Text = "编码：HEX";
            }
            else if (protocol == "protocol.dll")
            {
                comboBox_Protocol.SelectedIndex = 2;
                label16.Text = "编码：HEX";
            }
            else if (protocol == "hjt212-2005.dll")
            {
                comboBox_Protocol.SelectedIndex = 3;
                label16.Text = "编码：ASC";
            }
            else if (protocol == "ADJC-001.dll")
            {
                comboBox_Protocol.SelectedIndex = 4;
                label16.Text = "编码：ASC";
            } 
        }

        #region DataGridView样式
        private void dataGridViewStyle(DataGridView DGV)
        {
            DGV.BackgroundColor = this.ParentForm.BackColor;

            DGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DGV.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DGV.EnableHeadersVisualStyles = false;
            DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            DGV.ColumnHeadersHeight = 25;

            DGV.GridColor = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Color c = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;
            int Alpha = 30;
            int R = 255 + (c.R - 255) * Alpha / 255;
            int G = 255 + (c.G - 255) * Alpha / 255;
            int B = 255 + (c.B - 255) * Alpha / 255;
            DGV.RowsDefaultCellStyle.BackColor = Color.White;
            DGV.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(R, G, B);
            DGV.CellMouseEnter += new DataGridViewCellEventHandler(DGV_CellMouseEnter);
            DGV.CellMouseLeave += new DataGridViewCellEventHandler(DGV_CellMouseLeave);
            DGV.Paint += new PaintEventHandler(DGV_Paint);
        }

        void DGV_Paint(object sender, PaintEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            Pen p = new Pen(((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor);
            e.Graphics.DrawRectangle(p, new Rectangle(0, 0, DGV.Width - 1, DGV.Height - 1));
        }
        void DGV_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            if (e.RowIndex >= 0)
            {
                DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = colorTmp;
            }
        }

        Color colorTmp = Color.White;
        void DGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView DGV = sender as DataGridView;
            if (e.RowIndex >= 0)
            {
                colorTmp = DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor;
                DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = ((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor;

            }
        }
        #endregion

        private void comboBox_NFOINDEX_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            if (comboBox_NFOINDEX.SelectedIndex == 0 )
            {
                var tcplist = from ser in Lsm where ser.SERVICETYPE == "TCP" select ser;
                List<OperateXML.serviceModel> tl = new List<OperateXML.serviceModel>();
                foreach (var item in tcplist)
                {
                    tl.Add(item);
                }

                dataGridView1.DataSource = tl;
                dataGridView1.Columns[0].HeaderText = "信道类型";
                dataGridView1.Columns[0].Name = "SERVICETYPE";
                dataGridView1.Columns[1].HeaderText = "服务ID";
                dataGridView1.Columns[1].Name = "SERVICEID";
                dataGridView1.Columns[2].HeaderText = "IP地址";
                dataGridView1.Columns[2].Name = "IP_PORTNAME";
                dataGridView1.Columns[3].HeaderText = "端口号";
                dataGridView1.Columns[3].Name = "PORT_BAUDRATE";
                dataGridView1.Columns[4].Visible = false;

                label3.Text = "IP地址:";
                label4.Text = "端口号:";
                label11.Visible = false;
                textBox_num.Visible = false ;
            }
            else if (comboBox_NFOINDEX.SelectedIndex == 1)
            {
                var udplist = from ser in Lsm where ser.SERVICETYPE == "UDP" select ser;
                List<OperateXML.serviceModel> ul = new List<OperateXML.serviceModel>();
                foreach (var item in udplist)
                {
                    ul.Add(item);
                }

                dataGridView1.DataSource = ul;
                dataGridView1.Columns[0].HeaderText = "信道类型";
                dataGridView1.Columns[0].Name = "SERVICETYPE";
                dataGridView1.Columns[1].HeaderText = "服务ID";
                dataGridView1.Columns[1].Name = "SERVICEID";
                dataGridView1.Columns[2].HeaderText = "IP地址";
                dataGridView1.Columns[2].Name = "IP_PORTNAME";
                dataGridView1.Columns[3].HeaderText = "端口号";
                dataGridView1.Columns[3].Name = "PORT_BAUDRATE";
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[4].Name = "NUM";
                label3.Text = "IP地址:";
                label4.Text = "端口号:";
                label11.Visible = false;
                textBox_num.Visible = false;
            }
            else if (comboBox_NFOINDEX.SelectedIndex == 2)
            {
                var gsmlist = from ser in Lsm where ser.SERVICETYPE == "GSM" select ser;
                List<OperateXML.serviceModel> gl = new List<OperateXML.serviceModel>();
                foreach (var item in gsmlist)
                {
                    gl.Add(item);
                }

                dataGridView1.DataSource = gl;
                dataGridView1.Columns[0].HeaderText = "信道类型";
                dataGridView1.Columns[0].Name = "SERVICETYPE";
                dataGridView1.Columns[1].HeaderText = "服务ID";
                dataGridView1.Columns[1].Name = "SERVICEID";
                dataGridView1.Columns[2].HeaderText = "COM口";
                dataGridView1.Columns[2].Name = "IP_PORTNAME";
                dataGridView1.Columns[3].HeaderText = "波特率";
                dataGridView1.Columns[3].Name = "PORT_BAUDRATE";
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.Columns[4].HeaderText = "手机号";
                dataGridView1.Columns[4].Name = "NUM";
                label3.Text = "COM口:";
                label4.Text = "波特率:";
                label11.Text = "手机号:";
                label11.Visible = true;
                textBox_num.Visible = true;
            }
            else if (comboBox_NFOINDEX.SelectedIndex == 3)
            {
                var comlist = from ser in Lsm where ser.SERVICETYPE == "COM" select ser;
                List<OperateXML.serviceModel> cl = new List<OperateXML.serviceModel>();
                foreach (var item in comlist)
                {
                    cl.Add(item);
                }

                dataGridView1.DataSource = cl;
                dataGridView1.Columns[0].HeaderText = "信道类型";
                dataGridView1.Columns[0].Name = "SERVICETYPE";
                dataGridView1.Columns[1].HeaderText = "服务ID";
                dataGridView1.Columns[1].Name = "SERVICEID";
                dataGridView1.Columns[2].HeaderText = "COM口";
                dataGridView1.Columns[2].Name = "IP_PORTNAME";
                dataGridView1.Columns[3].HeaderText = "波特率";
                dataGridView1.Columns[3].Name = "PORT_BAUDRATE";
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.Columns[4].HeaderText = "卫星号";
                dataGridView1.Columns[4].Name = "NUM";
                label3.Text = "COM口:";
                label4.Text = "波特率:";
                label11.Text = "卫星号:";
                label11.Visible = true;
                textBox_num.Visible = true;
            }

            if (dataGridView1.DataSource != null)
            {
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
               
                if (dataGridView1.Rows.Count > 0)
                {
                    DataGridViewButtonColumn Column1 = new DataGridViewButtonColumn();
                    Column1.Width = 60;
                    Column1.DataPropertyName = "删 除";
                    Column1.Text = "删  除";
                    Column1.HeaderText = "删  除";
                    Column1.UseColumnTextForButtonValue = true;
                    this.dataGridView1.Columns.Add(Column1);
                    
                }
            }

            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex!=-1)
            {
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "删  除")
                {
                    IList<OperateXML.serviceModel> serv = null;
                    if (comboBox_NFOINDEX.SelectedIndex == 0 || comboBox_NFOINDEX.SelectedIndex == 1)
                    {
                        var ser = from s in Lsm where s.SERVICETYPE == dataGridView1.Rows[e.RowIndex].Cells["SERVICETYPE"].Value.ToString() && s.SERVICEID == dataGridView1.Rows[e.RowIndex].Cells["SERVICEID"].Value.ToString() && s.IP_PORTNAME == dataGridView1.Rows[e.RowIndex].Cells["IP_PORTNAME"].Value.ToString() && s.PORT_BAUDRATE == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["PORT_BAUDRATE"].Value.ToString()) select s;
                        serv = ser.ToList<OperateXML.serviceModel>();
                    }
                    else if (comboBox_NFOINDEX.SelectedIndex == 2 || comboBox_NFOINDEX.SelectedIndex == 3)
                    {
                        var ser = from s in Lsm where s.SERVICETYPE == dataGridView1.Rows[e.RowIndex].Cells["SERVICETYPE"].Value.ToString() && s.SERVICEID == dataGridView1.Rows[e.RowIndex].Cells["SERVICEID"].Value.ToString() && s.IP_PORTNAME == dataGridView1.Rows[e.RowIndex].Cells["IP_PORTNAME"].Value.ToString() && s.PORT_BAUDRATE == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["PORT_BAUDRATE"].Value.ToString()) && s.NUM == dataGridView1.Rows[e.RowIndex].Cells["NUM"].Value.ToString() select s;
                        serv = ser.ToList<OperateXML.serviceModel>();
                    }

                    foreach (var item in serv)
                    {
                        Lsm.Remove(item);
                    }
                    Program.wrx.WriteXML();
                    comboBox_NFOINDEX_SelectedIndexChanged(null, null);
                    //if (DevComponents.DotNetBar.MessageBoxEx.Show("信息配置成功是否重启软件？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    //    System.Environment.Exit(0);
                    //}
                }
                else 
                {
                    IList<OperateXML.serviceModel> serv = null;
                    if (comboBox_NFOINDEX.SelectedIndex == 0 || comboBox_NFOINDEX.SelectedIndex == 1)
                    {
                        var ser = from s in Lsm where s.SERVICETYPE == dataGridView1.Rows[e.RowIndex].Cells["SERVICETYPE"].Value.ToString() && s.SERVICEID == dataGridView1.Rows[e.RowIndex].Cells["SERVICEID"].Value.ToString() && s.IP_PORTNAME == dataGridView1.Rows[e.RowIndex].Cells["IP_PORTNAME"].Value.ToString() && s.PORT_BAUDRATE == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["PORT_BAUDRATE"].Value.ToString()) select s;
                        serv = ser.ToList<OperateXML.serviceModel>();
                    }
                    else if (comboBox_NFOINDEX.SelectedIndex == 2 || comboBox_NFOINDEX.SelectedIndex == 3)
                    {
                        var ser = from s in Lsm where s.SERVICETYPE == dataGridView1.Rows[e.RowIndex].Cells["SERVICETYPE"].Value.ToString() && s.SERVICEID == dataGridView1.Rows[e.RowIndex].Cells["SERVICEID"].Value.ToString() && s.IP_PORTNAME == dataGridView1.Rows[e.RowIndex].Cells["IP_PORTNAME"].Value.ToString() && s.PORT_BAUDRATE == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["PORT_BAUDRATE"].Value.ToString()) && s.NUM == dataGridView1.Rows[e.RowIndex].Cells["NUM"].Value.ToString() select s;
                        serv = ser.ToList<OperateXML.serviceModel>();

                        textBox_num.Text = serv.First().NUM;
                    }

                    textBox_serviceid.Text = serv.First().SERVICEID;
                    textBox_ip_portname.Text = serv.First().IP_PORTNAME;
                    textBox_port_baudrate.Text = serv.First().PORT_BAUDRATE.ToString();
                    
                }
            }
        }

        

        #region 数据库配置【从服务获取、配置本地、配置服务】
        private string DBValidate() 
        {
            string Msg = "";
            if (textBox_Source.Text.Trim() == "") 
            {
                Msg = "请填写服务器名称！"+"\n";
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
        private void button_Syn1_Click(object sender, EventArgs e)
        {
            GetXML("DataBaseConnect");
            timer1.Enabled = true;
            button_Syn1.Enabled = false;
        }
        private void button_SetLocal1_Click(object sender, EventArgs e)
        {
            string Msg = DBValidate();
            if (Msg != "")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show(Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string Type = string.Empty;
                string Port = string.Empty;
                if (comboBox_DBType.SelectedIndex == 0)
                {
                    Type = "MSSQL";
                    Port = "1433";
                }
                else if (comboBox_DBType.SelectedIndex == 1)
                { Type = "MYSQL"; }
                else
                { 
                    Type = "ORACLE"; 
                    Port = "1521";
                }
                Program.wrx.XMLObj.DBtype=Type;
                Program.wrx.XMLObj.DBport = Port;
                Program.wrx.XMLObj.DBserver=textBox_Source.Text.Trim();
                Program.wrx.XMLObj.DBcatalog=textBox_DataBase.Text.Trim();
                Program.wrx.XMLObj.DBusername= textBox_UserName.Text.Trim();
                Program.wrx.XMLObj.DBpassword= textBox_PassWord.Text.Trim();
                Program.wrx.WriteXML();
                DevComponents.DotNetBar.MessageBoxEx.Show("本地信息配置成功!");
            }
        }
        private void button_Set1_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                string xml = Program.wrx.GetXMLStr("DataBaseConnect");
                xml = "--File|DataBaseConnect" + "|" + xml;
                TcpControl.SendUItoServiceCommand(xml);

                if (DevComponents.DotNetBar.MessageBoxEx.Show("服务信息配置成功，请重启软件？\n如果继续配置选[NO]，配置完成后请重启本软件！", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    System.Environment.Exit(0);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，本地信息配置成功，无法将信息同步到服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }
        #endregion

        #region 信道配置【从服务获取、配置本地、配置服务】
        private string SerValidate()
        {
            string Msg = "";
            if (textBox_serviceid.Text.Trim() == "")
            {
                Msg = "请填写服务ID！" + "\n";
            }

            if (comboBox_NFOINDEX.SelectedIndex == 0 || comboBox_NFOINDEX.SelectedIndex == 1)
            {
                if (textBox_ip_portname.Text.Trim() == "")
                {
                    Msg += "请填写IP地址！" + "\n";
                }
                else
                {
                    string regText = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regText);
                    if (!reg.IsMatch(textBox_ip_portname.Text.Trim()))
                    { Msg += "请正确填写IP地址！" + "\n"; }
                }

                if (textBox_port_baudrate.Text.Trim() == "")
                {
                    Msg += "请填写端口号！" + "\n";
                }
                else
                {
                    int port = 0;
                    if (int.TryParse(textBox_port_baudrate.Text.Trim(), out port))
                    {
                        if (port > 65535 || port < 1)
                        {
                            Msg += "请正确填写端口号！" + "\n";
                        }
                    }
                    else
                    { Msg += "请正确填写端口号！" + "\n"; }
                }
            }
            else if (comboBox_NFOINDEX.SelectedIndex == 2 || comboBox_NFOINDEX.SelectedIndex == 3)
            {
                if (textBox_ip_portname.Text.Trim() == "")
                {
                    Msg += "请填写COM口！" + "\n";
                }

                if (textBox_port_baudrate.Text.Trim() == "")
                {
                    Msg += "请填写波特率！" + "\n";
                }
                else
                {
                    int port = 0;
                    if (!int.TryParse(textBox_port_baudrate.Text.Trim(), out port))
                    {
                        Msg += "请正确填写波特率！" + "\n";
                    }
                }

                if (comboBox_NFOINDEX.SelectedIndex == 2 && textBox_num.Text.Trim() == "")
                {
                    Msg += "请填写手机号！" + "\n";
                }

                if (comboBox_NFOINDEX.SelectedIndex == 3 && textBox_num.Text.Trim() == "")
                {
                    Msg += "请填写卫星号！" + "\n";
                }
            }

              
            return Msg;
        }
        private void button_Syn2_Click(object sender, EventArgs e)
        {
            GetXML("Service");
            timer2.Enabled = true;
            button_Syn2.Enabled = false;
        }
        private void button_SetLocal2_Click(object sender, EventArgs e)
        {
            if (Lsm != null)
            {
                string Msg = SerValidate();
                if (Msg != "")
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show(Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    OperateXML.serviceModel s=new OperateXML.serviceModel();
                    s.SERVICEID = textBox_serviceid.Text.Trim();
                    s.IP_PORTNAME = textBox_ip_portname.Text.Trim();
                    s.PORT_BAUDRATE = int.Parse(textBox_port_baudrate.Text.Trim());
                    if (comboBox_NFOINDEX.SelectedIndex == 0)
                    {
                        s.SERVICETYPE = "TCP";
                    }
                    if (comboBox_NFOINDEX.SelectedIndex == 1)
                    {
                        s.SERVICETYPE = "UDP";
                    }
                    if (comboBox_NFOINDEX.SelectedIndex == 2)
                    {
                        s.SERVICETYPE = "GSM";
                        s.NUM = textBox_num.Text.Trim();
                    }
                    if (comboBox_NFOINDEX.SelectedIndex == 3)
                    {
                        s.SERVICETYPE = "COM";
                        s.NUM = textBox_num.Text.Trim();
                    }
                    Lsm.Add(s);
                    Program.wrx.WriteXML();
                    comboBox_NFOINDEX_SelectedIndexChanged(null, null);

                    DevComponents.DotNetBar.MessageBoxEx.Show("本地信息配置成功!");
                }
            }
        }
        private void button_Set2_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                string xml = Program.wrx.GetXMLStr("Service");
                xml = "--File|Service" + "|" + xml;
                TcpControl.SendUItoServiceCommand(xml);

                if (DevComponents.DotNetBar.MessageBoxEx.Show("服务信息配置成功，请重启软件？\n如果继续配置选[NO]，配置完成后请重启本软件！", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    System.Environment.Exit(0);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，本地信息配置成功，无法将信息同步到服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 协议配置【从服务获取、配置本地、配置服务】
        private void button_Syn3_Click(object sender, EventArgs e)
        {
            GetXML("Dll");
            timer3.Enabled = true;
            button_Syn3.Enabled = false;
        }
        private void button_SetLocal3_Click(object sender, EventArgs e)
        {
            if (comboBox_Protocol.SelectedIndex == 1)
            {
                Program.wrx.XMLObj.dllfile="HydrologicProtocol.dll";
                Program.wrx.XMLObj.dllclass="Service.Hydrologic";
                Program.wrx.XMLObj.HEXOrASC="HEX";
                Program.wrx.WriteXML();
            }
            else if (comboBox_Protocol.SelectedIndex == 2)
            {
                Program.wrx.XMLObj.dllfile="Protocol.dll";
                Program.wrx.XMLObj.dllclass="Service.WaterResource";
                Program.wrx.XMLObj.HEXOrASC="HEX";
                Program.wrx.WriteXML();
            }
            else if(comboBox_Protocol.SelectedIndex == 0)
            {
                Program.wrx.XMLObj.dllfile="GSProtocol.dll";
                Program.wrx.XMLObj.dllclass="Service.GS";
                Program.wrx.XMLObj.HEXOrASC="ASC";
                Program.wrx.WriteXML();
            }
            else if (comboBox_Protocol.SelectedIndex == 3)
            {
                Program.wrx.XMLObj.dllfile="HJT212-2005.dll";
                Program.wrx.XMLObj.dllclass="Service.HTJ212";
                Program.wrx.XMLObj.HEXOrASC="ASC";
                Program.wrx.WriteXML();
            }
            else if (comboBox_Protocol.SelectedIndex == 4)
            {
                Program.wrx.XMLObj.dllfile="ADJC-001.dll";
                Program.wrx.XMLObj.dllclass="ADJC_001.ADJC001";
                Program.wrx.XMLObj.HEXOrASC="ASC";
                Program.wrx.WriteXML();
            }

            DevComponents.DotNetBar.MessageBoxEx.Show("本地信息配置成功!");
        }
        private void button_Set3_Click(object sender, EventArgs e)
        {
            string protoco = "shuiwen";
            if (comboBox_Protocol.SelectedIndex == 1)
            {
                protoco = "shuiwen";
            }
            else if (comboBox_Protocol.SelectedIndex == 2)
            {
                protoco = "shuiziyuan";
            }
            else if (comboBox_Protocol.SelectedIndex == 0)
            {
                protoco = "yanyu";
            }
            else if (comboBox_Protocol.SelectedIndex == 3)
            { protoco = "zhengda212"; }

            if (TcpControl.Connected)
            {
                TcpControl.SendUItoServiceCommand("--pro|" + protoco);

                if (DevComponents.DotNetBar.MessageBoxEx.Show("服务信息配置成功，请重启软件？\n如果继续配置选[NO]，配置完成后请重启本软件！", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    System.Environment.Exit(0);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，本地信息配置成功，无法将信息同步到服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 与服务通讯配置【配置本地】
        private string UIValidate() 
        {
            string Msg = "";
            if (textBox_ip.Text.Trim() == "")
            {
                Msg += "请填写IP地址！" + "\n";
            }
            else
            {
                string regText = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regText);
                if (!reg.IsMatch(textBox_ip.Text.Trim()))
                { Msg += "请正确填写IP地址！" + "\n"; }
            }

            if (textBox_port.Text.Trim() == "")
            {
                Msg += "请填写端口号！" + "\n";
            }
            else
            {
                int port = 0;
                if (int.TryParse(textBox_port.Text.Trim(), out port))
                {
                    if (port > 65535 || port < 1)
                    {
                        Msg += "请正确填写端口号！" + "\n";
                    }
                }
                else
                { Msg += "请正确填写端口号！" + "\n"; }
            }

            return Msg;
        }
        private void button_SetLocal4_Click(object sender, EventArgs e)
        {
            string Msg = UIValidate();
            if (Msg != "")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show(Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                Program.wrx.XMLObj.UiTcpModel.IP=textBox_ip.Text.Trim();
                Program.wrx.XMLObj.UiTcpModel.PORT=int.Parse(textBox_port.Text.Trim());
                Program.wrx.WriteXML();
                if (DevComponents.DotNetBar.MessageBoxEx.Show("信息配置成功是否重启软件？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    System.Environment.Exit(0);
                }
            }
        }
        #endregion

        #region 【从服务获取】读秒
        int Syn1Sleep = 5;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Syn1Sleep == 0)
            {
                button_Syn1.Text = "从服务获取";
                Syn1Sleep = 5;
                timer1.Enabled = false;
                button_Syn1.Enabled = true;

                Form_Init();
            }
            else
            {
                button_Syn1.Text = "获取(" + Syn1Sleep + ")";
                Syn1Sleep--;
            }
        }
        
        int Syn2Sleep = 5;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Syn2Sleep == 0)
            {
                button_Syn2.Text = "从服务获取";
                Syn2Sleep = 5;
                timer2.Enabled = false;
                button_Syn2.Enabled = true;

                Form_Init();
            }
            else
            {
                button_Syn2.Text = "获取(" + Syn2Sleep + ")";
                Syn2Sleep--;
            }
        }

        int Syn3Sleep = 5;
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (Syn3Sleep == 0)
            {
                button_Syn3.Text = "从服务获取";
                Syn3Sleep = 5;
                timer3.Enabled = false;
                button_Syn3.Enabled = true;

                Form_Init();
            }
            else
            {
                button_Syn3.Text = "获取(" + Syn3Sleep + ")";
                Syn3Sleep--;
            }
        }

        private void GetXML(string node)
        {
            if (TcpControl.Connected)
            {
                TcpControl.SendUItoServiceCommand("--file|" + node);
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法同步信息！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void button_Restar_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Environment.Exit(0);
        }

        private void buttonX_OK_Click(object sender, EventArgs e)
        {
            if (textBox_SYSUserName.Text.Trim() == "" || textBox_SYSPassWord.Text.Trim() == "")
            { DevComponents.DotNetBar.MessageBoxEx.Show("   用户名或密码配置不能为空！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else
            {
                if (buttonX_OK.Text == "确 认")
                {
                    if (textBox_SYSUserName.Text.Trim() ==Program.wrx.XMLObj. UserName && textBox_SYSPassWord.Text.Trim() ==Program.wrx.XMLObj. PassWord)
                    {
                        textBox_SYSUserName.Text = "";
                        textBox_SYSPassWord.Text = "";
                        label14.Text = "新密码：";
                        label15.Text = "新用户名：";
                        buttonX_OK.Text = "配 置";
                    }
                    else
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show("   输入有误，请输入正确用户名和密码！", "[错误]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string UserName = textBox_SYSUserName.Text.Trim();
                    string PassWord = textBox_SYSPassWord.Text.Trim();
                    Program.wrx.XMLObj.UserName=UserName;
                    Program.wrx.XMLObj.PassWord=PassWord;
                     Program.wrx.WriteXML();

                    textBox_SYSUserName.Text = "";
                    textBox_SYSPassWord.Text = "";
                    label14.Text = "当前密码：";
                    label15.Text = "当前用户名：";
                    buttonX_OK.Text = "确 认";
                    DevComponents.DotNetBar.MessageBoxEx.Show("   用户名和密码配置成功,软件重启后生效！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void comboBox_Protocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox_Protocol.SelectedIndex == 1)
            {
                label16.Text = "编码：HEX";
            }
            else if (comboBox_Protocol.SelectedIndex == 2)
            {
                label16.Text = "编码：HEX";
            }
            else
            {
                label16.Text = "编码：ASC";
            }
        }

    }
}
