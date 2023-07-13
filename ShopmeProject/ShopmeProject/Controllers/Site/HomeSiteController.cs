using ShopmeProject.Controllers.Admin;
using ShopmeProject.Models;
using ShopmeProject.ServiceSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
   
    public class HomeSiteController : BaseSiteController
    {
        private CategoryService categoryService;
        // GET: HomeSite
        public HomeSiteController()
        {
            categoryService = new CategoryService();
        }
        public ActionResult Index()
        {
            IEnumerable<Category> listCategories = categoryService.listNoChildrenCategories();
            return View(listCategories);
        }
    }
}