using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp.SetControl
{
    public partial class ShowCommandControl : UserControl
    {
        public ShowCommandControl()
        {
            InitializeComponent();
            advTree1.ImageList = PubObject.imgList;
        }


        private delegate void DelegateUpdadvTree_Right();
        private void adTree_Right_Init()
        {

            DevComponents.AdvTree.Node node;
            if (ExecCommandList.LC  != null)
            {
                RemoveLC();
                foreach (var cmd in ExecCommandList.LC)
                {
                    bool b = false;
                    foreach (DevComponents.AdvTree.Node Node in advTree1.Nodes)
                    {
                        if (Node.Tag.ToString() == cmd.STCD && Node.Cells[1].Tag.ToString() == cmd.CommandID && Node.Cells[2].Tag .ToString()==cmd.SERVICETYPE )
                        {
                            //图片更新
                            if (cmd.STATE == 0)
                            { Node.ImageIndex = 0; }
                            else if (cmd.STATE > 0 && cmd.STATE <= 3)
                            { Node.ImageIndex = 1; }
                            else if (cmd.STATE == -1)
                            { Node.ImageIndex = 2; }
                            else
                            { Node.ImageIndex = 3; }

                            Node.Cells[2].Text = cmd.DATETIME.ToString("MM月dd日 HH时mm分ss秒");
                            b = true;
                        }
                    }

                    if (!b )
                    {

                        node = new DevComponents.AdvTree.Node();
                        foreach (var item in ExecRTUList.Lrdm)
                        {
                            if (item.STCD == cmd.STCD)
                            {
                                node.Tag = cmd.STCD;
                                node.Text = item.NAME;
                                break;
                            }
                        }


                        foreach (var item in ExecCommandList.Commands)
                        {
                            if (item.CommandID == cmd.CommandID)
                            {
                                DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                                cell.Tag = item.CommandID;
                                cell.Text = item.Remark;
                                cell.ImageAlignment = DevComponents.AdvTree.eCellPartAlignment.Default;
                                node.Cells.Add(cell);
                                break;
                            }
                        }


                        if (cmd.STATE == 0)
                        { node.ImageIndex = 0; }
                        else if (cmd.STATE > 0 && cmd.STATE <= 3)
                        { node.ImageIndex = 1; }
                        else if (cmd.STATE == -1)
                        { node.ImageIndex = 2; }
                        else
                        { node.ImageIndex = 3; }

                        DevComponents.AdvTree.Cell cell1 = new DevComponents.AdvTree.Cell();
                        cell1.Text = cmd.DATETIME.ToString("MM月dd日 HH时mm分ss秒");
                        cell1.Tag = cmd.SERVICETYPE;
                        node.Cells.Add(cell1);
                        node.Cells.Add(new DevComponents.AdvTree.Cell());

                        node.Tooltip =  @"测站：" + node.Cells[0].Text  + "\n" +
                                         "站号：" + cmd.STCD + "\n" +
                                         "命令码：" + cmd.CommandID + "\n" +
                                         "命令：" + node.Cells[1].Text + "\n" +
                                        "服务类型：" + cmd.SERVICETYPE +"\n"+
                                        "时间：" + node.Cells[2].Text; 

                        advTree1.Nodes.Add(node);
                        node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Hidden;
                    }
                }



            }
        }
        //状态 过期的命令 从列表中删除
        private void RemoveLC()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (var cmd in ExecCommandList.LC)
            {
                if ((cmd.STATE == -1 || cmd.STATE == -2) && cmd.DATETIME.AddSeconds(60) < DateTime.Now)
                {
                    list.Add(cmd);
                }
            }
            foreach (var item in list)
            {
                ExecCommandList.LC.Remove(item as Command);
            }
        }

        private void ThreadUpdadvTree_Right()
        {
            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                        this.Invoke(new DelegateUpdadvTree_Right(adTree_Right_Init));
                    }
                    catch { }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void RemoveAdTreeNode() 
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (DevComponents.AdvTree.Node Node in advTree1.Nodes)
            {
                if ((Node.ImageIndex == 2 || Node.ImageIndex == 3)&& DateTime.Parse(DateTime.Now.Year + "年" + Node.Cells[2].Text).AddSeconds(60) < DateTime.Now) 
                {
                    list.Add(Node);
                }
            }



            foreach (var item in list)
            {
                advTree1.Nodes.Remove(item as DevComponents.AdvTree.Node);
            }
        }
        private void RemoveNode()
        {
            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                        this.Invoke(new DelegateUpdadvTree_Right(RemoveAdTreeNode));
                    }
                    catch { }
                }
                System.Threading.Thread.Sleep(5000);
            }
        }
        private void ShowCommandControl_Load(object sender, EventArgs e)
        {
            //更新树形控件中命令状态
            System.Threading.Thread updadvtree_Right = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadUpdadvTree_Right));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            updadvtree_Right.IsBackground = true;
            updadvtree_Right.Start();


            //更新树形控件中命令状态
            System.Threading.Thread RemoveNodeThread = new System.Threading.Thread(new System.Threading.ThreadStart(RemoveNode));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            RemoveNodeThread.IsBackground = true;
            RemoveNodeThread.Start();
        }

        public void SetColor(Color color) 
        {
            Color c = color;
            int Alpha = 30;
            int R = 255 + (c.R - 255) * Alpha / 255;
            int G = 255 + (c.G - 255) * Alpha / 255;
            int B = 255 + (c.B - 255) * Alpha / 255;
            elementStyle4.BackColor = Color.FromArgb(R, G, B);
            elementStyle3.BackColor = color;
            advTree1.BackgroundStyle.BorderColor = color;
            advTree1.GridLinesColor = color;
        }

        #region 右键
        DevComponents.AdvTree.Node node = null;
        private void advTree1_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否点击为右键
            if (e.Button == MouseButtons.Right)
            {
                if (advTree1.GetNodeAt(e.X, e.Y) != null)
                {
                    //第1参数表示右键菜单的控父件，第2参数为显示坐标
                    contextMenuStrip_Right.Show(sender as Control, e.Location);

                    node = advTree1.GetNodeAt(e.X, e.Y);
                }
            }
        }

        //同步
        private void toolStripMenuItem_Syn_Click(object sender, EventArgs e)
        {
            ExecCommandList.LC.Clear();
            advTree1.Nodes.Clear();
            if (TcpControl.Connected)
                TcpControl.SendUItoServiceCommand("--cmd|");
        }

        //全部删除
        private void toolStripMenuItem_DelAll_Click(object sender, EventArgs e)
        {
            ExecCommandList.LC.Clear();
            advTree1.Nodes.Clear();
            if (TcpControl.Connected)
               TcpControl.SendUItoServiceCommand("--cmd|clear");
        }

        //删除
        private void toolStripMenuItem_Del_Click(object sender, EventArgs e)
        {
            if (node != null)
            {
                DevComponents.AdvTree.Cell cell = node.Cells[1];
                DevComponents.AdvTree.Cell cell1 = node.Cells[2];
                string SERVICETYPE = cell1.Tag .ToString();
                var commands = from c in ExecCommandList.LC where c.STCD == node.Tag.ToString() && c.CommandID == cell.Tag.ToString() && c.SERVICETYPE == SERVICETYPE select c;
                if (commands.Count() > 0)
                {
                    //删除服务器端列表中的召测命令     --cmd|tcp|0012345679|02
                    if (TcpControl.Connected)
                        TcpControl.SendUItoServiceCommand("--cmd|" + commands.First().SERVICETYPE + "|" + commands.First().STCD + "|" + commands.First().CommandID);
                    //删除本地列表和控件中的命令
                    DevComponents.AdvTree.Node Nd = null;
                    foreach (var item in advTree1.Nodes)
                    {
                        if (item.Equals(node))
                        {
                            Nd = item as DevComponents.AdvTree.Node;
                            break;
                        }
                    }
                    if (Nd != null)
                        advTree1.Nodes.Remove(Nd);
                    
                    lock (ExecCommandList.LC)
                    {
                        List<Command> cmds = new List<Command>(commands);
                        foreach (var item in cmds)
                        {
                            ExecCommandList.LC.Remove(item);
                        }
                    }
                }
                node = null;
            }
        }
        #endregion

    }
}
