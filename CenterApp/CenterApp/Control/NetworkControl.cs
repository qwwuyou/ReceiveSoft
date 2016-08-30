using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace CenterApp
{
    public partial class NetworkControl : UserControl
    {
        public NetworkControl(StyleManager stylemanager)
        {
            InitializeComponent();
            SetStyle(stylemanager);
            Init();
        }

        public void SetStyle(StyleManager stylemanager)
        {
            panelEx1.Style.BackColor1.Color = stylemanager.MetroColorParameters.CanvasColor;
            panelEx1.Style.BorderColor.Color = stylemanager.MetroColorParameters.BaseColor;
        }

        private void Init() 
        {
            textBox_IP.Text =Program.wrx.XMLObj.LsM[0].IP_PORTNAME.Trim();
            textBox_Port.Text = Program.wrx.XMLObj.LsM[0].PORT_BAUDRATE.ToString().Trim();
        }

        private void NetworkControl_SizeChanged(object sender, EventArgs e)
        {
            panelEx1.Location = new Point((panelEx1.Parent.Width - panelEx1.Width) / 2, (panelEx1.Parent.Height - panelEx1.Height) / 2);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (ValidatIpPort())
            {
                Program.wrx.XMLObj.LsM[0].IP_PORTNAME = textBox_IP.Text.Trim();
                Program.wrx.XMLObj.LsM[0].PORT_BAUDRATE= int.Parse (textBox_Port.Text.Trim());
                Program.wrx.WriteXML();

                TcpControl.ReStart ();
            }
            else
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("IP或端口设置有误！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidatIpPort() 
        {
            System.Net.IPAddress ip;
            int port=0;
            bool b1 = System.Net.IPAddress.TryParse(textBox_IP.Text.Trim(), out ip);
            bool b2 = int.TryParse(textBox_Port.Text .Trim(),out port);
            if (b2 && port > 0 && port <= 65535)
                b2 = true;
            else
                b2 = false;

            if (b1 && b2)
            { return true; }
            else
                return false ;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TcpControl.Connected )
            { reflectionImage2.Image = global::CenterApp.Properties.Resources.yes; }
            else
            { reflectionImage2.Image = global::CenterApp.Properties.Resources.no; }
        }
    }
}
