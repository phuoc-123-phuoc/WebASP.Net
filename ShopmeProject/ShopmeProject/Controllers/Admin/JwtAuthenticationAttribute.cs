using Microsoft.IdentityModel.Tokens;
using ShopmeProject.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopmeProject.Controllers
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;
     

        public JwtAuthenticationAttribute(params string[] allowedRoles)
        {
           
            _allowedRoles = allowedRoles;

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           

           
            // Check if the controller is the LoginController and the action is Login
            if (filterContext.Controller.GetType() == typeof(LoginController) && filterContext.ActionDescriptor.ActionName == "Index")
            {
                return;
            }


            var jwtCookie = filterContext.HttpContext.Request.Cookies["jwt"];
            if (jwtCookie != null)
            {
               

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("dsbsdvchsdvchgdvchsdcvhvsgvc");

                try
                {
                    tokenHandler.ValidateToken(jwtCookie.Value, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    }, out var validatedToken);

                }
                catch (SecurityTokenException)
                {
                    // Token validation failed
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" }
                    });
                }
                catch (ArgumentException)
                {
                    // Token validation failed
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" }
                    });
                }
                // decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                var decodedToken = handler.ReadJwtToken(jwtCookie.Value);
                // extract the claims from the decoded token
                var claims = decodedToken.Claims;

                // Check if the user has the required role to access the current controller/action
                if (_allowedRoles != null && _allowedRoles.Any())
                {
                    var userRoles = claims.Where(c => c.Type == "role").Select(c => c.Value.Trim());

       
                    if (!userRoles.Intersect(_allowedRoles, StringComparer.OrdinalIgnoreCase).Any())
                    {
                        // User does not have the required role, redirect to login page
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "controller", "Error" },
                            { "action", "AccessDenied" }
                        });
                        return;
                    }
                }
                //foreach (var claim in claims)
                //{
                //    if(claim.Type == "role")
                //    Debug.WriteLine(claim.Value);
                //}
                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                filterContext.HttpContext.User = principal;


               
               



            }
            else
            {
                // No JWT token in cookie
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
            }
        }
      
    }
}