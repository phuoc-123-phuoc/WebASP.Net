using ShopmeProject.Controllers.Admin;
using ShopmeProject.Services;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers
{
    public class AccountController : BaseAdminController
    {
        private UserService userService;

        public AccountController()
        {
            userService = new UserService();
        }
        // GET: Account
        public ActionResult viewDetails()
        {
            var user = User as ClaimsPrincipal;
            string email = "";
            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
            }
            User userInDb = userService.getUserByEmail(email);
            if (user == null)
            {
                TempData["Message"] = "Could not find any user with email " + email;
                return RedirectToAction("Index","Users");
            }
            var roles = userService.ListRoles().ToList();
            foreach (var role in roles)
            {
                if (userInDb.Roles.FirstOrDefault(r => r.Id == role.Id) != null)
                {
                    roles.FirstOrDefault(r => r.Id == role.Id).isChecked = true;
                }
            }
            var userRolesViewModel = new UsersRolesViewModel()
            {
                user = userInDb,
                Roles = roles
            };
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View(userRolesViewModel);
        }
        public ActionResult saveDetails(User user,  HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                string uploadDir = Server.MapPath("/user-photos/" + user.Id);
                user.Photos = fileName;
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, image);

            }
            userService.saveDetails(user);
            TempData["Message"] = "The user has been saved successfully.";
            return RedirectToAction("viewDetails");
        }
    }
}