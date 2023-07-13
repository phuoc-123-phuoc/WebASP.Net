using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public static class OrderStatus
    {
        private static List<string> _orderStatus = new List<string>()
        { "NEW",
        "CANCELLED",
        "PROCESSING",
        "PACKAGED",
        "PICKED",
        "SHIPPING",
        "DELIVERED",
        "RETURNED",
        "PAID",
        "REFUNDED",
        "RETURN_REQUESTED"};

        public static List<string> orderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }
    }
}