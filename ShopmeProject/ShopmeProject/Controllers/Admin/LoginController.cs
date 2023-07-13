using Microsoft.IdentityModel.Tokens;
using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers
{
    public class LoginController : BaseController
    {
        private UserService userService;
        public LoginController()
        {
            userService = new UserService();
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            // Validate user credentials (for example, by checking them against a database)
            bool isValid = CheckCredentials(email, password);

            if (isValid)
            {
                var user = userService.getUserByEmail(email);
                List<Claim> roleClaims = new List<Claim>();
                foreach (var role in user.Roles)
                {
                    roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                // Create JWT token with email payload
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("dsbsdvchsdvchgdvchsdcvhvsgvc");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, email),
                         //new Claim(ClaimTypes.Role, "Admin"),
                         // new Claim(ClaimTypes.Role, "Manager")
                    }.Union(roleClaims)),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Store JWT token in cookie
                HttpCookie cookie = new HttpCookie("jwt", tokenString);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
        }

        private bool CheckCredentials(string email, string password)
        {
            
            return userService.validInfor(email, password);

        }
        public ActionResult Logout()
        {
            // Remove JWT cookie
            HttpCookie cookie = new HttpCookie("jwt");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }

    }
}