using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace TravelCat
{
    class GmailSender
    {
        public string account, password, sender, receiver, messageBody, subject;
        public bool IsHtml;
        public MailMessage MailMessage;
        public GmailSender() { }
        public GmailSender(string account, string password, string sender, string receiver, string messageBody, string subject, bool IsHtml = false)
        {
            this.account = account;
            this.password = password;
            this.sender = sender;
            this.receiver = receiver;
            this.messageBody = messageBody;
            this.subject = subject;
            this.IsHtml = IsHtml;
        }
        public GmailSender(string account, string password, MailMessage MailMessage, bool IsHtml = false)
        {
            this.account = account;
            this.password = password;
            this.MailMessage = MailMessage;
            this.IsHtml = IsHtml;
        }
        //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port  
        SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

        public void Send()
        {
            //設定帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential(account, password);
            //Gmial 的 smtp 必需要使用 SSL
            MySmtp.EnableSsl = true;

            //設定信件相關內容
            MailMessage MailMessage = new MailMessage(sender, receiver, subject, messageBody);
            MailMessage.IsBodyHtml = IsHtml;

            //發送Email
            MySmtp.Send(MailMessage); MySmtp.Dispose();
        }
    }
}
