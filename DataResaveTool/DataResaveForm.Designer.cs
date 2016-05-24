namespace DataResaveTool
{
    partial class DataResaveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Test = new System.Windows.Forms.Button();
            this.comboBox_DBType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_PassWord = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_DataBase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Source = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox_Decimal = new System.Windows.Forms.ComboBox();
            this.checkBox_8 = new System.Windows.Forms.CheckBox();
            this.checkBox_Where = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker_B = new System.Windows.Forms.DateTimePicker();
            this.comboBox_Minute = new System.Windows.Forms.ComboBox();
            this.checkBox_Rain = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBox_Format = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_PadLeft = new System.Windows.Forms.TextBox();
            this.radioB_Upd = new System.Windows.Forms.RadioButton();
            this.radioB_Add = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.listBox_Table = new System.Windows.Forms.ListBox();
            this.button_SetField = new System.Windows.Forms.Button();
            this.comboBox_Item = new System.Windows.Forms.ComboBox();
            this.listBox_Field = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Val = new System.Windows.Forms.TextBox();
            this.comboBox_items = new System.Windows.Forms.ComboBox();
            this.button_ClearXML = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_Left = new System.Windows.Forms.Button();
            this.button_Right = new System.Windows.Forms.Button();
            this.dataGridView_List = new System.Windows.Forms.DataGridView();
            this.textBox_Text = new System.Windows.Forms.TextBox();
            this.checkBox_Stage = new System.Windows.Forms.CheckBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_List)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Test);
            this.groupBox1.Controls.Add(this.comboBox_DBType);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox_PassWord);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox_UserName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_DataBase);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_Source);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(15, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 214);
            this.groupBox1.TabIndex = 78;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择转存数据库";
            // 
            // button_Test
            // 
            this.button_Test.Location = new System.Drawing.Point(95, 177);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(77, 23);
            this.button_Test.TabIndex = 78;
            this.button_Test.Text = "测试、保存";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // comboBox_DBType
            // 
            this.comboBox_DBType.FormattingEnabled = true;
            this.comboBox_DBType.Items.AddRange(new object[] {
            "MSSQL",
            "ORACLE"});
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
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(321, 104);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 75;
            this.label16.Text = "位";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(225, 104);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 74;
            this.label15.Text = "精确";
            // 
            // comboBox_Decimal
            // 
            this.comboBox_Decimal.FormattingEnabled = true;
            this.comboBox_Decimal.Items.AddRange(new object[] {
            "默认",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBox_Decimal.Location = new System.Drawing.Point(256, 101);
            this.comboBox_Decimal.Name = "comboBox_Decimal";
            this.comboBox_Decimal.Size = new System.Drawing.Size(59, 20);
            this.comboBox_Decimal.TabIndex = 73;
            // 
            // checkBox_8
            // 
            this.checkBox_8.AutoSize = true;
            this.checkBox_8.Location = new System.Drawing.Point(296, 19);
            this.checkBox_8.Name = "checkBox_8";
            this.checkBox_8.Size = new System.Drawing.Size(42, 16);
            this.checkBox_8.TabIndex = 72;
            this.checkBox_8.Text = "8位";
            this.checkBox_8.UseVisualStyleBackColor = true;
            // 
            // checkBox_Where
            // 
            this.checkBox_Where.AutoSize = true;
            this.checkBox_Where.Enabled = false;
            this.checkBox_Where.Location = new System.Drawing.Point(540, 18);
            this.checkBox_Where.Name = "checkBox_Where";
            this.checkBox_Where.Size = new System.Drawing.Size(48, 16);
            this.checkBox_Where.TabIndex = 71;
            this.checkBox_Where.Text = "条件";
            this.checkBox_Where.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dateTimePicker_B);
            this.groupBox2.Controls.Add(this.comboBox_Minute);
            this.groupBox2.Location = new System.Drawing.Point(276, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(593, 59);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "配置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(502, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 35;
            this.label1.Text = "分钟";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(338, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "转存周期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 31;
            this.label2.Text = "转存起始时间：";
            // 
            // dateTimePicker_B
            // 
            this.dateTimePicker_B.CustomFormat = "yyyy年MM月dd日 HH时mm分ss秒";
            this.dateTimePicker_B.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_B.Location = new System.Drawing.Point(105, 16);
            this.dateTimePicker_B.Name = "dateTimePicker_B";
            this.dateTimePicker_B.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_B.TabIndex = 32;
            // 
            // comboBox_Minute
            // 
            this.comboBox_Minute.FormattingEnabled = true;
            this.comboBox_Minute.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15"});
            this.comboBox_Minute.Location = new System.Drawing.Point(408, 19);
            this.comboBox_Minute.Name = "comboBox_Minute";
            this.comboBox_Minute.Size = new System.Drawing.Size(90, 20);
            this.comboBox_Minute.TabIndex = 34;
            // 
            // checkBox_Rain
            // 
            this.checkBox_Rain.AutoSize = true;
            this.checkBox_Rain.Enabled = false;
            this.checkBox_Rain.Location = new System.Drawing.Point(407, 75);
            this.checkBox_Rain.Name = "checkBox_Rain";
            this.checkBox_Rain.Size = new System.Drawing.Size(102, 16);
            this.checkBox_Rain.TabIndex = 84;
            this.checkBox_Rain.Text = "雨量为0不转存";
            this.checkBox_Rain.UseVisualStyleBackColor = true;
            this.checkBox_Rain.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 336);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 83;
            this.label14.Text = "执行次数：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 314);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 82;
            this.label12.Text = "本次";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 291);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 81;
            this.label11.Text = "统计";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.button_ClearXML);
            this.groupBox3.Controls.Add(this.button_OK);
            this.groupBox3.Controls.Add(this.button_Clear);
            this.groupBox3.Controls.Add(this.button_Left);
            this.groupBox3.Controls.Add(this.button_Right);
            this.groupBox3.Controls.Add(this.dataGridView_List);
            this.groupBox3.Controls.Add(this.textBox_Text);
            this.groupBox3.Location = new System.Drawing.Point(276, 89);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(665, 488);
            this.groupBox3.TabIndex = 80;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "转存操作";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox_Format);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.textBox_PadLeft);
            this.groupBox4.Controls.Add(this.radioB_Upd);
            this.groupBox4.Controls.Add(this.radioB_Add);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.listBox_Table);
            this.groupBox4.Controls.Add(this.button_SetField);
            this.groupBox4.Controls.Add(this.comboBox_Item);
            this.groupBox4.Controls.Add(this.listBox_Field);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBox_Val);
            this.groupBox4.Controls.Add(this.comboBox_items);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.checkBox_Where);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.checkBox_8);
            this.groupBox4.Controls.Add(this.comboBox_Decimal);
            this.groupBox4.Location = new System.Drawing.Point(17, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(642, 264);
            this.groupBox4.TabIndex = 83;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "添加/更新";
            // 
            // comboBox_Format
            // 
            this.comboBox_Format.FormattingEnabled = true;
            this.comboBox_Format.Items.AddRange(new object[] {
            "默认格式",
            "yyyy-MM-dd HH:mm:00",
            "yyyy-MM-dd HH:00:00",
            "yyyy-MM-dd 00:00:00"});
            this.comboBox_Format.Location = new System.Drawing.Point(227, 73);
            this.comboBox_Format.Name = "comboBox_Format";
            this.comboBox_Format.Size = new System.Drawing.Size(111, 20);
            this.comboBox_Format.TabIndex = 90;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(151, 131);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 12);
            this.label21.TabIndex = 89;
            this.label21.Text = "默认值：";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(151, 104);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 88;
            this.label20.Text = "值格式化：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(151, 76);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 12);
            this.label19.TabIndex = 86;
            this.label19.Text = "时间格式化：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(151, 49);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 85;
            this.label18.Text = "监测项目：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(151, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 84;
            this.label17.Text = "前补编码：";
            // 
            // textBox_PadLeft
            // 
            this.textBox_PadLeft.Location = new System.Drawing.Point(227, 18);
            this.textBox_PadLeft.MaxLength = 10;
            this.textBox_PadLeft.Name = "textBox_PadLeft";
            this.textBox_PadLeft.Size = new System.Drawing.Size(62, 21);
            this.textBox_PadLeft.TabIndex = 83;
            // 
            // radioB_Upd
            // 
            this.radioB_Upd.AutoSize = true;
            this.radioB_Upd.Location = new System.Drawing.Point(75, 21);
            this.radioB_Upd.Name = "radioB_Upd";
            this.radioB_Upd.Size = new System.Drawing.Size(47, 16);
            this.radioB_Upd.TabIndex = 82;
            this.radioB_Upd.Text = "更新";
            this.radioB_Upd.UseVisualStyleBackColor = true;
            // 
            // radioB_Add
            // 
            this.radioB_Add.AutoSize = true;
            this.radioB_Add.Checked = true;
            this.radioB_Add.Location = new System.Drawing.Point(22, 21);
            this.radioB_Add.Name = "radioB_Add";
            this.radioB_Add.Size = new System.Drawing.Size(47, 16);
            this.radioB_Add.TabIndex = 81;
            this.radioB_Add.TabStop = true;
            this.radioB_Add.Text = "添加";
            this.radioB_Add.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "表：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(179, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "字段：";
            // 
            // listBox_Table
            // 
            this.listBox_Table.FormattingEnabled = true;
            this.listBox_Table.ItemHeight = 12;
            this.listBox_Table.Location = new System.Drawing.Point(4, 172);
            this.listBox_Table.Name = "listBox_Table";
            this.listBox_Table.Size = new System.Drawing.Size(159, 88);
            this.listBox_Table.TabIndex = 39;
            this.listBox_Table.SelectedIndexChanged += new System.EventHandler(this.listBox_Table_SelectedIndexChanged);
            // 
            // button_SetField
            // 
            this.button_SetField.Location = new System.Drawing.Point(344, 188);
            this.button_SetField.Name = "button_SetField";
            this.button_SetField.Size = new System.Drawing.Size(31, 65);
            this.button_SetField.TabIndex = 80;
            this.button_SetField.Text = "配置列";
            this.button_SetField.UseVisualStyleBackColor = true;
            this.button_SetField.Click += new System.EventHandler(this.button_SetField_Click);
            // 
            // comboBox_Item
            // 
            this.comboBox_Item.FormattingEnabled = true;
            this.comboBox_Item.Items.AddRange(new object[] {
            "站    号",
            "元    素",
            "监测时间",
            "接收时间",
            "值",
            "其    他"});
            this.comboBox_Item.Location = new System.Drawing.Point(62, 43);
            this.comboBox_Item.Name = "comboBox_Item";
            this.comboBox_Item.Size = new System.Drawing.Size(80, 20);
            this.comboBox_Item.TabIndex = 30;
            this.comboBox_Item.SelectedIndexChanged += new System.EventHandler(this.comboBox_Item_SelectedIndexChanged);
            // 
            // listBox_Field
            // 
            this.listBox_Field.FormattingEnabled = true;
            this.listBox_Field.ItemHeight = 12;
            this.listBox_Field.Location = new System.Drawing.Point(180, 172);
            this.listBox_Field.Name = "listBox_Field";
            this.listBox_Field.Size = new System.Drawing.Size(158, 88);
            this.listBox_Field.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "字段：";
            // 
            // textBox_Val
            // 
            this.textBox_Val.Location = new System.Drawing.Point(227, 127);
            this.textBox_Val.Name = "textBox_Val";
            this.textBox_Val.Size = new System.Drawing.Size(111, 21);
            this.textBox_Val.TabIndex = 41;
            // 
            // comboBox_items
            // 
            this.comboBox_items.FormattingEnabled = true;
            this.comboBox_items.Location = new System.Drawing.Point(227, 44);
            this.comboBox_items.Name = "comboBox_items";
            this.comboBox_items.Size = new System.Drawing.Size(111, 20);
            this.comboBox_items.TabIndex = 42;
            // 
            // button_ClearXML
            // 
            this.button_ClearXML.Location = new System.Drawing.Point(553, 436);
            this.button_ClearXML.Name = "button_ClearXML";
            this.button_ClearXML.Size = new System.Drawing.Size(52, 23);
            this.button_ClearXML.TabIndex = 82;
            this.button_ClearXML.Text = "清空";
            this.button_ClearXML.UseVisualStyleBackColor = true;
            this.button_ClearXML.Click += new System.EventHandler(this.button_ClearXML_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(553, 407);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(52, 23);
            this.button_OK.TabIndex = 81;
            this.button_OK.Text = "保存";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(553, 348);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(52, 23);
            this.button_Clear.TabIndex = 79;
            this.button_Clear.Text = "清空";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_Left
            // 
            this.button_Left.Location = new System.Drawing.Point(554, 319);
            this.button_Left.Name = "button_Left";
            this.button_Left.Size = new System.Drawing.Size(52, 23);
            this.button_Left.TabIndex = 78;
            this.button_Left.Text = "<<";
            this.button_Left.UseVisualStyleBackColor = true;
            this.button_Left.Click += new System.EventHandler(this.button_Left_Click);
            // 
            // button_Right
            // 
            this.button_Right.Location = new System.Drawing.Point(553, 290);
            this.button_Right.Name = "button_Right";
            this.button_Right.Size = new System.Drawing.Size(52, 23);
            this.button_Right.TabIndex = 77;
            this.button_Right.Text = ">>";
            this.button_Right.UseVisualStyleBackColor = true;
            this.button_Right.Click += new System.EventHandler(this.button_Right_Click);
            // 
            // dataGridView_List
            // 
            this.dataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_List.Location = new System.Drawing.Point(17, 389);
            this.dataGridView_List.Name = "dataGridView_List";
            this.dataGridView_List.RowTemplate.Height = 23;
            this.dataGridView_List.Size = new System.Drawing.Size(515, 93);
            this.dataGridView_List.TabIndex = 76;
            // 
            // textBox_Text
            // 
            this.textBox_Text.Location = new System.Drawing.Point(17, 286);
            this.textBox_Text.Multiline = true;
            this.textBox_Text.Name = "textBox_Text";
            this.textBox_Text.Size = new System.Drawing.Size(515, 96);
            this.textBox_Text.TabIndex = 43;
            // 
            // checkBox_Stage
            // 
            this.checkBox_Stage.AutoSize = true;
            this.checkBox_Stage.Enabled = false;
            this.checkBox_Stage.Location = new System.Drawing.Point(546, 74);
            this.checkBox_Stage.Name = "checkBox_Stage";
            this.checkBox_Stage.Size = new System.Drawing.Size(102, 16);
            this.checkBox_Stage.TabIndex = 85;
            this.checkBox_Stage.Text = "水位为0不转存";
            this.checkBox_Stage.UseVisualStyleBackColor = true;
            this.checkBox_Stage.Visible = false;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(110, 239);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(52, 23);
            this.button_Start.TabIndex = 83;
            this.button_Start.Text = "启动";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // DataResaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 589);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkBox_Rain);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.checkBox_Stage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataResaveForm";
            this.Text = "DataResaveForm";
            this.Load += new System.EventHandler(this.DataResaveForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_List)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_DBType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_PassWord;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_DataBase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Source;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBox_Decimal;
        private System.Windows.Forms.CheckBox checkBox_8;
        private System.Windows.Forms.CheckBox checkBox_Where;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker_B;
        private System.Windows.Forms.ComboBox comboBox_Minute;
        private System.Windows.Forms.CheckBox checkBox_Rain;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_Text;
        private System.Windows.Forms.ComboBox comboBox_items;
        private System.Windows.Forms.TextBox textBox_Val;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox_Field;
        private System.Windows.Forms.ComboBox comboBox_Item;
        private System.Windows.Forms.ListBox listBox_Table;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_Stage;
        private System.Windows.Forms.DataGridView dataGridView_List;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_SetField;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_Left;
        private System.Windows.Forms.Button button_Right;
        private System.Windows.Forms.Button button_ClearXML;
        private System.Windows.Forms.Button button_Test;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioB_Upd;
        private System.Windows.Forms.RadioButton radioB_Add;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_PadLeft;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox comboBox_Format;
    }
}