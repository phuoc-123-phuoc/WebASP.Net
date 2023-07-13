using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class AuthenticationControllerHelperUtil
    {
        private MyDbContext _context;

        public AuthenticationControllerHelperUtil()
        {
            _context = new MyDbContext();
        }
        public Customer getAuthenticatedCustomer(string email)
        {
           return  _context.Customer.FirstOrDefault(x => x.Email == email);
        }
    }
}