using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class QuantityControl
    {
        public QuantityControl()
        {

        }
        public QuantityControl(int productId, string quantityValue)
        {
            this.productId = productId;
            this.quantityValue = quantityValue;
        }
        public int productId { get; set; }
        public string quantityValue { get; set; }
    }
}