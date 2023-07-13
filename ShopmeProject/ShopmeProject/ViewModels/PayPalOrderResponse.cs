using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class PayPalOrderResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }

        public bool Validate(string orderId)
        {
            return Id.Equals(orderId) && Status.Equals("COMPLETED");
        }
    }
}