using Microsoft.IdentityModel.Tokens;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class LoginCustomerController : BaseController
    {
        private CustomerService customerService;

        public LoginCustomerController()
        {
            customerService = new CustomerService();
        }
        // GET: LoginCustomer
        public ActionResult Index()
        {
            // Remove JWT cookie
            HttpCookie cookie = new HttpCookie("jwtcus");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            Debug.WriteLine(email + password);
            // Validate user credentials (for example, by checking them against a database)
            bool isValid = CheckCredentials(email, password);

            if (isValid)
            {
                var customer = customerService.getCustomerByEmail(email);
              
               
               
                // Create JWT token with email payload
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("dsbsdvchsdvchgdvchsdcvhvsgvc");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Name, customer.FirstName + " "+ customer.LastName)
                         //new Claim(ClaimTypes.Role, "Admin"),
                         // new Claim(ClaimTypes.Role, "Manager")
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Store JWT token in cookie
                HttpCookie cookie = new HttpCookie("jwtcus", tokenString);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "HomeSite");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
        }

        private bool CheckCredentials(string email, string password)
        {

            return customerService.validInfor(email, password);

        }
        public ActionResult Logout()
        {
            // Remove JWT cookie
            HttpCookie cookie = new HttpCookie("jwtcus");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index","HomeSite");
        }

    }
}