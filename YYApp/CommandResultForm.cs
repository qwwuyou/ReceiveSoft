using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp
{
    public partial class CommandResultForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        MainForm MF = null;
        public CommandResultForm(MainForm mf)
        {
            InitializeComponent();
            MF = mf;
        }

        private void CommandResultForm_Load(object sender, EventArgs e)
        {
            this.Width = MF.Width;
            this.Location = new System.Drawing.Point(MF.Location.X, MF.Location.Y + MF.Height);
            MF.ShowDataEvent += new MainForm.ShowDataDelegate(mf_ShowDataEvent);
        }

        void mf_ShowDataEvent(object sender, EventArgs e)
        {
            string text = MF.Text;
            if (text != "" && text.IndexOf("]\n") != -1)
            {
                richTextBoxEx_bottom.AppendText(text);
                if (richTextBoxEx_bottom.Text.Length > 20000)
                {
                    richTextBoxEx_bottom.Text = "";
                }
                richTextBoxEx_bottom.ScrollToCaret();
            }
        }

        private void CommandResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MF.ShowDataEvent -= new MainForm.ShowDataDelegate(mf_ShowDataEvent);
        }
    }
}
