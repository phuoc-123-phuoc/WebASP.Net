using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class CheckoutController : BaseSiteController
    {
        private CheckoutService checkoutService;
        private AddressService addressService;
        private ShippingRateService shipService;
        private ShoppingCartService cartService;
        private Services.SettingService settingService;
        private OrderService orderService;


        public CheckoutController()
        {
            checkoutService = new CheckoutService();
            addressService = new AddressService();
            shipService = new ShippingRateService();
            cartService = new ShoppingCartService();
            settingService = new Services.SettingService();
            orderService = new OrderService();
        }
        // GET: Checkout
        public ActionResult showCheckoutPage()
        {
            List<Setting> Settings = settingService.GetSettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

            Customer customer = GetCustomer();

            Address defaultAddress = addressService.getDefaultAddress(customer);

            ShippingRate shippingRate = null;
          
            if (defaultAddress != null)
            {
                ViewBag.shippingAddress = defaultAddress.getAddress;
                shippingRate = shipService.getShippingRateForAddress(defaultAddress);
                customer.FirstName = defaultAddress.FirstName;
                customer.LastName = defaultAddress.LastName;
                customer.addressLine1 = defaultAddress.addressLine1;
                customer.addressLine2 = defaultAddress.addressLine2;
                customer.state = defaultAddress.state;
                customer.city = defaultAddress.city;
                customer.postalCode = defaultAddress.postalCode;
                customer.Country.Code = defaultAddress.Country.Code;
                customer.phoneNumber = defaultAddress.phoneNumber;
            }
            else
            {
                ViewBag.shippingAddress = customer.Address;
                shippingRate = shipService.getShippingRateForCustomer(customer);
            }

            if (shippingRate == null)
            {
                return RedirectToAction("viewCart", "ShoppingCart");
            }

            List<CartItem> cartItems = cartService.listCartItems(customer);
            CheckoutInfo checkoutInfo = checkoutService.prepareCheckout(cartItems, shippingRate);

            string currencyCode = settingService.getCurrencyCode();
            ViewBag.currencyCode = currencyCode;
            ViewBag.cartItems = cartItems;
            ViewBag.customer = customer;
            return View("checkout", checkoutInfo);
        }
        [HttpPost]
        public ActionResult placeOrder(string paymentMethod)
        {
            Customer customer = GetCustomer();
            Address defaultAddress = addressService.getDefaultAddress(customer);
            ShippingRate shippingRate = null;

            if (defaultAddress != null)
            {
                shippingRate = shipService.getShippingRateForAddress(defaultAddress);
            }
            else
            {
                shippingRate = shipService.getShippingRateForCustomer(customer);
            }

            List<CartItem> cartItems = cartService.listCartItems(customer);
            CheckoutInfo checkoutInfo = checkoutService.prepareCheckout(cartItems, shippingRate);

            Order createdOrder = orderService.createOrder(customer, defaultAddress, cartItems, paymentMethod, checkoutInfo);

            cartService.deleteByCustomer(customer);
            var currentUrl = Request.Url.ToString();
            currentUrl = currentUrl.Replace("Checkout/processPayPalOrder", "OrderSite") ;
            OrderUtil sender = new OrderUtil();
            sender.SendMail(createdOrder,customer,currentUrl);
            return View("order_completed");
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
        public async Task<ActionResult> processPayPalOrder(string orderId, string paymentMethod)
        {

            string pageTitle = "Checkout Failure";
            string message = null;

            PayPalService payPalService = new PayPalService();
            bool isSuccess = await payPalService.validateOrder(orderId);

            if (isSuccess)
            {
                return placeOrder(paymentMethod);
            }
            else
            {
                pageTitle = "Checkout Failure";
                message = "ERROR: Transaction could not be completed because order information is invalid";

            }
            ViewBag.pageTitle = pageTitle;
            ViewBag.message = message;
            return View("message");
        }
    }
}