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
    public partial class SetControl : UserControl
    {
        StyleManager Stylemanager = null;
        public SetControl(StyleManager stylemanager)
        {
            InitializeComponent();
            Stylemanager = stylemanager;
            SetStyle(Stylemanager);
        }

        public void SetStyle(StyleManager stylemanager)
        {
            bubbleBar1.ButtonBackAreaStyle.BackColor = stylemanager.MetroColorParameters.CanvasColor;
            bubbleBar1.BackColor = stylemanager.MetroColorParameters.CanvasColor;
            bubbleBar1.ButtonBackAreaStyle.BorderColor =  stylemanager.MetroColorParameters.BaseColor;
        }


        private void SetControl_Load(object sender, EventArgs e)
        {
            Init();
        }

        public NetworkControl _NetworkControl = null;
        public SetDBControl _SetDBControl = null;
        public SetWebControl _SetWebControl = null;
        private void Init() 
        {
            _NetworkControl = new NetworkControl(Stylemanager);
            _NetworkControl.Dock = System.Windows.Forms.DockStyle.Fill;
            pageSliderPage1.Controls.Add(_NetworkControl);
            _NetworkControl.Show();


            _SetDBControl = new SetDBControl(Stylemanager);
            _SetDBControl.Dock = System.Windows.Forms.DockStyle.Fill;
            pageSliderPage2.Controls.Add(_SetDBControl);
            _SetDBControl.Show();

            _SetWebControl = new SetWebControl(Stylemanager);
            _SetWebControl.Dock = System.Windows.Forms.DockStyle.Fill;
            pageSliderPage3.Controls.Add(_SetWebControl);
            _SetWebControl.Show();

            pageSlider1.SelectedPageIndex = 0;
        }

        private void SetControl_SizeChanged(object sender, EventArgs e)
        {
            pageSlider1.Location = new Point((pageSlider1.Parent.Width - pageSlider1.Width + 100) / 2, (pageSlider1.Parent.Height - pageSlider1.Height) / 2);
            bubbleBar1.Location = new Point(pageSlider1.Location.X+38, pageSlider1.Parent.Height - 50);
        }

        private void bubbleButton1_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            pageSlider1.SelectedPageIndex = 0;
        }

        
        private void bubbleButton2_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            pageSlider1.SelectedPageIndex = 1;
        }

        
        private void bubbleButton3_Click(object sender, ClickEventArgs e)
        {
            pageSlider1.SelectedPageIndex = 2;
        }
    }
}
