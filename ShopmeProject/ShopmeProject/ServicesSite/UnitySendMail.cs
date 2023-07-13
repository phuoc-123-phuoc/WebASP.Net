using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class UnitySendMail
    {
        private MyDbContext _context;

        public UnitySendMail()
        {
            _context = new MyDbContext();
        }


        public void SendMail(Customer customer,string currentUrl)
        {
            // Create an instance of the MailMessage class
            MailMessage mail = new MailMessage();

            // Set the sender and recipient addresses
            mail.From = new MailAddress(_context.Settings.Find("MAIL_FROM").Value);
            mail.To.Add(customer.Email);

            // Set the subject and body of the email
            mail.Subject = _context.Settings.Find("CUSTOMER_VERIFY_SUBJECT").Value;
            mail.IsBodyHtml = true;
            mail.Body = _context.Settings.Find("CUSTOMER_VERIFY_CONTENT").Value
                .Replace("[[URL]]",currentUrl)
                .Replace("[[name]]", customer.FirstName +" "+ customer.LastName);

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