using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    public class ShippingRateRestController : BaseController
    {
        private ShippingRateService shippingRateService;
        public ShippingRateRestController()
        {
            shippingRateService = new ShippingRateService();
        }
        [HttpPost]
        public ActionResult getShippingCost(int productId, int countryId, string state)
        {
            float shippingCost = shippingRateService.calculateShippingCost(productId, countryId, state);
            return Content(shippingCost.ToString());
        }
    }
}