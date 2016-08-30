using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Xml;
using System.IO;

namespace YYApp
{
    public partial class SetHLJForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        string Type, Source, DataBase, UserName, PassWord;
        public SetHLJForm()
        {
            InitializeComponent();
        }

        private void ReadXml(string Path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path);
                XmlNode root = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
                XmlElement nls;
                nls = (XmlElement)root.SelectSingleNode("Type");
                Source = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("Source");
                Source = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("DataBase");
                DataBase = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("UserName");
                UserName = nls.InnerText;
                nls = (XmlElement)root.SelectSingleNode("PassWord");
                PassWord = nls.InnerText;
            }
            catch { }
        }
        private void WriteXml(string Path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path);
                XmlNode node = xmlDoc.SelectSingleNode("system").SelectSingleNode("DataBaseConnect");
                XmlElement nls;
                nls = (XmlElement)node.SelectSingleNode("Type");
                nls.InnerText = Type;
                nls = (XmlElement)node.SelectSingleNode("Source");
                nls.InnerText = Source;
                nls = (XmlElement)node.SelectSingleNode("DataBase");
                nls.InnerText = DataBase;
                nls = (XmlElement)node.SelectSingleNode("UserName");
                nls.InnerText = UserName;
                nls = (XmlElement)node.SelectSingleNode("PassWord");
                nls.InnerText = PassWord;
                xmlDoc.Save(Path);
            }
            catch { }
        }
        private void SetHLJForm_Load(object sender, EventArgs e)
        {
            string path = System.Windows.Forms.Application.StartupPath + "/Resave_hlj.xml";
            if (!File.Exists(path))
            { 
                DevComponents.DotNetBar.MessageBoxEx.Show("没有找到转存信息的xml文件，请联系系统设计人员!");
            }
            else 
            {
                ReadXml(path);
                textBox_Source.Text =Source ;
                textBox_DataBase.Text=DataBase ;
                textBox_UserName.Text=UserName;
                textBox_PassWord.Text = PassWord;
            }
        }

        int ConState = 0; //0默认  1成功   2失败
        private void button_OK_Click(object sender, EventArgs e)
        {
            string Msg = DBValidate();
            if (Msg != "")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show(Msg, "[提示]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string path = System.Windows.Forms.Application.StartupPath + "/Resave_hlj.xml";
                if (!File.Exists(path))
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show("没有找到转存信息的xml文件，请联系系统设计人员!");
                }
                else
                {
                    Source = textBox_Source.Text.Trim();
                    DataBase = textBox_DataBase.Text.Trim();
                    UserName=textBox_UserName.Text.Trim();
                    PassWord = textBox_PassWord.Text.Trim();
                    
                    WriteXml(path);
                    Service._51Data dt = new Service._51Data(path);
                    try
                    {
                        dt.conn.Open();
                        dt.conn.Close();
                        DevComponents.DotNetBar.MessageBoxEx.Show("转存信息配置成功,请重新启动数据通讯服务!");
                        ConState = 1;
                    }
                    catch
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show("转存信息有误无法连接转存库,请重新配置!");
                        ConState = 2;
                    }
                    
                }
            }

        }

        private string DBValidate()
        {
            string Msg = "";
            if (textBox_Source.Text.Trim() == "")
            {
                Msg = "请填写服务器名称！" + "\n";
            }
            if (textBox_DataBase.Text.Trim() == "")
            {
                Msg += "请填写数据库名称！" + "\n";
            }
            if (textBox_UserName.Text.Trim() == "")
            {
                Msg += "请填写用户名！" + "\n";
            }

            return Msg;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (ConState == 2)
            {
                if (DevComponents.DotNetBar.MessageBoxEx.Show(" 转存数据库连接测试失败，确认关闭配置窗口？", "[提示]", MessageBoxButtons.YesNo) == DialogResult.Yes)
                { this.Close(); }
            }
            else { this.Close(); }
            
        }

    }
}