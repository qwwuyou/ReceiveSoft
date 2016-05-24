namespace YYApp
{
    partial class CommandResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandResultForm));
            this.richTextBoxEx_bottom = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // richTextBoxEx_bottom
            // 
            this.richTextBoxEx_bottom.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.richTextBoxEx_bottom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.richTextBoxEx_bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxEx_bottom.ForeColor = System.Drawing.Color.Black;
            this.richTextBoxEx_bottom.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxEx_bottom.Name = "richTextBoxEx_bottom";
            this.richTextBoxEx_bottom.ReadOnly = true;
            this.richTextBoxEx_bottom.Size = new System.Drawing.Size(246, 119);
            this.richTextBoxEx_bottom.TabIndex = 3;
            // 
            // CommandResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 119);
            this.Controls.Add(this.richTextBoxEx_bottom);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandResultForm";
            this.Text = "命令结果窗口";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommandResultForm_FormClosing);
            this.Load += new System.EventHandler(this.CommandResultForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.DotNetBar.Controls.RichTextBoxEx richTextBoxEx_bottom;
    }
}