using ShopmeProject.Controllers.Admin;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers
{
   
    public class HomeController : BaseAdminController
    {
        private MyDbContext _context;

        public HomeController()
        {
            _context = new MyDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
       
        public ActionResult Index()
        {
            //var newRole = new Role
            //{
            //    Name = "Admin",
            //    Discription = "Manage everything"
            //};
            //_context.Roles.Add(newRole);
            //var savedRole = _context.SaveChanges();
           

            //var roles = _context.Roles.ToList();
            //var Name = roles.Last().Name;
            //_context.Dispose();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}