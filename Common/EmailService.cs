using System;
using System.Net;
using System.Net.Mail;

namespace JobsFinder_Main.Common
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromEmail = "hoaktyk0412@gmail.com";
                string password = "izpv dfga voko nolb";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while sending email: " + ex.Message);
            }
        }
    }
}
