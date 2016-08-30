using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Service;

namespace YYApp.SetControl
{
    public partial class ShowDataControl : UserControl
    {
        public ShowDataControl()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false; 

            #region 界面效果属性
           
            //superTabItem1.Tooltip = "双击该区域停止刷新数据";
            //superTabItem2.Tooltip = "双击该区域停止刷新数据";
            //superTabItem4.Tooltip = "双击该区域停止刷新数据";
            //listViewEx_Right.SmallImageList = imagelist;

            #endregion

            scc_Init();
            comboBox_Item_Init(comboBox_Item);
            comboBox_Item_Init(comboBox_Item1);
        }
        public ShowCommandControl scc = null;
        public void scc_Init()
        {
            panelEx_right.Controls.Clear();
            scc = new ShowCommandControl();
            scc.Dock = DockStyle.Fill;
            if (c != null)
            {
                scc.SetColor(c.Value );
            }
            panelEx_right.Controls.Add(scc);
        }

        private IList<Service.Model.YY_RTU_ITEM> ITEMList = null;
        private void comboBox_Item_Init( DevComponents .DotNetBar.Controls.ComboBoxEx  cb)
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(" where ItemCode!='-1' and ItemCode!='0000000000'");
            if (ItemList != null && ItemList.Count > 0)
            {
                Service.Model.YY_RTU_ITEM item=new Service.Model.YY_RTU_ITEM();
                item.ItemName="全部";
                item.ItemID = "-1";
                ItemList.Insert(0,item);

                if (Program.wrx.XMLObj.dllfile .ToLower() == "gsprotocol.dll")
                {
                    var items = from il in ItemList where il.ItemID == "180" || il.ItemID == "181" || il.ItemID == "182" || il.ItemID == "183" || il.ItemID == "184" || il.ItemID == "185" select il;

                    if (items.Count() > 0)
                    {
                        foreach (var tem in items.ToArray<Service.Model.YY_RTU_ITEM>())
                        {
                            ItemList.Remove(tem);
                        }

                        (from il in ItemList where il.ItemID == "12" select il).First().ItemName = "雨量";
                        (from il in ItemList where il.ItemID == "15" select il).First().ItemName = "水位1";
                        (from il in ItemList where il.ItemID == "16" select il).First().ItemName = "水位2";
                    }
                }


                ITEMList = ItemList;
                cb.DataSource = ItemList;
                cb.DisplayMember = "ItemName";
                cb.ValueMember = "ItemID";
                cb.SelectedIndex = 0;

            }
        }
        

        #region 线程操作控件的委托
        private delegate void DelegateUdpControls();
        private delegate void DelegateUpdadvTree_left();
        private delegate void DelegateAddData(DataTable dt);
        private delegate void DelegateAddCMDData();
        private delegate void DelegateAddRTUState();
        #endregion

        Color? c = null;
        public void SetColor(Color color, Color backcolor)
        {
            expandableSplitter1.BackColor2 = color;
            expandableSplitter2.BackColor2 = color;
            expandableSplitter3.BackColor2 = color;

            advTree_left.BackgroundStyle.BorderColor = color;
            panelEx_left.Style.BorderColor.Color = color;
            panelEx_fill.Style.BorderColor.Color = color;
            panelEx_right.Style.BorderColor.Color = color;
            panelEx_bottom.Style.BorderColor.Color = color;
            //listViewEx_Right.Border.BorderColor = color;
            //listViewEx_Right.BackColor = backcolor;


            richTextBoxEx_Fill1.BackColor = backcolor;
            richTextBoxEx_Fill2.BackColor = backcolor;
            richTextBoxEx_Fill3.BackColor = backcolor;
            richTextBoxEx_Fill4.BackColor = backcolor;
            richTextBoxEx_Fill5.BackColor = backcolor;

            richTextBoxEx_bottom.BackColor = backcolor;

            if (color.R != 169)
            {
                this.button_clear.Image = global::YYApp.Properties.Resources.Clear;
            }
            else 
            {
                this.button_clear.Image = global::YYApp.Properties.Resources.Clear1;
            }

            c = color;
            if (scc != null)
            {
                scc.SetColor(color);
            }
        }

        Thread addrealtimedata;
        private void ShowDataControl_Load(object sender, EventArgs e)
        {
            SetColor(((MainForm)this.ParentForm).styleManager1.MetroColorParameters.BaseColor, this.ParentForm.BackColor);

            ((MainForm)this.ParentForm).ShowDataEvent += new MainForm.ShowDataDelegate(ShowDataControl_ShowDataEvent);

            adTree_Init();


            //更新树形控件中测站状态
            Thread updadvtree_left = new Thread(new ThreadStart(ThreadUpdadvTree_left));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            updadvtree_left.IsBackground = true;
            updadvtree_left.Start();



            //添加实时数据
            addrealtimedata = new Thread(new ThreadStart(ThreadAddRealTimeData));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            addrealtimedata.IsBackground = true;
            addrealtimedata.Start();


            ////添加命令状态数据
            //Thread addcmdstate = new Thread(new ThreadStart(ThreadAddCMDState));
            //// 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            //addcmdstate.IsBackground = true;
            //addcmdstate.Start();
        }

        bool packover = true; //结束包标识
        void ShowDataControl_ShowDataEvent(object sender, EventArgs e)
        {
            string text = ((MainForm)this.ParentForm).Text;
            string[] txt = text.Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (var item in txt)
            {
                if (item != "")
                {
                    if (item.IndexOf("packover") > 0)
                    {
                        packover = true;
                        return;
                    }
                    QueueAdd(item, dt);
                    richTextBoxEx_Fill1.Rtf = rtf.GetRealTimeDataRtfString(dt);

                    richTextBoxEx_bottom.AppendText(item.Replace("++", "")+"\n");
                    if (richTextBoxEx_bottom.Text.Length > 20000)
                    {
                        richTextBoxEx_bottom.Text = "";
                    }
                    richTextBoxEx_bottom.ScrollToCaret();
                }
            }
            

        }

        //最多100条
        private string _stcd;
        private string _name;
        private string _itemid,_itemname;
        private DateTime? _tm;
        private DateTime? _downdate;
        private int _nfoindex;
        private decimal? _datavalue;
        private int _itemdecimal;
        private bool QueueAdd(string data ,DataTable dt)
        {
            if (packover) //如果是结束包,插入空行
            {
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { null, null, null, null, null, null, null, null, null, null };
                dt.Rows.InsertAt(dr, 0);


                if (dt.Rows.Count > 100)
                {
                    dt.Rows.RemoveAt(100);
                }
                packover = false;
            }

            data = data.TrimStart(new char[] { ' ' });
            if (data.IndexOf("数据特征") == 0)
            {

                #region ItemID
                int start = data.IndexOf('[') + 1; //取左括号所在的位置
                int end = data.IndexOf(']');
                string result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                string[] id_name = result.Split(new char[] { '-' });
                if (id_name.Length == 2)
                {
                    _itemname = id_name[0];
                    _itemid = id_name[1];
                }
                #endregion

                #region 时间
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                _tm = DateTime.Parse(result);
                _downdate = DateTime.Now;
                #endregion

                #region DATAVALUE
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                _datavalue = decimal.Parse(result);
                #endregion

                #region ItemDecimal
                var list = from l in ITEMList where l.ItemCode == _itemid select l;
                if (list.Count() > 0)
                {
                    _itemdecimal = list.First().ItemDecimal;
                }
                #endregion

                #region 信道
                if (data.IndexOf("TCP") > 0)
                {
                    _nfoindex = 1;
                }
                else if (data.IndexOf("UDP") > 0)
                {
                    _nfoindex = 2;
                }
                else if (data.IndexOf("GSM") > 0)
                {
                    _nfoindex = 3;
                }
                else if (data.IndexOf("COM") > 0) { _nfoindex = 4; }
                #endregion

                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { _tm, _stcd, _datavalue, _name, _itemid, _itemname, _itemdecimal, null, _downdate, _nfoindex, null };
                dt.Rows.InsertAt(dr, 0);

                if (dt.Rows.Count > 100)
                {
                    dt.Rows.RemoveAt(100);
                }
                return true;
            }
            else if (data.IndexOf("数据特征") > 0)
            {
                #region  stcd
                int start = data.IndexOf('(') + 1; //取左括号所在的位置
                int end = data.IndexOf(')');
                string result=null;
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("(" + result + ")", "");
                _stcd = result;
                #endregion

                #region name
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                _name = result;
                #endregion

                #region ItemID
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                string[] id_name = result.Split(new char[] { '-' });
                if (id_name.Length == 2)
                {
                     _itemname= id_name[0];
                     _itemid= id_name[1];
                }
                #endregion

                #region 时间
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                _tm = DateTime.Parse(result);
                _downdate = DateTime.Now;
                #endregion

                #region DATAVALUE
                start = data.IndexOf('[') + 1; //取左括号所在的位置
                end = data.IndexOf(']');
                result = data.Substring(start, end - start);   //取两括号之间的字符串
                data = data.Replace("[" + result + "]", "");
                _datavalue = decimal.Parse(result);
                #endregion

                #region 信道
                if (data.IndexOf("TCP") > 0)
                {
                    _nfoindex = 1;
                }
                else if (data.IndexOf("UDP") > 0)
                {
                    _nfoindex = 2;
                }
                else if (data.IndexOf("GSM") > 0)
                {
                    _nfoindex = 3;
                }
                else if (data.IndexOf("COM") > 0)
                { _nfoindex = 4; }
                #endregion

                #region ItemDecimal
                var list = from l in ITEMList where l.ItemCode == _itemid select l;
                if (list.Count() > 0) 
                {
                    _itemdecimal = list.First().ItemDecimal;
                }
                #endregion

                DataRow dr= dt.NewRow();
                dr.ItemArray = new object[] { _tm, _stcd, _datavalue, _name, _itemid, _itemname, _itemdecimal, null, _downdate, _nfoindex, null };
               dt.Rows.InsertAt(dr,0);

                if (dt.Rows.Count > 100)
                {
                    dt.Rows.RemoveAt(100);
                }
                return true;
            }
            return false;
        }


        private void adTree_Init()
        {
            advTree_left.Nodes.Clear();
            DevComponents.AdvTree.Node node;
            if (ExecRTUList.Lrdm!=null)
            foreach (var item in ExecRTUList.Lrdm)
            {
                node = new DevComponents.AdvTree.Node();
                node.Text = "<font color='red'>" + item.NAME + "(" + item.STCD + ")</font>";
                node.Tooltip = item.STCD;
                //node.NodeClick += new EventHandler(node_NodeClick);
                advTree_left.Nodes.Add(node);
            }
        }

        #region 各线程执行方法
        private void UpdadvTree_left()
        {
            if (ExecRTUList.Lrdm != null && ExecRTUList.Lrdm.Count()>0)
                foreach (DevComponents.AdvTree.Node item in advTree_left.Nodes)
                {
                    var rtu = from r in ExecRTUList.Lrdm where r.STCD == item.Tooltip  select r;
                    if (rtu.Count() > 0)
                    if (rtu.First().SERVICETYPE == "udp")
                    {
                        item.Text = "<font color='#2bc102'>" + rtu.First().NAME + "(" + rtu.First().STCD + ")" + "--udp</font>";
                    }
                    else if (rtu.First().SERVICETYPE == "tcp")
                    { item.Text = "<font color='#2bc102'>" + rtu.First().NAME + "(" + rtu.First().STCD + ")" + "--tcp</font>"; }
                    else
                    { item.Text = "<font color='red'>" + rtu.First().NAME + "(" + rtu.First().STCD + ")" + "</font>"; }
                }
        }
        private void ThreadUpdadvTree_left()
        {
            int second =advTree_left.Nodes.Count  / 50;
            if ( second==0)
            { second = 1; }
            else if (second >10)
            { second = 10; }

            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                        this.Invoke(new DelegateUpdadvTree_left(UpdadvTree_left));
                    }
                    catch { }
                }
                Thread.Sleep(second*1000);
            }
        }


        RTF rtf = new RTF();
        private void AddRealTimeData(DataTable dt) 
        {
            if (richTextBoxEx_Fill1.InvokeRequired)
            {
                DelegateAddData d = new DelegateAddData(AddRealTimeData);
                Invoke(d, new object[] { dt });
            }
            else
            {
                if (dt != null)
                    richTextBoxEx_Fill1.Rtf = rtf.GetRealTimeDataRtfString(dt);
            }
        }
        private void AddRealTimeNewData(DataTable dt)
        {
            if (richTextBoxEx_Fill2.InvokeRequired)
            {
                DelegateAddData d = new DelegateAddData(AddRealTimeNewData);
                Invoke(d, new object[] { dt });
            }
            else
            {
                if (dt != null)
                {
                    richTextBoxEx_Fill2.Rtf = rtf.GetRealTimeDataRtfString(dt);
                }
            }
        }
        DataTable dt;
        private void ThreadAddRealTimeData()
        {
            //while (true)
            //{
                if (PublicBD.ConnectState)
                {
                    dt = PublicBD.db.GetRealTimeData();
                    AddRealTimeData(dt);

                    DataTable NewDatadt = PublicBD.db.GetRealTimeNewData();
                    AddRealTimeNewData(DistinctSTSDTable(NewDatadt));
                }
                
            //    Thread.Sleep(5000);
            //}
        }

        //取每站数据的第一条
        private DataTable DistinctSTSDTable(DataTable olddt) 
        {
            DataTable NewTable = olddt.Clone();
            foreach (DataRow row in olddt.Rows)
            {
                DataRow dr = NewTable.NewRow();
                dr.ItemArray = row.ItemArray ;
                if (NewTable.Select("stcd='" + dr["stcd"].ToString() + "'").Count() == 0)
                {
                    NewTable.Rows.Add(dr);
                }
            }

            return NewTable;
        }

        #region//过滤数据
        private DataTable FilterRealTimeNewData(string ItemID) 
        {
            if (Program.wrx.XMLObj.dllfile.ToLower() == "gsprotocol.dll")
            {
                DataTable newdt = dt.Clone();
                if (ItemID == "12")
                {
                    return dt = PublicBD.db.GetRealTimeNewData(new string[]{"12","180","181"});
                }
                else if (ItemID == "15")
                {
                    return dt = PublicBD.db.GetRealTimeNewData(new string[] { "15", "182", "183" });
                }
                else if (ItemID == "16")
                {
                    return dt = PublicBD.db.GetRealTimeNewData(new string[] { "16", "184", "185" });
                }
               
            }
            return dt = PublicBD.db.GetRealTimeNewData(new string[] { ItemID });
        }
        private DataTable FilterRealTimeData(string STCD, string ItemID)
        {
            if (Program.wrx.XMLObj.dllfile .ToLower() == "gsprotocol.dll")
            {
                DataTable newdt = dt.Clone();
                if (ItemID == "12")
                {
                    return dt = PublicBD.db.GetRealTimeData(STCD, new string[] { "12", "180", "181" });
                }
                else if (ItemID == "15")
                {
                    return dt = PublicBD.db.GetRealTimeData(STCD,new string[] { "15", "182", "183" });
                }
                else if (ItemID == "16")
                {
                    return dt = PublicBD.db.GetRealTimeData(STCD,new string[] { "16", "184", "185" });
                }

            }
            return dt = PublicBD.db.GetRealTimeData(STCD,new string[] { ItemID });
        } 
        #endregion

        //不用了
        void node_NodeClick(object sender, EventArgs e)
        {
            superTabControl_Fill.SelectedTabIndex = 2;

            if (PublicBD.ConnectState)
            {
                //测站点击事件下显示单站数据
                DataTable dt = PublicBD.db.GetRealTimeData((sender as DevComponents.AdvTree.Node).Tooltip);
                if (dt != null)
                    richTextBoxEx_Fill3.Rtf = rtf.GetRealTimeDataRtfString(dt);

                //测站点击事件下显示单站状态数据
                dt = PublicBD.db.GetRTUState((sender as DevComponents.AdvTree.Node).Tooltip);
                dt = PublicBD.db.CreateRTUStateDataTable(dt);
                if (dt != null)
                    richTextBoxEx_Fill5.Rtf = rtf.GetRTUStateRtfString(dt);
            }
        }

        //不用了
        private void AddRTUNewState(DataTable dt)
        {
            if (richTextBoxEx_Fill4.InvokeRequired)
            {
                DelegateAddData d = new DelegateAddData(AddRTUNewState);
                Invoke(d, new object[] { dt });
            }
            else
            {
                if (dt != null)
                    richTextBoxEx_Fill4.Rtf = rtf.GetRTUStateRtfString(dt);
            }
        }

        //不用了
        private void ThreadAddRTUState()
        {
            while (true)
            {
                if (PublicBD.ConnectState)
                {
                    DataTable dt = PublicBD.db.GetRTUNewState();
                    dt = PublicBD.db.CreateRTUStateDataTable(dt);

                    AddRTUNewState(dt);
                }
                Thread.Sleep(5000);
            }
        }

        //private void AddCMDState()
        //{
        //    listViewEx_Right.Items.Clear();
        //    ListViewItem lvi = null;
        //    ListViewItem.ListViewSubItem lvsi = null;


        //    //状态 过期的命令 从列表中删除
        //    lock (ExecCommandList.LC)
        //        foreach (Command cmd in ExecCommandList.LC)
        //        {
        //            if ((cmd.STATE == -1 || cmd.STATE == -2) && cmd.DATETIME.AddSeconds(5 * 60) < DateTime.Now)
        //            {
        //                ExecCommandList.LC.Remove(cmd);
        //                break;
        //            }
        //        }

        //    foreach (var cmd in ExecCommandList.LC)
        //    {
        //        lvi = new ListViewItem();

        //        foreach (var item in ExecRTUList.Lrdm)
        //        {
        //            if (item.STCD == cmd.STCD)
        //            {
        //                lvi.Tag = cmd.STCD;
        //                lvi.Text = item.NAME;
        //                break;
        //            }
        //        }


        //        foreach (var item in ExecCommandList.Commands)
        //        {
        //            if (item.CommandID == cmd.CommandID)
        //            {
        //                lvsi = new ListViewItem.ListViewSubItem();
        //                lvsi.Tag = item.CommandID;
        //                lvsi.Text = item.Remark;
        //                lvi.SubItems.Add(lvsi);
        //                break;
        //            }
        //        }



        //        if (cmd.STATE == 0)
        //        { lvi.ImageIndex = 0; }
        //        else if (cmd.STATE > 0 && cmd.STATE <= 3)
        //        { lvi.ImageIndex = 1; }
        //        else if (cmd.STATE == -1)
        //        { lvi.ImageIndex = 2; }
        //        else
        //        { lvi.ImageIndex = 3; }


        //        lvsi = new ListViewItem.ListViewSubItem();
        //        lvsi.Text = cmd.DATETIME.ToString("MM月dd日 HH时mm分ss秒");
        //        lvi.SubItems.Add(lvsi);


        //        lvi.ToolTipText = @"站号：" + cmd.STCD + "\n" +
        //                           "命令码：" + cmd.CommandID + "\n" +
        //                           "服务类型：" + cmd.SERVICETYPE;


        //        listViewEx_Right.Items.Add(lvi);
        //    }


        //}
        //private void ThreadAddCMDState()
        //{
        //    while (true)
        //    {
        //        // 判断是否需要Invoke，多线程时需要
        //        if (this.InvokeRequired)
        //        {
        //            // 通过委托调用写主线程控件的程序，传递参数放在object数组中
        //            this.Invoke(new DelegateAddCMDData(AddCMDState));
        //        }
        //        Thread.Sleep(5000);
        //    }
        //}
        #endregion


        //bool addrealtimedataState = true;
        //private void superTabItem1_DoubleClick(object sender, EventArgs e)
        //{
        //    if (addrealtimedataState)
        //    {
        //        addrealtimedataState = false;
        //        superTabItem1.Tooltip ="双击该区域启动刷新数据";
        //        superTabItem2.Tooltip = "双击该区域启动刷新数据";
        //        addrealtimedata.Suspend();
        //    }
        //    else
        //    {
        //        addrealtimedataState = true;
        //        superTabItem1.Tooltip ="双击该区域停止刷新数据";
        //        superTabItem2.Tooltip = "双击该区域停止刷新数据";
        //        addrealtimedata.Resume();
        //    }
        //}

        //private void superTabItem2_DoubleClick(object sender, EventArgs e)
        //{
        //    if (addrealtimedataState)
        //    {
        //        addrealtimedataState = false;
        //        superTabItem1.Tooltip = "双击该区域启动刷新数据";
        //        superTabItem2.Tooltip = "双击该区域启动刷新数据";
        //        addrealtimedata.Suspend();
        //    }
        //    else
        //    {
        //        addrealtimedataState = true;
        //        superTabItem1.Tooltip = "双击该区域停止刷新数据";
        //        superTabItem2.Tooltip = "双击该区域停止刷新数据";
        //        addrealtimedata.Resume();
        //    }
        //}




        private void richTextBoxEx_bottom_DoubleClick(object sender, EventArgs e)
        {
            if (richTextBoxEx_bottom.SelectedText != "")
            {
                MessageBox.Show(richTextBoxEx_bottom.SelectedText);
            }
        }

        #region 弹出菜单(右侧)
        ////右键弹出菜单
        //private void listViewEx_Right_MouseClick(object sender, MouseEventArgs e)
        //{
        //    //判断是否点击为右键
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (listViewEx_Right.GetItemAt(e.X, e.Y) != null)
        //        {
        //            //第1参数表示右键菜单的控父件，第2参数为显示坐标
        //            this.contextMenuStrip_Right.Show(sender as Control, e.Location);

        //            lvi = listViewEx_Right.GetItemAt(e.X, e.Y);
        //        }
        //    }
        //}

        ////同步
        //private void toolStripMenuItem_Syn_Click(object sender, EventArgs e)
        //{
        //    ExecCommandList.LC.Clear();
        //    listViewEx_Right.Items.Clear();
        //    if (tcpclient.socket.Connected)
        //        tcpclient.socket.Send(Encoding.UTF8.GetBytes("--cmd|"));
        //}

        ////全部删除
        //private void toolStripMenuItem_DelAll_Click(object sender, EventArgs e)
        //{
        //    ExecCommandList.LC.Clear();
        //    listViewEx_Right.Items.Clear();
        //    if (tcpclient.socket.Connected)
        //        tcpclient.socket.Send(Encoding.UTF8.GetBytes("--cmd|clear"));
        //}

        ////删除
        //ListViewItem lvi = null;
        //private void toolStripMenuItem_Del_Click(object sender, EventArgs e)
        //{
        //    if (lvi != null)
        //    {
        //        ListViewItem.ListViewSubItem lvsi = lvi.SubItems[1];
        //        string SERVICETYPE = lvi.ToolTipText.Split(new string[] { "\n" }, StringSplitOptions.None)[2].Split(new char[] { '：' })[1];
        //        var commands = from c in ExecCommandList.LC where c.STCD == lvi.Tag.ToString() && c.CommandID == lvsi.Tag.ToString() && c.SERVICETYPE == SERVICETYPE select c;
        //        if (commands.Count() > 0)
        //        {
        //            //删除服务器端列表中的召测命令     --cmd|tcp|0012345679|02
        //            if (tcpclient.socket.Connected)
        //                tcpclient.socket.Send(Encoding.UTF8.GetBytes("--cmd|" + commands.First().SERVICETYPE + "|" + commands.First().STCD + "|" + commands.First().CommandID));
        //            //删除本地列表和控件中的命令
        //            listViewEx_Right.Items.Remove(lvi);
        //            lock (ExecCommandList.LC)
        //            {
        //                List<Command> cmds = new List<Command>(commands);
        //                foreach (var item in cmds)
        //                {
        //                    ExecCommandList.LC.Remove(item);
        //                }
        //            }
        //        }
        //        lvi = null;
        //    }
        //}
        #endregion

        #region 弹出菜单(左侧)
        private void advTree_left_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否点击为右键
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip_Left.Show(sender as Control, e.Location);

                contextMenuStripItemAdd();
            }
        }

        private void contextMenuStripItemAdd()
        {
            if (contextMenuStrip_Left.Items.Count == 0 || (contextMenuStrip_Left.Items.Count > 0 && contextMenuStrip_Left.Items[0].Text != "重新读取"))
            {
                ToolStripMenuItem toolStripMenuItem_Reload = new ToolStripMenuItem();
                contextMenuStrip_Left.Items.Insert(0, toolStripMenuItem_Reload);
                toolStripMenuItem_Reload.Name = "toolStripMenuItem_Reload";
                toolStripMenuItem_Reload.Text = "重新读取";
                toolStripMenuItem_Reload.Click += new System.EventHandler(this.toolStripMenuItem_Reload_Click);
            }
        }


        private void comboBox_Item_Init()
        {
            IList<Service.Model.YY_RTU_ITEM> ItemList = PublicBD.db.GetItemList(" where ItemCode!='-1' and ItemCode!='0000000000'");
            if (ItemList != null && ItemList.Count > 0)
            {
                ITEMList = ItemList;
                comboBox_Item.DataSource = ItemList;
                comboBox_Item.DisplayMember = "ItemName";
                comboBox_Item.ValueMember = "ItemID";
                comboBox_Item.SelectedIndex = 0;
            }
        }

        //重新读取测站列表
        private void toolStripMenuItem_Reload_Click(object sender, EventArgs e)
        {
            ExecRTUList.SetLrdm(Service.PublicBD.db.GetRTUList(""));
            adTree_Init();
        }
        #endregion

        string STCD_Note = null;
        private void advTree_left_CellSelected(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            superTabControl_Fill.SelectedTabIndex = 2;
            
            if (PublicBD.ConnectState)
            {
                STCD_Note = (e.Cell.Parent as DevComponents.AdvTree.Node).Tooltip;
                //测站点击事件下显示单站数据
                DataTable dt = PublicBD.db.GetRealTimeData(STCD_Note);
                if (dt != null)
                {
                    richTextBoxEx_Fill3.Rtf = rtf.GetRealTimeDataRtfString(dt);
                    contextMenuStripItemAddForDT(dt);
                }

                //测站点击事件下显示单站状态数据
                dt = PublicBD.db.GetRTUState((e.Cell.Parent as DevComponents.AdvTree.Node).Tooltip);
                dt = PublicBD.db.CreateRTUStateDataTable(dt);
                if (dt != null)
                {
                    richTextBoxEx_Fill5.Rtf = rtf.GetRTUStateRtfString(dt);
                }
            }
        }

        private void contextMenuStripItemAddForDT(DataTable dt)
        {
            if (Program.LoginState) //必须先登录才能执行右键菜单数据
            {
                contextMenuStrip_Left.Items.Clear();
                contextMenuStrip_Left.Items.Add("------------------------");

                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dt = dv.ToTable(true, new string[] { "ItemID", "ItemName" });
                    dv = new DataView(dt);    //dtname指需去掉重复行的datatable名
                    dt = dv.ToTable(true);


                    ToolStripMenuItem _toolStripMenuItem;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        _toolStripMenuItem = new ToolStripMenuItem();
                        contextMenuStrip_Left.Items.Add(_toolStripMenuItem);
                        _toolStripMenuItem.Tag = dt.Rows[i]["ItemID"].ToString();
                        _toolStripMenuItem.Text = dt.Rows[i]["ItemName"].ToString();
                        _toolStripMenuItem.Click += new EventHandler(_toolStripMenuItem_Click);
                    }

                }
            }
        }

        void _toolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            ((MainForm)this.ParentForm).pageSlider1.SelectedPageIndex = 3;
            ((MainForm)this.ParentForm).pageSliderPage4.Controls.Clear();
            SetControl.SetRealTimeControl stc = new SetControl.SetRealTimeControl((sender as ToolStripMenuItem).Tag.ToString(),STCD_Note);
            stc.Dock = DockStyle.Fill;
            ((MainForm)this.ParentForm).pageSliderPage4.Controls.Add(stc);
        }
       
        private void button_close_Click(object sender, EventArgs e)
        {
            this.richTextBoxEx_bottom.Text = "";
        }

        private void superTabItem4_Click(object sender, EventArgs e)
        {
            if (PublicBD.ConnectState)
            {
                DataTable dt = PublicBD.db.GetRTUNewState();
                dt = PublicBD.db.CreateRTUStateDataTable(dt);

                AddRTUNewState(dt);
            }
        }

        private void superTabItem2_Click(object sender, EventArgs e)
        {
            if (PublicBD.ConnectState)
            {
                DataTable NewDatadt = PublicBD.db.GetRealTimeNewData();
                AddRealTimeNewData(DistinctSTSDTable(NewDatadt));
            }
        }

        private void comboBox_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt != null)
            {
                DataTable NewDt = null;
                string ItemID=comboBox_Item.SelectedValue.ToString();
                if (ItemID == "-1")
                {
                    NewDt = PublicBD.db.GetRealTimeNewData();
                }
                else
                {
                    NewDt = FilterRealTimeNewData(ItemID);
                }

                if (NewDt!=null)
                richTextBoxEx_Fill2.Rtf = rtf.GetRealTimeDataRtfString(NewDt);
            }
        }

        private void comboBox_Item1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt != null)
            {
                DataTable NewDt = null;
                string ItemID = comboBox_Item1.SelectedValue.ToString();
                if (ItemID == "-1")
                {
                    NewDt = PublicBD.db.GetRealTimeData(STCD_Note);
                }
                else
                {
                    NewDt = FilterRealTimeData(STCD_Note, ItemID);
                }
                richTextBoxEx_Fill3.Rtf = rtf.GetRealTimeDataRtfString(NewDt);
            }
        }

      
       }
}
