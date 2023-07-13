using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    [JwtAuthentication("Salesperson", "Admin")]
    public class ReportController : BaseController
    {
        private Services.SettingService settingService;

        public ReportController()
        {
           
            settingService = new Services.SettingService();
        }
        // GET: Report
        public ActionResult Index()
        {
            List<Setting> Settings = settingService.getCurrencySettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }
            return View("reports");
        }
    }
}