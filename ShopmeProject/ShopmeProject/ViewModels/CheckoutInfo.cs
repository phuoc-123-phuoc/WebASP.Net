using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShopmeProject.ViewModels
{
    public class CheckoutInfo
    {
        public float productCost { get; set; }
        public float productTotal { get; set; }
        public float shippingCostTotal { get; set; }
        public float paymentTotal { get; set; }
        public int deliverDays { get; set; }
        public DateTime deliverDate { get; set; }
        public bool codSupported { get; set; }

        public DateTime getDeliverDate
        {
            get
            {
                DateTime currentDate = DateTime.Now;
                return currentDate.AddDays(deliverDays);
            }
        }

        public string GetPaymentTotal4PayPal()
        {
            return paymentTotal.ToString("###,###.##");
        }

    }
}