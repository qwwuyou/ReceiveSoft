namespace YYApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroAppButton1 = new DevComponents.DotNetBar.Metro.MetroAppButton();
            this.buttonItem_Login = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_dataShow = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_ReBootService = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_ReadRTU = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SetSystem = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_Log = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SendMail = new DevComponents.DotNetBar.ButtonItem();
            this.metroAppButton2 = new DevComponents.DotNetBar.Metro.MetroAppButton();
            this.buttonItem_SetCommand = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem6 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SetRTU = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SetCenter = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SetRTUWork = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_SetConfigData = new DevComponents.DotNetBar.ButtonItem();
            this.metroAppButton3 = new DevComponents.DotNetBar.Metro.MetroAppButton();
            this.buttonItem_RealTime = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_Rem = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_Img = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_Manual = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_CmdState = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem_DataResave = new DevComponents.DotNetBar.ButtonItem();
            this.metroTabItem_Exit = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.itemPanel_top = new DevComponents.DotNetBar.ItemPanel();
            this.pageSlider1 = new DevComponents.DotNetBar.Controls.PageSlider();
            this.pageSliderPage1 = new DevComponents.DotNetBar.Controls.PageSliderPage();
            this.pageSliderPage2 = new DevComponents.DotNetBar.Controls.PageSliderPage();
            this.pageSliderPage3 = new DevComponents.DotNetBar.Controls.PageSliderPage();
            this.pageSliderPage4 = new DevComponents.DotNetBar.Controls.PageSliderPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip_COM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_GetState = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_SetTime = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSlider1.SuspendLayout();
            this.contextMenuStrip_COM.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = "HELP";
            this.metroShell1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroAppButton1,
            this.metroAppButton2,
            this.metroAppButton3,
            this.metroTabItem_Exit});
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(1, 1);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.metroShell1.SettingsButtonText = "ABOUT";
            this.metroShell1.Size = new System.Drawing.Size(1034, 62);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.metroShell1.Text = "metroShell1";
            this.metroShell1.SettingsButtonClick += new System.EventHandler(this.metroShell1_SettingsButtonClick);
            this.metroShell1.HelpButtonClick += new System.EventHandler(this.metroShell1_HelpButtonClick);
            // 
            // metroAppButton1
            // 
            this.metroAppButton1.AutoExpandOnClick = true;
            this.metroAppButton1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.TextOnlyAlways;
            this.metroAppButton1.CanCustomize = false;
            this.metroAppButton1.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.metroAppButton1.ImagePaddingHorizontal = 0;
            this.metroAppButton1.ImagePaddingVertical = 0;
            this.metroAppButton1.Name = "metroAppButton1";
            this.metroAppButton1.ShowSubItems = false;
            this.metroAppButton1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem_Login,
            this.buttonItem_dataShow,
            this.buttonItem_ReBootService,
            this.buttonItem_ReadRTU,
            this.buttonItem_SetSystem,
            this.buttonItem4});
            this.metroAppButton1.Text = "&系统管理";
            // 
            // buttonItem_Login
            // 
            this.buttonItem_Login.Name = "buttonItem_Login";
            this.buttonItem_Login.Text = "登    录";
            this.buttonItem_Login.Click += new System.EventHandler(this.buttonItem_Login_Click);
            // 
            // buttonItem_dataShow
            // 
            this.buttonItem_dataShow.Name = "buttonItem_dataShow";
            this.buttonItem_dataShow.Text = "数据显示";
            this.buttonItem_dataShow.Click += new System.EventHandler(this.buttonItem_dataShow_Click);
            // 
            // buttonItem_ReBootService
            // 
            this.buttonItem_ReBootService.Name = "buttonItem_ReBootService";
            this.buttonItem_ReBootService.Text = "&重启服务";
            this.buttonItem_ReBootService.Click += new System.EventHandler(this.buttonItem_ReBootService_Click);
            // 
            // buttonItem_ReadRTU
            // 
            this.buttonItem_ReadRTU.Name = "buttonItem_ReadRTU";
            this.buttonItem_ReadRTU.Text = "&重置信息";
            this.buttonItem_ReadRTU.Tooltip = "数据接收服务重新读取RTU信息";
            this.buttonItem_ReadRTU.Click += new System.EventHandler(this.buttonItem_ReadRTU_Click);
            // 
            // buttonItem_SetSystem
            // 
            this.buttonItem_SetSystem.Name = "buttonItem_SetSystem";
            this.buttonItem_SetSystem.Text = "&参数设置";
            this.buttonItem_SetSystem.Click += new System.EventHandler(this.buttonItem_SetSystem_Click);
            // 
            // buttonItem4
            // 
            this.buttonItem4.Name = "buttonItem4";
            this.buttonItem4.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem_Log,
            this.buttonItem_SendMail});
            this.buttonItem4.Text = "&日志查看";
            // 
            // buttonItem_Log
            // 
            this.buttonItem_Log.Name = "buttonItem_Log";
            this.buttonItem_Log.Text = "&本地日志";
            this.buttonItem_Log.Click += new System.EventHandler(this.buttonItem_Log_Click);
            // 
            // buttonItem_SendMail
            // 
            this.buttonItem_SendMail.Name = "buttonItem_SendMail";
            this.buttonItem_SendMail.Text = "&服务日志发送到Mail";
            this.buttonItem_SendMail.Click += new System.EventHandler(this.buttonItem_SendMail_Click);
            // 
            // metroAppButton2
            // 
            this.metroAppButton2.AutoExpandOnClick = true;
            this.metroAppButton2.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.TextOnlyAlways;
            this.metroAppButton2.CanCustomize = false;
            this.metroAppButton2.Enabled = false;
            this.metroAppButton2.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.metroAppButton2.ImagePaddingHorizontal = 0;
            this.metroAppButton2.ImagePaddingVertical = 0;
            this.metroAppButton2.Name = "metroAppButton2";
            this.metroAppButton2.ShowSubItems = false;
            this.metroAppButton2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem_SetCommand,
            this.buttonItem6});
            this.metroAppButton2.Text = "&测站管理";
            // 
            // buttonItem_SetCommand
            // 
            this.buttonItem_SetCommand.Name = "buttonItem_SetCommand";
            this.buttonItem_SetCommand.Text = "&远程控制";
            this.buttonItem_SetCommand.Click += new System.EventHandler(this.buttonItem_SetCommand_Click);
            // 
            // buttonItem6
            // 
            this.buttonItem6.Name = "buttonItem6";
            this.buttonItem6.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem_SetRTU,
            this.buttonItem_SetCenter,
            this.buttonItem_SetRTUWork,
            this.buttonItem_SetConfigData});
            this.buttonItem6.Text = "&测站维护";
            // 
            // buttonItem_SetRTU
            // 
            this.buttonItem_SetRTU.Name = "buttonItem_SetRTU";
            this.buttonItem_SetRTU.Text = "基本信息";
            this.buttonItem_SetRTU.Click += new System.EventHandler(this.buttonItem_SetRTU_Click);
            // 
            // buttonItem_SetCenter
            // 
            this.buttonItem_SetCenter.Name = "buttonItem_SetCenter";
            this.buttonItem_SetCenter.Text = "中心站信息";
            this.buttonItem_SetCenter.Click += new System.EventHandler(this.buttonItem_SetCenter_Click);
            // 
            // buttonItem_SetRTUWork
            // 
            this.buttonItem_SetRTUWork.Name = "buttonItem_SetRTUWork";
            this.buttonItem_SetRTUWork.Text = "工作状态信息";
            this.buttonItem_SetRTUWork.Click += new System.EventHandler(this.buttonItem_SetRTUWork_Click);
            // 
            // buttonItem_SetConfigData
            // 
            this.buttonItem_SetConfigData.Name = "buttonItem_SetConfigData";
            this.buttonItem_SetConfigData.Text = "监测项配置信息";
            this.buttonItem_SetConfigData.Click += new System.EventHandler(this.buttonItem_SetConfigData_Click);
            // 
            // metroAppButton3
            // 
            this.metroAppButton3.AutoExpandOnClick = true;
            this.metroAppButton3.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.TextOnlyAlways;
            this.metroAppButton3.CanCustomize = false;
            this.metroAppButton3.Enabled = false;
            this.metroAppButton3.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.metroAppButton3.ImagePaddingHorizontal = 0;
            this.metroAppButton3.ImagePaddingVertical = 0;
            this.metroAppButton3.Name = "metroAppButton3";
            this.metroAppButton3.ShowSubItems = false;
            this.metroAppButton3.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem_RealTime,
            this.buttonItem_Rem,
            this.buttonItem_Img,
            this.buttonItem_Manual,
            this.buttonItem_CmdState,
            this.buttonItem_DataResave});
            this.metroAppButton3.Text = "&数据维护";
            // 
            // buttonItem_RealTime
            // 
            this.buttonItem_RealTime.Name = "buttonItem_RealTime";
            this.buttonItem_RealTime.Text = "&实时数据";
            this.buttonItem_RealTime.Click += new System.EventHandler(this.buttonItem_RealTime_Click);
            // 
            // buttonItem_Rem
            // 
            this.buttonItem_Rem.Name = "buttonItem_Rem";
            this.buttonItem_Rem.Text = "&固态数据";
            this.buttonItem_Rem.Click += new System.EventHandler(this.buttonItem_Rem_Click);
            // 
            // buttonItem_Img
            // 
            this.buttonItem_Img.Name = "buttonItem_Img";
            this.buttonItem_Img.Text = "&图像接收";
            this.buttonItem_Img.Click += new System.EventHandler(this.buttonItem_Img_Click);
            // 
            // buttonItem_Manual
            // 
            this.buttonItem_Manual.Name = "buttonItem_Manual";
            this.buttonItem_Manual.Text = "&人工置数";
            this.buttonItem_Manual.Click += new System.EventHandler(this.buttonItem_Manual_Click);
            // 
            // buttonItem_CmdState
            // 
            this.buttonItem_CmdState.Name = "buttonItem_CmdState";
            this.buttonItem_CmdState.Text = "&召测状态";
            this.buttonItem_CmdState.Click += new System.EventHandler(this.buttonItem_CmdState_Click);
            // 
            // buttonItem_DataResave
            // 
            this.buttonItem_DataResave.Name = "buttonItem_DataResave";
            this.buttonItem_DataResave.Text = "&数据同步";
            this.buttonItem_DataResave.Click += new System.EventHandler(this.buttonItem_DataResave_Click);
            // 
            // metroTabItem_Exit
            // 
            this.metroTabItem_Exit.Checked = true;
            this.metroTabItem_Exit.Name = "metroTabItem_Exit";
            this.metroTabItem_Exit.Text = "&退出";
            this.metroTabItem_Exit.Click += new System.EventHandler(this.metroTabItem_Exit_Click);
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "遥测数据接收软件";
            this.buttonItem1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1});
            this.metroStatusBar1.Location = new System.Drawing.Point(1, 577);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(1034, 24);
            this.metroStatusBar1.TabIndex = 1;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "READY";
            this.labelItem1.DoubleClick += new System.EventHandler(this.labelItem1_DoubleClick);
            // 
            // itemPanel_top
            // 
            this.itemPanel_top.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.itemPanel_top.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel_top.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel_top.ContainerControlProcessDialogKey = true;
            this.itemPanel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemPanel_top.ForeColor = System.Drawing.Color.Black;
            this.itemPanel_top.Location = new System.Drawing.Point(1, 63);
            this.itemPanel_top.Name = "itemPanel_top";
            this.itemPanel_top.Size = new System.Drawing.Size(1034, 40);
            this.itemPanel_top.TabIndex = 3;
            // 
            // pageSlider1
            // 
            this.pageSlider1.AnimationTime = 250;
            this.pageSlider1.BackColor = System.Drawing.Color.White;
            this.pageSlider1.Controls.Add(this.pageSliderPage1);
            this.pageSlider1.Controls.Add(this.pageSliderPage2);
            this.pageSlider1.Controls.Add(this.pageSliderPage3);
            this.pageSlider1.Controls.Add(this.pageSliderPage4);
            this.pageSlider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageSlider1.ForeColor = System.Drawing.Color.Black;
            this.pageSlider1.IndicatorVisible = false;
            this.pageSlider1.Location = new System.Drawing.Point(1, 103);
            this.pageSlider1.Name = "pageSlider1";
            this.pageSlider1.NextPageVisibleMargin = 0;
            this.pageSlider1.PageSpacing = 0;
            this.pageSlider1.ScrollBarVisibility = DevComponents.DotNetBar.Controls.eScrollBarVisibility.Hidden;
            this.pageSlider1.SelectedPage = this.pageSliderPage1;
            this.pageSlider1.Size = new System.Drawing.Size(1034, 474);
            this.pageSlider1.TabIndex = 0;
            this.pageSlider1.Text = "pageSlider1";
            this.pageSlider1.SelectedPageChanged += new System.EventHandler(this.pageSlider1_SelectedPageChanged);
            // 
            // pageSliderPage1
            // 
            this.pageSliderPage1.BackColor = System.Drawing.Color.White;
            this.pageSliderPage1.ForeColor = System.Drawing.Color.Black;
            this.pageSliderPage1.Location = new System.Drawing.Point(4, 4);
            this.pageSliderPage1.Name = "pageSliderPage1";
            this.pageSliderPage1.Size = new System.Drawing.Size(1026, 466);
            this.pageSliderPage1.TabIndex = 3;
            // 
            // pageSliderPage2
            // 
            this.pageSliderPage2.BackColor = System.Drawing.Color.White;
            this.pageSliderPage2.ForeColor = System.Drawing.Color.Black;
            this.pageSliderPage2.Location = new System.Drawing.Point(1030, 4);
            this.pageSliderPage2.Name = "pageSliderPage2";
            this.pageSliderPage2.Size = new System.Drawing.Size(1026, 466);
            this.pageSliderPage2.TabIndex = 0;
            // 
            // pageSliderPage3
            // 
            this.pageSliderPage3.BackColor = System.Drawing.Color.White;
            this.pageSliderPage3.ForeColor = System.Drawing.Color.Black;
            this.pageSliderPage3.Location = new System.Drawing.Point(2056, 4);
            this.pageSliderPage3.Name = "pageSliderPage3";
            this.pageSliderPage3.Size = new System.Drawing.Size(1026, 466);
            this.pageSliderPage3.TabIndex = 1;
            // 
            // pageSliderPage4
            // 
            this.pageSliderPage4.BackColor = System.Drawing.Color.White;
            this.pageSliderPage4.ForeColor = System.Drawing.Color.Black;
            this.pageSliderPage4.Location = new System.Drawing.Point(3082, 4);
            this.pageSliderPage4.Name = "pageSliderPage4";
            this.pageSliderPage4.Size = new System.Drawing.Size(1026, 466);
            this.pageSliderPage4.TabIndex = 3;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.png");
            this.imageList1.Images.SetKeyName(1, "2.png");
            this.imageList1.Images.SetKeyName(2, "3.png");
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "提示";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.Color.White;
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.DarkGray);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "1.png");
            this.imageList2.Images.SetKeyName(1, "2.png");
            this.imageList2.Images.SetKeyName(2, "3.png");
            this.imageList2.Images.SetKeyName(3, "4.png");
            // 
            // contextMenuStrip_COM
            // 
            this.contextMenuStrip_COM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_GetState,
            this.toolStripMenuItem_SetTime});
            this.contextMenuStrip_COM.Name = "contextMenuStrip1";
            this.contextMenuStrip_COM.Size = new System.Drawing.Size(149, 48);
            this.contextMenuStrip_COM.Text = "111";
            // 
            // toolStripMenuItem_GetState
            // 
            this.toolStripMenuItem_GetState.Name = "toolStripMenuItem_GetState";
            this.toolStripMenuItem_GetState.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem_GetState.Text = "获取卫星状态";
            this.toolStripMenuItem_GetState.Click += new System.EventHandler(this.toolStripMenuItem_GetState_Click);
            // 
            // toolStripMenuItem_SetTime
            // 
            this.toolStripMenuItem_SetTime.Name = "toolStripMenuItem_SetTime";
            this.toolStripMenuItem_SetTime.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem_SetTime.Text = "获取卫星时间";
            this.toolStripMenuItem_SetTime.Click += new System.EventHandler(this.toolStripMenuItem_SetTime_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1036, 602);
            this.Controls.Add(this.pageSlider1);
            this.Controls.Add(this.itemPanel_top);
            this.Controls.Add(this.metroStatusBar1);
            this.Controls.Add(this.metroShell1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标准版接收软件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.pageSlider1.ResumeLayout(false);
            this.contextMenuStrip_COM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroShell metroShell1;
        private DevComponents.DotNetBar.Metro.MetroAppButton metroAppButton1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem_Exit;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        //private DevComponents.DotNetBar.QatCustomizeItem qatCustomizeItem1;
        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.ItemPanel itemPanel_top;
        public DevComponents.DotNetBar.Controls.PageSlider pageSlider1;
        private DevComponents.DotNetBar.Controls.PageSliderPage pageSliderPage1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevComponents.DotNetBar.Controls.PageSliderPage pageSliderPage2;
        private DevComponents.DotNetBar.ButtonItem buttonItem_SetSystem;
        private DevComponents.DotNetBar.ButtonItem buttonItem4;
        private DevComponents.DotNetBar.Metro.MetroAppButton metroAppButton2;
        private DevComponents.DotNetBar.ButtonItem buttonItem_SetCenter;
        private DevComponents.DotNetBar.ButtonItem buttonItem_SetRTU;
        private DevComponents.DotNetBar.ButtonItem buttonItem_SetRTUWork;
        private DevComponents.DotNetBar.ButtonItem buttonItem_dataShow;
        private DevComponents.DotNetBar.Metro.MetroAppButton metroAppButton3;
        public  DevComponents.DotNetBar.Controls.PageSliderPage pageSliderPage4;
        private DevComponents.DotNetBar.ButtonItem buttonItem_SetConfigData;
        private DevComponents.DotNetBar.ButtonItem buttonItem_CmdState;
        public DevComponents.DotNetBar.StyleManager styleManager1;
        private System.Windows.Forms.ImageList imageList2;
        private DevComponents.DotNetBar.ButtonItem buttonItem_Log;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_COM;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_GetState;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_SetTime;
        public DevComponents.DotNetBar.Controls.PageSliderPage pageSliderPage3;
        public DevComponents.DotNetBar.ButtonItem buttonItem_Login;
        public DevComponents.DotNetBar.ButtonItem buttonItem_ReBootService;
        public DevComponents.DotNetBar.ButtonItem buttonItem_SetCommand;
        public DevComponents.DotNetBar.ButtonItem buttonItem_RealTime;
        public DevComponents.DotNetBar.ButtonItem buttonItem_Rem;
        public DevComponents.DotNetBar.ButtonItem buttonItem6;
        public DevComponents.DotNetBar.ButtonItem buttonItem_SendMail;
        public DevComponents.DotNetBar.ButtonItem buttonItem_ReadRTU;
        public DevComponents.DotNetBar.ButtonItem buttonItem_Manual;
        private DevComponents.DotNetBar.ButtonItem buttonItem_Img;
        public DevComponents.DotNetBar.ButtonItem buttonItem_DataResave;
        

    }
}

