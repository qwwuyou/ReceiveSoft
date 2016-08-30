namespace YYApp.SetControl
{
    partial class SetSystemControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_SetLocal1 = new DevComponents.DotNetBar.ButtonX();
            this.comboBox_DBType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button_Syn1 = new DevComponents.DotNetBar.ButtonX();
            this.button_Set1 = new DevComponents.DotNetBar.ButtonX();
            this.textBox_PassWord = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_DataBase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Source = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_SetLocal2 = new DevComponents.DotNetBar.ButtonX();
            this.button_Syn2 = new DevComponents.DotNetBar.ButtonX();
            this.button_SetLocal4 = new DevComponents.DotNetBar.ButtonX();
            this.textBox_num = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_Set2 = new DevComponents.DotNetBar.ButtonX();
            this.textBox_port_baudrate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ip_portname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_serviceid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_NFOINDEX = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.button_Set3 = new DevComponents.DotNetBar.ButtonX();
            this.button_Syn3 = new DevComponents.DotNetBar.ButtonX();
            this.comboBox_Protocol = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button_Restar = new DevComponents.DotNetBar.ButtonX();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonX_OK = new DevComponents.DotNetBar.ButtonX();
            this.textBox_SYSPassWord = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_SYSUserName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_SetLocal3 = new DevComponents.DotNetBar.ButtonX();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_SetLocal1);
            this.groupBox1.Controls.Add(this.comboBox_DBType);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.button_Syn1);
            this.groupBox1.Controls.Add(this.button_Set1);
            this.groupBox1.Controls.Add(this.textBox_PassWord);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox_UserName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_DataBase);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_Source);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(11, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 214);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库配置";
            // 
            // button_SetLocal1
            // 
            this.button_SetLocal1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_SetLocal1.Location = new System.Drawing.Point(86, 180);
            this.button_SetLocal1.Name = "button_SetLocal1";
            this.button_SetLocal1.Size = new System.Drawing.Size(75, 23);
            this.button_SetLocal1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_SetLocal1.TabIndex = 63;
            this.button_SetLocal1.Text = "配置本地";
            this.button_SetLocal1.Click += new System.EventHandler(this.button_SetLocal1_Click);
            // 
            // comboBox_DBType
            // 
            this.comboBox_DBType.FormattingEnabled = true;
            this.comboBox_DBType.Location = new System.Drawing.Point(95, 20);
            this.comboBox_DBType.Name = "comboBox_DBType";
            this.comboBox_DBType.Size = new System.Drawing.Size(126, 20);
            this.comboBox_DBType.TabIndex = 62;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 61;
            this.label13.Text = "数据库类型：";
            // 
            // button_Syn1
            // 
            this.button_Syn1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Syn1.Location = new System.Drawing.Point(6, 180);
            this.button_Syn1.Name = "button_Syn1";
            this.button_Syn1.Size = new System.Drawing.Size(75, 23);
            this.button_Syn1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Syn1.TabIndex = 60;
            this.button_Syn1.Text = "从服务获取";
            this.button_Syn1.Tooltip = "通讯服务配置信息同步到本地<i>(如服务与本软件共用配置信息，本功能无效)</i>";
            this.button_Syn1.Click += new System.EventHandler(this.button_Syn1_Click);
            // 
            // button_Set1
            // 
            this.button_Set1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Set1.Location = new System.Drawing.Point(166, 180);
            this.button_Set1.Name = "button_Set1";
            this.button_Set1.Size = new System.Drawing.Size(75, 23);
            this.button_Set1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Set1.TabIndex = 60;
            this.button_Set1.Text = "配置服务";
            this.button_Set1.Click += new System.EventHandler(this.button_Set1_Click);
            // 
            // textBox_PassWord
            // 
            this.textBox_PassWord.Location = new System.Drawing.Point(95, 147);
            this.textBox_PassWord.Name = "textBox_PassWord";
            this.textBox_PassWord.PasswordChar = '*';
            this.textBox_PassWord.Size = new System.Drawing.Size(126, 21);
            this.textBox_PassWord.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "密  码：";
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Location = new System.Drawing.Point(95, 114);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(126, 21);
            this.textBox_UserName.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "用户名：";
            // 
            // textBox_DataBase
            // 
            this.textBox_DataBase.Location = new System.Drawing.Point(95, 82);
            this.textBox_DataBase.Name = "textBox_DataBase";
            this.textBox_DataBase.Size = new System.Drawing.Size(126, 21);
            this.textBox_DataBase.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "数据库名称：";
            // 
            // textBox_Source
            // 
            this.textBox_Source.Location = new System.Drawing.Point(95, 50);
            this.textBox_Source.Name = "textBox_Source";
            this.textBox_Source.Size = new System.Drawing.Size(126, 21);
            this.textBox_Source.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "服务器名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_SetLocal2);
            this.groupBox2.Controls.Add(this.button_Syn2);
            this.groupBox2.Controls.Add(this.button_SetLocal4);
            this.groupBox2.Controls.Add(this.textBox_num);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBox_port);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBox_ip);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.button_Set2);
            this.groupBox2.Controls.Add(this.textBox_port_baudrate);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_ip_portname);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_serviceid);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox_NFOINDEX);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(265, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(608, 299);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "通讯服务信息配置";
            // 
            // button_SetLocal2
            // 
            this.button_SetLocal2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_SetLocal2.Location = new System.Drawing.Point(437, 33);
            this.button_SetLocal2.Name = "button_SetLocal2";
            this.button_SetLocal2.Size = new System.Drawing.Size(75, 23);
            this.button_SetLocal2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_SetLocal2.TabIndex = 66;
            this.button_SetLocal2.Text = "配置本地";
            this.button_SetLocal2.Click += new System.EventHandler(this.button_SetLocal2_Click);
            // 
            // button_Syn2
            // 
            this.button_Syn2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Syn2.Location = new System.Drawing.Point(353, 33);
            this.button_Syn2.Name = "button_Syn2";
            this.button_Syn2.Size = new System.Drawing.Size(75, 23);
            this.button_Syn2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Syn2.TabIndex = 60;
            this.button_Syn2.Text = "从服务获取";
            this.button_Syn2.Tooltip = "通讯服务配置信息同步到本地<i>(如服务与本软件共用配置信息，本功能无效)</i>";
            this.button_Syn2.Click += new System.EventHandler(this.button_Syn2_Click);
            // 
            // button_SetLocal4
            // 
            this.button_SetLocal4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_SetLocal4.Location = new System.Drawing.Point(527, 265);
            this.button_SetLocal4.Name = "button_SetLocal4";
            this.button_SetLocal4.Size = new System.Drawing.Size(75, 23);
            this.button_SetLocal4.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_SetLocal4.TabIndex = 69;
            this.button_SetLocal4.Text = "配置本地";
            this.button_SetLocal4.Click += new System.EventHandler(this.button_SetLocal4_Click);
            // 
            // textBox_num
            // 
            this.textBox_num.Location = new System.Drawing.Point(245, 33);
            this.textBox_num.MaxLength = 20;
            this.textBox_num.Name = "textBox_num";
            this.textBox_num.Size = new System.Drawing.Size(100, 21);
            this.textBox_num.TabIndex = 65;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(186, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 64;
            this.label11.Text = "端口号：";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(410, 267);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 21);
            this.textBox_port.TabIndex = 62;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(351, 270);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 61;
            this.label9.Text = "端口号：";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(204, 265);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(100, 21);
            this.textBox_ip.TabIndex = 60;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 270);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 12);
            this.label10.TabIndex = 59;
            this.label10.Text = "向界面发送数据配置信息   IP地址：";
            // 
            // button_Set2
            // 
            this.button_Set2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Set2.Location = new System.Drawing.Point(523, 33);
            this.button_Set2.Name = "button_Set2";
            this.button_Set2.Size = new System.Drawing.Size(75, 23);
            this.button_Set2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Set2.TabIndex = 60;
            this.button_Set2.Text = "配置服务";
            this.button_Set2.Click += new System.EventHandler(this.button_Set2_Click);
            // 
            // textBox_port_baudrate
            // 
            this.textBox_port_baudrate.Location = new System.Drawing.Point(410, 65);
            this.textBox_port_baudrate.MaxLength = 6;
            this.textBox_port_baudrate.Name = "textBox_port_baudrate";
            this.textBox_port_baudrate.Size = new System.Drawing.Size(100, 21);
            this.textBox_port_baudrate.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "端口号：";
            // 
            // textBox_ip_portname
            // 
            this.textBox_ip_portname.Location = new System.Drawing.Point(245, 65);
            this.textBox_ip_portname.Name = "textBox_ip_portname";
            this.textBox_ip_portname.Size = new System.Drawing.Size(100, 21);
            this.textBox_ip_portname.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP地址：";
            // 
            // textBox_serviceid
            // 
            this.textBox_serviceid.Location = new System.Drawing.Point(78, 65);
            this.textBox_serviceid.MaxLength = 20;
            this.textBox_serviceid.Name = "textBox_serviceid";
            this.textBox_serviceid.Size = new System.Drawing.Size(100, 21);
            this.textBox_serviceid.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "服务ID：";
            // 
            // comboBox_NFOINDEX
            // 
            this.comboBox_NFOINDEX.FormattingEnabled = true;
            this.comboBox_NFOINDEX.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "GSM",
            "COM"});
            this.comboBox_NFOINDEX.Location = new System.Drawing.Point(78, 33);
            this.comboBox_NFOINDEX.Name = "comboBox_NFOINDEX";
            this.comboBox_NFOINDEX.Size = new System.Drawing.Size(100, 20);
            this.comboBox_NFOINDEX.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "信道类型：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridView1.Location = new System.Drawing.Point(6, 95);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(592, 158);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // button_Set3
            // 
            this.button_Set3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Set3.Location = new System.Drawing.Point(429, 16);
            this.button_Set3.Name = "button_Set3";
            this.button_Set3.Size = new System.Drawing.Size(75, 23);
            this.button_Set3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Set3.TabIndex = 60;
            this.button_Set3.Text = "配置服务";
            this.button_Set3.Click += new System.EventHandler(this.button_Set3_Click);
            // 
            // button_Syn3
            // 
            this.button_Syn3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Syn3.Location = new System.Drawing.Point(269, 16);
            this.button_Syn3.Name = "button_Syn3";
            this.button_Syn3.Size = new System.Drawing.Size(75, 23);
            this.button_Syn3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Syn3.TabIndex = 70;
            this.button_Syn3.Text = "从服务获取";
            this.button_Syn3.Tooltip = "协议信息同步到本地<i>(如服务与本软件共用配置信息，本功能无效)</i>";
            this.button_Syn3.Click += new System.EventHandler(this.button_Syn3_Click);
            // 
            // comboBox_Protocol
            // 
            this.comboBox_Protocol.FormattingEnabled = true;
            this.comboBox_Protocol.Items.AddRange(new object[] {
            "其它协议",
            "水文协议",
            "水资源协议",
            "正大212",
            "爱德佳创"});
            this.comboBox_Protocol.Location = new System.Drawing.Point(78, 19);
            this.comboBox_Protocol.Name = "comboBox_Protocol";
            this.comboBox_Protocol.Size = new System.Drawing.Size(116, 20);
            this.comboBox_Protocol.TabIndex = 68;
            this.comboBox_Protocol.SelectedIndexChanged += new System.EventHandler(this.comboBox_Protocol_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 67;
            this.label12.Text = "协议类型：";
            // 
            // button_Restar
            // 
            this.button_Restar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_Restar.Location = new System.Drawing.Point(788, 326);
            this.button_Restar.Name = "button_Restar";
            this.button_Restar.Size = new System.Drawing.Size(75, 23);
            this.button_Restar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_Restar.TabIndex = 66;
            this.button_Restar.Text = "重 启";
            this.button_Restar.Tooltip = "修改数据后请重启通讯服务软件";
            this.button_Restar.Click += new System.EventHandler(this.button_Restar_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonX_OK);
            this.groupBox3.Controls.Add(this.textBox_SYSPassWord);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox_SYSUserName);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(11, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 137);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "登录信息配置";
            // 
            // buttonX_OK
            // 
            this.buttonX_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_OK.Location = new System.Drawing.Point(146, 97);
            this.buttonX_OK.Name = "buttonX_OK";
            this.buttonX_OK.Size = new System.Drawing.Size(75, 23);
            this.buttonX_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.buttonX_OK.TabIndex = 61;
            this.buttonX_OK.Text = "确 认";
            this.buttonX_OK.Click += new System.EventHandler(this.buttonX_OK_Click);
            // 
            // textBox_SYSPassWord
            // 
            this.textBox_SYSPassWord.Location = new System.Drawing.Point(95, 64);
            this.textBox_SYSPassWord.MaxLength = 20;
            this.textBox_SYSPassWord.Name = "textBox_SYSPassWord";
            this.textBox_SYSPassWord.PasswordChar = '*';
            this.textBox_SYSPassWord.Size = new System.Drawing.Size(126, 21);
            this.textBox_SYSPassWord.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "当前密码：";
            // 
            // textBox_SYSUserName
            // 
            this.textBox_SYSUserName.Location = new System.Drawing.Point(95, 29);
            this.textBox_SYSUserName.MaxLength = 20;
            this.textBox_SYSUserName.Name = "textBox_SYSUserName";
            this.textBox_SYSUserName.Size = new System.Drawing.Size(126, 21);
            this.textBox_SYSUserName.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 32);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 2;
            this.label15.Text = "当前用户名：";
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_SetLocal3);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.button_Syn3);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.button_Set3);
            this.groupBox4.Controls.Add(this.comboBox_Protocol);
            this.groupBox4.Location = new System.Drawing.Point(265, 310);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(510, 52);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "协议选择";
            // 
            // button_SetLocal3
            // 
            this.button_SetLocal3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.button_SetLocal3.Location = new System.Drawing.Point(349, 16);
            this.button_SetLocal3.Name = "button_SetLocal3";
            this.button_SetLocal3.Size = new System.Drawing.Size(75, 23);
            this.button_SetLocal3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.button_SetLocal3.TabIndex = 72;
            this.button_SetLocal3.Text = "配置本地";
            this.button_SetLocal3.Click += new System.EventHandler(this.button_SetLocal3_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(200, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 71;
            this.label16.Text = "编码：";
            // 
            // SetSystemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_Restar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetSystemControl";
            this.Size = new System.Drawing.Size(901, 416);
            this.Load += new System.EventHandler(this.SetSystemControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_port_baudrate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_ip_portname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_serviceid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_NFOINDEX;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridView1;
        public DevComponents.DotNetBar.ButtonX button_Set1;
        private System.Windows.Forms.TextBox textBox_PassWord;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_DataBase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Source;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.ButtonX button_Set3;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label10;
        public DevComponents.DotNetBar.ButtonX button_Set2;
        private System.Windows.Forms.TextBox textBox_num;
        private System.Windows.Forms.Label label11;
        private DevComponents.DotNetBar.ButtonX button_Syn1;
        private DevComponents.DotNetBar.ButtonX button_Syn2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        public DevComponents.DotNetBar.ButtonX button_Restar;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.ButtonX buttonX_OK;
        private System.Windows.Forms.TextBox textBox_SYSPassWord;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_SYSUserName;
        private System.Windows.Forms.Label label15;
        private DevComponents.DotNetBar.ButtonX button_Syn3;
        public DevComponents.DotNetBar.ButtonX button_SetLocal4;
        private System.Windows.Forms.ComboBox comboBox_Protocol;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBox_DBType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private DevComponents.DotNetBar.ButtonX button_SetLocal1;
        private DevComponents.DotNetBar.ButtonX button_SetLocal2;
        private DevComponents.DotNetBar.ButtonX button_SetLocal3;
    }
}
