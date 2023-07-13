using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    
    public class CustomerSiteController : BaseSiteController
    {
        private CustomerService customerService;
        public CustomerSiteController()
        {
            customerService = new CustomerService();
        }
        // GET: RegisterSite
        public ActionResult Register()
        {
          
            var countries = customerService.listAllCountries();
            var customer = new Customer();
            ViewBag.listCountries = countries;
            return View("register_form",customer);
        }
        public ActionResult createCustomer(Customer customer)
        {
            var currentUrl = Request.Url.ToString();

            customerService.save(customer);
           
            currentUrl = currentUrl.Replace("createCustomer", "verify?code="+customer.verificationCode);

            UnitySendMail sender = new UnitySendMail();
            sender.SendMail(customer, currentUrl);
           // Debug.WriteLine(currentUrl);
            var countries = customerService.listAllCountries();
          
            ViewBag.listCountries = countries;
            return Redirect("register_success");
        }

        [HttpPost]
        public ActionResult CheckDuplicateEmail(string email)
        {
            if (customerService.isNameUnique(email) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        public ActionResult register_success()
        {
            return View("register_success");
        }
        
        public ActionResult verify(string code)
        {
            if (customerService.verify(code))
            {
                return Redirect("verify_success");
            }
            return Redirect("verify_fail");
        }

        public ActionResult verify_success()
        {
            return View();
        }

        public ActionResult verify_fail()
        {
            return View();
        }

    }
}