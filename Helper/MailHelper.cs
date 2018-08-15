using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CoursePlatform.Helper
{
    public static class MailHelper
    {
        public static async Task<bool> SendContactMail(string FromName,string FromEmail,string Title,string Message)
        {
            var hostName = WebConfigurationManager.AppSettings["host1"].ToString();
            var portNumber = Int32.Parse(WebConfigurationManager.AppSettings["port1"]);
            var userName = WebConfigurationManager.AppSettings["userMail1"].ToString();
            var password = WebConfigurationManager.AppSettings["password1"].ToString();

            var body = "<p>Email From: {0} ({1})</p>" +
                "<p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("ccairouni@gmail.com"));
            message.From = new MailAddress(userName);
            message.Subject = Title;
            message.Body = string.Format(body, FromName, FromEmail,Message);
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = userName,
                    Password = password
                };
                smtp.Credentials = credential;
                smtp.Host = hostName;
                smtp.Port = portNumber;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return true;
            }
        }

        public static async Task<bool> SendActivationMail(string mail,string scheme, string host, string port, string activationcode)
        {
            var hostName = WebConfigurationManager.AppSettings["host2"].ToString();
            var portNumber = Int32.Parse(WebConfigurationManager.AppSettings["port2"]);
            var userName = WebConfigurationManager.AppSettings["userMail2"].ToString();
            var password = WebConfigurationManager.AppSettings["password2"].ToString();

            var varifyUrl = scheme + "://" + host + ":" + port + "/Account/Activate?code=" + activationcode;
            var body = "<br/><br/>تم تسجيل حسابك بنجاح" +
      " لتفعيل حسابك اضغط على الرابط التالى" +
      " <br/><br/><a href='" + varifyUrl + "'>" + varifyUrl + "</a> ";
            var message = new MailMessage();
            message.To.Add(new MailAddress(mail));
            message.From = new MailAddress(userName);
            message.Subject = "تم تسجيل حسابك بنجاح";
            message.Body = body;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = userName,
                    Password = password 
                };
                smtp.Credentials = credential;
                smtp.Host = hostName;
                smtp.Port = portNumber;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return true;
            }
        }

        public static async Task<bool> SendResetPasswordMail(string mail, string scheme, string host, string port, string resetcode)
        {
            var hostName = WebConfigurationManager.AppSettings["host2"].ToString();
            var portNumber = Int32.Parse(WebConfigurationManager.AppSettings["port2"]);
            var userName = WebConfigurationManager.AppSettings["userMail2"].ToString();
            var password = WebConfigurationManager.AppSettings["password2"].ToString();

            var varifyUrl = scheme + "://" + host + ":" + port + "/Account/ResetPassword?code=" + resetcode;
            var body = "<br/><br/>تغيير كلمة المرور" +
      " لتغير كلمة المرور الخاصة بك اضغط على الرابط التالى" +
      " <br/><br/><a href='" + varifyUrl + "'>" + varifyUrl + "</a> ";
            var message = new MailMessage();
            message.To.Add(new MailAddress(mail));
            message.From = new MailAddress(userName);
            message.Subject = "اعادة تعيين كلمة المرور";
            message.Body = body;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = userName,
                    Password = password
                };
                smtp.Credentials = credential;
                smtp.Host = hostName;
                smtp.Port = portNumber;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return true;
            }
        }
    }
}