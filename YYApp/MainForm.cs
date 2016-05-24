using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using System.Threading;
using Service;

namespace YYApp
{
    public partial class MainForm : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        LoginForm LF = null;
        string ProName = "";
        public MainForm(LoginForm lf,string proname)
        {
            InitializeComponent();
            LF = lf;
            ProName = proname;
        }


        #region 线程操作控件的委托
        private delegate void DelegateUdpControls();
        private delegate void DelegateUpdadvTree_left();
        private delegate void DelegateAddData();
        private delegate void DelegateAddRealTimeData();
        private delegate void DelegateAddRTUState();
        #endregion


        private void MainForm_Load(object sender, EventArgs e)
        {
            PubObject.PubObjectInit(imageList2);
            IList<Service.Model.YY_RTU_Basic> rtus=null;
            ExecCommandList.LC = new List<Command>();


            ExecServiceList.Lsm = Program.wrx.ReadXML(); //读取服务信息
            buttonItem1.Text += "[" + ProName + "]";


            AddControls();        //根据xml文件动态添加服务显示灯控件
            TcpControl.TcpClient_Init();     //tcp与服务交互初始化

            PublicBD.Path = Program.wrx.GetPath();
            PublicBD.ReInit();
            //连接数据库
            if (PublicBD.ConnectState)
            {
                //得到RTU列表
                rtus = Service.PublicBD.db.GetRTUList("");
                //得到命令列表
                ExecCommandList.Commands = Service.PublicBD.db.GetRTUCommandList();
                //得到命令临时表中的要发送的召测命令
                ExecRTUList.SetLrdm(rtus);

                //提示可能是自动入库的测站信息
                AlertFomrShow(rtus);

                //显示数据窗体控件
                buttonItem_dataShow_Click(null, null);
            }
            else 
            {
                //显示系统设置窗体控件
                buttonItem_SetSystem_Click(null,null);
            }




            ////根据服务在线状态更新服务显示灯
            Thread updcontrols = new Thread(new ThreadStart(ThreadUpdControls));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            updcontrols.IsBackground = true;
            updcontrols.Start();



            //添加明文数据
            Thread adddata = new Thread(new ThreadStart(ThreadAddData));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            adddata.IsBackground = true;
            adddata.Start();


            Thread ReConnect = new Thread(new ThreadStart(ThreadReConnect));
            ReConnect.IsBackground = true;
            ReConnect.Start();
         }


