namespace ConfigRationTool
{
    partial class ConfigRationForm
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
            this.comboBox_Item = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Set_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_db = new System.Windows.Forms.ComboBox();
            this.Conn_button = new System.Windows.Forms.Button();
            this.Conn_label = new System.Windows.Forms.Label();
            this.checkedListBox_config = new System.Windows.Forms.CheckedListBox();
            this.button_Del = new System.Windows.Forms.Button();
            this.button_DelAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox_Item
            // 
            this.comboBox_Item.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Item.FormattingEnabled = true;
            this.comboBox_Item.Location = new System.Drawing.Point(72, 50);
            this.comboBox_Item.Name = "comboBox_Item";
            this.comboBox_Item.Size = new System.Drawing.Size(218, 20);
            this.comboBox_Item.TabIndex = 0;
            this.comboBox_Item.SelectedIndexChanged += new System.EventHandler(this.comboBox_Item_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "监测项：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "配置项：";
            // 
            // Set_button
            // 
            this.Set_button.Location = new System.Drawing.Point(630, 14);
            this.Set_button.Name = "Set_button";
            this.Set_button.Size = new System.Drawing.Size(105, 23);
            this.Set_button.TabIndex = 4;
            this.Set_button.Text = "配 置";
            this.Set_button.UseVisualStyleBackColor = true;
            this.Set_button.Click += new System.EventHandler(this.Set_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "数据库：";
            // 
            // comboBox_db
            // 
            this.comboBox_db.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_db.FormattingEnabled = true;
            this.comboBox_db.Items.AddRange(new object[] {
            "MSSQL",
            "MYSQL"});
            this.comboBox_db.Location = new System.Drawing.Point(72, 17);
            this.comboBox_db.Name = "comboBox_db";
            this.comboBox_db.Size = new System.Drawing.Size(72, 20);
            this.comboBox_db.TabIndex = 5;
            // 
            // Conn_button
            // 
            this.Conn_button.Location = new System.Drawing.Point(191, 15);
            this.Conn_button.Name = "Conn_button";
            this.Conn_button.Size = new System.Drawing.Size(57, 23);
            this.Conn_button.TabIndex = 7;
            this.Conn_button.Text = "连 接";
            this.Conn_button.UseVisualStyleBackColor = true;
            this.Conn_button.Click += new System.EventHandler(this.Conn_button_Click);
            // 
            // Conn_label
            // 
            this.Conn_label.AutoSize = true;
            this.Conn_label.Location = new System.Drawing.Point(254, 20);
            this.Conn_label.Name = "Conn_label";
            this.Conn_label.Size = new System.Drawing.Size(0, 12);
            this.Conn_label.TabIndex = 8;
            // 
            // checkedListBox_config
            // 
            this.checkedListBox_config.FormattingEnabled = true;
            this.checkedListBox_config.Location = new System.Drawing.Point(364, 17);
            this.checkedListBox_config.Name = "checkedListBox_config";
            this.checkedListBox_config.Size = new System.Drawing.Size(232, 244);
            this.checkedListBox_config.TabIndex = 9;
            // 
            // button_Del
            // 
            this.button_Del.Location = new System.Drawing.Point(630, 53);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(105, 23);
            this.button_Del.TabIndex = 10;
            this.button_Del.Text = "取消当前项配置";
            this.button_Del.UseVisualStyleBackColor = true;
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // button_DelAll
            // 
            this.button_DelAll.Location = new System.Drawing.Point(630, 95);
            this.button_DelAll.Name = "button_DelAll";
            this.button_DelAll.Size = new System.Drawing.Size(105, 23);
            this.button_DelAll.TabIndex = 11;
            this.button_DelAll.Text = "取消所有配置";
            this.button_DelAll.UseVisualStyleBackColor = true;
            this.button_DelAll.Click += new System.EventHandler(this.button_DelAll_Click);
            // 
            // ConfigRationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 283);
            this.Controls.Add(this.button_DelAll);
            this.Controls.Add(this.button_Del);
            this.Controls.Add(this.checkedListBox_config);
            this.Controls.Add(this.Conn_label);
            this.Controls.Add(this.Conn_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_db);
            this.Controls.Add(this.Set_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_Item);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigRationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "监测项配置工具";
            this.Load += new System.EventHandler(this.ConfigRationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Item;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Set_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_db;
        private System.Windows.Forms.Button Conn_button;
        private System.Windows.Forms.Label Conn_label;
        private System.Windows.Forms.CheckedListBox checkedListBox_config;
        private System.Windows.Forms.Button button_Del;
        private System.Windows.Forms.Button button_DelAll;
    }
}

