using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenterApp
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           
            wrx.ReadXML();
            
            Application.Run(new MainForm());
           
        }

        public static OperateXML.WriteReadXML wrx = new OperateXML.WriteReadXML();
    }
}
