using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YYApp.CommandControl
{
    public partial class _36 : UserControl, ICommandControl
    {
        public _36()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            throw new NotImplementedException();
        }
    }
}
