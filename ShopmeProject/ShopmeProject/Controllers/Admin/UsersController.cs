using ShopmeProject.Models;
using ShopmeProject.Services;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers
{

    [JwtAuthentication("Admin")]
    public class UsersController : BaseController
    {
        private UserService userService;

        public UsersController()
        {
            userService = new UserService();
        }
        // GET: User
        public ActionResult Index()
        {
           // Debug.WriteLine(userService.validInfor("nam@codejava.net", "nhan2020"));
            return ListByPage(1,"");
        }

        public ActionResult ListByPage(int pageNum, string keyword)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<User> listUsers = userService.listByPage(pageNum,keyword);

            long startCount = (pageNum - 1)*userService.userPerPage() + 1;
            long endCount = startCount + userService.userPerPage() - 1;
            if (endCount > userService.totalRecordUser(keyword))
            {
                endCount = userService.totalRecordUser(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = userService.totalPageUser(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = userService.totalRecordUser(keyword);
            ViewBag.KeyWord = keyword;
            return View("Index",listUsers);
        }
        public ActionResult NewUser()
        {
            var user = new User();
            var roles = userService.ListRoles().ToList(); ;
            var userRolesViewModel = new UsersRolesViewModel()
            {
                user = user,
                Roles= roles
            };
            ViewBag.PageTitle = "Create New User";

            return View(userRolesViewModel);
        }

        [HttpPost]
        public ActionResult SaveUser(User user, List<Role> Roles, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                string uploadDir = Server.MapPath("/user-photos/" + user.Id);
                user.Photos = fileName;
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, image);

            }
                
            var rolesSelected = Roles.Where(r => r.isChecked == true).ToList();

            userService.save(user, rolesSelected);
            TempData["Message"] = "The user has been saved successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CheckDuplicateEmail(int id,string email)
        {
            if (userService.isEmailUnique(id,email) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User user = userService.Get(id);
            if (user == null)
            {
                TempData["Message"] = "Could not find any user with ID " + id;
                return RedirectToAction("Index");
            }
            
          
            ViewBag.PageTitle = "Edit User (ID: " + id + ")";
            var roles = userService.ListRoles().ToList(); 
            foreach(var role in roles)
            {
                if(user.Roles.FirstOrDefault(r => r.Id == role.Id) != null)
                {
                    roles.FirstOrDefault(r => r.Id == role.Id).isChecked = true;
                }
            }
            var userRolesViewModel = new UsersRolesViewModel()
            {
                user = user,
                Roles = roles
            };
            return View("NewUser", userRolesViewModel);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (userService.CountUsersById(id) > 0)
            {
                userService.delete(id);
                TempData["message"] = "The user ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any user with ID " + id;
            }
            return RedirectToAction("Index");
          }

        [HttpGet]
        public ActionResult SetEnabled(int id, bool enabled)
        {
           
            userService.updateUserEnabledStatus(id, enabled);
            string status = enabled ? "enabled" : "disabled";
            string message = $"The user ID {id} has been {status}";

            TempData["message"] = message;

            return RedirectToAction("Index", "Users");
        }



    }
}