namespace YYApp.SetControl
{
    partial class ShowCommandControl
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
            this.contextMenuStrip_Right = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Syn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_DelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader3 = new DevComponents.AdvTree.ColumnHeader();
            this.elementStyle3 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle4 = new DevComponents.DotNetBar.ElementStyle();
            this.contextMenuStrip_Right.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Right
            // 
            this.contextMenuStrip_Right.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Del,
            this.toolStripMenuItem_Syn,
            this.toolStripMenuItem_DelAll});
            this.contextMenuStrip_Right.Name = "contextMenuStrip1";
            this.contextMenuStrip_Right.Size = new System.Drawing.Size(125, 70);
            this.contextMenuStrip_Right.Text = "111";
            // 
            // toolStripMenuItem_Del
            // 
            this.toolStripMenuItem_Del.Name = "toolStripMenuItem_Del";
            this.toolStripMenuItem_Del.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Del.Text = "删除";
            this.toolStripMenuItem_Del.Click += new System.EventHandler(this.toolStripMenuItem_Del_Click);
            // 
            // toolStripMenuItem_Syn
            // 
            this.toolStripMenuItem_Syn.Name = "toolStripMenuItem_Syn";
            this.toolStripMenuItem_Syn.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Syn.Text = "同步";
            this.toolStripMenuItem_Syn.Click += new System.EventHandler(this.toolStripMenuItem_Syn_Click);
            // 
            // toolStripMenuItem_DelAll
            // 
            this.toolStripMenuItem_DelAll.Name = "toolStripMenuItem_DelAll";
            this.toolStripMenuItem_DelAll.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_DelAll.Text = "全部删除";
            this.toolStripMenuItem_DelAll.Click += new System.EventHandler(this.toolStripMenuItem_DelAll_Click);
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.Columns.Add(this.columnHeader1);
            this.advTree1.Columns.Add(this.columnHeader2);
            this.advTree1.Columns.Add(this.columnHeader3);
            this.advTree1.ColumnsBackgroundStyle = this.elementStyle3;
            this.advTree1.ColumnStyleNormal = this.elementStyle2;
            this.advTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree1.DragDropEnabled = false;
            this.advTree1.DragDropNodeCopyEnabled = false;
            this.advTree1.HotTracking = true;
            this.advTree1.Location = new System.Drawing.Point(0, 0);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.NodeStyleMouseOver = this.elementStyle4;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(250, 233);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.Styles.Add(this.elementStyle2);
            this.advTree1.Styles.Add(this.elementStyle3);
            this.advTree1.Styles.Add(this.elementStyle4);
            this.advTree1.TabIndex = 1;
            this.advTree1.Text = "advTree1";
            this.advTree1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.advTree1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "测站";
            this.columnHeader1.Width.Absolute = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "命令";
            this.columnHeader2.Width.Absolute = 160;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.Text = "时间";
            this.columnHeader3.Width.Absolute = 130;
            // 
            // elementStyle3
            // 
            this.elementStyle3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.elementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle3.Name = "elementStyle3";
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle4
            // 
            this.elementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle4.Name = "elementStyle4";
            // 
            // ShowCommandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.advTree1);
            this.Name = "ShowCommandControl";
            this.Size = new System.Drawing.Size(250, 233);
            this.Load += new System.EventHandler(this.ShowCommandControl_Load);
            this.contextMenuStrip_Right.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Right;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Del;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Syn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_DelAll;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
        private DevComponents.AdvTree.ColumnHeader columnHeader3;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.DotNetBar.ElementStyle elementStyle3;
        private DevComponents.DotNetBar.ElementStyle elementStyle4;
    }
}
