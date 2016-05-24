using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using OpenPop.Pop3;
//using OpenPop.Mime;

namespace _51Mail
{
    /**/
    /// <summary> 
    /// 发送邮件的类 
    /// </summary> 
    public class SendMail : IDisposable
    {
        private MailMessage mailMessage;
        private SmtpClient smtpClient;
        private string password;//发件人密码 
        /**/
        /// <summary> 
        /// 处审核后类的实例 
        /// </summary> 
        /// <param name="To">收件人地址</param> 
        /// <param name="From">发件人地址</param> 
        /// <param name="Body">邮件正文</param> 
        /// <param name="Title">邮件的主题</param> 
        /// <param name="Password">发件人密码</param> 
        public SendMail(string To, string From, string Body, string Title, string Password)
        {
            mailMessage = new MailMessage();
            mailMessage.To.Add(To);
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
        }
        /**/
        /// <summary> 
        /// 添加附件 
        /// </summary> 
        public void Attachments(string Path)
        {
            string[] path = Path.Split(',');
            Attachment data;
            ContentDisposition disposition;
            for (int i = 0; i < path.Length; i++)
            {
                data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化附件 
                disposition = data.ContentDisposition;
                //disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取附件的创建日期 
                //disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);//获取附件的修改日期 
                //disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附件的读取日期 
                mailMessage.Attachments.Add(data);//添加到附件中 
            }
        }

        /**/
        /// <summary> 
        /// 异步发送邮件 
        /// </summary> 
        /// <param name="CompletedMethod"></param> 
        public void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据 
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册异步发送邮件完成时的事件 
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
            }
        }
        /**/
        /// <summary> 
        /// 发送邮件 
        /// </summary> 
        public void Send()
        {
            if (mailMessage != null)
            {

                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据 
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.Send(mailMessage);

            }
        }

        public void Dispose()
        {
            if (smtpClient != null)
                smtpClient.Dispose();
            if (mailMessage != null)
                mailMessage.Dispose();
        }
    }


    public class ReadMail
    {
        /// <summary>
        /// 读取指定邮箱中的邮件
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        public void ReadMailInfo(out string IP, out string Port)
        {
            IP = "";
            Port = "";
            try 
            {
                using (Pop3Client client = new Pop3Client())
                {
                    if (client.Connected)
                    {
                        client.Disconnect();
                    }
                    // Connect to the server, false means don't use ssl
                    client.Connect("pop.163.com", 110, false);

                    // Authenticate ourselves towards the server by email account and password
                    client.Authenticate("yysoft2013@163.com", "hao1234567");

                    //email count
                    int messageCount = client.GetMessageCount();

                    for (int i = messageCount; i <= messageCount; i--)
                    {
                        if (ValidateHeader(client.GetMessageHeaders(i).Subject, out IP, out Port))
                        { return; }
                        //Console.WriteLine(client.GetMessageHeaders(i).Subject);
                    }

                    #region
                    //i = 1 is the first email; 1 is the oldest email
                    //for (int i = 1; i <= messageCount; i++)
                    //{
                    //    Message message = client.GetMessage(i);

                    //    string sender = message.Headers.From.DisplayName;
                    //    string from = message.Headers.From.Address;
                    //    string subject = message.Headers.Subject;
                    //    DateTime Datesent = message.Headers.DateSent;

                    //    MessagePart messagePart = message.MessagePart;

                    //    //email body, 
                    //    string body = " ";
                    //    if (messagePart.IsText)
                    //    {
                    //        body = messagePart.GetBodyAsText();
                    //    }
                    //    else if (messagePart.IsMultiPart)
                    //    {
                    //        MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                    //        if (plainTextPart != null)
                    //        {
                    //            // The message had a text/plain version - show that one
                    //            body = plainTextPart.GetBodyAsText();
                    //        }
                    //        else
                    //        {
                    //            // Try to find a body to show in some of the other text versions
                    //            List<MessagePart> textVersions = message.FindAllTextVersions();
                    //            if (textVersions.Count >= 1)
                    //                body = textVersions[0].GetBodyAsText();
                    //            else
                    //                body = "<<OpenPop>> Cannot find a text version body in this message.";
                    //        }
                    //    }

                    //}
                    #endregion

                }
            }
            catch
            {
                IP = "";
                Port = "";
            }
           
        }

        /// <summary>
        /// 验证字符串是否符合IP 端口格式
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public bool ValidateHeader(string Header, out  string IP, out string Port)  //demo [192.168.1.1:80]
        {
            bool b1 = Header.Contains("[");
            bool b2 = Header.Contains("]");
            bool b3 = Header.Contains(":");
            bool b4 = true;
            IP = "";
            Port = "";
            if (b1 && b2 && b3)
            {

                Header = Header.Replace("[", "").Replace("]", "");
                string[] temp = Header.Split(new char[] { ':' });
                if (temp.Length == 2)
                {
                    IPAddress ip;
                    if (IPAddress.TryParse(temp[0], out ip))
                    {
                        IP = ip.ToString();
                    }
                    int port;
                    if (int.TryParse(temp[1], out port))
                    {
                        Port = port.ToString();
                    }
                }
            }

            if (IP == "" || Port == "")
            {
                b4 = false;
            }

            return b4;
        }
    }
}
