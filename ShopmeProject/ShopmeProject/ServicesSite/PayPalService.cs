using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using ShopmeProject.Models;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class PayPalService
    {
        private MyDbContext _context;

        public PayPalService()
        {
            _context = new MyDbContext();
        }

        
        public async Task<bool>  validateOrder(String orderId)
        {
            PayPalOrderResponse orderResponse = await GetClientNameFromPayPalOrder(orderId);
            return orderResponse.Validate(orderId);

        }

        public async Task<PayPalOrderResponse> GetClientNameFromPayPalOrder(string orderId)
        {
            string CLIENT_ID = _context.Settings.Find("PAYPAL_API_CLIENT_ID").Value;
            string CLIENT_SECRET = _context.Settings.Find("PAYPAL_API_CLIENT_SECRET").Value;
            PayPalOrderResponse payPalOrderResponse = new PayPalOrderResponse();
            var client = new PayPalHttpClient(new SandboxEnvironment(CLIENT_ID, CLIENT_SECRET));
            var request = new OrdersGetRequest(orderId);
            var response = await client.Execute(request);
            var order = response.Result<PayPalCheckoutSdk.Orders.Order>();

           
            payPalOrderResponse.Id = order.Id;
            payPalOrderResponse.Status = order.Status;
          

            return payPalOrderResponse;
        }

    }
}