using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Service;

namespace CenterApp
{
    public partial class SetDBControl : UserControl
    {
        public SetDBControl(StyleManager stylemanager)
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
            textBox_Source.Text = Program.wrx.XMLObj.DBserver;
            textBox_DataBase.Text = Program.wrx.XMLObj.DBcatalog;
            textBox_UserName.Text = Program.wrx.XMLObj.DBusername;
            textBox_PassWord.Text = Program.wrx.XMLObj.DBpassword;
            ConState();
        }

        private void ConState()
        {
            //PublicBD.ReInit();

            if (PublicBD.ConnectState )
            {
                
                reflectionImage2.Image = global::CenterApp.Properties.Resources.yes;
            }
            else
            {
                
                reflectionImage2.Image = global::CenterApp.Properties.Resources.no;
            }
        }

        private void SetDBControl_SizeChanged(object sender, EventArgs e)
        {
            panelEx1.Location = new Point((panelEx1.Parent.Width - panelEx1.Width) / 2, (panelEx1.Parent.Height - panelEx1.Height) / 2);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            //ECO_User
            Program.wrx.XMLObj.DBserver= textBox_Source.Text.Trim();
            Program.wrx.XMLObj.DBcatalog = textBox_DataBase.Text.Trim();
            Program.wrx.XMLObj.DBusername = textBox_UserName.Text.Trim();
            Program.wrx.XMLObj.DBpassword = textBox_PassWord.Text.Trim();
            Program.wrx.WriteXML();

            ConState();
        }
    }
}
