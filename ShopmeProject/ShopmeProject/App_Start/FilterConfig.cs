using ShopmeProject.Controllers;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           // filters.Add(new JwtAuthenticationAttribute());
        }
    }
}
