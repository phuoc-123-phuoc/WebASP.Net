using ShopmeProject.Models;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class CheckoutService
    {

        public const int DIM_DIVISOR = 139;
       
        public CheckoutInfo prepareCheckout(List<CartItem> cartItems, ShippingRate shippingRate)
        {
            CheckoutInfo checkoutInfo = new CheckoutInfo();

            float productCost = CheckoutUtil.calculateProductCost(cartItems);
            float productTotal = CheckoutUtil.calculateProductTotal(cartItems);
            float shippingCostTotal = CheckoutUtil.calculateShippingCost(cartItems, shippingRate, DIM_DIVISOR);
            float paymentTotal = productTotal + shippingCostTotal;

            checkoutInfo.productCost =productCost;
            checkoutInfo.productTotal=productTotal;
            checkoutInfo.shippingCostTotal = shippingCostTotal;
            checkoutInfo.paymentTotal = paymentTotal;

            checkoutInfo.deliverDays = shippingRate.days;
            checkoutInfo.codSupported = shippingRate.codSupported;


            return checkoutInfo;
        }
    }
}