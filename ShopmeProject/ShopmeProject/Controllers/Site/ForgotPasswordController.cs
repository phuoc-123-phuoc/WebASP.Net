using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class ForgotPasswordController : BaseSiteController
    {
        private CustomerService customerService;
        public ForgotPasswordController()
        {
            customerService = new CustomerService();
        }
        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View("forgot_password");
        }

        [HttpPost]
        public ActionResult Index(string email)
        {
            string token = customerService.updateResetPasswordToken(email);
            if(token == "not found")
            {
                ViewBag.error = "Could not find any customer with the email " + email;
                return View("forgot_password");
            }
            var currentUrl = Request.Url.ToString();
            string link = currentUrl +  "/reset_password?token=" + token;
            CustomerForgetPasswordUtil sender = new CustomerForgetPasswordUtil();
            sender.SendMail(email, link);
            ViewBag.message = "We have sent a reset password link to your email."
                    + " Please check.";
            return View("forgot_password");
        }

        public ActionResult reset_password(string token)
        {
            Customer customer = customerService.getByResetPasswordToken(token);

            if (customer != null)
            {
                ViewBag.token = token;
            }
            else
            {
                ViewBag.pageTitle = "Invalid Token";
                ViewBag.message = "Invalid Token";
            
                return View("message");
            }


            return View("reset_password_form");
        }
        [HttpPost]
        public ActionResult reset_password(string password, string token)
        {
            string isFail = customerService.updatePassword(token, password);
            if(isFail == null)
            {
                ViewBag.pageTitle = "Reset Your Password";
                ViewBag.title = "Reset Your Password";
                ViewBag.message = "You have successfully changed your password.";
            }
            else
            {
                ViewBag.pageTitle = "Invalid Token";
                ViewBag.title = "Invalid Token";
                ViewBag.message = isFail;
            }
            return View("message");
        }
    }
}