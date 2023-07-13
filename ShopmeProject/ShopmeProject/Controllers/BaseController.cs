using ShopmeProject.Models;
using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShopmeProject.Controllers
{
   
    public class BaseController : Controller
    {
        private SettingService settingService;

       
        public BaseController()
        {
            settingService = new Services.SettingService();

           
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            List<Setting> Settings = settingService.GetSettings();


            // Assign a value to the HttpContext
            ViewBag.SITE_LOGO = Settings.FirstOrDefault(x => x.Key == "SITE_LOGO").Value;
            ViewBag.SITE_NAME = Settings.FirstOrDefault(x => x.Key == "SITE_NAME").Value;
            ViewBag.COPYRIGHT = Settings.FirstOrDefault(x => x.Key == "COPYRIGHT").Value;
            // Set the values of the global variables
            
        }
    }
}