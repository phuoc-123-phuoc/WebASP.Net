using ShopmeProject.Models;
using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    [JwtAuthentication("Salesperson", "Admin")]
    public class CustomerController : BaseController
    {
        private CustomerService customerService;

        public CustomerController()
        {
            customerService = new CustomerService();
        }
        // GET: Customer
        public ActionResult Index()
        {
            return ListByPage(1, "");
            var listCustomer = customerService.listAllCustomer();
            return View(listCustomer);
        }

        public ActionResult ListByPage(int pageNum, string keyword)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<Customer> listCustomer = customerService.listByPage(pageNum, keyword);

            long startCount = (pageNum - 1) * customerService.customerPerPage() + 1;
            long endCount = startCount + customerService.customerPerPage() - 1;
            if (endCount > customerService.totalRecordCustomer(keyword))
            {
                endCount = customerService.totalRecordCustomer(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = customerService.totalPageCustomer(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = customerService.totalRecordCustomer(keyword);
            ViewBag.KeyWord = keyword;
            return View("Index", listCustomer);
        }

        [HttpGet]
        public ActionResult SetEnabled(int id, bool enabled)
        {

            customerService.updateCustomerEnabledStatus(id, enabled);
            string status = enabled ? "enabled" : "disabled";
            string message = $"The customer ID {id} has been {status}";

            TempData["message"] = message;

            return RedirectToAction("Index", "Customer");
        }

        public ActionResult Detail(int id)
        {
            Customer customer = customerService.Get(id);
            return PartialView("customer_detail_modal", customer);
        }

        [HttpPost]
        public ActionResult CheckDuplicateEmail(int id, string email)
        {
            if (customerService.isEmailUnique(id,email) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        public ActionResult editCustomer(int id)
        {

            var countries = customerService.listAllCountries();
            var customer = customerService.Get(id);
            ViewBag.listCountries = countries;
            return View("customer_form", customer);
        }

        public ActionResult SaveCustomer(Customer customer)
        {
            customerService.Save(customer);
            TempData["Message"] = "The customer has been updated successfully.";
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (customerService.CountCustomerById(id) > 0)
            {
                customerService.delete(id);
                TempData["message"] = "The customer ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any customer with ID " + id;
            }
            return RedirectToAction("Index");
        }
    }
}