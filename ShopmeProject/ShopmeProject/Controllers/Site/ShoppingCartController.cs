using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    [JwtAuthenticationSite]
    public class ShoppingCartController : BaseSiteController
    {
        private ShoppingCartService shoppingCartService;
        private Services.SettingService settingService;
        private AddressService addressService;
        private ShippingRateService shippingRateService;
        public ShoppingCartController()
        {
            shoppingCartService = new ShoppingCartService();
            settingService = new Services.SettingService();
            addressService = new AddressService();
            shippingRateService = new ShippingRateService();
        }
        // GET: ShoppingCart
        public ActionResult viewCart()
        {
            List<Setting> Settings = settingService.GetSettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

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
            List<CartItem> cartItems = shoppingCartService.listCartItems(customer);
            float estimatedTotal =0;
            foreach(var item in cartItems)
            {
                estimatedTotal += item.getsubtotal;
            }

            Address defaultAddress = addressService.getDefaultAddress(customer);

            ShippingRate shippingRate = null;
            bool usePrimaryAddressAsDefault = false;

            if (defaultAddress != null)
            {
                shippingRate = shippingRateService.getShippingRateForAddress(defaultAddress);
            }
            else
            {
                usePrimaryAddressAsDefault = true;
                shippingRate = shippingRateService.getShippingRateForCustomer(customer);
            }
            ViewBag.customer = customer;
            ViewBag.usePrimaryAddressAsDefault = usePrimaryAddressAsDefault;
            ViewBag.shippingSupported = shippingRate != null;
            ViewBag.estimatedTotal = estimatedTotal;
            return View("shopping_cart",cartItems);
        }
    }
}