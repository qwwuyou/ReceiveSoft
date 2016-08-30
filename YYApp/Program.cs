using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace YYApp
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.ThreadException +=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            
            //Application.Run(new Form1());

        }


        private static  double GetCD(double oldCD)
        {
            Random rd = new Random();
            if (oldCD < 6 || oldCD > 8)
            { return Math.Round(rd.Next(6, 8) + rd.NextDouble(), 2); }
            else { return oldCD; }
        }

        //操作xml的对象
        public static OperateXML.WriteReadXML wrx = new OperateXML.WriteReadXML();
        public static string xmlpath = System.Windows.Forms.Application.StartupPath + "/System.xml";

        private static bool loginstate = false;
        public static bool LoginState 
        {
            get { return loginstate; }
            set { loginstate = value; }
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            

            string To = "YYsoft2013@163.com";
            string From = To;
            string Body = "管理员：<br>   hello！<br>系统捕获异常：<br>" + e.Exception.StackTrace ;
            string Title = "Error";
            string Password = "hao1234567";


            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("是否将错误信息发送到管理员邮箱？\n[Yes]发送并重启软件   [No]不发送并重启软件   [Cancel]关闭\n异常信息： " + e.ToString(), "[异常]", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            switch (result)
            {
                case DialogResult.Yes:
                    {
                        try
                        {
                            using (SendMail sm = new SendMail(To, From, Body, Title, Password))
                            {
                                sm.Send();
                                sm.Dispose();
                                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                System.Environment.Exit(0);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("系统出现如下异常：" + ex.ToString());
                        }
                        break;
                    }
                case DialogResult.No:
                    {
                        System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        System.Environment.Exit(0);
                        break;
                    }
                case DialogResult.Cancel:
                    {
                        System.Environment.Exit(0);
                        break;
                    }
            } 

            
           
        }


    }
}
