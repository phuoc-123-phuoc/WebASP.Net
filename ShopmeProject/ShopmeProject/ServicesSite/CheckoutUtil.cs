using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class CheckoutUtil
    {
		public static float calculateShippingCost(List<CartItem> cartItems, ShippingRate shippingRate, int division)
		{
			float shippingCostTotal = 0.0f;

			foreach (CartItem item in cartItems)
			{
				Product product = item.Product;
				float dimWeight = (product.Length * product.Width * product.Height) / division;
				float finalWeight = product.Weight > dimWeight ? product.Weight : dimWeight;
				float shippingCost = finalWeight * item.Quantity * shippingRate.rate;

				item.shippingCost = shippingCost;

				shippingCostTotal += shippingCost;
			}

			return shippingCostTotal;
		}

		public static float calculateProductTotal(List<CartItem> cartItems)
		{
			float total = 0.0f;

			foreach (CartItem item in cartItems)
			{
				total += item.getsubtotal;
			}

			return total;
		}

		public static float calculateProductCost(List<CartItem> cartItems)
		{
			float cost = 0.0f;

			foreach (CartItem item in cartItems)
			{
				cost += item.Quantity * item.Product.Cost;
			}

			return cost;
		}
	}
}