using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class ShippingRateService
    {
        private MyDbContext _context;

        public ShippingRateService()
        {
            _context = new MyDbContext();
        }

        public ShippingRate getShippingRateForCustomer(Customer customer)
        {
            string state = customer.state;
            if (string.IsNullOrEmpty(state))
            {
                state = customer.city;
            }

            return _context.ShippingRate
                .FirstOrDefault(x => x.Country.Name == customer.Country.Name && x.state == state);
        }

        public ShippingRate getShippingRateForAddress(Address address)
        {
            string state = address.state;
            if (string.IsNullOrEmpty(state))
            {
                state = address.city;
            }
            return _context.ShippingRate
                .FirstOrDefault(x => x.Country.Name == address.Country.Name && x.state == state);
            
        }
    }
}