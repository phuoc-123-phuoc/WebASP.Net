using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class OrderUtil
    {
        private MyDbContext _context;

        public OrderUtil()
        {
            _context = new MyDbContext();
        }


        public  void SendMail(Order order, Customer customer, string currentUrl)
        {
            string currencySymbol = _context.Settings.FirstOrDefault(x => x.Key == "CURRENCY_SYMBOL").Value; 
            string currencySymbolPosition = _context.Settings.FirstOrDefault(x => x.Key == "CURRENCY_SYMBOL_POSITION").Value; 
            string thousandsPointType = _context.Settings.FirstOrDefault(x => x.Key == "THOUSANDS_POINT_TYPE").Value; 
            string decimalDigits = _context.Settings.FirstOrDefault(x => x.Key == "DECIMAL_DIGITS").Value; 
            string decimalPointType = _context.Settings.FirstOrDefault(x => x.Key == "DECIMAL_POINT_TYPE").Value; 
            string total = FormatPrice(currencySymbol, currencySymbolPosition,order.total, thousandsPointType, decimalDigits, decimalPointType);
            // Create an instance of the MailMessage class
            MailMessage mail = new MailMessage();

            // Set the sender and recipient addresses
            mail.From = new MailAddress(_context.Settings.Find("MAIL_FROM").Value);
            mail.To.Add(customer.Email);

            // Set the subject and body of the email
            mail.Subject = _context.Settings.Find("ORDER_CONFIRMATION_SUBJECT").Value.Replace("[[orderId]]", order.Id.ToString());
            mail.IsBodyHtml = true;
            mail.Body = _context.Settings.Find("ORDER_CONFIRMATION_CONTENT").Value
                .Replace("[[name]]", customer.FullName)
                .Replace("[[orderId]]", order.Id.ToString())
                .Replace("[[orderTime]]", order.orderTime.ToString())
                .Replace("[[shippingAddress]]", order.getShippingAddress)
                .Replace("[[total]]", total)
                .Replace("[[paymentMethod]]",order.paymentMethod)
                .Replace("[[orderLink]]",currentUrl);

            // Create an instance of the SmtpClient class and configure it
            SmtpClient smtpClient = new SmtpClient(_context.Settings.Find("MAIL_HOST").Value, Convert.ToInt32(_context.Settings.Find("MAIL_PORT").Value));
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_context.Settings.Find("MAIL_USERNAME").Value, _context.Settings.Find("MAIL_PASSWORD").Value);

            // Send the email
            smtpClient.Send(mail);
        }

        public string  FormatPrice( string currencySymbol, string currencySymbolPosition, double productPrice, string thousandsPointType, string decimalDigits, string decimalPointType)
        {
            if (thousandsPointType == "POINT")
            {
                thousandsPointType = ".";
            }
            else
            {
                thousandsPointType = ",";
            }
            if (decimalPointType == "POINT")
            {
                decimalPointType = ".";
            }
            else
            {
                decimalPointType = ",";
            }
            var formattedPrice = "";
            if (currencySymbolPosition == "Before price")
            {
                formattedPrice += currencySymbol;
            }
            formattedPrice += string.Format("{0:N" + decimalDigits + "}", productPrice);

            formattedPrice = formattedPrice.Replace(",", thousandsPointType).Replace(".", decimalPointType);
            if (currencySymbolPosition == "After price")
            {
                formattedPrice += currencySymbol;
            }
            return formattedPrice;
        }
    }
}