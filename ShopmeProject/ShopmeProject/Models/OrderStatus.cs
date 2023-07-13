using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public enum OrderStatus
    {
        NEW,
        CANCELLED,
        PROCESSING,
        PACKAGED,
        PICKED,
        SHIPPING,
        DELIVERED,
        RETURNED,
        PAID,
        REFUNDED,
        RETURN_REQUESTED
    }

    public static class OrderStatusExtensions
    {
        public static string DefaultDescription(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.NEW:
                    return "Order was placed by the customer";
                case OrderStatus.CANCELLED:
                    return "Order was rejected";
                case OrderStatus.PROCESSING:
                    return "Order is being processed";
                case OrderStatus.PACKAGED:
                    return "Products were packaged";
                case OrderStatus.PICKED:
                    return "Shipper picked the package";
                case OrderStatus.SHIPPING:
                    return "Shipper is delivering the package";
                case OrderStatus.DELIVERED:
                    return "Customer received products";
                case OrderStatus.RETURNED:
                    return "Products were returned";
                case OrderStatus.PAID:
                    return "Customer has paid this order";
                case OrderStatus.REFUNDED:
                    return "Customer has been refunded";
                case OrderStatus.RETURN_REQUESTED:
                    return "Customer sent request to return purchase";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, "Invalid order status");
            }
        }
    }

}