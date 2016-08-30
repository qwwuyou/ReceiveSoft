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
    public partial class SetWebControl : UserControl
    {
        public SetWebControl(StyleManager stylemanager)
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
            //textBox_Url.Text = WriteReadXML.Url.Trim();
            //if (WriteReadXML.Browser == "iexplore.exe") 
            //{ radioButton_ie.Checked = true; }
            //else if (WriteReadXML.Browser == "firefox.exe")
            //{ radioButton_Firefox.Checked = true; }
            //else if (WriteReadXML.Browser == "chrome.exe")
            //{ radioButton_Chrome.Checked = true; }
        }

        private void SetWebControl_SizeChanged(object sender, EventArgs e)
        {
            panelEx1.Location = new Point((panelEx1.Parent.Width - panelEx1.Width) / 2, (panelEx1.Parent.Height - panelEx1.Height) / 2);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            //WriteReadXML.Url = textBox_Url.Text.Trim();

            //if (radioButton_ie.Checked )
            //{ WriteReadXML.Browser = "iexplore.exe"; }
            //else if (radioButton_Firefox.Checked)
            //{ WriteReadXML.Browser = "firefox.exe"; }
            //else if (radioButton_Chrome.Checked)
            //{ WriteReadXML.Browser = "chrome.exe"; }

            //WriteReadXML.WriteXML();
            DevComponents.DotNetBar.MessageBoxEx.Show("Web网址保存成功！", "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

    }
}
