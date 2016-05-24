namespace YYApp.CommandControl
{
    partial class _103
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radTX_B = new System.Windows.Forms.RadioButton();
            this.radTX_A = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 167);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radTX_B);
            this.groupBox2.Controls.Add(this.radTX_A);
            this.groupBox2.Location = new System.Drawing.Point(8, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 44);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "回复方式";
            // 
            // radTX_B
            // 
            this.radTX_B.AutoSize = true;
            this.radTX_B.Location = new System.Drawing.Point(109, 19);
            this.radTX_B.Name = "radTX_B";
            this.radTX_B.Size = new System.Drawing.Size(47, 16);
            this.radTX_B.TabIndex = 3;
            this.radTX_B.Text = "GPRS";
            this.radTX_B.UseVisualStyleBackColor = true;
            // 
            // radTX_A
            // 
            this.radTX_A.AutoSize = true;
            this.radTX_A.Checked = true;
            this.radTX_A.Location = new System.Drawing.Point(10, 19);
            this.radTX_A.Name = "radTX_A";
            this.radTX_A.Size = new System.Drawing.Size(47, 16);
            this.radTX_A.TabIndex = 2;
            this.radTX_A.TabStop = true;
            this.radTX_A.Text = "短信";
            this.radTX_A.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "提取当前数据";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(109, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 60;
            this.label13.Text = "短信信道时设置";
            // 
            // _103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "_103";
            this.Size = new System.Drawing.Size(235, 227);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radTX_B;
        private System.Windows.Forms.RadioButton radTX_A;
        private System.Windows.Forms.Label label13;
    }
}
