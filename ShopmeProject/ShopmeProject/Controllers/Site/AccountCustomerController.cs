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
    public class AccountCustomerController : BaseSiteController
    {
        private CustomerService customerService;

        public AccountCustomerController()
        {
            customerService = new CustomerService();
        }
        
        // GET: AccountCustomer
        public ActionResult editCustomer(string email, string redirect ="")
        {
            Debug.WriteLine(redirect);
            var countries = customerService.listAllCountries();
            var customer = customerService.Get(email);
            if(redirect != "")
            {
                ViewBag.redirect = redirect;
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.listCountries = countries;
            return View("customer_form", customer);
        }

        public ActionResult SaveCustomer(Customer customer, string redirect = "")
        {
            string email = customer.Email;
            customerService.Save(customer);

            if (redirect == "cart")
            {
                return RedirectToAction("viewCart", "ShoppingCart");
            }

            TempData["message"] = "The customer has been updated successfully.";
           
            var countries = customerService.listAllCountries();
            var customerSaved = customerService.Get(email);
            ViewBag.listCountries = countries;
            return RedirectToAction("editCustomer", customerSaved);
        }

    }
}