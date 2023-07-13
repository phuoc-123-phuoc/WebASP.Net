using ShopmeProject.DTO;
using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class OrderSiteController : BaseSiteController
    {
        private OrderService orderService;
        private Services.SettingService settingService;

        public OrderSiteController()
        {
            orderService = new OrderService();
            settingService = new Services.SettingService();
        }



        // GET: OrderSite
        public ActionResult Index()
        {
            return ListByPage(1, "");
            //IEnumerable<Product> products = productService.ListAll();
            //return View(products);
        }

        public ActionResult ListByPage(int pageNum, string Orderkeyword)
        {
            Customer customer = GetCustomer();
            List<Setting> Settings = settingService.getCurrencySettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }
            List<Order> listOrder = orderService.listForCustomerByPage(customer,pageNum, Orderkeyword);


            long startCount = (pageNum - 1) * orderService.orderPerPage() + 1;
            long endCount = startCount + orderService.orderPerPage() - 1;
            if (endCount > orderService.totalRecordOrder(customer, Orderkeyword))
            {
                endCount = orderService.totalRecordOrder(customer, Orderkeyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = orderService.totalPageOrder(customer, Orderkeyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = orderService.totalRecordOrder(customer, Orderkeyword);
            ViewBag.Orderkeyword = Orderkeyword;
            
            return View("orders_customer", listOrder);
        }


        public ActionResult Detail(int id)
        {
            List<Setting> Settings = settingService.getCurrencySettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

            Order order = orderService.Get(id);
            return PartialView("order_details_modal", order);
        }

        public Customer GetCustomer()
        {
            var user = User as ClaimsPrincipal;
            string email = "";
            var name = "";
            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
                name = user.FindFirst("unique_name")?.Value;
            }

            AuthenticationControllerHelperUtil authenticationControllerHelperUtil = new AuthenticationControllerHelperUtil();
            Customer customer = authenticationControllerHelperUtil.getAuthenticatedCustomer(email);
            return customer;
        }

        [HttpPost]
        public ActionResult HandleOrderReturnRequest(OrderReturnRequest returnRequest)
        {
            Customer customer = GetCustomer();
           

            if(customer == null)
            {
                return Json("You must login to make request", JsonRequestBehavior.AllowGet);
            }
            orderService.setOrderReturnRequested(returnRequest, customer);
            return Json(new OrderReturnResponse() { orderId = returnRequest.orderId}, JsonRequestBehavior.AllowGet);
        }
    }
}