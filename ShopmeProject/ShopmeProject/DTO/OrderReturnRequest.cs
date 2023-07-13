using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.DTO
{
    public class OrderReturnRequest
    {
        public int orderId { get; set; }
        public string reason { get; set; }
        public string note { get; set; }
    }
}