using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TatBlog.Services.Blogs
{
    public class MailService : IMailService
    {
        public async Task SendEmailMessage(string email, CancellationToken cancellationToken = default)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("hungservicetdq@gmail.com", "hmrqzcweqetgfxsw"),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            MailMessage mailMessage = new MailMessage(from: "hungservicetdq@gmail.com", to: email)
            {

                Subject = "Đăng Kí Theo  Dõi",
                IsBodyHtml = true,
                Body = "cảm ơn vì đã đăng ký " +
                $"<a href=\"https://localhost:7269/Newsletter/Unsubscribe/?email={email}\">https://localhost:7269/Newsletter/Unsubscribe/{email}</a>.<br>"
            };
            smtpClient.Send(mailMessage);
        }
    }
}