        #region 根据xml文件动态添加服务显示灯控件
        private void AddControls()
        {
            ArrayList al = new ArrayList();

            DevComponents.DotNetBar.LabelItem li = new DevComponents.DotNetBar.LabelItem();
            li.Text = "    ";
            DevComponents.DotNetBar.Controls.ReflectionImage ri;

            DevComponents.DotNetBar.Controls.ReflectionLabel rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            rl.Text = "<b><font size='+3'><font color='#B02B2C'>TCP:</font></font></b>";
            rl.Width = 40;
            rl.Height = 30;
            DevComponents.DotNetBar.ControlContainerItem cci = new DevComponents.DotNetBar.ControlContainerItem();
            cci.Control = rl;

            al.Add(li);
            al.Add(cci);


            var tcp = from t in ExecServiceList.Lsm where t.SERVICETYPE == "TCP" select t;
            int i = 0;
            foreach (var item in tcp)
            {
                i++;
                rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
                rl.Text = "<b><font color='#B02B2C'><font size='+.5'><i>" + item.SERVICEID + "</i></font></font></b>";
                rl.Width = item.SERVICEID.ToString().Length * 8;
                rl.Height = 30;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = rl;
                al.Add(cci);



                ri = new DevComponents.DotNetBar.Controls.ReflectionImage();
                if (item.STATE == false)
                {
                    ri.Image = imageList1.Images[0];
                }
                else { ri.Image = imageList1.Images[2]; }
                ri.MouseDoubleClick += new MouseEventHandler(ri_MouseDoubleClick);
                ri.Tag = item.SERVICETYPE + item.SERVICEID;
                ri.Width = 20;
                ri.Height = 25;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = ri;
                al.Add(cci);
            }

            rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            rl.Text = "<b><font size='+3'><font color='#B02B2C'>UDP:</font></font></b>";
            rl.Width = 40;
            rl.Height = 30;
            cci = new DevComponents.DotNetBar.ControlContainerItem();
            cci.Control = rl;


            li = new DevComponents.DotNetBar.LabelItem();
            li.Text = "    ";
            al.Add(li);
            al.Add(cci);

            var udp = from t in ExecServiceList.Lsm where t.SERVICETYPE == "UDP" select t;
            i = 0;
            foreach (var item in udp)
            {
                rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
                rl.Text = "<b><font color='#B02B2C'><font size='+.5'><i>" + item.SERVICEID + "</i></font></font></b>";
                rl.Width = item.SERVICEID.ToString().Length * 8;
                rl.Height = 30;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = rl;
                al.Add(cci);


                ri = new DevComponents.DotNetBar.Controls.ReflectionImage();
                if (item.STATE == false)
                {
                    ri.Image = imageList1.Images[0];
                }
                else { ri.Image = imageList1.Images[2]; }
                ri.MouseDoubleClick += new MouseEventHandler(ri_MouseDoubleClick);
                ri.Tag = item.SERVICETYPE + item.SERVICEID;
                ri.Width = 20;
                ri.Height = 25;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = ri;
                al.Add(cci);
            }


            rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            rl.Text = "<b><font size='+3'><font color='#B02B2C'>GSM:</font></font></b>";
            rl.Width = 40;
            rl.Height = 30;
            cci = new DevComponents.DotNetBar.ControlContainerItem();
            cci.Control = rl;

            li = new DevComponents.DotNetBar.LabelItem();
            li.Text = "    ";
            al.Add(li);
            al.Add(cci);


            var gsm = from t in ExecServiceList.Lsm where t.SERVICETYPE == "GSM" select t;
            i = 0;
            foreach (var item in gsm)
            {
                rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
                rl.Text = "<b><font color='#B02B2C'><font size='+.5'><i>" + item.SERVICEID + "</i></font></font></b>";
                rl.Width = item.SERVICEID.ToString().Length * 8;
                rl.Height = 30;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = rl;
                al.Add(cci);


                ri = new DevComponents.DotNetBar.Controls.ReflectionImage();
                if (item.STATE == false)
                {
                    ri.Image = imageList1.Images[0];
                }
                else { ri.Image = imageList1.Images[2]; }
                ri.MouseDoubleClick +=new MouseEventHandler(ri_MouseDoubleClick);
                ri.Tag = item.SERVICETYPE + item.SERVICEID;
                ri.Width = 20;
                ri.Height = 25;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = ri;
                al.Add(cci);
            }

            rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            rl.Text = "<b><font size='+3'><font color='#B02B2C'>COM:</font></font></b>";
            rl.Width = 40;
            rl.Height = 30;
            cci = new DevComponents.DotNetBar.ControlContainerItem();
            cci.Control = rl;

            li = new DevComponents.DotNetBar.LabelItem();
            li.Text = "    ";
            al.Add(li);
            al.Add(cci);

            var com = from t in ExecServiceList.Lsm where t.SERVICETYPE == "COM" select t;
            if (com.Count() > 0)
            {
                toolTip1.SetToolTip(rl, "右击操作");
                rl.MouseClick += new MouseEventHandler(rl_MouseClick);
            }
            i = 0;
            foreach (var item in com)
            {
                rl = new DevComponents.DotNetBar.Controls.ReflectionLabel();
                rl.Text = "<b><font color='#B02B2C'><font size='+.5'><i>" + item.SERVICEID + "</i></font></font></b>";
                rl.Width = item.SERVICEID.ToString().Length * 8;
                rl.Height = 30;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = rl;
                al.Add(cci);


                ri = new DevComponents.DotNetBar.Controls.ReflectionImage();
                if (item.STATE == false)
                {
                    ri.Image = imageList1.Images[0];
                }
                else { ri.Image = imageList1.Images[2]; }
                ri.MouseDoubleClick += new MouseEventHandler(ri_MouseDoubleClick);
                ri.Tag = item.SERVICETYPE + item.SERVICEID;
                ri.Width = 20;
                ri.Height = 25;
                cci = new DevComponents.DotNetBar.ControlContainerItem();
                cci.Control = ri;
                al.Add(cci);
            }

            DevComponents.DotNetBar.BaseItem[] bi = new DevComponents.DotNetBar.BaseItem[al.Count];
            for (i = 0; i < al.Count; i++)
            {
                bi[i] = al[i] as DevComponents.DotNetBar.BaseItem;
            }

            itemPanel_top.Items.AddRange(bi);


        }

