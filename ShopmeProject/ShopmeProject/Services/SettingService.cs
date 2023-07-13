using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Services
{
    public class SettingService
    {
        private MyDbContext _context;

        public SettingService()
        {
            _context = new MyDbContext();
        }

        public List<Currency> GetCurrencies()
        {
            return _context.Currencys.ToList();
        }

        public List<Setting> GetSettings()
        {
           
            return _context.Settings.ToList();
        }

        public List<Setting> getCurrencySettings()
        {
            return _context.Settings.Where(x => x.Category == "CURRENCY").ToList();
        }

        public void SaveGeneral(
            string SITE_NAME,
            string COPYRIGHT,
            string CURRENCY_ID,
            string CURRENCY_SYMBOL_POSITION,
            string DECIMAL_POINT_TYPE,
            string DECIMAL_DIGITS,
            string THOUSANDS_POINT_TYPE,
            string SITE_LOGO
            )
        {
            Setting siteName = _context.Settings.Find("SITE_NAME");
            siteName.Value = SITE_NAME;
            Setting copyRight = _context.Settings.Find("COPYRIGHT");
            copyRight.Value = COPYRIGHT;
            Setting currencyId = _context.Settings.Find("CURRENCY_ID");
            currencyId.Value = CURRENCY_ID;

            string symbol = _context.Currencys.Find(CURRENCY_ID).Symbol;
            Setting currencySymbol = _context.Settings.Find("CURRENCY_SYMBOL");
            currencySymbol.Value = symbol;

            Setting currencySymbolPositon = _context.Settings.Find("CURRENCY_SYMBOL_POSITION");
            currencySymbolPositon.Value = CURRENCY_SYMBOL_POSITION;
            Setting decimalPoinType = _context.Settings.Find("DECIMAL_POINT_TYPE");
            decimalPoinType.Value = DECIMAL_POINT_TYPE;
            Setting decimalDigits = _context.Settings.Find("DECIMAL_DIGITS");
            decimalDigits.Value = DECIMAL_DIGITS;
            Setting thousandsPointType = _context.Settings.Find("THOUSANDS_POINT_TYPE");
            thousandsPointType.Value = THOUSANDS_POINT_TYPE;
            if(SITE_LOGO != "")
            {
                Setting siteLogo = _context.Settings.Find("SITE_LOGO");
                siteLogo.Value = SITE_LOGO;
            }
            _context.SaveChanges();
        }

        public string getCurrencyCode()
        {
            Setting setting = _context.Settings.Find("CURRENCY_ID");
            string name = setting.Value;
            return _context.Currencys.Find(name).Code;
          
        }

        public void SaveMailServer(
             string MAIL_HOST,
            string MAIL_PORT,
            string MAIL_USERNAME,
            string MAIL_PASSWORD,
            string SMTP_AUTH,
            string SMTP_SECURED,
            string MAIL_FROM,
            string MAIL_SENDER_NAME
            )
        {
            Setting mailHost = _context.Settings.Find("MAIL_HOST");
            Setting mailPort = _context.Settings.Find("MAIL_PORT");
            Setting mailUserName = _context.Settings.Find("MAIL_USERNAME");
            Setting mailPassWord = _context.Settings.Find("MAIL_PASSWORD");
            Setting smtpAuth = _context.Settings.Find("SMTP_AUTH");
            Setting smtpSecured = _context.Settings.Find("SMTP_SECURED");
            Setting mailFrom = _context.Settings.Find("MAIL_FROM");
            Setting mailSenderName = _context.Settings.Find("MAIL_SENDER_NAME");

            mailHost.Value = MAIL_HOST;
            mailPort.Value = MAIL_PORT;
            mailUserName.Value = MAIL_USERNAME;
            mailPassWord.Value = MAIL_PASSWORD;
            smtpAuth.Value = SMTP_AUTH;
            smtpSecured.Value = SMTP_SECURED;
            mailFrom.Value = MAIL_FROM;
            mailSenderName.Value = MAIL_SENDER_NAME;

            _context.SaveChanges();


        }
      
        public void SaveMailTemplates(
            string CUSTOMER_VERIFY_SUBJECT,
            string CUSTOMER_VERIFY_CONTENT,
            string ORDER_CONFIRMATION_SUBJECT,
            string ORDER_CONFIRMATION_CONTENT)
        {
            Setting customerSub = _context.Settings.Find("CUSTOMER_VERIFY_SUBJECT");
            Setting customerCont = _context.Settings.Find("CUSTOMER_VERIFY_CONTENT");
            Setting orderSub = _context.Settings.Find("ORDER_CONFIRMATION_SUBJECT");
            Setting orderCont = _context.Settings.Find("ORDER_CONFIRMATION_CONTENT");
            customerSub.Value = CUSTOMER_VERIFY_SUBJECT;
            customerCont.Value = CUSTOMER_VERIFY_CONTENT;
            orderSub.Value = ORDER_CONFIRMATION_SUBJECT;
            orderCont.Value = ORDER_CONFIRMATION_CONTENT;
            _context.SaveChanges();

        }

        internal void SavePayment(string PAYPAL_API_BASE_URL, string PAYPAL_API_CLIENT_ID, string PAYPAL_API_CLIENT_SECRET)
        {
            Setting payUrl = _context.Settings.Find("PAYPAL_API_BASE_URL");
            Setting payId = _context.Settings.Find("PAYPAL_API_CLIENT_ID");
            Setting paySecret = _context.Settings.Find("PAYPAL_API_CLIENT_SECRET");
            payUrl.Value = PAYPAL_API_BASE_URL;
            payId.Value = PAYPAL_API_CLIENT_ID;
            paySecret.Value = PAYPAL_API_CLIENT_SECRET;
            _context.SaveChanges();
        }
    }
}