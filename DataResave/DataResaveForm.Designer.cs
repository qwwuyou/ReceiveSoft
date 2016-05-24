namespace DataResave
{
    partial class DataResaveForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
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
            this.comboBox_Item = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_B = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_Minute = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox_Table = new System.Windows.Forms.ListBox();
            this.listBox_Field = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_Left = new System.Windows.Forms.Button();
            this.button_ClearXML = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_SetField = new System.Windows.Forms.Button();
            this.dataGridView_List = new System.Windows.Forms.DataGridView();
            this.button_Right = new System.Windows.Forms.Button();
            this.textBox_Text = new System.Windows.Forms.TextBox();
            this.comboBox_items = new System.Windows.Forms.ComboBox();
            this.textBox_Val = new System.Windows.Forms.TextBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 214);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择转存数据库";
            // 
            // button_Test
            // 
            this.button_Test.Location = new System.Drawing.Point(70, 185);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(105, 23);
            this.button_Test.TabIndex = 2;
            this.button_Test.Text = "测试、保存";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // comboBox_DBType
            // 
            this.comboBox_DBType.FormattingEnabled = true;
            this.comboBox_DBType.Items.AddRange(new object[] {
            "MSSQL"});
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
            this.comboBox_Item.Location = new System.Drawing.Point(77, 34);
            this.comboBox_Item.Name = "comboBox_Item";
            this.comboBox_Item.Size = new System.Drawing.Size(80, 20);
            this.comboBox_Item.TabIndex = 30;
            this.comboBox_Item.SelectedIndexChanged += new System.EventHandler(this.comboBox_Item_SelectedIndexChanged);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 31;
            this.label2.Text = "转存起始时间：";
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
            // comboBox_Minute
            // 
            this.comboBox_Minute.FormattingEnabled = true;
            this.comboBox_Minute.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.comboBox_Minute.Location = new System.Drawing.Point(408, 19);
            this.comboBox_Minute.Name = "comboBox_Minute";
            this.comboBox_Minute.Size = new System.Drawing.Size(90, 20);
            this.comboBox_Minute.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "已有项：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(193, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "表：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(371, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "字段：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dateTimePicker_B);
            this.groupBox2.Controls.Add(this.comboBox_Minute);
            this.groupBox2.Location = new System.Drawing.Point(275, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 59);
            this.groupBox2.TabIndex = 38;
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
            // listBox_Table
            // 
            this.listBox_Table.FormattingEnabled = true;
            this.listBox_Table.ItemHeight = 12;
            this.listBox_Table.Location = new System.Drawing.Point(195, 34);
            this.listBox_Table.Name = "listBox_Table";
            this.listBox_Table.Size = new System.Drawing.Size(148, 88);
            this.listBox_Table.TabIndex = 39;
            this.listBox_Table.SelectedIndexChanged += new System.EventHandler(this.listBox_Table_SelectedIndexChanged);
            // 
            // listBox_Field
            // 
            this.listBox_Field.FormattingEnabled = true;
            this.listBox_Field.ItemHeight = 12;
            this.listBox_Field.Location = new System.Drawing.Point(373, 34);
            this.listBox_Field.Name = "listBox_Field";
            this.listBox_Field.Size = new System.Drawing.Size(152, 88);
            this.listBox_Field.TabIndex = 40;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_Left);
            this.groupBox3.Controls.Add(this.button_ClearXML);
            this.groupBox3.Controls.Add(this.button_OK);
            this.groupBox3.Controls.Add(this.button_Clear);
            this.groupBox3.Controls.Add(this.button_SetField);
            this.groupBox3.Controls.Add(this.dataGridView_List);
            this.groupBox3.Controls.Add(this.button_Right);
            this.groupBox3.Controls.Add(this.textBox_Text);
            this.groupBox3.Controls.Add(this.comboBox_items);
            this.groupBox3.Controls.Add(this.textBox_Val);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.listBox_Field);
            this.groupBox3.Controls.Add(this.comboBox_Item);
            this.groupBox3.Controls.Add(this.listBox_Table);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(275, 87);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(598, 278);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "转存字段对应";
            // 
            // button_Left
            // 
            this.button_Left.Location = new System.Drawing.Point(251, 181);
            this.button_Left.Name = "button_Left";
            this.button_Left.Size = new System.Drawing.Size(48, 23);
            this.button_Left.TabIndex = 70;
            this.button_Left.Text = "<<";
            this.button_Left.UseVisualStyleBackColor = true;
            this.button_Left.Click += new System.EventHandler(this.button_Left_Click);
            // 
            // button_ClearXML
            // 
            this.button_ClearXML.Location = new System.Drawing.Point(547, 181);
            this.button_ClearXML.Name = "button_ClearXML";
            this.button_ClearXML.Size = new System.Drawing.Size(48, 23);
            this.button_ClearXML.TabIndex = 69;
            this.button_ClearXML.Text = "清空";
            this.button_ClearXML.UseVisualStyleBackColor = true;
            this.button_ClearXML.Click += new System.EventHandler(this.button_ClearXML_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(547, 135);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(48, 23);
            this.button_OK.TabIndex = 68;
            this.button_OK.Text = "保存";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(251, 230);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(48, 23);
            this.button_Clear.TabIndex = 67;
            this.button_Clear.Text = "清空";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_SetField
            // 
            this.button_SetField.Location = new System.Drawing.Point(531, 40);
            this.button_SetField.Name = "button_SetField";
            this.button_SetField.Size = new System.Drawing.Size(31, 58);
            this.button_SetField.TabIndex = 66;
            this.button_SetField.Text = "配置列";
            this.button_SetField.UseVisualStyleBackColor = true;
            this.button_SetField.Click += new System.EventHandler(this.button_SetField_Click);
            // 
            // dataGridView_List
            // 
            this.dataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_List.Location = new System.Drawing.Point(316, 135);
            this.dataGridView_List.Name = "dataGridView_List";
            this.dataGridView_List.RowTemplate.Height = 23;
            this.dataGridView_List.Size = new System.Drawing.Size(231, 137);
            this.dataGridView_List.TabIndex = 65;
            // 
            // button_Right
            // 
            this.button_Right.Location = new System.Drawing.Point(251, 135);
            this.button_Right.Name = "button_Right";
            this.button_Right.Size = new System.Drawing.Size(48, 23);
            this.button_Right.TabIndex = 64;
            this.button_Right.Text = ">>";
            this.button_Right.UseVisualStyleBackColor = true;
            this.button_Right.Click += new System.EventHandler(this.button_Right_Click);
            // 
            // textBox_Text
            // 
            this.textBox_Text.Location = new System.Drawing.Point(31, 135);
            this.textBox_Text.Multiline = true;
            this.textBox_Text.Name = "textBox_Text";
            this.textBox_Text.Size = new System.Drawing.Size(205, 137);
            this.textBox_Text.TabIndex = 43;
            // 
            // comboBox_items
            // 
            this.comboBox_items.FormattingEnabled = true;
            this.comboBox_items.Location = new System.Drawing.Point(31, 60);
            this.comboBox_items.Name = "comboBox_items";
            this.comboBox_items.Size = new System.Drawing.Size(126, 20);
            this.comboBox_items.TabIndex = 42;
            // 
            // textBox_Val
            // 
            this.textBox_Val.Location = new System.Drawing.Point(31, 91);
            this.textBox_Val.Name = "textBox_Val";
            this.textBox_Val.Size = new System.Drawing.Size(126, 21);
            this.textBox_Val.TabIndex = 41;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(93, 253);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 42;
            this.button_Start.Text = "启动";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(24, 325);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 63;
            this.label12.Text = "已有项：";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DataResaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 377);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DataResaveForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataResaveForm_FormClosing);
            this.Load += new System.EventHandler(this.DataResaveForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_List)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Test;
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
        private System.Windows.Forms.ComboBox comboBox_Item;
        private System.Windows.Forms.DateTimePicker dateTimePicker_B;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_Minute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox_Table;
        private System.Windows.Forms.ListBox listBox_Field;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox_items;
        private System.Windows.Forms.TextBox textBox_Val;
        private System.Windows.Forms.Button button_SetField;
        private System.Windows.Forms.DataGridView dataGridView_List;
        private System.Windows.Forms.Button button_Right;
        private System.Windows.Forms.TextBox textBox_Text;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_ClearXML;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_Left;
    }
}