        #region COM信道的右键操作
        void rl_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否点击为右键
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip_COM.Show(sender as Control, e.Location);
            }
        }

        private void toolStripMenuItem_GetState_Click(object sender, EventArgs e)
        {
            var com = from t in ExecServiceList.Lsm where t.SERVICETYPE == "COM" select t;
            if (com.Count() > 0)
            {
                if (TcpControl.Connected)
                {
                    string command = "--com|sta";

                    bool b = TcpControl.SendUItoServiceCommand(command);
                    if (!b)
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show("命令发送失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重启各信道服务", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void toolStripMenuItem_SetTime_Click(object sender, EventArgs e)
        {
            var com = from t in ExecServiceList.Lsm where t.SERVICETYPE == "COM" select t;
            if (com.Count() > 0)
            {
                if (TcpControl.Connected)
                {
                    string command = "--com|tim";

                    bool b = TcpControl.SendUItoServiceCommand(command);
                    if (!b)
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show("命令发送失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重启各信道服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        
        
        void ri_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string tag = (sender as DevComponents.DotNetBar.Controls.ReflectionImage).Tag.ToString(); 
            var ser = from s in ExecServiceList.Lsm where (s.SERVICETYPE + s.SERVICEID) == tag select s;
            string SERVICETYPE = ser.First().SERVICETYPE;
            ser = from s in ExecServiceList.Lsm where s.SERVICETYPE == SERVICETYPE select s;
            service[] sers=ser.ToArray<service>();
            string Index = "";
            for (int i = 0; i < ser.Count(); i++)
            {
                if (sers[i].SERVICETYPE + sers[i].SERVICEID == tag)
                {
                    Index += "0:";
                }
                else 
                {
                    Index+="1:";
                }
            }

            if (TcpControl.Connected  && Index.Length >0)
            {
                Index = Index.Substring(0, Index.Length - 1);
                string command = "--" + SERVICETYPE.ToLower() + "|" + Index;
                bool b=TcpControl.SendUItoServiceCommand(command);
                if (!b) 
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("命令发送失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重启各信道服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 线程执行方法
        private bool state = false;
        private void UpdControls() 
        {
            DevComponents.DotNetBar.Controls.ReflectionImage ri;
            foreach (var item in itemPanel_top.Items)
            {
                DevComponents.DotNetBar.ControlContainerItem cci=item as DevComponents.DotNetBar.ControlContainerItem;
                if (cci != null &&cci.Control .Tag!=null) 
                {
                    ri=cci.Control as DevComponents.DotNetBar.Controls.ReflectionImage;
                    var ser = from s in ExecServiceList.Lsm where (s.SERVICETYPE + s.SERVICEID) == ri.Tag.ToString() select s;

                    //在线绿色，不在线红色。
                    if (ser.First().STATE == false)
                    {
                        ri.Image = imageList1.Images[0];
                    }
                    else
                    { ri.Image = imageList1.Images[2]; }

                    if (ser.First().LISTCOUNT != "" && ser.First().LISTCOUNT != null)
                    {
                        string[] strs = ser.First().LISTCOUNT.Split(new char[] { ',' });
                        if (strs.Length == 5)
                            toolTip1.SetToolTip(ri, ser.First().IP_PORTNAME + "：" + ser.First().PORT_BAUDRATE + "\n" + "客户端：" + strs[0] + " 接收数据：" + strs[1] + " 发送数据：" + strs[2] + " UI数据：" + strs[3] + " 透传数据：" + strs[4]);
                        if (strs.Length == 7)
                        {
                            //原版本卫星协议返回状态
                            #region
                            //string Toolstr =
                            //"通道1信号功率：" + strs[0] + "\n" +
                            //"通道2信号功率：" + strs[1] + "\n" +
                            //"通道1卫星波束：" + strs[2] + "\n" +
                            //"通道2卫星波束：" + strs[3] + "\n" +
                            //"响应波束：" + strs[4] + "\n" +
                            //"信号抑制：" + (strs[5] == "0" ? "有" : "无") + "\n" +
                            //"供电状态：" + (strs[6] == "0" ? "异常" : "正常");
                            //toolTip1.SetToolTip(ri, Toolstr);

                            ////卫星信号状态异常黄色
                            //if (strs[0] == "0" && strs[1] == "0")
                            //{
                            //    ri.Image = imageList1.Images[1];
                            //}
                            //else { ri.Image = imageList1.Images[2]; }
                            #endregion


                            //新版本卫星协议4.0返回状态
                            #region
                            string Toolstr = "";

                            //IC卡状态
                            if (strs[0] == "00")//正常
                            { Toolstr += "IC卡状态:正常\n"; }
                            else //异常
                            { Toolstr += "IC卡状态:异常\n"; }

                            //硬件状态
                            if (strs[1] == "00")//正常
                            { Toolstr += "硬件状态:正常\n"; }
                            else //异常
                            { Toolstr += "硬件状态:异常\n"; }

                            //电量百分比
                            Toolstr += "电量:" + strs[2] + "%\n";


                            string Power = strs[3];
                            string[] bs = new string[] { "＜-158dBW", "-156～-157dBW", "-154～-155dBW", "-152～-153dBW", "＞-152dBW" };
                            if (Power.Length == 12)
                                for (int i = 0; i < 6; i++)
                                {
                                    string s = Power.Substring(2 * i, 2);
                                    if (s == "00")
                                    {
                                        Toolstr += "波束" + (i + 1) + "#功率:" + bs[0] + "\n";
                                    }
                                    else if (s == "01")
                                    { Toolstr += "波束" + (i + 1) + "#功率:" + bs[1] + "\n"; }
                                    else if (s == "02")
                                    { Toolstr += "波束" + (i + 1) + "#功率:" + bs[2] + "\n"; }
                                    else if (s == "03")
                                    { Toolstr += "波束" + (i + 1) + "#功率:" + bs[3] + "\n"; }
                                    else if (s == "04")
                                    { Toolstr += "波束" + (i + 1) + "#功率:" + bs[4] + "\n"; }
                                }

                            toolTip1.SetToolTip(ri, Toolstr);


                            //卫星信号状态异常黄色
                            if (strs[0] == "00" && strs[1] == "00")
                            {
                                ri.Image = imageList1.Images[1];
                            }
                            else { ri.Image = imageList1.Images[2]; }
                            #endregion
                        }
                    }

                    //不在线红色
                    if (ser.First().STATE == false)
                    {
                        ri.Image = imageList1.Images[0];
                    }
                    
                    
                }
                
            }

          
        }
        private void UpdAppStyle() 
        {
            if (state != ExecServiceList.ServiceDBConnectionState)
            {
                state = ExecServiceList.ServiceDBConnectionState;
                //服务的数据库连接改变界面颜色
                if (ExecServiceList.ServiceDBConnectionState)
                {
                    //this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255))))));
                    this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(35)))), ((int)(((byte)(160))))));//77,149,218
                    itemPanel_top.BackgroundStyle.BorderColor = styleManager1.MetroColorParameters.BaseColor;

                    //metroAppButton1.Enabled = true;
                    metroAppButton2.Enabled = true;
                    metroAppButton3.Enabled = true;
                }
                else
                {
                    this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169))))));

                    itemPanel_top.BackgroundStyle.BorderColor = styleManager1.MetroColorParameters.BaseColor;

                    pageSlider1.SelectedPageIndex = 0;
                    //metroAppButton1.Enabled = false;
                    metroAppButton2.Enabled = false;
                    metroAppButton3.Enabled = false;
                }
                if(scc!=null)
                scc.SetColor(styleManager1.MetroColorParameters.BaseColor, styleManager1.MetroColorParameters.CanvasColor);
            }
        }
        private void ThreadUpdControls()
        {
            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                        this.Invoke(new DelegateUdpControls(UpdControls));
                        this.Invoke(new DelegateUdpControls(UpdAppStyle));
                    }
                    catch { }
                }
                Thread.Sleep(1000);
            }
        }


        private void AddData()
        {
            lock (TcpControl.DataQueue)
            {
                while (TcpControl.DataQueue.Count > 0)
                {
                    string str = TcpControl.DataQueue.Dequeue();
                    string temp = ExecServiceList.UpdLsm(str);
                    temp = ExecServiceList.UpdDBConnectionState(temp);
                    temp = ExecRTUList.Updrdm(temp);
                    temp = ExecCommandList.UpdCommand(temp);
                    temp = SynXml.synxml(temp);


                    Text=temp.Replace("++", "");
                  
                }
            }

            TcpControl.SendData();
            if (!TcpControl.Connected)
            {
                ExecServiceList.UpdLsm();
                ExecRTUList.Updrdm();
                ExecServiceList.ServiceDBConnectionState = false;
            }
        }
        private void ThreadAddData()
        {
            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        try
                        {
                            // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                            this.Invoke(new DelegateAddData(AddData));
                        }
                        catch { }
                    }
                    catch { }
                }
                Thread.Sleep(100);
            }
        }

        private void ThreadReConnect() 
        {
            while(true)
            {
                if (!TcpControl.Connected) 
                {
                    TcpControl.TcpClient_Init();
                }
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region 用于显示明文的事件，各子窗体可订阅
        // 定义一个委托
        public delegate void ShowDataDelegate(object sender, EventArgs e);

        // 定义一个事件
        public event ShowDataDelegate ShowDataEvent;

        protected virtual void OnShowData(EventArgs e)
        {
            if (this.ShowDataEvent != null)
                this.ShowDataEvent(this, e);
        }
        private string _text;
        public string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;
                // 文本改变时触发Change事件
                this.OnShowData(new EventArgs());
            }
        }

        private CommandResultForm _crf;
        public CommandResultForm CRF 
        {
            get { return this._crf; }
            set
            {
                this._crf = value;
            }
        }
        #endregion

        #region 界面所有点击操作
        YYApp.SetControl.ShowDataControl scc = null;
        private void buttonItem_dataShow_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 0;
            if (scc == null)
            {
                scc = new SetControl.ShowDataControl();
                scc.Dock = DockStyle.Fill;
                pageSliderPage1.Controls.Add(scc);
            }
           
        }

        YYApp.CommandControl.SetCommand sc = null;
        private void buttonItem_SetCommand_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 1;
            if (sc == null)
            {
                sc = new CommandControl.SetCommand(styleManager1.MetroColorParameters.BaseColor, styleManager1.MetroColorParameters.CanvasColor,pageSlider1);
                sc.Dock = DockStyle.Fill;
                pageSliderPage2.Controls.Add(sc);
            }
            
        }

        private void buttonItem_SetRTU_Click(object sender, EventArgs e)
        {
            pageSliderPage2.Controls.Clear();
            sc = null;

            pageSlider1.SelectedPageIndex = 2;
            pageSliderPage3.Controls.Clear();
            SetControl.SetRTUControl src = new SetControl.SetRTUControl();
            //src.Location = new Point((pageSliderPage3.Width - src.Width) / 2, (pageSliderPage3.Height - src.Height) / 2);
            src.Dock=DockStyle.Fill;
            pageSliderPage3.Controls.Add(src);
        }

        private void buttonItem_SetCenter_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 2;
            pageSliderPage3.Controls.Clear();
            SetControl.SetCenterControl scc = new SetControl.SetCenterControl();
            //scc.Location = new Point((pageSliderPage3.Width - scc.Width) / 2, (pageSliderPage3.Height - scc.Height) / 2);
            scc.Dock = DockStyle.Fill;
            pageSliderPage3.Controls.Add(scc);
        }

        private void buttonItem_SetRTUWork_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 2;
            pageSliderPage3.Controls.Clear();

            //燕禹协议
            if (Program.wrx.ReadDllXML().ToLower() == "gsprotocol.dll")
            {
                SetControl.SetYyRTUWorkControl swc = new SetControl.SetYyRTUWorkControl();
                //scc.Location = new Point((pageSliderPage3.Width - scc.Width) / 2, (pageSliderPage3.Height - scc.Height) / 2);
                swc.Dock = DockStyle.Fill;
                pageSliderPage3.Controls.Add(swc);
            }//水资源协议
            else if (Program.wrx.ReadDllXML().ToLower() == "protocol.dll")
            {
                SetControl.SetRTUWorkControl swc = new SetControl.SetRTUWorkControl();
                //scc.Location = new Point((pageSliderPage3.Width - scc.Width) / 2, (pageSliderPage3.Height - scc.Height) / 2);
                swc.Dock = DockStyle.Fill;
                pageSliderPage3.Controls.Add(swc);
            }
            else  //其他协议（目前指水文）
            {
                SetControl.SetHydrologicRTUWorkControl swc = new SetControl.SetHydrologicRTUWorkControl();
                //scc.Location = new Point((pageSliderPage3.Width - scc.Width) / 2, (pageSliderPage3.Height - scc.Height) / 2);
                swc.Dock = DockStyle.Fill;
                pageSliderPage3.Controls.Add(swc);
            }
            
        }

        private void buttonItem_RealTime_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 3;
            pageSliderPage4.Controls.Clear();
            SetControl.SetRealTimeControl stc = new SetControl.SetRealTimeControl();
            stc.Dock=DockStyle.Fill;
            pageSliderPage4.Controls.Add(stc);
        }

        private void buttonItem_Rem_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 3;
            pageSliderPage4.Controls.Clear();
            SetControl.SetRemControl src = new SetControl.SetRemControl();
            src.Dock=DockStyle.Fill;
            pageSliderPage4.Controls.Add(src);
        }

        private void buttonItem_Img_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 3;
            pageSliderPage4.Controls.Clear();
            SetControl.SetImgControl satc = new SetControl.SetImgControl();
            satc.Dock = DockStyle.Fill;
            pageSliderPage4.Controls.Add(satc);
        }

      
        private void buttonItem_Manual_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 3;
            pageSliderPage4.Controls.Clear();
            SetControl.SetManualControl smc = new SetControl.SetManualControl();
            smc.Dock = DockStyle.Fill;
            pageSliderPage4.Controls.Add(smc);
        }

        //SetControl.SetDataResaveControl sdtc = null;
        private void buttonItem_DataResave_Click(object sender, EventArgs e)
        {
            
            if (System.Diagnostics.Process.GetProcessesByName("DataResaveApplication").ToList().Count > 0)
            {
                //´æÔÚ
                DevComponents.DotNetBar.MessageBoxEx.Show("转存工具已经启动！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {//²»´æÔÚ
                try
                {
                    System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + "/DataResaveApplication.exe");
                }
                catch { }
            }

            //pageSliderPage4.Controls.Clear();
            //pageSlider1.SelectedPageIndex = 3;
            //if (sdtc == null) 
            //{
            //    sdtc = new SetControl.SetDataResaveControl();                
            //}
            //sdtc.Dock = DockStyle.Fill;
            //pageSliderPage4.Controls.Add(sdtc);
            
        }

        private void buttonItem_SetConfigData_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 2;
            pageSliderPage3.Controls.Clear();
            SetControl.SetConfigDataControl scdc = new SetControl.SetConfigDataControl();
            //scc.Location = new Point((pageSliderPage3.Width - scc.Width) / 2, (pageSliderPage3.Height - scc.Height) / 2);
            scdc.Dock = DockStyle.Fill;
            pageSliderPage3.Controls.Add(scdc);
        }

        private void buttonItem_SetSystem_Click(object sender, EventArgs e)
        {
            pageSliderPage2.Controls.Clear();
            sc = null;

            pageSlider1.SelectedPageIndex = 2;
            pageSliderPage3.Controls.Clear();
            SetControl.SetSystemControl ssc = new SetControl.SetSystemControl();
            //src.Location = new Point((pageSliderPage3.Width - src.Width) / 2, (pageSliderPage3.Height - src.Height) / 2);
            ssc.Dock = DockStyle.Fill;
            pageSliderPage3.Controls.Add(ssc);
        }

        public void buttonItem_ReBootService_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                TcpControl.SendUItoServiceCommand("--ser|");
            }
            else 
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重启各信道服务！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonItem_CmdState_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 3;
            pageSliderPage4.Controls.Clear();
            SetControl.GetCommandState gcs = new SetControl.GetCommandState( );
            gcs.Dock = DockStyle.Fill;
            pageSliderPage4.Controls.Add(gcs);
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            pageSlider1.SelectedPageIndex = 0;
            if (scc == null)
            {
                scc = new SetControl.ShowDataControl( );
                scc.Dock = DockStyle.Fill;
                pageSliderPage1.Controls.Add(scc);
            }
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DevComponents.DotNetBar.MessageBoxEx.Show(" 确认退出当前系统？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                System.Environment.Exit(0);
            else
                e.Cancel = true;
        }

        private void metroTabItem_Exit_Click(object sender, EventArgs e)
        {
            if (DevComponents.DotNetBar.MessageBoxEx.Show(" 确认退出当前系统？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                System.Environment.Exit(0);
        }

        private void buttonItem_Login_Click(object sender, EventArgs e)
        {
            LF.Visible = true;
            LF.textBox_username.Focus();
        }

        public void buttonItem_ReadRTU_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                TcpControl.SendUItoServiceCommand("--rtu|");
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法重置信息！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 界面其他
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (CRF  != null)
                try
                {
                   CRF.Location = new System.Drawing.Point(this.Location.X, this.Location.Y +this.Height);
                }
                catch { }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (CRF != null)
                try
                {
                    CRF.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + this.Height);
                    CRF.Width = this.Width;
                }
                catch { }
            
        }

        private void pageSlider1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (pageSlider1.SelectedPageIndex == 0)
            {
                if (scc!=null && scc.scc != null)
                {
                    scc.scc_Init();
                }
            }
            else if (pageSlider1.SelectedPageIndex == 1)
            {
                if (sc!=null && sc.scc != null)
                {
                    sc.scc_Init(styleManager1.MetroColorParameters.BaseColor);
                }
            }
        }
        #endregion

        #region Alert弹出提示框
        private void AlertFomrShow(IList<Service.Model.YY_RTU_Basic> rtus)
        {
            if (Program.LoginState)
            {
                var Rtus = from r in rtus where r.STCD == r.NiceName select r;
                if (Rtus.Count() > 0)
                {
                    AlertForm af = new AlertForm(Rtus.Count());
                    Rectangle r = Screen.GetWorkingArea(this);
                    af.Location = new Point(r.Right - af.Width, r.Bottom - af.Height);
                    af.AutoClose = true;
                    af.AutoCloseTimeOut = 15;
                    af.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.BottomToTop;
                    af.AlertAnimationDuration = 300;
                    af.Show(false);
                }
            }
        }
        #endregion

        AboutForm af = null;
        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {
            if (af == null)
            {
                af = new AboutForm();
                af.ShowDialog();
            }
            else { af.ShowDialog(); }
        }

        private void buttonItem_Log_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath+"/log"); 
        }

        private void buttonItem_SendMail_Click(object sender, EventArgs e)
        {
            if (TcpControl.Connected)
            {
                TcpControl.SendUItoServiceCommand("--mal|");
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("与服务端通讯异常，无法发送命令！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void metroShell1_HelpButtonClick(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + "/标准版接收软件用户手册.mht";
                p.Start();
            }
            catch
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("文件无法打开，请检查帮助文件是否存在！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            }
        }

        private void labelItem1_DoubleClick(object sender, EventArgs e)
        {
            if (Program.wrx.ReadResaveName() == "HLJ")
            {
                SetHLJForm hlj = new SetHLJForm();
                hlj.ShowDialog();
            }
        }
       
    }

    public static class PubObject 
    {
        public static System.Windows.Forms.ImageList imgList;
        public static void PubObjectInit(System.Windows.Forms.ImageList imgList1)
        {
            imgList = imgList1;
        }
    }
}