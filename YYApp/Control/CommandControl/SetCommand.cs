                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace YYApp.CommandControl
{
    
    public partial class SetCommand : UserControl
    {
        private DevComponents.DotNetBar.Controls.PageSlider ps;
        public SetCommand(Color color, Color backcolor,  DevComponents.DotNetBar.Controls.PageSlider pageSlider1)
        {
            InitializeComponent();

            ps = pageSlider1;

            panelEx_Fill.Style.BorderColor.Color = color;
            panelEx3.Style.BorderColor.Color = color;
            panelEx4.Style.BorderColor.Color = color;
            panelEx_Fill.Style.BorderColor.Color = color;

            advTree_left.BackgroundStyle.BorderColor = color;
            itemPanel_Command.BackgroundStyle.BorderColor = color;

            expandableSplitter1.BackColor2 = color;
            expandableSplitter2.BackColor2 = color;
            expandableSplitter3.BackColor2 = color;


            advTree_left.BackColor = backcolor;
            itemPanel_Command.BackColor = backcolor;
            
            panelEx_Fill.Style.BackColor1.Color = backcolor;
            panelEx3.Style.BackColor1.Color = backcolor;
            panelEx4.Style.BackColor1.Color = backcolor;

            cbBNFOINDEX.SelectedIndex = 0;


            adTree_Init();
            itemPanel_Command_Init();


            scc_Init(color);
        }
        public YYApp.SetControl.ShowCommandControl scc = null;
        public void scc_Init(Color color)
        {
            panelEx1.Controls.Clear();
            scc = new YYApp.SetControl.ShowCommandControl();
            scc.Dock = DockStyle.Fill;
            scc.SetColor(color);
            panelEx1.Controls.Add(scc);
        }


        private void adTree_Init()
        {
            advTree_left.Nodes.Clear();
            DevComponents.AdvTree.Node node;
            foreach (var item in ExecRTUList.Lrdm)
            {
                node = new DevComponents.AdvTree.Node();
                node.CheckBoxVisible = true;
                node.Text = "<font color='red'>" + item.NAME + "(" + item.STCD + ")</font>";
                node.Tooltip = item.STCD;
                advTree_left.Nodes.Add(node);
            }
        }

        private void itemPanel_Command_Init()
        {
            IList<Service.Model.YY_RTU_COMMAND> Commands = Service.PublicBD.db.GetRTUCommandList();
            DevComponents.DotNetBar.BaseItem[] BI = new DevComponents.DotNetBar.BaseItem[Commands.Count];
            DevComponents.DotNetBar.ButtonItem bi = null;

            int i = 0;
            foreach (var item in Commands)
            {
                bi = new DevComponents.DotNetBar.ButtonItem();
                bi.Click += new EventHandler(bi_Click);
                bi.Text = item.Remark;
                bi.Tooltip = "(" + item.CommandID + ")" + item.Remark;
                bi.Tag = item.CommandID;
                BI[i] = bi;
                i++;
            }

            this.itemPanel_Command.Items.AddRange(BI);
        }

        string CommandCode = null;
        ICommandControl cc = null;
        void bi_Click(object sender, EventArgs e)
        {
            foreach (var item in itemPanel_Command.Items)
            {
                (item as DevComponents.DotNetBar.ButtonItem).Checked = false;
            }

            string[] stcds = GetStcds();
            if (stcds.Count() == 0)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DevComponents.DotNetBar.ButtonItem bi = sender as DevComponents.DotNetBar.ButtonItem;
            bi.Checked = true;

            panelEx_Fill.Controls.Clear();
            if (Program.wrx.ReadDllXML().ToLower() == "gsprotocol.dll")
            {
                switch (bi.Tag.ToString())
                {
                    #region 燕禹协议
                    case "0067":
                        {
                            cc = new YYApp.CommandControl._103();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "0069":
                        {
                            cc = new YYApp.CommandControl._105(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "006C":
                        {
                            cc = new YYApp.CommandControl._108();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "006D":
                        {
                            cc = new YYApp.CommandControl._109();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }

            if (Program.wrx.ReadDllXML().ToLower() == "protocol.dll") 
            {
                switch (bi.Tag.ToString())
                {
                    #region 水资源
                    case "02":
                        {
                            cc = new CommandControl._02();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "10":
                        {
                            cc = new YYApp.CommandControl._10();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "50":
                        {
                            cc = new YYApp.CommandControl._10();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "11":
                        {
                            cc = new YYApp.CommandControl._11();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "51":
                        {
                            cc = new YYApp.CommandControl._11();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "12":
                        {
                            cc = new YYApp.CommandControl._12(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "52":
                        {
                            cc = new YYApp.CommandControl._12(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "15":
                        {
                            cc = new YYApp.CommandControl._15();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "53":
                        {
                            cc = new YYApp.CommandControl._A1(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "54":
                        {
                            cc = new YYApp.CommandControl._A0(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "55":
                        {
                            cc = new YYApp.CommandControl._15();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "16":
                        {
                            cc = new YYApp.CommandControl._16();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "56":
                        {
                            cc = new YYApp.CommandControl._16();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "17":
                        {
                            cc = new YYApp.CommandControl._17(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "57":
                        {
                            cc = new YYApp.CommandControl._17(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "18":
                        {
                            cc = new YYApp.CommandControl._18(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "58":
                        {
                            cc = new YYApp.CommandControl._18(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "19":
                        {
                            cc = new YYApp.CommandControl._19(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "20":
                        {
                            cc = new YYApp.CommandControl._20(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "59":
                        {
                            cc = new YYApp.CommandControl._19(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1A":
                        {
                            cc = new YYApp.CommandControl._1A(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "5A":
                        {
                            cc = new YYApp.CommandControl._1A(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1B":
                        {
                            cc = new YYApp.CommandControl._1B(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1C":
                        {
                            cc = new YYApp.CommandControl._1C(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "60":
                        {
                            cc = new YYApp.CommandControl._1C(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1D":
                        {
                            cc = new YYApp.CommandControl._1D(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "62":
                        {
                            cc = new YYApp.CommandControl._1D(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1E":
                        {
                            cc = new YYApp.CommandControl._1E(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "63":
                        {
                            cc = new YYApp.CommandControl._1E(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "1F":
                        {
                            cc = new YYApp.CommandControl._1F(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "64":
                        {
                            cc = new YYApp.CommandControl._1F(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "30":
                        {
                            cc = new YYApp.CommandControl._30(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "31":
                        {
                            cc = new YYApp.CommandControl._30(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "32":
                        {
                            cc = new YYApp.CommandControl._32(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "33":
                        {
                            cc = new YYApp.CommandControl._32(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "34":
                        {
                            cc = new YYApp.CommandControl._34(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "90":
                        {
                            cc = new YYApp.CommandControl._90();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "91":
                        {
                            cc = new YYApp.CommandControl._91();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "92":
                        {
                            cc = new YYApp.CommandControl._92(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "93":
                        {
                            cc = new YYApp.CommandControl._92(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "94":
                        {
                            cc = new YYApp.CommandControl._94();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "95":
                        {
                            cc = new YYApp.CommandControl._95();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "96":
                        {
                            cc = new YYApp.CommandControl._96(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "A0":
                        {
                            cc = new YYApp.CommandControl._A0(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "A1":
                        {
                            cc = new YYApp.CommandControl._A1(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "5D":
                        {
                            cc = new YYApp.CommandControl._5D();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "5E":
                        {
                            cc = new YYApp.CommandControl._5E();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "5F":
                        {
                            cc = new YYApp.CommandControl._5F();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "B0":
                        {
                            cc = new YYApp.CommandControl._B1();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "B1":
                        {
                            cc = new YYApp.CommandControl._B1();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "B2":
                        {
                            cc = new YYApp.CommandControl._B2();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }

            if (Program.wrx.ReadDllXML().ToLower() == "hydrologicprotocol.dll")
            {
                switch (bi.Tag.ToString())
                {
                    #region 水文
                    case "00": 
                        {
                            cc = new CommandControl._00(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "36":
                        {
                            cc = new CommandControl._36();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "37":
                        {
                            cc = new YYApp.CommandControl._37();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "38":
                        {
                            cc = new YYApp.CommandControl._38();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "39":
                        {
                            cc = new YYApp.CommandControl._39();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "3A":
                        {
                            cc = new YYApp.CommandControl._3A();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "40":
                        {
                            cc = new YYApp.CommandControl._40(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "41":
                        {
                            cc = new YYApp.CommandControl._41();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "42":
                        {
                            cc = new YYApp.CommandControl._42(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "43":
                        {
                            cc = new YYApp.CommandControl._43(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "44":
                        {
                            cc = new YYApp.CommandControl._44();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "45":
                        {
                            cc = new YYApp.CommandControl._45();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "46":
                        {
                            cc = new YYApp.CommandControl._46();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "47":
                        {
                            cc = new YYApp.CommandControl._47();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "48":
                        {
                            cc = new YYApp.CommandControl._48();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "49":
                        {
                            cc = new YYApp.CommandControl._49(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4A":
                        {
                            cc = new YYApp.CommandControl._4A();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4B":
                        {
                            cc = new YYApp.CommandControl._4B(GetStcds());
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4C":
                        {
                            cc = new YYApp.CommandControl._4C();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4D":
                        {
                            cc = new YYApp.CommandControl._4D();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4E":
                        {
                            cc = new YYApp.CommandControl._4E();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "4F":
                        {
                            cc = new YYApp.CommandControl._4F();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "50":
                        {
                            cc = new YYApp.CommandControl._50();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                    case "51":
                        {
                            cc = new YYApp.CommandControl._51();
                            panelEx_Fill.Controls.Add((Control)cc);
                        }
                        break;
                  
                    #endregion
                    default:
                        break;
                }
            }
        }



        /// <summary>
        /// 发送召测命令
        /// </summary>
        /// <param name="Commands">命令集</param>
        /// <param name="stcds">站号集</param>
        /// <param name="CommandCode">命令码</param>
        private void SendCommand(string[] Commands, string[] stcds, string CommandCode)
        {
            //++tcp|数据报|stcd
            string header = "";
            switch (cbBNFOINDEX.SelectedIndex)
            {
                case 0:
                    header = "++tcp|";
                    break;
                case 1:
                    header = "++udp|";
                    break;
                case 2:
                    header = "++gsm|";
                    break;
                default:
                    header = "++com|";
                    break;
            }
            if (TcpControl.Connected)
            {
                for (int i = 0; i < Commands.Length; i++)
                {
                    TcpControl.SendUItoServiceCommand(header + Commands[i] + "|" + stcds[i] + "|" + CommandCode + "\n");
                    System.Threading.Thread.Sleep(5);
                }
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("系统与服务连接异常！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 得到选中测站站号
        /// </summary>
        /// <returns></returns>
        private string[] GetStcds()
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            foreach (var item in advTree_left.Nodes)
            {
                if ((item as DevComponents.AdvTree.Node).Checked)
                {
                    al.Add((item as DevComponents.AdvTree.Node).Tooltip);
                }
            }
            string[] stcds = new string[al.Count];
            for (int i = 0; i < al.Count; i++)
            {
                stcds[i] = al[i].ToString();
            }

            return stcds;
        }

        private void BTback_Click(object sender, EventArgs e)
        {
            ps.SelectedPageIndex = 0;
        }

        private void SetCommand_Load(object sender, EventArgs e)
        {
            //添加命令状态数据
            Thread addcmdstate = new Thread(new ThreadStart(ThreadAddCMDState));
            // 设置为背景线程，主线程一旦退出，该线程也不等待而立即结束
            addcmdstate.IsBackground = true;
            addcmdstate.Start();
        }
        private delegate void DelegateAddData();
      
        private void UpdadvTree_left()
        {
            if (ExecRTUList.Lrdm != null)
                foreach (DevComponents.AdvTree.Node item in advTree_left.Nodes)
                {
                    var rtu = from r in ExecRTUList.Lrdm where r.STCD == item.Tooltip select r;
                    if(rtu.Count()>0)
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
        private void ThreadAddCMDState()
        {
            while (true)
            {
                // 判断是否需要Invoke，多线程时需要
                if (this.InvokeRequired)
                {
                    try
                    {
                        // 通过委托调用写主线程控件的程序，传递参数放在object数组中
                        this.Invoke(new DelegateAddData(UpdadvTree_left));
                    }
                    catch { }
                }
                Thread.Sleep(1000);
            }
        }

        
        private void BTsend_Click(object sender, EventArgs e)
        {
            string[] stcds = GetStcds();
            if (panelEx_Fill.Controls.Count == 0)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择要下发的命令！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (stcds.Count() == 0)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择测站！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] Commands = cc.GetCommand(stcds, cbBNFOINDEX.SelectedItem.ToString(), out CommandCode);
            if (Commands == null)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("召测数据报生成失败！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //燕禹协议
                if (Program.wrx.ReadDllXML().ToLower() == "gsprotocol.dll")
                {
                    if (cbBNFOINDEX.SelectedIndex == 0 || cbBNFOINDEX.SelectedIndex == 1)
                    {
                        string[] Stcds = StcdOnLine(stcds);
                        if (Stcds.Length == 0)
                        {
                            DevComponents.DotNetBar.MessageBoxEx.Show("所选择的测站都不在线，只有在线测站可以召测！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (stcds.Length != Stcds.Length)
                        {
                            DevComponents.DotNetBar.MessageBoxEx.Show("所选择的测站有" + (stcds.Length - Stcds.Length) + "个不在线，仅对在线测站下发命令！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        SendCommand(Commands, Stcds, CommandCode);
                        timer1.Enabled = true;
                        BTsend.Enabled = false;
                    }
                    else 
                    {
                        SendCommand(Commands, stcds, CommandCode);
                        timer1.Enabled = true;
                        BTsend.Enabled = false;
                    }
                } 
                //水资源
                if (Program.wrx.ReadDllXML().ToLower() == "protocol.dll")
                {
                    SendCommand(Commands, stcds, CommandCode);
                    timer1.Enabled = true;
                    BTsend.Enabled = false;
                }

                //水文
                if (Program.wrx.ReadDllXML().ToLower() == "hydrologicprotocol.dll")
                {                    
                    SendCommand(Commands, stcds, CommandCode);
                    timer1.Enabled = true;
                    BTsend.Enabled = false;
                }
            }
        }

        private string[] StcdOnLine(string[] stcds) 
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            foreach (var item in stcds)
            {
                var stcd = from rtu in ExecRTUList.Lrdm where rtu.STCD == item && rtu.SERVICETYPE !=null select rtu;
                if (stcd.Count() > 0)
                { al.Add(stcd.First().STCD); }
            }
            string[] Stcds = new string[al.Count];
            if (al.Count > 0) 
            {
                for (int i = 0; i < al.Count; i++)
                {
                    Stcds[i]=al[i] as string;
                }
            }

            return Stcds;
        }

        int sleep = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sleep == 0)
            {
                BTsend.Text = "召测";
                sleep = 3;
                timer1.Enabled = false;
                BTsend.Enabled = true;
            }
            else
            {
                BTsend.Text = "召测(" + sleep + ")";
                sleep--;
            }
            
        }


        CommandResultForm crf = null;
        private void BTcmdrut_Click(object sender, EventArgs e)
        {
            if (crf == null)
            {
                crf = new CommandResultForm(((MainForm)this.ParentForm));
                ((MainForm)this.ParentForm).CRF = crf;
                crf.Show();
            }
            else 
            {
                crf.Close();
                crf.Dispose();
                crf = null;
                ((MainForm)this.ParentForm).CRF = crf;
            }
        }

        private void advTree_left_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            panelEx_Fill.Controls.Clear();
            if (e.Node.Checked)
                e.Node.Checked = false;
            else
                e.Node.Checked = true;
        }

        #region 弹出菜单(左侧)
        private void advTree_left_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否点击为右键
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip_Left.Show(sender as Control, e.Location);
                
            }
        }

        //重新读取测站列表
        private void toolStripMenuItem_Reload_Click(object sender, EventArgs e)
        {
            ExecRTUList.SetLrdm(Service.PublicBD.db.GetRTUList(""));
            adTree_Init();
        }
        #endregion
    }


    /// <summary>
    /// 单命令控件接口
    /// </summary>
    public interface ICommandControl
    {

        /// <summary>
        /// 得到命令集
        /// </summary>
        /// <param name="Stcds">站号列表</param>
        /// <param name="NFOINDEX">信道</param>
        /// <param name="CommandCode">命令码</param>
        /// <returns></returns>
        string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode);

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             