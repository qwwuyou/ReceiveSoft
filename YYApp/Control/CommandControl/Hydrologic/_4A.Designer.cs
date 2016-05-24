namespace YYApp.CommandControl
{
    partial class _4A
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
            this.dateTimePicker_B = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dateTimePicker_B
            // 
            this.dateTimePicker_B.CustomFormat = "yyyy年MM月dd日 HH时mm分ss秒";
            this.dateTimePicker_B.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_B.Location = new System.Drawing.Point(51, 12);
            this.dateTimePicker_B.Name = "dateTimePicker_B";
            this.dateTimePicker_B.Size = new System.Drawing.Size(168, 21);
            this.dateTimePicker_B.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 30;
            this.label2.Text = "测站时间：";
            // 
            // _4A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dateTimePicker_B);
            this.Controls.Add(this.label2);
            this.Name = "_4A";
            this.Size = new System.Drawing.Size(292, 275);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker_B;
        private System.Windows.Forms.Label label2;
    }
}
