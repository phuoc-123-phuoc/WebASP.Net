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
    public class ShippingRateController : BaseController
    {
        private ShippingRateService shippingRateService;
        public ShippingRateController()
        {
            shippingRateService = new ShippingRateService();
        }
        public ActionResult Index()
        {
            return ListByPage(1, "");
        }
        public ActionResult ListByPage(int pageNum, string keyword)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<ShippingRate> shippingRates = shippingRateService.listByPage(pageNum, keyword);

            long startCount = (pageNum - 1) * shippingRateService.shippingRatePerPage() + 1;
            long endCount = startCount + shippingRateService.shippingRatePerPage() - 1;
            if (endCount > shippingRateService.totalRecordShippingRate(keyword))
            {
                endCount = shippingRateService.totalRecordShippingRate(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = shippingRateService.totalPageShippingRate(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = shippingRateService.totalRecordShippingRate(keyword);
            ViewBag.KeyWord = keyword;
            return View("shipping_rates", shippingRates);
        }

        public ActionResult newRate()
        {
            List<Country> listCountries = shippingRateService.listAllCountries();
            ShippingRate rate = new ShippingRate();
            ViewBag.listCountries = listCountries;
            ViewBag.Title = "New Rate";
            return View("shipping_rate_form", rate);
        }
        [HttpPost]
        public ActionResult saveRate(ShippingRate rate, bool codSupported)
        {
            rate.codSupported = codSupported;
            string messa = shippingRateService.save(rate);

            if(messa == null)
            {
                ViewBag.message = "The shipping rate has been saved successfully.";
            }
            else
            {
                ViewBag.message = messa;
            }
            return RedirectToAction("Index");
        }

        public ActionResult editRate(int id)
        {
            ShippingRate rate = shippingRateService.get(id);
            if (rate != null)
            {
                List<Country> listCountries = shippingRateService.listAllCountries();
                ViewBag.listCountries = listCountries;
               
                ViewBag.Title = "Edit Rate (ID: " + id + ")";
                return View("shipping_rate_form", rate);
            }
            else
            {
                ViewBag.message = "Could not find shipping rate with ID " + id;
                return RedirectToAction("Index");
            }
          
        }

        public ActionResult updateCODSupport(int id, bool supported)
        {
            string messa = shippingRateService.updateCODSupport(id, supported);
            if(messa == null)
            {
                ViewBag.message = "COD support for shipping rate ID " + id + " has been updated.";
            }
            else
            {
                ViewBag.message = messa;
            }

            return RedirectToAction("Index");
        }

        public ActionResult deleteRate(int id)
        {
            string messa = shippingRateService.delete(id);
            if(messa == null)
            {
                ViewBag.message = "The shipping rate ID " + id + " has been deleted.";
            }
            else
            {
                ViewBag.message = messa;
            }
            return RedirectToAction("Index");
        }
    }
}