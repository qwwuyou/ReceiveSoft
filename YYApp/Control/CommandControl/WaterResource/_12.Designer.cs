namespace YYApp.CommandControl
{
    partial class _12
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
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.rb6 = new System.Windows.Forms.RadioButton();
            this.rb5 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(20, 115);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(155, 16);
            this.rb3.TabIndex = 5;
            this.rb3.TabStop = true;
            this.rb3.Text = "查询/应答工作模式-(02)";
            this.rb3.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(20, 69);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(125, 16);
            this.rb2.TabIndex = 4;
            this.rb2.TabStop = true;
            this.rb2.Text = "自报工作模式-(01)";
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(20, 30);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(125, 16);
            this.rb1.TabIndex = 3;
            this.rb1.TabStop = true;
            this.rb1.Text = "兼容工作模式-(00)";
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // rb4
            // 
            this.rb4.AutoSize = true;
            this.rb4.Location = new System.Drawing.Point(20, 158);
            this.rb4.Name = "rb4";
            this.rb4.Size = new System.Drawing.Size(155, 16);
            this.rb4.TabIndex = 6;
            this.rb4.TabStop = true;
            this.rb4.Text = "调试/维修工作模式-(03)";
            this.rb4.UseVisualStyleBackColor = true;
            // 
            // rb6
            // 
            this.rb6.AutoSize = true;
            this.rb6.Location = new System.Drawing.Point(120, 35);
            this.rb6.Name = "rb6";
            this.rb6.Size = new System.Drawing.Size(47, 16);
            this.rb6.TabIndex = 11;
            this.rb6.TabStop = true;
            this.rb6.Text = "查询";
            this.rb6.UseVisualStyleBackColor = true;
            this.rb6.CheckedChanged += new System.EventHandler(this.rb6_CheckedChanged);
            // 
            // rb5
            // 
            this.rb5.AutoSize = true;
            this.rb5.Checked = true;
            this.rb5.Location = new System.Drawing.Point(25, 35);
            this.rb5.Name = "rb5";
            this.rb5.Size = new System.Drawing.Size(47, 16);
            this.rb5.TabIndex = 10;
            this.rb5.TabStop = true;
            this.rb5.Text = "设置";
            this.rb5.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb3);
            this.groupBox1.Controls.Add(this.rb1);
            this.groupBox1.Controls.Add(this.rb2);
            this.groupBox1.Controls.Add(this.rb4);
            this.groupBox1.Location = new System.Drawing.Point(3, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 202);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // _12
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rb6);
            this.Controls.Add(this.rb5);
            this.Name = "_12";
            this.Size = new System.Drawing.Size(301, 428);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.RadioButton rb6;
        private System.Windows.Forms.RadioButton rb5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
