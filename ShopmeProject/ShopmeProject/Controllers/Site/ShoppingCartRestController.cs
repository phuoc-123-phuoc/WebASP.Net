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
    public class ShoppingCartRestController : BaseSiteController
    {
        private ShoppingCartService shoppingCartService;
        public ShoppingCartRestController()
        {
            shoppingCartService = new ShoppingCartService();
        }
       [HttpPost]
        public ActionResult addProductToCart(int productId, int quantity)
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

            if (customer != null)
            {
                 string updatedQuantity = shoppingCartService.addProduct(productId,quantity, customer);
                if(updatedQuantity.Length > 3)
                {
                    return Content(updatedQuantity);
                }
                return Content(updatedQuantity + " item(s) of this product were added to your shopping cart.");
            }
            else
            {
                return Content("You must login to add this product to cart.");
            }
                
        }
        [HttpPost]
        public ActionResult updateQuantity(int productId, int quantity)
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
            if (customer != null)
            {
                float subtotal = shoppingCartService.updateQuantity(productId, quantity, customer);
                return Content(subtotal.ToString());
            }
            else
            {
                return Content("You must login to change quantity of product.");
            }
        }

        [HttpPost]
        public ActionResult removeProduct(int productId)
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
            if (customer != null)
            {
                 shoppingCartService.removeProduct(productId, customer); ;
                return Content("The product has been removed from your shopping cart.");
            }
            else
            {
                return Content("You must login to remove product.");
            }
        }
    }
}