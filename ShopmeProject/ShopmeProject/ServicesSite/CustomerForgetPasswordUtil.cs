using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class CustomerForgetPasswordUtil
    {

        private MyDbContext _context;

        public CustomerForgetPasswordUtil()
        {
            _context = new MyDbContext();
        }

        public void SendMail(string email, string link)
        {
            // Create an instance of the MailMessage class
            MailMessage mail = new MailMessage();

            // Set the sender and recipient addresses
            mail.From = new MailAddress(_context.Settings.Find("MAIL_FROM").Value);
            mail.To.Add(email);

            // Set the subject and body of the email
            mail.Subject = "Here's the link to reset your password";
            mail.IsBodyHtml = true;
            mail.Body =  "<p>Hello,</p>"
                + "<p>You have requested to reset your password.</p>"
                + "Click the link below to change your password:</p>"
                + "<p><a href=\"" + link + "\">Change my password</a></p>"
                + "<br>"
                + "<p>Ignore this email if you do remember your password, "
                + "or you have not made the request.</p>";

            // Create an instance of the SmtpClient class and configure it
            SmtpClient smtpClient = new SmtpClient(_context.Settings.Find("MAIL_HOST").Value, Convert.ToInt32(_context.Settings.Find("MAIL_PORT").Value));
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_context.Settings.Find("MAIL_USERNAME").Value, _context.Settings.Find("MAIL_PASSWORD").Value);

            // Send the email
            smtpClient.Send(mail);
        }
    }
}