namespace YYApp.CommandControl
{
    partial class _4F
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
            this.label8 = new System.Windows.Forms.Label();
            this.RB_close = new System.Windows.Forms.RadioButton();
            this.RB_open = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 37;
            this.label8.Text = "水量定值控制命令：";
            // 
            // RB_close
            // 
            this.RB_close.AutoSize = true;
            this.RB_close.Checked = true;
            this.RB_close.Location = new System.Drawing.Point(80, 35);
            this.RB_close.Name = "RB_close";
            this.RB_close.Size = new System.Drawing.Size(47, 16);
            this.RB_close.TabIndex = 39;
            this.RB_close.TabStop = true;
            this.RB_close.Text = "退出";
            this.RB_close.UseVisualStyleBackColor = true;
            // 
            // RB_open
            // 
            this.RB_open.AutoSize = true;
            this.RB_open.Location = new System.Drawing.Point(14, 35);
            this.RB_open.Name = "RB_open";
            this.RB_open.Size = new System.Drawing.Size(47, 16);
            this.RB_open.TabIndex = 38;
            this.RB_open.Text = "投入";
            this.RB_open.UseVisualStyleBackColor = true;
            // 
            // _4F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RB_close);
            this.Controls.Add(this.RB_open);
            this.Name = "_4F";
            this.Size = new System.Drawing.Size(310, 357);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton RB_close;
        private System.Windows.Forms.RadioButton RB_open;
    }
}
