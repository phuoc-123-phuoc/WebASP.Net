using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.DTO
{
    public class OrderDetailDTO
    {
        public int quantity { get; set; }
        public string Name { get; set; }
        public float productCost { get; set; }
        public float shippingCost { get; set; }
        public float subtotal { get; set; }

    }
}